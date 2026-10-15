using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Entity.DTO.Breeds
{
    public class BreedDto : BreedUpdateDto
    {
        public string Specie { get; set; } = null!;
    }
}
