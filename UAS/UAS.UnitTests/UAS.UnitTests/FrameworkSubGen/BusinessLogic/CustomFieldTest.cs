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
using ShimCustomField = FrameworkSubGen.DataAccess.Fakes.ShimCustomField;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    public class CustomFieldTest : Fakes
    {
        private const int AccountId = 10;

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
            const int customFieldCount = 20;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.CustomField>(customFieldCount);

            ShimCustomField.SaveBulkXmlString = xmlString =>
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
            var customField = new CustomField();
            var success = customField.SaveBulkXml(entities.ToList(), AccountId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(1),
                () => linesWritten.ShouldBe(customFieldCount),
                () => messageBuffer[10].ShouldBe($"Checking CustomField: 11 of {customFieldCount}"));
        }

        [Test]
        public void SaveBulkXML_ValidParameter_TwoBatchesWritten()
        {
            // Arrange
            const int customFieldCount = 525;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.CustomField>(customFieldCount);

            ShimCustomField.SaveBulkXmlString = xmlString =>
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
            var customField = new CustomField();
            var success = customField.SaveBulkXml(entities.ToList(), AccountId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(2),
                () => linesWritten.ShouldBe(customFieldCount),
                () => messageBuffer[520].ShouldBe($"Checking CustomField: 521 of {customFieldCount}"));
        }

        [Test]
        public void SaveBulkXML_Exception_ExceptionLogged()
        {
            // Arrange
            var entities = CreateEntities<Entity.CustomField>(10);
            ApiLog apiLog = null;

            ShimCustomField.SaveBulkXmlString = xmlString => throw new InvalidOperationException("CustomFieldTest_SaveBulkXMLTestApiLog");

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLog = new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                };
            };

            // Act
            var customField = new CustomField();
            var success = customField.SaveBulkXml(entities.ToList(), AccountId);

            // Assert
            success.ShouldBe(false);
            apiLog.ShouldSatisfyAllConditions(
                () => apiLog.Entity.ShouldBe("FrameworkSubGen.BusinessLogic.CustomField"),
                () => apiLog.Method.ShouldBe("CustomField"));
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
