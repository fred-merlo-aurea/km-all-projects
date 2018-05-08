using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkSubGen.BusinessLogic;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using KM.Common.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using Entity = FrameworkSubGen.Entity;
using ShimAccount = FrameworkSubGen.DataAccess.Fakes.ShimAccount;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    public class AccountTest : Fakes
    {
        private Mocks _mocks;
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            SetupFakes(_mocks);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SaveBulkXML_ValidParameter_OneBatchWritten()
        {
            // Arrange
            const int accountCount = 20;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.Account>(accountCount);

            ShimAccount.SaveBulkXmlString = xmlString =>
            {
                xmlBuffer.Add(xmlString);
                return true;
            };

            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (message, color) =>
            {
                messageBuffer.Add(message);
                linesWritten++;
            };

            // Act
            var account = new Account();
            var success = account.SaveBulkXml(entities);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(1),
                () => linesWritten.ShouldBe(accountCount),
                () => messageBuffer[10].ShouldBe($"Checking Account: 11 of {accountCount}"));
        }

        [Test]
        public void SaveBulkXML_ValidParameter_TwoBatchesWritten()
        {
            // Arrange
            const int accountCount = 525;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.Account>(accountCount);

            ShimAccount.SaveBulkXmlString = xmlString =>
            {
                xmlBuffer.Add(xmlString);
                return true;
            };

            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (message, color) =>
            {
                messageBuffer.Add(message);
                linesWritten++;
            };

            // Act
            var account = new Account();
            var success = account.SaveBulkXml(entities);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(2),
                () => linesWritten.ShouldBe(accountCount),
                () => messageBuffer[520].ShouldBe($"Checking Account: 521 of {accountCount}"));
        }

        [Test]
        public void SaveBulkXML_Exception_ExceptionLogged()
        {
            // Arrange
            var entities = CreateEntities<Entity.Account>(10);
            ApiLog apiLog = null;

            ShimAccount.SaveBulkXmlString = xmlString => throw new InvalidOperationException("AccountTest_SaveBulkXMLTestApiLog");

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLog = new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                };
            };

            // Act
            var account = new Account();
            var success = account.SaveBulkXml(entities);

            // Assert
            success.ShouldBe(false);
            apiLog.ShouldSatisfyAllConditions(
                () => apiLog.Entity.ShouldBe("FrameworkSubGen.BusinessLogic.Account"),
                () => apiLog.Method.ShouldBe("Account"));
        }

        private static IList<T> CreateEntities<T>(int count) where T : Entity.IEntity, new()
        {
            return Enumerable
                .Range(1, count)
                .Select(x => new T())
                .ToList();
        }
    }
}
