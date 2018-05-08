namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ILayoutPlansManager
    {
        bool Exists(int layoutPlanId, int customerId);

        bool Exists(int groupID, string criteria);
    }
}