using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class Director
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Nationality { get; set; }

        // Navigation property
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
