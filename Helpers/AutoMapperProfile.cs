namespace TechnicalTest.Helpers;

using AutoMapper;
using TechnicalTest.Entities;
using TechnicalTest.Models.Users;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<User, AuthenticateResponse>();

        CreateMap<RegisterRequest, User>();

        CreateMap<UpdateRequest, User>().ForAllMembers(x => x.Condition((src, dest, prop) =>
        {
            if (prop == null) return false;
            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

            return true;
        }));
    }
}
