using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Ecn.Communicator.Main.Admin.Interfaces;
using ECN_Framework_Entities.Accounts;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class BaseChannelAbuseOperationsHandler : BaseChannelOperationsHandler
    {
        private const string ActivityDomainPathKey = "Activity_DomainPath";

        private ILandingPageAssignContent _landingPageAssignContent;
        private ILandingPageOption _landingPageOption;

        public BaseChannelAbuseOperationsHandler(
            ILandingPageAssign landingPageAssign,
            ILandingPageAssignContent landingPageAssignContent,
            ILandingPageOption landingPageOption)
            : base(landingPageAssign)
        {
            _landingPageAssignContent = landingPageAssignContent;
            _landingPageOption = landingPageOption;
        }

        public IList<LandingPageOption> LandingPageOptions { get; private set; }

        protected override string PageName
        {
            get
            {
                return "report abuse";
            }
        }

        protected override int LandingPageID
        {
            get
            {
                return 4;
            }
        }

        protected override string GetPreviewUrl(DataTable datatable, int landingPageAssignId)
        {
            if (datatable == null)
            {
                throw new ArgumentNullException(nameof(datatable));
            }

            if (datatable.Rows == null 
                || datatable.Rows.Count <= 0 
                || datatable.Columns == null
                || datatable.Columns.Count <= 0)
            {
                throw new InvalidOperationException("Failed to get page Id from datatable");
            }

            var pageId = datatable.Rows[0][0]?.ToString();
            return string.Format(
                "{0}/engines/reportSpam.aspx?p={1}&preview={2}",
                ConfigurationManager.AppSettings[ActivityDomainPathKey],
                pageId,
                landingPageAssignId);
        }

        protected override void InitializeLandingPageOptions()
        {
            LandingPageOptions = _landingPageOption.GetByLPID(LandingPageID);
        }

        protected override void DisplayThankYouText(TextBox txtThankYou, int landingPageAssignId)
        {
            var lpacList = _landingPageAssignContent.GetByLPAID(landingPageAssignId);

            try
            {
                var thankYouText = lpacList
                    .First(x => x.LPOID == LandingPageOptions
                                                    .First(y => y
                                                                .Name
                                                                .ToLower()
                                                                .Replace(" ", string.Empty)
                                                                .Contains("thankyou"))
                                                    .LPOID);

                if (thankYouText != null)
                {
                    txtThankYou.Text = thankYouText.Display;
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
                throw;
            }
        }
    }
}
