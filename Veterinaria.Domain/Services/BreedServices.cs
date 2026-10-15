using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO.Breeds;
using Veterinaria.Entity.Models.Verinaria;

namespace Veterinaria.Domain.Services
{
    public class BreedServices : IBreedServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BreedServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #region Services


        public async Task<List<BreedDto>> GetAllBySpecieId(int specieId)
        {
            List<BreedEntity> entities = await _unitOfWork.BreedRepository.FindAllAsync(x => x.SpecieId == specieId,
                                                                                        s => s.SpeciesEntity);
            List<BreedDto> breeds = entities.Select(x => new BreedDto()
            {
                BreedId = x.Id,
                Name = x.Name,
                SpecieId = x.SpecieId,
                Specie = x.SpeciesEntity.Name

            }).ToList();

            return breeds;
        }

        private async Task<BreedEntity> GetEntityAsync(int breedId)
        {
            BreedEntity? entity = await _unitOfWork.BreedRepository.FirstOrDefaultAsync(x => x.Id == breedId);

            if (entity == null)
                throw new Exception("La raza a procesar no existe.");

            return entity;
        }

        public async Task<bool> Add(BreedAddDto breed)
        {
            BreedEntity entity = new BreedEntity()
            {
                Name = breed.Name,
                SpecieId = breed.SpecieId
            };
            _unitOfWork.BreedRepository.Insert(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> Update(BreedUpdateDto breed)
        {
            BreedEntity entity = await GetEntityAsync(breed.BreedId);
            entity.Name = breed.Name;
            entity.SpecieId = breed.SpecieId;
            _unitOfWork.BreedRepository.Update(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> Delete(int breedId)
        {
            BreedEntity entity = await GetEntityAsync(breedId);
            _unitOfWork.BreedRepository.Delete(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }
        #endregion
    }
}
