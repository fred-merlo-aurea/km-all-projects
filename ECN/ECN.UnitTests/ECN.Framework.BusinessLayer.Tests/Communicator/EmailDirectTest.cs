using System;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class EmailDirectTest
    {
        private const string ValidateMethodName = "Validate";
        private const string SampleContent = "SampleContent";
        private IDisposable _shimsObject;
        private PrivateType _privateEmailDirectType;

        [SetUp]
        public void SetUp()
        {
            _shimsObject = ShimsContext.Create();
            _privateEmailDirectType = new PrivateType(typeof(EmailDirect));
        }

        [TearDown]
        public void CleanUp()
        {
            _shimsObject.Dispose();
        }
    }
}
