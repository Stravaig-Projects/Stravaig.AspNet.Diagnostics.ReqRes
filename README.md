# Stravaig.AspNet.Diagnostics.ReqRes

A piece of middleware for ASP.NET Core applications that logs the request / response.

* ![Build Stravaig.AspNet.Diagnostics.ReqRes](https://github.com/Stravaig-Projects/Stravaig.AspNet.Diagnostics.ReqRes/workflows/Build%20Stravaig.AspNet.Diagnostics.ReqRes/badge.svg)

**CAUTION: This package should be used sparingly. It will slow requests, and may expose PII (Personally Identifiable Information) in logs which may be a violation of GDPR.**


## Usage

In your startup class add the middleware to your application's pipeline in the `Configure` method. It should go near the start so that it can pick up the request as it comes in (just in case any other middleware modifies it) and so that the all the other middleware can add to the response for those additions to be logged too.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseRequestResponseDiagnostics();
    // Add other things to your app pipeline.
}
```

