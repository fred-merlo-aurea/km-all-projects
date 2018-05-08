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
    public interface IZipCode
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> Select(Guid accessKey);
    }
}
