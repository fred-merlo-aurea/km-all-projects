using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Entities = ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastSetupInfoTest
    {
        private IDisposable _shimObject;

        private Entities::BlastSchedule GetBlastSchedule(
            string schedStartDate, 
            string period, 
            string schedTime, 
            string schedEndDate,
            int? dayToSend,
            int? total,
            bool? isAmount)
        {
            return new Entities::BlastSchedule
            {
                BlastScheduleID = 1,
                SchedStartDate = schedStartDate,
                Period = period,
                SchedTime = schedTime,
                SchedEndDate = schedEndDate,
                DaysList = new List<Entities::BlastScheduleDays>
                {
                    new Entities::BlastScheduleDays
                    {
                        DayToSend = dayToSend,
                        Total = total,
                        IsAmount = isAmount
                    }
                }
            };
        }

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
        public void GetNextScheduledBlastSetupInfo_ScheduleIsNull_ReturnNull()
        {
            // Arrange
            var blastScheduleID = 1;
            var includeToday = true;
            ShimBlastSchedule.GetByBlastScheduleIDInt32Boolean = (id, child) => null;

            // Act	
            var actualResult = BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, includeToday);

            // Assert
            actualResult.ShouldBeNull();
        }

        [Test]
        [TestCase("o", "2018/1/1", "2020/1/1", true, null, 1, true)]
        [TestCase("o", "2018/1/1", "2020/1/1", true, null, 1, false)]
        [TestCase("o", "2018/1/1", "2020/1/1", false, null, 1, false)]
        [TestCase("o", "2018/1/1", "2020/1/1", false, 3, null, false)]
        [TestCase("o", "2018/1/1", "2020/1/1", false, 4, null, true)]
        [TestCase("o", "2018/1/1", "2020/1/1", false, 1, null, true)]
        [TestCase("d", "2018/1/1", "2020/1/1", false, 1, null, true)]
        [TestCase("d", "2018/1/1", "2020/1/1", false, 1, null, false)]
        [TestCase("w", "2018/1/1", "2020/1/1", false, 3, null, false)]
        [TestCase("w", "2018/1/1", "2020/1/1", false, 4, null, true)]
        [TestCase("w", "2018/1/1", "2020/1/1", false, 1, null, true)]
        [TestCase("m", "2018/12/1", "2020/1/1", false, 1, 1, true)]
        [TestCase("m", "2018/1/1", "2020/1/1", false, 99, 1, false)]
        public void GetNextScheduledBlastSetupInfo_MultipleCases_ReturnBlastSetupInfo(
            string period,
            string startDate,
            string endDate,
            bool includeToday,
            int? dayToSend,
            int? total,
            bool? isAmount)
        {
            // Arrange
            var blastScheduleID = 1;
            var schedTime = DateTime.Now.ToShortTimeString();
            ShimBlastSchedule.GetByBlastScheduleIDInt32Boolean = (id, child) =>
                GetBlastSchedule(startDate, period, schedTime, endDate, dayToSend, total, isAmount);
            ShimDateTime.NowGet = () => new DateTime(2018, 3, 13);

            // Act	
            var actualResult = BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, includeToday);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.BlastScheduleID.ShouldBe(blastScheduleID);
        }

        [Test]
        public void GetNextScheduledBlastSetupInfo_DateNowIsSmaller_ReturnBlastSetupInfo()
        {
            // Arrange
            var blastScheduleID = 1;
            var schedTime = "10:00 AM";
            ShimBlastSchedule.GetByBlastScheduleIDInt32Boolean = (id, child) =>
                GetBlastSchedule("2018/1/1", "d", schedTime, "2020/1/1", 1, null, false);
            ShimDateTime.NowGet = () => new DateTime(2018, 2, 13, 11, 0, 0);

            // Act	
            var actualResult = BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, true);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.BlastScheduleID.ShouldBe(blastScheduleID);
        }

        [Test]
        public void GetNextScheduledBlastSetupInfo_InvalidCase_ReturnBlastSetupInfo()
        {
            // Arrange
            var blastScheduleID = 1;
            var schedTime = "10:00 AM";
            ShimBlastSchedule.GetByBlastScheduleIDInt32Boolean = (id, child) =>
                GetBlastSchedule("2018/1/1", "InvalidCase", schedTime, "2020/1/1", 1, null, false);

            // Act	
            var actualResult = BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, true);

            // Assert
            actualResult.ShouldBeNull();
        }

        [Test]
        public void GetNextScheduledBlastSetupInfo_MonthAndDateNowIsSmaller_ReturnBlastSetupInfo()
        {
            // Arrange
            var blastScheduleID = 1;
            var schedTime = "10:00 AM";
            ShimBlastSchedule.GetByBlastScheduleIDInt32Boolean = (id, child) =>
                GetBlastSchedule("2018/12/31", "m", schedTime, "2020/1/1", 2, 1, false);
            ShimDateTime.NowGet = () => new DateTime(2018, 2, 12, 11, 0, 0);

            // Act	
            var actualResult = BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, true);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.BlastScheduleID.ShouldBe(blastScheduleID);
        }
    }
}
