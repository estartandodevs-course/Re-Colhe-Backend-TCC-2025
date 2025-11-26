using ReColhe.Application.Common.Interfaces;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse<LoginCommandResponse>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<CommandResponse<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Buscar usuário pelo email
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario == null)
            {
                return CommandResponse<LoginCommandResponse>.AdicionarErro(
                    statusCode: HttpStatusCode.BadRequest,
                    mensagem: "Email ou senha inválidos."
                );
            }

            // Verificar a senha
            bool senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);

            if (!senhaValida)
            {
                return CommandResponse<LoginCommandResponse>.AdicionarErro(
                    statusCode: HttpStatusCode.BadRequest,
                    mensagem: "Email ou senha inválidos."
                );
            }

            // Gerar o Token (Os dados do usuário são embutidos aqui dentro)
            var token = _tokenService.GenerateToken(usuario);

            var response = new LoginCommandResponse(token);

            // Retornar Sucesso
            return CommandResponse<LoginCommandResponse>.Sucesso(response, statusCode: HttpStatusCode.OK);
        }
    }
}