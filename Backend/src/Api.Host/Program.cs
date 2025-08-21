using System.Globalization;
using Afama.Go.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

string[] supportedLanguages = { "en", "nl", "fr", "de" };

Log.Logger = new LoggerConfiguration()
     .MinimumLevel.Debug()
     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
     .Enrich.FromLogContext()
     .Enrich.WithMachineName()
     .Enrich.WithThreadId()
     .Enrich.WithEnvironmentUserName()
     .Enrich.WithClientIp()
     .WriteTo.ApplicationInsights(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING"), TelemetryConverter.Traces)
     .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.AddAzureAppConfiguration();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AfamaGo API", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    { $"api://{builder.Configuration["AzureAd:ClientId"]}/.default", "Access AfamaGo API" }
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { $"api://{builder.Configuration["AzureAd:ClientId"]}/.default" }
        }
    });
});

builder.Services.AddProblemDetails();

try
{
    Log.Information("Initialising application");
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        await app.InitialiseDatabaseAsync();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseManager API v1");
            c.OAuthClientId(builder.Configuration["AzureAd:SwaggerClientId"]);
            c.OAuthUsePkce();
        });

        app.Map("/", () => Results.Redirect("/swagger"));
    }
    else
    {
        app.UseHsts();
        app.Map("/", () => Results.Redirect("/api"));
    }

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseRequestLocalization(options =>
    {
        options.DefaultRequestCulture = new RequestCulture(supportedLanguages[0]);

        var supportedLanguagesCultures = supportedLanguages.Select(lang => new CultureInfo(lang)).ToList();
        options.SupportedCultures = supportedLanguagesCultures;
        options.SupportedUICultures = supportedLanguagesCultures;
    });

    app.UseSerilogRequestLogging();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.MapEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program { }
