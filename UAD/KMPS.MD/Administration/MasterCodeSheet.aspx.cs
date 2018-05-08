using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using KM.Common.Utilities.Xml;
using KMPS.MD.Objects;

namespace KMPS.MDAdmin
{
    public partial class MasterCodeSheet : KMPS.MD.Main.WebPageHelper
    {
        private const string RootNodeName = "mastergrouplist";
        private const string ItemNodeName = "mastergroup";
        private const int MaxCollectionSize = 5000;

        private string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        private string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        private int MasterGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["MasterGroupID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        //static string prevPage = String.Empty;
        //public static string connStr = DataFunctions.GetClientSqlConnection(Master.clientconnections);
        public string groupvalue;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Master Groups";
            Master.SubMenu = "Master Code Sheet";
            btnSave.Text = "SAVE";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            divMessage.Visible = false;
            lblMessage.Text = "";
            lblpnlHeader.Text = "Add Responses";

            if (!IsPostBack)
            {
                drpMasterGroups.DataSource = MasterGroup.GetAll(Master.clientconnections);
                drpMasterGroups.DataBind();

                if (MasterGroupID() > 0)
                {
                    drpMasterGroups.Items.FindByValue(MasterGroupID().ToString()).Selected = true;
                }

                SortField = "MASTERVALUE";
                SortDirection = "ASC";
                LoadGrid();
            }
        }

        protected void LoadGrid()
        {
            try
            {
                List<KMPS.MD.Objects.MasterCodeSheet> mc = KMPS.MD.Objects.MasterCodeSheet.GetByMasterGroupID(Master.clientconnections, Convert.ToInt32(drpMasterGroups.SelectedItem.Value));

                List<KMPS.MD.Objects.MasterCodeSheet> lst = null;

                if (mc != null && mc.Count > 0)
                {
                    switch (SortField.ToUpper())
                    {
                        case "MASTERVALUE":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = mc.OrderBy(o => o.MasterValue).ToList();
                            else
                                lst = mc.OrderByDescending(o => o.MasterValue).ToList();
                            break;

                        case "MASTERDESC":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = mc.OrderBy(o => o.MasterDesc).ToList();
                            else
                                lst = mc.OrderByDescending(o => o.MasterDesc).ToList();
                            break;
                        case "MASTERDESC1":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = mc.OrderBy(o => o.MasterDesc1).ToList();
                            else
                                lst = mc.OrderByDescending(o => o.MasterDesc1).ToList();
                            break;
                        case "ENABLESEARCHING":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = mc.OrderBy(o => o.EnableSearching).ToList();
                            else
                                lst = mc.OrderByDescending(o => o.EnableSearching).ToList();
                            break;
                    }
                }

                if (lst != null)
                {
                    if (txtSearch.Text != string.Empty)
                    {
                        lst = lst.FindAll(x => x.MasterValue.ToLower().Contains(txtSearch.Text.ToLower()) || (x.MasterDesc.ToLower().Contains(txtSearch.Text.ToLower())));
                    }
                }

                gvMasterCodeSheet.DataSource = lst;
                gvMasterCodeSheet.DataBind();
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }


        protected void gvMasterCodeSheet_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
            {
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }        

        protected void gvMasterCodeSheet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMasterCodeSheet.PageIndex = e.NewPageIndex;
            LoadGrid();
        }


        protected void dvMasterCodeSheet_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception != null)
        {
                lblErrorMessage.Text = "Error : " + e.Exception.Message;
                divError.Visible = true;
                e.ExceptionHandled = true;
            }
            else
            {
                gvMasterCodeSheet.DataBind();
                drpMasterGroups.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {            
            try
            {

                if (Convert.ToInt32(hfMasterID.Value) == 0)
                {
                    if (KMPS.MD.Objects.MasterCodeSheet.ExistsByName(Master.clientconnections, txtResponseValue.Text, Convert.ToInt32(drpMasterGroups.SelectedValue)) != 0)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = " Value Already Exists";
                        return;
                    }
                }
                else
                {
                    if (KMPS.MD.Objects.MasterCodeSheet.ExistsByIDName(Master.clientconnections, Convert.ToInt32(hfMasterID.Value), txtResponseValue.Text, Convert.ToInt32(drpMasterGroups.SelectedValue)) != 0)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = " Value Already Exists";
                        return;
                    }
                }

                KMPS.MD.Objects.MasterCodeSheet ms = new KMPS.MD.Objects.MasterCodeSheet();
                ms.MasterID = Convert.ToInt32(hfMasterID.Value);
                ms.MasterGroupID =  Convert.ToInt32(drpMasterGroups.SelectedValue);
                ms.MasterValue = txtResponseValue.Text;
                ms.MasterDesc = txtResponseDesc.Text;
                ms.MasterDesc1 = txtResponseDesc1.Text;
                ms.EnableSearching = Convert.ToBoolean(drpEnableSearching.SelectedValue);

                if (ms.MasterID > 0)
                {
                    ms.UpdatedByUserID = Master.LoggedInUser;
                    ms.DateUpdated = DateTime.Now;
                }
                else
                {
                    ms.CreatedByUserID = Master.LoggedInUser;
                    ms.DateCreated = DateTime.Now;
                }

                KMPS.MD.Objects.MasterCodeSheet.Save(Master.clientconnections, ms);

                Response.Redirect("MasterCodeSheet.aspx?MasterGroupID=" + drpMasterGroups.SelectedItem.Value, true);
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MasterCodeSheet.aspx?MasterGroupID=" + drpMasterGroups.SelectedItem.Value);
        }

        protected void gvMasterCodeSheet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    KMPS.MD.Objects.MasterCodeSheet ms = KMPS.MD.Objects.MasterCodeSheet.GetByMasterID(Master.clientconnections, Convert.ToInt32(gvMasterCodeSheet.DataKeys[Convert.ToInt32(e.CommandArgument)].Value));

                    hfMasterID.Value = ms.MasterID.ToString();
                    drpMasterGroups.SelectedValue = ms.MasterGroupID.ToString();
                    txtResponseValue.Text = ms.MasterValue;
                    txtResponseDesc.Text = ms.MasterDesc;
                    txtResponseDesc1.Text = ms.MasterDesc1;
                    drpEnableSearching.SelectedValue = ms.EnableSearching.ToString();
                    btnSave.Text = "SAVE";
                    lblpnlHeader.Text = "Edit Responses";
                }
                if (e.CommandName == "Delete")
                {
                    KMPS.MD.Objects.MasterCodeSheet.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }

        protected void gvMasterCodeSheet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        } 
        protected void drpMasterGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadGrid();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileSelector.PostedFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                if (ValidateUploadCsvHeaders(FileSelector.PostedFile.InputStream))
                {
                    var xDocList = SerializeCsvFile(FileSelector.PostedFile.InputStream, RootNodeName, ItemNodeName, MaxCollectionSize).ToList();

                    try
                    {
                        if (!UploadedMasterValuesContainCommas(xDocList))
                        {
                                foreach (XDocument xDoc in xDocList)
                                {
                                    var count = (from c in xDocList.Descendants() where c.Value.Contains(',') select c).Count();

                                    KMPS.MD.Objects.MasterCodeSheet.Import(Master.clientconnections, Convert.ToInt32(drpMasterGroups.SelectedValue), xDoc, Master.LoggedInUser);
                                }

                                lblMessage.Text = "File successfully uploaded.";
                                divMessage.Visible = true;

                                LoadGrid();
                        }
                        else
                        {
                            throw new Exception("Values in MASTERVALUE cannot contain commas.");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrorMessage.Text = "An error has occured uploading your file.<br /><br />" + ex.Message;
                        divError.Visible = true;
                    }
                }
                else
                {

                    lblErrorMessage.Text = "The headers for your csv file are incorrect. The correct format is IGroupNo,MasterValue,MasterDesc";
                    divError.Visible = true;
                }                
            }
            else
            {
                lblErrorMessage.Text = "ERROR - Cannot Upload File: <br><br>Only files with an extension of .csv are supported.";
                divError.Visible = true;
            }
        }

        private bool UploadedMasterValuesContainCommas(List<XDocument> xDocList)
        {
            bool containsValue = false;

            foreach (var xDoc in xDocList)
            {
                containsValue = (from c in xDoc.Descendants("MasterGroup").Elements("MASTERVALUE")
                                 where c.Value.Contains(',')
                                 select c).Count() > 0;

                if (containsValue)
                {
                    break;
                }
            }

            return containsValue;
        }

        private bool ValidateUploadCsvHeaders(Stream stream)
        {
            bool isValid = false;

            MemoryStream memoryStream = new MemoryStream();

            try
            {
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    string headerString = reader.ReadLine();
                    isValid = string.Compare(headerString, "IGROUPNO,MASTERVALUE,MASTERDESC", true) == 0;
                }
            }
            finally
            {
                stream.Position = 0;

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }

            return isValid;
        }

        private static IEnumerable<XDocument> SerializeCsvFile(Stream inputStream, string rootNode, string itemNode, int maxCollectionSize)
        {
            return XmlSerializerUtility.SerializeCsvFile(inputStream, rootNode, itemNode, maxCollectionSize);
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            UploadCsvFileLabel.Text = string.Concat("Master Group: ", drpMasterGroups.SelectedItem.Text);
            mdlPopupCsv.Show();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadGrid();
        }
    }
}