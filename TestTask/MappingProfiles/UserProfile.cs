using AutoMapper;
using TestDB.Entities;
using TestTask.DTOs;
using TestTask.Responses;

namespace TestTask.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserResponse, UserDTO>();
            CreateMap<UserDTO, UserResponse>();

        }
    }
}
