namespace Afama.Go.Api.Domain.Entities;
public class Course : BaseAuditableEntity
{
    public string Name { get; set; } = "";
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public Guid ClubId { get; set; }
    public Club Club { get; set; } = null!;

    public ICollection<CourseTeacher> Teachers { get; set; } = [];
}
