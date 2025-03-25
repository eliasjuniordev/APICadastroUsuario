using API_CadastroUsuario.Dtos;
using API_CadastroUsuario.Models;
using AutoMapper;
using Dapper;
using System.Data.SqlClient;


namespace API_CadastroUsuario.Services
{
    public class UsuarioService : IUsuario
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UsuarioService(IConfiguration configuration,IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;

        }
        public async Task<ResponseModel<List<UsuarioDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioDto>> response = new ResponseModel<List<UsuarioDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
               var usuariosBanco = await connection.QueryAsync<UsuarioDto>("SELECT * FROM Usuarios");
                
                if (usuariosBanco.Count() == 0)
                {
                    response.Status = false;
                    response.Mensagem = "Nenhum usuário encontrado";
                    return response;
                }
                
                var UsuarioMapeado = _mapper.Map<List<UsuarioDto>>(usuariosBanco);
                response.Dados = UsuarioMapeado;
                response.Mensagem = "Usuários encontrados com sucesso";
            }
            return response;
        }

        public async Task<ResponseModel<Usuario>> BuscarUsuariosId(int UsuarioId)
        {
            ResponseModel<Usuario> response = new ResponseModel<Usuario>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE Id = @Id", new { Id = UsuarioId });
               
                if (usuarioBanco == null)
                {
                    response.Status = false;
                    response.Mensagem = "Usuário não encontrado";
                    return response;
                }
                
                response.Dados = usuarioBanco;
                response.Mensagem = "Usuário encontrado com sucesso";
            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioDto>>> CriarUsuario(CriarUsuarioDto criarUsuarioDto)
        {
            ResponseModel<List<UsuarioDto>> response = new ResponseModel<List<UsuarioDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
         
                var UsuariosBanco = await connection.ExecuteAsync("INSERT INTO Usuarios (Nome, Email, Cargo, Salario,CPF,Senha,Situacao) " +
                    "VALUES (@Nome, @Email, @Cargo, @Salario,@CPF,@Senha,@Situacao)", 
                    new { Nome = criarUsuarioDto.Nome, Email = criarUsuarioDto.Email, Cargo = criarUsuarioDto.Cargo, Salario = criarUsuarioDto.Salario, CPF= criarUsuarioDto.CPF,Senha = criarUsuarioDto.Senha,Situacao = criarUsuarioDto.Situacao });

                if (UsuariosBanco == 0)
                {
                    response.Status = false;
                    response.Mensagem = "Erro ao criar usuário";
                    return response;
                }
                var usuarios= await ListarUsuarios(connection);
                var usuariosMapeados = _mapper.Map<List<UsuarioDto>>(usuarios);
                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuário criado com sucesso";



            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioDto>>> EditarUsuario(EditarUsuarioDto editarUsuarioDto)
        {
            ResponseModel<List<UsuarioDto>> response = new ResponseModel<List<UsuarioDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var UsuariosBanco = await connection.ExecuteAsync("UPDATE Usuarios SET Nome = @Nome, Email = @Email, Cargo = @Cargo, Salario = @Salario,CPF = @CPF,Situacao = @Situacao WHERE Id = @Id",
                    new { Nome = editarUsuarioDto.Nome, Email = editarUsuarioDto.Email, Cargo = editarUsuarioDto.Cargo, Salario = editarUsuarioDto.Salario, CPF = editarUsuarioDto.CPF, Situacao = editarUsuarioDto.Situacao, Id = editarUsuarioDto.Id });
                
                if (UsuariosBanco == 0)
                {
                    response.Status = false;
                    response.Mensagem = "Erro ao editar usuário";
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);
                var usuariosMapeados = _mapper.Map<List<UsuarioDto>>(usuarios);
                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuário editado com sucesso";
            }

            return response;

        }

        public Task<ResponseModel<UsuarioDto>> RemoverUsuario(int UsuarioId)
        {
            using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = UsuarioId });
                
                if (usuarioBanco == 0)
                {
                    return Task.FromResult(new ResponseModel<UsuarioDto> { Status = false, Mensagem = "Erro ao remover usuário" });
                }
                return Task.FromResult(new ResponseModel<UsuarioDto> { Status = true, Mensagem = "Usuário removido com sucesso" });
            }

        }

        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("select*from Usuarios");
        }

     
    }
}
