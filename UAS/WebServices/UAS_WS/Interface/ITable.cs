using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ITable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Object.Table>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="table"></param>
        /// <param name="client"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [OperationContract]
        Response<DataTable> SelectDataTable(Guid accessKey, string table, int client, int file);        
    }
}
