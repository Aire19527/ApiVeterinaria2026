using Veterinaria.Entity.DTO.Users;
using Veterinaria.Entity.Models.Secure;

namespace Veterinaria.Domain.Services.Interfaces
{
    public interface IUserServices
    {
        Task<UserEntity?> GetUserByEmail(string email);
        Task<List<UserDto>> GetAll();
        Task<bool> Add(UserAddDto userAdd);
        Task<bool> Update(UserUpdateDto user);
        Task<bool> Delete(int userId);
    }
}
