using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReColhe.Application.Auth.Login;
using ReColhe.Application.Mediator;
using System.Net;

namespace ReColhe.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }
        private IActionResult HandleLoginResponse<T>(CommandResponse<T> response)
        {
            if (response.Success)
            {
                return Ok(response.Data);
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
        /// Autentica um usuário e retorna um token JWT
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _sender.Send(command);
            return HandleLoginResponse(response);
        }
    }
}