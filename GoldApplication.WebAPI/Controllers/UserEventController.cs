using AutoMapper;
using GoldApplication.WebAPI.Dtos.UserEvent;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.Event;
using GoldApplication.WebAPI.Services.ProductUser;
using GoldApplication.WebAPI.Services.User;
using GoldApplication.WebAPI.Services.UserEvent;
using GoldApplication.WebAPI.Utilities.Date;
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
    [ApiExplorerSettings(GroupName = "Gold_Api_UserEvent")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UserEventController : ControllerBase
    {
        private readonly IUserEventRepository _userEvent;
        private readonly IEventRepository _event;
        private readonly IUserRepository _user;
        private readonly IProductUserRepository _productUser;
        private readonly IMapper _mapper;
        public UserEventController(IProductUserRepository productUser, IUserRepository user, IUserEventRepository userEvent, IMapper mapper, IEventRepository eventRepository)
        {
            _userEvent = userEvent;
            _mapper = mapper;
            _event = eventRepository;
            _user = user;
            _productUser = productUser;
        }
        /// <summary>
        /// دریافت خریداران معامله
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{eventId:long}", Name = "GetAllUsersSellForEvent")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersSellForEvent(long eventId)
        {
            if (!_event.IsExistById(eventId))
                return await Task.Run(() => NotFound());

            var list = _userEvent.GetAllForEvent(eventId);
            if (list.Count() < 1)
                return await Task.Run(() => Ok(new List<UserEventDto>()));

            List<UserEventDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<UserEventDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }
        /// <summary>
        /// دریافت معاملات یک خریدار توسط ادمین
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId:long}", Name = "GetAllEventsSellForUserByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEventsSellForUserByAdmin(long userId)
        {
            if (!_user.IsExistUserById(userId))
                return await Task.Run(() => NotFound());

            var list = _userEvent.GetAllForUser(userId);
            if (list.Count() < 1)
                return await Task.Run(() => Ok(new List<UserEventDto>()));

            List<UserEventDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<UserEventDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }
        /// <summary>
        ///  دریافت معاملات یک خریدار توسط کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]/", Name = "GetAllEventsSellForUserByUser")]
        [Authorize]
        public async Task<IActionResult> GetAllEventsSellForUserByUser()
        {
            var mobile = User.Identity.Name;
            var user = _user.GetByMobile(mobile);
            if (!_user.IsExistUserById(user.UserId))
                return await Task.Run(() => NotFound());

            var list = _userEvent.GetAllForUser(user.UserId);
            if (list.Count() < 1)
                return await Task.Run(() => Ok(new List<UserEventDto>()));

            List<UserEventDto> result = new();
            foreach (var item in list)
            {
                result.Add(_mapper.Map<UserEventDto>(item));
            }
            return await Task.Run(() => Ok(result));
        }
        /// <summary>
        /// خرید در معامله توسط کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateUserEventByUser")]
        [Authorize]
        public async Task<IActionResult> CreateUserEventByUser([FromBody] CreateUserEventByUserDto model)
        {
            if (model == null || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "لطفا اطلاعات را با دقت پر کنید .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var mobile = User.Identity.Name;
            var user = _user.GetByMobile(mobile);
            if (!_user.IsExistUserById(user.UserId))
                return await Task.Run(() => NotFound());

            var eventt = _event.GetById(model.ProductEventId);
            if (eventt == null || !eventt.IsActive)
                return await Task.Run(() => NotFound());

            var productUser = _productUser.GetProductUserForUser(user.UserId, eventt.ProductId);
            if (productUser == null)
            {
                ModelState.AddModelError("Geram", "کاربر گرامی شما به این محصول دسترسی ندارید .");
                return await Task.Run(() => BadRequest(ModelState));
            }

            if (_userEvent.IsExist(user.UserId, model.ProductEventId))
                return await Task.Run(() => BadRequest(new { message = "کاربر گرامی شما یک بار در این معامله شرکت کرده اید ." }));


            if (model.Geram > productUser.Geram)
            {
                ModelState.AddModelError("Geram", $"کاربر گرامی شما نمیتوانید بیش از  {productUser.Geram}   خریداری کنید . .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!_userEvent.HaveMojoodi(eventt, model.Geram))
            {
                ModelState.AddModelError("Geram", $"کاربر گرامی      {productUser.Geram}   موجودی از معامله نمانده است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var userEvent = _mapper.Map<UserEvent>(model);
            UserEvent userEvent1 = _userEvent.AddTimeToProductUser(user.UserId, userEvent);
            if (!_userEvent.Create(userEvent1))
            {
                ModelState.AddModelError("", "خطای سیستمی !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            return await Task.Run(() => Ok(_mapper.Map<UserEventDto>(userEvent1)));
        }
        /// <summary>
        /// ویرایش خرید توسط کاربر
        /// </summary>
        /// <param name="eventUserId"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPatch("[action]/{eventUserId:long}",Name ="UpdateUserEventByUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUserEventByUser(long eventUserId ,[FromBody] UpdateUserEventDto update)
        {
            if (update == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if(eventUserId != update.UserEventId)
                return await Task.Run(() => BadRequest(new { message ="اطلاعات را صحیح وارد کنید ." }));

            var user = _user.GetByMobile(User.Identity.Name);
            if(user.UserId != update.UserId)
                return await Task.Run(() => NotFound());

            var eventt = _event.GetById(update.ProductEventId);
            if (eventt == null || !eventt.IsActive)
                return await Task.Run(() => NotFound());

            var productUser = _productUser.GetProductUserForUser(user.UserId, eventt.ProductId);
            if (update.Geram > productUser.Geram)
            {
                ModelState.AddModelError("Geram", $"کاربر گرامی شما نمیتوانید بیش از  {productUser.Geram}   خریداری کنید . .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!_userEvent.HaveMojoodiForUpdate(eventt, update))
            {
                ModelState.AddModelError("Geram", $"کاربر گرامی      {productUser.Geram}   موجودی از معامله نمانده است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var userEvent = _mapper.Map<UserEvent>(update);
            UserEvent userEvent1 = _userEvent.AddTimeToProductUser(user.UserId, userEvent);
            if (!_userEvent.Update(userEvent1))
            {
                ModelState.AddModelError("", "خطای سیستمی !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            return await Task.Run(() => Ok(_mapper.Map<UserEventDto>(userEvent1)));
        }
       
        /// <summary>
        /// حذف خرید توسط ادمین
        /// </summary>
        /// <param name="eventUserId"></param>
        /// <returns></returns>
        [HttpDelete("{eventUserId:long}", Name = "DeleteEventUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEventUser(long eventUserId)
        {
            var userEvent = _userEvent.GetById(eventUserId);
            if (userEvent == null)
                return await Task.Run(() => NotFound());

            if (!_userEvent.Delete(userEvent))
            {
                ModelState.AddModelError("", "خطای سیستمی !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            return await Task.Run(() => NoContent());
        }
    }
}
