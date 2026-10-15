using Microsoft.EntityFrameworkCore.Storage;
using Veterina.Core.Repository.Interface;
using Veterinaria.Entity.Models.Secure;
using Veterinaria.Entity.Models.Verinaria;

namespace Veterina.Core.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IRepository<UserEntity> UserRepository { get; }
        IRepository<RolEntity> RolRepository { get; }
        IRepository<PermissionEntity> PermissionRepository { get; }
        IRepository<RolUserEntity> RolUserRepository { get; }
        IRepository<RolPermissionEntity> RolPermissionRepository { get; }

        IRepository<SpeciesEntity> SpeciesRepository { get; }
        IRepository<BreedEntity> BreedRepository { get; }
        IRepository<PetEntity> PetRepository { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();
    }
}
