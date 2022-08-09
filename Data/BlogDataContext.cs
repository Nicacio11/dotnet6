using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Blog.Data.Mappings;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        // public DbSet<PostTag> PostTags { get; set; }
        // public DbSet<Role> Roles { get; set; }
        // public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        // public DbSet<PostWithTagsCount> PostWithTagsCount { get; set; }
        // public DbSet<UserRole> UserRoles { get; set; }

        public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            // modelBuilder.Entity<PostWithTagsCount>(x => 
            // {
            //     x.ToSqlQuery("SELECT TITLE, COUNT(Id) as Count FROM TAGS WHERE PostId = Id");
            // });
        }
    }
}