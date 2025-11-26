using Microsoft.IdentityModel.Tokens;
using ReColhe.Application.Common.Interfaces;
using ReColhe.Domain.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReColhe.API.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration["Jwt:Key"];
            var expireHours = _configuration.GetValue<int>("Jwt:ExpireHours", 24);

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Chave JWT não encontrada na configuração.");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim("id", usuario.UsuarioId.ToString()),
                new Claim("nome", usuario.Nome),
                new Claim("email", usuario.Email),
                new Claim("tipo_usuario", usuario.TipoUsuarioId.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}