using System;
using System.Diagnostics.CodeAnalysis;
using ecn.digitaledition;
using ECN.Digitaledition.Test.Helper;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Digitaledition.Test
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class MagazineTest : PageHelper
    {
        private const string SampleAddress = "SampleAddress";
        private const string SamplePhone = "9999999999";
        private const string SampleChannel = "SampleChannel";
        private const string TestPublicationCode = "TestPublicationCode";
        private const string SampleEdition = "SampleEdition";
        private const string SampleEmail = "test@test.com";
        private const string SampleFileName = "TestFileName";
        private const string SampleIpAddress = "192.168.0.1";
        private const string SessionKeyIpAddress = "IP";
        private const string DefaultContactFormLink = "http://www.km-ecn-contact.com/";
        private const string SampleLogoLink = "http://www.km.com";
        private const string SampleLogoURL = "http://www.km-ecn.com/images/km-logo-plate.gif";
        private const string SampleSubscriptionLink = "http://www.km-ecn-subscription.com/";
        private const string PageLoadMethodName = "Page_Load";

        private Magazine _testEntity;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            QueryString.Clear();
            _testEntity = new Magazine();
            InitializeAllControls(_testEntity);
            _privateObject = new PrivateObject(_testEntity);
        }
    }
}
