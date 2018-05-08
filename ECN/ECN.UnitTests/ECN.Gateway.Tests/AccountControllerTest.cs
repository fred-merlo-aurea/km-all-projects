using System.Diagnostics.CodeAnalysis;
using ecn.gateway.Controllers;
using ECN.Gateway.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Gateway.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class AccountControllerTest : ControllerHelper
    {
        private const string PubCodeKey = "PubCode";
        private const string TypeCodeKey = "TypeCode";
        private const string SamplePubCode = "SamplePubCode";
        private const string SampleTypeCode = "SampleTypeCode";
        private const string UserNameKey = "UserName";
        private const string SampleEmailAddress = "sample@test.com";
        private AccountController _testEntity;
        private PrivateObject _privateTestEntity;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            QueryString.Clear();
            QueryString.Add(PubCodeKey, SamplePubCode);
            QueryString.Add(TypeCodeKey, SampleTypeCode);
            base.SetPageSessionContext();
            _testEntity = new AccountController();
            _privateTestEntity = new PrivateObject(_testEntity);
        }
    }
}
