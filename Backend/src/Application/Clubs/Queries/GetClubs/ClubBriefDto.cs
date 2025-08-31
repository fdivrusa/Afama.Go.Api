using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Clubs.Queries.GetClubs;
public class ClubBriefDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Club, ClubBriefDto>()
                .ForMember(x => x.Location, opt => opt.MapFrom(src => src.Street + " " + src.Number + " - " + src.ZipCode + " " + src.City));
        }
    }
}
