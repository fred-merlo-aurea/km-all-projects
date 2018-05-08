using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IReports
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="GetClientFileLog")]
        Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="logDate"></param>
        /// /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="GetClientFileLogForDate")]
        Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName, DateTime logDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name="GetClientFileLogForDates")]
        Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetClientFileCount")]
        Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, int clientID, string clientName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// /// <param name="clientName"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetClientFileCountForDates")]
        Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, int clientID, string clientName, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetFileCountForDates")]
        Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, DateTime startDate, DateTime endDate);
    }
}
