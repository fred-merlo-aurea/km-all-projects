using System;
using ecn.webservice;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class BlastManagerTest
    {
        private PrivateObject _privateObject;
        private IDisposable _shims;
        private const string Time = "time";
        private const string Date = "date";
        private const string MethodCreateNewXml = "CreateNewXMLScheduleFromOld";
        private const string OldXmlTypeO = "<Schedule><Type ID='O'><StartTime>time</StartTime><StartDate>date</StartDate></Type></Schedule>";
        private const string OldXmlTypeD = "<Schedule><Type ID='D'><StartTime>time</StartTime><StartDate>2018/05/02</StartDate></Type></Schedule>";
        private const string OldXmlTypeM = "<Schedule><Type ID='M'><StartTime>time</StartTime><DayOfMonth>2</DayOfMonth></Type></Schedule>";
        private const string OldXmlTypeW = "<Schedule><Type ID='W'><StartTime>time</StartTime><EveryNoWeeks>2</EveryNoWeeks>" +
            "<DayOfWeek>sunday</DayOfWeek><DayOfWeek>Monday</DayOfWeek><DayOfWeek>tuesday</DayOfWeek><DayOfWeek>wednesday</DayOfWeek>" +
            "<DayOfWeek>thursday</DayOfWeek><DayOfWeek>friday</DayOfWeek><DayOfWeek>saturday</DayOfWeek></Type></Schedule>";

        [SetUp]
        public void SetupXml()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDownXml()
        {
            _shims?.Dispose();
        }

        [Test]
        public void CreateNewXMLScheduleFromOld_ForXmlTypeO_ReturnXmlDocument()
        {
            // Arrange
            _manager = new BlastManager();
            _privateObject = new PrivateObject(_manager);

            // Act
            var result = _privateObject.Invoke(MethodCreateNewXml, new object[] { OldXmlTypeO }) as string;

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Date),
                () => result.ShouldContain(Time));
        }

        [Test]
        public void CreateNewXMLScheduleFromOld_ForXmlTypeD_ReturnXmlDocument()
        {
            // Arrange
            _manager = new BlastManager();
            _privateObject = new PrivateObject(_manager);

            // Act
            var result = _privateObject.Invoke(MethodCreateNewXml, new object[] { OldXmlTypeD }) as string;

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Time));
        }

        [Test]
        public void CreateNewXMLScheduleFromOld_ForXmlTypeW_ReturnXmlDocument()
        {
            // Arrange
            _manager = new BlastManager();
            _privateObject = new PrivateObject(_manager);

            // Act
            var result = _privateObject.Invoke(MethodCreateNewXml, new object[] { OldXmlTypeW }) as string;

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Time));
        }

        [Test]
        public void CreateNewXMLScheduleFromOld_ForXmlTypeM_ReturnXmlDocument()
        {
            // Arrange
            _manager = new BlastManager();
            _privateObject = new PrivateObject(_manager);

            // Act
            var result = _privateObject.Invoke(MethodCreateNewXml, new object[] { OldXmlTypeM }) as string;

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Time));
        }
    }
}
