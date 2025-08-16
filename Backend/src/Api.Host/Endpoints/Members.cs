
using Afama.Go.Api.Application.Members.Queries.GetMembers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Afama.Go.Api.Web.Endpoints;

public class Members : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/members")
            .RequireAuthorization()
            .MapGet(GetMembers);
    }

    public async Task<Ok<IEnumerable<MemberBriefDto>>> GetMembers(ISender sender, [AsParameters] GetMembersQuery query)
    {
        var members = await sender.Send(query);
        return TypedResults.Ok(members);
    }
}
