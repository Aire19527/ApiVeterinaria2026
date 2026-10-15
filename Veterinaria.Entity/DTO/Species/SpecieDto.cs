using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Species
{
    public class SpecieDto
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
