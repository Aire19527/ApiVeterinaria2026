using Veterinaria.Entity.Models.Secure;

namespace Veterinaria.Domain.Services.Interfaces
{
    public interface IRolServices
    {
        Task<List<RolEntity>> GetAll();
    }
}
