using ecn.communicator.main.Salesforce.SF_Pages;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class GroupTypeExtensions
    {
        public static bool IsSf(this GroupType group)
        {
            return group == GroupType.SF;
        }
        public static bool IsEcn(this GroupType group)
        {
            return group == GroupType.ECN;
        }
        public static GroupType Opposite(this GroupType btnType)
        {
            return btnType.IsEcn() ? GroupType.SF : GroupType.ECN;
        }
    }
}