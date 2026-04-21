# Spawtz Tech Screen

A multi-tenant ASP.NET Core web application for managing sports competition results. Each tenant represents an independent sports organisation with its own database, branding, and configurable behaviour — all running on shared infrastructure.

**Current tenants:** Kent 5-a-Side, Play Rounders, Simple Cricket

## Tech Stack & Local Setup

### Prerequisites

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download/dotnet/9.0 |
| SQL Server LocalDB | Included with Visual Studio, or install standalone | https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb |
| Visual Studio 2022 (recommended) | 17.8+ | https://visualstudio.microsoft.com/ |
| Git | Any recent version | https://git-scm.com/ |

### Frameworks & Libraries

- **ASP.NET Core 9.0** — Web framework (Razor views, MVC controllers)
- **Entity Framework Core 8.0** — ORM with SQL Server provider
- **Dapper** — Lightweight SQL queries alongside EF Core
- **Bootstrap 5** — CSS framework (bundled in `wwwroot/lib/`)
- **jQuery** — JavaScript library (bundled in `wwwroot/lib/`)
- **htmx 2.0** — HTML-driven AJAX interactions (loaded via CDN)

### Running Locally

1. Clone the repository and open `TechScreen.sln` in Visual Studio (or your editor of choice).

2. The app uses SQL Server LocalDB. No manual database setup is needed — the app automatically creates and migrates a database per tenant on startup, and seeds fixture data if the database is empty.

3. Run the web project:
   ```
   cd TechScreen.Web
   dotnet run
   ```
   The app starts on `http://localhost:5185` by default.

4. **Subdomain routing:** Each tenant is accessed via subdomain. For local development you need to access tenant sites at:
   - `http://kent5aside.localhost:5185/Competitions`
   - `http://playrounders.localhost:5185/Competitions`
   - `http://simplecricket.localhost:5185/Competitions`

   Modern browsers resolve `*.localhost` to `127.0.0.1` automatically. If yours doesn't, add entries to your hosts file.

5. The root site at `http://localhost:5185` lists all available tenants.

### EF Core Migrations

A design-time factory (`DesignTimeDbContextFactory`) is included so EF tooling works without a running tenant context:

```
cd TechScreen.Web
dotnet ef migrations add <MigrationName>
```

## Architecture Overview

### Multi-Tenancy

Tenants are defined in `appsettings.json` under `Multitenancy:Tenants`. Each tenant has an ID, display name, database name, and theme colours. A middleware (`TenantResolutionMiddleware`) extracts the tenant from the request hostname subdomain and populates a request-scoped `TenantContext`, which is available via dependency injection throughout the request.

Each tenant gets its own SQL Server database, created from a connection string template (`TechScreen_{DatabaseName}`). All tenants share the same EF Core schema and migrations.

### Solution Structure

```
TechScreen.sln
  TechScreen.Abstractions/     Shared interfaces — the plugin API contract
  TechScreen.Web/              Main ASP.NET Core application
  TechScreen.Kent5aside/       Tenant plugin: Kent 5-a-Side
  TechScreen.PlayRounders/     Tenant plugin: Play Rounders
  TechScreen.SimpleCricket/    Tenant plugin: Simple Cricket
```

### Plugin API

The plugin system allows each tenant to override default service implementations without modifying the core application. Plugins are discovered automatically at startup by scanning for assemblies matching `TechScreen.Tenant*.dll`.

#### Creating a Tenant Plugin

1. Create a new class library project. Set the assembly name to `TechScreen.Tenant.<YourTenant>`:

   ```xml
   <PropertyGroup>
     <AssemblyName>TechScreen.Tenant.MyLeague</AssemblyName>
   </PropertyGroup>
   ```

2. Reference `TechScreen.Abstractions` and implement `ITenantPlugin`:

   ```csharp
   using TechScreen.Abstractions;

   public class MyLeaguePlugin : ITenantPlugin
   {
       public string TenantId => "myleague";

       public void ConfigureServices(ITenantServiceBuilder services)
       {
           services.AddScoped<IFooterService, MyLeagueFooterService>();
           services.AddScoped<IBrandingService, MyLeagueBrandingService>();
       }
   }
   ```

3. Add the tenant to `appsettings.json` under `Multitenancy:Tenants` with an ID, name, database, and theme.

4. Add a project reference from `TechScreen.Web` to your plugin project.

#### Overridable Services

These interfaces are registered with defaults and can be overridden per-tenant via `ITenantServiceBuilder.AddScoped`:

| Interface | Purpose | Default Behaviour |
|-----------|---------|-------------------|
| `IPointsConfiguration` | Win/draw/loss point values and standings sort order | Win=3, Draw=1, Loss=0. Ordered by points then goal difference. |
| `IBrandingService` | Navbar logo (`IHtmlContent`) | Renders the TechScreen.svg logo |
| `IFooterService` | Footer text | Static "TechScreen" text |

If a tenant does not override a service, the default implementation is used. The resolution happens per-request based on the current `TenantContext` — no tenant-specific wiring is needed in the main application.

#### How Plugin Discovery Works

`TenantPluginLoader.DiscoverPlugins()` scans the output directory for assemblies matching `TechScreen.Tenant*.dll`, loads them, and finds all classes implementing `ITenantPlugin`. Each plugin's `ConfigureServices` method is called during startup, registering tenant-specific service overrides into a `TenantServiceRegistry`. At request time, the DI container checks the registry for a tenant override before falling back to the default.
