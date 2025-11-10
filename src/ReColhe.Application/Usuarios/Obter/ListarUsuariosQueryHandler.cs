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

                var usuariosResponse = new List<UsuarioResponse>();

                var response = new ListarUsuariosResponse { Usuarios = usuariosResponse };

                return await Task.FromResult(CommandResponse<ListarUsuariosResponse>.Sucesso(response));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(CommandResponse<ListarUsuariosResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao listar os usuários: {ex.Message}"
                ));
            }
        }
    }
}