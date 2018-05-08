using System;
using System.Linq;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    class BlastScheduleAssistant : IBlastScheduleAssistant
    {
        private const string BlastInfoDateFormat = "MM/dd/yyyy";
        private readonly Entities.BlastSchedule _schedule;
        private readonly BlastSetupInfoNextDayFinder _nextDayFinder;
        private readonly BlastSetupInfoFactory _setupInfoFactory;

        public BlastScheduleAssistant(
            Entities.BlastSchedule schedule)
        {
            _schedule = schedule;
            _nextDayFinder = new BlastSetupInfoNextDayFinder();
            _setupInfoFactory = new BlastSetupInfoFactory(_schedule, BlastInfoDateFormat);
        }

        public Entities.BlastSetupInfo PrepareMonthlySetupInfo()
        {
            var selectedDate = _nextDayFinder.FindSelectedDateForMonthlyPeriod(_schedule.DaysList[0], _schedule.SchedTime);

            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(selectedDate.ToString(BlastInfoDateFormat)))
            {
                return _setupInfoFactory.CreateRecurringSetupInfoCheckingDaysList(selectedDate);
            }

            return null;
        }

        public Entities.BlastSetupInfo PrepareWeeklySetupInfo()
        {
            Entities.BlastSetupInfo setupInfo = null;

            if (_schedule.DaysList != null && _schedule.DaysList.Any() && _schedule.DaysList[0].DayToSend.HasValue)
            {
                var today = DateTime.Now.DayOfWeek;
                var next = _nextDayFinder.FindSelectedDateForForWeeklyPeriod(_schedule.DaysList, today);

                //we found an item to schedule in the next x number of weeks
                if (next < today)
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == (int)next)
                        {
                            var nextDate = string.Empty;
                            nextDate = DateTime.Now.AddDays(7 - (int)today + (int)next + ((int)day.Weeks * 7) - 7).ToString(BlastInfoDateFormat);

                            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(Convert.ToDateTime(nextDate + " " + _schedule.SchedTime).ToString(BlastInfoDateFormat)))
                            {
                                setupInfo = _setupInfoFactory.CreateRecurringSetupInfo(day, nextDate);
                                break;
                            }
                        }
                    }
                }

                //we found an item to schedule this week
                if (next > today)
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == (int)next)
                        {
                            var nextDate = string.Empty;
                            if (next > today)
                            {
                                nextDate = DateTime.Now.AddDays(next - today).ToString(BlastInfoDateFormat);
                            }
                            else
                            {
                                nextDate = DateTime.Now.AddDays(7 - (int)today + (int)next).ToString(BlastInfoDateFormat);
                            }

                            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(Convert.ToDateTime(nextDate + " " + _schedule.SchedTime).ToString(BlastInfoDateFormat)))
                            {
                                setupInfo = _setupInfoFactory.CreateRecurringSetupInfo(day, nextDate);
                                break;
                            }
                        }
                    }
                }

                //adding this so a weekly recurring blast set up to only go out 1 day a week gets scheduled JWelter 03032015
                //if next is still equal to today, that means there is only 1 day the blast is set up to recur on.
                if (next == today)
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == (int)next)
                        {
                            var nextDate = DateTime.Now.AddDays(7 * day.Weeks.Value).ToString(BlastInfoDateFormat);

                            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(Convert.ToDateTime(nextDate + " " + _schedule.SchedTime).ToString(BlastInfoDateFormat)))
                            {
                                setupInfo = _setupInfoFactory.CreateRecurringSetupInfo(day, nextDate);
                                break;
                            }
                        }
                    }
                }
            }

            return setupInfo;
        }

        public Entities.BlastSetupInfo PrepareDailySetupInfo()
        {
            Entities.BlastSetupInfo setupInfo = null;
            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(DateTime.Now.AddDays(1).ToString(BlastInfoDateFormat)))
            {
                setupInfo = _setupInfoFactory.CreateRecurringBlastSetupInfoForDailyPeriod();
            }

            return setupInfo;
        }

        public Entities.BlastSetupInfo PrepareOneTimeSetupInfo()
        {
            Entities.BlastSetupInfo setupInfo = null;
            if (_schedule.DaysList != null && _schedule.DaysList.Any() && _schedule.DaysList[0].DayToSend.HasValue)
            {
                //there are multiple days
                var today = DateTime.Now.DayOfWeek;
                var next = _nextDayFinder.FindSelectedDateForForOneTimePeriod(_schedule.DaysList, today);

                //we found an item to schedule
                if (next != today)
                {
                    foreach (var day in _schedule.DaysList)
                    {
                        if (day.DayToSend == (int)next)
                        {
                            var nextDate = string.Empty;
                            if (next > today)
                            {
                                nextDate = DateTime.Now.AddDays(next - today).ToString(BlastInfoDateFormat);
                            }
                            else
                            {
                                nextDate = DateTime.Now.AddDays(7 - (int)today + (int)next).ToString(BlastInfoDateFormat);
                            }

                            //get the info
                            if (Convert.ToDateTime(_schedule.SchedEndDate) >= Convert.ToDateTime(Convert.ToDateTime(nextDate + " " + _schedule.SchedTime).ToString(BlastInfoDateFormat)))
                            {
                                setupInfo = _setupInfoFactory.CreateOneTimeSetupInfo(day, nextDate);
                                break;
                            }
                        }
                    }
                }
            }

            return setupInfo;
        }
    }
}
