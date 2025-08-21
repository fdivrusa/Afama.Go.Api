# AfamaGo - Ki-Do-Kaï Club Management System

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

**CRITICAL**: Set appropriate timeouts (60+ minutes) for all build commands. DO NOT use default timeouts that may cause premature cancellation.

### Bootstrap, Build, and Test the Repository

Follow these commands in exact order:

```bash
# Install .NET 9.0.100 SDK (REQUIRED - application uses .NET 9)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.100
export PATH="$HOME/.dotnet:$PATH"

# Install Entity Framework tools globally
dotnet tool install --global dotnet-ef

# Navigate to Backend directory
cd Backend

# Restore packages - takes 30-35 seconds. NEVER CANCEL.
dotnet restore Afama.Go.Api.sln

# Build the solution - takes 10-15 seconds. NEVER CANCEL.
dotnet build Afama.Go.Api.sln --configuration Release --no-restore

# Run all tests - takes 3-5 seconds. NEVER CANCEL.
dotnet test Afama.Go.Api.sln --configuration Release --no-build --verbosity normal
```

**TIMEOUT RECOMMENDATIONS**:
- Package restore: Set timeout to 120+ seconds
- Build commands: Set timeout to 60+ seconds  
- Test commands: Set timeout to 30+ seconds
- **NEVER CANCEL** any build or test command - wait for completion

### Run the Application

```bash
# Navigate to API Host project
cd src/Api.Host

# Development: Uses InMemory database (no setup required)
ASPNETCORE_ENVIRONMENT=Development dotnet run --urls "http://localhost:5000"

# The API will be available at:
# - Development: http://localhost:5000
# - Swagger UI: http://localhost:5000/swagger
```

**Note**: Application uses InMemory database in Development environment. For Production, requires SQL Server with proper connection string configuration.

### Azure Deployment

```bash
# Initialize Azure resources
azd auth login
azd init

# Deploy to Azure - takes 10-20 minutes. NEVER CANCEL. Set timeout to 30+ minutes.
azd up
```

## Validation

**CRITICAL**: Always manually validate any changes via these complete end-to-end scenarios:

### Scenario 1: Fresh Development Setup
1. Install .NET 9.0.100 SDK and EF tools
2. Navigate to Backend directory
3. Run `dotnet restore` (expect 30-35 seconds)
4. Run `dotnet build` (expect 10-15 seconds)
5. Run `dotnet test` (expect 3-5 seconds, 50+ tests should pass)
6. Run the API application
7. Access Swagger UI at http://localhost:5000/swagger
8. Verify API endpoints are accessible

### Scenario 2: Code Changes Validation
After making any code changes:
1. Run `dotnet build` to ensure compilation succeeds
2. Run `dotnet test` to ensure no regressions
3. Start the application and test affected endpoints
4. Verify Swagger documentation is updated if API changes were made

### Scenario 3: Database Migration Validation
For Infrastructure changes affecting database:
1. Create migration: `dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Api.Host`
2. Apply migration: `dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host`
3. Verify application starts successfully
4. Test CRUD operations for affected entities

### Always Run Before Completing Tasks
```bash
# Build and test in Release configuration
cd Backend
dotnet build Afama.Go.Api.sln --configuration Release
dotnet test Afama.Go.Api.sln --configuration Release --no-build

# Start application and verify it runs
cd src/Api.Host
ASPNETCORE_ENVIRONMENT=Development dotnet run --urls "http://localhost:5000"
# Access http://localhost:5000/swagger to verify API is responding
```

## Project Architecture

### Key Projects in the Solution
- **src/Api.Host**: Web API entry point and hosting configuration
- **src/Application**: Application logic, use cases, CQRS handlers (MediatR)  
- **src/Domain**: Domain entities, business rules, and domain events
- **src/Infrastructure**: External concerns (database, logging, Azure services)
- **tests/**: Comprehensive test coverage (Unit, Integration, Functional)
- **infra/**: Azure Bicep templates for infrastructure as code

### Important File Locations
- Main solution: `Backend/Afama.Go.Api.sln`
- Package management: `Backend/Directory.Packages.props` (centralized)
- API configuration: `Backend/src/Api.Host/appsettings.*.json`
- Database context: `Backend/src/Infrastructure/Data/ApplicationDbContext.cs`
- EF migrations: `Backend/src/Infrastructure/Data/Migrations/`

## Common Tasks

### Code Scaffolding (Clean Architecture Templates)
```bash
cd Backend/src/Application

# Install template if not available
dotnet new install Clean.Architecture.Solution.Template::9.0.8

# Create a new command
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int

# Create a new query  
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

### Database Operations
```bash
cd Backend

# Add new migration
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Api.Host

# Update database
dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host

# Remove last migration
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api.Host
```

### Azure Infrastructure Management
```bash
# Deploy infrastructure only
azd provision

# Deploy application only  
azd deploy

# View environment variables
azd env get-values
```

## Build System Troubleshooting

### Path Inconsistencies (KNOWN ISSUES)
- **Backend/README.md** incorrectly references `src\Web\` - should be `src\Api.Host\`
- **Backend/build.cake** script has incorrect webServerPath - should be `./src/Api.Host`
- **Backend/azure.yaml** references wrong project path - should be `./src/Api.Host`

### Database Configuration
- **Development**: Uses InMemory database (no setup required)
- **Production**: Requires SQL Server with connection string in appsettings
- Existing migrations are SQL Server-specific, may require regeneration for other providers

### Package Version Management
Uses Central Package Management. All versions defined in `Backend/Directory.Packages.props`:
- EfcoreVersion: 9.0.0
- AspnetVersion: 9.0.0
- MicrosoftExtensionsVersion: 9.0.0

## CI/CD Pipeline

### GitHub Actions Workflow
Located at `.github/workflows/backend-ci.yml`:
- Triggers on push/PR to Backend/ directory
- Runs on Ubuntu latest
- Uses .NET 9.0.100 SDK
- Executes restore, build, and all test suites
- Uploads test results as artifacts

### Test Coverage
- **Unit Tests**: 50+ tests in Application and Domain layers
- **Integration Tests**: Infrastructure and database testing
- **Functional Tests**: End-to-end API scenarios
- All tests complete in under 5 seconds total

## Performance Expectations

### Command Timing (Fresh Environment)
- .NET SDK installation: 2-3 minutes
- Package restore (first time): 30-35 seconds
- Solution build (Release): 10-15 seconds
- Test execution: 3-5 seconds
- Application startup: 5-10 seconds

### Timeout Settings for Tools
- **Restore operations**: 120+ seconds
- **Build operations**: 60+ seconds
- **Test operations**: 30+ seconds
- **Azure deployment**: 30+ minutes
- **Application startup**: 60+ seconds

## Validation URLs and Endpoints

### Development URLs
- **API Base**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health

### Production URLs (Azure)
- **API Base**: https://[your-app-name].azurewebsites.net
- **Swagger UI**: https://[your-app-name].azurewebsites.net/swagger

## Prerequisites and Installation Commands

### Required SDKs and Tools
```bash
# .NET 9.0.100 SDK (EXACT VERSION REQUIRED)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.100

# Entity Framework Core tools
dotnet tool install --global dotnet-ef

# Azure Developer CLI (for deployment)
curl -fsSL https://aka.ms/install-azd.sh | bash

# Ensure PATH includes .NET
export PATH="$HOME/.dotnet:$PATH"
```

### Verification Commands
```bash
# Verify .NET installation
dotnet --version  # Should output: 9.0.100

# Verify EF tools
dotnet ef --version  # Should work without errors

# Verify Azure CLI
azd version  # Should show Azure Developer CLI version
```

**CRITICAL REMINDER**: Always set explicit timeouts of 60+ minutes for build commands and 30+ minutes for test commands. DO NOT stop long running commands - builds may take longer than expected on slower systems.