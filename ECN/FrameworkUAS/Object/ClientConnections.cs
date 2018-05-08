using System;
using System.Runtime.Serialization;

namespace KMPlatform.Object
{
    [Serializable]
    [DataContract]
    public class ClientConnections
    {
        public ClientConnections() 
        {
            ClientTestDBConnectionString = string.Empty;
            ClientLiveDBConnectionString = string.Empty;
        }
        public ClientConnections(string test, string live)
        {
            ClientTestDBConnectionString = test;
            ClientLiveDBConnectionString = live;
        }
        public ClientConnections(Entity.Client client)
        {
            ClientTestDBConnectionString = client.ClientTestDBConnectionString;
            ClientLiveDBConnectionString = client.ClientLiveDBConnectionString;
        }
        #region Properties
        [DataMember]
        public string ClientTestDBConnectionString { get; set; }
        [DataMember]
        public string ClientLiveDBConnectionString { get; set; }
        #endregion
    }
}
