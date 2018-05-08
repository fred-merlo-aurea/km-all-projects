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
    public interface IFileLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FileLog>> SelectClient(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FileLog>> SelectProcessCode(Guid accessKey, string processCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.FileLog>> SelectFileLog(Guid accessKey, int sourceFileID, string processCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.FileLog x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Object.FileLog>> SelectDistinctProcessCodePerSourceFile(Guid accessKey, int clientID);
    }
}
