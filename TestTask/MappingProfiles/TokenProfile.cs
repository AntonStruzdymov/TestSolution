using AutoMapper;
using TestDB.Entities;
using TestTask.DTOs;
using TestTask.Responses;

namespace TestTask.MappingProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<RefreshToken,TokenDTO>();
            CreateMap<TokenDTO, RefreshToken>();
            CreateMap<TokenDTO, TokenResponse>();
        }
    }
}
