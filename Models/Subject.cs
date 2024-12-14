using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Chapter>? Chapters { get; set; }
    }
}
