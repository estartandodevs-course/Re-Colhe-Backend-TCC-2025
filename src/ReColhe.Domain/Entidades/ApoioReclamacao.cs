using System.ComponentModel.DataAnnotations;

namespace ReColhe.Domain.Entidades
{
    public class ApoioReclamacao
    {
        public int ApoioReclamacaoId { get; set; }
        public int UsuarioId { get; set; }
        public int ReclamacaoId { get; set; }
        public DateTime DataApoio { get; set; }
    }
}
