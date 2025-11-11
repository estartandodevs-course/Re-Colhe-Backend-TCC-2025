using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using System.Collections.Generic;

namespace ReColhe.Application.Notificacoes.Listar
{
    public class ListarNotificacoesQuery : IRequest<CommandResponse<IEnumerable<Notificacao>>>
    {
        public int UsuarioId { get; set; }
    }
}