using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Infrastructure.Data;
using Afama.Go.Api.Host.Services;
using Afama.Go.Api.Application.Members.Commands;
using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAutoMapper(typeof(Program), typeof(CreateMemberCommand));

        builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "Afama.Go.Api API";
        });
    }

    public static void AddAzureAppConfiguration(this IHostApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            var azAppConfigUri = new Uri($"https://{builder.Configuration["AzureAppConfig:InstanceName"]}.azconfig.io");
            builder.Configuration.AddAzureAppConfiguration(options => options.Connect(azAppConfigUri, new ManagedIdentityCredential())
            .ConfigureKeyVault(kv => kv.SetCredential(new ManagedIdentityCredential())));
        }
        else
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }
}
