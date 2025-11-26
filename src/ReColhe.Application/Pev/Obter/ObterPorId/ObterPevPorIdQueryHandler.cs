using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Application.Pev.Obter.ObterPorId;
using ReColhe.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ReColhe.Application.Pev.Obter
{
    public class ObterPevPorIdQueryHandler : IRequestHandler<ObterPevPorIdQuery, CommandResponse<ObterPevPorIdResponse>>
    {
        private readonly IPevRepository _pevRepository;

        public ObterPevPorIdQueryHandler(IPevRepository pevRepository)
        {
            _pevRepository = pevRepository;
        }

        public async Task<CommandResponse<ObterPevPorIdResponse>> Handle(ObterPevPorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pev = await _pevRepository.GetByIdAsync(request.PevId);

                if (pev == null)
                {
                    return CommandResponse<ObterPevPorIdResponse>.AdicionarErro(
                        "PEV não encontrado.",
                        HttpStatusCode.NotFound
                    );
                }

                var response = new ObterPevPorIdResponse
                {
                    PevId = pev.PevId,
                    Nome = pev.Nome,
                    Endereco = pev.Endereco,
                    Telefone = pev.Telefone,
                    OpenTime = pev.OpenTime,
                    CloseTime = pev.CloseTime,
                    OpeningDays = pev.OpeningDays,

                    Materiais = pev.Materiais.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(m => m.Trim())
                                           .ToList(),

                    Posicao = new List<decimal> { pev.Latitude, pev.Longitude }
                };

                return CommandResponse<ObterPevPorIdResponse>.Sucesso(response);
            }
            catch (Exception ex)
            {
                return CommandResponse<ObterPevPorIdResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao buscar o PEV: {ex.Message}"
                );
            }
        }
    }
}