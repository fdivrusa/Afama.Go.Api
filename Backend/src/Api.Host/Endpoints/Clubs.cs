using Afama.Go.Api.Application.Clubs.Queries.GetClubDetails;
using Afama.Go.Api.Application.Clubs.Queries.GetClubs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Afama.Go.Api.Host.Endpoints;

public class Clubs : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/clubs")
            .RequireAuthorization()
            .MapGet(GetClubs)
            .MapGet(GetClubDetails, "{id}");
    }
    public async Task<Ok<IEnumerable<ClubBriefDto>>> GetClubs(ISender sender, [AsParameters] GetClubsQuery query)
    {
        var clubs = await sender.Send(query);
        return TypedResults.Ok(clubs);
    }

    public async Task<Results<Ok<ClubDetailsDto>, NotFound>> GetClubDetails(ISender sender, Guid id)
    {
        var query = new GetClubDetailsQuery { ClubId = id };
        var clubDetails = await sender.Send(query);
        return TypedResults.Ok(clubDetails);
    }

}
