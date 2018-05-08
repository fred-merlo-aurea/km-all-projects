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
    public interface IFpsArchive
    {
        /// <summary>
        /// Insert/Update record
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fpsArchive"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FpsArchive fpsArchive);
        
    }
}
