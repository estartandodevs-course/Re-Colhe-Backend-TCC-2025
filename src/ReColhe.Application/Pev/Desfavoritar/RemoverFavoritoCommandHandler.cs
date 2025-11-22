using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;
using System;

namespace ReColhe.Application.Pev.Desfavoritar
{
    public class RemoverFavoritoCommandHandler : IRequestHandler<RemoverFavoritoCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioPevFavoritoRepository _favoritoRepository;

        public RemoverFavoritoCommandHandler(IUsuarioPevFavoritoRepository favoritoRepository)
        {
            _favoritoRepository = favoritoRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(RemoverFavoritoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //  Encontrar o favorito
                var favorito = await _favoritoRepository.BuscarAsync(request.UsuarioId, request.PevId);

                if (favorito == null)
                {
                    return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
                }

                //  Remover e Salvar
                await _favoritoRepository.RemoverAsync(favorito);
                await _favoritoRepository.UnitOfWork.CommitAsync(cancellationToken);

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