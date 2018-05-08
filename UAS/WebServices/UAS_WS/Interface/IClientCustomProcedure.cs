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
    public interface IClientCustomProcedure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClient")]
        Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> Select(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.ClientCustomProcedure x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveReturnID")]
        Response<int> SaveReturnID(Guid accessKey, FrameworkUAS.Entity.ClientCustomProcedure x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sproc"></param>
        /// <param name="sourceFileID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> ExecuteSproc(Guid accessKey, string sproc, int sourceFileID, KMPlatform.Entity.Client client);
    }
}
