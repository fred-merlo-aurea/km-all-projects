using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines
{
    public class ProcessLoadArgs
    {
        public bool SaveAltEmail { get; set; }
        public string TransType { get; set; }
        public string Demo7 { get; set; }
        public int CommonApplication { get; set; }
        public Email Email { get; set; }
        public Group CommunicatorGroup { get; set; }
        public SmartFormsHistory SmartFormHistory { get; set; }
        public string ConversionTrackingBlastId { get; set; }
        public string ConversionTrackingEmailId { get; set; }
        public bool ReturnValue { get; set; }
    }
}