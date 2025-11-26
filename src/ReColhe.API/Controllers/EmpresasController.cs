using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Empresas.Criar;
using ReColhe.Application.Empresas.Editar;
using ReColhe.Application.Empresas.Excluir;
using ReColhe.Application.Empresas.Listar;
using ReColhe.Application.Empresas.ObterPorId;
using ReColhe.Application.Mediator;
using System.Net;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly ISender _sender;

        public EmpresasController(ISender sender)
        {
            _sender = sender;
        }

        private IActionResult HandleCommandResponse<T>(CommandResponse<T> response)
        {
            if (response.Success)
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Ok(response.Data),
                    HttpStatusCode.Created => CreatedAtAction(nameof(GetEmpresaById), new { id = (response.Data as CriarEmpresaCommandResponse)?.EmpresaId }, response.Data),
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
        /// Lista todas as empresas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEmpresas()
        {
            var query = new ListarEmpresasQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Busca uma empresa específica pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpresaById(int id)
        {
            var query = new ObterEmpresaPorIdQuery(id);
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Cadastra uma nova empresa
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEmpresa([FromBody] CriarEmpresaCommand command)
        {
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Atualiza os dados de uma empresa
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpresa(int id, [FromBody] EditarEmpresaCommand command)
        {
            command.SetEmpresaId(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Remove uma empresa do sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var command = new ExcluirEmpresaCommand(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }
    }
}