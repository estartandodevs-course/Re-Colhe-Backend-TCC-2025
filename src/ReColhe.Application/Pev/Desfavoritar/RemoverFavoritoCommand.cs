using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Pev.Desfavoritar
{
    public class RemoverFavoritoCommand : IRequest<CommandResponse<Unit>>
    {
        public int UsuarioId { get; private set; }
        public int PevId { get; private set; }

        public RemoverFavoritoCommand(int usuarioId, int pevId)
        {
            UsuarioId = usuarioId;
            PevId = pevId;
        }
    }
}