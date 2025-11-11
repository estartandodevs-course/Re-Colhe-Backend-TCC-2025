using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Categorias.Criar;
using ReColhe.Application.Categorias.Editar;
using ReColhe.Application.Categorias.Excluir;
using ReColhe.Application.Categorias.Listar;
using ReColhe.Application.Categorias.ObterPorId;
using ReColhe.Application.Mediator;
using System.Net;
using System.Threading.Tasks;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoriasController(ISender sender)
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
                    HttpStatusCode.Created => CreatedAtAction(nameof(GetCategoriaById), new { id = (response.Data as CriarCategoriaCommandResponse).CategoriaId }, response.Data),
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
        public async Task<IActionResult> GetAllCategorias()
        {
            var query = new ListarCategoriasQuery();
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var query = new ObterCategoriaPorIdQuery(id);
            var response = await _sender.Send(query);
            return HandleCommandResponse(response);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] CriarCategoriaCommand command)
        {
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] EditarCategoriaCommand command)
        {
            command.SetCategoriaId(id); 
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var command = new ExcluirCategoriaCommand(id);
            var response = await _sender.Send(command);
            return HandleCommandResponse(response);
        }
    }
}