using ReColhe.Application.Mediator;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Pev.Favoritar
{
    public class AdicionarFavoritoCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore] 
        public int PevId { get; private set; }

        public int UsuarioId { get; set; }

        public void SetPevId(int pevId)
        {
            PevId = pevId;
        }
    }
}