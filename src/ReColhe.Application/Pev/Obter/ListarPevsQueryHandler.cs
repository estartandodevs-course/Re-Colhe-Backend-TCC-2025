using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using MediatR;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ReColhe.Application.Pev.Obter
{
    public class ListarPevsQueryHandler : IRequestHandler<ListarPevsQuery, CommandResponse<ListarPevsResponse>>
    {
        private readonly IPevRepository _pevRepository;

        public ListarPevsQueryHandler(IPevRepository pevRepository)
        {
            _pevRepository = pevRepository;
        }

        public async Task<CommandResponse<ListarPevsResponse>> Handle(ListarPevsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pevs = await _pevRepository.GetAllAsync();

                var pevsResponse = pevs.Select(p => new PevResponseItem
                {
                    PevId = p.PevId,
                    Nome = p.Nome,
                    Endereco = p.Endereco,
                    Telefone = p.Telefone,
                    Materiais = p.Materiais.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(m => m.Trim())
                                           .ToList(),

                    Posicao = new List<decimal> { p.Latitude, p.Longitude }
                });

                var response = new ListarPevsResponse { Pevs = pevsResponse };

                return CommandResponse<ListarPevsResponse>.Sucesso(response);
            }
            catch (Exception ex)
            {
                return CommandResponse<ListarPevsResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao listar os PEVs: {ex.Message}"
                );
            }
        }
    }
}