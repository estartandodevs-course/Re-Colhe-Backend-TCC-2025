namespace ReColhe.Application.Usuarios.Criar
{
    public class CriarUsuarioCommandResponse
    {
        public int UsuarioId { get; private set; }

        public CriarUsuarioCommandResponse(int usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}