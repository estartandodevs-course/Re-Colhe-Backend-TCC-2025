using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Empresas.Criar
{
    public class CriarEmpresaCommandHandler : IRequestHandler<CriarEmpresaCommand, CommandResponse<CriarEmpresaCommandResponse>>
    {
        private readonly IEmpresaRepository _empresaRepository;

        public CriarEmpresaCommandHandler(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<CommandResponse<CriarEmpresaCommandResponse>> Handle(CriarEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<CriarEmpresaCommandResponse>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

            try
            {
                var cnpjJaUtilizado = await _empresaRepository.CnpjJaUtilizadoAsync(request.Cnpj);

                if (cnpjJaUtilizado)
                {
                    return CommandResponse<CriarEmpresaCommandResponse>.AdicionarErro(
                        statusCode: HttpStatusCode.Conflict,
                        mensagem: "O CNPJ informado já está em uso."
                    );
                }

                var empresa = new Empresa
                {
                    NomeFantasia = request.NomeFantasia,
                    Cnpj = request.Cnpj,
                    EmailContato = request.EmailContato,
                    TelefoneContato = request.TelefoneContato
                };

                await _empresaRepository.CriarAsync(empresa);
                await _empresaRepository.UnitOfWork.CommitAsync(cancellationToken);

                var response = new CriarEmpresaCommandResponse(empresa.EmpresaId);

                return CommandResponse<CriarEmpresaCommandResponse>.Sucesso(response, statusCode: HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return CommandResponse<CriarEmpresaCommandResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao criar a empresa: {ex.Message}"
                );
            }
        }
    }
}