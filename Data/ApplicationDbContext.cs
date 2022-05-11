using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

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

        public DbSet<Order> Order { get; set; }

        public DbSet<CartItem> CartItem { get; set; }

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

            builder.Entity<OrderItem>(b =>
            {
                b.HasKey(c => new { c.BookIsBn, c.OrderId });
                b.HasOne(c => c.Book)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(c => c.Order)
                .WithMany(u => u.Items)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}