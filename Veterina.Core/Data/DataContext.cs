using Microsoft.EntityFrameworkCore;
using Veterinaria.Entity.Models.Secure;
using Veterinaria.Entity.Models.Verinaria;

namespace Veterina.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<UserEntity>  UserEntities { get; set; }
        public DbSet<RolEntity>  RolEntities { get; set; }
        public DbSet<RolPermissionEntity> RolPermissionEntities { get; set; }
        public DbSet<RolUserEntity> RolUserEntities { get; set; }
        public DbSet<PermissionEntity> PermissionEntities { get; set; }

        public DbSet<BreedEntity> BreedEntities { get; set; }
        public DbSet<PetEntity> PetEntities { get; set; }
        public DbSet<SpeciesEntity> SpeciesEntities { get; set; }
    }
}
