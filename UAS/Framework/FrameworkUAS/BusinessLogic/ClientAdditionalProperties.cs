using System;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class ClientAdditionalProperties
    {
        public Object.ClientAdditionalProperties SetObjects(int clientId, bool isFileDeleted)
        {
            Object.ClientAdditionalProperties retItem = new Object.ClientAdditionalProperties();

            SourceFile s = new SourceFile();
            ClientFTP cftp = new ClientFTP();
            ClientCustomProcedure ccp = new ClientCustomProcedure();
            KMPlatform.BusinessLogic.ClientConfigurationMap cm = new KMPlatform.BusinessLogic.ClientConfigurationMap();

            retItem.ClientID = clientId;
            retItem.SourceFilesList = s.Select(clientId, true, isFileDeleted).ToList();
            retItem.ClientCustomProceduresList = ccp.SelectClient(clientId).ToList();
            retItem.ClientFtpDirectoriesList = cftp.SelectClient(clientId).ToList();
            retItem.ClientConfigurations = cm.Select(clientId).ToList();

            return retItem;
        }
    }
}
