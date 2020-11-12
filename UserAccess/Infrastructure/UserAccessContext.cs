using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccess.Domain.Users.Rfas;
using UserAccess.Domain.Roles;
using UserAccess.Domain.Users;
using System.Collections.Generic;
using System;

namespace UserAccess.Infrastructure
{
    public class UserAccessContext : DbContext
    {

        public DbSet<Rfa> Rfas { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public UserAccessContext(DbContextOptions<UserAccessContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

    }

    internal class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(200);
            builder.Property(x => x.FirstName).HasColumnName("FirstName").HasMaxLength(200);
            builder.Property(x => x.LastName).HasColumnName("LastName").HasMaxLength(200);
            builder.Property(x => x.IsActive).HasColumnName("IsActive");
            builder.HasOne<Role>("Role");
        }
    }

    internal class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(200);
            builder.Property(x => x.Permissions).HasColumnName("Permissions");

            builder.HasData(new List<Role>{
                new Role(new Name("rfa"), new Permission(32)){Id = Guid.NewGuid() },
                new Role(new Name("admin"), new Permission(64)){Id = Guid.NewGuid() },
                new Role(new Name("default"), new Permission(1)){Id = Guid.NewGuid() }
                });
        }
    }

    internal class RfaEntityTypeConfiguration : IEntityTypeConfiguration<Rfa>
    {
        public void Configure(EntityTypeBuilder<Rfa> builder)
        {
            builder.ToTable("RfaInfo");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(10);
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.Profession).HasMaxLength(200);
        }
    }
}

