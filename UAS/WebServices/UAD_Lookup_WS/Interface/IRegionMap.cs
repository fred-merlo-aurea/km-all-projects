using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IRegionMap
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.RegionMap>> Select(Guid accessKey);
    }
}
