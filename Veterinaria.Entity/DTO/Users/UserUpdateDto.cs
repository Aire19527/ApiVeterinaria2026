using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Users
{
    public class UserUpdateDto
    {
        public int IdUser { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede superar {1} caracteres.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Apellido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede superar {1} caracteres.")]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El campo {0} no puede superar {1} caracteres.")]
        public string Email { get; set; } = null!;
    }
}
