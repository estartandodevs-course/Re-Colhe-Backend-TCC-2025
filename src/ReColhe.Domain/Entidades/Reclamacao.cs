namespace ReColhe.Domain.Entidades
{
    public class Reclamacao
    {
        public int ReclamacaoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public StatusReclamacao Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public int AutorId { get; set; }
        public int CategoriaId { get; set; }
    }
    public enum StatusReclamacao { Pendente, EmAnalise, Resolvido, Cancelado }
}
