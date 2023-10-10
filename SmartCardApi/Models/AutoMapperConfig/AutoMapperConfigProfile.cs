using AutoMapper;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Authentication.LogIn;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using SmartCardApi.Models.DTOs.Card;
using SmartCardApi.Models.Identity;

namespace SmartCardApi.Models.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile 
    {
        public AutoMapperConfigProfile() 
        {
            CreateMap<CardCreateDTO, Card>();

            CreateMap<Card, CardGetDTO>();

            CreateMap<CardUpdateDTO, Card>();

            CreateMap<CardDeleteDTO, Card>();

            CreateMap<UserSignupDTO, AppUser>();
        }
    }
}
