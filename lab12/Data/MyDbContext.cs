using lab12.Models;
using Microsoft.EntityFrameworkCore;

namespace lab12.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Article> Article { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                        .HasOne(a => a.Category)
                        .WithMany(c => c.Articles)
                        .HasForeignKey(a => a.CategoryId);
        }
    }
}