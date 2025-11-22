using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Notificacoes.Listar
{
    public class ListarNotificacoesQueryHandler : IRequestHandler<ListarNotificacoesQuery, CommandResponse<IEnumerable<Notificacao>>>
    {
        private readonly INotificacaoRepository _repository;
        public ListarNotificacoesQueryHandler(INotificacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<IEnumerable<Notificacao>>> Handle(ListarNotificacoesQuery query, CancellationToken cancellationToken)
        {
            var notificacoes = await _repository.ListarPorUsuarioAsync(query.UsuarioId);

            // CORREÇÃO 3: Queries (Leitura) devem retornar CommandResponse.Sucesso
            return CommandResponse<IEnumerable<Notificacao>>.Sucesso(notificacoes);
        }
    }
}