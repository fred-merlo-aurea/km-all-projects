using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using ecn.communicator.classes;
using ECN.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Classes
{
    /// <summary>
    /// Unit tests for <see cref="ECN_Blasts"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ECN_BlastsTest
    {
        private const string ConnString = "connString";
        private const string ConnectionString = "test-connection-string";
        private const int CustomerId = 10;
        private const int ReadCount = 1;
        private const string EmailSubject = "-- Select ECN Blast --";
        private const int DummyBlastID = -1;

        private IDisposable _context;
        private IList<ECN_Blasts> _blasts;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _blasts = new List<ECN_Blasts>
            {
                typeof(ECN_Blasts).CreateInstance(),
                typeof(ECN_Blasts).CreateInstance(true)
            };

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetBlastByCustomerID_WhenCalled_VerifyReaderData()
        {
            // Arrange
            var expected = new List<ECN_Blasts>
            {
                new ECN_Blasts
                {
                    EmailSubject = EmailSubject,
                    BlastID = DummyBlastID
                },
                typeof(ECN_Blasts).CreateInstance(),
                new ECN_Blasts()
            };

            // Act
            var results = ECN_Blasts.GetBlastByCustomerID(CustomerId);

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
                var query = $" SELECT *  FROM Blasts  WHERE CustomerID={CustomerId} ";
                command.CommandText.ShouldBe(query);

                return new ShimSqlDataReader
                {
                    Read = () =>
                    {
                        read++;
                        return read <= ReadCount;
                    },
                    GetOrdinalString = name => typeof(ECN_Blasts).GetPropertyIndex(name),
                    ItemGetInt32 = index => _blasts[read].GetPropertyValue(index),
                    IsDBNullInt32 = index =>
                    {
                        var propertyValue = _blasts[read].GetPropertyValue(index);
                        return propertyValue == null || propertyValue.Equals(propertyValue.GetType().GetDefaultValue());
                    }
                }.Instance;
            };
        }
    }
}
