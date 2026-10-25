using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entity.DTO.Users
{
    public class UserUpdateDto
    {
        public int IdUser { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(150)]
        [Required]
        public string Email { get; set; } = null!;
    }
}
