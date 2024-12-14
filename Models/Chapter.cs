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
        public string ChapterTitle { get; set; }

        public int? SubjectId { get; set; }

        //public int? GradeId { get; set; }

        public virtual Subject? Subject { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? Subj { get; set; }



        //public virtual Grade? Grade { get; set; }

        //[NotMapped]
        //public IEnumerable<SelectListItem>? Grad { get; set; }

    }
}
