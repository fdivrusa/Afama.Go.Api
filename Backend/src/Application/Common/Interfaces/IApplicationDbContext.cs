using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Member> Members { get; }
    DbSet<Club> Clubs { get; }
    DbSet<CourseTeacher> CourseTeachers { get; }
    DbSet<Course> Courses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
