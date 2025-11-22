using ReColhe.Application.Mediator;
using MediatR;
using ReColhe.Application.Pev.Obter;

namespace ReColhe.Application.Pev.Obter.ObterPorId
{
    // A Query precisa do ID. Retorna um único PEV na resposta.
    public class ObterPevPorIdQuery : IRequest<CommandResponse<ObterPevPorIdResponse>>
    {
        public int PevId { get; private set; }

        public ObterPevPorIdQuery(int pevId)
        {
            PevId = pevId;
        }
    }
}