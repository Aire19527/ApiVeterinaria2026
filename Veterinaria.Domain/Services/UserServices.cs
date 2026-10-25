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
            await Validations(userAdd.Name, userAdd.LastName, userAdd.Email, userAdd.Password);
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

        public async Task<bool> Update(UserUpdateDto user)
        {
            UserEntity entity = await GetEntity(user.IdUser);
            await Validations(user.Name, user.LastName, user.Email, currentEmail: entity.Email);

            entity.Name = user.Name;
            entity.LastName = user.LastName;
            entity.Email = user.Email;

            _unitOfWork.UserRepository.Update(entity);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> Delete(int userId)
        {
            UserEntity entity = await GetEntity(userId);
            _unitOfWork.UserRepository.Delete(entity);

            return await _unitOfWork.SaveAsync() > 0;
        }
        #endregion


        #region Private
        // password: solo en alta. null en edición, porque no se cambia la contraseña.
        // currentEmail: email guardado. null en alta (siempre se valida unicidad).
        //               En edición, si coincide con el nuevo, no se consulta la BD.
        private async Task Validations(string name, string? lastName, string email, string? password = null, string? currentEmail = null)
        {
            if (!Helper.IsEmail(email))
                throw new BusinessException($"El email: {email} no es válido");

            // Alta: currentEmail es null y el correo no puede existir en BD.
            // Edición: solo se consulta si el correo cambió (sin distinguir mayúsculas).
            bool isNewUser = currentEmail == null;
            bool emailChanged = !isNewUser
                && !string.Equals(currentEmail!.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase);

            if (isNewUser || emailChanged)
                await ExistEmailInDB(email);

            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessException($"El nombre es requerido");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new BusinessException($"El apellido es requerido");

            // La contraseña solo se exige al crear el usuario.
            if (password != null && string.IsNullOrWhiteSpace(password))
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
