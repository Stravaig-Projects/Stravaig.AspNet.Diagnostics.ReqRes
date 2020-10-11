using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddStravaigReqResLogging(this IServiceCollection services, ReqResLoggingSettings settings)
        {
            services.AddSingleton(settings);
            return services;
        }

        public static IServiceCollection AddStravaigReqResLogging(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("Stravaig").GetSection("ReqResLogging");
            var settings = new ReqResLoggingSettings();
            section.Bind(settings);
            return AddStravaigReqResLogging(services, settings);
        }
        
        public static IServiceCollection AddStravaigReqResLogging(this IServiceCollection services)
        {
            var settings = new ReqResLoggingSettings();
            return AddStravaigReqResLogging(services, settings);
        }
    }
}