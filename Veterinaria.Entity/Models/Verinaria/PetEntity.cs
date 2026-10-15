using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Entity.Models.Secure;

namespace Veterinaria.Entity.Models.Verinaria
{
    [Table("Pet", Schema = "Vet")]
    public class PetEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [ForeignKey("SpeciesEntity")]
        public int SpecieId { get; set; }
        public SpeciesEntity SpeciesEntity { get; set; } = null!;

        [ForeignKey("UserEntity")]
        public int UserId { get; set; }
        public UserEntity UserEntity { get; set; } = null!;

        public DateTime BirthDate { get; set; }
        public decimal Weigth { get; set; }

        [MaxLength(500)]
        public string? PhotoPath { get; set; }
    }
}
