using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Titlul capitolului este obligatoriu")]
        [StringLength(100, ErrorMessage = "Lungimea maxima trebuie sa fie 100")]
        [MinLength(4, ErrorMessage = "Lungimea trebuie sa fie minim 4")]
        public string ChapterTitle { get; set; }

        [Required(ErrorMessage = "Materia este obligatorie!")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Clasa este obligatorie!")]
        public int? GradeId { get; set; }



        public virtual ICollection<Article>? Articles { get; set; }


        public virtual Subject? Subject { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? Subj { get; set; }



        public virtual Grade? Grade { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Grad { get; set; }

    }
}
