using AutoMapper;
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
            CreateMap<CardCreateDTO, Card>();

            CreateMap<Card, CardGetDTO>();

            CreateMap<CardUpdateDTO, Card>();

            CreateMap<CardDeleteDTO, Card>();

            CreateMap<UserSignupDTO, AppIdentityUser>();

            /*            CreateMap<UserLoginDTO, AppUser>();*/

            CreateMap<AppIdentityUser, UserDTO>();

            CreateMap<AppIdentityUser, DomainUser>();

            //CreateMap<CardGetDTO, Card>();
        }
    }
}
