using AutoMapper;
using GoldApplication.WebAPI.Dtos;
using GoldApplication.WebAPI.Dtos.User;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.User;
using GoldApplication.WebAPI.Utilities.Security;
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
    [ApiExplorerSettings(GroupName = "Gold_Api_User")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;
        private readonly IMapper _mapper;

        public UserController(IUserRepository user, IMapper mapper)
        {
            _user = user;
            _mapper = mapper;
        }
        /// <summary>
        /// دریافت لیست کاربران (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(200, Type = typeof(List<Dtos.User.ShowUserDto>))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _user.GetAll();
            List<Dtos.User.ShowUserDto> userDtos = new();
            foreach(var item in users)
            {
                userDtos.Add(_mapper.Map<Dtos.User.ShowUserDto>(item));
            }

            return await Task.Run(() => (Ok(userDtos)));
        }

        /// <summary>
        /// دریافت کاربر با آی دی (Admin)
        /// </summary>
        /// <param name="id">آی دی کاربر</param>
        /// <returns></returns>
        [HttpGet("{id:long}", Name = "GetUser")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(200, Type = typeof(Dtos.User.ShowUserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = _user.GetById(id);
            if (user.UserId < 1)
                return await Task.Run(() => NotFound());

            return await Task.Run(() => (base.Ok(_mapper.Map<Dtos.User.ShowUserDto>(user))));
        }
        /// <summary>
        /// ثبت کاربر جدید (Admin)
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(201, Type = typeof(Dtos.User.ShowUserDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if (_user.IsExistMobileForCreate(createUserDto.Mobile))
            {
                ModelState.AddModelError("Mobile", "موبایل وارد شده تکراری است .");
                return await Task.Run(() => StatusCode(404, ModelState));
            }
            createUserDto.Password = PasswordHelper.EncodeProSecurity(createUserDto.Password);
            if (!_user.Create(_mapper.Map<User>(createUserDto)))
            {
                ModelState.AddModelError("", $"هنگام افزودن این کاربر ({createUserDto.Mobile}) مشکلی پیش اومد . لطفا مجددا تلاش کنید.");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            var user = _user.GetByMobile(createUserDto.Mobile);
            return await Task.Run(() => base.CreatedAtRoute("GetUser", new { id = user.UserId }, _mapper.Map<Dtos.User.ShowUserDto>(user)));
        }
        /// <summary>
        /// ویرایش کاربر (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        [HttpPatch("{id:long}", Name = "UpdateUser")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(204, Type = typeof(Dtos.User.ShowUserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] Dtos.UserDto updateUserDto)
        {
            if (updateUserDto == null || updateUserDto.UserId != id || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if (_user.IsExistMobileForUpdate(updateUserDto.UserId, updateUserDto.Mobile.Trim()))
            {
                ModelState.AddModelError("Mobile", "موبایل وارد شده تکراری است .");
                return await Task.Run(() => StatusCode(404, ModelState));
            }
            var user = _mapper.Map<User>(updateUserDto);
            if (!_user.Update(user))
            {
                ModelState.AddModelError("", $"هنگام ویرایش این کاربر ({updateUserDto.Mobile}) مشکلی پیش اومد . لطفا مجددا تلاش کنید.");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            return await Task.Run(() => CreatedAtRoute("GetUser", new { id = updateUserDto.UserId }, _mapper.Map<Dtos.User.ShowUserDto>(user)));
        }
        /// <summary>
        /// فعال و غیر فعال کردن کاربر (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}", Name = "ActiveUser")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveUser(long id)
        {
            if (!_user.IsExistUserById(id))
                return await Task.Run(() => NotFound());

            return await Task.Run(() => Ok(_user.Active(id)));
        }
    }
}
