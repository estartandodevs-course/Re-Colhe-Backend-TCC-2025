using MediatR;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.Application.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.Criar
{
    public class CriarReclamacaoCommandHandler : IRequestHandler<CriarReclamacaoCommand, CommandResponse<CriarReclamacaoCommandResponse>>
    {
        private readonly IReclamacaoRepository _repository;
        public CriarReclamacaoCommandHandler(IReclamacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<CriarReclamacaoCommandResponse>> Handle(CriarReclamacaoCommand command, CancellationToken cancellationToken)
        {
            var reclamacao = new Reclamacao
            {
                Titulo = command.Titulo,
                Descricao = command.Descricao,
                Status = StatusReclamacao.Pendente,
                DataCriacao = DateTime.Now,
                AutorId = command.AutorId,
                CategoriaId = command.CategoriaId
            };

            await _repository.CriarAsync(reclamacao);

            await _repository.UnitOfWork.CommitAsync(cancellationToken);

            var response = new CriarReclamacaoCommandResponse { ReclamacaoId = reclamacao.ReclamacaoId };
            return CommandResponse<CriarReclamacaoCommandResponse>.Sucesso(response, System.Net.HttpStatusCode.Created);
        }
    }
}