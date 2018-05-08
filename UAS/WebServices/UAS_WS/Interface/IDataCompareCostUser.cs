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
    public interface IDataCompareCostUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.DataCompareCostUser>> Select(Guid accessKey, int userId);
    }
}
