namespace Afama.Go.Api.Infrastructure.Data.Configurations;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Club> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
        builder.Property(x => x.Street).HasMaxLength(256);
        builder.Property(x => x.ZipCode).HasMaxLength(16);
        builder.Property(x => x.Number);
        builder.Property(x => x.Box).HasMaxLength(16);
        builder.Property(x => x.City).HasMaxLength(128);
        builder.Property(x => x.Country).HasMaxLength(64);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
