using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CinemaApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<CinemaAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Projection> Projections { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie>()
               .Property(z => z.Id)
               .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Order>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
               .HasOne<CinemaAppUser>(z => z.Owner)
               .WithOne(z => z.UserShoppingCart)
               .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<Order>()
               .HasOne<ShoppingCart>(z => z.ShoppingCart)
               .WithOne(z => z.Order)
               .HasForeignKey<Order>(z => z.ShoppingCartId);

            builder.Entity<Order>()
                .HasOne<CinemaAppUser>(z => z.Owner)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.OwnerId);

            builder.Entity<Projection>()
                .HasOne<Movie>(z => z.Movie)
                .WithMany(z => z.Projections)
                .HasForeignKey(z => z.MovieId);

            builder.Entity<Ticket>()
                .HasOne<Projection>(z => z.Projection)
                .WithMany(z => z.Tickets)
                .HasForeignKey(z => z.ProjectionId);

        }
    }
    
}
