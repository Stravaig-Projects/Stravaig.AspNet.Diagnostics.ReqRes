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
    // ReSharper disable once ClassNeverInstantiated.Global
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
            await LogRequest(context);
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                await LogResponse(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            var res = context.Response;
            StringBuilder requestLog = new StringBuilder("Response:"+Environment.NewLine);
            foreach (var header in res.Headers)
            {
                requestLog.AppendLine($"{header.Key}: {string.Join(";", header.Value.Cast<string>())}");
            }

            res.Body.Seek(0, SeekOrigin.Begin);
            var responseReader = new StreamReader(res.Body);
            var content = await responseReader.ReadToEndAsync();
            if (!string.IsNullOrEmpty(content))
            {
                requestLog.AppendLine();
                requestLog.AppendLine(content);
            }

            _logger.LogInformation(requestLog.ToString());
        }

        private async Task LogRequest(HttpContext context)
        {
            var req = context.Request;
            req.EnableBuffering();
            StringBuilder requestLog = new StringBuilder("Request:" + Environment.NewLine);
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
        }
    }
}