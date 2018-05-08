using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ecn.communicator.classes;
using ECN.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Classes
{
    /// <summary>
    /// Unit tests for <see cref="ECN_EmailActivityLog"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ECN_EmailActivityLogTest
    {
        private const string ConnString = "activity";
        private const string ConnectionString = "test-connection-string";
        private const int BlastId = 10;
        private const int ReadCount = 1;

        private IDisposable _context;
        private IList<ECN_EmailActivityLog> _logs;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _logs = new List<ECN_EmailActivityLog>
            {
                typeof(ECN_EmailActivityLog).CreateInstance(),
                typeof(ECN_EmailActivityLog).CreateInstance(true)
            };

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, TestCaseSource(nameof(ActionTypeCodes))]
        public void GetByBlastID_WhenCalled_VerifyReaderData(string actionTypeCode)
        {
            // Arrange
            var expected = new List<ECN_EmailActivityLog>
            {
                typeof(ECN_EmailActivityLog).CreateInstance(),
                new ECN_EmailActivityLog(),
                new ECN_EmailActivityLog
                {
                    ActionTypeCode = actionTypeCode
                }
            };

            // Act
            var results = ECN_EmailActivityLog.GetByBlastID(BlastId);

            // Assert
            results
                .IsListContentMatched(expected)
                .ShouldBeTrue();
        }

        private void SetupFakes()
        {
            ShimSqlConnection.ConstructorString = (_, conn) => conn.ShouldBe(ConnectionString);
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                [ConnString] = ConnectionString
            };

            ShimSqlConnection.AllInstances.Open = _ => { };

            var read = -1;
            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                command.CommandText.ShouldBe(Query.ToString());

                return new ShimSqlDataReader
                {
                    Read = () =>
                    {
                        read++;
                        return read <= ReadCount;
                    },
                    GetOrdinalString = name => typeof(ECN_EmailActivityLog).GetPropertyIndex(name),
                    ItemGetInt32 = index => _logs[read].GetPropertyValue(index),
                    IsDBNullInt32 = index =>
                    {
                        var propertyValue = _logs[read].GetPropertyValue(index);
                        return propertyValue == null || propertyValue.Equals(propertyValue.GetType().GetDefaultValue());
                    }
                }.Instance;
            };
        }

        private static StringBuilder Query => new StringBuilder()
            .Append(" SELECT l.EAID,")
            .Append(" l.EmailID,")
            .Append(" e.EmailAddress,")
            .Append(" l.BlastID,")
            .Append(" l.ActionTypeCode,")
            .Append(" l.ActionDate,")
            .Append(" l.ActionValue,")
            .Append(" l.ActionNotes,")
            .Append(" l.Processed")
            .Append(" FROM EmailActivityLog l")
            .Append(" INNER JOIN ecn5_communicator..Emails e ON l.EmailID = e.EmailID")
            .Append(" WHERE l.BlastID = " + BlastId);

        private static readonly IEnumerable<string> ActionTypeCodes = new List<string>
        {
            "ABUSERPT_UNSUB",
            "MASTSUP_UNSUB",
            "testsend",
            "bounce",
            "resend",
            "FEEDBACK_UNSUB",
            "send",
            "click",
            "open",
            "subscribe",
            "conversion",
            "read",
            "refer",
            "conversionRevenue"
        };
    }
}
