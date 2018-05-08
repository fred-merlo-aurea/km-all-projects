using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.blastsmanager
{
    public partial class preview : System.Web.UI.Page 
    {		
		protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.viewreport))
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Delivery_Report))
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View) 
                    )
            {    
				int requestBlastID = getBlastID();
				if (requestBlastID>0) 
                    ShowPreview(requestBlastID);
                else if (getCampaignItemID() > 0)
                {
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(getCampaignItemID(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    if (blastList.Count > 0)
                        ShowPreview(blastList[0].BlastID);
                    else
                        LabelPreview.Text = "Blasts not Scheduled for this CampaignItem";
                }
                else
                    LabelPreview.Text = "No BlastID Specified";
			}
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();		
			}
		}

        private int getCampaignItemID()
        {
            if (Request.QueryString["CampaignItemID"] != null)
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            else
                return 0;
        }

		private int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            else
                return 0;
		}

		private void ShowPreview(int blastID)
        {
			string body="";
			string EmailSubject="";
			string EmailFrom="";
			string TableOptions="";
			string TemplateSource="";
			int? Slot1;
			int? Slot2;
            int? Slot3 ;
            int? Slot4 ;
            int? Slot5 ;
            int? Slot6 ;
            int? Slot7 ;
            int? Slot8 ;
            int? Slot9 ;

            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(blastID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            if (blast.BlastType == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion.ToString())
            {
                // Getting sample
                ECN_Framework_Entities.Communicator.Sample sample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID_NoAccessCheck(blast.SampleID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                // Get winning blastID and the blast from it
                int winningBlastID = Convert.ToInt32(ECN_Framework_BusinessLayer.Activity.View.BlastActivity.ChampionByProc_NoAccessCheck(blast.SampleID.Value, sample.CustomerID.Value, true, sample.ABWinnerType).Rows[0][0].ToString());
                blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(winningBlastID, true);
            }
            if (blast.Layout==null) 
            {
                body = "Associated Layout or Template has been removed.";
                LabelPreview.Text = body;
			}
            else
            {
                EmailSubject = blast.EmailSubject;
                EmailFrom = blast.EmailFrom;
                TemplateSource = blast.Layout.Template.TemplateSource;
                TableOptions = blast.Layout.TableOptions;
                Slot1 = blast.Layout.ContentSlot1.Value;
                Slot2 = blast.Layout.ContentSlot2;
                Slot3 = blast.Layout.ContentSlot3;
                Slot4 = blast.Layout.ContentSlot4;
                Slot5 = blast.Layout.ContentSlot5;
                Slot6 = blast.Layout.ContentSlot6;
                Slot7 = blast.Layout.ContentSlot7;
                Slot8 = blast.Layout.ContentSlot8;
                Slot9 = blast.Layout.ContentSlot9;
                body = ECN_Framework_BusinessLayer.Communicator.Layout.EmailBody(TemplateSource, blast.Layout.Template.TemplateText, TableOptions, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                //body = DoRSSFeed(body);
                ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed.Replace(ref body,
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID,
                    false, // get HTML content
                    blastID); // use cached content for sent blasts                
                body = RegexUtilities.GetCleanUrlContent(body);
                LabelPreview.Text = body;
            }
		}
	}
}
