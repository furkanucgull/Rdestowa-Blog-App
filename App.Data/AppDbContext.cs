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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İlişki tanımlamaları ve diğer konfigürasyonlar burada yapılır.

            // Örnek: Comment tablosundaki UserID sütununa yapılan referans değişikliği
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Daha fazla konfigürasyonlar eklenebilir...
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
                new UserEntity { Id = 1, Email = "Alp@example.com", Password = "alp0123", Name = "Alp Zenginoglu", Address = "Address 1", Phone = "1234567890", IsEmailConfirmed = true },
                new UserEntity { Id = 2, Email = "Maro@example.com", Password = "Maro0123", Name = "Mariam Kobakhidze", Address = "Address 2", Phone = "0987654321" },
                 new UserEntity { Id = 3, Email = "Maryia@example.com", Password = "Maryia0123", Name = "Maryia Chamruk", Address = "Address 2", Phone = "0987654324" },
                  new UserEntity { Id = 4, Email = "Camilla@example.com", Password = "Camilla0123", Name = "Camilla Carbonero", Address = "Address 2", Phone = "095765421" },
                   new UserEntity { Id = 5, Email = "Furkan@example.com", Password = "Furkan0123", Name = "Furkan Ucgul", Address = "Address 2", Phone = "095454332" }
            );

            // BlogEntity seed
            modelBuilder.Entity<BlogEntity>().HasData(
                new BlogEntity { PostID = 1, Title = "Blog 1", Content = "Content of Blog 1", UserID = 1, Author = "Alp Zenginoglu", CommentCount = 15, Category = "Electronik" },
                new BlogEntity { PostID = 2, Title = "Blog 2", Content = "Content of Blog 2", UserID = 2, Author = "Mariam Kobakhidze", CommentCount = 7, Category = "Nature" }
            );

            // CommentEntity seed
            modelBuilder.Entity<CommentEntity>().HasData(
                new CommentEntity { CommentID = 1, Content = "Comment 1", UserID = 1, PostID = 1 },
                new CommentEntity { CommentID = 2, Content = "Comment 2", UserID = 2, PostID = 1 }

            );
        }
    }

}
