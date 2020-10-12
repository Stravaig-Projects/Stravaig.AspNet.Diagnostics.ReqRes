using Microsoft.Extensions.Logging;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    public class ReqResLoggingSettings
    {
        public bool Enabled { get; set; } = true;
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}