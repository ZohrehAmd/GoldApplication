using AutoMapper;
using GoldApplication.WebAPI.Dtos.Product;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.ProductUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Gold_Api_ProductUser")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductUserController : ControllerBase
    {
        private readonly IProductUserRepository _productUser;
        private readonly IMapper _mapper;
        public ProductUserController(IProductUserRepository productUser, IMapper mapper)
        {
            _productUser = productUser;
            _mapper = mapper;
        }
        /// <summary>
        /// دریافت خریداران یک محصول (Admin)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{productId:long}",Name = "GetAllUserForProduct")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(List<ProductUserDto>))]
        public async Task<IActionResult> GetAllUserForProduct(long productId)
        {
            var list = _productUser.GetAllUserForProduct(productId);
            List<ProductUserDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<ProductUserDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }
        /// <summary>
        /// دریافت محصولات یک خریدار
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId:long}", Name = "GetAllProductForUser")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(List<ProductUserDto>))]
        public async Task<IActionResult> GetAllProductForUser(long userId)
        {
            var list = _productUser.GetAllProductForUser(userId);
            List<ProductUserDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<ProductUserDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }
        /// <summary>
        /// دریافت حد  خرید کاربر از محصول 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId:long},{productId:long}", Name = "GetProductUserForUser")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(List<ProductUserDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductUserForUser(long userId, long productId)
        {
           if(userId < 1  || productId < 1)
                return await Task.Run(() => NotFound());

            var result = _productUser.GetProductUserForUser(userId, productId);
            if(result == null)
                return await Task.Run(() => NotFound());

            return await Task.Run(() => Ok(_mapper.Map<ProductUserDto>(result)));
        }
        /// <summary>
        /// افزودن کاربر به محصول (Admin)
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(List<ProductUserDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateUserProduct([FromBody] CreateUserProductDto create)
        {
            if (create == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if(_productUser.IsExist(create.UserId , create.ProductId))
            {
                ModelState.AddModelError("UserId", $"قبلا برای این کاربر این محصول را وارد کرده اید .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var result = _mapper.Map<ProductUser>(create);
            _productUser.Create(result);
            return await Task.Run(() => CreatedAtRoute("GetProductUserForUser", 
                new { userId = create.UserId , productId = create.ProductId },
                _mapper.Map<ProductUserDto>(_productUser.GetProductUserForUser(create.UserId,create.ProductId))));
        }
        /// <summary>
        /// ویرایش مقدار خرید کاربر از محصول (Admin)
        /// </summary>
        /// <param name="productUserId"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPatch("{productUserId:long}",Name = "UpdateUserProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserProduct(long productUserId,[FromBody] ProductUserDto update)
        {
            if (update == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if(update.ProductUserId != productUserId)
            {
                ModelState.AddModelError("", "اطلاعات را صحیح بفرستید .");
                     return await Task.Run(() => BadRequest(ModelState));
            }
            var userProduct = _mapper.Map<ProductUser>(update);
            if (!_productUser.Update(userProduct))
            {
                ModelState.AddModelError("UserId", "خطای سیستمی !!! مجددا تلاش کنید  .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            return await Task.Run(()=>CreatedAtRoute("GetProductUserForUser", 
                new { userId = update.UserId, productId = update.ProductId },
                _mapper.Map<ProductUserDto>(_productUser.GetProductUserForUser(update.UserId, update.ProductId))));
        }
        /// <summary>
        /// حذف کاربر از محصول (Admin)
        /// </summary> 
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}", Name = "DeleteProductUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductUser(long id)
        {
            if (!_productUser.IsExistProductUserById(id))
                return await Task.Run(() => NotFound());
            ProductUser productUser = _productUser.GetById(id);
            return await Task.Run(() => Ok(_productUser.Delete(productUser)));
        }
    }
}
