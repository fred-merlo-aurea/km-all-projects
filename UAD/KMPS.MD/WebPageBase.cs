using System;
using System.Web.UI;
using KM.Common;
using PlatformUser = KM.Platform.User;
using EntityUser = KMPlatform.Entity.User;
using ServiceEnum = KMPlatform.Enums.Services;
using FeatureEnum = KMPlatform.Enums.ServiceFeatures;
using AccessEnum = KMPlatform.Enums.Access;

namespace KMPS.MD
{
    public class WebPageBase : Page
    {
        private const string AccessErrorPagePath = "~/SecurityAccessError.aspx";

        protected void RedirectIfNoViewAccess(EntityUser currentUser, FeatureEnum feature, string redirectPage = AccessErrorPagePath)
        {
            if (!PlatformUser.HasAccess(currentUser, ServiceEnum.UAD, feature, AccessEnum.View))
            {
                Response.Redirect(redirectPage);
            }
        }

        protected virtual void SetChildVisibility(Control parentControl, string childId, bool visible)
        {
            Guard.NotNull(parentControl, nameof(parentControl));
            var child = parentControl.FindControl(childId);
            Guard.NotNull(child, () => new InvalidOperationException($"Unable to find child control by id '{childId}'"));

            child.Visible = visible;
        }

        protected int GetInt32(string text)
        {
            int result;
            int.TryParse(text, out result);

            return result;
        }
    }
}