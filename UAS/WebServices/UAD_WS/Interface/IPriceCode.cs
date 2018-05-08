using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IPriceCode
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.PriceCode>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForPriceCode")]
        Response<FrameworkUAD.Entity.PriceCode> Select(Guid accessKey, string priceCode, int publicationID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PriceCode x, KMPlatform.Object.ClientConnections client);
    }
}
