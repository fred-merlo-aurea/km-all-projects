using System;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    class BlastSetupInfoNextDayFinder
    {
        public DayOfWeek FindSelectedDateForForOneTimePeriod(IEnumerable<Entities.BlastScheduleDays> daysList, DayOfWeek today)
        {
            Guard.NotNull(daysList, nameof(daysList));

            var next = today;
            var orderedDaysList = daysList.OrderBy(x => x.DayToSend).ToList();
            foreach (var day in orderedDaysList)
            {
                if (day.DayToSend > Convert.ToInt32(today))
                {
                    next = (DayOfWeek)day.DayToSend;
                    break;
                }
            }

            //if we don't have a higher day get the highest number day that is less than today
            if (next == today)
            {
                foreach (var day in orderedDaysList)
                {
                    if (day.DayToSend < Convert.ToInt32(today))
                    {
                        next = (DayOfWeek)day.DayToSend;
                        break;
                    }
                }
            }

            return next;
        }

        public DateTime FindSelectedDateForMonthlyPeriod(Entities.BlastScheduleDays blastScheduleDays, string scheduleTime)
        {
            Guard.NotNull(blastScheduleDays, nameof(blastScheduleDays));

            var startDate = DateTime.Now;
            var selectedDate = DateTime.Now;
            if (blastScheduleDays.DayToSend == 99)
            {
                if (startDate.Month == 12)
                {
                    var month = 1;
                    var year = startDate.Year + 1;
                    selectedDate = Convert.ToDateTime(month.ToString() + "/" + DateTime.DaysInMonth(year, month).ToString() + "/" + year.ToString() + " " + scheduleTime);
                }
                else
                {
                    var month = startDate.AddMonths(1).Month;
                    selectedDate = Convert.ToDateTime(month.ToString() + "/" + DateTime.DaysInMonth(startDate.Year, month).ToString() + "/" + startDate.Year.ToString() + " " + scheduleTime);
                }
            }
            else
            {
                var year = startDate.Year;
                var month = startDate.Month;
                for (var i = 1; i < 13; i++)
                {
                    month++;
                    if (month > 12)
                    {
                        year++;
                        month = 1;
                    }
                    if (Convert.ToInt32(blastScheduleDays.DayToSend.ToString()) <= DateTime.DaysInMonth(year, month))
                    {

                        selectedDate = Convert.ToDateTime(month.ToString() + "/" + blastScheduleDays.DayToSend.ToString() + "/" + year.ToString() + " " + scheduleTime);
                        break;
                    }
                }
            }

            return selectedDate;
        }

        public DayOfWeek FindSelectedDateForForWeeklyPeriod(IEnumerable<Entities.BlastScheduleDays> daysList, DayOfWeek today)
        {
            var next = today;

            //get the next higher number day
            var orderedDaysList = daysList.OrderBy(x => x.DayToSend).ToList();
            foreach (var day in orderedDaysList)
            {
                if (day.DayToSend > Convert.ToInt32(today))
                {
                    next = (DayOfWeek)day.DayToSend;
                    break;
                }
            }

            //if we don't have a higher day get the highest number day that is less than today
            if (next == today)
            {
                foreach (var day in orderedDaysList)
                {
                    if (day.DayToSend < Convert.ToInt32(today))
                    {
                        next = (DayOfWeek)day.DayToSend;
                        break;
                    }
                }
            }

            return next;
        }
    }
}
