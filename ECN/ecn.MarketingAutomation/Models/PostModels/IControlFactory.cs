namespace ecn.MarketingAutomation.Models.PostModels
{
    public interface IControlFactory
    {
        ControlBase PrepareForCopy(ControlBase control, int marketingAutomationId);
    }
}