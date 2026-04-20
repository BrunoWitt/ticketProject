namespace Src.Configurations;

using AutoMapper;
using Src.Modules.User.Models;
using Src.Modules.User.DTOs;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUserDTO, UsuarioModel>();
        CreateMap<UsuarioModel, CreateUserDTO>();

        CreateMap<UpdateUserDTO, UsuarioModel>();

        CreateMap<UsuarioModel, LoginDTO>();
    }
}