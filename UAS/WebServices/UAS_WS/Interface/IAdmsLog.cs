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
    public interface IAdmsLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdmsLog>> Select(Guid accessKey, int clientID, int sourceFileId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileId"></param>
        /// <param name="fileStart"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdmsLog>> SelectFileStartDate(Guid accessKey, int clientID, int sourceFileId, DateTime fileStart);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="fileNameExact"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.AdmsLog>> SelectFileExactName(Guid accessKey, int clientID, string fileNameExact);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.AdmsLog> SelectProcessCode(Guid accessKey, string processCode);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.AdmsLog x);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="fileStatus"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateFileStatus(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, int userId, bool createLog = true, int sourceFileId = -1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="step"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateCurrentStep(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.ADMS_StepType step, int userId, bool createLog = true, int sourceFileId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateProcessingStatus(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.FileStatusType status, int userId, bool createLog = false, int sourceFileId = -1);
        
    }
}
