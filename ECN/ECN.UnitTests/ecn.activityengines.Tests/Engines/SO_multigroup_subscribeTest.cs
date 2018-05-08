using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using ecn.communicator.classes;
using ecn.communicator.classes.Fakes;
using ECN.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    /// <summary>
    /// Unit tests for <see cref="SO_multigroup_subscribe"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SO_multigroup_subscribeTest
    {
        private IDisposable _context;
        private SO_multigroup_subscribe _page;
        private Emails _oldEmails;
        private Emails _emails;
        private SqlCommand _actualCommand;
        private static CultureInfo _previousCulture;
        private int _emailId;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _page = typeof(SO_multigroup_subscribe).CreateInstance();
            _emails = typeof(Emails).CreateInstance();
            _emailId = EmailId;

            SetUpFields();
            SetupFakes();
        }

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DefaultCulture);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(SqlCommandParametersTestData))]
        public void CreateEmailRecord_NullOldEmails_VerifyInsertCommandParameters(
            string parameterName,
            int size,
            SqlDbType dbType,
            string parameterValue)
        {
            // Arrange
            ShimPage.AllInstances.RequestGet = p => new HttpRequest(Filename, Url, string.Empty);
            _oldEmails = null;

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateEmailRecord, new object[] { new Groups() }, _page);

            // Assert
            var parameter = _actualCommand.Parameters[parameterName];
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_emails),
                () => parameter.ShouldNotBeNull(),
                () => parameter.SqlDbType.ShouldBe(dbType),
                () => parameter.Value.ToString().ShouldBe(parameterValue),
                () => parameter.Size.ShouldBe(size));
        }

        [Test]
        [TestCaseSource(nameof(SqlCommandParametersWithInvalidQueryStringTestData))]
        public void CreateEmailRecord_NullOldEmailsInvalidQueryString_VerifyInsertCommandParameters(
            string parameterName,
            int size,
            SqlDbType dbType,
            string parameterValue)
        {
            // Arrange
            _oldEmails = null;

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateEmailRecord, new object[] { new Groups() }, _page);

            // Assert
            var parameter = _actualCommand.Parameters[parameterName];
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_emails),
                () => parameter.ShouldNotBeNull(),
                () => parameter.SqlDbType.ShouldBe(dbType),
                () => parameter.Value.ToString().ShouldBe(parameterValue),
                () => parameter.Size.ShouldBe(size));
        }

        [Test]
        [TestCaseSource(nameof(SqlCommandParametersTestData))]
        public void CreateEmailRecord_WithOldEmails_VerifyUpdateCommandParameters(
            string parameterName,
            int size,
            SqlDbType dbType,
            string parameterValue)
        {
            // Arrange
            ShimPage.AllInstances.RequestGet = p => new HttpRequest(Filename, Url, string.Empty);
            _oldEmails = new Emails();
            _emailId = 0;

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateEmailRecord, new object[] { new Groups() }, _page);

            // Assert
            if (parameterName != EmailAddressParam &&
                parameterName != CustomerIDParam)
            {
                var parameter = _actualCommand.Parameters[parameterName];
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(_emails),
                    () => parameter.ShouldNotBeNull(),
                    () => parameter.SqlDbType.ShouldBe(dbType),
                    () => parameter.Value.ToString().ShouldBe(parameterValue),
                    () => parameter.Size.ShouldBe(size));
            }
        }

        [Test]
        [TestCaseSource(nameof(SqlCommandParametersWithInvalidQueryStringTestData))]
        public void CreateEmailRecord_WithOldEmailsInvalidQueryString_VerifyUpdateCommandParameters(
            string parameterName,
            int size,
            SqlDbType dbType,
            string parameterValue)
        {
            // Arrange
            _oldEmails = new Emails();
            _emailId = 0;

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateEmailRecord, new object[] { new Groups() }, _page);

            // Assert
            if (parameterName != EmailAddressParam &&
                parameterName != CustomerIDParam)
            {
                var parameter = _actualCommand.Parameters[parameterName];
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(_emails),
                    () => parameter.ShouldNotBeNull(),
                    () => parameter.SqlDbType.ShouldBe(dbType),
                    () => parameter.Value.ToString().ShouldBe(parameterValue),
                    () => parameter.Size.ShouldBe(size));
            }
        }

        [Test]
        public void CreateNote_ValidQueryString_ReturnCorrectEmailVariables()
        {
            // Arrange
            ShimPage.AllInstances.RequestGet = p => new HttpRequest(Filename, Url, string.Empty);
            var expected = new StringBuilder();

            expected.AppendLine($"<br><b>Blast ID:</b>&nbsp;{BlastId}");
            expected.AppendLine($"<br><b>Group IDs:</b>&nbsp;{SubscriptionGroupIDs}");
            expected.AppendLine($"<br><b>Smart Form ID:</b>&nbsp;{SmartFormId}");
            expected.AppendLine($"<br><b>Email Address:</b>&nbsp;{EmailAddress}");

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateNote, new object[0], _page);

            // Assert
            result.ToString().ShouldBe(expected.ToString());
        }

        [Test]
        public void CreateNote_InvalidQueryString_ReturnCorrectEmailVariables()
        {
            // Arrange
            var expected = new StringBuilder();

            expected.AppendLine("<br><b>Blast ID:</b>&nbsp;0");
            expected.AppendLine($"<br><b>Group IDs:</b>&nbsp;{SubscriptionGroupIDs}");
            expected.AppendLine("<br><b>Smart Form ID:</b>&nbsp;0");
            expected.AppendLine("<br><b>Email Address:</b>&nbsp;");

            // Act
            var result = typeof(SO_multigroup_subscribe).CallMethod(CreateNote, new object[0], _page);

            // Assert
            result.ToString().ShouldBe(expected.ToString());
        }

        private void SetUpFields()
        {
            _page.SetField(nameof(EmailAddress), EmailAddress);
            _page.SetField(nameof(CustomerID), (object)CustomerID);
            _page.SetField(nameof(SubscriptionGroupIDs), SubscriptionGroupIDs);
        }

        private void SetupFakes()
        {
            FakeParams();
            FakeAppSettings();
            FakeSqlLogic();

            ShimGroups.AllInstances.WhatEmailForCustomerString = (_, email) =>
            {
                email.ShouldBe(EmailAddress);
                return _oldEmails;
            };

            ShimEmails.GetEmailByIDInt32 = id =>
            {
                id.ShouldBe(_emailId);
                return _emails;
            };
        }

        private void FakeAppSettings()
        {
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                ["connString"] = ConnectonString
            };
        }

        private void FakeSqlLogic()
        {
            ShimSqlConnection.ConstructorString = (_, conn) => { conn.ShouldBe(ConnectonString); };
            ShimSqlConnection.AllInstances.Open = _ => { };
            ShimSqlConnection.AllInstances.Close = _ => { };
            ShimSqlCommand.AllInstances.Prepare = _ => { };
            ShimSqlCommand.AllInstances.ExecuteScalar = command =>
            {
                _actualCommand = command;
                return EmailId;
            };

            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                _actualCommand = command;
                return -1;
            };
        }

        private void FakeParams()
        {
            ShimHttpRequest.AllInstances.ParamsGet = request => new NameValueCollection
            {
                ["sfID"] = SmartFormId,
                ["b"] = BlastId,
                ["e"] = EmailAddress,
                ["c"] = CustomerId,
                ["t"] = Title,
                ["fn"] = FirstName,
                ["ln"] = LastName,
                ["n"] = FullName,
                ["adr"] = StreetAddress,
                ["adr2"] = StreetAddress2,
                ["compname"] = CompanyName,
                ["city"] = City,
                ["state"] = State,
                ["ctry"] = Country,
                ["zc"] = ZipCode,
                ["ph"] = Phone,
                ["mph"] = MobilePhone,
                ["fax"] = Fax,
                ["website"] = Website,
                ["age"] = Age,
                ["income"] = Income,
                ["gndr"] = Gender,
                ["occ"] = Occupation,
                ["bdt"] = DateOfBirth,
                ["usr1"] = User1,
                ["usr2"] = User2,
                ["usr3"] = User3,
                ["usr4"] = User4,
                ["usr5"] = User5,
                ["usr6"] = User6,
                ["usrevt1"] = UserEvent1,
                ["usrevtdt1"] = UserEventDateTime,
                ["usrevt2"] = UserEvent2,
                ["usrevtdt2"] = UserEventDateTime2
            };
        }
    }
}
