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

        // Navigation properties (podem ser virtuais ou remover se der problema)
        public virtual TipoUsuario? TipoUsuario { get; set; }
        public virtual Empresa? Empresa { get; set; }
        // Relação de Usuario para a tabela de junção com o PEVs
        public virtual ICollection<UsuarioPevFavorito> PevFavoritos { get; set; } = new List<UsuarioPevFavorito>();
    }
}