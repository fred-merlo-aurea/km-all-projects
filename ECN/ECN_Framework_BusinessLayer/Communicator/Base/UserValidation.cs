using ECN_Framework_Entities;

namespace ECN_Framework_BusinessLayer.Communicator.Base
{
    static public class UserValidation
    {
        static string Invalidation(
            int? userId,
            int? customerId,
            bool checkIsAdmin)
        {
            if (userId == null || userId.Value == -1)
            {
                return "User: UserID is invalid";
            }
            var user = KMPlatform.BusinessLogic.User.GetByUserID(userId.Value, false);
            if (user == null)
            {
                return "User: Could not retrieve User model";
            }
            if (!checkIsAdmin)
            {
                return null;
            }
            var isAdmin = user.IsPlatformAdministrator;
            if (!isAdmin)
            {
                if (customerId == null || customerId.Value == -1)
                {
                    return "User: CustomerId is invalid";
                }
                if (!KMPlatform.BusinessLogic.User.Exists(userId.Value, customerId.Value))
                {
                    return "User: User does not exist in the channel";
                }
            }
            return null;
        }
        static public string Invalidate(IUserValidate model)
        {
            var result = (string)null;
            var dynamicModel = (dynamic)model;
            if (model.HasValidID)
            {
                result = Invalidation(
                    (int?)dynamicModel.UpdatedUserID,
                    (int?)dynamicModel.CustomerID,
                    false);
            }
            else
            {
                result = Invalidation(
                    (int?)dynamicModel.CreatedUserID,
                    (int?)dynamicModel.CustomerID,
                    true);
            }
            return result;
        }
    }
}
