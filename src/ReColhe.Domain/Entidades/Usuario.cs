namespace ReColhe.Domain.Entidades
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int TipoUsuarioId { get; set; }
        public int? EmpresaId { get; set; }
        public string? Cep { get; set; }

        // Navigation properties
        public virtual TipoUsuario? TipoUsuario { get; set; }
        public virtual Empresa? Empresa { get; set; }
        public virtual ICollection<UsuarioPevFavorito> PevFavoritos { get; set; } = new List<UsuarioPevFavorito>();
    }
}