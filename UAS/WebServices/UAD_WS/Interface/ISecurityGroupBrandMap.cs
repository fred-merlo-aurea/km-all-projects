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
    public interface ISecurityGroupBrandMap
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> SelectForBrand(Guid accessKey, KMPlatform.Object.ClientConnections client, int brandID);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> SelectForSecurityGroup(Guid accessKey, KMPlatform.Object.ClientConnections client, int securityGroupID);
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.SecurityGroupBrandMap x);
    }
}
