using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ContentTest
    {
        private const string SampleFeed1 = "SampleFeed1.SomeSampleContent";
        private const string SampleFeed2 = "SampleFeed2.SomeSampleContent";
        private const string DynamicFeed3 = "ECN.DynamicTag.1.ECN.DynamicTag";
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
