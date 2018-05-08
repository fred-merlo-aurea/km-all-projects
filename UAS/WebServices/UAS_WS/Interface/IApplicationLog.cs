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
    public interface IApplicationLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationLog>> SelectApplication(Guid accessKey, int applicationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationLog>> SelectApplicationWithDateRange(Guid accessKey, int applicationId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationLog>> SelectWithDateRange(Guid accessKey, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="app"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.ApplicationLog x, KMPlatform.BusinessLogic.Enums.Applications app, KMPlatform.BusinessLogic.Enums.SeverityTypes severity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationLogId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateNotified(Guid accessKey, int applicationLogId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ex"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="severity"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> LogError(Guid accessKey, Exception ex, string sourceMethod, KMPlatform.BusinessLogic.Enums.Applications application, KMPlatform.BusinessLogic.Enums.SeverityTypes severity, string note = "", int clientId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ex"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> LogCriticalError(Guid accessKey, string ex, string sourceMethod, KMPlatform.BusinessLogic.Enums.Applications application, string note = "", int clientId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ex"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> LogNonCriticalError(Guid accessKey, Exception ex, string sourceMethod, KMPlatform.BusinessLogic.Enums.Applications application, string note = "", int clientId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="note"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> LogNonCriticalErrorNote(Guid accessKey, string note, string sourceMethod, KMPlatform.BusinessLogic.Enums.Applications application, int clientId = -1);
    }
}
