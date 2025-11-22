using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;
using System;

namespace ReColhe.Application.Pev.Favoritar
{
    public class AdicionarFavoritoCommandHandler : IRequestHandler<AdicionarFavoritoCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPevRepository _pevRepository;
        private readonly IUsuarioPevFavoritoRepository _favoritoRepository;

        public AdicionarFavoritoCommandHandler(
            IUsuarioRepository usuarioRepository,
            IPevRepository pevRepository,
            IUsuarioPevFavoritoRepository favoritoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _pevRepository = pevRepository;
            _favoritoRepository = favoritoRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(AdicionarFavoritoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //  Validar se o Usuário existe
                var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);
                if (usuario == null)
                {
                    return CommandResponse<Unit>.AdicionarErro("Usuário não encontrado.", HttpStatusCode.NotFound);
                }

                //  Validar se o PEV existe
                var pev = await _pevRepository.GetByIdAsync(request.PevId);
                if (pev == null)
                {
                    return CommandResponse<Unit>.AdicionarErro("PEV não encontrado.", HttpStatusCode.NotFound);
                }

                // Validar se o favorito já não existe
                var favoritoExistente = await _favoritoRepository.BuscarAsync(request.UsuarioId, request.PevId);
                if (favoritoExistente != null)
                {
                    return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.OK);
                }

                // Criar a entidade de junção
                var novoFavorito = new UsuarioPevFavorito
                {
                    UsuarioId = request.UsuarioId,
                    PevId = request.PevId,
                    DataAdicao = DateTime.UtcNow
                };

                // Adicionar 
                await _favoritoRepository.CriarAsync(novoFavorito);
                await _favoritoRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao favoritar o PEV: {ex.Message}"
                );
            }
        }
    }
}