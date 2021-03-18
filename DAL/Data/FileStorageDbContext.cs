using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class FileStorageDbContext : IdentityDbContext<User, IdentityRole<int>, int,
        IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<FileStatistics> Statistics { get; set; }

        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasOne(x => x.Statistics)
                .WithOne(x => x.File)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<File>()
                .HasOne(x => x.User)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            modelBuilder.Entity<File>()
                .Property(x => x.Uploaded)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<File>()
                .Property(x => x.LastUpdated)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
