using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Entity.Models.Verinaria
{
    [Table("Species", Schema = "Vet")]
    public class SpeciesEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        public List<BreedEntity> BreedEntities { get; set; } = new List<BreedEntity>();
        public List<PetEntity> PetEntities { get; set; } = new List<PetEntity>();
    }

}
