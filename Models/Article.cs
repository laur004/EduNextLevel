using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
	public class Article
	{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere")]
        [MinLength(5, ErrorMessage = "Titlul trebuie sa aiba minim 5 caractere")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Continutul articolului este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }


        // PASUL 6: useri si roluri
        // cheie externa (FK) - un articol este postat de catre un user
        public string? UserId { get; set; }

        //proprietatea virtuala - un articol este postat de catre un user
        public virtual ApplicationUser? User { get; set; }


        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int? ChapterId { get; set; }

        public string? Image { get; set; } // Calea imaginii (opțională)
        public string? PdfPath { get; set; } // Calea fișierului PDF (opțional)

        public virtual Chapter? Chapter { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Chap { get; set; }


    }
}
