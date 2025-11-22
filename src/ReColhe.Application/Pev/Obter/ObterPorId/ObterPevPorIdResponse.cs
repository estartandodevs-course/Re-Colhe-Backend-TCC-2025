using System.Collections.Generic;

namespace ReColhe.Application.Pev.Obter
{
    public class ObterPevPorIdResponse
    {
        public int PevId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;

        public List<string> Materiais { get; set; } = new List<string>();
        public List<decimal> Posicao { get; set; } = new List<decimal>();
    }
}