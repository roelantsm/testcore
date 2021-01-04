using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models.DB
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //     => options.UseSqlite("Data Source=blogging.db");


        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //
        //     foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
        //         .SelectMany(e => e.GetForeignKeys()))
        //     {
        //         foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        //     }
        // }
    }
}