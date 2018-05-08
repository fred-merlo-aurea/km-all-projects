namespace ecn.webservice.Facades.Params
{
    public class AddBlastParams
    {
        public int MessageId { get; set; }
        public int ListId { get; set; }
        public int FilterId { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ReplyEmail { get; set; }
        public bool IsTest { get; set; }
        public string RefBlasts { get; set; }
    }
}