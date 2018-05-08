namespace ecn.automatedreporting.Reports
{
    public interface IDeliveryReport
    {
        ReturnReport Execute();
        string Body { get; set; }
    }
}
