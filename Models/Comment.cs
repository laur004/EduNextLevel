using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}
