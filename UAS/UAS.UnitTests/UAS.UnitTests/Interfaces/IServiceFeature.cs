using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface IServiceFeature
    {
        List<KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow> GetClientGroupTreeList(int? clientGroupID, bool isAdditionalCost);
        List<KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow> GetClientTreeList(int clientGroupID, int? clientID, bool isAdditionalCost);
        List<KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow> GetEmptySecurityGroupTreeList(int clientGroupID, int clientID);
        List<KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow> GetSecurityGroupTreeList(int securityGroupID, int clientGroupID, int clientID);
        bool Save(KMPlatform.Entity.ServiceFeature x);
        int SaveReturnId(KMPlatform.Entity.ServiceFeature x);
        List<KMPlatform.Entity.ServiceFeature> Select();
        List<KMPlatform.Entity.ServiceFeature> Select(int serviceID);
        List<KMPlatform.Entity.ServiceFeature> SelectOnlyEnabled(int serviceID);
        List<KMPlatform.Entity.ServiceFeature> SelectOnlyEnabledClientGroupID(int serviceID, int clientGroupID);
        List<KMPlatform.Entity.ServiceFeature> SelectOnlyEnabledClientID(int serviceID, int clientID);
        KMPlatform.Entity.ServiceFeature SelectServiceFeature(int serviceFeatureID);
    }
}