using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;
using System;
using System.Linq;

namespace ReColhe.Application.Pev.Criar
{
    public class CriarPevCommandHandler : IRequestHandler<CriarPevCommand, CommandResponse<CriarPevCommandResponse>>
    {
        private readonly IPevRepository _pevRepository;

        public CriarPevCommandHandler(IPevRepository pevRepository)
        {
            _pevRepository = pevRepository;
        }

        public async Task<CommandResponse<CriarPevCommandResponse>> Handle(CriarPevCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<CriarPevCommandResponse>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

            try
            {
                var materiaisParaDb = string.Join(", ", request.Materiais);

                var pev = new ReColhe.Domain.Entidades.Pev
                {
                    Nome = request.Nome,
                    Endereco = request.Endereco,
                    Telefone = request.Telefone,
                    HorarioFuncionamento = request.HorarioFuncionamento,

                    Materiais = materiaisParaDb,
                    Latitude = request.Posicao[0], // Pega o primeiro item (lat)
                    Longitude = request.Posicao[1] // Pega o segundo item (lng)
                };

                await _pevRepository.AddAsync(pev);
                await _pevRepository.UnitOfWork.CommitAsync(cancellationToken);

                var response = new CriarPevCommandResponse(pev.PevId);

                return CommandResponse<CriarPevCommandResponse>.Sucesso(response, statusCode: HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return CommandResponse<CriarPevCommandResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao criar o PEV: {ex.Message}"
                );
            }
        }
    }
}