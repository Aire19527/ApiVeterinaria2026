using Veterina.Core.Repository;
using Veterina.Core.Repository.Interface;
using Veterina.Core.UnitOfWork;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Domain.Services;
using Veterinaria.Domain.Services.Interfaces;

namespace API.Veterinaria.Handlers
{
    public static class DependencyInyectionHandler
    {
        public static void DependencyInyectionConfig(IServiceCollection services)
        {
            // Repository await UnitofWork parameter ctor explicit
            //services.AddScoped<CustomValidationFilterAttribute, CustomValidationFilterAttribute>();

            // Infrastructure
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            services.AddScoped<IRolServices, RolServices>();
            services.AddScoped<ISpeciesServices, SpeciesServices>();
            services.AddScoped<IBreedServices, BreedServices>();
        }
    }
}
