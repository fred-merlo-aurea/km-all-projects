using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using FrameworkUAS.Entity;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IFpsCustomRule
    {
        /// <summary>
        /// return list of all custom file import rules created by client
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FpsCustomRule>> SelectClientId(Guid accessKey, int clientId);

        /// <summary>
        /// return list of all custom file import rules created by client which are used by specific source file
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FpsCustomRule>> SelectSourceFileId(Guid accessKey, int sourceFileId);

        /// <summary>
        /// Insert/Update record
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fpsCustomRule"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FpsCustomRule fpsCustomRule);

        /// <summary>
        /// delete reference to all mapped rules for source file 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Delete(Guid accessKey, int sourceFileId);
        
    }
}
