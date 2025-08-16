# AfamaGo - Ki-Do-Kaï Club Management System

[![Build Status](https://github.com/fdivrusa/Afama.Go.Api/workflows/ci/badge.svg)](https://github.com/fdivrusa/Afama.Go.Api/actions)
[![Azure Dev](https://img.shields.io/badge/Azure-Ready-blue)](https://azure.microsoft.com)

AfamaGo is a modern club management system designed specifically for the Ki-Do-Kaï club, practicing Tai-Jitsu KJR style within the AFAMA federation in Belgium. This project serves as both a practical solution for club administration and a learning platform to explore and implement various Azure services and modern web development practices.

## 🥋 Project Purpose

This application is designed to manage various aspects of the Ki-Do-Kaï club practicing Tai-Jitsu KJR, including:
- Member registration and management
- Training session scheduling
- Event organization
- Communication tools
- Administrative functions

**Note**: This is a personal learning project focused on exploring Azure technologies rather than a commercial venture.

## 🏗️ Architecture Overview

AfamaGo follows a modern, cloud-native architecture with clear separation of concerns:

### Backend (.NET 8)
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
├── Backend/                    # .NET 8 Backend
│   ├── src/
│   │   ├── Api.Host/          # Web API entry point
│   │   ├── Application/       # Application logic & use cases
│   │   ├── Domain/           # Domain entities & business rules
│   │   ├── Infrastructure/   # External concerns (DB, logging, etc.)
│   │   └── Web/             # Web layer (if needed)
│   ├── tests/               # Test projects
│   ├── infra/              # Azure Bicep templates
│   └── azure.yaml          # Azure Developer CLI configuration
├── Frontend/               # Angular Frontend
│   ├── bff/               # Backend-for-Frontend
│   └── client/            # Angular SPA
└── README.md
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/fdivrusa/Afama.Go.Api.git
   cd AfamaGo
   ```

2. **Backend Setup**
   ```bash
   cd Backend
   dotnet restore
   dotnet build
   ```

3. **Database Setup**
   ```bash
   # Update connection string in appsettings.Development.json
   dotnet ef database update --project src/Infrastructure --startup-project src/Api.Host
   ```

4. **Run the Backend API**
   ```bash
   cd src/Api.Host
   dotnet run
   ```
   The API will be available at `https://localhost:5001`

5. **Frontend Setup** (when available)
   ```bash
   cd Frontend/client
   npm install
   npm start
   ```

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
- **.NET 8**: Latest LTS version of .NET
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

### Frontend Technologies
- **Angular 17+**: Modern web framework
- **TypeScript**: Type-safe JavaScript
- **Angular Material**: UI component library
- **RxJS**: Reactive programming
- **NgRx**: State management (when needed)

### Infrastructure & DevOps
- **Azure Bicep**: Infrastructure as Code
- **GitHub Actions**: CI/CD pipeline
- **Azure Developer CLI**: Deployment automation
- **Azure Container Registry**: Container image storage (if needed)

## 🧪 Testing Strategy

The project includes comprehensive testing at multiple levels:

- **Unit Tests**: Domain and application logic testing
- **Integration Tests**: Database and external service integration
- **Functional Tests**: End-to-end API testing
- **Load Tests**: Performance and scalability validation

Run tests using:
```bash
dotnet test
```

## 📊 Monitoring & Observability

- **Application Insights**: Application performance monitoring
- **Structured Logging**: Comprehensive logging with Serilog
- **Health Checks**: Application health monitoring
- **Metrics Collection**: Custom metrics and KPIs

## 🔐 Security Features

- **Azure AD B2C**: Identity and access management
- **JWT Authentication**: Secure API access
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
- Development: `https://localhost:5001/swagger`
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
- ✅ Authentication and authorization patterns
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