using Microsoft.Extensions.Logging;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    public class ReqResLoggingSettings
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}