using MediatR;
using ReColhe.Application.Common.Dtos;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Net;

namespace ReColhe.Application.Usuarios.Obter
{
    public class ObterUsuarioPorIdQueryHandler : IRequestHandler<ObterUsuarioPorIdQuery, CommandResponse<ObterUsuarioPorIdResponse>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ObterUsuarioPorIdQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<ObterUsuarioPorIdResponse>> Handle(ObterUsuarioPorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);

                if (usuario == null)
                {
                    return CommandResponse<ObterUsuarioPorIdResponse>.AdicionarErro(
                        "Usuário não encontrado.",
                        HttpStatusCode.NotFound
                    );
                }

                var response = new ObterUsuarioPorIdResponse
                {
                    UsuarioId = usuario.UsuarioId,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    TipoUsuarioId = usuario.TipoUsuarioId,
                    EmpresaId = usuario.EmpresaId,
                    EmpresaNome = usuario.Empresa?.NomeFantasia ?? string.Empty,
                    PevsFavoritos = usuario.PevFavoritos.Select(pf => new PevSimplesResponse
                    {
                        PevId = pf.Pev.PevId,
                        Nome = pf.Pev.Nome,
                        Endereco = pf.Pev.Endereco
                    }).ToList()
                };

                return CommandResponse<ObterUsuarioPorIdResponse>.Sucesso(response);
            }
            catch (Exception ex)
            {
                return CommandResponse<ObterUsuarioPorIdResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao buscar o usuário: {ex.Message}"
                );
            }
        }
    }
}