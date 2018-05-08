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
    public interface ICountry
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.Country>> Select(Guid accessKey);

        [OperationContract]
        Response<bool> CountryRegionCleanse(Guid accessKey, int sourceFileID, string processCode);
    }
}
