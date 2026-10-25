using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Breeds
{
    public class BreedAddDto
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede superar {1} caracteres.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Especie")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor a 0.")]
        public int SpecieId { get; set; }
    }
}
