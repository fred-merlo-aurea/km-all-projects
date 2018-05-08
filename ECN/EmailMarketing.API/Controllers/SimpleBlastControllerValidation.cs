using System;
using System.Text;
using ecn.common.classes;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;

namespace EmailMarketing.API.Controllers
{
    public class SimpleBlastControllerValidation
    {
        public const string InvalidLayoutDefaultErrorMessage = "Content for LayoutID is not validated";
        private const string UnlimitedLicenseValue = "UNLIMITED";
        private const string NotApplicable = "N/A";
        private const string NoLicense = "NO LICENSE";
        private const string ContentIdsSeparator = ",";

        public static bool IsLayoutValid(int layoutId)
        {
            var contentIDs = new StringBuilder();

            var layout = BusinessLayerCommunicator.Layout.GetByLayoutID_NoAccessCheck(layoutId, false);

            ValidatContentSlot(contentIDs, layout.ContentSlot1);
            ValidatContentSlot(contentIDs, layout.ContentSlot2);
            ValidatContentSlot(contentIDs, layout.ContentSlot3);
            ValidatContentSlot(contentIDs, layout.ContentSlot4);
            ValidatContentSlot(contentIDs, layout.ContentSlot5);
            ValidatContentSlot(contentIDs, layout.ContentSlot6);
            ValidatContentSlot(contentIDs, layout.ContentSlot7);
            ValidatContentSlot(contentIDs, layout.ContentSlot8);
            ValidatContentSlot(contentIDs, layout.ContentSlot9);

            return string.IsNullOrWhiteSpace(contentIDs.ToString());
        }

        public static bool IsLicenseValid(ECN_Framework_Entities.Accounts.Customer customer)
        {
            var licenseCheck = new LicenseCheck();
            var blastLicensed = licenseCheck.Current(customer.CustomerID.ToString());
            var blastAvailable = licenseCheck.Available(customer.CustomerID.ToString());

            if (blastLicensed.Equals(UnlimitedLicenseValue, StringComparison.Ordinal))
            {
                blastAvailable = NotApplicable;
            }

            return !blastAvailable.Equals(NoLicense, StringComparison.Ordinal);
        }

        private static void ValidatContentSlot(StringBuilder contentIds, int? contentSlot)
        {
            if (contentSlot > 0)
            {
                var content = BusinessLayerCommunicator.Content.GetByContentID_NoAccessCheck((int)contentSlot, false);
                var isValidated = content.IsValidated.HasValue ? (bool)content.IsValidated : false;
                if (!isValidated)
                {
                    contentIds.Append(ContentIdsSeparator + content.ContentID.ToString());
                }
            }
        }
    }
}