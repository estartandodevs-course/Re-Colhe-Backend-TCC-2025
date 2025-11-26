using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReColhe.Domain.Entidades
{
    public class Pev
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PevId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Endereco { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string OpenTime { get; set; } = string.Empty;

        [MaxLength(10)]
        public string CloseTime { get; set; } = string.Empty; 

        [MaxLength(50)]
        public string OpeningDays { get; set; } = string.Empty; 

        [MaxLength(255)]
        public string Materiais { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10, 8)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(11, 8)")]
        public decimal Longitude { get; set; }

        public virtual ICollection<UsuarioPevFavorito> UsuariosFavoritos { get; set; } = new List<UsuarioPevFavorito>();
    }
}