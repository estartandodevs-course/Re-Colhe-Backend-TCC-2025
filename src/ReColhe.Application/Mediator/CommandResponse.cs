using FluentValidation.Results;
using System.Net;

namespace ReColhe.Application.Mediator
{
    public class CommandResponse<T>
    {

        public bool Success { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Os dados de retorno em caso de sucesso (pode ser nulo).
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// Lista de mensagens de erro em caso de falha.
        /// </summary>
        public List<string> Erros { get; private set; } = new List<string>();


        private CommandResponse() { }

        /// <summary>
        /// Cria uma resposta de Sucesso.
        /// </summary>
        public static CommandResponse<T> Sucesso(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new CommandResponse<T>
            {
                Success = true,     
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Cria uma resposta para erro de validação (HTTP 400).
        /// </summary>
        public static CommandResponse<T> ErroValidacao(ValidationResult validationResult)
        {
            var response = new CommandResponse<T>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            response.Erros.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
            return response;
        }

        /// <summary>
        /// Cria uma resposta para um erro único (ex: HTTP 404 Not Found).
        /// </summary>
        public static CommandResponse<T> AdicionarErro(string mensagem, HttpStatusCode statusCode)
        {
            return new CommandResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Erros = new List<string> { mensagem }
            };
        }

        /// <summary>
        /// Cria uma resposta para um erro crítico/inesperado (HTTP 500).
        /// </summary>
        public static CommandResponse<T> ErroCritico(string mensagem)
        {
            return new CommandResponse<T>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Erros = new List<string> { mensagem }
            };
        }
    }
}