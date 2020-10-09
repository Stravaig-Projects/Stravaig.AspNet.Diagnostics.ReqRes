using System.Threading.Tasks;
using Example;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using Stravaig.Extensions.Logging.Diagnostics;

namespace Stravaig.AspNet.Diagnostics.ReqRes.Tests
{
    public class SimpleHappyPathTests
    {
        private WebApplicationFactory<Startup> _factory;
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [Test]
        public async Task RequestAndResponseRecorded()
        {
            var logProvider = new TestCaptureLoggerProvider();
            using var wapFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.AddProvider(logProvider);
                });
            });
            using var client = wapFactory.CreateDefaultClient();

            await client.GetAsync("/");

            var entries = logProvider.GetLogEntriesFor<ReqResDiagnosticMiddleware>();

            entries.Count.ShouldBe(2);
            entries[0].FormattedMessage.ShouldContain("Request");
            entries[1].FormattedMessage.ShouldContain("Response");
        }
    }
}