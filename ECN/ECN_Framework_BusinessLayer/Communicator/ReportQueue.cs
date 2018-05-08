using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECN_Framework_Common.Objects;
using EntityReportSchedule = ECN_Framework_Entities.Communicator.ReportSchedule;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ReportQueue
    {
        private const string ScheduleTypeRecurring = "Recurring";
        private const string ScheduleTypeOneTime = "One-Time";
        private const string RecurrenceTypeDaily = "daily";
        private const string RecurrenceTypeWeekly = "weekly";
        private const string RecurrenceTypeMonthly = "monthly";
        private static readonly int[] DayShiftValues = { 7, 6, 5, 4, 3, 2, 1 };

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ReportQueue;

        public static List<ECN_Framework_Entities.Communicator.ReportQueue> GetReportHistory(int ReportScheduleID)
        {
            List<ECN_Framework_Entities.Communicator.ReportQueue> retList = new List<ECN_Framework_Entities.Communicator.ReportQueue>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.ReportQueue.GetReportHistory(ReportScheduleID);
                scope.Complete();
            }
            return retList;

        }

        public static ECN_Framework_Entities.Communicator.ReportQueue GetNextPendingForReportSchedule(int ReportScheduleID)
        {
            ECN_Framework_Entities.Communicator.ReportQueue rq = null;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rq = ECN_Framework_DataLayer.Communicator.ReportQueue.GetNextPendingForReportSchedule(ReportScheduleID);
                scope.Complete();
            }
            return rq;
        }

        public static void UpdateStatus(int RQID, string Status,string FailureReason)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ReportQueue.UpdateStatus(RQID, Status,FailureReason);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.ReportQueue rq, bool ASendTimeUpdate)
        {
            int retID = -1;
            bool valid = ASendTimeUpdate ? true : Validate(rq);
            if (valid || rq.Status.ToLower().Equals("sent"))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    retID = ECN_Framework_DataLayer.Communicator.ReportQueue.Save(rq);
                    scope.Complete();
                }
            }
            return retID;
        }

        

        private static bool Validate(ECN_Framework_Entities.Communicator.ReportQueue rq)
        {
            bool retBool = true;

            if(Exists(rq))
            {
                retBool = false;
            }


            return retBool;
        }

        private static bool Exists(ECN_Framework_Entities.Communicator.ReportQueue rq)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ReportQueue.Exists(rq);
                scope.Complete();
            }
            return exists;
        }

        public static void Resend(int ReportQueueID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ReportQueue.ResendReport(ReportQueueID);
                scope.Complete();
            }
        }

        public static void Delete_ReportScheduleID(int ReportScheduleID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ReportQueue.Delete_ReportScheduleID(ReportScheduleID);
                scope.Complete();
            }
        }

        public static ECN_Framework_Entities.Communicator.ReportQueue GetNextToSend(int? blastid)
        {
            ECN_Framework_Entities.Communicator.ReportQueue rq = new ECN_Framework_Entities.Communicator.ReportQueue();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rq = ECN_Framework_DataLayer.Communicator.ReportQueue.GetNextToSend(blastid);
                scope.Complete();
            }
            return rq;
        }

        public static DateTime? GetNextDateToRun(EntityReportSchedule reportSchedule, bool isNew)
        {
            DateTime? dateToSend = null;

            DateTime start;
            DateTime end;
            DateTime.TryParse(reportSchedule.StartDate, out start);
            DateTime.TryParse(reportSchedule.EndDate, out end);

            if (reportSchedule.ScheduleType.Equals(ScheduleTypeRecurring))
            {
                if (reportSchedule.RecurrenceType.Equals(RecurrenceTypeDaily, StringComparison.OrdinalIgnoreCase)
                    && end.Date >= DateTime.Now.Date)
                {
                    dateToSend = DateToSendRecurrenceDaily(reportSchedule, isNew, end);
                }
                else if (reportSchedule.RecurrenceType.Equals(RecurrenceTypeWeekly, StringComparison.OrdinalIgnoreCase) 
                         && end.Date >= DateTime.Now.Date)
                {
                    dateToSend = DateToSendRecurrenceWeekly(reportSchedule, isNew, dateToSend, start, end);
                }
                else if (reportSchedule.RecurrenceType.Equals(RecurrenceTypeMonthly, StringComparison.OrdinalIgnoreCase) 
                         && end.Date >= DateTime.Now.Date)
                {
                    dateToSend = DateToSendRecurrenceMonthly(reportSchedule, start, end);
                }
            }
            else if (reportSchedule.ScheduleType.Equals(ScheduleTypeOneTime))
            {
                dateToSend = Convert.ToDateTime($"{reportSchedule.StartDate} {reportSchedule.StartTime}");
            }

            return dateToSend;
        }

        private static DateTime? DateToSendRecurrenceWeekly(
            EntityReportSchedule reportSchedule,
            bool isNew,
            DateTime? dateToSend,
            DateTime start,
            DateTime end)
        {
            var today = DateTime.Now;

            if (reportSchedule.RunSunday.HasValue && reportSchedule.RunSunday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Sunday);
            }
            else if (reportSchedule.RunMonday.HasValue && reportSchedule.RunMonday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Monday);
            }
            else if (reportSchedule.RunTuesday.HasValue && reportSchedule.RunTuesday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Tuesday);
            }
            else if (reportSchedule.RunWednesday.HasValue && reportSchedule.RunWednesday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Wednesday);
            }
            else if (reportSchedule.RunThursday.HasValue && reportSchedule.RunThursday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Thursday);
            }
            else if (reportSchedule.RunFriday.HasValue && reportSchedule.RunFriday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Friday);
            }
            else if (reportSchedule.RunSaturday.HasValue && reportSchedule.RunSaturday.Value)
            {
                dateToSend = DateToSend(reportSchedule, isNew, today, DayOfWeek.Saturday);
            }

            var startDateTime = Convert.ToDateTime($"{start:MM/dd/yyyy} {reportSchedule.StartTime}");
            if (DateTime.Now < startDateTime)
            {
                while (dateToSend < startDateTime)
                {
                    dateToSend = dateToSend.Value.AddDays(7);
                }
            }
            else
            {
                if (dateToSend < DateTime.Now)
                {
                    dateToSend = dateToSend.Value.AddDays(7);
                }
            }

            if (dateToSend.Value.Date > end.Date)
            {
                dateToSend = null;
            }

            return dateToSend;
        }

        private static DateTime? DateToSend(EntityReportSchedule reportSchedule, bool isNew, DateTime today, DayOfWeek dayOfWeek)
        {
            var shiftedArray = Shift(DayShiftValues, (int)today.DayOfWeek);
            DateTime dateToSend;
            if (today.DayOfWeek == dayOfWeek)
            {
                dateToSend = !isNew
                                 ? Convert.ToDateTime($"{today.AddDays(shiftedArray[0]):MM/dd/yyyy} {reportSchedule.StartTime}")
                                 : Convert.ToDateTime($"{today:MM/dd/yyyy} {reportSchedule.StartTime}");
            }
            else
            {
                var offSet = (int)today.DayOfWeek;

                dateToSend = Convert.ToDateTime(
                    $"{today.AddDays(shiftedArray[offSet]):MM/dd/yyyy} {reportSchedule.StartTime}");
            }

            return dateToSend;
        }

        private static int[] Shift(IReadOnlyList<int> originalArray, int offset)
        {
            var shiftedArray = new int[originalArray.Count];

            for (var index = 0; index < shiftedArray.Length; index++)
            {
                var offsetIndex = (index + offset) % shiftedArray.Length;
                shiftedArray[offsetIndex] = originalArray[index];
            }

            return shiftedArray;
        }

        private static DateTime? DateToSendRecurrenceMonthly(EntityReportSchedule reportSchedule, DateTime start, DateTime end)
        {
            DateTime? dateToSend;
            DateTime monthScheduleDay;
            var today = DateTime.Now.Day;
            today = today * -1;
            if (reportSchedule.MonthLastDay.HasValue && reportSchedule.MonthLastDay.Value.ToString()
                    .Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                monthScheduleDay = DateTime.Now.AddDays(today)
                    .AddDays(reportSchedule.MonthScheduleDay.Value);

                dateToSend = Convert.ToDateTime($"{monthScheduleDay:MM/dd/yyyy} {reportSchedule.StartTime}");
                if (DateTime.Now < Convert.ToDateTime($"{start:MM/dd/yyyy} {reportSchedule.StartTime}"))
                {
                    while (dateToSend < Convert.ToDateTime($"{start:MM/dd/yyyy} {reportSchedule.StartTime}"))
                    {
                        dateToSend = dateToSend.Value.AddMonths(1);
                    }
                }
                else
                {
                    if (dateToSend < DateTime.Now)
                    {
                        dateToSend = dateToSend.Value.AddMonths(1);
                    }
                }
            }
            else
            {
                // Get to first of current month and add one month to get to first of next month
                monthScheduleDay = DateTime.Now.AddDays(today + 1)
                    .AddMonths(1);

                // Substract one day to get to last day of current month
                monthScheduleDay = monthScheduleDay.AddDays(-1);

                dateToSend = Convert.ToDateTime($"{monthScheduleDay:MM/dd/yyyy} {reportSchedule.StartTime}");

                if (DateTime.Now < Convert.ToDateTime($"{start:MM/dd/yyyy} {reportSchedule.StartTime}"))
                {
                    while (dateToSend < Convert.ToDateTime($"{start:MM/dd/yyyy} {reportSchedule.StartTime}"))
                    {
                        dateToSend = dateToSend.Value
                            .AddDays(1)
                            .AddMonths(1)
                            .AddDays(-1);
                    }
                }
                else
                {
                    if (dateToSend < DateTime.Now)
                    {
                        dateToSend = dateToSend.Value.AddMonths(1);
                    }
                }
            }

            if (dateToSend.Value.Date > end.Date)
            {
                dateToSend = null;
            }

            return dateToSend;
        }

        private static DateTime? DateToSendRecurrenceDaily(EntityReportSchedule reportSchedule, bool isNew, DateTime end)
        {
            DateTime start;
            DateTime.TryParse(reportSchedule.StartDate, out start);
            DateTime? dateToSend = Convert.ToDateTime(
                !isNew
                    ? $"{DateTime.Now.AddDays(1):MM/dd/yyyy} {reportSchedule.StartTime}"
                    : $"{Convert.ToDateTime(reportSchedule.StartDate):MM/dd/yyyy} {reportSchedule.StartTime}");

            if (dateToSend.Value.Date > end.Date)
            {
                dateToSend = null;
            }

            return dateToSend;
        }
    }
}
