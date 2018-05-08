using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail.Fakes;
using ecn.blastengine;
using ecn.blastengine.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using KM.Common.Fakes;
using ClassesFakes = ecn.common.classes.Fakes;
using CommunicatorClassesFakes = ecn.communicator.classes.Fakes;
using KM.Common.Entity.Fakes;
using ECN_Framework_Common.Objects;

namespace ECN.BlastEngine.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastEngineTest
    {
        private IDisposable _shimObject;
        private readonly NameValueCollection appSettings = new NameValueCollection();
        private const string doBlast = "MULTIPLE";
        private const string engineID = "232323";
        private const string statusCode = "sunilpending";
        private const string FromEmailAddress = "test@test.com";
        private const string ToEmailAddress = "sample@sample.com";
        private const string SamppleSmtpServer = "192.168.0.1";
        private const string SamplePath = "SamplePath";
        private const string ecnEngineAccessKey = "651A1297-59D1-4857-93CB-0B049635762E";
        private const string MethodMain = "Main";
        private const string DummyString = "DummyString";
        private const string AdminSendTo = "AdminSendTo";
        private const string BlastFunction = "_BlastFunction";
        private const string Single = "SINGLE";
        private const string Multiple = "MULTIPLE";
        private const string CreateCache = "CreateCache";
        private const string RemoveCache = "Removecache";
        private const string GetAvailableLicenses = "GetAvailableLicenses";
        private const string UnlimitedString = "UNLIMITED";
        private const string NoLicense = "NO LICENSE";
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string ContentExists = "ContentExists";
        private const int DummyId = 1;
        private const string KMCommonApplication = "KMCommon_Application";
        private const string SendEmailNotification = "SendEmailNotification";
        private ECNBlastEngine _testEntity;
        private PrivateObject _privateECNBlastEngineObj;
        private PrivateType _privateTypeECNBlastEngine;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            appSettings.Clear();
            appSettings.Add("DoBlast", doBlast);
            appSettings.Add("EngineID", engineID);
            appSettings.Add("StatusCode", statusCode);
            appSettings.Add("AdminSendFrom", FromEmailAddress);
            appSettings.Add("AcctManagersSendTo", ToEmailAddress);
            appSettings.Add("SMTPServer", SamppleSmtpServer);
            appSettings.Add("ECNEngineAccessKey", ecnEngineAccessKey);
            appSettings.Add("Communicator_VirtualPath", SamplePath);
            ShimConfigurationManager.AppSettingsGet = () => appSettings;

            _testEntity = new ECNBlastEngine();
            _privateECNBlastEngineObj = new PrivateObject(_testEntity);
            _privateTypeECNBlastEngine = new PrivateType(typeof(ECNBlastEngine));
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Main_ArgsLengthIsZero_ReachEnd()
        {
            // Arrange
            var methodStartBlastEngineInvoked = false;
            ShimECNBlastEngine.AllInstances.StartBlastEngine = _ => { methodStartBlastEngineInvoked = true; };

            // Act
            _privateTypeECNBlastEngine.InvokeStatic(MethodMain, new object[] { new string[] { } });

            // Assert
            methodStartBlastEngineInvoked.ShouldBeTrue();
        }

        [Test]
        public void Main_InvalidArgs_ReachEnd()
        {
            // Arrange
            const string ArgsValue = "/?";

            // Act, Assert
            _privateTypeECNBlastEngine.InvokeStatic(MethodMain, new object[] { new string[] { ArgsValue } });
        }

        [Test]
        public void Main_ValidArgs_ReachEnd()
        {
            // Arrange
            const string ArgsValue = "/test";
            var methodSendEmailNotification = false;
            ShimECNBlastEngine.AllInstances.SendEmailNotificationStringString =
                (_, __, ___) => { methodSendEmailNotification = true; };
            appSettings.Add(AdminSendTo, FromEmailAddress);

            // Act
            _privateTypeECNBlastEngine.InvokeStatic(MethodMain, new object[] { new string[] { ArgsValue } });

            // Assert
            methodSendEmailNotification.ShouldBeTrue();
        }

        [Test]
        [TestCase(Single)]
        [TestCase(Multiple)]
        public void StartBlastEngine_OnValidCall_DoBlast(string emailFunction)
        {
            // Arrange
            var handleBlastInvoked = false;
            _privateECNBlastEngineObj.SetField(BlastFunction, emailFunction);
            ShimECNBlastEngine.AllInstances.HandleSingleBlastEmailFunctions = (_, __) =>
            {
                handleBlastInvoked = true;
                throw new Exception();
            };
            ShimECNBlastEngine.AllInstances.HandleBlastEmailFunctions = (_, __) =>
            {
                handleBlastInvoked = true;
                throw new Exception();
            };

            // Act
            // adding try/catch to break the infinite loop line #162 while (true){...}
            try
            {
                _testEntity.StartBlastEngine();
            }
            catch
            {
            }

            // Assert
            handleBlastInvoked.ShouldBeTrue();
        }

        [Test]
        public void CreateCache_OnValidCall_ReachEnd()
        {
            // Arrange
            var getByBaseChannelIDInvoked = false;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastLayout
            {
                GroupID = DummyId,
                LayoutID = DummyId,
                CustomerID = DummyId
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = _ => new Group();
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (_, __) => new Layout();
            ShimCustomer.GetByCustomerIDInt32Boolean = (_, __) => new Customer
            {
                BaseChannelID = DummyId
            };
            ShimSubscriptionManagement.GetByBaseChannelIDInt32 = _ =>
            {
                getByBaseChannelIDInvoked = true;
                return new List<SubscriptionManagement>();
            };

            // Act
            _privateECNBlastEngineObj.Invoke(CreateCache, DummyId);

            // Assert
            getByBaseChannelIDInvoked.ShouldBeTrue();
        }

        [Test]
        public void Removecache_OnValidCall_ReachEnd()
        {
            // Arrange
            var removeFromCacheInvoked = false;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastLayout
            {
                GroupID = DummyId,
                LayoutID = DummyId,
                CustomerID = DummyId
            };
            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.RemoveFromCacheStringString = (_, __) =>
            {
                removeFromCacheInvoked = true;
            };

            // Act
            _privateECNBlastEngineObj.Invoke(RemoveCache, DummyId);

            // Assert
            removeFromCacheInvoked.ShouldBeTrue();
        }

        [Test]
        [TestCase(UnlimitedString, TestOne, true)]
        [TestCase(NoLicense, TestOne, false)]
        [TestCase(TestOne, TestZero, true)]
        [TestCase(TestOne, TestOne, true)]
        [TestCase(TestZero, TestOne, false)]
        public void GetAvailableLicenses_OnValidCall_ReturnBoolean(
            string blastLicensed,
            string countToSend,
            bool expectedResult)
        {
            // Arrange
            const string TestBlastY = "y";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastLayout
            {
                GroupID = DummyId,
                LayoutID = DummyId,
                CustomerID = DummyId,
                TestBlast = TestBlastY
            };
            ClassesFakes::ShimLicenseCheck.AllInstances.CurrentString = (_, __) => blastLicensed;
            ClassesFakes::ShimLicenseCheck.AllInstances.AvailableString = (_, __) => blastLicensed;
            ClassesFakes::ShimLicenseCheck.AllInstances.UsedString = (_, __) => DummyString;
            CommunicatorClassesFakes::ShimEmailFunctions.GetBlastRemainingCountInt32Int32Int32Int32StringStringBoolean
                = (_, _2, _3, _4, _5, _6, _7) => int.Parse(countToSend);

            // Act
            var actualResult = _privateECNBlastEngineObj.Invoke(GetAvailableLicenses, DummyId);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ContentExists_OnException_ReturnString(bool isECNException)
        {
            // Arrange
            appSettings.Add(KMCommonApplication, TestOne);
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (_, _2, _3, _4, _5, _6) => DummyId;
            var expectedString = $"Content ID: {DummyId}";
            ShimSqlCommand.ConstructorString = (_, __) => { };
            ShimSqlCommand.AllInstances.ParametersGet = _ => new ShimSqlParameterCollection();
            ClassesFakes::ShimDataFunctions.ExecuteScalarSqlCommand = _ =>
            {
                if(isECNException)
                {
                    throw new ECNException(new List<ECNError>
                    {
                        new ECNError
                        {
                            Entity = Enums.Entity.APILogging
                        }
                    });
                }
                throw new Exception();
            };
            ShimECNBlastEngine.AllInstances.SetBlastToErrorInt32 = (_, __) => { };

            // Act
            var actualResult = _privateECNBlastEngineObj.Invoke(ContentExists, DummyId) as string;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrEmpty(),
                () => actualResult.ShouldContain(expectedString));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ContentExists_OnValidCall_ReturnString(bool isValid)
        {
            // Arrange
            const string ExpectedString = "Invalid Content processed in Blast Engine";
            ShimSqlCommand.ConstructorString = (_, __) => { };
            ShimSqlCommand.AllInstances.ParametersGet = _ => new ShimSqlParameterCollection();
            ClassesFakes::ShimDataFunctions.ExecuteScalarSqlCommand = _ => DummyId;
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (_, __) => new Content();
            ShimContent.ValidateHTMLContentString = _ => isValid;

            // Act
            var actualResult = _privateECNBlastEngineObj.Invoke(ContentExists, DummyId) as string;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () =>
                {
                    if(isValid)
                    {
                        actualResult.ShouldBeNullOrEmpty();
                    }
                    else
                    {
                        actualResult.ShouldNotBeNullOrEmpty();
                        actualResult.ShouldContain(ExpectedString);
                    }
                });
        }

        [Test]
        public void SendEmailNotification_OnValidCall_SendEmail()
        {
            // Arrange
            var emailSent = false;
            appSettings.Add(AdminSendTo, FromEmailAddress);
            ShimSmtpClient.AllInstances.SendMailMessage = (_, __) => { emailSent = true; };

            // Act
            var actualResult = _privateECNBlastEngineObj.Invoke(SendEmailNotification, DummyString, DummyString);

            // Assert
            emailSent.ShouldBeTrue();
        }
    }
}
