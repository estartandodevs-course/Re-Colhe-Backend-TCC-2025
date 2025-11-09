using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReColhe.Domain.Entidades
{
    /// <summary>
    /// Representa a entidade Pev (Ponto de Entrega Voluntária) no banco de dados.
    /// Mapeia a tabela 'PEV'.
    /// </summary>
    public class Pev
    {
        [Key] // Chave Primária
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int PevId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; }

        [MaxLength(500)]
        public string Endereco { get; set; } 

        [MaxLength(20)]
        public string Telefone { get; set; } 

        [MaxLength(255)]
        public string HorarioFuncionamento { get; set; }

        [MaxLength(255)]
        public string Materiais { get; set; } 

        // Vem do campo "position"
        [Column(TypeName = "decimal(10, 8)")]
        public decimal Latitude { get; set; } // position[0]

        [Column(TypeName = "decimal(11, 8)")]
        public decimal Longitude { get; set; } // position[1]

        // relacao com a tabela intermediaria com o ususario
        //public virtual ICollection<UsuarioPevFavorito> FavoritadoPor { get; set; }

        //public Pev()
        //{
        //    FavoritadoPor = new HashSet<UsuarioPevFavorito>();
        //}
    }
}