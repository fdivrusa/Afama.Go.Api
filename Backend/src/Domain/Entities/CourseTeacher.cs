public class CourseTeacher : BaseAuditableEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid TeacherId { get; set; }
    public Member Teacher { get; set; } = null!;

    public CourseTeacherType CourseTeacherType { get; set; } = CourseTeacherType.Main;
}
