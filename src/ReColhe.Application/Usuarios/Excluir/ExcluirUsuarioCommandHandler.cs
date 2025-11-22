using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Excluir
{
    public class ExcluirUsuarioCommandHandler : IRequestHandler<ExcluirUsuarioCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ExcluirUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<Unit>.ErroValidacao(request.ResultadoDasValidacoes);
            }

            var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);

            if (usuario == null)
            {
                return CommandResponse<Unit>.AdicionarErro("Usuário não encontrado.", HttpStatusCode.NotFound);
            }

            // Excluir o usuário
            await _usuarioRepository.Excluir(usuario);
            await _usuarioRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
        }
    }
}