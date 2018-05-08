using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BlastScheduleTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
           _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void CreateScheduleFromXML_EmptyXML_ReturnNull()
        {
            // Arrange
            const string XmlString = "<xml/>";
            var userId = 1;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(XmlString, userId);

            // Assert
            actualResult.ShouldBeNull();
        }

        [Test]
        public void CreateScheduleFromXML_SendNowNotNull_ReturnBlastSchedule()
        {
            // Arrange
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"SendNow\">");
            xmlString.Append("<SplitType ID=\"SingleDay\">");
            xmlString.Append("<Child>TestValue</Child>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append("<SplitType ID=\"SingleDay\">");
            xmlString.Append("<Amount>1</Amount>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.DaysList.ShouldNotBeEmpty(),
                () => actualResult.DaysList.Count.ShouldBeGreaterThan(0)
            );
        }

        [Test]
        public void CreateScheduleFromXML_OneTimeNotNullAndSingleDay_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append("<SplitType ID=\"SingleDay\">");
            xmlString.Append("<Child>TestValue</Child>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append("<SplitType ID=\"SingleDay\">");
            xmlString.Append("<Amount>1</Amount>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_OneTimeNotNullAndEvenly_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append("<SplitType ID=\"Evenly\">");
            xmlString.Append("<Days>");
            xmlString.Append("<Day ID=\"1\" />");
            xmlString.Append("<Day ID=\"2\" />");
            xmlString.Append("</Days>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_OneTimeNotNullAndManually_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"OneTime\">");
            xmlString.Append("<SplitType ID=\"Manually\">");
            xmlString.Append("<Days>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("<Day ID=\"1\">");
            xmlString.Append("<Total>1</Total>");
            xmlString.Append("</Day>");
            xmlString.Append("</Days>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_RecurringNotNullAndDaily_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append($"<EndDate>{datetime.AddMonths(2).ToShortDateString()}</EndDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append("<RecurrenceType ID=\"Daily\">");
            xmlString.Append("<Amount>1</Amount>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("</RecurrenceType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_RecurringNotNullAndWeeklyEvenly_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append($"<EndDate>{datetime.AddMonths(2).ToShortDateString()}</EndDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append("<RecurrenceType ID=\"Weekly\">");
            xmlString.Append("<HowManyWeeks>1</HowManyWeeks>");
            xmlString.Append("<SplitType ID=\"Evenly\">");
            xmlString.Append("<Days>");
            xmlString.Append("<Day ID=\"1\" />");
            xmlString.Append("<Day ID=\"2\" />");
            xmlString.Append("</Days>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</RecurrenceType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_RecurringNotNullAndWeeklyManually_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append($"<EndDate>{datetime.AddMonths(2).ToShortDateString()}</EndDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append("<RecurrenceType ID=\"Weekly\">");
            xmlString.Append("<HowManyWeeks>1</HowManyWeeks>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("<SplitType ID=\"Manually\">");
            xmlString.Append("<Days>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("<Day ID=\"1\">");
            xmlString.Append("<Total>1</Total>");
            xmlString.Append("</Day>");
            xmlString.Append("</Days>");
            xmlString.Append("</SplitType>");
            xmlString.Append("</RecurrenceType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }

        [Test]
        public void CreateScheduleFromXML_RecurringNotNullAndMonthly_ReturnBlastSchedule()
        {
            // Arrange
            var datetime = DateTime.Now.AddDays(2);
            var xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append($"<StartTime>{datetime.ToShortTimeString()}</StartTime>");
            xmlString.Append($"<StartDate>{datetime.ToShortDateString()}</StartDate>");
            xmlString.Append($"<EndDate>{datetime.AddMonths(2).ToShortDateString()}</EndDate>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("<Schedule>");
            xmlString.Append("<ScheduleType ID=\"Recurring\">");
            xmlString.Append("<RecurrenceType ID=\"Monthly\">");
            xmlString.Append("<DayOfMonth>1</DayOfMonth>");
            xmlString.Append("<Amount>1</Amount>");
            xmlString.Append("<IsAmount>True</IsAmount>");
            xmlString.Append("</RecurrenceType>");
            xmlString.Append("</ScheduleType>");
            xmlString.Append("</Schedule>");
            xmlString.Append("</xml>");
            var userId = 1;
            ShimBlastSchedule.InsertBlastSchedule = (sch) => 1;
            var blastSchedule = new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                CreatedBy = 1
            };
            ShimBlastSchedule.GetByBlastScheduleIDInt32 = (id) => blastSchedule;

            // Act	
            var actualResult = BlastSchedule.CreateScheduleFromXML(xmlString.ToString(), userId);

            // Assert
            actualResult.ShouldBe(blastSchedule);
        }
    }
}
