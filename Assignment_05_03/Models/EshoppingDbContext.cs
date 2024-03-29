using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Assignment_05_03.Models
{
    public class EshoppingDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public EshoppingDbContext()
        {

        }
        // DI Resolve
        public EshoppingDbContext(DbContextOptions<EshoppingDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryUniqueId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
