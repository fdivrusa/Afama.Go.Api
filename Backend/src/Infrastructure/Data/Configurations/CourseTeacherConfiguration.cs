using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afama.Go.Api.Infrastructure.Data.Configurations;
public class CourseTeacherConfiguration : IEntityTypeConfiguration<CourseTeacher>
{
    public void Configure(EntityTypeBuilder<CourseTeacher> b)
    {
        b.HasKey(ct => new { ct.CourseId, ct.TeacherId });

        // Relationships
        // Cascade delete when a Course is deleted, Restrict delete when a Teacher is deleted
        //This ensures that if a Course is removed, all associated CourseTeacher entries are also removed.
        b.HasOne(ct => ct.Course)
         .WithMany(c => c.Teachers)
         .HasForeignKey(ct => ct.CourseId)
         .OnDelete(DeleteBehavior.Cascade);

        //This ensures that if a Teacher is removed, the associated CourseTeacher entries remain intact, preserving historical data.
        b.HasOne(ct => ct.Teacher)
         .WithMany()
         .HasForeignKey(ct => ct.TeacherId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(ct => new { ct.CourseId, ct.TeacherId }).IsUnique();
    }
}
