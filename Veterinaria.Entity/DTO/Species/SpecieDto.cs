using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Species
{
    public class SpecieDto
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede superar {1} caracteres.")]
        public string Name { get; set; } = null!;
    }
}
