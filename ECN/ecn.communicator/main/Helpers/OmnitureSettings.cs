namespace ecn.communicator.main.Helpers
{
    public class OmnitureSettings
    {
        public bool AllowCustOverride { get; set; }
        public bool OverrideBaseChannel { get; set; }
        public bool HasBaseSetup { get; set; }
        public bool HasCustSetup { get; set; }

        public bool UseBaseChannel()
        {
            return (!AllowCustOverride || !OverrideBaseChannel) && HasBaseSetup;
        }

        public bool UseOverride()
        {
            return AllowCustOverride && OverrideBaseChannel && HasCustSetup;
        }
    }
}