namespace ReColhe.Application.Auth.Login
{
    public class LoginCommandResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int TipoUsuarioId { get; set; }
        public string Token { get; set; }
        public LoginCommandResponse(int id, string nome, string email, int tipoUsuarioId, string token)
        {
            Id = id;
            Nome = nome;
            Email = email;
            TipoUsuarioId = tipoUsuarioId;
            Token = token;
        }
    }
}