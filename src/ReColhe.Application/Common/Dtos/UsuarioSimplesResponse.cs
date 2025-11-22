namespace ReColhe.Application.Common.Dtos
{
    /// <summary>
    /// DTO enxuto para listas aninhadas (ex: quem favoritou um PEV)
    /// </summary>
    public class UsuarioSimplesResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}