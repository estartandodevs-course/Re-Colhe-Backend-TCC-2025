using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;
using System;
using System.Linq;

namespace ReColhe.Application.Pev.Editar
{
    public class EditarPevCommandHandler : IRequestHandler<EditarPevCommand, CommandResponse<Unit>>
    {
        private readonly IPevRepository _pevRepository;

        public EditarPevCommandHandler(IPevRepository pevRepository)
        {
            _pevRepository = pevRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(EditarPevCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<Unit>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

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

                var materiaisParaDb = string.Join(", ", request.Materiais);

                pevExistente.Nome = request.Nome;
                pevExistente.Endereco = request.Endereco;
                pevExistente.Telefone = request.Telefone;
                pevExistente.HorarioFuncionamento = request.HorarioFuncionamento;

                pevExistente.Materiais = materiaisParaDb;
                pevExistente.Latitude = request.Posicao[0];
                pevExistente.Longitude = request.Posicao[1];

                await _pevRepository.UpdateAsync(pevExistente);
                await _pevRepository.UnitOfWork.CommitAsync(cancellationToken);

                return CommandResponse<Unit>.Sucesso(Unit.Value, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CommandResponse<Unit>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao editar o PEV: {ex.Message}"
                );
            }
        }
    }
}