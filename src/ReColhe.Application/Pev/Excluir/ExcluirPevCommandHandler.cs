using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Pev.Excluir 
{
    public class ExcluirPevCommandHandler : IRequestHandler<ExcluirPevCommand, CommandResponse<Unit>>
    {
        private readonly IPevRepository _pevRepository;

        public ExcluirPevCommandHandler(IPevRepository pevRepository)
        {
            _pevRepository = pevRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(ExcluirPevCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pevExistente = await _pevRepository.GetByIdAsync(request.PevId);

                if (pevExistente == null)
                {
                    return CommandResponse<Unit>.AdicionarErro(
                        "PEV não encontrado.",
                        HttpStatusCode.NotFound
                    );
                }

                await _pevRepository.DeleteAsync(pevExistente);
                await _pevRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao excluir o PEV: {ex.Message}"
                );
            }
        }
    }
}