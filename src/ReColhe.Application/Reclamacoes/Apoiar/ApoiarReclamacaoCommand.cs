using MediatR;
using ReColhe.Application.Mediator;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Reclamacoes.Apoiar
{
    public class ApoiarReclamacaoCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int UsuarioId { get; private set; }

        [JsonIgnore]
        public int ReclamacaoId { get; private set; }

        public void SetUsuarioId(int id) => UsuarioId = id;
        public void SetReclamacaoId(int id) => ReclamacaoId = id;
    }
}