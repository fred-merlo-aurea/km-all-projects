using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace CanonESubscriptionForm.forms
{
    public partial class CSC_RFQ : System.Web.UI.Page
    {
        private ArrayList aCategories = null;

        private ArrayList RFQSession
        {
            get
            {
                if (ViewState["RFQSession"] == null)
                {
                    aCategories = new ArrayList();
                    ViewState["RFQSession"] = aCategories;
                }
                return (ArrayList)ViewState["RFQSession"];
            }
            set
            {
                ViewState["RFQSession"] = value;
            }
        }

        private int PageIndex
        {
            get
            {
                if (ViewState["PageIndex"] == null)
                    return 0;
                else
                    return (int)ViewState["PageIndex"];
            }

            set { ViewState["PageIndex"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;

            aCategories = RFQSession;
            
            if (!IsPostBack)
            {
                
                chkCategories.DataSource = GetCategories();
                chkCategories.DataValueField = "value";
                chkCategories.DataTextField = "text";
                chkCategories.DataBind();

                aCategories.Add("Categories");
                ResetPageControls();
            }
            else
            {
                if (aCategories.Count == 0)
                    Response.Redirect("CSC_RFQ.aspx");
            }
        }

        private void ResetPageControls()
        {
            if (PageIndex == 0)
            {
                btnback.Visible = false;
                btnFinish.Visible = false;
                btnNext.Visible = true;
                btnNext.Text = "Next";
                pnlCategories.Visible = true;
                pnlAdvertisers.Visible = false;
                pnlSubscriptionForm.Visible = false;
                pnlThankYou.Visible = false;
                pnlSubscriptionForm2.Visible = false;
                btnNext.Attributes.Add("OnClick", "");
            }
            else if (PageIndex == aCategories.Count - 2)
            {
                loadstate();
                btnback.Visible = true;
                btnNext.Text = "Next";
                btnFinish.Visible = false;
                btnNext.Visible = true;
                pnlCategories.Visible = false;
                pnlAdvertisers.Visible = false;
                pnlSubscriptionForm.Visible = true;
                pnlSubscriptionForm2.Visible = false;
                pnlThankYou.Visible = false;
                btnNext.Attributes.Add("OnClick", "javascript:return validateForm();");
            }
            else if (PageIndex == aCategories.Count - 1)
            {
                btnback.Visible = true;
                btnFinish.Visible = true;
                btnNext.Visible = false;
                pnlCategories.Visible = false;
                pnlAdvertisers.Visible = false;
                pnlSubscriptionForm.Visible = false;
                pnlSubscriptionForm2.Visible = true;
                pnlThankYou.Visible = false;
                btnNext.Attributes.Add("OnClick", "");
            }
            else
            {
                btnback.Visible = true;
                btnFinish.Visible = false;
                btnNext.Visible = true;
                btnNext.Text = "Next";
                pnlCategories.Visible = false;
                pnlAdvertisers.Visible = true;
                pnlSubscriptionForm.Visible = false;
                pnlSubscriptionForm2.Visible = false;
                pnlThankYou.Visible = false;
                btnNext.Attributes.Add("OnClick", "");
                RenderPage();
            }

        }

        private void RenderPage()
        {
            Categories c = (Categories)aCategories[PageIndex];

            DataView dv = GetAdvertisers(c.Value);

            lblCategory.Text = c.Text;

            chkAdvertisers.DataSource = dv;
            chkAdvertisers.DataValueField = "ID";
            chkAdvertisers.DataTextField = "Name";
            chkAdvertisers.DataBind();
            chkAdvertisers.ClearSelection();

            for (int x = 0; x <= c.SelectedAdvertisers.Count - 1; x++)
            {
                chkAdvertisers.Items.FindByValue(c.SelectedAdvertisers[x].ToString()).Selected = true;
            }
        }

        private Categories GetCategories(string Value, string Text)
        {
            try
            {
                for (int i = 1; i <= aCategories.Count - 3; i++)
                {
                    Categories ExistingCategory = (Categories)aCategories[i];

                    if (ExistingCategory.Value == Value)
                    {
                        return ExistingCategory;
                    }
                }
            }
            catch { }

            return new Categories(Value, Text);
        }

        private DataView GetAdvertisers(string Category)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("Advertiser.xml"));

            XmlNodeList lst = doc.SelectNodes("//Category[@Value='" + Category + "']/..");
            string xml = "";

            foreach (XmlNode node in lst)
            {
                xml += node.OuterXml;
            }
            xml += "";

            if (xml == "")
            {
                return null;
            }
            else
            {
                xml = "<xml>" + xml + "</xml>";

                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Server.MapPath("Advertiser.xsd"));
                ds.ReadXml(new StringReader(xml));

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "Rank";

                return dv;
            }

        }

        private DataView GetCategories()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("Categories.xml"));

            XmlNodeList lst = doc.SelectNodes("//category[@Active='1']");
            string xml = "";

            foreach (XmlNode node in lst)
            {
                xml += node.OuterXml;

            }
            xml += "";

            if (xml == "")
            {
                return null;
            }
            else
            {
                xml = "<xml>" + xml + "</xml>";

                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xml));

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "text";

                return dv;
            }

        }

        private string GetAdvertiserName(string Value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("Advertiser.xml"));
                return doc.SelectSingleNode("//Advertiser[@ID='" + Value + "']").Attributes["Name"].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetAdvertiserEmail(string Value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("Advertiser.xml"));
                return doc.SelectSingleNode("//Advertiser[@ID='" + Value + "']").Attributes["Email"].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        #region Button Events
        protected void btnback_Click(object sender, EventArgs e)
        {
            //if (PageIndex != aCategories.Count - 1)
            //    SaveAdvertisers();

            PageIndex--;
            ResetPageControls();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (PageIndex == 0)
            {
                ArrayList newCategoryList = new ArrayList();
                newCategoryList.Add("Categories");

                for (int i = 0; i <= chkCategories.Items.Count - 1; i++)
                {
                    if (chkCategories.Items[i].Selected)
                    {
                        DataView dv = GetAdvertisers(chkCategories.Items[i].Value);

                        if (dv != null)
                            newCategoryList.Add(GetCategories(chkCategories.Items[i].Value, chkCategories.Items[i].Text));
                        else
                            chkCategories.Items[i].Selected = false;
                    }
                }
                newCategoryList.Add("Form");
                newCategoryList.Add("Form2");

                aCategories.Clear();

                aCategories = newCategoryList;
                RFQSession = aCategories;
                PageIndex = 0;

                if (aCategories.Count == 3)
                {
                    pnlError.Visible = true;
                    lblErrMessage.Text = "Please select one or mulitple equipment catagories from the list below.";
                    return;
                }
            }
            else
            {
                if (PageIndex < aCategories.Count - 2)
                {
                    Categories c = (Categories)aCategories[PageIndex];

                    c.SelectedAdvertisers.Clear();

                    for (int i = 0; i <= chkAdvertisers.Items.Count - 1; i++)
                    {
                        if (chkAdvertisers.Items[i].Selected)
                        {
                            c.SelectedAdvertisers.Add(chkAdvertisers.Items[i].Value);
                        }
                    }

                    if (c.SelectedAdvertisers.Count == 0)
                    {
                        pnlError.Visible = true;
                        lblErrMessage.Text = "Please select one or mulitple equipment supplier from the list below.";

                        return;
                    }
                }
            }
            PageIndex++;
            ResetPageControls();
        }

        #endregion


        private void HttpPost(string postparams)
        {
            // parameters: name1=value1&name2=value2	

            WebRequest webRequest = WebRequest.Create(String.Format(ConfigurationManager.AppSettings["ECN_ActivityEngine_Path"].ToString(), postparams));
            webRequest.Method = "GET";
            WebResponse WebResp = webRequest.GetResponse();
        } 

        private string ReplaceCodeSnippets(string sCategories)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ConfigurationManager.AppSettings["CSC_Advertiser_Email"].ToString());

            sb = sb.Replace("%%firstname%%", txtFirstName.Text);
            sb = sb.Replace("%%lastname%%", txtLastName.Text);
            sb = sb.Replace("%%jobtitle%%", txtJobTitle.Text);
            sb = sb.Replace("%%company%%", txtCompany.Text);
            sb = sb.Replace("%%address%%", txtAddress.Text);
            sb = sb.Replace("%%city%%", txtCity.Text);
            sb = sb.Replace("%%state%%", drpState.SelectedItem.Value);
            sb = sb.Replace("%%zip%%", txtZip.Text);
            sb = sb.Replace("%%country%%", txtCountry.Text);
            sb = sb.Replace("%%phone%%", txtPhone.Text);
            sb = sb.Replace("%%fax%%", txtFax.Text);
            sb = sb.Replace("%%cellphone%%", txtCellPhone.Text);
            sb = sb.Replace("%%email%%", txtEmail.Text);
            sb = sb.Replace("%%materialproduced%%", txtMaterialProduced.Text);
            sb = sb.Replace("%%materialweightcf%%", txtMaterialWeightCF.Text == string.Empty ? "" : txtMaterialWeightCF.Text + " per cubic foot ");
            sb = sb.Replace("%%materialweightcm%%", txtMaterialWeightCM.Text == string.Empty ? "" : txtMaterialWeightCM.Text + " per cubic meter");
            sb = sb.Replace("%%totalweight%%", txtTotalWeight.Text);
            sb = sb.Replace("%%materialtemp%%", txtMaterialTemp.Text);
            sb = sb.Replace("%%matericalchar%%", getCheckboxValues(chkMatericalChar));
            sb = sb.Replace("%%other%%", txtOther.Text);
            sb = sb.Replace("%%bulkdensitylbs%%", txtBulkDensitylbs.Text == string.Empty ? "" : txtBulkDensitylbs.Text + " lbs ");
            sb = sb.Replace("%%bulkdensitykgs%%", txtBulkDensitykgs.Text == string.Empty ? "" : txtBulkDensitykgs.Text + " kgs");
            sb = sb.Replace("%%particlesizem%%", txtParticleSizeM.Text == string.Empty ? "" : txtParticleSizeM.Text + " microns ");
            sb = sb.Replace("%%particulesize%%", txtParticuleSizeI.Text == string.Empty ? "" : txtParticuleSizeI.Text + " inches ");
            sb = sb.Replace("%%particulesizems%%", txtParticuleSizeMS.Text == string.Empty ? "" : txtParticuleSizeMS.Text + " mesh size  ");
            sb = sb.Replace("%%moisturecontent%%", txtMoistureContent.Text);
            sb = sb.Replace("%%equimentcomments%%", txtEquimentcomments.Text);
            sb = sb.Replace("%%categories%%", sCategories);

            return sb.ToString();
        }

        private string getCheckboxValues(CheckBoxList lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }
            return selectedvalues;
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {

                string PostParams = string.Empty;
                string AdvParams = string.Empty;

                PostParams = "?c=" + System.Configuration.ConfigurationManager.AppSettings["CSC_RFQ_CustomerID"].ToString();
                PostParams += "&f=html&s=S";
                PostParams += "&g=" + System.Configuration.ConfigurationManager.AppSettings["CSC_RFQ_GroupID"].ToString();
                PostParams += "&sfID=" + System.Configuration.ConfigurationManager.AppSettings["CSC_RFQ_SFID"].ToString();
                PostParams += "&e=" + txtEmail.Text;
                PostParams += "&fn=" + txtFirstName.Text;
                PostParams += "&ln=" + txtLastName.Text;
                PostParams += "&n=" + txtFirstName.Text + " " + txtLastName.Text;

                if (txtJobTitle.Text.Trim() != "")
                    PostParams += "&occ=" + txtJobTitle.Text;

                PostParams += "&compname=" + txtCompany.Text;
                PostParams += "&adr=" + txtAddress.Text;
                PostParams += "&city=" + txtCity.Text;
                PostParams += "&state=" + drpState.SelectedItem.Value;
                PostParams += "&zc=" + txtZip.Text;
                PostParams += "&ctry=" + txtCountry.Text;
                PostParams += "&ph=" + txtPhone.Text;

                if (txtFax.Text.Trim() != "")
                    PostParams += "&fax=" + txtFax.Text;

                if (txtCellPhone.Text.Trim() != "")
                    PostParams += "&mph=" + txtCellPhone.Text;

                if (txtMaterialProduced.Text.Trim() != "")
                    PostParams += "&user_MaterialProduced=" + txtMaterialProduced.Text;
                if (txtMaterialWeightCF.Text.Trim() != "")
                    PostParams += "&user_MaterialWeightCF=" + txtMaterialWeightCF.Text;
                if (txtMaterialWeightCM.Text.Trim() != "")
                    PostParams += "&user_MaterialWeightCM=" + txtMaterialWeightCM.Text;
                if (txtTotalWeight.Text.Trim() != "")
                    PostParams += "&user_TotalWeight=" + txtTotalWeight.Text;
                if (txtMaterialTemp.Text.Trim() != "")
                    PostParams += "&user_MaterialTemp=" + txtMaterialTemp.Text;
                if (getCheckboxValues(chkMatericalChar).Trim() != "")
                    PostParams += "&user_MatericalChar=" + getCheckboxValues(chkMatericalChar);
                if (txtOther.Text.Trim() != "")
                    PostParams += "&user_MC_Other=" + txtOther.Text;
                if (txtBulkDensitylbs.Text.Trim() != "")
                    PostParams += "&user_BulkDensitylbs=" + txtBulkDensitylbs.Text;
                if (txtBulkDensitykgs.Text.Trim() != "")
                    PostParams += "&user_BulkDensitykgs=" + txtBulkDensitykgs.Text;
                if (txtParticleSizeM.Text.Trim() != "")
                    PostParams += "&user_ParticleSizeM=" + txtParticleSizeM.Text;
                if (txtParticuleSizeI.Text.Trim() != "")
                    PostParams += "&user_ParticuleSizeI=" + txtParticuleSizeI.Text;
                if (txtParticuleSizeMS.Text.Trim() != "")
                    PostParams += "&user_ParticuleSizeMS=" + txtParticuleSizeMS.Text;
                if (txtMoistureContent.Text.Trim() != "")
                    PostParams += "&user_MoistureContent=" + txtMoistureContent.Text;
                if (txtEquimentcomments.Text.Trim() != "")
                    PostParams += "&user_Equimentcomments=" + txtEquimentcomments.Text;

                Hashtable hAdvert = new Hashtable();

                for (int i = 1; i <= aCategories.Count - 3; i++)
                {
                    AdvParams = string.Empty;

                    Categories c = (Categories)aCategories[i];
                    PostParams += "&user_" + c.Value + "=X";

                    for (int x = 0; x <= c.SelectedAdvertisers.Count - 1; x++)
                    {
                        AdvParams += (AdvParams == string.Empty ? c.SelectedAdvertisers[x] : "," + c.SelectedAdvertisers[x]);

                        if (hAdvert.Contains(c.SelectedAdvertisers[x].ToString()))
                        {
                            hAdvert[c.SelectedAdvertisers[x].ToString()] = hAdvert[c.SelectedAdvertisers[x].ToString()] + "<BR>" + c.Text;
                        }
                        else
                        {
                            hAdvert.Add(c.SelectedAdvertisers[x].ToString(), c.Text);
                        }
                    }

                    PostParams += "&user_ADV_" + c.Value + "=" + AdvParams;
                }

                //Send Email

                foreach (string keyCol in hAdvert.Keys)
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(ConfigurationManager.AppSettings["CSC_RFQ_Email_From"].ToString());
                    message.To.Add(GetAdvertiserEmail(keyCol));
                    message.CC.Add(ConfigurationManager.AppSettings["CSC_RFQ_Email_CC"].ToString());
                    message.Subject = "PBE Request for Quote.";
                    message.Body = ReplaceCodeSnippets(hAdvert[keyCol].ToString());
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.High;

                    SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
                    smtp.Send(message);
                }

                //HTTP Post.
                HttpPost(PostParams);

                RFQSession = null;

                pnlAdvertisers.Visible = false;
                pnlCategories.Visible = false;
                pnlSubscriptionForm.Visible = false;
                btnback.Visible = false;
                btnNext.Visible = false;
                btnFinish.Visible = false;
                pnlSubscriptionForm2.Visible = false;
                pnlThankYou.Visible = true;
            }
            catch (WebException ex)
            {
                pnlError.Visible = true;
                lblErrMessage.Text = ex.Message;
            }
        }

        private void loadstate()
        {
            string SelectedState = "";

            if (drpState.SelectedIndex > -1)
                SelectedState = drpState.SelectedItem.Value;

            drpState.Items.Clear();

            addlistitem("", "Select a State", "");
            addlistitem("AK", "Alaska", "USA");
            addlistitem("AL", "Alabama", "USA");
            addlistitem("AR", "Arkansas", "USA");
            addlistitem("AZ", "Arizona", "USA");
            addlistitem("CA", "California", "USA");
            addlistitem("CO", "Colorado", "USA");
            addlistitem("CT", "Connecticut", "USA");
            addlistitem("DC", "Washington D.C.", "USA");
            addlistitem("DE", "Delaware", "USA");
            addlistitem("FL", "Florida", "USA");
            addlistitem("GA", "Georgia", "USA");
            addlistitem("HI", "Hawaii", "USA");
            addlistitem("IA", "Iowa", "USA");
            addlistitem("ID", "Idaho", "USA");
            addlistitem("IL", "Illinois", "USA");
            addlistitem("IN", "Indiana", "USA");
            addlistitem("KS", "Kansas", "USA");
            addlistitem("KY", "Kentucky", "USA");
            addlistitem("LA", "Louisiana", "USA");
            addlistitem("MA", "Massachusetts", "USA");
            addlistitem("MD", "Maryland", "USA");
            addlistitem("ME", "Maine", "USA");
            addlistitem("MI", "Michigan", "USA");
            addlistitem("MN", "Minnesota", "USA");
            addlistitem("MO", "Missourri", "USA");
            addlistitem("MS", "Mississippi", "USA");
            addlistitem("MT", "Montana", "USA");
            addlistitem("NC", "North Carolina", "USA");
            addlistitem("ND", "North Dakota", "USA");
            addlistitem("NE", "Nebraska", "USA");
            addlistitem("NH", "New Hampshire", "USA");
            addlistitem("NJ", "New Jersey", "USA");
            addlistitem("NM", "New Mexico", "USA");
            addlistitem("NV", "Nevada", "USA");
            addlistitem("NY", "New York", "USA");
            addlistitem("OH", "Ohio", "USA");
            addlistitem("OK", "Oklahoma", "USA");
            addlistitem("OR", "Oregon", "USA");
            addlistitem("PA", "Pennsylvania", "USA");
            addlistitem("PR", "Puerto Rico", "USA");
            addlistitem("RI", "Rhode Island", "USA");
            addlistitem("SC", "South Carolina", "USA");
            addlistitem("SD", "South Dakota", "USA");
            addlistitem("TN", "Tennessee", "USA");
            addlistitem("TX", "Texas", "USA");
            addlistitem("UT", "Utah", "USA");
            addlistitem("VA", "Virginia", "USA");
            addlistitem("VT", "Vermont", "USA");
            addlistitem("WA", "Washington", "USA");
            addlistitem("WI", "Wisconsin", "USA");
            addlistitem("WV", "West Virginia", "USA");
            addlistitem("WY", "Wyoming", "USA");
            addlistitem("AB", "Alberta", "Canada");
            addlistitem("BC", "British Columbia", "Canada");
            addlistitem("MB", "Manitoba", "Canada");
            addlistitem("NB", "New Brunswick", "Canada");
            addlistitem("NF", "New Foundland", "Canada");
            addlistitem("NS", "Nova Scotia", "Canada");
            addlistitem("ON", "Ontario", "Canada");
            addlistitem("PE", "Prince Edward Island", "Canada");
            addlistitem("QC", "Quebec", "Canada");
            addlistitem("SK", "Saskatchewan", "Canada");
            addlistitem("YT", "Yukon Territories", "Canada");
            addlistitem("AGS", "Aguascalientes", "Mexico");
            addlistitem("BCN", "Baja California Norte", "Mexico");
            addlistitem("BCS", "Baja California Sur", "Mexico");
            addlistitem("CAM", "Campeche", "Mexico");
            addlistitem("CHIS", "Chiapas", "Mexico");
            addlistitem("CHIH", "Chihuahua", "Mexico");
            addlistitem("COAH", "Coahuila", "Mexico");
            addlistitem("COL", "Colima", "Mexico");
            addlistitem("DF", "Distrito Federal", "Mexico");
            addlistitem("DGO", "Durango", "Mexico");
            addlistitem("GTO", "Guanajuato", "Mexico");
            addlistitem("GRO", "Guerrero", "Mexico");
            addlistitem("HGO", "Hidalgo ", "Mexico");
            addlistitem("JAL", "Jalisco ", "Mexico");
            addlistitem("EDM", "Mexico - Estado de", "Mexico");
            addlistitem("MICH", "Michoacán", "Mexico");
            addlistitem("MOR", "Morelos", "Mexico");
            addlistitem("NAY", "Nayarit", "Mexico");
            addlistitem("NL", "Nuevo Leon", "Mexico");
            addlistitem("OAX", "Oaxaca", "Mexico");
            addlistitem("PUE", "Puebla ", "Mexico");
            addlistitem("QRO", "Querétaro", "Mexico");
            addlistitem("QROO", "Quintana Roo", "Mexico");
            addlistitem("SLP", "San Luis Potosí", "Mexico");
            addlistitem("SIN", "Sinaloa", "Mexico");
            addlistitem("SON", "Sonora", "Mexico");
            addlistitem("TAB", "Tabasco", "Mexico");
            addlistitem("TAMPSS", "Tamaulipas", "Mexico");
            addlistitem("TLAX", "Tlaxcala", "Mexico");
            addlistitem("VER", "Veracruz", "Mexico");
            addlistitem("YUC", "Yucatan", "Mexico");
            addlistitem("ZAC", "Zacatecas", "Mexico");
      
            addlistitem("OT", "Other", "Foreign");

            try
            {
                drpState.Items.FindByValue(SelectedState).Selected = true;
            }
            catch { }
        }

        private void addlistitem(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpState.Items.Add(item);

        }
    }
}
[Serializable()]
public class Categories
{

    private string _value;
    private string _text;
    private ArrayList _SelectedAdvertisers = new ArrayList();

    public Categories(string v, string t)
    {
        this._value = v;
        this._text = t;
    }

    public string Value
    {
        get { return _value; }
    }

    public string Text
    {
        get { return _text; }
    }

    public ArrayList SelectedAdvertisers
    {
        get { return _SelectedAdvertisers; }
    }
}