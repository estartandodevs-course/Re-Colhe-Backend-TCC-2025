using FluentValidation.Results;
using System.Net;

namespace ReColhe.Application.Mediator
{
    public class CommandResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }

        // MÉTODOS EXATOS DO PROJETO EXEMPLO
        public static CommandResponse<T> Sucesso(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new CommandResponse<T>
            {
                Success = true,
                Data = data,
                ErrorCode = ((int)statusCode).ToString()
            };
        }

        public static CommandResponse<T> AdicionarErro(string mensagem, HttpStatusCode statusCode)
        {
            return new CommandResponse<T>
            {
                Success = false,
                ErrorCode = ((int)statusCode).ToString(),
                ErrorMessage = mensagem
            };
        }

        public static CommandResponse<T> ErroValidacao(ValidationResult validationResult)
        {
            var errors = validationResult.Errors.Select(e => new ValidationError
            {
                PropertyName = e.PropertyName,
                ErrorCode = e.ErrorCode,
                ErrorMessage = e.ErrorMessage
            }).ToList();

            return new CommandResponse<T>
            {
                Success = false,
                ErrorCode = ((int)HttpStatusCode.BadRequest).ToString(),
                ValidationErrors = errors
            };
        }

        public static CommandResponse<T> ErroCritico(string mensagem)
        {
            return new CommandResponse<T>
            {
                Success = false,
                ErrorCode = ((int)HttpStatusCode.InternalServerError).ToString(),
                ErrorMessage = mensagem
            };
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}