using AutoMapper;
using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
        }
    }
}
