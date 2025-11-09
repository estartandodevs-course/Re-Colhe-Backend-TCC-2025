namespace ReColhe.Application.Usuarios.Obter
{
    public class ObterUsuarioPorIdResponse
    {
        public string Mensagem { get; set; } = "Usuário recuperado com sucesso.";
        public UsuarioDetalhadoResponse Usuario { get; set; } = new();
    }

    public class UsuarioDetalhadoResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? EmpresaId { get; set; }
        public TipoUsuarioResponse TipoUsuario { get; set; } = new();
    }
}