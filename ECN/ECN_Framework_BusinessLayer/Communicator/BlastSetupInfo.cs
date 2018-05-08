using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities = ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding;
using KM.Common;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastSetupInfo
    {
        public static Entities.BlastSetupInfo GetNextScheduledBlastSetupInfo(int blastID)
        {
            Entities.BlastSetupInfo blastSetupInfo = null;

            var schedule = BlastSchedule.GetByBlastID(blastID, true);

            if (schedule != null && Convert.ToDateTime(schedule.SchedEndDate) > Convert.ToDateTime(DateTime.Now.Date.ToString("MM/dd/yyyy")))
                {
                    var blastScheduleAssistant = new BlastScheduleAssistant(schedule);
                    switch (schedule.Period)
                    {
                        case "o":
                            blastSetupInfo = blastScheduleAssistant.PrepareOneTimeSetupInfo();
                            break;

                        case "d":
                            blastSetupInfo = blastScheduleAssistant.PrepareDailySetupInfo();
                            break;

                        case "w":
                            blastSetupInfo = blastScheduleAssistant.PrepareWeeklySetupInfo();
                            break;

                        case "m":
                            blastSetupInfo = blastScheduleAssistant.PrepareMonthlySetupInfo();
                            break;

                        default:
                            break;
                    }
                }

            return blastSetupInfo;
        }

        public static Entities.BlastSetupInfo GetNextScheduledBlastSetupInfo(int blastScheduleID, bool includeToday)
        {
            Entities.BlastSetupInfo setupInfo = null;
            var schedule = BlastSchedule.GetByBlastScheduleID(blastScheduleID, true);

            if (schedule != null)
            {
                var startDateTime = GetStartDateTime(includeToday, schedule);
                var blastScheduleAssistant = new BlastScheduleAssistantIncludeToday(
                                                                schedule,
                                                                startDateTime);

                if (Convert.ToDateTime(schedule.SchedEndDate + " " + schedule.SchedTime) > startDateTime)
                {
                    switch (schedule.Period)
                    {
                        case "o":
                            setupInfo = blastScheduleAssistant.PrepareOneTimeSetupInfo();
                            break;

                        case "d":
                            setupInfo = blastScheduleAssistant.PrepareDailySetupInfo();
                            break;

                        case "w":
                            setupInfo = blastScheduleAssistant.PrepareWeeklySetupInfo();
                            break;

                        case "m":
                            setupInfo = blastScheduleAssistant.PrepareMonthlySetupInfo();
                            break;

                        default:
                            break;
                    }
                }
            }

            return setupInfo;
        }

        private static DateTime GetStartDateTime(bool includeToday, Entities.BlastSchedule schedule)
        {
            Guard.NotNull(schedule, nameof(schedule));
            var startDateTime = Convert.ToDateTime(schedule.SchedStartDate + " " + schedule.SchedTime);
            if (!includeToday)
            {
                if (DateTime.Now.AddDays(1) > startDateTime)
                {
                    startDateTime = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                if (DateTime.Now > startDateTime)
                {
                    startDateTime = DateTime.Now;
                }
            }

            return startDateTime;
        }
    }
}
