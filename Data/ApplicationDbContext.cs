using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Models;

namespace ProiectDAW.Data
{
    //PAS3
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Grade> Grades { get; set; }    

        public DbSet<Subject> Subjects { get; set; }
        
        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
