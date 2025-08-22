namespace Afama.Go.Api.Domain.Entities;
public class Club : BaseAuditableEntity
{
    public string Name { get; set; } = "";
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int? Number { get; set; }
    public string? Box { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    public ICollection<Course> Courses { get; set; } = [];
    public ICollection<Member> Members { get; set; } = [];
}
