using AutoMapper;
using Domain.DTOs.Authentication.SignUp;
using Domain.DTOs.Card;
using Domain.DTOs.Identity;
using Domain.Identity;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.User;

namespace Domain.AutoMapperConfig
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
