using ReColhe.Application.Mediator;
using MediatR;

namespace ReColhe.Application.Pev.Excluir 
{
    public class ExcluirPevCommand : IRequest<CommandResponse<Unit>>
    {
        public int PevId { get; private set; }

        public ExcluirPevCommand(int pevId)
        {
            PevId = pevId;
        }
    }
}