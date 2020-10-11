using System.Collections.Generic;
using System.Threading.Tasks;
using Example;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using Stravaig.Extensions.Logging.Diagnostics;

namespace Stravaig.AspNet.Diagnostics.ReqRes.Tests
{
    public class LogLevelsTests
    {
        private WebApplicationFactory<Startup> _factory;
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [Test]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.Warning)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Critical)]
        public async Task RequestAndResponseRecorded(LogLevel level)
        {
            var logProvider = new TestCaptureLoggerProvider();
            using var wapFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("Logging:LogLevel:Stravaig.AspNet.Diagnostics.ReqRes.ReqResDiagnosticMiddleware", nameof(LogLevel.Trace)),
                    });
                });
                builder.ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.AddProvider(logProvider);
                });
                builder.ConfigureServices(services =>
                {
                    var settings = new ReqResLoggingSettings
                    {
                        LogLevel = level,
                    };
                    services.AddStravaigReqResLogging(settings);
                });
            });
            using var client = wapFactory.CreateDefaultClient();

            await client.GetAsync("/");

            var entries = logProvider.GetLogEntriesFor<ReqResDiagnosticMiddleware>();

            entries.Count.ShouldBe(2);
            entries[0].FormattedMessage.ShouldContain("Request");
            entries[0].LogLevel.ShouldBe(level);
            entries[1].FormattedMessage.ShouldContain("Response");
            entries[1].LogLevel.ShouldBe(level);
        }
        
    }
}