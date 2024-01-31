using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Contexts;
public class UserDataContext(DbContextOptions<UserDataContext> options) : DbContext(options)
{
    public virtual DbSet<UserRoleEntity> Roles { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<UserAuthenticationEntity> Authentications { get; set; }
    public virtual DbSet<UserProfileEntity> Profiles { get; set; }
    public virtual DbSet<UserAddressEntity> Addresses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Property(b => b.LastLogin)
            .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<UserEntity>()
            .Property(b => b.RegistrationDate)
            .HasDefaultValueSql("getdate()");
    }
}
