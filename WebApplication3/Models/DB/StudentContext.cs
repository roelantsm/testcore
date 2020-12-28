using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models.DB
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
                
        }
        
        public DbSet<Student> Students { get; set; }
        
        
         // protected override void OnModelCreating(ModelBuilder modelBuilder)
         // {
         //   base.OnModelCreating(modelBuilder);
         //   modelBuilder.Seed();
         // }
    }
}