using System.Diagnostics.CodeAnalysis;
using ecn.communicator.MasterPages.Fakes;
using ecn.communicator.main.admin.LandingPages;
using ecn.communicator.main.admin.LandingPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using MasterPage = ecn.communicator.MasterPages;
using NUnit.Framework;
using MSUTesting = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BaseChannelUpdateEmailTest : PageHelper
    {
        private const string InvalidCodeSnippet = "%%";
        private const string EmptyCodeSnippet = "%% %%";
        private const string DummyCodeSnippet = "%%Dummy%%";
        private const string BaseChannelNameCodeSnippet = "%%basechannelname%%";
        private const string PageNameCodeSnippet = "%%pagename%%";
        private const string GroupDescriptionCodeSnippet = "%%groupdescription%%";
        private const string ErrorBadFormattedCodeSnippet = "There is a badly formed codesnippet";
        private const string ErrorInvalidBaseChannelCodeSnippet = "Invalid codesnippet, only %%basechannelname%% and %%pagename%% are allowed";

        private BaseChannelUpdateEmail _baseChannel;
        private MSUTesting.PrivateObject _privateTestObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _baseChannel = new BaseChannelUpdateEmail();
            _privateTestObject = new MSUTesting.PrivateObject(_baseChannel);
            InitializeAllControls(_baseChannel);
            InitializeSessionFakes();
        }

        [Test]
        public void ValidCodeSnippets_EmptyString_ReturnsTrueCodeSnippetWithNullMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    string.Empty
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_SingleMatch_ReturnsFalseCodeSnippetWithErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    InvalidCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.False);
            Assert.That(codeSnippetError.message, Is.EqualTo(ErrorBadFormattedCodeSnippet));
        }

        [Test]
        public void ValidCodeSnippets_TwoMatches_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    EmptyCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesButNotbasechannelnameORpagenameORgroupdescription_ReturnsFalseCodeSnippetWithErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    DummyCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.False);
            Assert.That(codeSnippetError.message, Is.EqualTo(ErrorInvalidBaseChannelCodeSnippet));
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithbasechannelname_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    BaseChannelNameCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithpagename_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    PageNameCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithgroupdescription_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _baseChannel,
                "validCodeSnippets",
                new object[]
                {
                    GroupDescriptionCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        private void InitializeSessionFakes()
        {
            QueryString.Clear();
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = true,
                CustomerID = 1
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimBaseChannelUpdateEmail.AllInstances.MasterGet = (s) => new MasterPage.Communicator { };
            ShimCommunicator.AllInstances.UserSessionGet = (m) => shimSession.Instance;
        }
    }
}
