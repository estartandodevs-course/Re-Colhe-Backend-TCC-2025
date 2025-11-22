using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}