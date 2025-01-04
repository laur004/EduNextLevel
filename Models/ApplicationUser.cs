using Microsoft.AspNetCore.Identity;

//PAS1

namespace ProiectDAW.Models
{
    public class ApplicationUser: IdentityUser
    {
        // PASUL 6: useri si roluri
        // un user poate posta mai multe comentarii
        public virtual ICollection<Comment>? Comments { get; set; }

        // un user poate posta mai multe articole
        public virtual ICollection<Article>? Articles { get; set; }
    }
}
