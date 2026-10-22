using Veterinaria.Entity.DTO.Tokens;

namespace Veterinaria.Domain.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<TokenDto> Login(LoginDto login);
    }
}
