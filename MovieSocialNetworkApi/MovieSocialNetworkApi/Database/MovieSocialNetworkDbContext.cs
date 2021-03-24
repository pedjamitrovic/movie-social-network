﻿using Microsoft.EntityFrameworkCore;
using MovieSocialNetworkApi.Entities;

namespace MovieSocialNetworkApi.Database
{
    public class MovieSocialNetworkDbContext : DbContext
    {
        public MovieSocialNetworkDbContext(DbContextOptions<MovieSocialNetworkDbContext> options) : base(options)
        {
        }

        public DbSet<Content> Contents { get; set; }
        public DbSet<SystemEntity> SystemEntities { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<GroupAdmin> GroupAdmins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Relation>()
                .HasKey(e => new { e.FollowingId, e.FollowerId });

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.Follower)
                .WithMany(e => e.Following)
                .HasForeignKey(e => e.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.Following)
                .WithMany(e => e.Followers)
                .HasForeignKey(e => e.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(e => e.Reporter)
                .WithMany(e => e.ReporterReports)
                .HasForeignKey(e => e.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(e => e.ReportedSystemEntity)
                .WithMany(e => e.ReportedReports)
                .HasForeignKey(e => e.ReportedSystemEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupAdmin>()
                .HasKey(e => new { e.GroupId, e.AdminId });

            modelBuilder.Entity<GroupAdmin>()
                .HasOne(e => e.Group)
                .WithMany(e => e.GroupAdmin)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupAdmin>()
                .HasOne(e => e.Admin)
                .WithMany(e => e.GroupAdmin)
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
