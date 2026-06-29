# Template.WebApi.Clean

Plantilla de **ASP.NET Core Web API** con **Clean Architecture**, **CQRS** (Cortex.Mediator), **API Versioning**, **Scalar UI**, **EF Core + Dapper**, **Repository + UnitOfWork** y **Distributed Caching**.

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

| Parámetro      | Tipo   | Default     | Descripción                         |
|----------------|--------|-------------|-------------------------------------|
| `-n`           | string | requerido   | Nombre del proyecto                 |
| `--framework`  | choice | `net10.0`   | Versión de .NET (solo net10.0)      |
| `--no-restore` | bool   | false       | Omite la restauración de paquetes   |

## Estructura

```
MiApi/
├── MiApi/                                    ← Host (ASP.NET Core)
│   ├── Cache/
│   │   ├── CachedAttribute.cs                ← Filtro cache para GET endpoints
│   │   └── CacheTimeHelper.cs                ← Constantes de duración
│   ├── Controllers/
│   │   └── V1/
│   │       └── SamplesController.cs          ← Endpoints CQRS de ejemplo con versionado
│   ├── Installers/
│   │   ├── Contracts/                        ← Interfaces IInstaller
│   │   ├── Extensions/                       ← Scanning de installers
│   │   ├── AutoMapperInstaller.cs
│   │   ├── DbInstaller.cs                    ← Dapper + EF Core (DbContextFactory + UnitOfWork)
│   │   ├── MediatorRegisterInstaller.cs
│   │   ├── OptionsInstaller.cs
│   │   └── ServicesInstaller.cs              ← Auto-registro + manual (domain services)
│   ├── Routes/ApiRoutes.cs
│   ├── Program.cs
│   └── appsettings.json
├── MiApi.Application/                        ← Capa de aplicación
│   ├── AutoMapper/
│   ├── Configurations/
│   ├── Handlers/
│   │   └── Samples/
│   │       ├── Commands/
│   │       │   ├── CreateSampleCommand.cs
│   │       │   └── CreateSampleCommandHandler.cs
│   │       └── Queries/
│   │           ├── GetAllSamplesQuery.cs       ← Ejemplo GET all con CQRS
│   │           ├── GetAllSamplesQueryHandler.cs
│   │           ├── GetSampleQuery.cs
│   │           └── GetSampleQueryHandler.cs
│   └── Models/
├── MiApi.Domain/                             ← Capa de dominio
│   ├── DataBase/DapperContext.cs
│   ├── Models/SampleEntity.cs
│   └── Services/
│       ├── IDateTimeService.cs               ← Abstracción singleton
│       ├── IResponseCacheService.cs          ← Abstracción de cache distribuido
│       └── ISampleRepository.cs              ← Abstracción scoped
├── MiApi.Infrastructure/                     ← Capa de infraestructura
│   ├── Data/
│   │   ├── ApplicationDbContext.cs           ← DbContext con Fluent API
│   │   ├── Models/
│   │   │   ├── EntityBase.cs                 ← Clases base (EntityBase, EntityBaseWithId, AuditEntityBase)
│   │   │   ├── PagedResultBase.cs
│   │   │   ├── PagedResult.cs
│   │   │   └── SampleEfEntity.cs             ← Entidad EF Core de ejemplo
│   │   ├── Contracts/
│   │   │   ├── IRepositoryCommand.cs         ← C (Create, Update, Delete)
│   │   │   ├── IRepositoryQuery.cs           ← R (Get, GetAll, GetPaged, Query)
│   │   │   └── IUnitOfWork.cs                ← Transacciones + repositorios
│   │   ├── Extensions/
│   │   │   ├── ExpressionBuilder.cs          ← Composición de expresiones And/Or
│   │   │   └── QueryableExtensions.cs        ← WithTracking helper
│   │   ├── RepositoryCommand.cs              ← Implementación EF Core genérica
│   │   ├── RepositoryQuery.cs                ← Implementación EF Core genérica
│   │   └── UnitOfWork.cs                     ← IDbContextFactory + ExecutionStrategy
│   ├── Exceptions/                           ← Jerarquía de 7 excepciones
│   ├── Extensions/
│   │   ├── ApplicationBuilderMiddlewareExtensions.cs
│   │   └── ServiceCollectionExtensions.cs    ← AddRegisterService para auto-registro
│   ├── Middlewares/ErrorHandlingMiddleware.cs ← ProblemDetails (RFC 7807)
│   └── Services/
│       ├── DateTimeService.cs                ← Implementación de IDateTimeService
│       ├── ResponseCacheService.cs           ← Implementación con IDistributedCache
│       ├── SampleRepository.cs               ← Implementación in-memory de ISampleRepository
│       ├── IServiceScoped.cs                 ← Marcadores para auto-registro
│       ├── IServiceSingleton.cs
│       └── IServiceTransient.cs
└── MiApi.sln
```

## Stack

| Tecnología                    | Propósito                              |
|-------------------------------|----------------------------------------|
| .NET 10                       | Runtime                                |
| ASP.NET Core                  | Web API                                |
| Asp.Versioning                | API versioning                         |
| Cortex.Mediator               | CQRS (Commands/Queries)                |
| Mapster                       | Object mapping                         |
| Entity Framework Core         | ORM (Repository + UnitOfWork pattern)  |
| Dapper                        | Data access liviano                    |
| Scalar.AspNetCore             | API documentation (UI)                 |
| StackExchange.Redis           | Caching                                |
| FluentValidation              | Validation (via Mediator)              |

## Arquitectura

### Clean Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        Host (API)                            │
│  Controllers → Installers → Middlewares → Routes → Program   │
├─────────────────────────────────────────────────────────────┤
│                      Application                              │
│  Handlers (CQRS) → Models → AutoMapper → Configurations      │
├─────────────────────────────────────────────────────────────┤
│                       Domain                                  │
│  Models → Services (abstracciones) → DapperContext            │
├─────────────────────────────────────────────────────────────┤
│                    Infrastructure                             │
│  Data (EF Core Repo/UoW) → Exceptions → Middlewares → DI     │
│  Services (implementaciones) → Extensions                     │
└─────────────────────────────────────────────────────────────┘
```

Las dependencias fluyen hacia adentro:
- **Host** → Application, Domain, Infrastructure
- **Application** → Domain
- **Infrastructure** → Domain
- **Domain** → sin dependencias

### CQRS con Cortex.Mediator

La plantilla incluye tres endpoints de ejemplo:

| Método | Ruta                          | Query/Command                  | Handler                          |
|--------|-------------------------------|--------------------------------|----------------------------------|
| GET    | `api/v1/samples`              | `GetAllSamplesQuery`           | `GetAllSamplesQueryHandler`      |
| GET    | `api/v1/samples/{id}`         | `GetSampleQuery`               | `GetSampleQueryHandler`          |
| POST   | `api/v1/samples`              | `CreateSampleCommand`          | `CreateSampleCommandHandler`     |

Los handlers inyectan servicios del dominio (`ISampleRepository`, `IDateTimeService`) y usan `ILogger<T>`.

### Inyección de dependencias

#### Auto-registro por marcadores

Las interfaces `IServiceScoped`, `IServiceSingleton` e `IServiceTransient` actúan como marcadores. Cualquier clase que implemente una interfaz de negocio que herede de estos marcadores se registra automáticamente:

```csharp
// Infrastructure/Services/ — el marcador
public interface IServiceScoped { }

// Domain/Services/ o Infrastructure/Services/ — interfaz de negocio
public interface IMiServicio : IServiceScoped { Task HacerAlgo(); }

// Infrastructure/Services/ — implementación
public class MiServicio : IMiServicio { public Task HacerAlgo() => Task.CompletedTask; }
```

#### Registro manual para abstracciones del dominio

Las interfaces definidas en Domain se registran manualmente en `ServicesInstaller`:

```csharp
services.AddSingleton<IDateTimeService, DateTimeService>();
services.AddScoped<ISampleRepository, SampleRepository>();
```

#### EF Core — UnitOfWork

El `UnitOfWork<ApplicationDbContext, SampleEfEntity>` se registra como scoped en `DbInstaller` usando `IDbContextFactory`:

```csharp
services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IUnitOfWork<ApplicationDbContext, SampleEfEntity>>(sp =>
{
    var factory = sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    return new UnitOfWork<ApplicationDbContext, SampleEfEntity>(factory, tracking: false);
});
```

### Repository + UnitOfWork

El patrón usa genéricos con tres tipos base:

| Clase base                     | Propósito                              |
|--------------------------------|----------------------------------------|
| `EntityBase`                   | Marcador vacío                         |
| `EntityBaseWithId`             | Agrega `Guid Id` (Key, Identity)       |
| `AuditEntityBase`              | Agrega CreatedAt/CreatedByName/UpdatedAt/UpdatedByName |

**Uso típico:**

```csharp
public class MiEntity : EntityBaseWithId
{
    public string Name { get; set; }
}

// En un handler o servicio:
public class MiHandler
{
    private readonly IUnitOfWork<ApplicationDbContext, MiEntity> uow;

    public async Task<List<MiEntity>> ObtenerTodos()
    {
        return (await uow.RepositoryQuery.GetAllAsync()).ToList();
    }

    public async Task Crear(MiEntity entity)
    {
        uow.RepositoryCommand.Create(entity);
        await uow.SaveChangesAsync();
    }
}
```

El `UnitOfWork` usa `IDbContextFactory` para crear un `DbContext` por unidad de trabajo y ejecuta `SaveChangesAsync` dentro de una transacción con `ExecutionStrategy` (resiliencia ante fallos transitorios).

### Distributed Caching

La plantilla incluye un sistema de cacheo distribuido basado en `IDistributedCache` listo para usar:

| Archivo | Capa | Rol |
|---|---|---|
| `RedisCacheSettings` | Application/Configurations | POCO con `Enabled` + `ConnectionString` |
| `IResponseCacheService` | Domain/Services | Interfaz: `CacheResponseAsync` / `GetCachedResponseAsync` |
| `ResponseCacheService` | Infrastructure/Services | Implementación con `IDistributedCache` + `System.Text.Json` |
| `CachedAttribute` | Host/Cache | Filtro `IAsyncActionFilter` para decorar endpoints |
| `CacheTimeHelper` | Host/Cache | Constantes de duración (ej. `SixHundredSeconds = 600`) |

**Uso:** decorar el action del controller:

```csharp
[HttpGet]
[Cached(CacheTimeHelper.SixHundredSeconds)]
public async Task<IActionResult> GetAllSamplesAsync() { ... }
```

**Flujo de ejecución:**

1. El filtro verifica que el método sea GET — si no, pasa directo.
2. Lee `RedisCacheSettings.Enabled` — si está deshabilitado, pasa directo.
3. Genera una clave única con el path + query params ordenados.
4. Si existe respuesta cacheada → devuelve `ContentResult` con `200 OK` directo.
5. Si no existe → ejecuta el action, y si el resultado es `OkObjectResult` lo guarda en cache.

**Configuración por defecto** (`appsettings.json`):

```json
{
  "RedisCacheSettings": {
    "Enabled": false,
    "ConnectionString": ""
  }
}
```

Con `Enabled: false` el filtro no cachea nada — ideal para development.

**Switchear a Redis:**

El template usa `AddDistributedMemoryCache()` por defecto (anda out-of-the-box). Para usar Redis real:

```csharp
// en ServicesInstaller.cs
// Reemplazar:
services.AddDistributedMemoryCache();
// Por:
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("RedisConnectionString");
});
```

Y agregar el paquete NuGet:

```bash
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

### Dos enfoques de acceso a datos

La plantilla incluye ambos para que puedas elegir:

| Enfoque       | Capa de abstracción             | Ideal para                          |
|---------------|---------------------------------|-------------------------------------|
| **Dapper**    | `IDbConnection` + SQL directo    | Consultas rápidas, reportes, SP     |
| **EF Core**   | `IRepositoryQuery/Command` + UoW | CRUD complejo, cambios tracking     |

### Instaladores automáticos

Los servicios se registran automáticamente escaneando el ensamblado en busca de clases que implementen `IInstallerServiceCollection` o `IInstallerApplicationBuilder`:

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

Todas las excepciones se capturan en `ErrorHandlingMiddleware` y se convierten a **ProblemDetails (RFC 7807)**:

| Excepción                          | HTTP Status Code |
|------------------------------------|------------------|
| `BadRequestProjectException`       | 400              |
| `UnauthorizedAccessProyectException` | 401            |
| `ForbiddenProjectException`        | 403              |
| `NotFoundProjectException`         | 404              |
| `TimeoutProjectException`          | 408              |
| `CustomExceptionProjectException`  | Custom (según StatusCode) |
| `ProjectException` (base)          | 500              |

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

### Agregar una nueva entidad EF Core

1. Crear la clase en `Domain/Models/` (o `Infrastructure/Data/Models/` si hereda de `EntityBaseWithId`)
2. Agregar `DbSet<T>` en `Infrastructure/Data/ApplicationDbContext.cs`
3. Configurar la entidad en `OnModelCreating` con Fluent API
4. Agregar `IUnitOfWork<ApplicationDbContext, TNuevaEntidad>` en el registro de DI si es necesario

## Licencia

MIT
