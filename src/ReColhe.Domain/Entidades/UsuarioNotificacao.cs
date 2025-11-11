namespace ReColhe.Domain.Entidades
{
    public class UsuarioNotificacao
    {
        public int UsuarioNotificacaoId { get; set; }
        public int UsuarioId { get; set; }
        public int NotificacaoId { get; set; }
        public bool Lida { get; set; }
    }
}
