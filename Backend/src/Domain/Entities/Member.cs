using Afama.Go.Api.Domain.Common;
using Afama.Go.Api.Domain.Enums;

namespace Afama.Go.Api.Domain.Entities;
public class Member : BaseAuditableEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public MemberType MemberType { get; set; } = MemberType.Student;
    public string? KnownPathologies { get; set; }
}
