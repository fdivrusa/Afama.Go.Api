using Afama.Go.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afama.Go.Api.Infrastructure.Data.Configurations;
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.FirstName)
            .IsRequired()
            .HasMaxLength(64);
        builder.Property(m => m.LastName)
            .IsRequired()
            .HasMaxLength(64);
        builder.Property(m => m.BirthDate)
            .IsRequired(false);
        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(48);
        builder.Property(m => m.KnownPathologies)
            .IsRequired(false)
            .HasMaxLength(2048);
    }
}
