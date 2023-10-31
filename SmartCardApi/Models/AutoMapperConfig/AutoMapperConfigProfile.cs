using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Authentication.LogIn;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using SmartCardApi.Models.DTOs.Card;
using SmartCardApi.Models.DTOs.Identity;
using SmartCardApi.Models.Identity;
using SmartCardApi.Models.User;

namespace SmartCardApi.Models.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile 
    {
        public AutoMapperConfigProfile() 
        {
            CreateMap<UserSignupDTO, AppIdentityUser>();

            CreateMap<AppIdentityUser, DomainUser>();

            CreateMap<AppIdentityUser, UserDTO>();

            CreateMap<CardCreateDTO, Card>();

            CreateMap<CardUpdateDTO, Card>();

            CreateMap<CardDeleteDTO, Card>();

            CreateMap<Card, CardGetDTO>();
        }
    }
}
