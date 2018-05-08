using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;

using ecn.common.classes;

namespace PaidPub.main.Forms
{
    public partial class Add : System.Web.UI.Page
    {
        private int FormID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["FormID"]); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (FormID > 0)
                {
                    LoadForm();
                }
                LoadNewsletters();
            }
        }

        private void LoadNewsletters()
        {
            string selectedGroups = getallListboxValues(lstSelectedNewsLetters);

            DataTable dtNewsletter = DataFunctions.GetDataTable("select g.GroupID, (case when ec.categoryID is null then '' else ec.name end) + ' / ' +  g.Groupname as Groupname from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID left outer join CANON_PAIDPUB_eNewsLetter_Category ec on n.categoryID = ec.categoryID where g.groupID not in (" + (selectedGroups == "" ? "0" : selectedGroups) + ") and n.customerID = " + Session["CustomerID"].ToString() + " order by groupname", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
            lstNewsletters.DataSource = dtNewsletter;
            lstNewsletters.DataTextField = "GroupName";
            lstNewsletters.DataValueField = "GroupID";
            lstNewsletters.DataBind();
        }

        private void LoadSelectedNewsletters(string groupIDs)
        {
            DataTable dtNewsletter = DataFunctions.GetDataTable("select g.GroupID, (case when ec.categoryID is null then '' else ec.name end) + ' / ' +  g.Groupname as Groupname from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID left outer join CANON_PAIDPUB_eNewsLetter_Category ec on n.categoryID = ec.categoryID where g.groupID in (" + groupIDs + ") and n.customerID = " + Session["CustomerID"].ToString(), ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            string[] IDs = groupIDs.Split(',');

            for (int i = 0; i < IDs.Length; i++)
            {
                try
                {
                    foreach (DataRow dr in dtNewsletter.Rows)
                    { 
                        if (Convert.ToInt32(IDs[i]) == Convert.ToInt32(dr["groupID"]))
                        {
                            lstSelectedNewsLetters.Items.Add(new ListItem(dr["groupname"].ToString(), dr["groupID"].ToString()));
                            break;
                        }
                    }
                }
                catch
                { }
            }
        }


        private void LoadForm()
        {
            DataTable dtForm = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_Forms where customerID = " + Session["CustomerID"].ToString() + " and FormID = " + FormID, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtForm.Rows.Count > 0)
            {
                pnlURL.Visible = true;
                txtformname.Text = dtForm.Rows[0]["Name"].ToString();
                DescHTML.Value = dtForm.Rows[0]["Description"].ToString();
                HeaderHTML.Value = dtForm.Rows[0]["HeaderHTML"].ToString();
                FooterHTML.Value = dtForm.Rows[0]["FooterHTML"].ToString();
                newsletterHTML.Value = dtForm.Rows[0]["newsletterHTML"].ToString();

                rbFormType.ClearSelection();

                if (Convert.ToBoolean(dtForm.Rows[0]["IsTrial"]))
                    rbFormType.Items.FindByValue("1").Selected = true;
                else
                    rbFormType.Items.FindByValue("0").Selected = true;


                txtURL.Text = "http://eforms.kmpsgroup.com/paidpub/subscribe.aspx?Code=" + dtForm.Rows[0]["Code"].ToString();

                LoadSelectedNewsletters(dtForm.Rows[0]["groupIDs"].ToString());

                
            }
            else
            {
                lblErrorMessage.Text = "Subscription Form not exists.";
                lblErrorMessage.Visible = true;
                btnSave.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;
            string GroupIDs = getallListboxValues(lstSelectedNewsLetters);

            if (GroupIDs != "")
            {
                try
                {
                    if (FormID > 0)
                    {
                        sqlquery = "update CANON_PAIDPUB_Forms set Name = '" + txtformname.Text.Replace("'", "''") + "', Description = '" + DescHTML.Value.Replace("'", "''") + "', HeaderHTML = '" + HeaderHTML.Value.Replace("'", "''") + "', FooterHTML = '" + FooterHTML.Value.Replace("'", "''") + "', newsletterHTML = '" + newsletterHTML.Value.Replace("'", "''") + "', GroupIDs = '" + GroupIDs + "',IsActive = " + rbStatus.SelectedItem.Value + ", IsTrial = " + rbFormType.SelectedItem.Value  + " where CustomerID = " + Session["CustomerID"].ToString() + " and FormID = " + FormID.ToString();
                    }
                    else
                    {
                        string guid = System.Guid.NewGuid().ToString().Substring(0, 10);

                        sqlquery = "INSERT INTO CANON_PAIDPUB_Forms VALUES ('" + txtformname.Text.Replace("'", "''") + "','" + DescHTML.Value.Replace("'", "''") + "','" + guid + "'," + Session["CustomerID"].ToString() + ",'" + HeaderHTML.Value.Replace("'", "''") + "','" + FooterHTML.Value.Replace("'", "''") + "','" + newsletterHTML.Value.Replace("'", "''") + "','" + GroupIDs + "'," + rbStatus.SelectedItem.Value + "," + rbFormType.SelectedItem.Value + ")";
                    }
                    //Response.Write(sqlquery);

                    DataFunctions.Execute("misc", sqlquery);
                    Response.Redirect("default.aspx");
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "ERROR : " + ex.Message;
                    lblErrorMessage.Visible = true;
                }
            }
            else
            {
                {
                    lblErrorMessage.Text = "ERROR : Select Newsletter.";
                    lblErrorMessage.Visible = true;
                }
            }
        }

        private string getListboxValues(ListBox lst)
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

        private string getallListboxValues(ListBox lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
            }
            return selectedvalues;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in lstNewsletters.Items)
            {
                if (item.Selected)
                {
                    lstSelectedNewsLetters.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
            LoadNewsletters();
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            ArrayList selecteditem = new ArrayList();

            foreach (ListItem item in lstSelectedNewsLetters.Items)
            {
                if (item.Selected)
                    selecteditem.Add(item);
                   
            }
            for (int j = 0; j <= selecteditem.Count - 1; j++)
                lstSelectedNewsLetters.Items.Remove((ListItem) selecteditem[j]);

            LoadNewsletters();
        }

        private void moveItem(int i, int selIndex)
        {
            string selValue;
            string selText;

            if (selIndex + i < 0 || selIndex + i > lstSelectedNewsLetters.Items.Count - 1) { return; }

            selValue = lstSelectedNewsLetters.Items[selIndex].Value;
            selText = lstSelectedNewsLetters.Items[selIndex].Text;

            lstSelectedNewsLetters.Items[selIndex].Value = lstSelectedNewsLetters.Items[selIndex + i].Value;
            lstSelectedNewsLetters.Items[selIndex].Text = lstSelectedNewsLetters.Items[selIndex + i].Text;

            lstSelectedNewsLetters.Items[selIndex + i].Value = selValue;
            lstSelectedNewsLetters.Items[selIndex + i].Text = selText;

            lstSelectedNewsLetters.Items.FindByValue(selValue).Selected = true;

        }
        protected void btnmoveup_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList selecteditem = new ArrayList();
            int i = 0;
            foreach (ListItem item in lstSelectedNewsLetters.Items)
            {
                if (item.Selected)
                {
                    selecteditem.Add(i);
                }
                i++;
            }
            lstSelectedNewsLetters.ClearSelection();

            for (int j = 0; j <= selecteditem.Count - 1; j++)
                    moveItem(-1, Convert.ToInt32(selecteditem[j]));
        }

        protected void btnmovedown_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList selecteditem = new ArrayList();
            int i = 0;
            foreach (ListItem item in lstSelectedNewsLetters.Items)
            {
                if (item.Selected)
                {
                    selecteditem.Add(i);
                }
                i++;
            }

            lstSelectedNewsLetters.ClearSelection();

            for (int j = selecteditem.Count - 1; j >= 0; j--)
                moveItem(1, Convert.ToInt32(selecteditem[j]));
        }


    }
}
