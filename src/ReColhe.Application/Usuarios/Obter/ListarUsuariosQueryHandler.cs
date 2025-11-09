using MediatR;
using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Repository;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Usuarios.Obter
{
    public class ListarUsuariosQueryHandler
        : IRequestHandler<ListarUsuariosQuery, CommandResponse<ListarUsuariosResponse>>
    {
        private readonly IApplicationDbContext _context;

        public ListarUsuariosQueryHandler(IApplicationDbContext context)  //
        {
            _context = context;
        }

        public async Task<CommandResponse<ListarUsuariosResponse>> Handle(
            ListarUsuariosQuery request,
            CancellationToken cancellationToken)
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Select(u => new UsuarioResponse
                {
                    UsuarioId = u.UsuarioId,
                    Nome = u.Nome,
                    Email = u.Email,
                    TipoUsuario = new TipoUsuarioResponse
                    {
                        Id = u.TipoUsuario.TipoUsuarioId,
                        Tipo = u.TipoUsuario.Nome
                    }
                })
                .ToListAsync(cancellationToken);

            var response = new ListarUsuariosResponse
            {
                Usuarios = usuarios
            };

            return CommandResponse<ListarUsuariosResponse>.Sucesso(response);
        }
    }
}