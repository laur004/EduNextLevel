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


        // PASUL 6: useri si roluri
        // cheie externa (FK) - un comentariu este postat de catre un user
        public string? UserId { get; set; }

        //proprietatea virtuala - un comentariu este postat de catre un user
        public virtual ApplicationUser? User { get; set; }

        public virtual Article Article { get; set; }
    }
}
