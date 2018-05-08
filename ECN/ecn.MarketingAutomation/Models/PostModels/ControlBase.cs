using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Newtonsoft.Json;
using ecn.MarketingAutomation.Models.PostModels.Controls;

namespace ecn.MarketingAutomation.Models.PostModels
{
    [Newtonsoft.Json.JsonConverter(typeof(BaseConverter))]
    public abstract class ControlBase
    {
        private const int FormNameMaxLen = 255;
        private const string TooLong = "Control Name is limited to 255 characters";
        #region properties
        public abstract ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType ControlType { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "category")]
        public string ControlTypeAsString
        {
            get { return ControlType.ToString(); }
            set
            {
                ControlType = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), value);
            }
        }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ECNID")]
        public abstract int ECNID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "x")]
        public abstract decimal xPosition { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "y")]
        public abstract decimal yPosition { get; set; }

        public abstract int MarketingAutomationID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public abstract string ControlID { get; set; }

        [MaxLength(FormNameMaxLen, ErrorMessage = TooLong)]
        [Newtonsoft.Json.JsonProperty(PropertyName = "text")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Control Name is required")]
        public abstract string Text { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "extratext")]
        public abstract string ExtraText { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "isdirty")]
        public abstract bool IsDirty { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "MAControlID")]
        public abstract int MAControlID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "isConfigured")]
        public bool IsConfigured { get; set; }

        #region telerik kendo diagram props

        [Newtonsoft.Json.JsonProperty(PropertyName = "control_label")]
        public string control_label { get { return Text; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "control_text")]
        public string control_text { get { return Text; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "cursor")]
        public string cursor { get { return "pointer"; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "content")]
        public abstract shapecontent content { get; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "selectable")]
        public bool selectable { get { return true; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "serializable")]
        public bool serializable { get { return true; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enable")]
        public bool enable { get { return true; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public abstract string type { get; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "path")]
        public string path { get { return ""; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "autoSize")]
        public bool autoSize { get { return true; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "visual")]
        public bool? visual { get { return null; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "minWidth")]
        public int minWidth { get { return 20; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "minHeight")]
        public int minHeight { get { return 20; } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "width")]
        public abstract decimal width { get; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "height")]
        public abstract decimal height { get; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "editable")]
        public abstract shapeEditable editable { get; set; }


        [Newtonsoft.Json.JsonProperty(PropertyName = "connectors")]
        public List<connectors> connectors
        {
            get
            {
                List<connectors> retList = new List<PostModels.connectors>();
                retList.Add(new PostModels.connectors() { name = "Top" });
                retList.Add(new PostModels.connectors() { name = "Bottom" });
                retList.Add(new PostModels.connectors() { name = "Left" });
                retList.Add(new PostModels.connectors() { name = "Right" });
                retList.Add(new PostModels.connectors() { name = "Auto" });

                return retList;
            }
        }
        [Newtonsoft.Json.JsonProperty(PropertyName = "rotation")]
        public rotation rotation { get { return new rotation(); } }
        [Newtonsoft.Json.JsonProperty(PropertyName = "fill")]
        public abstract string fill { get; }

        #endregion
        #endregion
        #region get models/get objects/get deserialized
        public static List<ControlBase> GetModelsFromObject(List<ECN_Framework_Entities.Communicator.MAControl> controls, List<ECN_Framework_Entities.Communicator.MAConnector> connectors, List<ControlBase> originalControls, KMPlatform.Entity.User user)
        {
            List<ControlBase> retList = new List<PostModels.ControlBase>();
            foreach (ECN_Framework_Entities.Communicator.MAControl c in controls)
            {
                switch (c.ControlType)
                {
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                        Models.PostModels.Controls.CampaignItem ci = new Controls.CampaignItem();

                        ECN_Framework_Entities.Communicator.CampaignItem cItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(c.ECNID, user, false);
                        if (cItem != null && cItem.CampaignItemID > 0)
                        {
                            ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(cItem.CustomerID.Value, false);
                            ECN_Framework_Entities.Communicator.Campaign campaign = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(cItem.CampaignID.Value, user, false);
                            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(cItem.CampaignItemID, user, true);

                            if (cib.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                                ci.editable.remove = false;
                            else
                                ci.editable.remove = true;

                            ci.ControlID = c.ControlID;
                            ci.CustomerID = campaign.CustomerID.Value;
                            ci.CustomerName = cust.CustomerName;
                            ci.CampaignID = campaign.CampaignID;
                            ci.CampaignName = campaign.CampaignName;
                            ci.CampaignItemID = c.ECNID;
                            ci.CampaignItemName = cItem.CampaignItemName;
                            ECN_Framework_Entities.Communicator.MAControl parentCI = FindParentCI(ci, controls, connectors);
                            if (parentCI.ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start)
                            {
                                ci.SubCategory = "Start";
                            }
                            else
                            {
                                ci.SubCategory = "Group";
                            }
                            ci.ECNID = c.ECNID;
                            ci.EmailSubject = cib[0].Blast.EmailSubject;
                            ci.ExtraText = "";
                            ci.FromEmail = cib[0].Blast.EmailFrom;
                            ci.FromName = cib[0].Blast.EmailFromName;
                            ci.IsDirty = false;
                            ci.MarketingAutomationID = c.MarketingAutomationID;
                            ci.MessageID = cib[0].Blast.LayoutID.Value;
                            ci.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cib[0].LayoutID.Value, false).LayoutName;
                            ci.ReplyTo = cib[0].Blast.ReplyTo;
                            ci.CampaignItemTemplateID = cItem.CampaignItemTemplateID.HasValue ? cItem.CampaignItemTemplateID.Value :-1;
                            if (ci.CampaignItemTemplateID > 0)
                                ci.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(ci.CampaignItemTemplateID, user, false).TemplateName;
                            else
                                ci.CampaignItemTemplateName = string.Empty;
                            ci.BlastField1 = cItem.BlastField1;
                            ci.BlastField2 = cItem.BlastField2;
                            ci.BlastField3 = cItem.BlastField3;
                            ci.BlastField4 = cItem.BlastField4;
                            ci.BlastField5 = cItem.BlastField5;
                            ci.SelectedGroupFilters = GetFilters(cib, user, true);
                            ci.SelectedGroups = GetGroups(cib, true);
                            ci.SendTime = cItem.SendTime.Value;
                            ci.SuppressedGroupFilters = GetFilters(cib, user, false);
                            ci.SuppressedGroups = GetGroups(cib, false);
                            ci.xPosition = c.xPosition;
                            ci.yPosition = c.yPosition;
                            ci.MAControlID = c.MAControlID;
                            ci.IsConfigured = true;
                            ci.Text = c.Text;
                            retList.Add(ci);
                        }
                        else
                        {

                        }
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click://This is a blast with a smart segment based off the previous blast
                        Controls.Click click = new Controls.Click();
                        ECN_Framework_Entities.Communicator.CampaignItem bClick = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibClick = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bClick.CampaignItemID, user, true);

                        if (cibClick.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            click.editable.remove = false;
                        else
                            click.editable.remove = true;

                        click.ControlID = c.ControlID;
                        click.ECNID = c.ECNID;
                        click.ExtraText = "";
                        click.FromEmail = cibClick[0].Blast.EmailFrom;
                        click.FromName = cibClick[0].Blast.EmailFromName;
                        click.IsDirty = false;
                        click.EmailSubject = cibClick[0].Blast.EmailSubject;
                        click.MarketingAutomationID = c.MarketingAutomationID;
                        click.MessageID = cibClick[0].Blast.LayoutID.Value;
                        click.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibClick[0].LayoutID.Value, false).LayoutName;
                        click.ReplyTo = bClick.ReplyTo;
                        click.CampaignItemTemplateID = bClick.CampaignItemTemplateID.HasValue ? bClick.CampaignItemTemplateID.Value : -1;
                        if (click.CampaignItemTemplateID > 0)
                            click.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(click.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            click.CampaignItemTemplateName = string.Empty;
                        click.BlastField1 = bClick.BlastField1;
                        click.BlastField2 = bClick.BlastField2;
                        click.BlastField3 = bClick.BlastField3;
                        click.BlastField4 = bClick.BlastField4;
                        click.BlastField5 = bClick.BlastField5;
                        click.Text = c.Text;
                        click.xPosition = c.xPosition;
                        click.yPosition = c.yPosition;
                        click.MAControlID = c.MAControlID;
                        click.CampaignItemName = bClick.CampaignItemName;
                        click.EstSendTime = cibClick[0].Blast.SendTime;
                        click.IsConfigured = true;
                        retList.Add(click);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click://This is a click trigger
                        Controls.Direct_Click direct_click = new Controls.Direct_Click();

                        ECN_Framework_Entities.Communicator.LayoutPlans lp = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(lp.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciDC = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(b.BlastID, false);
                        if (lp.Criteria.StartsWith("http"))
                        {
                            direct_click.AnyLink = false;
                            direct_click.ActualLink = lp.Criteria;
                            try
                            {
                                DataTable dtLinkAlias = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(b.CustomerID.Value, lp.LayoutID.Value, user);

                                direct_click.SpecificLink = dtLinkAlias.Select("Link = '" + lp.Criteria + "'").First()["Alias"].ToString();
                            }
                            catch (Exception ex)
                            {
                                direct_click.SpecificLink = "";
                            }
                        }
                        else
                        {
                            direct_click.AnyLink = true;
                            direct_click.ActualLink = "";
                            direct_click.SpecificLink = "";
                        }
                        direct_click.ControlID = c.ControlID;
                        direct_click.ECNID = c.ECNID;
                        direct_click.EmailSubject = b.EmailSubject;
                        direct_click.ExtraText = "";
                        direct_click.FromEmail = b.EmailFrom;
                        direct_click.FromName = b.EmailFromName;
                        direct_click.IsDirty = false;
                        direct_click.MarketingAutomationID = c.MarketingAutomationID;
                        direct_click.MessageID = b.LayoutID.Value;
                        direct_click.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(b.LayoutID.Value, false).LayoutName;
                        direct_click.ReplyTo = b.ReplyTo;
                        direct_click.CampaignItemTemplateID = ciDC.CampaignItemTemplateID.HasValue ? ciDC.CampaignItemTemplateID.Value : -1;
                        if (direct_click.CampaignItemTemplateID > 0)
                            direct_click.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(direct_click.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            direct_click.CampaignItemTemplateName = string.Empty;
                        direct_click.BlastField1 = ciDC.BlastField1;
                        direct_click.BlastField2 = ciDC.BlastField2;
                        direct_click.BlastField3 = ciDC.BlastField3;
                        direct_click.BlastField4 = ciDC.BlastField4;
                        direct_click.BlastField5 = ciDC.BlastField5;
                        direct_click.Text = c.Text;
                        direct_click.xPosition = c.xPosition;
                        direct_click.yPosition = c.yPosition;
                        direct_click.IsCancelled = lp.Status.ToLower().Equals("y") ? false : lp.Status.ToLower().Equals("p") ? false : true;
                        direct_click.CancelDate = lp.UpdatedDate.HasValue ? lp.UpdatedDate.Value : lp.CreatedDate.Value;
                        if (((direct_click.IsCancelled) && (direct_click.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(lp.BlastID.Value, lp.CustomerID.Value)))
                            direct_click.editable.remove = false;
                        else
                            direct_click.editable.remove = true;
                        direct_click.MAControlID = c.MAControlID;
                        direct_click.CampaignItemName = ciDC.CampaignItemName;
                        direct_click.IsConfigured = true;
                        retList.Add(direct_click);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form://This is a click trigger
                        Controls.Form form = new Controls.Form();
                        Controls.Form origForm = new Controls.Form();
                        try
                        {
                            origForm = (Controls.Form)originalControls.FirstOrDefault(x => x.ControlID == c.ControlID);
                        }
                        catch (Exception ex) { }
                        form.ControlID = c.ControlID;
                        form.ECNID = c.ECNID;
                        form.ExtraText = "";
                        form.IsDirty = false;
                        form.MarketingAutomationID = c.MarketingAutomationID;
                        form.FormID = c.ECNID;
                        form.FormName = ECN_Framework_BusinessLayer.FormDesigner.Form.GetByFormID_NoAccessCheck(c.ECNID).Name;
                        
                        form.SpecificLink = origForm.SpecificLink;
                        form.ActualLink = origForm.ActualLink;
                        form.xPosition = c.xPosition;
                        form.yPosition = c.yPosition;
                        form.MAControlID = c.MAControlID;
                        form.Text = c.Text;
                        form.IsConfigured = true;
                        retList.Add(form);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon://This is a click trigger
                        Controls.FormAbandon formabandon = new Controls.FormAbandon();

                        ECN_Framework_Entities.Communicator.LayoutPlans lpformab = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast bformab = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(lpformab.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciformab = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(bformab.BlastID, false);
                        if (lpformab.Criteria.StartsWith("http"))
                        {
                            formabandon.AnyLink = false;
                            formabandon.ActualLink = lpformab.Criteria;
                            try
                            {
                                DataTable dtLinkAlias = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(bformab.CustomerID.Value, lpformab.LayoutID.Value, user);

                                formabandon.SpecificLink = dtLinkAlias.Select("Link = '" + lpformab.Criteria + "'").First()["Alias"].ToString();
                            }
                            catch (Exception ex)
                            {
                                formabandon.SpecificLink = "";
                            }
                        }
                        else
                        {
                            formabandon.AnyLink = true;
                            formabandon.ActualLink = "";
                            formabandon.SpecificLink = "";
                        }
                        formabandon.ControlID = c.ControlID;
                        formabandon.ECNID = c.ECNID;
                        formabandon.EmailSubject = bformab.EmailSubject;
                        formabandon.ExtraText = "";
                        formabandon.FromEmail = bformab.EmailFrom;
                        formabandon.FromName = bformab.EmailFromName;
                        formabandon.IsDirty = false;
                        formabandon.MarketingAutomationID = c.MarketingAutomationID;
                        formabandon.MessageID = bformab.LayoutID.Value;
                        formabandon.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(bformab.LayoutID.Value, false).LayoutName;
                        formabandon.ReplyTo = bformab.ReplyTo;
                        formabandon.CampaignItemTemplateID = ciformab.CampaignItemTemplateID.HasValue ? ciformab.CampaignItemTemplateID.Value : -1;
                        if (formabandon.CampaignItemTemplateID > 0)
                            formabandon.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(formabandon.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            formabandon.CampaignItemTemplateName = string.Empty;
                        formabandon.BlastField1 = ciformab.BlastField1;
                        formabandon.BlastField2 = ciformab.BlastField2;
                        formabandon.BlastField3 = ciformab.BlastField3;
                        formabandon.BlastField4 = ciformab.BlastField4;
                        formabandon.BlastField5 = ciformab.BlastField5;
                        formabandon.Text = c.Text;
                        formabandon.xPosition = c.xPosition;
                        formabandon.yPosition = c.yPosition;
                        formabandon.IsCancelled = lpformab.Status.ToLower().Equals("y") ? false : lpformab.Status.ToLower().Equals("p") ? false : true;
                        formabandon.CancelDate = lpformab.UpdatedDate.HasValue ? lpformab.UpdatedDate.Value : lpformab.CreatedDate.Value;
                        if (((formabandon.IsCancelled) && (formabandon.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(lpformab.BlastID.Value, lpformab.CustomerID.Value)))
                            formabandon.editable.remove = false;
                        else
                            formabandon.editable.remove = true;
                        formabandon.MAControlID = c.MAControlID;
                        formabandon.CampaignItemName = ciformab.CampaignItemName;
                        formabandon.IsConfigured = true;
                        retList.Add(formabandon);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit://This is a click trigger
                        Controls.FormSubmit formsubmit = new Controls.FormSubmit();

                        ECN_Framework_Entities.Communicator.LayoutPlans lpformsub = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast bformsub = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(lpformsub.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciformsub = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(bformsub.BlastID, false);
                        if (lpformsub.Criteria.StartsWith("http"))
                        {
                            formsubmit.AnyLink = false;
                            formsubmit.ActualLink = lpformsub.Criteria;
                            try
                            {
                                DataTable dtLinkAlias = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(bformsub.CustomerID.Value, lpformsub.LayoutID.Value, user);

                                formsubmit.SpecificLink = dtLinkAlias.Select("Link = '" + lpformsub.Criteria + "'").First()["Alias"].ToString();
                            }
                            catch (Exception ex)
                            {
                                formsubmit.SpecificLink = "";
                            }
                        }
                        else
                        {
                            formsubmit.AnyLink = true;
                            formsubmit.ActualLink = "";
                            formsubmit.SpecificLink = "";
                        }
                        formsubmit.ControlID = c.ControlID;
                        formsubmit.ECNID = c.ECNID;
                        formsubmit.EmailSubject = bformsub.EmailSubject;
                        formsubmit.ExtraText = "";
                        formsubmit.FromEmail = bformsub.EmailFrom;
                        formsubmit.FromName = bformsub.EmailFromName;
                        formsubmit.IsDirty = false;
                        formsubmit.MarketingAutomationID = c.MarketingAutomationID;
                        formsubmit.MessageID = bformsub.LayoutID.Value;
                        formsubmit.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(bformsub.LayoutID.Value, false).LayoutName;
                        formsubmit.ReplyTo = bformsub.ReplyTo;
                        formsubmit.CampaignItemTemplateID = ciformsub.CampaignItemTemplateID.HasValue ? ciformsub.CampaignItemTemplateID.Value : -1;
                        if (formsubmit.CampaignItemTemplateID > 0)
                            formsubmit.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(formsubmit.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            formsubmit.CampaignItemTemplateName = string.Empty;
                        formsubmit.BlastField1 = ciformsub.BlastField1;
                        formsubmit.BlastField2 = ciformsub.BlastField2;
                        formsubmit.BlastField3 = ciformsub.BlastField3;
                        formsubmit.BlastField4 = ciformsub.BlastField4;
                        formsubmit.BlastField5 = ciformsub.BlastField5;
                        formsubmit.Text = c.Text;
                        formsubmit.xPosition = c.xPosition;
                        formsubmit.yPosition = c.yPosition;
                        formsubmit.IsCancelled = lpformsub.Status.ToLower().Equals("y") ? false : lpformsub.Status.ToLower().Equals("p") ? false : true;
                        formsubmit.CancelDate = lpformsub.UpdatedDate.HasValue ? lpformsub.UpdatedDate.Value : lpformsub.CreatedDate.Value;
                        if (((formsubmit.IsCancelled) && (formsubmit.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(lpformsub.BlastID.Value, lpformsub.CustomerID.Value)))
                            formsubmit.editable.remove = false;
                        else
                            formsubmit.editable.remove = true;
                        formsubmit.MAControlID = c.MAControlID;
                        formsubmit.CampaignItemName = ciformsub.CampaignItemName;
                        formsubmit.IsConfigured = true;
                        retList.Add(formsubmit);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open://this is an open trigger
                        Controls.Direct_Open direct_open = new Controls.Direct_Open();
                        ECN_Framework_Entities.Communicator.LayoutPlans lp2 = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast b2 = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(lp2.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciDO = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(b2.BlastID, false);

                        direct_open.ControlID = c.ControlID;
                        direct_open.ECNID = c.ECNID;
                        direct_open.EmailSubject = b2.EmailSubject;
                        direct_open.ExtraText = "";
                        direct_open.FromEmail = b2.EmailFrom;
                        direct_open.FromName = b2.EmailFromName;
                        direct_open.IsDirty = false;
                        direct_open.MarketingAutomationID = c.MarketingAutomationID;
                        direct_open.MessageID = b2.LayoutID.Value;
                        direct_open.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(b2.LayoutID.Value, false).LayoutName;
                        direct_open.ReplyTo = b2.ReplyTo;
                        direct_open.CampaignItemTemplateID = ciDO.CampaignItemTemplateID.HasValue ? ciDO.CampaignItemTemplateID.Value : -1;
                        if (direct_open.CampaignItemTemplateID > 0)
                            direct_open.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(direct_open.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            direct_open.CampaignItemTemplateName = string.Empty;
                        direct_open.BlastField1 = ciDO.BlastField1;
                        direct_open.BlastField2 = ciDO.BlastField2;
                        direct_open.BlastField3 = ciDO.BlastField3;
                        direct_open.BlastField4 = ciDO.BlastField4;
                        direct_open.BlastField5 = ciDO.BlastField5;
                        direct_open.Text = c.Text;
                        direct_open.xPosition = c.xPosition;
                        direct_open.yPosition = c.yPosition;
                        direct_open.IsCancelled = lp2.Status.ToLower().Equals("y") ? false : lp2.Status.ToLower().Equals("p") ? false : true;
                        direct_open.CancelDate = lp2.UpdatedDate.HasValue ? lp2.UpdatedDate.Value : lp2.CreatedDate.Value;
                        if (((direct_open.IsCancelled) && (direct_open.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(lp2.BlastID.Value, lp2.CustomerID.Value)))
                            direct_open.editable.remove = false;
                        else
                            direct_open.editable.remove = true;
                        direct_open.MAControlID = c.MAControlID;
                        direct_open.CampaignItemName = ciDO.CampaignItemName;
                        direct_open.IsConfigured = true;
                        retList.Add(direct_open);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen://this is an open trigger
                        Controls.Direct_NoOpen direct_NoOpen = new Controls.Direct_NoOpen();
                        ECN_Framework_Entities.Communicator.TriggerPlans lp3 = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByTriggerPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast bDirectNoOpen = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(lp3.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciDNoOpen = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(bDirectNoOpen.BlastID, false);

                        direct_NoOpen.ControlID = c.ControlID;
                        direct_NoOpen.ECNID = c.ECNID;
                        direct_NoOpen.EmailSubject = bDirectNoOpen.EmailSubject;
                        direct_NoOpen.ExtraText = "";
                        direct_NoOpen.FromEmail = bDirectNoOpen.EmailFrom;
                        direct_NoOpen.FromName = bDirectNoOpen.EmailFromName;
                        direct_NoOpen.IsDirty = false;
                        direct_NoOpen.MarketingAutomationID = c.MarketingAutomationID;
                        direct_NoOpen.MessageID = bDirectNoOpen.LayoutID.Value;
                        direct_NoOpen.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(bDirectNoOpen.LayoutID.Value, false).LayoutName;
                        direct_NoOpen.ReplyTo = bDirectNoOpen.ReplyTo;
                        direct_NoOpen.CampaignItemTemplateID = ciDNoOpen.CampaignItemTemplateID.HasValue ? ciDNoOpen.CampaignItemTemplateID.Value : -1;
                        if (direct_NoOpen.CampaignItemTemplateID > 0)
                            direct_NoOpen.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(direct_NoOpen.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            direct_NoOpen.CampaignItemTemplateName = string.Empty;
                        direct_NoOpen.BlastField1 = ciDNoOpen.BlastField1;
                        direct_NoOpen.BlastField2 = ciDNoOpen.BlastField2;
                        direct_NoOpen.BlastField3 = ciDNoOpen.BlastField3;
                        direct_NoOpen.BlastField4 = ciDNoOpen.BlastField4;
                        direct_NoOpen.BlastField5 = ciDNoOpen.BlastField5;
                        direct_NoOpen.Text = c.Text;
                        direct_NoOpen.xPosition = c.xPosition;
                        direct_NoOpen.yPosition = c.yPosition;
                        direct_NoOpen.IsCancelled = lp3.Status.ToLower().Equals("y") ? false : lp3.Status.ToLower().Equals("p") ? false : true;
                        direct_NoOpen.CancelDate = lp3.UpdatedDate.HasValue ? lp3.UpdatedDate.Value : lp3.CreatedDate.Value;
                        if (((direct_NoOpen.IsCancelled) && (direct_NoOpen.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(lp3.BlastID.Value, lp3.CustomerID.Value)))
                            direct_NoOpen.editable.remove = false;
                        else
                            direct_NoOpen.editable.remove = true;
                        direct_NoOpen.MAControlID = c.MAControlID;
                        direct_NoOpen.CampaignItemName = ciDNoOpen.CampaignItemName;
                        direct_NoOpen.IsConfigured = true;
                        retList.Add(direct_NoOpen);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End://this means the end of an automation-- no ECN ID
                        Controls.End end = new Controls.End();
                        end.ControlID = c.ControlID;
                        end.ECNID = 0;
                        end.ExtraText = "";
                        end.MarketingAutomationID = c.MarketingAutomationID;
                        end.Text = "";
                        end.xPosition = c.xPosition;
                        end.yPosition = c.yPosition;
                        end.MAControlID = c.MAControlID;
                        end.IsConfigured = true;
                        retList.Add(end);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group://This is a group placeholder
                        Controls.Group group = new Controls.Group();
                        ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(c.ECNID);
                        ECN_Framework_Entities.Accounts.Customer gCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(g.CustomerID, false);
                        group.ControlID = c.ControlID;
                        group.CustomerID = g.CustomerID;
                        group.CustomerName = gCust.CustomerName;
                        group.ECNID = c.ECNID;
                        group.ExtraText = "";
                        group.GroupID = g.GroupID;
                        group.GroupName = g.GroupName;
                        group.IsDirty = false;
                        group.MarketingAutomationID = c.MarketingAutomationID;
                        group.Text = c.Text;
                        group.xPosition = c.xPosition;
                        group.yPosition = c.yPosition;
                        group.MAControlID = c.MAControlID;
                        group.IsConfigured = true;
                        group.editable.remove = false;
                        retList.Add(group);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick://This is a blast with a no click smart segment
                        Controls.NoClick noclick = new Controls.NoClick();
                        ECN_Framework_Entities.Communicator.CampaignItem bNoClick = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibNoClick = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bNoClick.CampaignItemID, user, true);

                        if (cibNoClick.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            noclick.editable.remove = false;
                        else
                            noclick.editable.remove = true;

                        noclick.ControlID = c.ControlID;
                        noclick.ECNID = c.ECNID;
                        noclick.ExtraText = "";
                        noclick.FromEmail = cibNoClick[0].Blast.EmailFrom;
                        noclick.FromName = cibNoClick[0].Blast.EmailFromName;
                        noclick.EmailSubject = cibNoClick[0].Blast.EmailSubject;
                        noclick.IsDirty = false;
                        noclick.MarketingAutomationID = c.MarketingAutomationID;
                        noclick.MessageID = cibNoClick[0].Blast.LayoutID.Value;
                        noclick.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibNoClick[0].LayoutID.Value, false).LayoutName;
                        noclick.ReplyTo = bNoClick.ReplyTo;
                        noclick.Text = c.Text;
                        noclick.xPosition = c.xPosition;
                        noclick.yPosition = c.yPosition;
                        noclick.MAControlID = c.MAControlID;
                        noclick.CampaignItemName = bNoClick.CampaignItemName;
                        noclick.EstSendTime = cibNoClick[0].Blast.SendTime;
                        noclick.CampaignItemTemplateID = bNoClick.CampaignItemTemplateID.HasValue ? bNoClick.CampaignItemTemplateID.Value : -1;
                        if (noclick.CampaignItemTemplateID > 0)
                            noclick.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(noclick.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            noclick.CampaignItemTemplateName = string.Empty;
                        noclick.BlastField1 = bNoClick.BlastField1;
                        noclick.BlastField2 = bNoClick.BlastField2;
                        noclick.BlastField3 = bNoClick.BlastField3;
                        noclick.BlastField4 = bNoClick.BlastField4;
                        noclick.BlastField5 = bNoClick.BlastField5;
                        noclick.IsConfigured = true;
                        retList.Add(noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen://This is a blast with a no open smart segment
                        Controls.NoOpen noopen = new Controls.NoOpen();
                        ECN_Framework_Entities.Communicator.CampaignItem bNoOpen = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibNoOpen = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bNoOpen.CampaignItemID, user, true);

                        if (cibNoOpen.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            noopen.editable.remove = false;
                        else
                            noopen.editable.remove = true;

                        noopen.ControlID = c.ControlID;
                        noopen.ECNID = c.ECNID;
                        noopen.ExtraText = "";
                        noopen.FromEmail = cibNoOpen[0].Blast.EmailFrom;
                        noopen.FromName = cibNoOpen[0].Blast.EmailFromName;
                        noopen.EmailSubject = cibNoOpen[0].Blast.EmailSubject;
                        noopen.IsDirty = false;
                        noopen.MarketingAutomationID = c.MarketingAutomationID;
                        noopen.MessageID = cibNoOpen[0].Blast.LayoutID.Value;
                        noopen.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibNoOpen[0].LayoutID.Value, false).LayoutName;
                        noopen.ReplyTo = bNoOpen.ReplyTo;
                        noopen.CampaignItemTemplateID = bNoOpen.CampaignItemTemplateID.HasValue ? bNoOpen.CampaignItemTemplateID.Value : -1;
                        if (noopen.CampaignItemTemplateID > 0)
                            noopen.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(noopen.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            noopen.CampaignItemTemplateName = string.Empty;
                        noopen.BlastField1 = bNoOpen.BlastField1;
                        noopen.BlastField2 = bNoOpen.BlastField2;
                        noopen.BlastField3 = bNoOpen.BlastField3;
                        noopen.BlastField4 = bNoOpen.BlastField4;
                        noopen.BlastField5 = bNoOpen.BlastField5;
                        noopen.Text = c.Text;
                        noopen.xPosition = c.xPosition;
                        noopen.yPosition = c.yPosition;
                        noopen.MAControlID = c.MAControlID;
                        noopen.CampaignItemName = bNoOpen.CampaignItemName;
                        noopen.EstSendTime = cibNoOpen[0].Blast.SendTime;
                        noopen.IsConfigured = true;
                        retList.Add(noopen);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent://This is a blast with a not sent smart segment
                        Controls.NotSent notsent = new Controls.NotSent();
                        ECN_Framework_Entities.Communicator.CampaignItem bNotSent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibNotSent = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bNotSent.CampaignItemID, user, true);

                        if (cibNotSent.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            notsent.editable.remove = false;
                        else
                            notsent.editable.remove = true;

                        notsent.ControlID = c.ControlID;
                        notsent.ECNID = c.ECNID;
                        notsent.ExtraText = "";
                        notsent.FromEmail = cibNotSent[0].Blast.EmailFrom;
                        notsent.FromName = cibNotSent[0].Blast.EmailFromName;
                        notsent.EmailSubject = cibNotSent[0].Blast.EmailSubject;
                        notsent.IsDirty = false;
                        notsent.MarketingAutomationID = c.MarketingAutomationID;
                        notsent.MessageID = cibNotSent[0].Blast.LayoutID.Value;
                        notsent.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibNotSent[0].LayoutID.Value, false).LayoutName;
                        notsent.ReplyTo = bNotSent.ReplyTo;
                        notsent.CampaignItemTemplateID = bNotSent.CampaignItemTemplateID.HasValue ? bNotSent.CampaignItemTemplateID.Value : -1;
                        if (notsent.CampaignItemTemplateID > 0)
                            notsent.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(notsent.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            notsent.CampaignItemTemplateName = string.Empty;
                        notsent.BlastField1 = bNotSent.BlastField1;
                        notsent.BlastField2 = bNotSent.BlastField2;
                        notsent.BlastField3 = bNotSent.BlastField3;
                        notsent.BlastField4 = bNotSent.BlastField4;
                        notsent.BlastField5 = bNotSent.BlastField5;
                        notsent.Text = c.Text;
                        notsent.xPosition = c.xPosition;
                        notsent.yPosition = c.yPosition;
                        notsent.MAControlID = c.MAControlID;
                        notsent.CampaignItemName = bNotSent.CampaignItemName;
                        notsent.EstSendTime = cibNotSent[0].Blast.SendTime;
                        notsent.IsConfigured = true;
                        retList.Add(notsent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open://This is a blast with an open smart segment
                        Controls.Open open = new Controls.Open();
                        ECN_Framework_Entities.Communicator.CampaignItem bOpen = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibOpen = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bOpen.CampaignItemID, user, true);

                        if (cibOpen.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            open.editable.remove = false;
                        else
                            open.editable.remove = true;

                        open.ControlID = c.ControlID;
                        open.ECNID = c.ECNID;
                        open.ExtraText = "";
                        open.FromEmail = cibOpen[0].Blast.EmailFrom;
                        open.FromName = cibOpen[0].Blast.EmailFromName;
                        open.EmailSubject = cibOpen[0].Blast.EmailSubject;
                        open.IsDirty = false;
                        open.MarketingAutomationID = c.MarketingAutomationID;
                        open.MessageID = cibOpen[0].Blast.LayoutID.Value;
                        open.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibOpen[0].LayoutID.Value, false).LayoutName;
                        open.ReplyTo = bOpen.ReplyTo;
                        open.CampaignItemTemplateID = bOpen.CampaignItemTemplateID.HasValue ? bOpen.CampaignItemTemplateID.Value : -1;
                        if (open.CampaignItemTemplateID > 0)
                            open.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(open.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            open.CampaignItemTemplateName = string.Empty;
                        open.BlastField1 = bOpen.BlastField1;
                        open.BlastField2 = bOpen.BlastField2;
                        open.BlastField3 = bOpen.BlastField3;
                        open.BlastField4 = bOpen.BlastField4;
                        open.BlastField5 = bOpen.BlastField5;
                        open.Text = c.Text;
                        open.xPosition = c.xPosition;
                        open.yPosition = c.yPosition;
                        open.MAControlID = c.MAControlID;
                        open.CampaignItemName = bOpen.CampaignItemName;
                        open.EstSendTime = cibOpen[0].Blast.SendTime;
                        open.IsConfigured = true;
                        retList.Add(open);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick://This is a blast with an open/no click smart segment
                        Controls.Open_NoClick open_noclick = new Controls.Open_NoClick();
                        ECN_Framework_Entities.Communicator.CampaignItem bOpen_NoClick = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibOpen_NoClick = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bOpen_NoClick.CampaignItemID, user, true);

                        if (cibOpen_NoClick.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            open_noclick.editable.remove = false;
                        else
                            open_noclick.editable.remove = true;

                        open_noclick.ControlID = c.ControlID;
                        open_noclick.ECNID = c.ECNID;
                        open_noclick.ExtraText = "";
                        open_noclick.FromEmail = cibOpen_NoClick[0].Blast.EmailFrom;
                        open_noclick.FromName = cibOpen_NoClick[0].Blast.EmailFromName;
                        open_noclick.EmailSubject = cibOpen_NoClick[0].Blast.EmailSubject;
                        open_noclick.IsDirty = false;
                        open_noclick.MarketingAutomationID = c.MarketingAutomationID;
                        open_noclick.MessageID = cibOpen_NoClick[0].Blast.LayoutID.Value;
                        open_noclick.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibOpen_NoClick[0].LayoutID.Value, false).LayoutName;
                        open_noclick.ReplyTo = bOpen_NoClick.ReplyTo;
                        open_noclick.CampaignItemTemplateID = bOpen_NoClick.CampaignItemTemplateID.HasValue ? bOpen_NoClick.CampaignItemTemplateID.Value : -1;
                        if (open_noclick.CampaignItemTemplateID > 0)
                            open_noclick.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(open_noclick.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            open_noclick.CampaignItemTemplateName = string.Empty;
                        open_noclick.BlastField1 = bOpen_NoClick.BlastField1;
                        open_noclick.BlastField2 = bOpen_NoClick.BlastField2;
                        open_noclick.BlastField3 = bOpen_NoClick.BlastField3;
                        open_noclick.BlastField4 = bOpen_NoClick.BlastField4;
                        open_noclick.BlastField5 = bOpen_NoClick.BlastField5;
                        open_noclick.Text = c.Text;
                        open_noclick.xPosition = c.xPosition;
                        open_noclick.yPosition = c.yPosition;
                        open_noclick.MAControlID = c.MAControlID;
                        open_noclick.CampaignItemName = bOpen_NoClick.CampaignItemName;
                        open_noclick.EstSendTime = cibOpen_NoClick[0].Blast.SendTime;
                        open_noclick.IsConfigured = true;
                        retList.Add(open_noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent://This is a blast with a sent smart segment
                        Controls.Sent sent = new Controls.Sent();
                        ECN_Framework_Entities.Communicator.CampaignItem bSent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibSent = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bSent.CampaignItemID, user, true);

                        if (cibSent.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            sent.editable.remove = false;
                        else
                            sent.editable.remove = true;

                        sent.ControlID = c.ControlID;
                        sent.ECNID = c.ECNID;
                        sent.ExtraText = "";
                        sent.FromEmail = cibSent[0].Blast.EmailFrom;
                        sent.FromName = cibSent[0].Blast.EmailFromName;
                        sent.EmailSubject = cibSent[0].Blast.EmailSubject;
                        sent.IsDirty = false;
                        sent.MarketingAutomationID = c.MarketingAutomationID;
                        sent.MessageID = cibSent[0].Blast.LayoutID.Value;
                        sent.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibSent[0].LayoutID.Value, false).LayoutName;
                        sent.ReplyTo = bSent.ReplyTo;
                        sent.CampaignItemTemplateID = bSent.CampaignItemTemplateID.HasValue ? bSent.CampaignItemTemplateID.Value : -1;
                        if (sent.CampaignItemTemplateID > 0)
                            sent.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(sent.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            sent.CampaignItemTemplateName = string.Empty;
                        sent.BlastField1 = bSent.BlastField1;
                        sent.BlastField2 = bSent.BlastField2;
                        sent.BlastField3 = bSent.BlastField3;
                        sent.BlastField4 = bSent.BlastField4;
                        sent.BlastField5 = bSent.BlastField5;
                        sent.Text = c.Text;
                        sent.xPosition = c.xPosition;
                        sent.yPosition = c.yPosition;
                        sent.MAControlID = c.MAControlID;
                        sent.CampaignItemName = bSent.CampaignItemName;
                        sent.EstSendTime = cibSent[0].Blast.SendTime;
                        sent.IsConfigured = true;
                        retList.Add(sent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start://This is the start of an automation--no ECN ID
                        Controls.Start start = new Controls.Start();
                        start.ControlID = c.ControlID;
                        start.ECNID = 0;
                        start.ExtraText = "";
                        start.IsDirty = false;
                        start.MarketingAutomationID = c.MarketingAutomationID;
                        start.Text = "";
                        start.xPosition = c.xPosition;
                        start.yPosition = c.yPosition;
                        start.MAControlID = c.MAControlID;
                        start.IsConfigured = true;
                        retList.Add(start);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:// this is a group trigger--subscribe
                        Controls.Subscribe subscribe = new Controls.Subscribe();
                        ECN_Framework_Entities.Communicator.LayoutPlans groupSub = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast b3 = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(groupSub.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciSub = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(b3.BlastID, false);
                        //if (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(groupSub.BlastID.Value, groupSub.CustomerID.Value))
                        //    subscribe.editable.remove = false;
                        //else
                        //    subscribe.editable.remove = true;

                        subscribe.ControlID = c.ControlID;
                        subscribe.ECNID = c.ECNID;
                        subscribe.EmailSubject = b3.EmailSubject;
                        subscribe.ExtraText = "";
                        subscribe.FromEmail = b3.EmailFrom;
                        subscribe.FromName = b3.EmailFromName;
                        subscribe.IsDirty = false;
                        subscribe.MarketingAutomationID = c.MarketingAutomationID;
                        subscribe.MessageID = b3.LayoutID.Value;
                        subscribe.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(b3.LayoutID.Value, false).LayoutName;
                        subscribe.ReplyTo = b3.ReplyTo;
                        subscribe.CampaignItemTemplateID = ciSub.CampaignItemTemplateID.HasValue ? ciSub.CampaignItemTemplateID.Value : -1;
                        if (subscribe.CampaignItemTemplateID > 0)
                            subscribe.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(subscribe.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            subscribe.CampaignItemTemplateName = string.Empty;
                        subscribe.BlastField1 = ciSub.BlastField1;
                        subscribe.BlastField2 = ciSub.BlastField2;
                        subscribe.BlastField3 = ciSub.BlastField3;
                        subscribe.BlastField4 = ciSub.BlastField4;
                        subscribe.BlastField5 = ciSub.BlastField5;
                        subscribe.Text = c.Text;
                        subscribe.xPosition = c.xPosition;
                        subscribe.yPosition = c.yPosition;
                        subscribe.IsCancelled = groupSub.Status.ToLower().Equals("y") ? false : groupSub.Status.ToLower().Equals("p") ? false : true;
                        subscribe.CancelDate = groupSub.UpdatedDate.HasValue ? groupSub.UpdatedDate.Value : groupSub.CreatedDate.Value;

                        if (((subscribe.IsCancelled) && (subscribe.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(groupSub.BlastID.Value, groupSub.CustomerID.Value)))
                            subscribe.editable.remove = false;
                        else
                            subscribe.editable.remove = true;
                        subscribe.MAControlID = c.MAControlID;
                        subscribe.CampaignItemName = ciSub.CampaignItemName;
                        subscribe.IsConfigured = true;
                        retList.Add(subscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed://this is a blast with a suppressed smart segment
                        Controls.Suppressed suppressed = new Controls.Suppressed();
                        ECN_Framework_Entities.Communicator.CampaignItem bSuppressed = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(c.ECNID, false);
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibSuppressed = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(bSuppressed.CampaignItemID, user, true);

                        if (cibSuppressed.Any(x => x.BlastID.HasValue ? IsActiveOrSent(x.BlastID.Value, x.CustomerID.Value) == true : false || x.Blast.StatusCode.ToLower().Equals("cancelled")))
                            suppressed.editable.remove = false;
                        else
                            suppressed.editable.remove = true;

                        suppressed.ControlID = c.ControlID;
                        suppressed.ECNID = c.ECNID;
                        suppressed.ExtraText = "";
                        suppressed.FromEmail = cibSuppressed[0].Blast.EmailFrom;
                        suppressed.EmailSubject = cibSuppressed[0].Blast.EmailSubject;
                        suppressed.FromName = cibSuppressed[0].Blast.EmailFromName;
                        suppressed.IsDirty = false;
                        suppressed.MarketingAutomationID = c.MarketingAutomationID;
                        suppressed.MessageID = cibSuppressed[0].Blast.LayoutID.Value;
                        suppressed.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cibSuppressed[0].LayoutID.Value, false).LayoutName;
                        suppressed.ReplyTo = bSuppressed.ReplyTo;
                        suppressed.CampaignItemTemplateID = bSuppressed.CampaignItemTemplateID.HasValue ? bSuppressed.CampaignItemTemplateID.Value : -1;
                        if (suppressed.CampaignItemTemplateID > 0)
                            suppressed.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(suppressed.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            suppressed.CampaignItemTemplateName = string.Empty;
                        suppressed.BlastField1 = bSuppressed.BlastField1;
                        suppressed.BlastField2 = bSuppressed.BlastField2;
                        suppressed.BlastField3 = bSuppressed.BlastField3;
                        suppressed.BlastField4 = bSuppressed.BlastField4;
                        suppressed.BlastField5 = bSuppressed.BlastField5;
                        suppressed.Text = c.Text;
                        suppressed.xPosition = c.xPosition;
                        suppressed.yPosition = c.yPosition;
                        suppressed.MAControlID = c.MAControlID;
                        suppressed.CampaignItemName = bSuppressed.CampaignItemName;
                        suppressed.EstSendTime = cibSuppressed[0].Blast.SendTime;
                        suppressed.IsConfigured = true;
                        retList.Add(suppressed);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe://this is a group trigger -- unsubscribe
                        Controls.Unsubscribe unsubscribe = new Controls.Unsubscribe();
                        ECN_Framework_Entities.Communicator.LayoutPlans groupUnSub = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(c.ECNID, user);
                        ECN_Framework_Entities.Communicator.Blast b4 = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(groupUnSub.BlastID.Value, false);
                        ECN_Framework_Entities.Communicator.CampaignItem ciUnSub = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(b4.BlastID, false);
                        //if (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(groupUnSub.BlastID.Value, groupUnSub.CustomerID.Value))
                        //    unsubscribe.editable.remove = false;
                        //else
                        //    unsubscribe.editable.remove = true;

                        unsubscribe.ControlID = c.ControlID;
                        unsubscribe.ECNID = c.ECNID;
                        unsubscribe.EmailSubject = b4.EmailSubject;
                        unsubscribe.ExtraText = "";
                        unsubscribe.FromEmail = b4.EmailFrom;
                        unsubscribe.FromName = b4.EmailFromName;
                        unsubscribe.IsDirty = false;
                        unsubscribe.MarketingAutomationID = c.MarketingAutomationID;
                        unsubscribe.MessageID = b4.LayoutID.Value;
                        unsubscribe.MessageName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(b4.LayoutID.Value, false).LayoutName;
                        unsubscribe.ReplyTo = b4.ReplyTo;
                        unsubscribe.CampaignItemTemplateID = ciUnSub.CampaignItemTemplateID.HasValue ? ciUnSub.CampaignItemTemplateID.Value : -1;
                        if (unsubscribe.CampaignItemTemplateID > 0)
                            unsubscribe.CampaignItemTemplateName = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Get_CampaignItemTemplateID(unsubscribe.CampaignItemTemplateID, user, false).TemplateName;
                        else
                            unsubscribe.CampaignItemTemplateName = string.Empty;
                        unsubscribe.BlastField1 = ciUnSub.BlastField1;
                        unsubscribe.BlastField2 = ciUnSub.BlastField2;
                        unsubscribe.BlastField3 = ciUnSub.BlastField3;
                        unsubscribe.BlastField4 = ciUnSub.BlastField4;
                        unsubscribe.BlastField5 = ciUnSub.BlastField5;
                        unsubscribe.Text = c.Text;
                        unsubscribe.xPosition = c.xPosition;
                        unsubscribe.yPosition = c.yPosition;
                        unsubscribe.IsCancelled = groupUnSub.Status.ToLower().Equals("y") ? false : groupUnSub.Status.ToLower().Equals("p") ? false : true;
                        unsubscribe.CancelDate = groupUnSub.UpdatedDate.HasValue ? groupUnSub.UpdatedDate.Value : groupUnSub.CreatedDate.Value;
                        if (((unsubscribe.IsCancelled) && (unsubscribe.CancelDate.HasValue)) || (ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastID(groupUnSub.BlastID.Value, groupUnSub.CustomerID.Value)))
                            unsubscribe.editable.remove = false;
                        else
                            unsubscribe.editable.remove = true;
                        unsubscribe.MAControlID = c.MAControlID;
                        unsubscribe.CampaignItemName = ciUnSub.CampaignItemName;
                        unsubscribe.IsConfigured = true;
                        retList.Add(unsubscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait://this is a wait -- no ECN ID
                        Controls.Wait wait = new Controls.Wait();
                        Controls.Wait origWait = new Controls.Wait();
                        try
                        {
                            origWait = (Controls.Wait)originalControls.FirstOrDefault(x => x.ControlID == c.ControlID);
                        }
                        catch (Exception ex) { }
                        wait.ControlID = c.ControlID;
                        wait.ECNID = 0;
                        wait.ExtraText = "";
                        wait.IsDirty = false;
                        wait.MarketingAutomationID = c.MarketingAutomationID;
                        wait.Text = c.Text;
                        wait.WaitTime = origWait.WaitTime;
                        wait.xPosition = c.xPosition;
                        wait.yPosition = c.yPosition;
                        wait.MAControlID = c.MAControlID;
                        wait.IsConfigured = true;
                        ECN_Framework_Entities.Communicator.MAControl child = GetChild(wait, controls, connectors);
                        switch (child.ControlType)
                        {
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed:
                                ECN_Framework_Entities.Communicator.CampaignItem ciWait = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(child.ECNID, user, false);
                                ECN_Framework_Entities.Communicator.MAControl maParent = GetParent(wait, controls, connectors);
                                ECN_Framework_Entities.Communicator.MAControl maParentCI = GetParentCI(wait, controls, connectors);
                                if (maParent != null && maParentCI != null)
                                {
                                    ECN_Framework_Entities.Communicator.CampaignItem ciParent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(maParent.ECNID, user, false);
                                    TimeSpan tsCI = ciWait.SendTime.Value - ciParent.SendTime.Value;
                                    wait.Days = tsCI.Days;
                                    wait.Hours = tsCI.Hours;
                                    wait.Minutes = tsCI.Minutes;

                                    if (wait.Days != origWait.Days || wait.Hours != origWait.Hours || wait.Minutes != origWait.Minutes)
                                    {
                                        wait.TimeChanged = true;
                                        wait.OriginalDays = origWait.Days;
                                        wait.OriginalHours = origWait.Hours;
                                        wait.OriginalMinutes = origWait.Minutes;
                                    }
                                    else
                                    {
                                        wait.TimeChanged = false;
                                    }
                                }
                                break;
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe:
                                ECN_Framework_Entities.Communicator.LayoutPlans lpWait = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(child.ECNID, user);
                                TimeSpan ts = TimeSpan.FromDays(Convert.ToDouble(lpWait.Period));
                                wait.Days = ts.Days;
                                wait.Hours = ts.Hours;
                                wait.Minutes = ts.Minutes;
                                if (wait.Days != origWait.Days || wait.Hours != origWait.Hours || wait.Minutes != origWait.Minutes)
                                {
                                    wait.TimeChanged = true;
                                    wait.OriginalDays = origWait.Days;
                                    wait.OriginalHours = origWait.Hours;
                                    wait.OriginalMinutes = origWait.Minutes;
                                }
                                else
                                {
                                    wait.TimeChanged = false;
                                }
                                break;
                            case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen:
                                ECN_Framework_Entities.Communicator.TriggerPlans tpWait = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetByTriggerPlanID(child.ECNID, user);
                                TimeSpan tsTP = TimeSpan.FromDays(Convert.ToDouble(tpWait.Period));
                                wait.Days = tsTP.Days;
                                wait.Hours = tsTP.Hours;
                                wait.Minutes = tsTP.Minutes;
                                if (wait.Days != origWait.Days || wait.Hours != origWait.Hours || wait.Minutes != origWait.Minutes)
                                {
                                    wait.TimeChanged = true;
                                    wait.OriginalDays = origWait.Days;
                                    wait.OriginalHours = origWait.Hours;
                                    wait.OriginalMinutes = origWait.Minutes;
                                }
                                else
                                {
                                    wait.TimeChanged = false;
                                }
                                break;
                        }

                        retList.Add(wait);
                        break;
                }


            }
            return retList;
        }

        private static List<PostModels.ECN_Objects.FilterSelect> GetFilters(List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibList, KMPlatform.Entity.User user, bool SelectFilters)
        {
            List<PostModels.ECN_Objects.FilterSelect> retList = new List<ECN_Objects.FilterSelect>();
            if (SelectFilters)//Only get filters for selected groups
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in cibList)
                {
                    List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> blastFilters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(cib.CampaignItemBlastID);
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in blastFilters)
                    {
                        if (cibf.FilterID.HasValue && cibf.FilterID.Value > 0)
                        {
                            PostModels.ECN_Objects.FilterSelect fs = new ECN_Objects.FilterSelect();
                            ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                            fs.CustomerID = f.CustomerID.Value;
                            fs.FilterID = cibf.FilterID.Value;
                            fs.FilterName = f.FilterName;
                            fs.GroupID = f.GroupID.Value;

                            retList.Add(fs);
                        }
                    }
                }
            }
            else// get filters for suppressed groups
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> cisList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(cibList[0].CampaignItemID.Value, true);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in cisList)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter f in cis.Filters)
                    {
                        if (f.FilterID.HasValue && f.FilterID.Value > 0)
                        {
                            PostModels.ECN_Objects.FilterSelect fs = new ECN_Objects.FilterSelect();
                            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(f.FilterID.Value);
                            fs.CustomerID = filter.CustomerID.Value;
                            fs.FilterID = f.FilterID.Value;
                            fs.FilterName = filter.FilterName;
                            fs.GroupID = filter.GroupID.Value;

                            retList.Add(fs);
                        }
                    }
                }
            }
            return retList;
        }

        private static List<PostModels.ECN_Objects.GroupSelect> GetGroups(List<ECN_Framework_Entities.Communicator.CampaignItemBlast> cibList, bool SelectGroups)
        {
            List<PostModels.ECN_Objects.GroupSelect> gsList = new List<ECN_Objects.GroupSelect>();
            if (SelectGroups)//only get selected groups
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in cibList)
                {
                    PostModels.ECN_Objects.GroupSelect gs = new ECN_Objects.GroupSelect();
                    ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(cib.GroupID.Value);
                    gs.CustomerID = g.CustomerID;

                    gs.FolderID = g.FolderID.HasValue ? g.FolderID.Value : 0;
                    gs.GroupDescription = g.GroupDescription;
                    gs.GroupID = g.GroupID;
                    gs.GroupName = g.GroupName;

                    gsList.Add(gs);
                }
            }
            else//get suppressed groups
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> listCIS = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(cibList[0].CampaignItemID.Value, false);

                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in listCIS)
                {
                    PostModels.ECN_Objects.GroupSelect gs = new ECN_Objects.GroupSelect();
                    ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(cis.GroupID.Value);
                    gs.CustomerID = g.CustomerID;
                    gs.FolderID = g.FolderID.HasValue ? g.FolderID.Value : 0;
                    gs.GroupDescription = g.GroupDescription;
                    gs.GroupID = g.GroupID;
                    gs.GroupName = g.GroupName;

                    gsList.Add(gs);
                }
            }

            return gsList;
        }

        public static List<ControlBase> Deserialize(dynamic json)
        {
            var retList = new List<ControlBase>();

            foreach (var dynamicControl in json.shapes)
            {
                var control = ControlDeserializer.Deserialize(dynamicControl);
                if (control != null)
                {
                    retList.Add(control);
                }
            }

            return retList;
        }

        public static string Serialize(List<ControlBase> controls, List<Connector> connectors, int MAID)
        {
            string fullDiagram = "{\"shapes\":";
            //Serialize Controls
            string controlsJSON = Newtonsoft.Json.JsonConvert.SerializeObject(controls);
            fullDiagram += controlsJSON + ",\"connections\":";
            //Serialize Connectors
            string connectorsJSON = Newtonsoft.Json.JsonConvert.SerializeObject(connectors);
            //Combine
            fullDiagram += connectorsJSON + "}";
            return fullDiagram;
        }
        #endregion

        public static List<ControlBase> GetModelsForCopy(List<ECN_Framework_Entities.Communicator.MAControl> controls, int MAID)
        {
            List<ControlBase> retList = new List<PostModels.ControlBase>();
            foreach (ECN_Framework_Entities.Communicator.MAControl c in controls)
            {
                switch (c.ControlType)
                {
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                        Models.PostModels.Controls.CampaignItem ci = new Controls.CampaignItem();

                        ci.ControlID = c.ControlID;
                        ci.CustomerID = -1;
                        ci.CustomerName = "";
                        ci.CampaignID = -1;
                        ci.CampaignName = "";
                        ci.CampaignItemID = -1;
                        ci.CampaignItemName = "";
                        ci.ECNID = -1;
                        ci.EmailSubject = "";
                        ci.ExtraText = "";
                        ci.FromEmail = "";
                        ci.FromName = "";
                        ci.IsDirty = false;
                        ci.MarketingAutomationID = MAID;
                        ci.MessageID = -1;
                        ci.MessageName = "";
                        ci.ReplyTo = "";
                        ci.SelectedGroupFilters = new List<ECN_Objects.FilterSelect>();
                        ci.SelectedGroups = new List<ECN_Objects.GroupSelect>();
                        ci.SendTime = DateTime.Now;
                        ci.SuppressedGroupFilters = new List<ECN_Objects.FilterSelect>();
                        ci.SuppressedGroups = new List<ECN_Objects.GroupSelect>();
                        ci.xPosition = c.xPosition;
                        ci.yPosition = c.yPosition;
                        ci.IsConfigured = false;
                        retList.Add(ci);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click://This is a blast with a smart segment based off the previous blast
                        Controls.Click click = new Controls.Click();

                        click.ControlID = c.ControlID;
                        click.ECNID = -1;
                        click.ExtraText = "";
                        click.FromEmail = "";
                        click.FromName = "";
                        click.IsDirty = false;
                        click.MarketingAutomationID = MAID;
                        click.MessageID = -1;
                        click.MessageName = "";
                        click.ReplyTo = "";
                        click.Text = "Click";
                        click.xPosition = c.xPosition;
                        click.yPosition = c.yPosition;
                        click.CampaignItemName = "";
                        click.IsConfigured = false;
                        retList.Add(click);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click://This is a click trigger
                        Controls.Direct_Click direct_click = new Controls.Direct_Click();

                        direct_click.AnyLink = true;
                        direct_click.SpecificLink = "";
                        direct_click.ControlID = c.ControlID;
                        direct_click.ECNID = -1;
                        direct_click.EmailSubject = "";
                        direct_click.ExtraText = "";
                        direct_click.FromEmail = "";
                        direct_click.FromName = "";
                        direct_click.IsDirty = false;
                        direct_click.MarketingAutomationID = MAID;
                        direct_click.MessageID = -1;
                        direct_click.MessageName = "";
                        direct_click.ReplyTo = "";
                        direct_click.Text = "Direct Email Click";
                        direct_click.xPosition = c.xPosition;
                        direct_click.yPosition = c.yPosition;
                        direct_click.IsCancelled = false;
                        direct_click.CancelDate = null;
                        direct_click.CampaignItemName = "";
                        direct_click.IsConfigured = false;
                        retList.Add(direct_click);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form://This is a click trigger
                        Controls.Form form = new Controls.Form();
                        form.AnyLink = true;
                        form.SpecificLink = "";
                        form.ControlID = c.ControlID;
                        form.ECNID = -1;
                        form.IsDirty = false;
                        form.MarketingAutomationID = MAID;
                        form.FormID = -1;
                        form.FormName = "";
                        form.Text = "Form";
                        form.xPosition = c.xPosition;
                        form.yPosition = c.yPosition;
                        form.IsConfigured = false;
                        retList.Add(form);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon://This is a click trigger
                        Controls.FormAbandon formab = new Controls.FormAbandon();
                        formab.AnyLink = false;
                        formab.SpecificLink = "";
                        formab.ControlID = c.ControlID;
                        formab.ECNID = -1;
                        formab.EmailSubject = "";
                        formab.ExtraText = "";
                        formab.FromEmail = "";
                        formab.FromName = "";
                        formab.IsDirty = false;
                        formab.MarketingAutomationID = MAID;
                        formab.MessageID = -1;
                        formab.MessageName = "";
                        formab.ReplyTo = "";
                        formab.Text = "Form Abandon";
                        formab.xPosition = c.xPosition;
                        formab.yPosition = c.yPosition;
                        formab.IsCancelled = false;
                        formab.CancelDate = null;
                        formab.CampaignItemName = "";
                        formab.IsConfigured = false;
                        retList.Add(formab);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit://This is a click trigger
                        Controls.FormSubmit formsub = new Controls.FormSubmit();
                        formsub.AnyLink = false;
                        formsub.SpecificLink = "";
                        formsub.ControlID = c.ControlID;
                        formsub.ECNID = -1;
                        formsub.EmailSubject = "";
                        formsub.ExtraText = "";
                        formsub.FromEmail = "";
                        formsub.FromName = "";
                        formsub.IsDirty = false;
                        formsub.MarketingAutomationID = MAID;
                        formsub.MessageID = -1;
                        formsub.MessageName = "";
                        formsub.ReplyTo = "";
                        formsub.Text = "Form Submit";
                        formsub.xPosition = c.xPosition;
                        formsub.yPosition = c.yPosition;
                        formsub.IsCancelled = false;
                        formsub.CancelDate = null;
                        formsub.CampaignItemName = "";
                        formsub.IsConfigured = false;
                        retList.Add(formsub);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open://this is an open trigger
                        Controls.Direct_Open direct_open = new Controls.Direct_Open();

                        direct_open.ControlID = c.ControlID;
                        direct_open.ECNID = -1;
                        direct_open.EmailSubject = "";
                        direct_open.ExtraText = "";
                        direct_open.FromEmail = "";
                        direct_open.FromName = "";
                        direct_open.IsDirty = false;
                        direct_open.MarketingAutomationID = MAID;
                        direct_open.MessageID = -1;
                        direct_open.MessageName = "";
                        direct_open.ReplyTo = "";
                        direct_open.Text = "Direct Email Open";
                        direct_open.xPosition = c.xPosition;
                        direct_open.yPosition = c.yPosition;
                        direct_open.IsCancelled = false;
                        direct_open.CancelDate = null;
                        direct_open.CampaignItemName = "";
                        direct_open.IsConfigured = false;
                        retList.Add(direct_open);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen://this is an open trigger
                        Controls.Direct_NoOpen direct_NoOpen = new Controls.Direct_NoOpen();

                        direct_NoOpen.ControlID = c.ControlID;
                        direct_NoOpen.ECNID = -1;
                        direct_NoOpen.EmailSubject = "";
                        direct_NoOpen.ExtraText = "";
                        direct_NoOpen.FromEmail = "";
                        direct_NoOpen.FromName = "";
                        direct_NoOpen.IsDirty = false;
                        direct_NoOpen.MarketingAutomationID = MAID;
                        direct_NoOpen.MessageID = -1;
                        direct_NoOpen.MessageName = "";
                        direct_NoOpen.ReplyTo = "";
                        direct_NoOpen.Text = "Direct Email No Open";
                        direct_NoOpen.xPosition = c.xPosition;
                        direct_NoOpen.yPosition = c.yPosition;
                        direct_NoOpen.IsCancelled = false;
                        direct_NoOpen.CancelDate = null;
                        direct_NoOpen.CampaignItemName = "";
                        direct_NoOpen.IsConfigured = false;
                        retList.Add(direct_NoOpen);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End://this means the end of an automation-- no ECN ID
                        Controls.End end = new Controls.End();
                        end.ControlID = c.ControlID;
                        end.ECNID = 0;
                        end.ExtraText = "";
                        end.MarketingAutomationID = MAID;
                        end.Text = "";
                        end.xPosition = c.xPosition;
                        end.yPosition = c.yPosition;
                        end.IsConfigured = true;
                        retList.Add(end);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group://This is a group placeholder
                        Controls.Group group = new Controls.Group();
                        group.ControlID = c.ControlID;
                        group.CustomerID = -1;
                        group.CustomerName = "";
                        group.ECNID = -1;
                        group.ExtraText = "";
                        group.GroupID = -1;
                        group.GroupName = "";
                        group.IsDirty = false;
                        group.MarketingAutomationID = MAID;
                        group.Text = "Group";
                        group.xPosition = c.xPosition;
                        group.yPosition = c.yPosition;
                        group.IsConfigured = false;
                        retList.Add(group);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick://This is a blast with a no click smart segment
                        Controls.NoClick noclick = new Controls.NoClick();
                        noclick.ControlID = c.ControlID;
                        noclick.ECNID = -1;
                        noclick.ExtraText = "";
                        noclick.FromEmail = "";
                        noclick.FromName = "";
                        noclick.IsDirty = false;
                        noclick.MarketingAutomationID = MAID;
                        noclick.MessageID = -1;
                        noclick.MessageName = "";
                        noclick.ReplyTo = "";
                        noclick.Text = "Click";
                        noclick.xPosition = c.xPosition;
                        noclick.yPosition = c.yPosition;
                        noclick.CampaignItemName = "";
                        noclick.IsConfigured = false;
                        retList.Add(noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen://This is a blast with a no open smart segment
                        Controls.NoOpen noopen = new Controls.NoOpen();
                        noopen.ControlID = c.ControlID;
                        noopen.ECNID = -1;
                        noopen.ExtraText = "";
                        noopen.FromEmail = "";
                        noopen.FromName = "";
                        noopen.IsDirty = false;
                        noopen.MarketingAutomationID = MAID;
                        noopen.MessageID = -1;
                        noopen.MessageName = "";
                        noopen.ReplyTo = "";
                        noopen.Text = "No Open";
                        noopen.xPosition = c.xPosition;
                        noopen.yPosition = c.yPosition;
                        noopen.CampaignItemName = "";
                        noopen.IsConfigured = false;
                        retList.Add(noopen);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent://This is a blast with a not sent smart segment
                        Controls.NotSent notsent = new Controls.NotSent();
                        notsent.ControlID = c.ControlID;
                        notsent.ECNID = -1;
                        notsent.ExtraText = "";
                        notsent.FromEmail = "";
                        notsent.FromName = "";
                        notsent.IsDirty = false;
                        notsent.MarketingAutomationID = MAID;
                        notsent.MessageID = -1;
                        notsent.MessageName = "";
                        notsent.ReplyTo = "";
                        notsent.Text = "Not Sent";
                        notsent.xPosition = c.xPosition;
                        notsent.yPosition = c.yPosition;
                        notsent.CampaignItemName = "";
                        notsent.IsConfigured = false;
                        retList.Add(notsent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open://This is a blast with an open smart segment
                        Controls.Open open = new Controls.Open();
                        open.ControlID = c.ControlID;
                        open.ECNID = -1;
                        open.ExtraText = "";
                        open.FromEmail = "";
                        open.FromName = "";
                        open.IsDirty = false;
                        open.MarketingAutomationID = MAID;
                        open.MessageID = -1;
                        open.MessageName = "";
                        open.ReplyTo = "";
                        open.Text = "Open";
                        open.xPosition = c.xPosition;
                        open.yPosition = c.yPosition;
                        open.CampaignItemName = "";
                        open.IsConfigured = false;
                        retList.Add(open);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick://This is a blast with an open/no click smart segment
                        Controls.Open_NoClick open_noclick = new Controls.Open_NoClick();
                        open_noclick.ControlID = c.ControlID;
                        open_noclick.ECNID = -1;
                        open_noclick.ExtraText = "";
                        open_noclick.FromEmail = "";
                        open_noclick.FromName = "";
                        open_noclick.IsDirty = false;
                        open_noclick.MarketingAutomationID = MAID;
                        open_noclick.MessageID = -1;
                        open_noclick.MessageName = "";
                        open_noclick.ReplyTo = "";
                        open_noclick.Text = "Open - No Click";
                        open_noclick.xPosition = c.xPosition;
                        open_noclick.yPosition = c.yPosition;
                        open_noclick.CampaignItemName = "";
                        open_noclick.IsConfigured = false;
                        retList.Add(open_noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent://This is a blast with a sent smart segment
                        Controls.Sent sent = new Controls.Sent();
                        sent.ControlID = c.ControlID;
                        sent.ECNID = -1;
                        sent.ExtraText = "";
                        sent.FromEmail = "";
                        sent.FromName = "";
                        sent.IsDirty = false;
                        sent.MarketingAutomationID = MAID;
                        sent.MessageID = -1;
                        sent.MessageName = "";
                        sent.ReplyTo = "";
                        sent.Text = "Sent";
                        sent.xPosition = c.xPosition;
                        sent.yPosition = c.yPosition;
                        sent.CampaignItemName = "";
                        sent.IsConfigured = false;
                        retList.Add(sent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start://This is the start of an automation--no ECN ID
                        Controls.Start start = new Controls.Start();
                        start.ControlID = c.ControlID;
                        start.ECNID = 0;
                        start.ExtraText = "";
                        start.IsDirty = false;
                        start.MarketingAutomationID = MAID;
                        start.Text = "";
                        start.xPosition = c.xPosition;
                        start.yPosition = c.yPosition;
                        start.IsConfigured = true;
                        retList.Add(start);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:// this is a group trigger--subscribe
                        Controls.Subscribe subscribe = new Controls.Subscribe();

                        subscribe.ControlID = c.ControlID;
                        subscribe.ECNID = -1;
                        subscribe.EmailSubject = "";
                        subscribe.ExtraText = "";
                        subscribe.FromEmail = "";
                        subscribe.FromName = "";
                        subscribe.IsDirty = false;
                        subscribe.MarketingAutomationID = MAID;
                        subscribe.MessageID = -1;
                        subscribe.MessageName = "";
                        subscribe.ReplyTo = "";
                        subscribe.Text = "Subscribe";
                        subscribe.xPosition = c.xPosition;
                        subscribe.yPosition = c.yPosition;
                        subscribe.IsCancelled = false;
                        subscribe.CancelDate = null;
                        subscribe.CampaignItemName = "";
                        subscribe.IsConfigured = false;
                        retList.Add(subscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed://this is a blast with a suppressed smart segment
                        Controls.Suppressed suppressed = new Controls.Suppressed();
                        suppressed.ControlID = c.ControlID;
                        suppressed.ECNID = -1;
                        suppressed.ExtraText = "";
                        suppressed.FromEmail = "";
                        suppressed.FromName = "";
                        suppressed.IsDirty = false;
                        suppressed.MarketingAutomationID = MAID;
                        suppressed.MessageID = -1;
                        suppressed.MessageName = "";
                        suppressed.ReplyTo = "";
                        suppressed.Text = "Suppressed";
                        suppressed.xPosition = c.xPosition;
                        suppressed.yPosition = c.yPosition;
                        suppressed.CampaignItemName = "";
                        suppressed.IsConfigured = false;
                        retList.Add(suppressed);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe://this is a group trigger -- unsubscribe
                        Controls.Unsubscribe unsubscribe = new Controls.Unsubscribe();

                        unsubscribe.ControlID = c.ControlID;
                        unsubscribe.ECNID = -1;
                        unsubscribe.EmailSubject = "";
                        unsubscribe.ExtraText = "";
                        unsubscribe.FromEmail = "";
                        unsubscribe.FromName = "";
                        unsubscribe.IsDirty = false;
                        unsubscribe.MarketingAutomationID = MAID;
                        unsubscribe.MessageID = -1;
                        unsubscribe.MessageName = "";
                        unsubscribe.ReplyTo = "";
                        unsubscribe.Text = "Unsubscribe";
                        unsubscribe.xPosition = c.xPosition;
                        unsubscribe.yPosition = c.yPosition;
                        unsubscribe.IsCancelled = false;
                        unsubscribe.CancelDate = null;
                        unsubscribe.CampaignItemName = "";
                        unsubscribe.IsConfigured = false;
                        retList.Add(unsubscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait://this is a wait -- no ECN ID
                        Controls.Wait wait = new Controls.Wait();
                        wait.ControlID = c.ControlID;
                        wait.ECNID = 0;
                        wait.ExtraText = "";
                        wait.IsDirty = false;
                        wait.MarketingAutomationID = MAID;
                        wait.Text = "Wait";
                        wait.WaitTime = 0.0M;
                        wait.xPosition = c.xPosition;
                        wait.yPosition = c.yPosition;
                        wait.IsConfigured = false;
                        retList.Add(wait);
                        break;
                }


            }
            return retList;
        }

        public static List<ControlBase> GetModelsForCopyFromControlBase(
            List<ControlBase> controls,
            List<Connector> origConnectors,
            ref List<Connector> connectors,
            int marketingAutomationId)
        {
            var resultList = new List<ControlBase>();
            var connectorsToRemove = new List<Connector>();
            var controlsToRemove = new List<ControlBase>();
            var initialCtrlCount = controls.Count;
            var controlFactory = new ControlFactory();

            foreach (var control in controls)
            {
                var preparedControl = controlFactory.PrepareForCopy(control, marketingAutomationId);
                if (preparedControl != null)
                {
                    resultList.Add(preparedControl);
                }

                UpdateControlsToRemove(
                    controls,
                    origConnectors,
                    preparedControl,
                    ref controlsToRemove,
                    ref connectorsToRemove);
            }

            if (initialCtrlCount != controlsToRemove.Count)
            {
                foreach (var controlToRemove in controlsToRemove)
                {
                    resultList.Remove(controlToRemove);
                }

                connectors = origConnectors;
                foreach (var connectorToRemove in connectorsToRemove)
                {
                    connectors.Remove(connectorToRemove);
                }
            }

            return resultList;
        }

        private static void UpdateControlsToRemove(
            List<ControlBase> controls,
            List<Connector> origConnectors,
            ControlBase control,
            ref List<ControlBase> controlsToRemove,
            ref List<Connector> connectorsToRemove)
        {
            switch (control.ControlType)
            {
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click:
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open:
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen:
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:
                    var cancellable = (ICancellable) control;

                    if (cancellable.IsCancelled)
                    {
                        Connector.GetControlsToRemove(
                            controls,
                            origConnectors,
                            ref controlsToRemove,
                            ref connectorsToRemove,
                            control.ControlID);
                    }
                    break;
            }
        }

        public static string GetJSONDiagramFromControls(List<ControlBase> controls, List<Connector> connectors, int MAID)
        {
            List<ControlBase> retList = new List<ControlBase>();
            string jsonToReturn = "";
            foreach (ControlBase c in controls)
            {
                //37170 copying configured properties
                switch (c.ControlType)
                {
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                        Models.PostModels.Controls.CampaignItem ci = (Controls.CampaignItem)c;
                        bool doesntExist = false;
                        if (!ci.CreateCampaignItem && ci.ECNID > 0)
                        {
                            ECN_Framework_Entities.Communicator.CampaignItem ciECN = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(ci.ECNID, false);
                            if (ciECN != null)
                            {
                                //ci.SendTime = ciECN.SendTime.Value;
                            }
                            else
                            {
                                doesntExist = true;
                            }
                        }
                        

                        if (ci.ECNID > 0 && !doesntExist)
                            ci.IsConfigured = true;
                        else if(!doesntExist && ci.ECNID <= 0)
                        {
                            //dont do anything
                        }
                        else
                        {

                            ci.CampaignID = -1;
                            ci.CampaignName = "";
                            ci.CreateCampaign = false;


                            ci.CampaignItemID = -1;
                            ci.ECNID = -1;
                            ci.IsConfigured = false;
                            ci.CreateCampaignItem = true;
                            if(ci.CampaignItemTemplateID >0)
                                ci.UseCampaignItemTemplate = true;
                            else
                                ci.UseCampaignItemTemplate = false;
                        }

                        retList.Add(ci);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click://This is a blast with a smart segment based off the previous blast
                        Controls.Click click = (Controls.Click)c;
                        if (click.MessageID > 0)
                            click.IsConfigured = true;

                        retList.Add(click);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click://This is a click trigger
                        Controls.Direct_Click direct_click = (Controls.Direct_Click)c;
                        if (direct_click.MessageID > 0)
                            direct_click.IsConfigured = true;

                        retList.Add(direct_click);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form://This is a click trigger
                        Controls.Form form = (Controls.Form)c;
                        if (form.FormID > 0)
                            form.IsConfigured = true;
                        retList.Add(form);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon://This is a click trigger
                        Controls.FormAbandon formab = (Controls.FormAbandon)c;
                        if (formab.MessageID > 0)
                            formab.IsConfigured = true;
                        retList.Add(formab);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit://This is a click trigger
                        Controls.FormSubmit formsub = (Controls.FormSubmit)c;
                        if (formsub.MessageID > 0)
                            formsub.IsConfigured = true;
                        retList.Add(formsub);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open://this is an open trigger
                        Controls.Direct_Open direct_open = (Controls.Direct_Open)c;
                        if (direct_open.MessageID > 0)
                            direct_open.IsConfigured = true;
                        retList.Add(direct_open);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen://this is an open trigger
                        Controls.Direct_NoOpen direct_NoOpen = (Controls.Direct_NoOpen)c;
                        if (direct_NoOpen.MessageID > 0)
                            direct_NoOpen.IsConfigured = true;
                        retList.Add(direct_NoOpen);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End://this means the end of an automation-- no ECN ID
                        Controls.End end = (Controls.End)c;
                        end.IsConfigured = true;
                        retList.Add(end);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group://This is a group placeholder
                        Controls.Group group = (Controls.Group)c;
                        if (group.GroupID > 0)
                            group.IsConfigured = true;
                        retList.Add(group);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick://This is a blast with a no click smart segment
                        Controls.NoClick noclick = (Controls.NoClick)c;
                        if (noclick.MessageID > 0)
                            noclick.IsConfigured = true;
                        retList.Add(noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen://This is a blast with a no open smart segment
                        Controls.NoOpen noopen = (Controls.NoOpen)c;
                        if (noopen.MessageID > 0)
                            noopen.IsConfigured = true;

                        retList.Add(noopen);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent://This is a blast with a not sent smart segment
                        Controls.NotSent notsent = (Controls.NotSent)c;
                        if (notsent.MessageID > 0)
                            notsent.IsConfigured = true;
                        retList.Add(notsent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open://This is a blast with an open smart segment
                        Controls.Open open = (Controls.Open)c;
                        if (open.MessageID > 0)
                            open.IsConfigured = true;
                        retList.Add(open);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick://This is a blast with an open/no click smart segment
                        Controls.Open_NoClick open_noclick = (Controls.Open_NoClick)c;
                        if (open_noclick.MessageID > 0)
                            open_noclick.IsConfigured = true;
                        retList.Add(open_noclick);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent://This is a blast with a sent smart segment
                        Controls.Sent sent = (Controls.Sent)c;
                        if (sent.MessageID > 0)
                            sent.IsConfigured = true;
                        retList.Add(sent);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start://This is the start of an automation--no ECN ID
                        Controls.Start start = (Controls.Start)c;
                        start.IsConfigured = true;
                        retList.Add(start);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:// this is a group trigger--subscribe
                        Controls.Subscribe subscribe = (Controls.Subscribe)c;
                        if (subscribe.MessageID > 0)
                            subscribe.IsConfigured = true;
                        retList.Add(subscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed://this is a blast with a suppressed smart segment
                        Controls.Suppressed suppressed = (Controls.Suppressed)c;
                        if (suppressed.MessageID > 0)
                            suppressed.IsConfigured = true;
                        retList.Add(suppressed);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe://this is a group trigger -- unsubscribe
                        Controls.Unsubscribe unsubscribe = (Controls.Unsubscribe)c;
                        if (unsubscribe.MessageID > 0)
                            unsubscribe.IsConfigured = true;
                        retList.Add(unsubscribe);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait://this is a wait -- no ECN ID
                        Controls.Wait wait = (Controls.Wait)c;
                        if (wait.Days.HasValue || wait.Hours.HasValue || wait.Minutes.HasValue)
                            wait.IsConfigured = true;
                        retList.Add(wait);
                        break;
                }


            }

            jsonToReturn = Serialize(retList, connectors, MAID);
            return jsonToReturn;
        }
        public static bool IsActiveOrSent(int blastID, int customerID)
        {
            return ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(blastID, customerID);
        }

        private static ECN_Framework_Entities.Communicator.MAControl GetChild(ControlBase control, List<ECN_Framework_Entities.Communicator.MAControl> controls, List<ECN_Framework_Entities.Communicator.MAConnector> connectors)
        {
            ECN_Framework_Entities.Communicator.MAConnector startConn = connectors.First(x => x.From == control.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return controls.First(x => startConn.To == x.ControlID);


            }
            else
            {
                return null;
            }
        }
        private static ECN_Framework_Entities.Communicator.MAControl FindParentCI(ControlBase control, List<ECN_Framework_Entities.Communicator.MAControl> controls, List<ECN_Framework_Entities.Communicator.MAConnector> connectors)
        {

            ECN_Framework_Entities.Communicator.MAConnector startConn = connectors.First(x => x.To == control.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return controls.First(x => startConn.From == x.ControlID);
            }
            else
            {
                return null;
            }
        }
        private static ECN_Framework_Entities.Communicator.MAControl GetParentCI(ControlBase control, List<ECN_Framework_Entities.Communicator.MAControl> controls, List<ECN_Framework_Entities.Communicator.MAConnector> connectors)
        {
            ECN_Framework_Entities.Communicator.MAConnector startConn = connectors.First(x => x.To == control.ControlID);
            ECN_Framework_Entities.Communicator.MAControl maParent = null;
            bool cont = true;
            int index = 0;
            while (cont)
            {
                if (startConn != null)
                {
                    if (controls.First(x => startConn.From == x.ControlID).ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem)
                    {
                        cont = false;
                        maParent = controls.First(x => startConn.From == x.ControlID);

                    }
                    else
                    {
                        startConn = connectors.First(x => x.To == controls.First(y => startConn.From == y.ControlID).ControlID);
                    }
                    //start looping through it's children



                }
                else
                {
                    cont = false;

                }
                index++;
                if (index > controls.Count)
                    cont = false;
            }
            return maParent;
        }

        private static ECN_Framework_Entities.Communicator.MAControl GetParent(ControlBase control, List<ECN_Framework_Entities.Communicator.MAControl> controls, List<ECN_Framework_Entities.Communicator.MAConnector> connectors)
        {
            ECN_Framework_Entities.Communicator.MAConnector startConn = connectors.First(x => x.To == control.ControlID);
            ECN_Framework_Entities.Communicator.MAControl maParent = null;

            maParent = controls.First(x => startConn.From == x.ControlID);

            return maParent;
        }
        public static ControlBase GetParent(ControlBase control, List<Models.PostModels.ControlBase> controls, List<Models.PostModels.Connector> connectors)
        {
            Models.PostModels.Connector startConn = connectors.First(x => x.to.shapeId == control.ControlID);
            ControlBase maParent = null;

            maParent = controls.First(x => startConn.from.shapeId == x.ControlID);

            return maParent;
        }

              
        public static ControlBase GetChildconn(Models.PostModels.Connector conn, List<Models.PostModels.ControlBase> controls)
        {
            Models.PostModels.ControlBase startCon = controls.First(x => x.ControlID == conn.to.shapeId);
            if (startCon != null)
            {
                //start looping through it's children
                return controls.First(x => startCon.ControlID== x.ControlID);
            }
            else
            {
                return null;
            }
        }
        public static ControlBase GetChild(ControlBase control, List<Models.PostModels.ControlBase> controls, List<Models.PostModels.Connector> connectors)
        {
            Models.PostModels.Connector startConn = connectors.First(x => x.from.shapeId == control.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return controls.First(x => startConn.to.shapeId == x.ControlID);


            }
            else
            {
                return null;
            }
        }
        public static ControlBase GetParentCI(ControlBase control, List<Models.PostModels.ControlBase> controls, List<Models.PostModels.Connector> connectors)
        {
            Models.PostModels.Connector startConn = connectors.First(x => x.to.shapeId == control.ControlID);
            ControlBase maParent = null;
            bool cont = true;
            int index = 0;
            while (cont)
            {
                if (startConn != null)
                {
                    if (controls.First(x => startConn.from.shapeId == x.ControlID).ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem)
                    {
                        cont = false;
                        maParent = controls.First(x => startConn.from.shapeId == x.ControlID);

                    }
                    else
                    {
                        startConn = connectors.FirstOrDefault(x => x.to.shapeId == controls.First(y => startConn.from.shapeId == y.ControlID).ControlID);
                    }
                    //start looping through it's children



                }
                else
                {
                    cont = false;

                }
                index++;
                if (index > controls.Count)
                    cont = false;
            }
            return maParent;
        }
        public static int blastLicenseCount_Update(int CampaignItemID)
        {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                if (ci.BlastList != null)
                {
                    System.Text.StringBuilder xmlGroups = new System.Text.StringBuilder();
                    xmlGroups.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlGroups.Append("<NoBlast>");
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cibl in ci.BlastList)
                    {
                        xmlGroups.Append("<Group id=\"" + cibl.GroupID.ToString() + "\">");
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cibl.Filters.Where(x => x.SmartSegmentID != null).ToList())
                        {

                            xmlGroups.Append("<SmartSegmentID id=\"" + cibf.SmartSegmentID + "\">");
                            xmlGroups.Append("<RefBlastIDs>" + cibf.RefBlastIDs + "</RefBlastIDs></SmartSegmentID>");

                        }

                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cibl.Filters.Where(x => x.FilterID != null).ToList())
                        {
                            xmlGroups.Append("<FilterID id=\"" + cibf.FilterID.ToString() + "\" />");
                        }

                        xmlGroups.Append("</Group>");
                    }
                 
                    if (ci.SuppressionList != null)
                    {
                        xmlGroups.Append("<SuppressionGroup>");
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cisl in ci.SuppressionList)
                        {
                            xmlGroups.Append("<Group id=\"" + cisl.GroupID + "\">");
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cisl.Filters.Where(x => x.SmartSegmentID != null))
                            {

                                xmlGroups.Append("<SmartSegmentID id=\"" + cibf.SmartSegmentID + "\">");
                                xmlGroups.Append("<RefBlastIDs>" + cibf.RefBlastIDs + "</RefBlastIDs></SmartSegmentID>");

                            }

                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cisl.Filters.Where(x => x.FilterID != null))
                            {
                                xmlGroups.Append("<FilterID id=\"" + cibf.FilterID + "\" />");
                            }
                            xmlGroups.Append("</Group>");
                        }
                        xmlGroups.Append("</SuppressionGroup>");
                    }

                    xmlGroups.Append("</NoBlast>");

                    if (ci.BlastList.Count > 0)
                    {
                        DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetEstimatedSendsCount(xmlGroups.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ci.IgnoreSuppression.HasValue ? ci.IgnoreSuppression.Value : false);
                        return Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                return 0;
            }
                
        }

    }
    public class shapecontent : content
    {
        public shapecontent() { }

        public shapecontent(string text, string color)
        {
            this.text = text;
            this.color = color;
        }
        public shapecontent(string text, string color, string align)
        {
            this.text = text;
            this.color = color;
            this.align = align;
        }
        public int fontSize { get { return 16; } }
        public string text { get; set; }
    }

    public class connectors
    {
        public connectors() { }

        public string name { get; set; }
    }

    public class rotation
    {
        public rotation() { }

        public int angle { get { return 0; } }
    }


    public class shapeEditable
    {
        public shapeEditable() { remove = true; snap = false; }

        public bool remove { get; set; }

        public bool snap { get; set; }
    }

    public class BaseSpecifiedConcreteClassConverter : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {

            if (typeof(ControlBase).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }


    }

    public class BaseConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {

            return (objectType == typeof(ControlBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Load(reader);
            switch ((ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), jo.GetValue("category").ToString()))
            {
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                    return JsonConvert.DeserializeObject<Controls.CampaignItem>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click:
                    return JsonConvert.DeserializeObject<Controls.Click>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click:
                    return JsonConvert.DeserializeObject<Controls.Direct_Click>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open:
                    return JsonConvert.DeserializeObject<Controls.Direct_Open>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen:
                    return JsonConvert.DeserializeObject<Controls.Direct_NoOpen>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End:
                    return JsonConvert.DeserializeObject<Controls.End>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group:
                    return JsonConvert.DeserializeObject<Controls.Group>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick:
                    return JsonConvert.DeserializeObject<Controls.NoClick>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen:
                    return JsonConvert.DeserializeObject<Controls.NoOpen>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent:
                    return JsonConvert.DeserializeObject<Controls.NotSent>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open:
                    return JsonConvert.DeserializeObject<Controls.Open>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick:
                    return JsonConvert.DeserializeObject<Controls.Open_NoClick>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent:
                    return JsonConvert.DeserializeObject<Controls.Sent>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start:
                    return JsonConvert.DeserializeObject<Controls.Start>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:
                    return JsonConvert.DeserializeObject<Controls.Subscribe>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed:
                    return JsonConvert.DeserializeObject<Controls.Suppressed>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe:
                    return JsonConvert.DeserializeObject<Controls.Unsubscribe>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait:
                    return JsonConvert.DeserializeObject<Controls.Wait>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form:
                    return JsonConvert.DeserializeObject<Controls.Form>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon:
                    return JsonConvert.DeserializeObject<Controls.FormAbandon>(jo.ToString(), SpecifiedSubclassConversion);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit:
                    return JsonConvert.DeserializeObject<Controls.FormSubmit>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}