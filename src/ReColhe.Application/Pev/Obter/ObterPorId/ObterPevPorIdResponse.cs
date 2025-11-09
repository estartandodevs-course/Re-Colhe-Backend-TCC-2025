namespace ReColhe.Application.Pev.Obter
{
    public class ObterPevPorIdResponse
    {
        public int PevId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;
        public string Materiais { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}