using Veterinaria.Entity.DTO.Breeds;

namespace Veterinaria.Domain.Services.Interfaces
{
    public interface IBreedServices
    {
        Task<List<BreedDto>> GetAllBySpecieId(int specieId);
        Task<bool> Add(BreedAddDto breed);
        Task<bool> Update(BreedUpdateDto breed);
        Task<bool> Delete(int breedId);
    }
}
