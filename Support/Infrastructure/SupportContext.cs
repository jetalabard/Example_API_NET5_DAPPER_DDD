using Support.Domain.ApplicationInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Support.Infrastructure
{
    public class SupportContext : DbContext
    {
        public DbSet<ApplicationInfo> ApplicationInfos { get; set; }

        public SupportContext(DbContextOptions<SupportContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    internal class ApplicationInfoEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationInfo>
    {
        public void Configure(EntityTypeBuilder<ApplicationInfo> builder)
        {
            builder.ToTable("ApplicationInfo");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id");
            builder.Property(e => e.NameApp).HasMaxLength(200);
            builder.Property(e => e.CodeApp).HasMaxLength(200);
            builder.Property(e => e.VersionApp).HasMaxLength(10);

            builder.HasData(new ApplicationInfo(new NameApp("Example"), new CodeApp("Example_Code"), new VersionApp("0.1.0")));
        }
    }
}

