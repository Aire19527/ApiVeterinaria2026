using Microsoft.EntityFrameworkCore.Storage;
using Veterina.Core.Repository.Interface;
using Veterinaria.Entity.Models.Secure;

namespace Veterina.Core.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IRepository<UserEntity> UserRepository { get; }
        IRepository<RolEntity> RolRepository { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();
    }
}
