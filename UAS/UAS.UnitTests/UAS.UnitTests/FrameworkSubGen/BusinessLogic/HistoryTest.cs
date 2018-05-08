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
using ShimHistory = FrameworkSubGen.DataAccess.Fakes.ShimHistory;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    public class HistoryTest : Fakes
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
            const int historyCount = 20;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.History>(historyCount);

            ShimHistory.SaveBulkXmlString = xmlString =>
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
            var history = new History();
            var success = history.SaveBulkXml(entities);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(1),
                () => linesWritten.ShouldBe(historyCount),
                () => messageBuffer[10].ShouldBe($"Checking History: 11 of {historyCount}"));
        }

        [Test]
        public void SaveBulkXML_ValidParameter_TwoBatchesWritten()
        {
            // Arrange
            const int historyCount = 525;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.History>(historyCount);

            ShimHistory.SaveBulkXmlString = xmlString =>
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
            var history = new History();
            var success = history.SaveBulkXml(entities);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(2),
                () => linesWritten.ShouldBe(historyCount),
                () => messageBuffer[520].ShouldBe($"Checking History: 521 of {historyCount}"));
        }

        [Test]
        public void SaveBulkXML_Exception_ExceptionLogged()
        {
            // Arrange
            var entities = CreateEntities<Entity.History>(10);
            ApiLog apiLog = null;

            ShimHistory.SaveBulkXmlString = xmlString => throw new InvalidOperationException("HistoryTest_SaveBulkXMLTestApiLog");

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLog = new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                };
            };

            // Act
            var history = new History();
            var success = history.SaveBulkXml(entities);

            // Assert
            success.ShouldBe(false);
            apiLog.ShouldSatisfyAllConditions(
                () => apiLog.Entity.ShouldBe("FrameworkSubGen.BusinessLogic.History"),
                () => apiLog.Method.ShouldBe("History"));
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
