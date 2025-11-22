using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;

namespace ReColhe.Application.Usuarios.Obter
{
    public class ListarUsuariosQueryHandler : IRequestHandler<ListarUsuariosQuery, CommandResponse<ListarUsuariosResponse>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ListarUsuariosQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<ListarUsuariosResponse>> Handle(ListarUsuariosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usuarios = await _usuarioRepository.ObterTodosComTipoUsuarioAsync();

                // SEGUNDO: Mapear para o DTO de resposta
                var usuariosResponse = usuarios.Select(u => new UsuarioResponse
                {
                    UsuarioId = u.UsuarioId,
                    Nome = u.Nome,
                    Email = u.Email,
                    TipoUsuario = new TipoUsuarioResponse
                    {
                        Id = u.TipoUsuario.TipoUsuarioId,
                        Tipo = u.TipoUsuario.Nome
                    }
                }).ToList();

                var response = new ListarUsuariosResponse
                {
                    Usuarios = usuariosResponse
                };

                return CommandResponse<ListarUsuariosResponse>.Sucesso(response);
            }
            catch (Exception ex)
            {
                return CommandResponse<ListarUsuariosResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao listar os usuários: {ex.Message}"
                );
            }
        }
    }
}