using AutoMapper;
using GoldApplication.WebAPI.Dtos;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Services.Event;
using GoldApplication.WebAPI.Services.Product;
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
    [ApiExplorerSettings(GroupName = "Gold_Api_Event")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _event;
        private readonly IProductRepository _product;
        private readonly IMapper _mapper;
        public EventController(IProductRepository product, IEventRepository eventRepository, IMapper mapper)
        {
            _event = eventRepository;
            _mapper = mapper;
            _product = product;
        }
        /// <summary>
        /// دریافت همه ی معاملات (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> GetAllEvent()
        {
            var list = _event.GetAll();
            List<EventDto> events = new();
            foreach(var item in list)
            {
                events.Add(_mapper.Map<EventDto>(item));
            }
            return await Task.Run(() => Ok(events));
        }
        /// <summary>
        /// دریافت معاملات یک محصول (Admin)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{productId:long}",Name ="GetEventForProduct")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetEventForProduct(long productId)
        {
            if(!_product.IsExistProductById(productId))
                return await Task.Run(() => NotFound());

            var list = _event.GetAllEventForProduct(productId);

            if(list.Count() < 1)
                return await Task.Run(() => new EmptyResult());

            List<EventDto> events = new();
            foreach (var item in list)
            {
                events.Add(_mapper.Map<EventDto>(item));
            }
            return await Task.Run(() => Ok(events));
        }
        /// <summary>
        /// دریافت معامله با آی دی 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}",Name ="GetEventById")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetEventById(long id)
        {
            var productEvent = _event.GetById(id);
            if (productEvent == null)
                return await Task.Run(() => NotFound());

            return await Task.Run(() => (Ok(_mapper.Map<EventDto>(productEvent))));
        }
        /// <summary>
        /// افزودن معامله (Admin)
        /// </summary>
        /// <param name="createEvent"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] EventCreateDto createEvent)
        {
            if (createEvent == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));

            if (!_product.IsExistProductById(createEvent.ProductId))
            {
                ModelState.AddModelError("ProductId", "محصول اشتباه است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if(!TimeFormat.IsTime(createEvent.StartTime))
            {
                ModelState.AddModelError("StartTime", "فرمت ساعت شروع اشتباه است است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!TimeFormat.IsTime(createEvent.EndTime))
            {
                ModelState.AddModelError("EndTime", "فرمت ساعت پایان اشتباه است است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            var productEvent = _mapper.Map<ProductEvent>(createEvent);
            if (!_event.Create(productEvent))
            {
                ModelState.AddModelError("", $"خطای سیستمی هنگام افزودن {createEvent.Title} !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            return await Task.Run(() => CreatedAtRoute("GetById", new { id = productEvent.ProductId }, _mapper.Map<EventDto>(productEvent)));
        }
        /// <summary>
        /// ویرایش معامله (Admin)
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="eventUpdate"></param>
        /// <returns></returns>
        [HttpPatch("{EventId:long}",Name ="UpdateEvent")]
        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(204, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEvent(long EventId, [FromBody] EventUpdateDto eventUpdate)
        {
            if (eventUpdate == null || !ModelState.IsValid)
                return await Task.Run(() => BadRequest(ModelState));


            if (!_product.IsExistProductById(eventUpdate.ProductId))
            {
                ModelState.AddModelError("ProductId", "محصول موجود نیست است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (EventId != eventUpdate.ProductEventId)
            {
                ModelState.AddModelError("ProductId", "معامله را اشتباهی ارسال کرده اید");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!TimeFormat.IsTime(eventUpdate.StartTime))
            {
                ModelState.AddModelError("StartTime", "فرمت ساعت شروع اشتباه است است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!TimeFormat.IsTime(eventUpdate.EndTime))
            {
                ModelState.AddModelError("EndTime", "فرمت ساعت پایان اشتباه است است .");
                return await Task.Run(() => BadRequest(ModelState));
            }
            if (!_event.Update(_mapper.Map<ProductEvent>(eventUpdate)))
            {
                ModelState.AddModelError("", $"خطای سیستمی هنگام ویرایش {eventUpdate.Title} !!! لطفا مجددا تلاش کنید .");
                return await Task.Run(() => StatusCode(500, ModelState));
            }
            return await Task.Run(() => CreatedAtRoute("GetEventById", new { id = eventUpdate.ProductEventId }, eventUpdate));
        }
        /// <summary>
        /// فعال و غیر فعال کردن معامله (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}", Name = "ActiveEvent")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveEvent(long id)
        {
            if (!_event.IsExistById(id))
                return await Task.Run(() => NotFound());

            return await Task.Run(() => Ok(_event.ActiveEvent(id)));
        }
    }
}
