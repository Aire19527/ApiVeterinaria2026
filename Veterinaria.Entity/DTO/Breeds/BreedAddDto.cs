using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Breeds
{
    public class BreedAddDto
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        public int SpecieId { get; set; }
    }
}
