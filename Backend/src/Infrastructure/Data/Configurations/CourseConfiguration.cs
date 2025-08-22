using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afama.Go.Api.Infrastructure.Data.Configurations;
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(128);

        builder.HasOne(x => x.Club)
         .WithMany(c => c.Courses)
         .HasForeignKey(x => x.ClubId)
         .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ClubId, x.DayOfWeek, x.StartTime });
    }
}
