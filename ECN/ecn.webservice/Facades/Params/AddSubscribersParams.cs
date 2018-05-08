namespace ecn.webservice.Facades.Params
{
    public class AddSubscribersParams
    {
        public int ListId;
        public string SubscriptionType;
        public string FormatType;
        public string XmlString;
        public bool AutoGenerateUdfs;
        public string Source;
        public string CompositeKey;
        public bool OverwriteWithNull;
        public int? SmartFormId;
        public bool CreateEmailActivityLog;
        public bool EscapeApostrophesInEmailAddress;
    }
}