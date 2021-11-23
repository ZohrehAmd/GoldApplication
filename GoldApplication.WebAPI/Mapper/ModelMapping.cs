using AutoMapper;
using GoldApplication.WebAPI.Dtos;
using GoldApplication.WebAPI.Dtos.Product;
using GoldApplication.WebAPI.Dtos.User;
using GoldApplication.WebAPI.Dtos.UserEvent;
using GoldApplication.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Mapper
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductUser, ProductUserDto>().ReverseMap();
            CreateMap<ProductUser, CreateUserProductDto>().ReverseMap();
            CreateMap<ProductEvent, EventCreateDto>().ReverseMap();
            CreateMap<ProductEvent, EventUpdateDto>().ReverseMap();
            CreateMap<ProductEvent, EventDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, ShowUserDto>().ReverseMap();
            CreateMap<UserEvent, UserEventDto>().ReverseMap();
            CreateMap<UserEvent, CreateUserEventByUserDto>().ReverseMap();
            CreateMap<UserEvent, UpdateUserEventDto>().ReverseMap();
            CreateMap<UserEvent, UpdateUserEventByAdminDto>().ReverseMap();
        }
    }
}
