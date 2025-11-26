using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Empresas.Excluir
{
    public class ExcluirEmpresaCommandHandler : IRequestHandler<ExcluirEmpresaCommand, CommandResponse<Unit>>
    {
        private readonly IEmpresaRepository _empresaRepository;

        public ExcluirEmpresaCommandHandler(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(ExcluirEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<Unit>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

            try
            {
                var empresa = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId);

                if (empresa == null)
                {
                    return CommandResponse<Unit>.AdicionarErro("Empresa não encontrada.", HttpStatusCode.NotFound);
                }

                await _empresaRepository.RemoverAsync(empresa);
                await _empresaRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao excluir a empresa: {ex.Message}"
                );
            }
        }
    }
}