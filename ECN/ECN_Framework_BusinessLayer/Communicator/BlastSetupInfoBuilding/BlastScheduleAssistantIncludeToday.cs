using System;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    class BlastScheduleAssistantIncludeToday : IBlastScheduleAssistant
    {
        private readonly Entities.BlastSchedule _schedule;
        private readonly BlastSetupInfoFactory _setupInfoFactory;
        private readonly BlastSetupInfoNextDayFinderIncludingToday _nextDayFinder;
        private const string BlastInfoDateFormat = "MM/dd/yyyy";

        public BlastScheduleAssistantIncludeToday(
            Entities.BlastSchedule schedule,
            DateTime startDateTime)
        {
            _schedule = schedule;
            _setupInfoFactory = new BlastSetupInfoFactory(_schedule, BlastInfoDateFormat);
            _nextDayFinder = new BlastSetupInfoNextDayFinderIncludingToday(
                startDateTime,
                schedule,
                BlastInfoDateFormat);
        }

        public Entities.BlastSetupInfo PrepareMonthlySetupInfo()
        {
            var selectedDate = _nextDayFinder.FindSelectedDateForMonthlyPeriod();

            if (selectedDate < Convert.ToDateTime(_schedule.SchedEndDate + " " + _schedule.SchedTime))
            {
                return _setupInfoFactory.CreateRecurringSetupInfoCheckingDaysList(selectedDate);
            }

            return null;
        }

        public Entities.BlastSetupInfo PrepareWeeklySetupInfo()
        {
            Entities.BlastSetupInfo setupInfo = null;
            if (_schedule.DaysList != null && _schedule.DaysList.Count > 0 && _schedule.DaysList[0].DayToSend != null)
            {
                var nextDateTime = _nextDayFinder.FindSelectedDateForWeeklyPeriod();
                //we found an item to schedule
                if (nextDateTime != null && nextDateTime < Convert.ToDateTime(_schedule.SchedEndDate + " " + _schedule.SchedTime))
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == Convert.ToInt32(Convert.ToDateTime(nextDateTime).DayOfWeek))
                        {
                            setupInfo = _setupInfoFactory.CreateSetupInfoForNextDay(nextDateTime, day);
                            break;
                        }
                    }
                }
            }

            return setupInfo;
        }

        public Entities.BlastSetupInfo PrepareDailySetupInfo()
        {
            var nextDateTime = _nextDayFinder.FindSelectedDateForDailyPeriod();

            if (Convert.ToDateTime(_schedule.SchedEndDate + " " + _schedule.SchedTime) > Convert.ToDateTime(nextDateTime))
            {
                return _setupInfoFactory.CreateRecurringSetupInfo(nextDateTime);
            }

            return null;
        }

        public Entities.BlastSetupInfo PrepareOneTimeSetupInfo()
        {
            Entities.BlastSetupInfo setupInfo = null;
            if (_schedule.DaysList != null && _schedule.DaysList.Count > 0 && _schedule.DaysList[0].DayToSend != null)
            {
                var nextDateTime = _nextDayFinder.FindSelectedDateForOneTimePeriod();

                //we found an item to schedule
                if (nextDateTime != null && nextDateTime < Convert.ToDateTime(_schedule.SchedEndDate + " " + _schedule.SchedTime))
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == Convert.ToInt32(Convert.ToDateTime(nextDateTime).DayOfWeek))
                        {
                            setupInfo = _setupInfoFactory.CreateBlastSetupInfo(nextDateTime, day);
                            break;
                        }
                    }
                }
            }
            else
            {
                //single day
                setupInfo = _setupInfoFactory.CreateOneTimeSheduleSetupInfo();
            }

            return setupInfo;
        }

    }
}
