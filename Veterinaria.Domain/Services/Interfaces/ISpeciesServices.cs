using Veterinaria.Entity.DTO.Species;
using Veterinaria.Entity.Models.Verinaria;

namespace Veterinaria.Domain.Services.Interfaces
{
    public interface ISpeciesServices
    {
        Task<List<SpecieDto>> GetAll();
        Task<bool> Add(string name);
        Task<bool> Update(SpecieDto specie);
        Task<bool> Delete(int specieId);
    }
}
