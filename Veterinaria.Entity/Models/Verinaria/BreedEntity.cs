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
    [Table("Breed", Schema = "Vet")]
    public class BreedEntity
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;


        [ForeignKey("SpeciesEntity")]
        public int SpecieId { get; set; }
        public SpeciesEntity SpeciesEntity { get; set; } = null!;
    }
}
