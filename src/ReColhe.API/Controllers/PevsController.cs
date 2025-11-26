using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Pev.Criar;
using ReColhe.Application.Pev.Editar;
using ReColhe.Application.Pev.Excluir;
using ReColhe.Application.Pev.Obter;
using ReColhe.Application.Pev.Obter.ObterPorId;
using ReColhe.Application.Pev.Favoritar;
using ReColhe.Application.Pev.Desfavoritar;
using System.Net;
using System.Threading.Tasks;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/pevs")]
    public class PevsController : ControllerBase
    {
        private readonly ISender _sender;

        public PevsController(ISender sender)
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
                    HttpStatusCode.Created => CreatedAtAction(nameof(GetPevById), new { id = (response.Data as CriarPevCommandResponse)!.PevId }, response.Data),
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
        /// Lista todos os PEVs.
        /// Rota: GET /api/v1/pevs
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPevs()
        {
            var query = new ListarPevsQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Busca um PEV específico pelo ID.
        /// Rota: GET /api/v1/pevs/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPevById(int id)
        {
            var query = new ObterPevPorIdQuery(id);
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Cadastra um novo Ponto de Entrega Voluntária.
        /// Rota: POST /api/v1/pevs
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePev([FromBody] CriarPevCommand command)
        {
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Atualiza os dados de um PEV (Atualização parcial via PATCH)
        /// Rota: PATCH /api/v1/pevs/{id}
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditarPev(int id, [FromBody] EditarPevCommand command)
        {
            var commandComId = command;
            commandComId.SetPevId(id);
            var response = await _sender.Send(commandComId);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Remove um PEV do sistema.
        /// Rota: DELETE /api/v1/pevs/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePev(int id)
        {
            var command = new ExcluirPevCommand(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Adiciona um PEV aos favoritos de um usuário.
        /// Rota: POST /api/v1/pevs/{pevId}/favoritar
        /// </summary>
        [HttpPost("{pevId}/favoritar")]
        public async Task<IActionResult> FavoritarPev([FromRoute] int pevId, [FromBody] AdicionarFavoritoCommand command)
        {
            command.SetPevId(pevId);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        /// <summary>
        /// Remove um PEV dos favoritos de um usuário.
        /// Rota: DELETE /api/v1/pevs/{pevId}/favoritar/{usuarioId}
        /// </summary>
        [HttpDelete("{pevId}/favoritar/{usuarioId}")]
        public async Task<IActionResult> DesfavoritarPev([FromRoute] int pevId, [FromRoute] int usuarioId)
        {
            var command = new RemoverFavoritoCommand(usuarioId, pevId);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }
    }
}