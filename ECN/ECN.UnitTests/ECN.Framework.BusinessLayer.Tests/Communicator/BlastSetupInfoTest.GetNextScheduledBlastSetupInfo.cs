using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Entities = ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public partial class BlastSetupInfoTest
    {
        private const int BlastIdNotInDB = -1;
        private const int DefaultId = 1;
        private const string DefaultDateFormatForGetNextScheduledBlastSetupInfo = "MM/dd/yyyy";

        [Test]
        public void GetNextScheduledBlastSetupInfo_BlastIdNotInDB_ResturnsNull()
        {
            // Arrange           
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                return null;
            };

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(BlastIdNotInDB);

            // Assert
            blastSetupInfo.ShouldBeNull();
        }

        [Test]
        [TestCase("o", false, false, false)]
        [TestCase("o", false, true, true)]
        [TestCase("o", false, true, false)]
        [TestCase("o", true, false, false)]
        public void GetNextScheduledBlastSetupInfo_PeriodO_ReturnsBlastSetupInfo(string period, bool isNextLessThanToday, bool isNextGreaterThanToday, bool isAmount)
        {
            // Arrange
            Entities.BlastSetupInfo expectedBlastSetupInfo;
            SetupGetNextScheduledBlastSetupInfoForPeriodO(period, isNextLessThanToday, isNextGreaterThanToday, isAmount, out expectedBlastSetupInfo);

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(DefaultId);

            // Assert
            if ((!isNextGreaterThanToday && !isNextLessThanToday) || isNextLessThanToday)
            {
                blastSetupInfo.ShouldBeNull();
            }
            else
            {
                AssertBlastSetupInfo(blastSetupInfo, expectedBlastSetupInfo);
            }
        }

        [Test]
        [TestCase("d", false)]
        [TestCase("d", true)]
        public void GetNextScheduledBlastSetupInfo_PeriodD_ReturnsBlastSetupInfo(string period, bool isAmount)
        {
            // Arrange
            Entities.BlastSetupInfo expectedBlastSetupInfo;
            SetupGetNextScheduledBlastSetupInfoForPeriodD(period, isAmount, out expectedBlastSetupInfo);

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(DefaultId);

            // Assert           
            AssertBlastSetupInfo(blastSetupInfo, expectedBlastSetupInfo);
        }

        [Test]
        [TestCase("w", false, false, false)]
        [TestCase("w", false, false, true)]
        [TestCase("w", false, true, true)]
        [TestCase("w", false, true, false)]
        [TestCase("w", true, false, true)]
        [TestCase("w", true, false, false)]
        public void GetNextScheduledBlastSetupInfo_PeriodW_ReturnsBlastSetupInfo(string period, bool isNextLessThanToday, bool isNextGreaterThanToday, bool isAmount)
        {
            // Arrange
            Entities.BlastSetupInfo expectedBlastSetupInfo;
            SetupGetNextScheduledBlastSetupInfoForPeriodW(period, isNextLessThanToday, isNextGreaterThanToday, isAmount, out expectedBlastSetupInfo);

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(DefaultId);

            // Assert
            AssertBlastSetupInfo(blastSetupInfo, expectedBlastSetupInfo);
        }

        [Test]
        [TestCase("m", true, true, true)]
        [TestCase("m", true, false, false)]
        [TestCase("m", false, true, false)]
        public void GetNextScheduledBlastSetupInfo_PeriodM_ReturnsBlastSetupInfo(string period, bool isDayToSend99, bool isCurrentMonthDecember, bool isAmount)
        {
            // Arrange
            Entities.BlastSetupInfo expectedBlastSetupInfo;
            SetupGetNextScheduledBlastSetupInfoForPeriodM(period, isDayToSend99, isCurrentMonthDecember, isAmount, out expectedBlastSetupInfo);

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(DefaultId);

            // Assert
            AssertBlastSetupInfo(blastSetupInfo, expectedBlastSetupInfo);
        }

        [Test]
        public void GetNextScheduledBlastSetupInfo_PeriodNull_ReturnsNull()
        {
            // Arrange
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                return new Entities.BlastSchedule
                {
                    SchedEndDate = DateTime.Now.AddDays(1).ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo),
                    Period = null
                };
            };

            // Act
            var blastSetupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(DefaultId);

            // Assert
            blastSetupInfo.ShouldBeNull();
        }
        private void SetupGetNextScheduledBlastSetupInfoForPeriodO(string period, bool isNextLessThanToday, bool isNextGreaterThanToday, bool isAmount, out Entities.BlastSetupInfo expectedBlastSetupInfo)
        {
            const int sendNowAmount = 1;
            var today = DateTime.Now.DayOfWeek;
            var next = today;
            var schedEndDate = DateTime.Now.AddDays(2)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
            var dayToSend = (int)DateTime.Now.DayOfWeek;
            expectedBlastSetupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = "Schedule One-Time",
                IsTestBlast = false,
                BlastScheduleID = DefaultId,
                BlastFrequency = "ONETIME",
                SendNowIsAmount = isAmount,
                SendNowAmount = sendNowAmount,
            };
            if (isNextLessThanToday)
            {
                dayToSend = (int)DateTime.Now.DayOfWeek - 1;
                next = (DayOfWeek)dayToSend;
                var nextDate = DateTime.Now.AddDays(7 - Convert.ToInt32(today) + Convert.ToInt32(next))
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime(nextDate);
            }
            else if (isNextGreaterThanToday)
            {
                dayToSend = (int)DateTime.Now.DayOfWeek + 1;
                next = (DayOfWeek)dayToSend;
                var nextDate = DateTime.Now.AddDays(next - today)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime(nextDate);
            }

            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                if (blastID == BlastIdNotInDB)
                {
                    return null;
                }

                return new Entities.BlastSchedule
                {
                    SchedEndDate = schedEndDate,
                    Period = period,
                    BlastScheduleID = blastID,
                    DaysList = new List<Entities.BlastScheduleDays>
                    {
                        new Entities.BlastScheduleDays
                        {
                            DayToSend = dayToSend,
                            IsAmount = isAmount,
                            Total = sendNowAmount
                        }
                    }
                };
            };
        }
        private void SetupGetNextScheduledBlastSetupInfoForPeriodD(string period, bool isAmount, out Entities.BlastSetupInfo expectedBlastSetupInfo)
        {
            var sendNowAmount = 1;
            var today = DateTime.Now.DayOfWeek;
            var next = today;
            var schedEndDate = DateTime.Now.AddDays(2)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
            expectedBlastSetupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = "Schedule Recurring",
                IsTestBlast = false,
                BlastScheduleID = DefaultId,
                BlastFrequency = "RECURRING",
                SendTime = Convert.ToDateTime(DateTime.Now.AddDays(1)
                                                      .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo)),
                SendNowIsAmount = isAmount,
                SendNowAmount = sendNowAmount,
            };
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                if (blastID == BlastIdNotInDB)
                {
                    return null;
                }

                return new Entities.BlastSchedule
                {
                    SchedEndDate = schedEndDate,
                    Period = period,
                    BlastScheduleID = blastID,
                    DaysList = new List<Entities.BlastScheduleDays>
                    {
                        new Entities.BlastScheduleDays
                        {
                            IsAmount = isAmount,
                            Total = sendNowAmount
                        }
                    }
                };
            };
        }
        private void SetupGetNextScheduledBlastSetupInfoForPeriodW(string period, bool isNextLessThanToday, bool isNextGreaterThanToday, bool isAmount, out Entities.BlastSetupInfo expectedBlastSetupInfo)
        {
            var sendNowAmount = 1;
            var weeks = 2;
            var scheduledEndDate = DateTime.Now.AddDays(weeks * 7)
                                           .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
            var today = DateTime.Now.DayOfWeek;
            var next = today;
            int dayToSend;
            expectedBlastSetupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = "Schedule Recurring",
                IsTestBlast = false,
                BlastScheduleID = DefaultId,
                BlastFrequency = "RECURRING",
                SendNowIsAmount = isAmount,
                SendNowAmount = sendNowAmount,
            };
            if (isNextLessThanToday)
            {
                dayToSend = (int)DateTime.Now.DayOfWeek - 1;
                next = (DayOfWeek)dayToSend;
                var nextDate = DateTime.Now
                                       .AddDays(7 - Convert.ToInt32(today) + Convert.ToInt32(next) + (Convert.ToInt32(weeks) * 7) - 7)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime(nextDate);
            }
            else if (isNextGreaterThanToday)
            {
                dayToSend = (int)DateTime.Now.DayOfWeek + 1;
                next = (DayOfWeek)dayToSend;
                var nextDate = DateTime.Now.AddDays(next - today)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime(nextDate);
            }
            else
            {
                dayToSend = (int)DateTime.Now.DayOfWeek;
                var nextDate = DateTime.Now.AddDays(7 * weeks)
                                       .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime(nextDate);
            }
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                if (blastID == BlastIdNotInDB)
                {
                    return null;
                }

                return new Entities.BlastSchedule
                {
                    SchedEndDate = scheduledEndDate,
                    Period = period,
                    BlastScheduleID = blastID,
                    DaysList = new List<Entities.BlastScheduleDays>
                    {
                        new Entities.BlastScheduleDays
                        {
                            DayToSend = dayToSend,
                            IsAmount = isAmount,
                            Total = sendNowAmount,
                            Weeks = weeks
                        }
                    }
                };
            };
        }
        private void SetupGetNextScheduledBlastSetupInfoForPeriodM(string period, bool isDayToSend99, bool isCurrentMonthDecember, bool isAmount, out Entities.BlastSetupInfo expectedBlastSetupInfo)
        {
            var sendNowAmount = 1;
            var weeks = 2;
            var scheduledEndDate = DateTime.Now.AddDays(weeks * 7)
                                           .ToString(DefaultDateFormatForGetNextScheduledBlastSetupInfo);
            var today = DateTime.Now.DayOfWeek;
            var next = today;
            var dayToSend = 1;
            expectedBlastSetupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = "Schedule Recurring",
                IsTestBlast = false,
                BlastScheduleID = DefaultId,
                BlastFrequency = "RECURRING",
                SendNowIsAmount = isAmount,
                SendNowAmount = sendNowAmount,
            };
            ShimDateTime.NowGet = () =>
            {
                if (isCurrentMonthDecember)
                {
                    return new DateTime(2000, 12, 1);
                }
                else
                {
                    return new DateTime(2000, 1, 1);
                }
            };

            if (isDayToSend99)
            {
                dayToSend = 99;
                if (isCurrentMonthDecember)
                {
                    var month = 1;
                    var startDate = DateTime.Now;
                    var year = startDate.Year + 1;
                    expectedBlastSetupInfo.SendTime = Convert.ToDateTime($"{month.ToString()}/{DateTime.DaysInMonth(year, month).ToString()}/{year.ToString()}");
                }
                else
                {
                    var startDate = DateTime.Now;
                    var month = startDate.AddMonths(1).Month;
                    expectedBlastSetupInfo.SendTime = Convert.ToDateTime($"{month.ToString()}/{DateTime.DaysInMonth(startDate.Year, month).ToString()}/{startDate.Year.ToString()}");
                }
            }
            else
            {
                dayToSend = 10;
                var startDate = DateTime.Now;
                var year = startDate.Year + 1;
                var month = 1;
                expectedBlastSetupInfo.SendTime = Convert.ToDateTime($"{month.ToString()}/{dayToSend.ToString()}/{year.ToString()}");
            }

            ShimBlastSchedule.GetByBlastIDInt32Boolean = (blastID, getChildren) =>
            {
                if (blastID == BlastIdNotInDB)
                {
                    return null;
                }

                return new Entities.BlastSchedule
                {
                    SchedEndDate = scheduledEndDate,
                    Period = period,
                    BlastScheduleID = blastID,
                    DaysList = new List<Entities.BlastScheduleDays>
                    {
                        new Entities.BlastScheduleDays
                        {
                            DayToSend = dayToSend,
                            IsAmount = isAmount,
                            Total = sendNowAmount,
                            Weeks = weeks
                        }
                    }
                };
            };
        }
        private void AssertBlastSetupInfo(Entities.BlastSetupInfo resultBlastSetupInfo, Entities.BlastSetupInfo expectedBlastSetupInfo)
        {
            resultBlastSetupInfo.BlastScheduleID.ShouldSatisfyAllConditions(
                   () => resultBlastSetupInfo.ScheduleType.ShouldBe(expectedBlastSetupInfo.ScheduleType),
                   () => resultBlastSetupInfo.IsTestBlast.ShouldBe(expectedBlastSetupInfo.IsTestBlast),
                   () => resultBlastSetupInfo.BlastScheduleID.ShouldBe(expectedBlastSetupInfo.BlastScheduleID),
                   () => resultBlastSetupInfo.BlastFrequency.ShouldBe(expectedBlastSetupInfo.BlastFrequency),
                   () => resultBlastSetupInfo.SendNowIsAmount.ShouldBe(expectedBlastSetupInfo.SendNowIsAmount),
                   () => resultBlastSetupInfo.SendNowAmount.ShouldBe(expectedBlastSetupInfo.SendNowAmount),
                   () => resultBlastSetupInfo.SendTime.ShouldBe(expectedBlastSetupInfo.SendTime)
               );
        }
    }
}