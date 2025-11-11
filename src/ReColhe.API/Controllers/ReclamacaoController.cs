using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Mediator;
using System.Net;
using System.Threading.Tasks;
using ReColhe.Application.Reclamacoes.Criar;
using ReColhe.Application.Reclamacoes.Editar;
using ReColhe.Application.Reclamacoes.Excluir;
using ReColhe.Application.Reclamacoes.Listar;
using ReColhe.Application.Reclamacoes.ObterPorId;
using ReColhe.Application.Reclamacoes.Apoiar;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/reclamacoes")]
    public class ReclamacoesController : ControllerBase
    {
        private readonly ISender _sender;

        public ReclamacoesController(ISender sender)
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
                    // Ajustado para o response de CriarReclamacao
                    HttpStatusCode.Created => CreatedAtAction(nameof(GetReclamacaoById), new { id = (response.Data as CriarReclamacaoCommandResponse).ReclamacaoId }, response.Data),
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

        
        [HttpGet]
        public async Task<IActionResult> GetAllReclamacoes()
        {
            var query = new ListarReclamacoesQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReclamacaoById(int id)
        {
            var query = new ObterReclamacaoPorIdQuery(id);
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateReclamacao([FromBody] CriarReclamacaoCommand command)
        {
             

            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReclamacao(int id, [FromBody] EditarReclamacaoCommand command)
        {
            command.ReclamacaoId = id;
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclamacao(int id)
        {
            
            var command = new ExcluirReclamacaoCommand(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
        [HttpPost("{id}/apoiar")]
        public async Task<IActionResult> ApoiarReclamacao(int id)
        {
            var command = new ApoiarReclamacaoCommand();

            
            command.SetReclamacaoId(id);

            

            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
    }
}