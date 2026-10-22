using NETCore.Encrypt;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Common.Exceptions;
using Veterinaria.Common.Helpers;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO.Users;
using Veterinaria.Entity.Models.Secure;

namespace Veterinaria.Domain.Services
{
    public class UserServices : IUserServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public UserServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion

        // ctrl+k+s => crear regiones
        #region Methods 

        public async Task<List<UserDto>> GetAll()
        {
            List<UserEntity> entities = await _unitOfWork.UserRepository.GetAllAsync();
            List<UserDto> users = entities.Select(x => new UserDto()
            {
                IdUser = x.IdUser,
                Name = x.Name,
                Email = x.Email,
                LastName = x.LastName,
            }).ToList();

            return users;
        }


        public async Task<bool> Add(UserAddDto userAdd)
        {
            await Validations(userAdd);
            UserEntity entity = new UserEntity()
            {
                Name = userAdd.Name,
                LastName = userAdd.LastName,
                Email = userAdd.Email,
                Password = EncryptProvider.Sha256(userAdd.Password),
            };

            _unitOfWork.UserRepository.Insert(entity);
            return await _unitOfWork.SaveAsync() > 0;
        }



        private async Task Validations(UserAddDto userAdd)
        {
            string email = userAdd.Email;
            if (!Helper.IsEmail(email))
            {
                throw new BusinessException($"El email: {email} no es válido");
            }

            //validar si existe email
            await ExistEmailInDB(email);

            if (string.IsNullOrWhiteSpace(userAdd.Name))
                throw new BusinessException($"El nombre es requerido");

            if (string.IsNullOrWhiteSpace(userAdd.LastName))
                throw new BusinessException($"El apellido es requerido");

            if (string.IsNullOrWhiteSpace(userAdd.Password))
                throw new BusinessException($"La contraseña es requerida");

        }



        public async Task<UserEntity?> GetUserByEmail(string email) => await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        private async Task<UserEntity> GetEntity(int idUser)
        {
            UserEntity? entity = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.IdUser == idUser);
            if (entity == null)
            {
                string message = string.Format(GeneralMessages.ItemNoFound, "User");
                throw new BusinessException(message);
            }

            return entity;
        }

        private async Task ExistEmailInDB(string email)
        {
            UserEntity? entity = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email);
            if (entity != null)
            {
                //string message = string.Format(GeneralMessages.ItemNoFound, "User");
                throw new BusinessException($"Usuario con el email: {email} ya existe en el sistema.");
            }
        }



        #endregion
    }
}
