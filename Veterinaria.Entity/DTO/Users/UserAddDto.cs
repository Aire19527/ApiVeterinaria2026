using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Users
{
    public class UserAddDto
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(150)]
        [Required]
        public string Email { get; set; } = null!;

        [MaxLength(1000)]
        [Required]
        public string Password { get; set; } = null!;
    }
}
