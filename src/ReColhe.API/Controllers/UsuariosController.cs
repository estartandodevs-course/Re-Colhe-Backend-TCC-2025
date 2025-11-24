using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Usuarios.Criar;
using ReColhe.Application.Usuarios.Editar;
using ReColhe.Application.Usuarios.Excluir;
using ReColhe.Application.Usuarios.Obter;
using System.Net;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ISender _sender;

        public UsuariosController(ISender sender)
        {
            _sender = sender;
        }

        private IActionResult HandleCommandResponse<T>(ReColhe.Application.Mediator.CommandResponse<T> response)
        {
            if (response.Success)
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Ok(response.Data),
                    HttpStatusCode.Created => CreatedAtAction(nameof(ObterUsuarioPorId), new { id = (response.Data as CriarUsuarioCommandResponse)?.UsuarioId }, response.Data),
                    HttpStatusCode.NoContent => NoContent(),
                    _ => Ok(response.Data)
                };
            }

            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => NotFound(new { erros = response.Erros }),
                HttpStatusCode.Conflict => Conflict(new { erros = response.Erros }),
                HttpStatusCode.BadRequest => BadRequest(new { erros = response.Erros }),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, new { erros = response.Erros })
            };
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var query = new ListarUsuariosQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Busca um usuário específico pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var query = new ObterUsuarioPorIdQuery(id);
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioCommand command)
        {
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Atualiza os dados de um usuário
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioCommand command)
        {
            var commandComId = new EditarUsuarioCommand(id, command.Nome, command.Email, command.Cep);
            var response = await _sender.Send(commandComId);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var command = new ExcluirUsuarioCommand(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }
    }
}