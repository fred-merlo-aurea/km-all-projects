namespace DQM.Helpers.Validation
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }
        public static string ToGoodBadString(this bool value)
        {
            return value ? "Processed" : "Invalid";
        }
    }
}