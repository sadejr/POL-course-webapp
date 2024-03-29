﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ArticleDbContext : IdentityDbContext<ApplicationUser>
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category) // Each Article has one Category
                .WithMany(c => c.Articles) // Each Category has many Articles
                .HasForeignKey(a => a.CategoryId); // The foreign key in the Article table that points to Category

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Article { get; set; }
    }
}
