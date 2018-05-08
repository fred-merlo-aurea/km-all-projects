using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using FrameworkSubGen.BusinessLogic;
using Entity = FrameworkSubGen.Entity;
using Core_AMS.Utilities.Fakes;
using FrameworkSubGen.DataAccess.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    public class SubscriberTest
    {
        private IDisposable _context;
        private Subscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _subscriber = new Subscriber();

            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, consoleColor) => { };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SaveBulkXml_ValidSubscribersList_SavesXml()
        {
            // Arrange
            var subscribersList = CreateSubscriberList(1);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var actualSavedXml = String.Empty;
            var expectedNrOfLinesWrittenToConsole = 1;
            var expectedNrOfXmlsSaved = 1;
            var expectedSavedXml = $"<XML>xml_string{Environment.NewLine}</XML>";
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimSubscriber.SaveBulkXmlString = xml =>
            {
                actualNrOfXmlsSaved++;
                actualSavedXml = xml;
                return true;
            };            

            // Act
            _subscriber.SaveBulkXml(subscribersList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualSavedXml.ShouldBe(expectedSavedXml);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
        }

        [Test]
        public void SaveBulkXml_ValidSubscribersList_SavesXmlInBatches()
        {
            // Arrange
            var subscribersList = CreateSubscriberList(600);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var expectedNrOfLinesWrittenToConsole = 600;
            var expectedNrOfXmlsSaved = 2;
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimSubscriber.SaveBulkXmlString = xml =>
            {
                actualNrOfXmlsSaved++;
                return true;
            };

            // Act
            _subscriber.SaveBulkXml(subscribersList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
        }

        [Test]
        public void SaveBulkXml_SaveXmlException_ExceptionIsCaughtAndLogged()
        {
            // Arrange
            var subscribersList = CreateSubscriberList(600);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var actualNrOfLoggedExceptions = 0;
            var expectedNrOfLinesWrittenToConsole = 600;
            var expectedNrOfXmlsSaved = 1;
            var expectedNrOfLoggedExceptions = 1;
            var exceptionWasThrown = false;
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimSubscriber.SaveBulkXmlString = xml =>
            {
                if (!exceptionWasThrown)
                {
                    exceptionWasThrown = true;
                    throw new InvalidOperationException();
                }
                actualNrOfXmlsSaved++;
                return true;
            };
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (instance, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    actualNrOfLoggedExceptions++;
                    return 1;
                };

            // Act
            _subscriber.SaveBulkXml(subscribersList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
            actualNrOfLoggedExceptions.ShouldBe(expectedNrOfLoggedExceptions);
        }

        private List<Entity.Subscriber> CreateSubscriberList(int count)
        {
            var accountId = 1;
            var subscriberId = 1;
            return Enumerable.Repeat(
                new Entity.Subscriber
                {
                    account_id = accountId++,
                    subscriber_id = subscriberId++,
                    first_name = "",
                    last_name = "",
                    email = "email",
                    password = "pass",
                    password_md5 = "pass_md5",
                    renewal_code = "",
                    source = "source",
                    create_date = DateTime.Now,
                    delete_date = DateTime.Now
                },
                count).ToList();
        }
    }
}
