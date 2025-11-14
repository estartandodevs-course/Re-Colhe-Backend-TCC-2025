namespace ReColhe.Application.Common.Dtos
{
    /// <summary>
    /// DTO enxuto para listas aninhadas (ex: favoritos do usuário)
    /// </summary>
    public class PevSimplesResponse
    {
        public int PevId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
    }
}