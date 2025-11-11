using MediatR;
using ReColhe.Domain.Repository;
using ReColhe.Application.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.Editar
{
    public class EditarReclamacaoCommandHandler : IRequestHandler<EditarReclamacaoCommand, CommandResponse<Unit>>
    {
        private readonly IReclamacaoRepository _repository;
        public EditarReclamacaoCommandHandler(IReclamacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Unit>> Handle(EditarReclamacaoCommand command, CancellationToken cancellationToken)
        {
            var reclamacao = await _repository.BuscarPorIdAsync(command.ReclamacaoId);
            if (reclamacao == null)
                return CommandResponse<Unit>.AdicionarErro("Reclamação não encontrada.", System.Net.HttpStatusCode.NotFound);

            reclamacao.Titulo = command.Titulo;
            reclamacao.Descricao = command.Descricao;
            reclamacao.Status = command.Status;
            reclamacao.CategoriaId = command.CategoriaId;

            await _repository.EditarAsync(reclamacao);

            await _repository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.NoContent);
        }
    }
}