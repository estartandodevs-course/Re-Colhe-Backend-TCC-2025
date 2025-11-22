namespace ReColhe.Domain.Entidades
{
    public class UsuarioPevFavorito
    {
        public int UsuarioId { get; set; }
        public int PevId { get; set; }

        public DateTime DataAdicao { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Pev Pev { get; set; }
    }
}