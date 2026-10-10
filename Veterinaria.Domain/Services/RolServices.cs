using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.Models.Secure;

namespace Veterinaria.Domain.Services
{
    public class RolServices: IRolServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        public async Task<List<RolEntity>> GetAll()
        {
            var rols= await _unitOfWork.RolRepository.GetAllAsync();

            return rols;

        }
    }
}
