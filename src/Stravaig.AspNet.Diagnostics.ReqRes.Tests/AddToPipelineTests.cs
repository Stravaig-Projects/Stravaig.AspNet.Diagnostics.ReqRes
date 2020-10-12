using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.AspNet.Diagnostics.ReqRes.Tests
{
    public class AddToPipelineTests
    {
        [Test]
        public void ApplicationBuilderExtensionEnsuresSettingsAvailable()
        {
            // Arrange
            var emptyServiceProvider = new ServiceCollection().BuildServiceProvider();
            var appBuilder = new ApplicationBuilder(emptyServiceProvider);
            
            // Act & Assert
            var exception = Should.Throw<InvalidOperationException>(() => appBuilder.UseRequestResponseDiagnostics());
            Console.WriteLine(exception);
            exception.Message.ShouldStartWith("The service provider does not have the Stravaig.AspNet.Diagnostics.ReqRes settings configured.");
        }

        [Test]
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void AddsToPipelineOnlyWhenEnabled(bool isEnabled, int timesUseCalled)
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStravaigReqResLogging(new ReqResLoggingSettings {Enabled = isEnabled});
            var provider = services.BuildServiceProvider();
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            mockAppBuilder.Setup(app => app.ApplicationServices)
                .Returns(provider);
            mockAppBuilder.Setup(app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(mockAppBuilder.Object);
            var appBuilder = mockAppBuilder.Object;
            
            // Act
            appBuilder.UseRequestResponseDiagnostics();

            // Assert
            mockAppBuilder.Verify(
                app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()),
                Times.Exactly(timesUseCalled));
        }
    }
}