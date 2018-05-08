using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class Reports
    {
        #region ClientFileLog
        public List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName)
        {
            List<Report.ClientFileLog> x = null;
            x = DataAccess.Reports.GetClientFileLog(clientID, clientName).ToList();

            return x;
        }
        public List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName, DateTime logDate)
        {
            List<Report.ClientFileLog> x = null;
            x = DataAccess.Reports.GetClientFileLog(clientID, clientName, logDate).ToList();

            return x;
        }
        public List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            List<Report.ClientFileLog> x = null;
            x = DataAccess.Reports.GetClientFileLog(clientID, clientName, startDate,endDate).ToList();

            return x;
        }
        #endregion
        #region FileCount
        public List<Report.FileCount> GetFileCount(int clientID, string clientName)
        {
            List<Report.FileCount> x = null;
            x = DataAccess.Reports.GetFileCount(clientID, clientName).ToList();

            return x;
        }
        public List<Report.FileCount> GetFileCount(int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            List<Report.FileCount> x = null;
            x = DataAccess.Reports.GetFileCount(clientID, clientName, startDate, endDate).ToList();

            return x;
        }
        public List<Report.FileCount> GetFileCount(DateTime startDate, DateTime endDate)
        {
            List<Report.FileCount> x = null;
            x = DataAccess.Reports.GetFileCount(startDate, endDate).ToList();

            return x;
        }
        #endregion
        #region TransformationCount
        public DataTable GetTransformationCount(int clientId, string clientName)
        {
            DataTable x = new DataTable();
            x = DataAccess.Reports.GetTransformationCount(clientId, clientName);

            return x;
        }
        #endregion
    }
}
