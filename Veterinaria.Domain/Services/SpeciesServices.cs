using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Common.Exceptions;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO.Species;
using Veterinaria.Entity.Models.Verinaria;

namespace Veterinaria.Domain.Services
{
    public class SpeciesServices : ISpeciesServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpeciesServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #region Services


        public async Task<List<SpecieDto>> GetAll()
        {
            List<SpeciesEntity> entities = await _unitOfWork.SpeciesRepository.GetAllAsync();
            List<SpecieDto> species = entities.Select(x => new SpecieDto()
            {
                Id= x.Id,
                Name= x.Name,
            }).ToList();

            return species;
        }

        public async Task<bool> Add(string name)
        {
            SpeciesEntity entity = new SpeciesEntity()
            {
                Name = name,
            };

            _unitOfWork.SpeciesRepository.Insert(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> Update(SpecieDto specie)
        {
            SpeciesEntity entity = await GetEntityAsync(specie.Id);
            entity.Name = specie.Name;
            _unitOfWork.SpeciesRepository.Update(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> Delete(int specieId)
        {
            SpeciesEntity entity = await GetEntityAsync(specieId);
            _unitOfWork.SpeciesRepository.Delete(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }

        #endregion

        #region Private
        private async Task<SpeciesEntity> GetEntityAsync(int specieId)
        {
            SpeciesEntity? entity = await _unitOfWork.SpeciesRepository.FirstOrDefaultAsync(x => x.Id == specieId);
            if (entity == null)
            {
                string message = string.Format(GeneralMessages.ItemNoFound, "Especie");
                throw new BusinessException(message);
            }

            return entity;
        } 
        #endregion
    }
}
