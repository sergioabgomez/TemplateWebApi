# Template.WebApi.Clean

Plantilla de **ASP.NET Core Web API** con **Clean Architecture**, **CQRS**, **API Versioning** y **Scalar UI**.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Instalación

```bash
dotnet new install TemplateWebApi
```

## Uso

```bash
dotnet new webapi-clean -n MiApi
cd MiApi
dotnet run
```

Esto crea un proyecto con el nombre `MiApi` — todos los namespaces, carpetas y referencias se actualizan automáticamente.

Abrir en el navegador: `https://localhost:7111/scalar/v1`

### Parámetros

| Parámetro    | Tipo   | Default  | Descripción                    |
|-------------|--------|----------|--------------------------------|
| `-n`        | string | requerido | Nombre del proyecto            |
| `--framework` | choice | `net10.0` | Versión de .NET (solo net10.0) |
| `--no-restore` | bool | false | Omite la restauración de paquetes |

## Estructura

```
MiApi/
├── MiApi/                              ← Host (ASP.NET Core)
│   ├── Controllers/                    ← Endpoints REST
│   ├── Installers/
│   │   ├── Contracts/                  ← Interfaces IInstaller
│   │   ├── Extensions/                 ← Scanning de installers
│   │   ├── AutoMapperInstaller.cs
│   │   ├── DbInstaller.cs
│   │   ├── MediatorRegisterInstaller.cs
│   │   ├── OptionsInstaller.cs
│   │   └── ServicesInstaller.cs
│   ├── Routes/ApiRoutes.cs
│   ├── Program.cs
│   └── appsettings.json
├── MiApi.Application/                  ← Capa de aplicación
│   ├── AutoMapper/
│   ├── Configurations/
│   ├── Handlers/                       ← CQRS Handlers
│   │   └── Samples/                    ← Sample GET + POST
│   └── Models/
├── MiApi.Domain/                       ← Capa de dominio
│   └── DataBase/
│       ├── DapperContext.cs
│       └── Entities/
├── MiApi.Infrastructure/               ← Capa de infraestructura
│   ├── Exceptions/                     ← Jerarquía de 7 excepciones
│   ├── Middlewares/                    ← Error handling middleware
│   ├── Extensions/                     ← DI extensions
│   ├── Models/                         ← Error models
│   ├── Services/                       ← Marker interfaces
│   └── Bootstrappers/
└── MiApi.sln
```

## Stack

| Tecnología           | Propósito                    |
|----------------------|------------------------------|
| .NET 10              | Runtime                      |
| ASP.NET Core         | Web API                      |
| Asp.Versioning       | API versioning               |
| Cortex.Mediator      | CQRS (Commands/Queries)      |
| Mapster              | Object mapping               |
| Dapper               | Data access                  |
| Scalar.AspNetCore    | API documentation (UI)       |
| StackExchange.Redis  | Caching                      |
| FluentValidation     | Validation (via Mediator)    |

## Arquitectura

### Clean Architecture

```
┌─────────────────────────────────────────────────┐
│                   Host (API)                     │
│  Controllers → Installers → Middlewares → Routes │
├─────────────────────────────────────────────────┤
│               Application                        │
│  Handlers (CQRS) → Models → AutoMapper → Config │
├─────────────────────────────────────────────────┤
│               Domain                             │
│  DbContext → DapperContext → Entities            │
├─────────────────────────────────────────────────┤
│             Infrastructure                       │
│  Exceptions → Middlewares → Services → DI        │
└─────────────────────────────────────────────────┘
```

### CQRS con Cortex.Mediator

La plantilla incluye dos endpoints de ejemplo:

- **GET** `api/v1/samples/{id}` → `GetSampleQuery` → `GetSampleQueryHandler`
- **POST** `api/v1/samples` → `CreateSampleCommand` → `CreateSampleCommandHandler`

### Instaladores automáticos

Los servicios se registran automáticamente escaneando el ensamblado en busca de clases que implementen `IInstallerServiceCollection`. Para agregar un nuevo installer:

```csharp
public class MiInstaller : IInstallerServiceCollection
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMiServicio();
    }
}
```

### Jerarquía de excepciones

| Excepción                     | HTTP Status Code |
|-------------------------------|------------------|
| `BadRequestProjectException`  | 400              |
| `UnauthorizedAccessProyectException` | 401      |
| `ForbiddenProjectException`   | 403              |
| `NotFoundProjectException`    | 404              |
| `TimeoutProjectException`     | 408              |
| `CustomExceptionProjectException` | Custom     |
| `ProjectException`            | 500              |

## Personalización

### Connection Strings

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "SqlConnectionString": "Server=.;Database=MiDb;Trusted_Connection=True;"
  }
}
```

### Scalar UI

La configuración de Scalar está en `Program.cs`:

```csharp
app.MapScalarApiReference(options =>
{
    options.WithTitle("Mi API");
    options.WithTheme(ScalarTheme.Purple);
});
```

## Licencia

MIT
