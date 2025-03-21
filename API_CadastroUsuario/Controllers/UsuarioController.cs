using API_CadastroUsuario.Dtos;
using API_CadastroUsuario.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_CadastroUsuario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;
        public UsuarioController(IUsuario usuario)
        {
            
            _usuario = usuario;

        }


        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios()
        {
            var usuarios = await _usuario.BuscarUsuarios();
            
            if (usuarios.Status == false)
            {
                return NotFound(usuarios);
            }
            
            return Ok(usuarios);

        }

        [HttpGet("{UsuarioId}")]
        public async Task<IActionResult> BuscarUsuariosId(int UsuarioId)
        {
            var usuario = await _usuario.BuscarUsuariosId(UsuarioId);

            if (usuario.Status == false)
            {
                return NotFound(usuario);
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(CriarUsuarioDto criarUsuarioDto)
        {
            var usuario = await _usuario.CriarUsuario(criarUsuarioDto);
            
            if (usuario.Status == false)
            {
                return BadRequest(usuario);
            }
            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(EditarUsuarioDto editarUsuarioDto)
        {
            var usuario = await _usuario.EditarUsuario(editarUsuarioDto);

            if (usuario.Status == false)
            {
                return BadRequest(usuario);
            }
            return Ok(usuario);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverUsuario(int UsuarioId)
        {
            var usuario = await _usuario.RemoverUsuario(UsuarioId);

            if (usuario.Status == false)
            {
                return BadRequest(usuario);
            }
            return Ok(usuario);
        }
    }
}
