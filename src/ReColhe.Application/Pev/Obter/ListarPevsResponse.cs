using ReColhe.Domain.Entidades;
using System.Collections.Generic;

namespace ReColhe.Application.Pev.Obter
{
    public class ListarPevsResponse
    {
        public IEnumerable<PevResponseItem> Pevs { get; set; } = new List<PevResponseItem>();
    }

    public class PevResponseItem
    {
        public int PevId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
        public string OpeningDays { get; set; } = string.Empty;

        public List<string> Materiais { get; set; } = new List<string>();
        public List<decimal> Posicao { get; set; } = new List<decimal>();
    }
}