using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Members.Queries.GetMembers;
public class MemberBriefDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string MemberType { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Member, MemberBriefDto>()
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src => src.MemberType.ToString()));
        }
    }
}
