using System;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class SubGen
    {
        public bool SubGen_Subscriber_Update(string xml, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubGen.SubGen_Subscriber_Update(xml, client);
        }
        public bool SubGen_Address_Update(string xml, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubGen.SubGen_Address_Update(xml, client);
        }
    }
}
