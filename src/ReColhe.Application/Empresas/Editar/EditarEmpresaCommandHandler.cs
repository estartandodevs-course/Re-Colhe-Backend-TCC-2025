using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Empresas.Editar
{
    public class EditarEmpresaCommandHandler : IRequestHandler<EditarEmpresaCommand, CommandResponse<Unit>>
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EditarEmpresaCommandHandler(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(EditarEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<Unit>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

            try
            {
                var empresaExistente = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId);

                if (empresaExistente == null)
                {
                    return CommandResponse<Unit>.AdicionarErro("Empresa não encontrada.", HttpStatusCode.NotFound);
                }

                var cnpjJaUtilizado = await _empresaRepository.CnpjJaUtilizadoAsync(request.Cnpj, request.EmpresaId);

                if (cnpjJaUtilizado)
                {
                    return CommandResponse<Unit>.AdicionarErro("O CNPJ informado já está em uso.", HttpStatusCode.Conflict);
                }

                empresaExistente.NomeFantasia = request.NomeFantasia;
                empresaExistente.Cnpj = request.Cnpj;
                empresaExistente.EmailContato = request.EmailContato;
                empresaExistente.TelefoneContato = request.TelefoneContato;

                await _empresaRepository.AtualizarAsync(empresaExistente);
                await _empresaRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao editar a empresa: {ex.Message}"
                );
            }
        }
    }
}