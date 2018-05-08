using System;
using System.Linq;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    class BlastSetupInfoNextDayFinderIncludingToday
    {
        private readonly string _blastInfoDateFormat;
        private readonly DateTime _startDateTime;
        private readonly Entities.BlastSchedule _schedule;

        public BlastSetupInfoNextDayFinderIncludingToday(DateTime startDateTime, Entities.BlastSchedule schedule, string blastInfoDateFormat)
        {
            _startDateTime = startDateTime;
            _schedule = schedule;
            _blastInfoDateFormat = blastInfoDateFormat;
        }

        public DateTime? FindSelectedDateForDailyPeriod()
        {
            DateTime? nextDateTime = null;
            if (Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime) >= _startDateTime)
            {
                nextDateTime = Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
            }
            else
            {
                nextDateTime = Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime).AddDays(1);
            }

            return nextDateTime;
        }

        public DateTime? FindSelectedDateForWeeklyPeriod()
        {
            DateTime? nextDateTime = null;
            //there are multiple days
            var today = Convert.ToInt32(_startDateTime.DayOfWeek);

            var daysList = _schedule.DaysList.OrderBy(x => x.DayToSend).ToList();
            foreach (var day in daysList)
            {
                //if today is in the schedule
                if (day.DayToSend == today)
                {
                    //if schedule time > current time
                    if (Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime) >= _startDateTime)
                    {
                        //use today
                        nextDateTime = Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                        break;
                    }
                }
                //get the first day in the schedule after today
                else if (day.DayToSend > Convert.ToInt32(today))
                {
                    //use next higher day
                    nextDateTime = Convert.ToDateTime(_startDateTime.AddDays(Convert.ToInt32(day.DayToSend) - today).ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                    break;
                }
            }
            //if the schedule doesn't include today or a higher day of the week, find the lowest day of the week
            if (nextDateTime == null)
            {
                foreach (var day in daysList)
                {
                    if (day.DayToSend < Convert.ToInt32(today))
                    {
                        //use lowest day of next week
                        nextDateTime = Convert.ToDateTime(_startDateTime.AddDays(7 - today + Convert.ToInt32(day.DayToSend) + (Convert.ToInt32(day.Weeks) * 7) - 7).ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                        break;
                    }
                }
            }

            return nextDateTime;
        }

        public DateTime? FindSelectedDateForMonthlyPeriod()
        {
            DateTime? nextDateTime = null;
            var month = _startDateTime.Month;
            var year = _startDateTime.Year;
            if (_schedule.DaysList[0].DayToSend == 99)
            {
                var day = DateTime.DaysInMonth(year, month);
                nextDateTime = Convert.ToDateTime(month.ToString() + "/" + day.ToString() + "/" + year.ToString() + " " + _schedule.SchedTime.ToString());
                if (nextDateTime < _startDateTime)
                {
                    if (Convert.ToDateTime(nextDateTime).Month == 12)
                    {
                        month = 1;
                        year = Convert.ToDateTime(nextDateTime).Year + 1;
                    }
                    else
                    {
                        month = Convert.ToDateTime(nextDateTime).Month + 1;
                        year = Convert.ToDateTime(nextDateTime).Year;
                    }
                    day = DateTime.DaysInMonth(year, month);
                    nextDateTime = Convert.ToDateTime(month.ToString() + "/" + day.ToString() + "/" + year.ToString() + " " + _schedule.SchedTime.ToString());
                }
            }
            else
            {
                var day = Convert.ToInt32(_schedule.DaysList[0].DayToSend);
                if (day <= DateTime.DaysInMonth(year, month) && Convert.ToDateTime(month.ToString() + "/" + day.ToString() + "/" + year.ToString() + " " + _schedule.SchedTime.ToString()) >= _startDateTime)
                {
                    nextDateTime = Convert.ToDateTime(month.ToString() + "/" + day.ToString() + "/" + year.ToString() + " " + _schedule.SchedTime.ToString());
                }
                else
                {
                    for (var i = 1; i < 13; i++)
                    {
                        month++;
                        if (month > 12)
                        {
                            year++;
                            month = 1;
                        }
                        if (day <= DateTime.DaysInMonth(year, month))
                        {
                            nextDateTime = Convert.ToDateTime(month.ToString() + "/" + day.ToString() + "/" + year.ToString() + " " + _schedule.SchedTime.ToString());
                            break;
                        }
                    }
                }
            }

            return nextDateTime;
        }

        public DateTime? FindSelectedDateForOneTimePeriod()
        {
            DateTime? nextDateTime = null;
            //there are multiple days
            var today = Convert.ToInt32(_startDateTime.DayOfWeek);

            var daysList = _schedule.DaysList.OrderBy(x => x.DayToSend).ToList();
            foreach (var day in daysList)
            {
                //if today is in the schedule
                if (day.DayToSend == today)
                {
                    //if schedule time > current time
                    if (Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime) >= _startDateTime)
                    {
                        //use today
                        nextDateTime = Convert.ToDateTime(_startDateTime.ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                        break;
                    }
                }
                //get the first day in the schedule after today
                else if (day.DayToSend > Convert.ToInt32(today))
                {
                    //use next higher day
                    nextDateTime = Convert.ToDateTime(_startDateTime.AddDays(Convert.ToInt32(day.DayToSend) - today).ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                    break;
                }
            }
            //if the schedule doesn't include today or a higher day of the week, find the lowest day of the week
            if (nextDateTime == null)
            {
                foreach (var day in daysList)
                {
                    if (day.DayToSend < Convert.ToInt32(today))
                    {
                        //use lowest day of next week
                        nextDateTime = Convert.ToDateTime(_startDateTime.AddDays(7 - today + Convert.ToInt32(day.DayToSend)).ToString(_blastInfoDateFormat) + " " + _schedule.SchedTime);
                        break;
                    }
                }
            }

            return nextDateTime;
        }
    }
}
