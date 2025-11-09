using MediatR;
using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Repository;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Usuarios.Obter
{
    public class ObterUsuarioPorIdQueryHandler
        : IRequestHandler<ObterUsuarioPorIdQuery, CommandResponse<ObterUsuarioPorIdResponse>>
    {
        private readonly IApplicationDbContext _context;

        public ObterUsuarioPorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<ObterUsuarioPorIdResponse>> Handle(
            ObterUsuarioPorIdQuery request,
            CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Include(u => u.Empresa)
                .Where(u => u.UsuarioId == request.UsuarioId)
                .Select(u => new UsuarioDetalhadoResponse
                {
                    UsuarioId = u.UsuarioId,
                    Nome = u.Nome,
                    Email = u.Email,
                    EmpresaId = u.EmpresaId,
                    TipoUsuario = new TipoUsuarioResponse
                    {
                        Id = u.TipoUsuario.TipoUsuarioId,
                        Tipo = u.TipoUsuario.Nome
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (usuario == null)
            {
                return CommandResponse<ObterUsuarioPorIdResponse>.AdicionarErro(
                    "Usuário não encontrado.",
                    System.Net.HttpStatusCode.NotFound);
            }

            var response = new ObterUsuarioPorIdResponse
            {
                Usuario = usuario
            };

            return CommandResponse<ObterUsuarioPorIdResponse>.Sucesso(response);
        }
    }
}