using API_CadastroUsuario.Dtos;
using API_CadastroUsuario.Models;
using AutoMapper;

namespace API_CadastroUsuario.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() 
        {

            CreateMap<Usuario,UsuarioDto>();
        
        }
    }
}
