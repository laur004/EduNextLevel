using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Titlu capitolui este obligatoriu")]
        public string ChapterTitle { get; set; }

        public virtual Subject? Subject { get; set; }

        //public virtual Grade? Grade { get; set; }
    }
}
