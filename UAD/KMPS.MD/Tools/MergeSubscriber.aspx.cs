using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using KMPS.MD.Objects;
using Telerik.Web.UI;

namespace KMPS.MD.Tools
{
    public partial class MergeSubscriber : System.Web.UI.Page
    {
        private int _subscriptionIDToKeep = 0;
        private int _subscriptionIDToRemove = 0;
        private List<string> _pubsToKeep = new List<string>();
        private List<string> _pubsToRemove = new List<string>();
        private const string ControlCBID1 = "cbID1";
        private const string ControlCBID2 = "cbID2";
        private const string HiddenID1 = "hfID1";
        private const string HiddenID2 = "hfID2";
        private const string LabelGroup1 = "lbltbIGrp_No1";
        private const string LabelGroup2 = "lbltbIGrp_No2";
        private const string HiddenCirc1 = "hfIsCirc1";
        private const string HiddenCirc2 = "hfIsCirc2";
        private const string LabelPub1 = "lblPubName1";
        private const string HiddenPubSubscriptionID1 = "hfPubSubscriptionID1";
        private const string HiddenPubSubscriptionID2 = "hfPubSubscriptionID2";
        private const string PubSubscriptionIDToKeep = "PubSubscriptionIDToKeep";
        private const string PubSubscriptionID = "PubSubscriptionID";
        private const string PubSubscriptionIDToRemove = "PubSubscriptionIDToRemove";
        private const string SubscriberNotSelected = "Subscriber not selected. Please select one";
        private const string SubscriberSelected = "Both subscriber is selected. Please select one";
        private const string ErrorPubCirculationMessage = "These records both contain the same circulation pubcode, and cannot be merged.  Contact your KM Audience Database Specialist if you need more information.";
        private const string CatCode = "Cat Code";
        private const string Status = "Status";
        private const string QualificationSource = "Qualification Source";
        private const string OpenActivityCount = "Open Activity Count";
        private const string ClickActivityCount = "Click Activity Count";
        private const string VisitActivityCount = "Visit Activity Count";
        private const string Open = "open";
        private const string Click = "click";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "Merge Subscriber";
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;
            divMessage.Visible = false;
            lblMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    var subscriber1 = Subscriber.GetByIGrp_No(Master.clientconnections, Guid.Parse(tbIGrp_No1.Text));
                    var subscriber2 = Subscriber.GetByIGrp_No(Master.clientconnections, Guid.Parse(tbIGrp_No2.Text));

                    if (!ValidateSubscription(subscriber1, subscriber2))
                    {
                        return;
                    }

                    hfSubscriptionID1.Value = subscriber1.SubscriptionID.ToString();
                    hfSubscriptionID2.Value = subscriber2.SubscriptionID.ToString();

                    var list = GetSubscriberList(subscriber1, subscriber2);

                    try
                    {
                        var categoryList = Category.GetAll();
                        list.Add(Tuple.Create(CatCode, categoryList.Find(x => x.CategoryCodeID == Convert.ToInt32(subscriber1.CategoryID))
                                            .CategoryCodeName, categoryList.Find(x => x.CategoryCodeID == Convert.ToInt32(subscriber2.CategoryID)).CategoryCodeName));

                        list.Add(Tuple.Create(Status, TransactionCodeType.GetByPubTransactionID(subscriber1.TransactionID.GetValueOrDefault()).TransactionCodeTypeName,
                                            TransactionCodeType.GetByPubTransactionID(subscriber2.TransactionID.GetValueOrDefault()).TransactionCodeTypeName));
                        list.Add(Tuple.Create(QualificationSource, Code.GetByQSourceID(subscriber1.QSourceID.GetValueOrDefault()).DisplayName,
                                            Code.GetByQSourceID(subscriber2.QSourceID.GetValueOrDefault()).DisplayName));
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Description of the error: {0}", ex);
                    }

                    var lsActivity1 = SubscriberActivity.Get(Master.clientconnections, Convert.ToInt32(subscriber1.SubscriptionID), 0);
                    var lsActivity2 = SubscriberActivity.Get(Master.clientconnections, Convert.ToInt32(subscriber2.SubscriptionID), 0);

                    list.Add(Tuple.Create(OpenActivityCount, lsActivity1.Count(o => o.Activity == Open).ToString(), lsActivity2.Count(o => o.Activity == Open).ToString()));
                    list.Add(Tuple.Create(ClickActivityCount, lsActivity1.Count(o => o.Activity == Click).ToString(), lsActivity2.Count(o => o.Activity == Click).ToString()));

                    var lsVisitActivity1 = SubscriberVisitActivity.Get(Master.clientconnections, Convert.ToInt32(subscriber1.SubscriptionID));
                    var lsVisitActivity2 = SubscriberVisitActivity.Get(Master.clientconnections, Convert.ToInt32(subscriber2.SubscriptionID));

                    list.Add(Tuple.Create(VisitActivityCount, lsVisitActivity1.Count().ToString(), lsVisitActivity2.Count().ToString()));

                    rgSubscriptionDetails.DataSource = list;
                    rgSubscriptionDetails.DataBind();

                    rgProducts.DataSource = GetSubscriberData(subscriber1, subscriber2);
                    rgProducts.DataBind();

                    rgProducts.Visible = true;
                    rgSubscriptionDetails.Visible = true;
                    btnMerge.Visible = true;
                }
            }
            catch(Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        private object GetSubscriberData(Subscriber subscriber1, Subscriber subscriber2)
        {
            var sp1 = SubscriberPubs.GetSubscriberPubs(Master.clientconnections, Convert.ToInt32(subscriber1.SubscriptionID), 0);
            var sp2 = SubscriberPubs.GetSubscriberPubs(Master.clientconnections, Convert.ToInt32(subscriber2.SubscriptionID), 0);

            var sp1Data = (from a in sp1
                           join b in sp2 on a.PubID equals b.PubID into temp
                           from b in temp.DefaultIfEmpty()
                           select new
                           {
                               PubID1 = a.PubID,
                               PubID2 = b != null ? b.PubID : 0,
                               PubName1 = a.pubname,
                               PubName2 = b != null ? b.pubname : default(string),
                               ID1 = "cb" + a.SubscriptionID + a.PubID,
                               ID2 = "cb0" + a.PubID,
                               IsCirc1 = a.IsCirc,
                               IsCirc2 = b != null ? b.IsCirc : false,
                           });

            var sp2RemainingData = (from r in sp2
                                    where !(from a in sp1Data select a.PubID1).Contains(r.PubID)
                                    select new
                                    {
                                        PubID1 = 0,
                                        PubID2 = r.PubID,
                                        PubName1 = default(string),
                                        PubName2 = r.pubname,
                                        ID1 = "cb0" + r.PubID,
                                        ID2 = "cb" + r.SubscriptionID + r.PubID,
                                        IsCirc1 = false,
                                        IsCirc2 = r.IsCirc
                                    });

            var fullOuterjoinData = sp1Data.Concat(sp2RemainingData);
            return fullOuterjoinData.ToList();
        }

        private bool ValidateSubscription(Subscriber subscriber1, Subscriber subscriber2)
        {
            if (Convert.ToInt32(subscriber1.SubscriptionID) == 0)
            {
                DisplayError($"{tbIGrp_No1.Text} does not exist.");
                return false;
            }
            else if (Convert.ToInt32(subscriber2.SubscriptionID) == 0)
            {
                DisplayError($"{tbIGrp_No2.Text} does not exist.");
                return false;
            }

            if (tbIGrp_No1.Text.ToUpper() == tbIGrp_No2.Text.ToUpper())
            {
                DisplayError("Both IGrpNos are same. Please select different IGrpNos to merge.");
                return false;
            }

            return true;
        }

        private IList<Tuple<string, string, string>>GetSubscriberList(Subscriber subscriber1, Subscriber subscriber2)
        {
            var list = new List<Tuple<string, string, string>>
            {
                Tuple.Create("First Name", subscriber1.FName, subscriber2.FName),
                Tuple.Create("Last Name", subscriber1.LName, subscriber2.LName),
                Tuple.Create("Title", subscriber1.Title, subscriber2.Title),
                Tuple.Create("Company", subscriber1.Company, subscriber2.Company),
                Tuple.Create("Address", subscriber1.Address, subscriber2.Address),
                Tuple.Create("Mailstop(Address2)", subscriber1.MailStop, subscriber2.MailStop),
                Tuple.Create("Address3", subscriber1.Address3, subscriber2.Address3),
                Tuple.Create("City", subscriber1.City, subscriber2.City),
                Tuple.Create("State", subscriber1.State, subscriber2.State),
                Tuple.Create("Zip", subscriber1.Zip, subscriber2.Zip),
                Tuple.Create("Country", subscriber1.Country, subscriber2.Country),
                Tuple.Create("ForZip", subscriber1.ForZip, subscriber2.ForZip),
                Tuple.Create("Plus4", subscriber1.Plus4, subscriber2.Plus4),
                Tuple.Create("Phone", subscriber1.Phone, subscriber2.Phone),
                Tuple.Create("Fax", subscriber1.Fax, subscriber2.Fax),
                Tuple.Create("Email", subscriber1.Email, subscriber2.Email),
                Tuple.Create("QDate", subscriber1.QDate.ToString(), subscriber2.QDate.ToString()),
                Tuple.Create("Created Date", subscriber1.DateCreated.ToString(), subscriber2.DateCreated.ToString())
            };

            return list;
        }

        string IGRPSelected = string.Empty;

        protected void rgSubscriptionDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Label lblFieldName = (Label)e.Item.FindControl("lblFieldName");
                Label lblFieldValue1 = (Label)e.Item.FindControl("lblFieldValue1");
                Label lblFieldValue2 = (Label)e.Item.FindControl("lblFieldValue2");

                if (lblFieldName.Text.ToUpper() == "CREATED DATE")
                {
                    int c = String.Compare(lblFieldValue1.Text, lblFieldValue2.Text);

                    if (c < 0)
                        IGRPSelected = "Subscriber1";
                    else if (c == 0)
                        IGRPSelected = "Subscriber11Subscriber12";
                    else
                        IGRPSelected = "Subscriber2";
                }

                if (IGRPSelected == "Subscriber11Subscriber12")
                {
                    if (lblFieldName.Text.ToUpper() == "OPEN ACTIVITY COUNT")
                    {
                        int d = String.Compare(lblFieldValue1.Text, lblFieldValue2.Text);

                        if (d < 0)
                            IGRPSelected = "Subscriber2";
                        else if (d == 0)
                            IGRPSelected = "Subscriber11Subscriber12";
                        else
                            IGRPSelected = "Subscriber1";
                    }
                }
            }

            if (e.Item is GridFooterItem)
            {
                CheckBox cbID1 = (CheckBox)e.Item.FindControl("cbID1");
                CheckBox cbID2 = (CheckBox)e.Item.FindControl("cbID2");

                if (IGRPSelected == "Subscriber1")
                    cbID1.Checked = true;
                else if (IGRPSelected == "Subscriber2")
                    cbID2.Checked = true;
            }
        }

        protected void rgProducts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                try
                {
                    GridDataItem item = e.Item as GridDataItem;
                    int PubID1 = Convert.ToInt32(item.GetDataKeyValue("PubID1").ToString());
                    int PubID2 = Convert.ToInt32(item.GetDataKeyValue("PubID2").ToString());

                    PubSubscriptions lps1 = PubSubscriptions.GetByPubIDSubscriptionID(Master.clientconnections, PubID1, Convert.ToInt32(hfSubscriptionID1.Value));
                    PubSubscriptions lps2 = PubSubscriptions.GetByPubIDSubscriptionID(Master.clientconnections, PubID2, Convert.ToInt32(hfSubscriptionID2.Value));

                    var pslist = new List<Tuple<string, string, string>>();

                    pslist.Add(Tuple.Create("First Name", lps1.FirstName, lps2.FirstName));
                    pslist.Add(Tuple.Create("Last Name", lps1.LastName, lps2.LastName));
                    pslist.Add(Tuple.Create("Title", lps1.Title, lps2.Title));
                    pslist.Add(Tuple.Create("Company", lps1.Company, lps2.Company));
                    pslist.Add(Tuple.Create("Address", lps1.Address1, lps2.Address1));
                    pslist.Add(Tuple.Create("Address2", lps1.Address2, lps2.Address2));
                    pslist.Add(Tuple.Create("Address3", lps1.Address3, lps2.Address3));
                    pslist.Add(Tuple.Create("City", lps1.City, lps2.City));
                    pslist.Add(Tuple.Create("State", lps1.RegionCode, lps2.RegionCode));
                    pslist.Add(Tuple.Create("Zip", lps1.ZipCode, lps2.ZipCode));
                    pslist.Add(Tuple.Create("Country", lps1.Country, lps2.Country));
                    pslist.Add(Tuple.Create("ForZip", lps1.ForZip, lps2.ForZip));
                    pslist.Add(Tuple.Create("Plus4", lps1.Plus4, lps2.Plus4));
                    pslist.Add(Tuple.Create("Phone", lps1.Phone, lps2.Phone));
                    pslist.Add(Tuple.Create("Fax", lps1.Fax, lps2.Fax));
                    pslist.Add(Tuple.Create("Email", lps1.Email, lps2.Email));
                    pslist.Add(Tuple.Create("QDate", lps1.QualificationDate.ToString(), lps2.QualificationDate.ToString()));
                    pslist.Add(Tuple.Create("Created Date", lps1.DateCreated.ToString(), lps2.DateCreated.ToString()));

                    try
                    {
                        List<Category> c = Category.GetAll();
                        pslist.Add(Tuple.Create("Cat Code", c.Find(x => x.CategoryCodeID == Convert.ToInt32(lps1.PubCategoryID)).CategoryCodeName, c.Find(x => x.CategoryCodeID == Convert.ToInt32(lps2.PubCategoryID)).CategoryCodeName));

                        pslist.Add(Tuple.Create("Status", TransactionCodeType.GetByPubTransactionID(lps1.PubTransactionID.GetValueOrDefault()).TransactionCodeTypeName, TransactionCodeType.GetByPubTransactionID(lps2.PubTransactionID.GetValueOrDefault()).TransactionCodeTypeName));
                        pslist.Add(Tuple.Create("Qualification Source", Code.GetByQSourceID(lps1.PubQSourceID.GetValueOrDefault()).DisplayName, Code.GetByQSourceID(lps2.PubQSourceID.GetValueOrDefault()).DisplayName));
                    }
                    catch
                    {
                    }

                    List<SubscriberOpenActivity> lsOpenActivity1 = SubscriberOpenActivity.GetByPubSubscriptionID(Master.clientconnections, Convert.ToInt32(lps1.PubSubscriptionID), Convert.ToInt32(PubID1));
                    List<SubscriberOpenActivity> lsOpenActivity2 = SubscriberOpenActivity.GetByPubSubscriptionID(Master.clientconnections, Convert.ToInt32(lps2.PubSubscriptionID), Convert.ToInt32(PubID2));
                    List<SubscriberClickActivity> lsClickActivity1 = SubscriberClickActivity.GetByPubSubscriptionID(Master.clientconnections, Convert.ToInt32(lps1.PubSubscriptionID), Convert.ToInt32(PubID1));
                    List<SubscriberClickActivity> lsClickActivity2 = SubscriberClickActivity.GetByPubSubscriptionID(Master.clientconnections, Convert.ToInt32(lps2.PubSubscriptionID), Convert.ToInt32(PubID2));

                    pslist.Add(Tuple.Create("Open Activity Count", lsOpenActivity1.Count().ToString(), lsOpenActivity2.Count().ToString()));
                    pslist.Add(Tuple.Create("Click Activity Count", lsClickActivity1.Count().ToString(), lsClickActivity2.Count().ToString()));

                    List<PubSubscriptionsDimension> psd1 = PubSubscriptionsDimension.GetPubSubscriptionsDimension(Master.clientconnections, Convert.ToInt32(hfSubscriptionID1.Value), Convert.ToInt32(PubID1));
                    List<PubSubscriptionsDimension> psd2 = PubSubscriptionsDimension.GetPubSubscriptionsDimension(Master.clientconnections, Convert.ToInt32(hfSubscriptionID2.Value), Convert.ToInt32(PubID2));

                    var sp1Data = (from a in psd1
                                   join b in psd2 on a.ResponseGroupID equals b.ResponseGroupID into temp
                                   from b in temp.DefaultIfEmpty()
                                   select new
                                   {
                                       ResponseGroupID1 = a.ResponseGroupID,
                                       ResponseGroupID2 = b != null ? b.ResponseGroupID : 0,
                                       ResponseGroupName1 = a.ResponseGroupName,
                                       ResponseGroupName2 = b != null ? b.ResponseGroupName : default(string),
                                       ResponseDesc1 = a.ResponseDesc,
                                       ResponseDesc2 = b != null ? b.ResponseDesc : default(string)
                                   });

                    var sp2RemainingData = (from r in psd2
                                            where !(from a in sp1Data select (int)a.ResponseGroupID1).Contains((int)r.ResponseGroupID)
                                            select new
                                            {
                                                ResponseGroupID1 = 0,
                                                ResponseGroupID2 = r.ResponseGroupID,
                                                ResponseGroupName1 = default(string),
                                                ResponseGroupName2 = r.ResponseGroupName,
                                                ResponseDesc1 = default(string),
                                                ResponseDesc2 = r.ResponseDesc
                                            });

                    var fullOuterjoinData = sp1Data.Concat(sp2RemainingData);

                    foreach (var s in fullOuterjoinData)
                    {
                        pslist.Add(Tuple.Create(s.ResponseGroupName1 == null || s.ResponseGroupName1 == string.Empty ? s.ResponseGroupName2 : s.ResponseGroupName1, s.ResponseDesc1 == null ? "" : s.ResponseDesc1, s.ResponseDesc2 == null ? "" : s.ResponseDesc2));
                    }

                    RadGrid rgPubSubscriptionDetails = (RadGrid)e.Item.FindControl("rgPubSubscriptionDetails");
                    rgPubSubscriptionDetails.DataSource = pslist.ToList();
                    rgPubSubscriptionDetails.DataBind();

                    HiddenField hfPubSubscriptionID1 = (HiddenField)e.Item.FindControl("hfPubSubscriptionID1");
                    HiddenField hfPubID1 = (HiddenField)e.Item.FindControl("hfPubID1");
                    hfPubSubscriptionID1.Value = lps1.PubSubscriptionID.ToString();
                    hfPubID1.Value = PubID1.ToString();

                    HiddenField hfPubSubscriptionID2 = (HiddenField)e.Item.FindControl("hfPubSubscriptionID2");
                    HiddenField hfPubID2 = (HiddenField)e.Item.FindControl("hfPubID2");
                    hfPubSubscriptionID2.Value = lps2.PubSubscriptionID.ToString();
                    hfPubID2.Value = PubID2.ToString();

                    CheckBox cbID1 = (CheckBox)e.Item.FindControl("cbID1");
                    CheckBox cbID2 = (CheckBox)e.Item.FindControl("cbID2");
                    if (PubID1 == 0)
                    {
                        cbID2.Checked = true;
                        cbID2.Enabled = false;
                    }

                    if (PubID2 == 0)
                    {
                        cbID1.Checked = true;
                        cbID1.Enabled = false;
                    }

                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

         protected void ResetControls()
        {
            tbIGrp_No1.Text = string.Empty;
            tbIGrp_No2.Text = string.Empty;
            hfSubscriptionID1.Value = string.Empty;
            hfSubscriptionID2.Value = string.Empty;
            rgProducts.DataSource = null;
            rgProducts.DataBind();
            rgSubscriptionDetails.DataSource = null;
            rgSubscriptionDetails.DataBind();
            rgProducts.Visible = false;
            rgSubscriptionDetails.Visible = false;
            btnMerge.Visible = false;
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            try
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    var iGRPSelected = string.Empty; 

                    foreach (GridDataItem gridDataItem in rgProducts.Items)
                    {
                        var hfIsCirc1 = (HiddenField)gridDataItem.FindControl(HiddenCirc1);
                        var hfIsCirc2 = (HiddenField)gridDataItem.FindControl(HiddenCirc2);

                        if (Convert.ToBoolean(hfIsCirc1.Value) == true && Convert.ToBoolean(hfIsCirc2.Value) == true)
                        {
                            DisplayError(ErrorPubCirculationMessage);
                            return;
                        }
                    }

                    if (!VerifyFooter())
                    {
                        return;
                    }

                    if (!VerifyProductItems())
                    {
                        return;
                    }

                    var docToKeep = new XDocument(new XElement(PubSubscriptionIDToKeep,  
                             from i in _pubsToKeep select new XElement(PubSubscriptionID, i)));

                    var docToRemove = new XDocument(new XElement(PubSubscriptionIDToRemove,
                             from i in _pubsToRemove select new XElement(PubSubscriptionID, i)));

                    Subscriber.Merge(Master.clientconnections, _subscriptionIDToKeep, _subscriptionIDToRemove, docToKeep, docToRemove, Master.LoggedInUser);
                    divMessage.Visible = true;
                    lblMessage.Text = $"IGrpNo : {iGRPSelected} has been updated.";
                    ResetControls();
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        private bool VerifyFooter()
        {
            foreach (var footerItem in rgSubscriptionDetails.MasterTableView.GetItems(GridItemType.Footer))
            {
                var cbID1 = (CheckBox)footerItem.FindControl(ControlCBID1);
                var cbID2 = (CheckBox)footerItem.FindControl(ControlCBID2);
                var hfID1 = (HiddenField)footerItem.FindControl(HiddenID1);
                var hfID2 = (HiddenField)footerItem.FindControl(HiddenID2);
                var lbltbIGrp_No1 = (Label)footerItem.FindControl(LabelGroup1);
                var lbltbIGrp_No2 = (Label)footerItem.FindControl(LabelGroup2);

                if (!cbID1.Checked && !cbID2.Checked)
                {
                    DisplayError(SubscriberNotSelected);
                    return false;
                }

                if (cbID1.Checked && cbID2.Checked)
                {
                    DisplayError(SubscriberSelected);
                    return false;
                }

                if (cbID1.Checked)
                {
                    IGRPSelected = lbltbIGrp_No1.Text;
                    _subscriptionIDToKeep = Convert.ToInt32(hfID1.Value);
                    _subscriptionIDToRemove = Convert.ToInt32(hfID2.Value);
                }
                else
                {
                    IGRPSelected = lbltbIGrp_No2.Text;
                    _subscriptionIDToKeep = Convert.ToInt32(hfID2.Value);
                    _subscriptionIDToRemove = Convert.ToInt32(hfID1.Value);
                }
            }

            return true;
        }

        private bool VerifyProductItems()
        {
            foreach (GridDataItem gridDataItem in rgProducts.Items)
            {
                var cbID1 = (CheckBox)gridDataItem.FindControl(ControlCBID1);
                var cbID2 = (CheckBox)gridDataItem.FindControl(ControlCBID2);
                var lblPubName1 = (Label)gridDataItem.FindControl(LabelPub1);
                var hfPubSubscriptionID1 = (HiddenField)gridDataItem.FindControl(HiddenPubSubscriptionID1);
                var hfPubSubscriptionID2 = (HiddenField)gridDataItem.FindControl(HiddenPubSubscriptionID2);

                if (!cbID1.Checked && !cbID2.Checked)
                {
                    DisplayError($"{lblPubName1.Text} is not selected. Please select one.");
                    return false;
                }

                if (cbID1.Checked && cbID2.Checked)
                {
                    DisplayError($"Both {lblPubName1.Text} is selected. Please select one.");
                    return false;
                }

                if (cbID1.Checked)
                {
                    _pubsToKeep.Add(hfPubSubscriptionID1.Value);
                    if (Convert.ToInt32(hfPubSubscriptionID2.Value) > 0)
                    {
                        _pubsToRemove.Add(hfPubSubscriptionID2.Value);
                    }
                }
                else
                {
                    if (cbID2.Checked)
                    {
                        _pubsToKeep.Add(hfPubSubscriptionID2.Value);
                        if (Convert.ToInt32(hfPubSubscriptionID1.Value) > 0)
                        {
                            _pubsToRemove.Add(hfPubSubscriptionID1.Value);
                        }
                    }
                }
            }

            return true;
        }
    }
}