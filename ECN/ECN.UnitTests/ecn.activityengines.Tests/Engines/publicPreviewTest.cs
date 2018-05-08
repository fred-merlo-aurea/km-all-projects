using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using ecn.activityengines.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PublicPreviewTest : PageHelper
    {
        private const string DummyString = "dummyString";
        private const string HttpUserAgentKey = "HTTP_USER_AGENT";
        private const string MobileKeywordiPhone = "iphone";
        private IDisposable _context;

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void IsMobileBrowser_ValidMobileType_ReturnTrue()
        {
            // Arrange
            ShimForIsMobileBrowser(MobileKeywordiPhone);
            
            // Act
            var result = publicPreview.IsMobileBrowser();

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void IsMobileBrowser_InvalidMobileType_ReturnFalse()
        {
            // Arrange
            ShimForIsMobileBrowser(DummyString);

            // Act
            var result = publicPreview.IsMobileBrowser();

            // Assert
            result.ShouldBeFalse();
        }

        private void ShimForIsMobileBrowser(string headerValue)
        {
            _context = ShimsContext.Create();

            var shimHttpBrowserCapabilities = new ShimHttpBrowserCapabilities();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                BrowserGet = () => shimHttpBrowserCapabilities.Instance,
                ServerVariablesGet = () => new NameValueCollection
                {
                    { HttpUserAgentKey, headerValue }
                }
            };
        }
    }
}
