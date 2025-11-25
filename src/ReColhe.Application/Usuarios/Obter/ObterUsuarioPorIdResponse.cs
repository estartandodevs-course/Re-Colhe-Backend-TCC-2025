using ReColhe.Application.Common.Dtos;

namespace ReColhe.Application.Usuarios.Obter
{
    public class ObterUsuarioPorIdResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TipoUsuarioId { get; set; }
        public int? EmpresaId { get; set; }
        public string EmpresaNome { get; set; } = string.Empty;
        public string? Cep { get; set; }
        public List<PevSimplesResponse> PevsFavoritos { get; set; } = new List<PevSimplesResponse>();
    }

    public class UsuarioDetalhadoResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? EmpresaId { get; set; }
        public string? Cep { get; set; }
        public TipoUsuarioResponse TipoUsuario { get; set; } = new();
    }
}