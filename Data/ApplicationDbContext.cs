using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Store> Store { get; set; }

        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CartItem>(b =>
            {
                b.HasKey(c => new { c.BookIsBn, c.UserId });
                b.HasOne(c => c.Book)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public DbSet<WebApplication1.Models.CartItem> CartItem { get; set; }
    }
}