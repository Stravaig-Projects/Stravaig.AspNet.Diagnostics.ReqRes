# Stravaig.AspNet.Diagnostics.ReqRes

A piece of middleware for ASP.NET Core applications that logs the request / response.

* ![Build Stravaig.AspNet.Diagnostics.ReqRes](https://github.com/Stravaig-Projects/Stravaig.AspNet.Diagnostics.ReqRes/workflows/Build%20Stravaig.AspNet.Diagnostics.ReqRes/badge.svg)

**CAUTION: This package should be used sparingly. It will slow requests, and may expose PII (Personally Identifiable Information) or secrets in logs which may be a violation of GDPR.**

## Usage

First, you need to add the services required for this middleware to run. There are three options.

Option 1, sets up the default. So requests and responses are logged at the information level.
```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddStravaigReqResLogging();
  // Add services required by other components of your app
}
```

Option 2, allows you to pass in a settings object, which permits you to define which logging level you want to use:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  var settings = new ReqResLoggingSettings{ LogLevel = LogLevel.Debug };
  services.AddStravaigReqResLogging(settings);
  // Add services required by other components of your app
}
```

Option 3, allows you to take the configuration from the system's configuration (e.g. appsettings.json, etc.). This expects the configuration to be located at `Stravaig:ReqResLogging`. If it is not found then the default settings will be used:

```csharp
public class Startup
{
  public Startup(IConfiguration configuration)
  {
    // Allow the framework to inject an IConfiguration object
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    // Add the Req/Res logging library using the IConfiguration object
    services.AddStravaigReqResLogging(Configuration);
    // Add services required by other components of your app
  }
```

Finally, Add the middleware to your application's pipeline in the `Configure` method. It should go near the start so that it can pick up the request as it comes in (just in case any other middleware modifies it) and so that the all the other middleware can add to the response for those additions to be logged too.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseRequestResponseDiagnostics();
    // Add other things to your app pipeline.
}
```

## Settings

In the configuration the settings are at:
```
{
  "Stravaig" :{
    "ReqResLogging":{
      "Enabled": true,
      "LogLevel": "Information"
    }
  }
}
```

Or, you can configure it directly with the `ReqResLoggingSettings` class.

### Enabled

Default: true

When true, this will ensure that the middleware is added to the pipeline when `app.UseRequestResponseDiagnostics()` is called, otherwise the middleware won't be added. This allows the call to stay in the application and the pipeline based on a configuration setting.

### LogLevel

Default: Information

This maps to the [Microsoft.Extensions.Logging.LogLevel enum](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1).
It defines the log level the request/response logs are to be logged at.

