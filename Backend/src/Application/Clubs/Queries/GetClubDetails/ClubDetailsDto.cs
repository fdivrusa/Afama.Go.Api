using Afama.Go.Api.Application.Members.Queries.GetMembers;
using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Clubs.Queries.GetClubDetails;
public class ClubDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Street { get; set; } = string.Empty;
    public string? ZipCode { get; set; } = string.Empty;
    public int? Number { get; set; } = null;
    public string? Box { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public List<MemberBriefDto> Members { get; set; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Club, ClubDetailsDto>();
        }
    }
}
