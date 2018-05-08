using System;
using System.Configuration;
using System.Data;
using Ecn.Communicator.Main.Admin.Interfaces;
using KM.Common;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class BaseChannelForwardToFriendOperationsHandler : BaseChannelOperationsHandler
    {
        private const string ActivityDomainPathKey = "Activity_DomainPath";

        public BaseChannelForwardToFriendOperationsHandler(
            ILandingPageAssign landingPageAssign) 
            : base(landingPageAssign)
        {
        }

        protected override string PageName
        {
            get
            {
                return "forward to friend";
            }
        }

        protected override int LandingPageID
        {
            get
            {
                return 3;
            }
        }

        protected override string GetPreviewUrl(DataTable datatable, int landingPageAssignId)
        {
            if (datatable == null)
            {
                throw new ArgumentNullException(nameof(datatable));
            }

            var itemArray = datatable.Rows[0].ItemArray;
            var blastId = Guard.ParseStringToInt(itemArray[0]?.ToString());
            var emailId = Guard.ParseStringToInt(itemArray[1]?.ToString());

            return string.Format(
                "{0}/engines/emailtofriend.aspx?e={1}&b={2}&preview={3}",
                ConfigurationManager.AppSettings[ActivityDomainPathKey],
                emailId,
                blastId,
                landingPageAssignId);
        }
    }
}
