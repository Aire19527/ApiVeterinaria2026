using Microsoft.EntityFrameworkCore.Storage;
using Veterina.Core.Data;
using Veterina.Core.Repository;
using Veterina.Core.Repository.Interface;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Entity.Models.Secure;

namespace Veterina.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Attributes
        private readonly DataContext _context;
        private bool disposed = false;
        #endregion Attributes

        #region builder
        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        #endregion

        #region Properties

        private IRepository<UserEntity> userRepository;
        private IRepository<RolEntity> rolRepository;
        #endregion

        #region Members

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<UserEntity>(_context);

                return userRepository;
            }
        }
        public IRepository<RolEntity> RolRepository
        {
            get
            {
                if (rolRepository == null)
                    rolRepository = new Repository<RolEntity>(_context);

                return rolRepository;
            }
        }
        #endregion



        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
