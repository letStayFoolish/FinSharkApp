# ASP.NET Core fundamentals
_Source: [https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-9.0&tabs=macos](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-9.0&tabs=macos)_

## Overview

### `Program.cs`

ASP.NET Core apps created with the web templates contain the application startup code in `Program.cs` file.
The `Program.cs` file is where:
- **Services** required by the app are **configured**;
- THe app's **request handling pipeline** is **defined** as a series of middleware components.

### Dependency Injection (Services)

When an ASP.NET Core app receives an HTTP request, the code handling the request sometimes needs to access other services.

ASP.NET Core features built-in **dependency injection (DI)** that make configured services available throughout an app. Services are added to the DI Container with _WebApplicationBuilder.Services_ (`builder.Services`) in the code. When the **WebApplicationBuilder** is instantiated, many framework-provided services are automatically added. `builder` is a `WebApplicationBuilder` in the following code:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddControllersWithView();
// PersonService instance is registered as implementation of the interface IPersonService.
// The deligate signature now expects an IPersonService parameter instead of PersonService parameter.
// When the app runs and a client requests the root URL, the service container provides an instance of the PersonService class because it is registered as the implementation of the IPersonService interface.
builder.Services.AddSingleton<IPersonService, PersonService>();
// So, when comoponent asks for IPersonService, provide an instance of class PersonService.
var app = builder.Build();
```
#### Tip:
Think of `IPersonService` as a contract. It defines the methods and properties that an implementation must have. The delegate wants an instance of `IPersonService`. It doesn't care at all about the underlying implementation, only that the instance has the methods and properties defined in the contract.

In the previous code, `CreateBuilder` adds configuration, logging, and many other services to the DI container. The DI framework provides an instance of a requested service at run time.

In the dependency injection pattern, component recives its dependencies from external source rather than creating them itself. This pattern decouples the code from the dependency, which makes code easier to test and maintain.

Another way to resolve service from DI is by using constructor injection.

```csharp
// using...
namespace api.Repository;

public class StockRepository : IStockRepository
{
  private readonly ApplicationDbContext _context;

  public StockRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<List<Stock>> GetAllAsync(QueryObject query)
  {
    var stocks = _context.Stocks.Include(item => item.Comments).ThenInclude(c => c.AppUser).AsQueryable();
    // Validation
    var skipNumber = (query.PageNumber - 1) * query.PageSize;
    return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
  }
}
```
In this code, the `StockRepository` constructor takes parameter `context`, which is resolved at the run time into the `_context` variable. An context object is used to get the stock list from db in the `GetAllAsync`.

## Services Lifetime
When you register a service, you have to choose a lifetime that matches how the service is used in the app. The **lifetime** affects how the service behaves when it is injected to component(s).

### Singleton Lifetime

Services registered with **singleton lifetime** are created once when the app starts and are reused for a lifetime of the app. This lifetime is useful for a service that is **expensive to create** or that **don't change often**.
For example, the service that reads configuration settings from a file can be registered as **singleton**.

**Every request, from every client, uses the same instance of the service.**


Use the `AddSingleton` method to add a singleton service to the service container.

### Scoped Lifetime

Services registered with **scoped lifetime** are created once per configured scope, which ASP.NET Core sets up for each request. A scoped service in ASP.NET Core is typically created when a request is received and disposed of when the request is completed. Useful when, for example, a service fetches customer's data from a database.

Use the `AddScoped` method to add a scoped service to the service container.

### Transient Lifetime

Services registered with **transient lifetime** are created each time they are requested. This lifetime is useful for lightweight, stateless services. For example, a service that performs a specialized calculation can be registered as a transient service.

**Every time the service is injected into a component, a new instance of the service is created.**

Use the `AddTransient` method to add a transient service to the service container.

### Middleware

The **request handling pipeline** is composed as a **series of middleware components**. Each component performs operations on a **HttpContext** and either invokes next middleware in the pipeline or terminates the request.

By convention, a middleware component is added to the pipeline by invoking a `Use{Feature}` extension method.

```csharp
/// rest of the code...

app.UseHttpsRedirection();
app.UseAntiforgery();

app.Run();
```
### Host

On a startup, an ASP.NET Core app builds a _host_. The host encapsulates all of the app's resources, such as:
- An HTTP server implementation,
- Middleware components,
- Logging,
- Dependency injection (DI) services,
- Configuration

The ASP.NET Core **WebApplication** and **WebApplicationBuilder** types are recomended and used in all ASP.NET Core templates.

The `WebApplicationBuilder.Build` method configures a host with a set of default options, such as:
- Use **Kastrel** as the web server and enables IIS integration.
- Load **configuration** from `appsettings.json`, environment variables, command line arguments, and other configuration sources.
- Send logging output to the console and debug providers.

### Servers

An ASP.NET Core app uses HTTP server implementation to listen for HTTP requests. The server surfaces reqeusts to the app as a set of request features composed into a **HttpContext**.

### Configuration

ASP.NET Core provides a configuration framework that gets settings as name-value pairs from an ordered set of configuration providers. By the default ASP.NET Core apps are configured to read from `appsettings.json`, environment variables, command line, and more. When the app's configuration is loaded, values from environment variables override values from `appsettings.json`.

### Environments

Execution environments, such as `Development`, `Stagging`, and `Production`, are available in ASP.NET Core. Specify the environment an app is running in by setting the `ASPNETCORE_ENVIRONEMNT` environment variable. ASP.NET Core reads that environment variable at app startup and stores the value in an `IWebHostEnvironment` implementation. This implementation is available anywhere across the application via dependency injection (DI).
```csharp
if(!app.Environement.IsDevelopment()) 
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}
```
The following example configures the exception handler and HTTP Strict Transport Security Protocol (HSTS) middleware when **not** running in `development` environment.

### Logging
### Routing

Routing in ASP.NET Core is a mechanism that maps incoming requests to specific endpoints in an application. It enables you to define URL patterns that correspond to different components, such as Blazor components, Razor pages, MVC controller actions, or middleware.

The `UseRouting(IApplicationBuilder)` method adds routing middleware to the request pipeline. This middleware processes the routing information and determines the appropriate endpoint for each request. You don't have to explicitly call `UseRouting` unless you want to change the order in which middleware is processed.

### Error Handling
### Make HTTP Requests

An implementation of `IHttpClientFactory` is available for creating `HttpClient` instances.

