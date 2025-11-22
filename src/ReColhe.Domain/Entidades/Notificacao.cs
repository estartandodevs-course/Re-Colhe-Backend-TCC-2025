namespace ReColhe.Domain.Entidades
{
    public class Notificacao
    {
        public int NotificacaoId { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }
        public ICollection<UsuarioNotificacao> UsuarioNotificacoes { get; set; }

    }
}
