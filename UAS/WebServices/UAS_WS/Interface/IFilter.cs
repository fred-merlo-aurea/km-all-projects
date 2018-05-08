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
    public interface IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.Filter x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accesssKey"></param>
        /// <param name="publicationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.Filter>> Select(Guid accesssKey, int publicationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accesssKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.Filter>> SelectClient(Guid accesssKey, int clientID);
    }
}
