namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface ICancellable
    {
        bool IsCancelled { get; set; }
    }
}