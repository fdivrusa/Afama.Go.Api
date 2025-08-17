
using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Application.Members.Queries.GetMemberDetails;
using Afama.Go.Api.Application.Members.Queries.GetMembers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Afama.Go.Api.Host.Endpoints;

public class Members : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/members")
            .RequireAuthorization()
            .MapGet(GetMembers)
            .MapGet(GetMemberDetails, "{id}")
            .MapPost(CreateMember);
    }

    public async Task<Ok<IEnumerable<MemberBriefDto>>> GetMembers(ISender sender, [AsParameters] GetMembersQuery query)
    {
        var members = await sender.Send(query);
        return TypedResults.Ok(members);
    }

    public async Task<Results<Ok<MemberDetailsDto>, NotFound>> GetMemberDetails(ISender sender, Guid id)
    {
        var query = new GetMemberDetailsQuery { MemberId = id };
        var memberDetails = await sender.Send(query);
        return TypedResults.Ok(memberDetails);
    }

    public async Task<Created<Guid>> CreateMember(ISender sender, CreateMemberCommand command)
    {
        var memberId = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Members)}/{memberId}", memberId);
    }
}
