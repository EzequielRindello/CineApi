using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class Director
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required][MaxLength(200)] public string Name { get; set; }

        [Required][MaxLength(100)] public string Nationality { get; set; }

        // Navigation property
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
