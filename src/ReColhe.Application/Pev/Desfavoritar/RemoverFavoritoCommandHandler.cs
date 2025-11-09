using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Net;

namespace ReColhe.Application.Pev.Desfavoritar
{
    public class RemoverFavoritoCommandHandler : IRequestHandler<RemoverFavoritoCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public RemoverFavoritoCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(RemoverFavoritoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Encontrar o favorito
                var favorito = await _usuarioRepository.ObterFavoritoAsync(request.UsuarioId, request.PevId);

                if (favorito == null)
                {
                    return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
                }

                //  Remover e Salvar
                await _usuarioRepository.RemoverFavorito(favorito);
                await _usuarioRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao desfavoritar o PEV: {ex.Message}"
                );
            }
        }
    }
}