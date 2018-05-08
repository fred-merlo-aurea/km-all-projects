using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class SimpleBlastSchedule
    {
        public enum ScheduleType { OneTime, Recurring }
        public enum RecurrenceType { OneTime, Daily, Weekly, Monthly }
        public enum SplitType {  Evenly }
        public enum DayOfWeek {  Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
        public ScheduleType? Type { get; set; }
        public RecurrenceType? Recurrence { get; set; }
        public SplitType? Split { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? HowManyWeeks { get; set; }
        public int? DayOfMonth { get; set; }
        public DayOfWeek? Day { get; set; }

        public override string ToString()
        { // recurrence 
            RecurrenceType recurrence = Recurrence.HasValue ? Recurrence.Value : RecurrenceType.OneTime;
            DateTime startDate = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddSeconds(15);
            DateTime endDate = EndDate.HasValue ? EndDate.Value : DateTime.Now.AddYears(25);
            DayOfWeek dayOfWeek = Day.HasValue ? Day.Value : DayOfWeek.Monday;
            SplitType splitType = Split.HasValue ? Split.Value : SplitType.Evenly;
            int dayOfMonth = DayOfMonth.HasValue ? DayOfMonth.Value : 1;
            int howManyWeeks = HowManyWeeks.HasValue ? HowManyWeeks.Value : 56 * 25;

            switch(Type)
            {
                case SimpleBlastSchedule.ScheduleType.Recurring:
                    switch (recurrence)
                    {
                        case RecurrenceType.Daily:
                            return String.Format( "<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>"
                                                + "<RecurrenceType ID=\"Daily\"/></ScheduleType></Schedule>", 
                                                startDate.ToShortDateString(),startDate.ToString("HH:mm:ss"), EndDate.Value.ToShortDateString());
                        case RecurrenceType.Weekly:
                            return 
                              String.Format( "<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>"
                                           + "<RecurrenceType ID=\"Weekly\"><HowManyWeeks>{3}</HowManyWeeks><SplitType ID=\"Evenly\"><Days><Day ID=\"{4}\"></Day></Days></SplitType>"
                                           + "</RecurrenceType></ScheduleType></Schedule>"
                                           , startDate.ToShortDateString(), startDate.ToString("HH:mm:ss"), EndDate.Value.ToShortDateString(), howManyWeeks, dayOfWeek);
                        case RecurrenceType.Monthly:
                            return 
                                String.Format( "<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>"
                                             + "<RecurrenceType ID=\"Monthly\"><DayOfMonth>{3}</DayOfMonth></RecurrenceType></ScheduleType></Schedule>"
                                             , startDate.ToShortDateString(), startDate.ToString("HH:mm:ss"), EndDate.Value.ToShortDateString(), dayOfMonth);
                    }
                    break;
            }
            
            return String.Format(
                "<Schedule><ScheduleType ID=\"OneTime\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><SplitType ID=\"SingleDay\"/></ScheduleType></Schedule>",
                startDate.ToShortDateString(), startDate.ToString("HH:mm:ss"));
        }
    }
}