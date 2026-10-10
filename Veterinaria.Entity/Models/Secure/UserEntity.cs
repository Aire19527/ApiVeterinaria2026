using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Entity.Models.Secure
{
    [Table("User", Schema = "Security")]
    public class UserEntity
    {
        [Key]
        public int IdUser { get; set; }

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

        public virtual List<RolUserEntity> RolUserEntities { get; set; } = new List<RolUserEntity>();
    }
}
