# AfamaGo - Ki-Do-Kaï Club Management System

[![Backend CI](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/download/dotnet/9.0)

AfamaGo is a modern club management system designed specifically for the Ki-Do-Kaï club, practicing Tai-Jitsu KJR style within the AFAMA federation in Belgium. This project serves as both a practical solution for club administration and a learning platform to explore and implement various Azure services and modern web development practices.

## 🥋 Project Purpose

This application is designed to manage various aspects of the Ki-Do-Kaï club practicing Tai-Jitsu KJR, including:
- Member registration and management
- Training session scheduling
- Event organization
- Communication tools
- Administrative functions

**Note**: This is a personal learning project focused on exploring Azure technologies rather than a commercial venture.

## 📊 Build & Test Status

| Branch | CI Status | Last Updated |
|--------|-----------|--------------|
| master | [![CI](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml) | ![Last Commit](https://img.shields.io/github/last-commit/fdivrusa/AfamaGo/master) |
| develop | [![CI](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml) | ![Last Commit](https://img.shields.io/github/last-commit/fdivrusa/AfamaGo/develop) |

### Test Coverage
- **Unit Tests**: Application & Domain layers
- **Integration Tests**: Infrastructure & Database
- **Functional Tests**: End-to-end API scenarios
- **Test Results**: View detailed test reports in [GitHub Actions](https://github.com/fdivrusa/AfamaGo/actions/workflows/backend-ci.yml)

## 🏗️ Architecture Overview

AfamaGo follows a modern, cloud-native architecture with clear separation of concerns:

### Backend (.NET 9)
- **Clean Architecture**: Domain-driven design with clear layer separation
- **API-First**: RESTful APIs with OpenAPI/Swagger documentation
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: Azure AD B2C integration
- **Logging**: Structured logging with Serilog
- **Testing**: Comprehensive unit, integration, and functional tests

### Frontend (Angular + BFF)
- **Angular**: Modern TypeScript-based SPA
- **BFF Pattern**: Backend-for-Frontend for secure API communication
- **Responsive Design**: Mobile-first approach
- **State Management**: NgRx for complex state scenarios

### Infrastructure as Code
- **Azure Bicep**: Infrastructure provisioning and management
- **Azure Developer CLI (azd)**: Streamlined deployment workflow
- **GitHub Actions**: CI/CD pipeline automation

## 📁 Project Structure

```
AfamaGo/
├── Backend/                    # .NET 9 Backend
│   ├── src/
│   │   ├── Api.Host/          # Web API entry point
│   │   ├── Application/       # Application logic & use cases
│   │   ├── Domain/           # Domain entities & business rules
│   │   └── Infrastructure/   # External concerns (DB, logging, etc.)
│   │       └── Data/
│   │           └── Migrations/  # EF Core migrations
│   ├── tests/               # Test projects
│   ├── infra/              # Azure Bicep templates
│   └── azure.yaml          # Azure Developer CLI configuration
├── Frontend/               # Angular Frontend (planned)
│   ├── bff/               # Backend-for-Frontend
│   └── client/            # Angular SPA
└── README.md
```

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (version 9.0.100)
- [Entity Framework Core tools](https://docs.microsoft.com/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)
- [SQL Server LocalDB or SQL Server Express](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (for local development)
- [Node.js 18+](https://nodejs.org/) (for future frontend)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/fdivrusa/AfamaGo.git
   cd AfamaGo
   ```

2. **Backend Setup**
   ```bash
   cd Backend
   dotnet restore Afama.Go.Api.sln
   dotnet build Afama.Go.Api.sln --configuration Release
   ```

3. **Database Setup**
   ```bash
   # Apply migrations to create/update local SQL Server database
   dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host
   ```

4. **Run the Backend API**
   ```bash
   cd src/Api.Host
   ASPNETCORE_ENVIRONMENT=Development dotnet run --urls "http://localhost:5000"
   ```
   The API will be available at `http://localhost:5000`
   Swagger documentation: `http://localhost:5000/swagger`

5. **Run Tests**
   ```bash
   cd Backend
   dotnet test Afama.Go.Api.sln --configuration Release --verbosity normal
   ```

## 🗄️ Database Management with Entity Framework

This project uses Entity Framework Core for database operations. Here's a comprehensive guide for managing database migrations:

### Prerequisites for EF Commands
Make sure you have EF Core tools installed globally:
```bash
dotnet tool install --global dotnet-ef
```

### Adding New Migrations

When you modify domain entities or add new ones, you need to create a migration to update the database schema:

#### Basic Migration Command
```bash
cd Backend
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Api.Host --output-dir Data/Migrations
```

#### Real-world Examples

1. **Adding Course and Club entities with relationships:**
   ```bash
   dotnet ef migrations add AddCourseClubAndCourseTeacher --project src/Infrastructure --startup-project src/Api.Host --output-dir Data/Migrations
   ```

2. **Adding Member management features:**
   ```bash
   dotnet ef migrations add AddMembershipAndGrading --project src/Infrastructure --startup-project src/Api.Host --output-dir Data/Migrations
   ```

3. **Adding event scheduling:**
   ```bash
   dotnet ef migrations add AddEventAndScheduling --project src/Infrastructure --startup-project src/Api.Host --output-dir Data/Migrations
   ```

#### Command Breakdown

- `dotnet ef migrations add <MigrationName>`: Creates a new migration
- `--project src/Infrastructure`: Specifies where the DbContext and migration files are located
- `--startup-project src/Api.Host`: Specifies the project containing the application configuration and connection strings
- `--output-dir Data/Migrations`: Places migration files in the `src/Infrastructure/Data/Migrations` directory

### Applying Migrations

#### Update Database to Latest Migration
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host
```

This command applies all pending migrations to bring your database schema up to date with your current model.

#### Update to Specific Migration
```bash
dotnet ef database update <MigrationName> --project src/Infrastructure --startup-project src/Api.Host
```

#### Apply Migrations in Production Environment
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host --configuration Release
```

#### Example Migration Execution Workflow
```bash
# Navigate to the Backend directory
cd Backend

# 1. Add a new migration for course management
dotnet ef migrations add AddCourseClubAndCourseTeacher --project src/Infrastructure --startup-project src/Api.Host --output-dir Data/Migrations

# 2. Review the generated migration files
# Check the files in src/Infrastructure/Data/Migrations/

# 3. Apply the migration to update the database
dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host

# 4. Verify the application starts successfully
cd src/Api.Host
ASPNETCORE_ENVIRONMENT=Development dotnet run --urls "http://localhost:5000"
```

### Migration Management Commands

#### View Migration History
```bash
dotnet ef migrations list --project src/Infrastructure --startup-project src/Api.Host
```

#### Remove Last Migration (if not applied to database)
```bash
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api.Host
```

#### Generate SQL Script for Migration
```bash
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api.Host --output migration.sql
```

#### Generate SQL Script for Specific Migration Range
```bash
dotnet ef migrations script <FromMigration> <ToMigration> --project src/Infrastructure --startup-project src/Api.Host --output update.sql
```

### Best Practices for Migrations

1. **Naming Conventions**: Use descriptive names that clearly indicate what the migration does:
   - ✅ `AddCourseClubAndCourseTeacher`
   - ✅ `UpdateMembershipStatusEnum`
   - ❌ `Update1` or `FixStuff`

2. **Review Generated Migrations**: Always review the generated migration code before applying:
   ```bash
   # Check the generated files in src/Infrastructure/Data/Migrations/
   ```

3. **Test Migrations**: Test migrations on a copy of production data:
   ```bash
   # Create a backup first, then test the migration
   dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host
   ```

4. **Environment-Specific Migrations**: For production deployments:
   ```bash
   # Generate SQL script for production deployment
   dotnet ef migrations script --project src/Infrastructure --startup-project src/Api.Host --configuration Release --output production-migration.sql
   ```

### Database Configuration by Environment

- **Development**: Uses local SQL Server instance (LocalDB or SQL Server Express)
- **Testing**: Uses InMemory database for isolated test execution
- **Production**: Uses Azure SQL Database

**Local SQL Server Configuration:**
The application is configured to use a local SQL Server instance in Development environment. The connection string is typically configured in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AfamaGoDB;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```

**For Testing:**
Tests use InMemory database to ensure isolation and fast execution without requiring a real database connection.

### Azure Deployment

This project uses Azure Developer CLI for streamlined deployment:

1. **Initialize Azure resources**
   ```bash
   azd auth login
   azd init
   ```

2. **Deploy to Azure**
   ```bash
   azd up
   ```

## 🔧 Technology Stack

### Backend Technologies
- **.NET 9**: Latest version of .NET
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for database operations
- **Azure SQL Database**: Cloud database solution
- **Azure App Service**: Hosting platform
- **Azure Key Vault**: Secrets management
- **Azure Application Insights**: Application monitoring
- **Serilog**: Structured logging
- **MediatR**: CQRS pattern implementation
- **FluentValidation**: Input validation
- **Mapster**: Object mapping

### Frontend Technologies (Planned)
- **Angular 17+**: Modern web framework
- **TypeScript**: Type-safe JavaScript
- **Angular Material**: UI component library
- **RxJS**: Reactive programming
- **NgRx**: State management (when needed)

### Infrastructure & DevOps
- **Azure Bicep**: Infrastructure as Code
- **GitHub Actions**: CI/CD pipeline
- **Azure Developer CLI**: Deployment automation

## 🧪 Testing Strategy

The project includes comprehensive testing at multiple levels:

- **Unit Tests**: Domain and application logic testing
- **Integration Tests**: Database and external service integration
- **Functional Tests**: End-to-end API testing

Run tests using:
```bash
cd Backend
dotnet test Afama.Go.Api.sln --configuration Release --verbosity normal
```

## 📊 Monitoring & Observability

- **Application Insights**: Application performance monitoring
- **Structured Logging**: Comprehensive logging with Serilog
- **Health Checks**: Application health monitoring
- **Metrics Collection**: Custom metrics and KPIs

## 🔐 Security Features

- **Azure AD B2C**: Identity and access management (planned)
- **JWT Authentication**: Secure API access (planned)
- **HTTPS Enforcement**: Encrypted communication
- **Input Validation**: Comprehensive data validation
- **CORS Configuration**: Cross-origin request security

## 🚀 Deployment Pipeline

The project uses GitHub Actions for automated CI/CD:

1. **Continuous Integration**
   - Code quality checks
   - Automated testing
   - Security scanning
   - Build verification

2. **Continuous Deployment**
   - Infrastructure provisioning
   - Application deployment
   - Database migrations
   - Post-deployment testing

## 📝 API Documentation

The API is fully documented using OpenAPI/Swagger. When running locally, visit:
- Development: `http://localhost:5000/swagger`
- Production: `https://your-app-name.azurewebsites.net/swagger`

## 🤝 Contributing

This is a personal learning project, but suggestions and feedback are welcome! Please feel free to:
- Open issues for bugs or feature requests
- Submit pull requests for improvements
- Share your thoughts on the architecture

## 📚 Learning Objectives

This project serves as a practical exploration of:
- ✅ Clean Architecture in .NET
- ✅ Azure cloud services integration
- ✅ Infrastructure as Code with Bicep
- ✅ Modern CI/CD practices
- ✅ Entity Framework Core and migrations
- 🔄 Authentication and authorization patterns (planned)
- 🔄 Microservices architecture (future)
- 🔄 Event-driven architecture (future)
- 🔄 Advanced Azure services (Functions, Service Bus, etc.)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Ki-Do-Kaï club and the AFAMA federation community for inspiration
- Tai-Jitsu KJR practitioners for their dedication to the art
- Microsoft Azure documentation and samples
- .NET community for best practices and patterns
- Open source contributors for the amazing tools and libraries

---

**Note**: This project is developed for educational purposes and personal use in managing the Ki-Do-Kaï club (Tai-Jitsu KJR style, AFAMA federation Belgium). It showcases modern Azure development practices and serves as a learning platform for cloud-native applications.