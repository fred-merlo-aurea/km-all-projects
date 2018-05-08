using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ReportSchedule
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ReportSchedule;
        public static ECN_Framework_Entities.Communicator.ReportSchedule GetByReportScheduleID(int ReportScheduleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ReportSchedule = ECN_Framework_DataLayer.Communicator.ReportSchedule.GetByReportScheduleID(ReportScheduleID);
                scope.Complete();
            }

            if (ReportSchedule != null && ReportSchedule.ReportID != null)
            {
                if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != ReportSchedule.CustomerID.Value)
                    throw new ECN_Framework_Common.Objects.SecurityException();
                ReportSchedule.Report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(ReportSchedule.ReportID.Value, user);
            }
            return ReportSchedule;
        }

        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetByCustomerID(int CustomerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ReportSchedule> ReportScheduleList = new List<ECN_Framework_Entities.Communicator.ReportSchedule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ReportScheduleList = ECN_Framework_DataLayer.Communicator.ReportSchedule.GetByCustomerID(CustomerID);
                foreach (ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule in ReportScheduleList)
                {
                    ReportSchedule.Report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(ReportSchedule.ReportID.Value, user);
                }
                scope.Complete();
            }
            return ReportScheduleList;
        }

        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetReportsToSend(DateTime timeToSend, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ReportSchedule> ReportScheduleList = new List<ECN_Framework_Entities.Communicator.ReportSchedule>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ReportScheduleList = ECN_Framework_DataLayer.Communicator.ReportSchedule.GetReportsToSend(timeToSend);
                foreach (ECN_Framework_Entities.Communicator.ReportSchedule report in ReportScheduleList)
                {
                    report.Report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(report.ReportID.Value, user);
                }
                scope.Complete();
            }

            return ReportScheduleList;
        }

        public static void Delete(int ReportScheduleID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ReportSchedule.Delete(ReportScheduleID, user.UserID);

                scope.Complete();
            }
        }
        public static void DeleteByBlastId(int blastId, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ReportSchedule.DeleteByBlastId(blastId, user.UserID);
                scope.Complete();
            }
        }
        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetByBlastId(int blastId)
        {
            List<ECN_Framework_Entities.Communicator.ReportSchedule> reportScheduleList = new List<ECN_Framework_Entities.Communicator.ReportSchedule>();
            using (TransactionScope scope = new TransactionScope())
            {
                reportScheduleList = ECN_Framework_DataLayer.Communicator.ReportSchedule.GetByBlastId(blastId);
                scope.Complete();
            }
            return reportScheduleList;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.ReportSchedule reportSchedule, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (reportSchedule.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {

                if (reportSchedule.EndDate != string.Empty)
                {
                    DateTime StartDate = Convert.ToDateTime(reportSchedule.StartDate);
                    DateTime EndDate = Convert.ToDateTime(reportSchedule.EndDate);

                    if (StartDate > EndDate)
                    {
                        errorList.Add(new ECNError(Entity, Method, "Date Range is invalid."));
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.ReportSchedule reportSchedule, KMPlatform.Entity.User user)
        {
            Validate(reportSchedule, user);
            using (TransactionScope scope = new TransactionScope())
            {
                int initialID = reportSchedule.ReportScheduleID;
                reportSchedule.ReportScheduleID = ECN_Framework_DataLayer.Communicator.ReportSchedule.Save(reportSchedule);
                ECN_Framework_Entities.Communicator.ReportQueue rq = null;
                DateTime? toSend = ECN_Framework_BusinessLayer.Communicator.ReportQueue.GetNextDateToRun(reportSchedule, true);
                if (initialID > 0)
                {

                    rq = ECN_Framework_BusinessLayer.Communicator.ReportQueue.GetNextPendingForReportSchedule(initialID);
                    if (toSend.HasValue)
                    {
                        bool isNew = false;
                        if (rq == null)
                        {
                            rq = new ECN_Framework_Entities.Communicator.ReportQueue();
                            isNew = true;
                        }
                        rq.ReportScheduleID = reportSchedule.ReportScheduleID;
                        rq.ReportID = reportSchedule.ReportID.Value;
                        rq.Status = "Pending";
                        rq.SendTime = toSend;
                        ECN_Framework_BusinessLayer.Communicator.ReportQueue.Save(rq, isNew ? false : true);
                    }
                }
                else
                {
                    rq = new ECN_Framework_Entities.Communicator.ReportQueue();
                    rq.ReportScheduleID = reportSchedule.ReportScheduleID;
                    rq.ReportID = reportSchedule.ReportID.Value;
                    rq.Status = "Pending";

                    if (toSend.HasValue)
                    {

                        rq.SendTime = toSend;
                        ECN_Framework_BusinessLayer.Communicator.ReportQueue.Save(rq, false);
                    }


                }
                scope.Complete();
            }
            return reportSchedule.ReportScheduleID;
        }
    }
}