using Afama.Go.Api.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Afama.Go.Api.Application.Common.Behaviours;
public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, IUser user) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly IUser _user = user;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.Id ?? string.Empty;
        string? userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = _user.Name;
        }

        _logger.LogInformation("Afama.Go.Api Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);

        return Task.CompletedTask;
    }
}
