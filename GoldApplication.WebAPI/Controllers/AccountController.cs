using AutoMapper;
using GoldApplication.WebAPI.Dtos.User;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.Account;
using GoldApplication.WebAPI.Services.User;
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
    [ApiExplorerSettings(GroupName = "Gold_Api_Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _account;
        private readonly IUserRepository _user;
        private readonly IMapper _mapper;
        public AccountController(IAccountRepository account, IUserRepository user, IMapper mapper)
        {
            _account = account;
            _user = user;
            _mapper = mapper;
        }
        /// <summary>
        /// ورود
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            if(!ModelState.IsValid || model == null)
                return await Task.Run(() => BadRequest(ModelState));

            if(!_user.IsExistUserByMobile(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "کاربری یافت نشد.");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if(!_account.IsPasswordTrue(model))
            {
                ModelState.AddModelError("Password", "کلمه عبور اشتباه است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if(!_user.GetByMobile(model.Mobile).IsActive)
            {
                ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var user = _account.LoginUser(model.Mobile, model.Password);
            if (user == null)
                return await Task.Run(() => BadRequest(new {message = "خطای سیستمی !!!!! مجددا تلاش کنید ." }));

            user.Password = "";
            return await Task.Run(() => Ok(user));
        }
        /// <summary>
        /// ثبت نام
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto register)
        {
            if (register == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if(_user.IsExistMobileForCreate(register.Mobile))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل تکراری است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var user = _account.RegisterUser(register);
            if (user == null)
            {
                ModelState.AddModelError("Mobile", "خطای سیستمی !!! مجددا تلاش کنید .. .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            return await Task.Run(() => Ok(_mapper.Map<ShowUserDto>(user)));
        }
    }
}
