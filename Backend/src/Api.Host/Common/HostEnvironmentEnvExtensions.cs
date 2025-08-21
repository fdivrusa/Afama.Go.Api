namespace Afama.Go.Api.Host.Common;

public static class HostEnvironmentEnvExtensions
{
    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("Local");
    }
}
