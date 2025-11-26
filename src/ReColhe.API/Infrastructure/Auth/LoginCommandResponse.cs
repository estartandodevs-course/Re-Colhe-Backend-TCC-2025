namespace ReColhe.Application.Auth.Login
{
    public class LoginCommandResponse
    {
        public string Token { get; set; }

        public LoginCommandResponse(string token)
        {
            Token = token;
        }
    }
}