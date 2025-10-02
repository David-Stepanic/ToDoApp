using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoTest.Models
{
    public class ToDoContext : IdentityDbContext<ApplicationUser>
    {

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDo> ToDos { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.User)
                .WithMany(u => u.ToDos)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(
                    new Category { CategoryId = "work", Name="Work"},
                    new Category { CategoryId = "home", Name = "Home" },
                    new Category { CategoryId = "ex", Name = "Exercise" },
                    new Category { CategoryId = "shop", Name = "Shopping" },
                    new Category { CategoryId = "call", Name = "Contact" }
                );
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "closed", Name = "Completed" }
                );
        }
    }
}
