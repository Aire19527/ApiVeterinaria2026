using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Tokens
{
    public class LoginDto
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El email del usuario es requerido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Formato inválido de Email.")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;
    }
}
