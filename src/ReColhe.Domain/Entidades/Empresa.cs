namespace ReColhe.Domain.Entidades
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? EmailContato { get; set; }
        public string? TelefoneContato { get; set; }
    }
}