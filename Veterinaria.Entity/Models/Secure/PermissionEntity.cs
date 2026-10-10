using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Entity.Models.Secure
{
    [Table("Permission", Schema = "Security")]
    public class PermissionEntity
    {
        [Key]
        public int IdPermission { get; set; }

        [MaxLength(50)]
        [Required]
        public string Permission { get; set; } = null!;


        [MaxLength(300)]
        public string? Description { get; set; }

        public virtual List<RolPermissionEntity> RolPermissionEntities { get; set; } = new List<RolPermissionEntity>();
    }
}
