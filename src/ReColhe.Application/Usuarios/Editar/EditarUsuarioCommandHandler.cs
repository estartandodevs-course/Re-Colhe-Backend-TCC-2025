using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Editar
{
    public class EditarUsuarioCommandHandler : IRequestHandler<EditarUsuarioCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public EditarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(EditarUsuarioCommand request, CancellationToken cancellationToken)
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

            if (request.Email != null)
            {
                var emailJaUtilizado = await _usuarioRepository.EmailJaUtilizado(request.Email);

                if (emailJaUtilizado)
                {
                    return CommandResponse<Unit>.AdicionarErro("O email informado já está em uso.", HttpStatusCode.Conflict);
                }
            }

            // Atualizar campos
            if (request.Nome != null)
                usuario.Nome = request.Nome;

            if (request.Email != null)
                usuario.Email = request.Email;

            if (request.Cep != null)
                usuario.Cep = request.Cep;

            await _usuarioRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.OK);
        }
    }
}