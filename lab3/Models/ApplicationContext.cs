using Microsoft.EntityFrameworkCore;

namespace lab3.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Book> Books { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reader>()
                .HasMany(p => p.Books)
                .WithMany(c => c.Readers);

            base.OnModelCreating(modelBuilder);
        }
    }
}
