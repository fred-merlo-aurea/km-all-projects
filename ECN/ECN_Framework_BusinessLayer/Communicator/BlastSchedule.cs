using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using System.Xml;
using KM.Common;
using DataLayer = ECN_Framework_DataLayer.Communicator;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastSchedule
    {
        private const string ScheduleTypeSendNow = "//Schedule/ScheduleType[@ID ='SendNow']";
        private const string ScheduleTypeOneTime = "//Schedule/ScheduleType[@ID ='OneTime']";
        private const string ScheduleTypeRecurring = "//Schedule/ScheduleType[@ID ='Recurring']";
        private const string SplitTypeSingleDay = "/SplitType[@ID ='SingleDay']";
        private const string SplitTypeEvenly = "/SplitType[@ID ='Evenly']";
        private const string SplitTypeManually = "/SplitType[@ID ='Manually']";
        private const string RecurrenceTypeDaily = "/RecurrenceType[@ID ='Daily']";
        private const string RecurrenceTypeWeekly = "/RecurrenceType[@ID ='Weekly']";
        private const string RecurrenceTypeMonthly = "/RecurrenceType[@ID ='Monthly']";
        private const string SchedulePeriodSendNow = "s";
        private const string SchedulePeriodDaily = "d";
        private const string SchedulePeriodWeekly = "w";
        private const string SchedulePeriodMonthly = "m";
        private const string SchedulePeriodOneTime = "o";
        private const string DateFormat = "MM/dd/yyyy";

        public static bool Exists(int blastScheduleID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastSchedule.Exists(blastScheduleID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int blastScheduleID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastSchedule.Exists(blastScheduleID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.BlastSchedule GetByBlastScheduleID(int blastScheduleID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastSchedule schedule = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                schedule = ECN_Framework_DataLayer.Communicator.BlastSchedule.GetByBlastScheduleID(blastScheduleID);
                scope.Complete();
            }
            if (schedule != null && getChildren)
            {
                schedule.DaysList = ECN_Framework_BusinessLayer.Communicator.BlastScheduleDays.GetByBlastScheduleID(blastScheduleID);
            }
            return schedule;
        }

        public static ECN_Framework_Entities.Communicator.BlastSchedule GetByBlastID(int blastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastSchedule schedule = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                schedule = ECN_Framework_DataLayer.Communicator.BlastSchedule.GetByBlastID(blastID);
                scope.Complete();
            }
            if (schedule != null && getChildren)
            {
                schedule.DaysList = ECN_Framework_BusinessLayer.Communicator.BlastScheduleDays.GetByBlastScheduleID(schedule.BlastScheduleID.Value);
            }
            return schedule;
        }

        public static ECN_Framework_Entities.Communicator.BlastSchedule GetByBlastID_UseAmbientTransaction(int blastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastSchedule schedule = null;

            using (TransactionScope scope = new TransactionScope())
            {
                schedule = ECN_Framework_DataLayer.Communicator.BlastSchedule.GetByBlastID(blastID);
                scope.Complete();
            }
            if (schedule != null && getChildren)
            {
                schedule.DaysList = ECN_Framework_BusinessLayer.Communicator.BlastScheduleDays.GetByBlastScheduleID_UseAmbientTransaction(schedule.BlastScheduleID.Value);
            }
            return schedule;
        }

        public static Entities.BlastSchedule CreateScheduleFromXML(string xmlSchedule, int userID)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlSchedule);

            var nodeSendNow = doc.SelectSingleNode(ScheduleTypeSendNow);
            var nodeOneTime = doc.SelectSingleNode(ScheduleTypeOneTime);
            var nodeRecurring = doc.SelectSingleNode(ScheduleTypeRecurring);

            if (nodeSendNow != null)
            {
                return CreateSendNowSchedule(doc);
            }
            else if (nodeOneTime != null)
            {
                return CreateOneTimeSchedule(doc, userID);
            }
            else if (nodeRecurring != null)
            {
                return CreateRecurringSchedule(doc, userID);
            }

            return null;
        }

        private static Entities.BlastSchedule CreateRecurringSchedule(XmlDocument doc, int userID)
        {
            var schedule = new Entities.BlastSchedule();

            var startTimeNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}/StartTime");
            var startDateNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}/StartDate");

            schedule.SchedTime = startTimeNode?.InnerText;
            schedule.SchedStartDate = startDateNode?.InnerText;

            if (Convert.ToDateTime($"{schedule.SchedStartDate} {schedule.SchedTime}") < DateTime.Now)
            {
                return null;
            }

            var endDateNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}/EndDate");
            DateTime endDate;
            if (!DateTime.TryParse(endDateNode?.InnerText, out endDate))
            {
                schedule.SchedEndDate = endDate.ToString(DateFormat);
            }
            else
            { 
                Trace.WriteLine($"Unable to parse {endDateNode?.InnerText} as DateTime");
            }
            schedule.CreatedBy = userID;

            var daysList = new List<Entities.BlastScheduleDays>();

            var nodeDaily = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeDaily}");
            var nodeWeekly = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeWeekly}");
            var nodeMonthly = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeMonthly}");

            if (nodeDaily != null)
            {
                schedule.Period = SchedulePeriodDaily;
                return CreateSingleDaySchedule(doc, schedule, nodeDaily.HasChildNodes, $"{ScheduleTypeRecurring}{RecurrenceTypeDaily}");
            }
            else if (nodeWeekly != null)
            {
                schedule.Period = SchedulePeriodWeekly;
                var nodeEvenly = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeWeekly}{SplitTypeEvenly}/Days");
                var nodeManually = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeWeekly}{SplitTypeManually}/Days");

                var nodeCurrent = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeWeekly}/HowManyWeeks");
                var weeks = Convert.ToInt32(nodeCurrent?.InnerText);

                var evenlyBlastSchedule = CreateEvenlyDaysOneTimeSchedule(schedule, nodeEvenly, weeks);
                var manuallyBlastSchedule = CreateManuallyOneTimeSchedule(
                    doc,
                    schedule,
                    nodeManually,
                    $"{ScheduleTypeRecurring}{RecurrenceTypeWeekly}{SplitTypeManually}",
                    weeks);
                return manuallyBlastSchedule == null ? evenlyBlastSchedule : manuallyBlastSchedule;
            }
            else if (nodeMonthly != null)
            {
                schedule.Period = SchedulePeriodMonthly;
                return CreateMonthlyRecurrentBlastSchedule(doc, schedule, nodeMonthly);
            }

            return null;
        }

        private static Entities.BlastSchedule CreateMonthlyRecurrentBlastSchedule(
            XmlDocument doc,
            Entities.BlastSchedule schedule,
            XmlNode nodeMonthly)
        {
            Guard.NotNull(doc, nameof(doc));
            Guard.NotNull(schedule, nameof(schedule));
            Guard.NotNull(nodeMonthly, nameof(nodeMonthly));

            var dayOfMonthNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeMonthly}/DayOfMonth");
            var days = new Entities.BlastScheduleDays()
            {
                DayToSend = Convert.ToInt32(dayOfMonthNode?.InnerText)
            };

            if (nodeMonthly.ChildNodes.Count > 1)
            {
                var amountNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeMonthly}/Amount");
                days.Total = Convert.ToInt32(amountNode?.InnerText);

                var isAmountNode = doc.SelectSingleNode($"{ScheduleTypeRecurring}{RecurrenceTypeMonthly}/IsAmount");
                days.IsAmount = Convert.ToBoolean(isAmountNode?.InnerText);
            }

            schedule.DaysList = new List<Entities.BlastScheduleDays>();
            schedule.DaysList.Add(days);

            return InsertAndGetSchedule(schedule);
        }

        private static Entities.BlastSchedule CreateOneTimeSchedule(XmlDocument doc, int userID)
        {
            Guard.NotNull(doc, nameof(doc));

            var startTimeNode = doc.SelectSingleNode($"{ScheduleTypeOneTime}/StartTime");
            var startDateNode = doc.SelectSingleNode($"{ScheduleTypeOneTime}/StartDate");

            var schedule = new Entities.BlastSchedule()
            {
                SchedTime = startTimeNode?.InnerText,
                SchedStartDate = startDateNode?.InnerText,
            };

            DateTime scheduleTime;
            if (!DateTime.TryParse($"{schedule.SchedStartDate} {schedule.SchedTime}", out scheduleTime))
            {
                Trace.WriteLine($"Unable to parse {schedule.SchedStartDate} {schedule.SchedTime} as a DateTime");
                return null;
            }
            if (scheduleTime < DateTime.Now)
            {
                return null;
            }

            scheduleTime = scheduleTime.AddDays(7);
            schedule.SchedEndDate = scheduleTime.ToString(DateFormat);
            schedule.Period = SchedulePeriodOneTime;
            schedule.CreatedBy = userID;

            var nodeSingleDay = doc.SelectSingleNode($"{ScheduleTypeOneTime}{SplitTypeSingleDay}");
            var nodeEvenly = doc.SelectSingleNode($"{ScheduleTypeOneTime}{SplitTypeEvenly}/Days");
            var nodeManually = doc.SelectSingleNode($"{ScheduleTypeOneTime}{SplitTypeManually}/Days");

            if (nodeSingleDay != null)
            {
                return CreateSingleDaySchedule(doc, schedule, nodeSingleDay.HasChildNodes, $"{ScheduleTypeOneTime}{SplitTypeSingleDay}");
            }
            else if (nodeEvenly != null)
            {
                return CreateEvenlyDaysOneTimeSchedule(schedule, nodeEvenly);
            }
            else if (nodeManually != null)
            {
                return CreateManuallyOneTimeSchedule(doc, schedule, nodeManually, $"{ScheduleTypeOneTime}{SplitTypeManually}");
            }

            return null;
        }

        private static Entities.BlastSchedule CreateManuallyOneTimeSchedule(
            XmlDocument doc, 
            Entities.BlastSchedule schedule, 
            XmlNode nodeManually, 
            string manuallyXPath,
            int? weeks = null)
        {
            if (nodeManually == null || !nodeManually.HasChildNodes)
            {
                return null;
            }

            Guard.NotNull(doc, nameof(doc));
            Guard.NotNull(schedule, nameof(schedule));

            var isAmountNode = doc.SelectSingleNode($"{manuallyXPath}/Days/IsAmount");
            var isAmount = Convert.ToBoolean(isAmountNode?.InnerText);

            schedule.DaysList = new List<Entities.BlastScheduleDays>();
            foreach (XmlNode node in nodeManually.ChildNodes)
            {
                if (node.Name == "Day")
                {
                    var days = new Entities.BlastScheduleDays();
                    days.IsAmount = isAmount;
                    var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), node.Attributes["ID"].Value, true);
                    days.DayToSend = (int)dayOfWeek;
                    days.Weeks = weeks;
                    days.Total = Convert.ToInt32(node.ChildNodes[0]?.InnerText);
                    schedule.DaysList.Add(days);
                }
            }

            return InsertAndGetSchedule(schedule);
        }

        private static Entities.BlastSchedule CreateEvenlyDaysOneTimeSchedule(
            Entities.BlastSchedule schedule, 
            XmlNode nodeEvenly, 
            int? weeks = null)
        {
            if (nodeEvenly == null || !nodeEvenly.HasChildNodes)
            {
                return null;
            }

            Guard.NotNull(schedule, nameof(schedule));

            var totalDays = nodeEvenly.ChildNodes.Count;
            var totalPercent = 100;
            var indivPercent = totalPercent / totalDays;
            var remainder = totalPercent % totalDays;
            var iter = 0;
            schedule.DaysList = new List<Entities.BlastScheduleDays>();
            foreach (XmlNode node in nodeEvenly.ChildNodes)
            {
                var days = new Entities.BlastScheduleDays();
                days.IsAmount = false;
                var dayOfWeek = (DayOfWeek)Enum.Parse(
                                        typeof(DayOfWeek),
                                        node.Attributes["ID"].Value,
                                        true);
                days.DayToSend = (int)dayOfWeek;
                days.Weeks = weeks;
                iter++;
                if (iter == totalDays)
                {
                    days.Total = indivPercent + remainder;
                }
                else
                {
                    days.Total = indivPercent;
                }
                schedule.DaysList.Add(days);
            }

            return InsertAndGetSchedule(schedule);
        }

        private static Entities.BlastSchedule CreateSingleDaySchedule(
            XmlDocument doc, 
            Entities.BlastSchedule schedule, 
            bool hasChildNodes, 
            string singleDayXPath)
        {
            if (hasChildNodes)
            {
                Guard.NotNull(doc, nameof(doc));
                Guard.NotNull(schedule, nameof(schedule));

                var amountNode = doc.SelectSingleNode($"{singleDayXPath}/Amount");
                var isAmountNode = doc.SelectSingleNode($"{singleDayXPath}/IsAmount");
                schedule.DaysList = new List<Entities.BlastScheduleDays>()
                {
                    new Entities.BlastScheduleDays()
                    {
                        Total = Convert.ToInt32(amountNode?.InnerText),
                        IsAmount = Convert.ToBoolean(isAmountNode?.InnerText)
                    }
                };
            }

            return InsertAndGetSchedule(schedule);
        }

        private static Entities.BlastSchedule InsertAndGetSchedule(Entities.BlastSchedule schedule)
        {
            var blastScheduleID = DataLayer.BlastSchedule.Insert(schedule);
            if (blastScheduleID > 0)
            {
                return DataLayer.BlastSchedule.GetByBlastScheduleID(blastScheduleID);
            }

            return null;
        }

        private static Entities.BlastSchedule CreateSendNowSchedule(XmlDocument doc)
        {
            Guard.NotNull(doc, nameof(doc));

            var nodeSingleDay = doc.SelectSingleNode($"{ScheduleTypeSendNow}{SplitTypeSingleDay}");
            if (nodeSingleDay == null || !nodeSingleDay.HasChildNodes)
            {
                return null;
            }

            var amountNode = doc.SelectSingleNode($"{ScheduleTypeOneTime}{SplitTypeSingleDay}/Amount");
            var isAmountNode = doc.SelectSingleNode($"{ScheduleTypeOneTime}{SplitTypeSingleDay}/IsAmount");
            return new Entities.BlastSchedule()
            {
                Period = SchedulePeriodSendNow,
                DaysList = new List<Entities.BlastScheduleDays>()
                {
                    new Entities.BlastScheduleDays
                    {
                        Total = Convert.ToInt32(amountNode?.InnerText),
                        IsAmount = Convert.ToBoolean(isAmountNode?.InnerText)
                    }
                }
            };
        }

        public static int Insert(ECN_Framework_Entities.Communicator.BlastSchedule schedule)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                count = ECN_Framework_DataLayer.Communicator.BlastSchedule.Insert(schedule);
                scope.Complete();
            }
            return count;
        }

        public static int Update(ECN_Framework_Entities.Communicator.BlastSchedule schedule, int blastID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                count = ECN_Framework_DataLayer.Communicator.BlastSchedule.Update(schedule, blastID);
                scope.Complete();
            }
            return count;
        }

        public static void Delete(int blastScheduleID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSchedule.Delete(blastScheduleID);
                scope.Complete();
            }
        }
    }
}
