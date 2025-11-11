using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Mediator;
using System.Net;
using System.Threading.Tasks;
using ReColhe.Application.Notificacoes.Listar;
using ReColhe.Application.Notificacoes.MarcarComoLida;


namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/notificacoes")]
    public class NotificacoesController : ControllerBase
    {
        private readonly ISender _sender;

        public NotificacoesController(ISender sender)
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
                    HttpStatusCode.Created => CreatedAtAction(nameof(GetNotificacaoById), new { id = 1 }, response.Data),
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
        public async Task<IActionResult> GetNotificacoesUsuario()
        {
            
            var query = new ListarNotificacoesQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        
        [HttpPut("{id}/ler")]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var command = new MarcarNotificacaoComoLidaCommand();

            command.SetNotificacaoId(id);

            

            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetNotificacaoById(int id) => Ok();

       
    }
}