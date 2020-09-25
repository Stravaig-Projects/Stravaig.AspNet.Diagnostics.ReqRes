using Microsoft.AspNetCore.Builder;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    // ReSharper disable once InconsistentNaming
    // named after the interface it extends, but is a class.
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseDiagnostics(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ReqResDiagnosticMiddleware>();
            return builder;
        }
    }
}