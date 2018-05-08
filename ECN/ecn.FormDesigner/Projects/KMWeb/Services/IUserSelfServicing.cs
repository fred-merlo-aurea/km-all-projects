using KMManagers;

namespace KMWeb.Services
{
    public interface IUserSelfServicing
    {
        string ResendPassword(string emailAddress, string other, int groupId, 
            int formId, string otherIdentification, SubscriberLoginManager subscriberLoginManager);

        string ChangeEmailAndResendPassword(string newEmailAddress, string emailAddress, string other, int groupId,
            int formId, string otherIdentification, SubscriberLoginManager subscriberLoginManager);
    }
}