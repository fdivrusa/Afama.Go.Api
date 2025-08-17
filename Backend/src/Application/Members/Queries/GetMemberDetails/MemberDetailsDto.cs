namespace Afama.Go.Api.Application.Members.Queries.GetMemberDetails;
public class MemberDetailsDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string MemberType { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Member, MemberDetailsDto>()
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src => src.MemberType.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastModified));
        }
    }
}


