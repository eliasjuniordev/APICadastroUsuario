using API_CadastroUsuario.Dtos;
using API_CadastroUsuario.Models;

namespace API_CadastroUsuario.Services
{
    public interface IUsuario 
    {
       Task<ResponseModel<List<UsuarioDto>>>BuscarUsuarios();
       Task<ResponseModel<Usuario>> BuscarUsuariosId(int UsuarioId);
       Task<ResponseModel<List<UsuarioDto>>> CriarUsuario(CriarUsuarioDto criarUsuarioDto);
       Task<ResponseModel<List<UsuarioDto>>> EditarUsuario(EditarUsuarioDto editarUsuarioDto);
       Task<ResponseModel<UsuarioDto>> RemoverUsuario(int UsuarioId);
    }
}
