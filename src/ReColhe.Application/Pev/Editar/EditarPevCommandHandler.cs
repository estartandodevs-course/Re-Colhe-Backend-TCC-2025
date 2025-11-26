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

                pevExistente.Nome = request.Nome ?? pevExistente.Nome;
                pevExistente.Endereco = request.Endereco ?? pevExistente.Endereco;
                pevExistente.Telefone = request.Telefone ?? pevExistente.Telefone;

                pevExistente.OpenTime = request.OpenTime ?? pevExistente.OpenTime;
                pevExistente.CloseTime = request.CloseTime ?? pevExistente.CloseTime;
                pevExistente.OpeningDays = request.OpeningDays ?? pevExistente.OpeningDays;

                if (request.Materiais != null)
                {
                    // Converte List<string> para string separada por vírgula para o DB
                    var materiaisParaDb = string.Join(",", request.Materiais.Select(m => m.Trim().ToLower()));
                    pevExistente.Materiais = materiaisParaDb;
                }

                if (request.Posicao != null && request.Posicao.Count == 2)
                {
                    pevExistente.Latitude = request.Posicao[0];
                    pevExistente.Longitude = request.Posicao[1];
                }

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