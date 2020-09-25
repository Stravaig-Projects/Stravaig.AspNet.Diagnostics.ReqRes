using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Stravaig.AspNet.Diagnostics.ReqRes
{
    public class ReqResDiagnosticMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ReqResDiagnosticMiddleware> _logger;

        public ReqResDiagnosticMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ReqResDiagnosticMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            StringBuilder requestLog = new StringBuilder();
            
            var req = context.Request;
            requestLog.AppendLine($"{req.Method} {req.GetDisplayUrl()}");
            foreach (var header in req.Headers)
            {
                requestLog.AppendLine($"{header.Key}: {string.Join(";", header.Value.Cast<string>())}");
            }

            var requestReader = new StreamReader(context.Request.Body);
            var content = await requestReader.ReadToEndAsync();
            if (!string.IsNullOrEmpty(content))
            {
                requestLog.AppendLine();
                requestLog.AppendLine(content);
            }
            context.Request.Body.Position = 0;
            _logger.LogInformation(requestLog.ToString());
            await _next(context);
        }
    }
}