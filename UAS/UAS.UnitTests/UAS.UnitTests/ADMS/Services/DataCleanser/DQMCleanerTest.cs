using System;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace UAS.UnitTests.ADMS.Services.DataCleanser
{
    [TestFixture]
    public partial class DQMCleanerTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }
    }
}
