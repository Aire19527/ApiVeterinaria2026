using Microsoft.EntityFrameworkCore.Storage;
using Veterina.Core.Data;
using Veterina.Core.Repository;
using Veterina.Core.Repository.Interface;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Entity.Models.Secure;
using Veterinaria.Entity.Models.Verinaria;

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
        private IRepository<PermissionEntity> permissionRepository;
        private IRepository<RolUserEntity> rolUserRepository;
        private IRepository<RolPermissionEntity> rolPermissionRepository;
        private IRepository<SpeciesEntity> speciesRepository;
        private IRepository<BreedEntity> breedRepository;
        private IRepository<PetEntity> petRepository;
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
        public IRepository<PermissionEntity> PermissionRepository
        {
            get
            {
                if (permissionRepository == null)
                    permissionRepository = new Repository<PermissionEntity>(_context);

                return permissionRepository;
            }
        }
        public IRepository<RolUserEntity> RolUserRepository
        {
            get
            {
                if (rolUserRepository == null)
                    rolUserRepository = new Repository<RolUserEntity>(_context);

                return rolUserRepository;
            }
        }
        public IRepository<RolPermissionEntity> RolPermissionRepository
        {
            get
            {
                if (rolPermissionRepository == null)
                    rolPermissionRepository = new Repository<RolPermissionEntity>(_context);

                return rolPermissionRepository;
            }
        }
        public IRepository<SpeciesEntity> SpeciesRepository
        {
            get
            {
                if (speciesRepository == null)
                    speciesRepository = new Repository<SpeciesEntity>(_context);

                return speciesRepository;
            }
        }
        public IRepository<BreedEntity> BreedRepository
        {
            get
            {
                if (breedRepository == null)
                    breedRepository = new Repository<BreedEntity>(_context);

                return breedRepository;
            }
        }
        public IRepository<PetEntity> PetRepository
        {
            get
            {
                if (petRepository == null)
                    petRepository = new Repository<PetEntity>(_context);

                return petRepository;
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
