using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserImageEntity> UserImages { get; set; }
        public DbSet<BlogEntity> BlogPosts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<CategoryBlogEntity> CategoryBlogEntities { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<BlogImageEntity> BlogImages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // UserEntity seed
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity { Id = 1, Email = "Alp@example.com", Password = "0123", Name = "Alp Zenginoglu", Address = "Address 1", Phone = "1234567890", IsEmailConfirmed = true },
                new UserEntity { Id = 2, Email = "Maro@example.com", Password = "0123", Name = "Mariam Kobakhidze", Address = "Address 2", Phone = "0987654321", IsEmailConfirmed = true },
                 new UserEntity { Id = 3, Email = "Maryia@example.com", Password = "0123", Name = "Maryia Chamruk", Address = "Address 2", Phone = "0987654324", IsEmailConfirmed = true },
                  new UserEntity { Id = 4, Email = "Camilla@example.com", Password = "0123", Name = "Camilla Carbonero", Address = "Address 2", Phone = "095765421",IsEmailConfirmed = true },
                   new UserEntity { Id = 5, Email = "Furkan@example.com", Password = "0123", Name = "Furkan Ucgul", Address = "Address 2", Phone = "095454332",IsEmailConfirmed = true }
            );
        }

    }
    }



