using System.Collections.Generic;
using KMPlatform.Object;

using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IProduct
    {
        bool Copy(ClientConnections client, int fromID, int toID);
        bool ExistsByPubTypeID(int pubTypeID, ClientConnections client);
        int Save(FrameworkUADEntity.Product x, ClientConnections client);
        List<FrameworkUADEntity.Product> Select(ClientConnections client, bool includeCustomProperties = false);
        FrameworkUADEntity.Product Select(string pubCode, ClientConnections client, bool includeCustomProperties = false);
        FrameworkUADEntity.Product Select(int pubID, ClientConnections client, bool includeCustomProperties = false, bool GetLatestData = false);
        List<FrameworkUADEntity.Product> SelectByBrandID(ClientConnections client, int brandID, bool includeCustomProperties = false);
        bool UpdateLock(ClientConnections client, int userID);
    }
}