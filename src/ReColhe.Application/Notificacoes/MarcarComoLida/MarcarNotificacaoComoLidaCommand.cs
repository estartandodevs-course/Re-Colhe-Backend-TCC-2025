using MediatR;
using ReColhe.Application.Mediator;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Notificacoes.MarcarComoLida
{
    public class MarcarNotificacaoComoLidaCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int NotificacaoId { get; private set; }

        [JsonIgnore]
        public int UsuarioId { get; private set; }

        public void SetNotificacaoId(int id) => NotificacaoId = id;

        public void SetUsuarioId(int id) => UsuarioId = id;
    }
}