using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<ResourcePermission> ResourcePermissions => Set<ResourcePermission>();
    public DbSet<SigningKey> SigningKey => Set<SigningKey>();

    public DbSet<OAuthClient> OAuthClients => Set<OAuthClient>();

    public DbSet<PasswordSetLinks> PasswordSetLinks => Set<PasswordSetLinks>();

    public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>(); 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure composite primary keys for join entities
        modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
        modelBuilder.Entity<ResourcePermission>().HasKey(rp => new { rp.ResourceId, rp.PermissionId });

        // Configure relationships
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResourcePermission>()
            .HasOne(rp => rp.Resource)
            .WithMany(r => r.ResourcePermissions)
            .HasForeignKey(rp => rp.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResourcePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Resource>()
            .HasOne(r => r.ParentResource)
            .WithMany(r => r.ChildResources)
            .HasForeignKey(r => r.ParentResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
        .Property(u => u.Type)
        .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder.SeedInitialData();
    }
}
