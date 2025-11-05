
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class CreateTheater
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Range(1, 500)]
        public int Capacity { get; set; }
    }
}