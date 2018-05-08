using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace KMManagers.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class KMEntitiesWrapperTest
    {
        private const string SampleDomain = "http://www.km.com";
        private const string CssDirKey = "CssDir";
        private const string ApiDomainKey = "ApiDomainKey";
        private const string ApiTimeoutKey = "ApiTimeoutKey";
        private const string AspNetMxLevelKey = "AspNetMxLevelKey";
        private const string BlockSubmitIfTimeoutKey = "BlockSubmitIfTimeoutKey";

        private IDisposable _shimsObject;

        [SetUp]
        public void SetUp()
        {
            _shimsObject = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shimsObject.Dispose();
        }
    }
}
