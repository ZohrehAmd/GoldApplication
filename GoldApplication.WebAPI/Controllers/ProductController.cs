using AutoMapper;
using GoldApplication.WebAPI.Dtos;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.Product;
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
    [ApiExplorerSettings(GroupName = "Gold_Api_Product")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _product;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository product,IMapper mapper)
        {
            _product = product;
            _mapper = mapper;
        }
        /// <summary>
        /// دریافت همه محصولات
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> GetAll()
        {
            var list = _product.GetAll();
            List<ProductDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<ProductDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }

        /// <summary>
        /// دریافت محصول با ای دی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}", Name = "GetById")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetById(long id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return await Task.Run(() => NotFound());

            return await Task.Run(() => (Ok(_mapper.Map<ProductDto>(product))));
        }
        /// <summary>
        /// افزودن محصول (Admin)
        /// </summary>
        /// <param name="createproduct"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductDto createproduct)
        {
            if (createproduct == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if (_product.IsExistProductWithTitleForCreate(createproduct.Title))
            {
                ModelState.AddModelError("Title", "نام محصول تکراری است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!_product.Create(_mapper.Map<Product>(createproduct)))
            {
                ModelState.AddModelError("", $"خطای سیستمی هنگام افزودن {createproduct.Title} !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            return await Task.Run(() => CreatedAtRoute("GetById", new {id=createproduct.ProductId },createproduct));
        }
        /// <summary>
        /// ویرایش محصول (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPatch("{id:long}", Name = "Update")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(long id, [FromBody] ProductDto update)
        {
            if (update == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if (id != update.ProductId)
            {
                ModelState.AddModelError("ProductId", "محصول را اشتباهی ارسال کرده اید");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (_product.IsExistProductWithTitleForUpdate(id, update.Title))
            {
                ModelState.AddModelError("Title", "محصول تکراری است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!_product.Update(_mapper.Map<Product>(update)))
            {
                ModelState.AddModelError("", $"خطای سیستمی هنگام ویرایش {update.Title} !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            return await Task.Run(() => CreatedAtRoute("GetById", new { id = update.ProductId }, update));
        }
        /// <summary>
        /// فعال و غیر فعال کردن محصول (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}", Name = "ActiveProduct")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveProduct(long id)
        {
            if (!_product.IsExistProductById(id))
                return await Task.Run(() => NotFound());

            return await Task.Run(() => Ok(_product.ActiveProduct(id)));
        }
    }
}
