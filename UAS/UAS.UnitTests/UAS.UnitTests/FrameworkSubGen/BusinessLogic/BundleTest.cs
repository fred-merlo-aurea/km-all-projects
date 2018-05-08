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
using ShimBundle = FrameworkSubGen.DataAccess.Fakes.ShimBundle;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    public class BundleTest : Fakes
    {
        private const int AccountId = 42;
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
            const int bundleCount = 20;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.Bundle>(bundleCount);

            ShimBundle.SaveBulkXmlString = xmlString =>
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
            var bundle = new Bundle();
            var success = bundle.SaveBulkXml(entities, AccountId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => entities.ShouldAllBe(x => x.account_id == AccountId),
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(1),
                () => linesWritten.ShouldBe(bundleCount),
                () => messageBuffer[10].ShouldBe($"Checking Bundle: 11 of {bundleCount}"));
        }

        [Test]
        public void SaveBulkXML_ValidParameter_TwoBatchesWritten()
        {
            // Arrange
            const int bundleCount = 525;
            var messageBuffer = new List<string>();
            var xmlBuffer = new List<string>();
            var linesWritten = 0;

            var entities = CreateEntities<Entity.Bundle>(bundleCount);

            ShimBundle.SaveBulkXmlString = xmlString =>
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
            var bundle = new Bundle();
            var success = bundle.SaveBulkXml(entities, AccountId);

            // Assert
            success.ShouldSatisfyAllConditions(
                () => entities.ShouldAllBe(x => x.account_id == AccountId),
                () => success.ShouldBeTrue(),
                () => xmlBuffer.Count.ShouldBe(2),
                () => linesWritten.ShouldBe(bundleCount),
                () => messageBuffer[520].ShouldBe($"Checking Bundle: 521 of {bundleCount}"));
        }

        [Test]
        public void SaveBulkXML_Exception_ExceptionLogged()
        {
            // Arrange
            var entities = CreateEntities<Entity.Bundle>(10);
            ApiLog apiLog = null;

            ShimBundle.SaveBulkXmlString = xmlString => throw new InvalidOperationException("BundleTest_SaveBulkXMLTestApiLog");

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
                {
                    apiLog = new ApiLog
                                 {
                                     Entity = myClass,
                                     Method = myMethod
                                 };
                };

            // Act
            var bundle = new Bundle();
            var success = bundle.SaveBulkXml(entities, AccountId);

            // Assert
            success.ShouldBe(false);
            apiLog.ShouldSatisfyAllConditions(
                () => apiLog.Entity.ShouldBe("FrameworkSubGen.BusinessLogic.Bundle"),
                () => apiLog.Method.ShouldBe("Bundle"));
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
