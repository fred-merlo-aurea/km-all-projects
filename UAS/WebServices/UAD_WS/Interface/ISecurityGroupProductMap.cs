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
    public interface ISecurityGroupProductMap
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> SelectForProduct(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> SelectForSecurityGroup(Guid accessKey, KMPlatform.Object.ClientConnections client, int securityGroupID);
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.SecurityGroupProductMap x);
    }
}
