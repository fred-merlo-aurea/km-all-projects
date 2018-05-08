using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.BusinessLogic
{
    public class Enums
    {
        #region KM Specific
        [DataContract]
        public enum KmSecurityGroups
        {
            [EnumMember]
            Account_Managers,
            [EnumMember]
            Administrators,
            [EnumMember]
            Circulation_Data_Entry,
            [EnumMember]
            DQM_Users
        }
        public static KmSecurityGroups GetKmSecurityGroups(string kmSecurityGroups)
        {
            try
            {
                kmSecurityGroups = kmSecurityGroups.Replace(" ", "_");
                return (KmSecurityGroups) System.Enum.Parse(typeof(KmSecurityGroups), kmSecurityGroups, true);
            }
            catch { return KmSecurityGroups.Circulation_Data_Entry; }
        }
        #endregion
        [DataContract]
        public enum Clients
        {
            [EnumMember]
            AAMP,
            [EnumMember]
            AINPublications,
            [EnumMember]
            Advanstar,
            [EnumMember]
            AHACoding,
            [EnumMember]
            Anthem,
            [EnumMember]
            Atcom,
            [EnumMember]
            ATHB,
            [EnumMember]
            Babcox,
            [EnumMember]
            BelmontPublications,
            [EnumMember]
            BriefMedia,
            [EnumMember]
            BusinessJournalsInc,
            [EnumMember]
            Canon,
            [EnumMember]
            Catersource,
            [EnumMember]
            CEG,
            [EnumMember]
            DEMO,
            [EnumMember]
            EHPublishing,
            [EnumMember]
            DeWittPublishing,
            [EnumMember]
            France,
            [EnumMember]
            GLM,
            [EnumMember]
            HappyDayMedia,
            [EnumMember]
            HealthForum,
            [EnumMember]
            HVCB,
            [EnumMember]
            HWPublishing,
            [EnumMember]
            ICD,
            [EnumMember]
            KnowledgeMarketing,
            [EnumMember]
            Lebhar,
            [EnumMember]
            Medtech,
            [EnumMember]
            Meister,
            [EnumMember]
            MSP,
            [EnumMember]
            MMA,
            [EnumMember]
            MTG,
            [EnumMember]
            NECA,
            [EnumMember]
            NoriaCorp,
            [EnumMember]
            Northstar,
            [EnumMember]
            OildomPublishing,
            [EnumMember]
            OutcomesLLC,
            [EnumMember]
            PennWell,
            [EnumMember]
            PentaVision,
            [EnumMember]
            SAETB,
            [EnumMember]
            Scranton,
            [EnumMember]
            SourceMedia,
            [EnumMember]
            SpecialityFoods,
            [EnumMember]
            Stamats,
            [EnumMember]
            Tabor,
            [EnumMember]
            TargetGroup,
            [EnumMember]
            TeamMHC,
            [EnumMember]
            TenMissions,
            [EnumMember]
            TMB,
            [EnumMember]
            TradePress,
            [EnumMember]
            UAStest,
            [EnumMember]
            UPI,
            [EnumMember]
            Vance,
            [EnumMember]
            Vcast,
            [EnumMember]
            Watt
        }
        public static Clients GetClient(string clientName)
        {
            try
            {
                return (Clients)System.Enum.Parse(typeof(Clients), clientName, true);
            }
            catch { return Clients.KnowledgeMarketing; }
        }
        [DataContract]
        public enum Applications
        {
            [EnumMember]
            Control_Center,
            [EnumMember]
            Circulation,
            [EnumMember]
            DQM,
            [EnumMember]
            Profile_Manager,
            [EnumMember]
            File_Mapper,
            [EnumMember]
            UAD_Dashboard,
            [EnumMember]
            UAD_Explorer,
            [EnumMember]
            Circulation_Explorer,
            [EnumMember]
            Email,
            [EnumMember]
            Forms,
            [EnumMember]
            UAD,
            [EnumMember]
            AMS_Desktop,
            [EnumMember]
            AMS_Web,
            [EnumMember]
            Data_Compare,
            [EnumMember]
            Data_Import_Export,
            [EnumMember]
            File_Mapper_Wizard,
            [EnumMember]
            ADMS_Engine,
            [EnumMember]
            AMS_Geocoding,
            [EnumMember]
            AMS_Operations,
            [EnumMember]
            Service_Que_Monitor
        }
        public static Applications GetApplication(string applicationName)
        {
            try
            {
                applicationName = applicationName.Replace(" ", "_");
                return (Applications)System.Enum.Parse(typeof(Applications), applicationName, true);
            }
            catch { return Applications.UAD; }
        }
        [DataContract]
        public enum RecordSource
        {
            CIRC,
            UAD,
            API
        }
        public static RecordSource GetRecordSource(string recordSource)
        {
            try
            {
                recordSource = recordSource.Replace(" ", "_");
                return (RecordSource) System.Enum.Parse(typeof(RecordSource), recordSource, true);
            }
            catch { return RecordSource.UAD; }
        }
        [DataContract]
        public enum Engine
        {
            ADMS,
            ADMS_DQM,
            SQM
        }
        public static Engine GetEngine(string engine)
        {
            try
            {
                engine = engine.Replace(" ", "_");
                return (Engine)System.Enum.Parse(typeof(Engine), engine, true);
            }
            catch { return Engine.ADMS; }
        }
        [DataContract]
        public enum CountriesWithRegions
        {
            [EnumMember]
            UNITED_STATES,
            [EnumMember]
            CANADA,
            [EnumMember]
            US_AND_CANADA,
            [EnumMember]
            INTERNATIONAL,
            [EnumMember]
            MEXICO
        }
        public enum ServerVariable
        {
            [EnumMember]
            ALL_HTTP,
            [EnumMember]
            ALL_RAW,
            [EnumMember]
            APPL_MD_PATH,
            [EnumMember]
            APPL_PHYSICAL_PATH,
            [EnumMember]
            AUTH_PASSWORD,
            [EnumMember]
            AUTH_TYPE,
            [EnumMember]
            AUTH_USER,
            [EnumMember]
            CERT_COOKIE,
            [EnumMember]
            CERT_FLAGS,
            [EnumMember]
            CERT_ISSUER,
            [EnumMember]
            CERT_KEYSIZE,
            [EnumMember]
            CERT_SECRETKEYSIZE,
            [EnumMember]
            CERT_SERIALNUMBER,
            [EnumMember]
            CERT_SERVER_ISSUER,
            [EnumMember]
            CERT_SERVER_SUBJECT,
            [EnumMember]
            CERT_SUBJECT,
            [EnumMember]
            CONTENT_LENGTH,
            [EnumMember]
            CONTENT_TYPE,
            [EnumMember]
            GATEWAY_INTERFACE,
            [EnumMember]
            HTTP_,
            [EnumMember]
            HTTP_ACCEPT,
            [EnumMember]
            HTTP_ACCEPT_LANGUAGE,
            [EnumMember]
            HTTP_COOKIE,
            [EnumMember]
            HTTP_REFERER,
            [EnumMember]
            HTTP_USER_AGENT,
            [EnumMember]
            HTTPS,
            [EnumMember]
            HTTPS_KEYSIZE,
            [EnumMember]
            HTTPS_SECRETKEYSIZE,
            [EnumMember]
            HTTPS_SERVER_ISSUER,
            [EnumMember]
            HTTPS_SERVER_SUBJECT,
            [EnumMember]
            INSTANCE_ID,
            [EnumMember]
            INSTANCE_META_PATH,
            [EnumMember]
            LOCAL_ADDR,
            [EnumMember]
            LOGON_USER,
            [EnumMember]
            PATH_INFO,
            [EnumMember]
            PATH_TRANSLATED,
            [EnumMember]
            QUERY_STRING,
            [EnumMember]
            REMOTE_ADDR,
            [EnumMember]
            REMOTE_HOST,
            [EnumMember]
            REMOTE_USER,
            [EnumMember]
            REQUEST_METHOD,
            [EnumMember]
            SCRIPT_NAME,
            [EnumMember]
            SERVER_NAME,
            [EnumMember]
            SERVER_PORT,
            [EnumMember]
            SERVER_PORT_SECURE,
            [EnumMember]
            SERVER_PROTOCOL,
            [EnumMember]
            SERVER_SOFTWARE,
            [EnumMember]
            URL

        }

    }
}
