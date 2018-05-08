using KMPlatform.Entity;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IValidateSubscribeControl
    {
        void Validate(Group subscribeGroup, User currentUser);
    }
}