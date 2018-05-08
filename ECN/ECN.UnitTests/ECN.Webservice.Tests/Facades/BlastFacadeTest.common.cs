using System;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice.Facades;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Webservice.Tests.Facades
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastFacadeTest
    {
        private BlastFacade _testEntity;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _testEntity = new BlastFacade();
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }
    }
}
