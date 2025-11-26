namespace ReColhe.Application.Usuarios.Obter
{
    public class ListarUsuariosResponse
    {
        public string Mensagem { get; set; } = "Usuários recuperados com sucesso.";
        public List<UsuarioResponse> Usuarios { get; set; } = new();
    }

    public class UsuarioResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Cep { get; set; }
        public TipoUsuarioResponse TipoUsuario { get; set; } = new();
    }

    public class TipoUsuarioResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}