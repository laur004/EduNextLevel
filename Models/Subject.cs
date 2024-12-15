using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele materiei este obligatoriu")]
        [StringLength(100, ErrorMessage = "Lungimea maxima trebuie sa fie 100")]
        [MinLength(4, ErrorMessage = "Lungimea trebuie sa fie minim 4")]
        public string Name { get; set; }

        public virtual ICollection<Chapter>? Chapters { get; set; }
    }
}
