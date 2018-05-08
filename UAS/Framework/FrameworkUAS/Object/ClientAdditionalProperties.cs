using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    public class ClientAdditionalProperties
    {
        public ClientAdditionalProperties()
        {
            SourceFilesList = new List<Entity.SourceFile>();
            ClientFtpDirectoriesList = new List<Entity.ClientFTP>();
            ClientCustomProceduresList = new List<Entity.ClientCustomProcedure>();
            ClientConfigurations = new List<KMPlatform.Entity.ClientConfigurationMap>();
        }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public List<FrameworkUAS.Entity.SourceFile> SourceFilesList { get; set; }
        [DataMember]
        public List<FrameworkUAS.Entity.ClientFTP> ClientFtpDirectoriesList { get; set; }
        [DataMember]
        public List<FrameworkUAS.Entity.ClientCustomProcedure> ClientCustomProceduresList { get; set; }
        [DataMember]
        public List<KMPlatform.Entity.ClientConfigurationMap> ClientConfigurations { get; set; }
    }
}
