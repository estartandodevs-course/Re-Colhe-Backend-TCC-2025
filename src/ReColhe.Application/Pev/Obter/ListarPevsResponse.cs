using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Pev.Obter
{
    // A resposta é uma lista de itens PEV
    public class ListarPevsResponse
    {
        public IEnumerable<PevResponseItem> Pevs { get; set; } = new List<PevResponseItem>();
    }

    // DTO para cada item da lista
    public class PevResponseItem
    {
        public int PevId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Materiais { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}