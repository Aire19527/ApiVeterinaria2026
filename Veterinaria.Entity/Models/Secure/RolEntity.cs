using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Entity.Models.Secure
{
    [Table("Rol", Schema = "Security")]
    public class RolEntity
    {
        [Key]
        public int IdRol { get; set; }

        [MaxLength(50)]
        [Required]
        public string Rol { get; set; } = null!;

        public virtual List<RolUserEntity> RolUserEntities { get; set; } = new List<RolUserEntity>();
        public virtual List<RolPermissionEntity> RolPermissionEntities { get; set; } = new List<RolPermissionEntity>();
    }
}
