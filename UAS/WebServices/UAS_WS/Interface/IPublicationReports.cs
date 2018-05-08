using FrameworkUAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IPublicationReports
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="publicationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.PublicationReports>> SelectPublication(Guid accessKey, int publicationID);
    }
}
