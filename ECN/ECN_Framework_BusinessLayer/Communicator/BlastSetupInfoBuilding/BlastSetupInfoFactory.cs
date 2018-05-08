using System;
using System.Linq;
using KM.Common;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    class BlastSetupInfoFactory
    {
        private const string RecurringBlastFrequency = "RECURRING";
        private const string RecurringScheduleType = "Schedule Recurring";
        private const string OneTimeBlastFrequency = "ONETIME";
        private const string OneTimeSheduleType = "Schedule One-Time";

        private readonly Entities.BlastSchedule _schedule;
        private readonly string BlastInfoDateFormat;

        public BlastSetupInfoFactory(Entities.BlastSchedule schedule, string blastInfoDateFormat)
        {
            _schedule = schedule;
            BlastInfoDateFormat = blastInfoDateFormat;
        }

        public Entities.BlastSetupInfo CreateRecurringSetupInfoCheckingDaysList(DateTime? sendTime)
        {
            return CreateSetupInfoCheckingDaysListInternal(RecurringScheduleType, RecurringBlastFrequency, sendTime);
        }

        public Entities.BlastSetupInfo CreateOneTimeSheduleSetupInfo()
        {
            return CreateSetupInfoCheckingDaysListInternal(
                OneTimeSheduleType,
                OneTimeBlastFrequency,
                Convert.ToDateTime(_schedule.SchedStartDate + " " + _schedule.SchedTime));
        }

        public Entities.BlastSetupInfo CreateRecurringBlastSetupInfoForDailyPeriod()
        {
            return CreateRecurringSetupInfo(Convert.ToDateTime(DateTime.Now.AddDays(1).ToString(BlastInfoDateFormat) + " " + _schedule.SchedTime));
        }

        public Entities.BlastSetupInfo CreateRecurringSetupInfo(
            Entities.BlastScheduleDays days,
            string nextDate)
        {
            return CreateBlastSetupInfoInternal(
                RecurringScheduleType,
                RecurringBlastFrequency,
                days,
                Convert.ToDateTime(nextDate + " " + _schedule.SchedTime));
        }

        public Entities.BlastSetupInfo CreateOneTimeSetupInfo(
            Entities.BlastScheduleDays days,
            string nextDate)
        {
            return CreateBlastSetupInfoInternal(
                OneTimeSheduleType,
                OneTimeBlastFrequency,
                days,
                Convert.ToDateTime(nextDate + " " + _schedule.SchedTime));
        }

        public Entities.BlastSetupInfo CreateRecurringSetupInfo(DateTime? sendTime)
        {
            var setupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = RecurringScheduleType,
                IsTestBlast = false,
                BlastScheduleID = _schedule.BlastScheduleID,
                BlastFrequency = RecurringBlastFrequency,
                SendTime = sendTime
            };

            if (_schedule.DaysList != null && _schedule.DaysList.Any())
            {
                setupInfo.SendNowIsAmount = Convert.ToBoolean(_schedule.DaysList[0].IsAmount);
                setupInfo.SendNowAmount = _schedule.DaysList[0].Total;
            }

            return setupInfo;
        }

        public Entities.BlastSetupInfo CreateSetupInfoForNextDay(DateTime? nextDateTime, Entities.BlastScheduleDays day)
        {
            return CreateBlastSetupInfoInternal(RecurringScheduleType, RecurringBlastFrequency, day, nextDateTime);
        }

        public Entities.BlastSetupInfo CreateBlastSetupInfo(
            DateTime? nextDateTime,
            Entities.BlastScheduleDays day)
        {
            return CreateBlastSetupInfoInternal(OneTimeSheduleType, OneTimeBlastFrequency, day, Convert.ToDateTime(nextDateTime));
        }

        private Entities.BlastSetupInfo CreateBlastSetupInfoInternal(
            string scheduleType,
            string blastFrequency,
            Entities.BlastScheduleDays days,
            DateTime? sendTime)
        {
            Guard.NotNull(days, nameof(days));

            return new Entities.BlastSetupInfo
            {
                ScheduleType = scheduleType,
                BlastFrequency = blastFrequency,
                IsTestBlast = false,
                BlastScheduleID = _schedule.BlastScheduleID,
                SendTime = sendTime,
                SendNowIsAmount = Convert.ToBoolean(days.IsAmount),
                SendNowAmount = days.Total
            };
        }

        private Entities.BlastSetupInfo CreateSetupInfoCheckingDaysListInternal(string ScheduleType, string BlastFrequency, DateTime? sendTime)
        {
            var setupInfo = new Entities.BlastSetupInfo
            {
                ScheduleType = ScheduleType,
                BlastFrequency = BlastFrequency,
                IsTestBlast = false,
                BlastScheduleID = _schedule.BlastScheduleID,
                SendTime = sendTime
            };

            if (_schedule.DaysList != null && _schedule.DaysList.Any() && _schedule.DaysList[0].Total != null)
            {
                setupInfo.SendNowIsAmount = Convert.ToBoolean(_schedule.DaysList[0].IsAmount);
                setupInfo.SendNowAmount = _schedule.DaysList[0].Total;
            }

            return setupInfo;
        }
    }
}
