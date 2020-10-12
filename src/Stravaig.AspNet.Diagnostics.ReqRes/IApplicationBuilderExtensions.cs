using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    // ReSharper disable once InconsistentNaming
    // named after the interface it extends, but is a class.
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseDiagnostics(this IApplicationBuilder builder)
        {
            var settings = builder.ApplicationServices.GetService<ReqResLoggingSettings>();
            ThrowOnMissingSettings(settings);
            if (settings.Enabled)
                builder.UseMiddleware<ReqResDiagnosticMiddleware>();
    
            return builder;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void ThrowOnMissingSettings(ReqResLoggingSettings settings)
        {
            if (settings == null)
                throw new InvalidOperationException(
                    $"The service provider does not have the Stravaig.AspNet.Diagnostics.ReqRes settings configured. Please call {nameof(IServiceCollectionExtensions.AddStravaigReqResLogging)} when setting up your {nameof(IServiceCollection)}." +
                    Environment.NewLine +
                    $"e.g. " +
                    Environment.NewLine +
                    "public class Startup" +
                    Environment.NewLine +
                    "{" +
                    Environment.NewLine +
                    "  public void ConfigureServices(IServiceCollection services)" +
                    Environment.NewLine +
                    "  {" +
                    Environment.NewLine +
                    "    services.AddStravaigReqResLogging(/* Configuration */);" +
                    Environment.NewLine +
                    "    // ... Other services to be added here." +
                    Environment.NewLine +
                    "  }" +
                    Environment.NewLine +
                    "  // ... Other parts of the Setup class here" +
                    Environment.NewLine +
                    "}");
        }
    }
}