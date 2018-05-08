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
using System.Linq;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;


namespace ecn.communicator.main.content
{
    public partial class LinkSource : ECN_Framework.WebPageHelper
    {
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "Link Source";
            Master.Heading = "Content/Messages > Link Source";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            if (!IsPostBack)
            {
                if (!KMPlatform.BusinessLogic.Client.HasServiceFeature(Master.UserSession.CurrentCustomer.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner, KMPlatform.Enums.Access.View))
                {
                    ViewState["LinkOwnerSortField"] = "LinkOwnerName";
                    ViewState["LinkOwnerSortDirection"] = "ASC";

                    ViewState["LinkTypeSortField"] = "CodeValue";
                    ViewState["LinkTypeSortDirection"] = "ASC";

                    loadLinkOwnerList();
                    loadLinkTypeList();

                    gvLinkType.Columns[1].Visible = dvLinkType.Visible = dvLinkOwner.Visible = gvLinkOwner.Columns[5].Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner, KMPlatform.Enums.Access.Edit);
                    gvLinkType.Columns[2].Visible = gvLinkOwner.Columns[6].Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner, KMPlatform.Enums.Access.Delete);
                }
                


            }
        }

        #region Link Owner

        private void loadLinkOwnerList()
        {
            List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> linkOwnerIndexList = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            var result = (from src in linkOwnerIndexList
                          orderby src.LinkOwnerName
                          select src).ToList();

            if (ViewState["LinkOwnerSortField"].ToString().Equals("LinkOwnerCode"))
            {
                result = (from src in linkOwnerIndexList
                          orderby src.LinkOwnerCode
                          select src).ToList();
            }
            else if (ViewState["LinkOwnerSortField"].ToString().Equals("ContactFirstName"))
            {
                result = (from src in linkOwnerIndexList
                          orderby src.ContactFirstName
                          select src).ToList();
            }
            else if (ViewState["LinkOwnerSortField"].ToString().Equals("ContactLastName"))
            {
                result = (from src in linkOwnerIndexList
                          orderby src.ContactLastName
                          select src).ToList();
            }
            else if (ViewState["LinkOwnerSortField"].ToString().Equals("IsActive"))
            {
                result = (from src in linkOwnerIndexList
                          orderby src.IsActive
                          select src).ToList();
            }
            else if (ViewState["LinkOwnerSortField"].ToString().Equals("LinkOwnerName"))
            {
                result = (from src in linkOwnerIndexList
                          orderby src.LinkOwnerName
                          select src).ToList();
            }
            
            if (ViewState["LinkOwnerSortDirection"].ToString().Equals("DESC"))
            {
                result.Reverse();
            }
            gvLinkOwner.DataSource = result;
            gvLinkOwner.DataBind();
        }

        private void loadLinkTypeList()
        {
            List<ECN_Framework_Entities.Communicator.Code> codeList = ECN_Framework_BusinessLayer.Communicator.Code.GetByCustomerAndCategory(ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE, Master.UserSession.CurrentUser);
            var result = (from src in codeList
                          orderby src.CodeValue
                          select src).ToList();
            if (ViewState["LinkTypeSortDirection"].ToString().Equals("DESC"))
            {
                codeList.Reverse();
            }
            gvLinkType.DataSource = codeList;
            gvLinkType.DataBind();
        }

        protected void dvLinkOwner_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwnerIndex = new ECN_Framework_Entities.Communicator.LinkOwnerIndex();
                linkOwnerIndex.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                linkOwnerIndex.LinkOwnerName = ((TextBox)dvLinkOwner.FindControl("LinkOwnerName")).Text;
                linkOwnerIndex.LinkOwnerCode = ((TextBox)dvLinkOwner.FindControl("LinkOwnerCode")).Text;
                linkOwnerIndex.ContactFirstName = ((TextBox)dvLinkOwner.FindControl("ContactFirstName")).Text;
                linkOwnerIndex.ContactLastName = ((TextBox)dvLinkOwner.FindControl("ContactLastName")).Text;
                linkOwnerIndex.ContactPhone = ((TextBox)dvLinkOwner.FindControl("ContactPhone")).Text;
                linkOwnerIndex.ContactEmail = ((TextBox)dvLinkOwner.FindControl("ContactEmail")).Text;
                linkOwnerIndex.Address = ((TextBox)dvLinkOwner.FindControl("Address")).Text;
                linkOwnerIndex.City = ((TextBox)dvLinkOwner.FindControl("City")).Text;
                linkOwnerIndex.State = ((TextBox)dvLinkOwner.FindControl("State")).Text;
                linkOwnerIndex.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                linkOwnerIndex.IsActive = Convert.ToBoolean(((RadioButtonList)dvLinkOwner.FindControl("IsActive")).SelectedValue);
                ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.Save(linkOwnerIndex, Master.UserSession.CurrentUser);
                loadLinkOwnerList();
                dvLinkOwner.ChangeMode(DetailsViewMode.Insert);
                dvLinkOwner.DataBind();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }      

        protected void dvLinkOwner_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                int LinkOwnerIndexID = Convert.ToInt32(e.Keys[0].ToString());
                ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwnerIndex = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID(LinkOwnerIndexID, Master.UserSession.CurrentUser);
                linkOwnerIndex.LinkOwnerName = ((TextBox)dvLinkOwner.FindControl("LinkOwnerName")).Text;
                linkOwnerIndex.LinkOwnerCode = ((TextBox)dvLinkOwner.FindControl("LinkOwnerCode")).Text;
                linkOwnerIndex.ContactFirstName = ((TextBox)dvLinkOwner.FindControl("ContactFirstName")).Text;
                linkOwnerIndex.ContactLastName = ((TextBox)dvLinkOwner.FindControl("ContactLastName")).Text;
                linkOwnerIndex.ContactPhone = ((TextBox)dvLinkOwner.FindControl("ContactPhone")).Text;
                linkOwnerIndex.ContactEmail = ((TextBox)dvLinkOwner.FindControl("ContactEmail")).Text;
                linkOwnerIndex.Address = ((TextBox)dvLinkOwner.FindControl("Address")).Text;
                linkOwnerIndex.City = ((TextBox)dvLinkOwner.FindControl("City")).Text;
                linkOwnerIndex.State = ((TextBox)dvLinkOwner.FindControl("State")).Text;
                linkOwnerIndex.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                linkOwnerIndex.IsActive = Convert.ToBoolean(((RadioButtonList)dvLinkOwner.FindControl("IsActive")).SelectedValue);
                ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.Save(linkOwnerIndex, Master.UserSession.CurrentUser);
                loadLinkOwnerList();
                dvLinkOwner.ChangeMode(DetailsViewMode.Insert);
                dvLinkOwner.DataBind();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void dvLinkOwner_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            dvLinkOwner.ChangeMode(DetailsViewMode.Insert);
        }

        protected void gvLinkOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvLinkOwner.ChangeMode(DetailsViewMode.Edit);
        }

        protected void gvLinkOwner_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString().Equals(ViewState["LinkOwnerSortField"].ToString()))
            {
                switch (ViewState["LinkOwnerSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["LinkOwnerSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["LinkOwnerSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["LinkOwnerSortField"] = e.SortExpression;
                ViewState["LinkOwnerSortDirection"] = "ASC";
            }
            loadLinkOwnerList();
            dvLinkOwner.ChangeMode(DetailsViewMode.Insert);
        }

        protected void gvLinkOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLinkOwner.PageIndex = e.NewPageIndex;
            loadLinkOwnerList();
            dvLinkOwner.ChangeMode(DetailsViewMode.Insert);
        }

        protected void gvLinkOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                int rowIndex = 0;
                try
                {
                    rowIndex = int.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    return;
                }
             

                if (e.CommandName == "EditLinkOwnerName")
                {
                    int LinkOwnerIndexID = Convert.ToInt32(gvLinkOwner.DataKeys[rowIndex]["LinkOwnerIndexID"].ToString());
                    ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwnerIndex = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID(LinkOwnerIndexID, Master.UserSession.CurrentUser);
                    var linkOwnerIndexList = new List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> { linkOwnerIndex };
                    dvLinkOwner.ChangeMode(DetailsViewMode.Edit);
                    dvLinkOwner.DataSource = linkOwnerIndexList;
                    dvLinkOwner.DataBind();
                }
                if (e.CommandName == "DeleteLinkOwnerName")
                {
                    int LinkOwnerIndexID = Convert.ToInt32(gvLinkOwner.DataKeys[rowIndex]["LinkOwnerIndexID"].ToString());
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.Delete(LinkOwnerIndexID, Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
                        loadLinkOwnerList();
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                }
            }
        }
        #endregion

        #region Link Type

        protected void gvLinkType_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int rowIndex = 0;
            try
            {
                rowIndex = int.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                return;
            }
            int CodeID = Convert.ToInt32(gvLinkType.DataKeys[rowIndex]["CodeID"].ToString());

            if (e.CommandName == "EditLinkType")
            {
                ECN_Framework_Entities.Communicator.Code code = ECN_Framework_BusinessLayer.Communicator.Code.GetByCodeID(CodeID, Master.UserSession.CurrentUser);
                List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();
                codeList.Add(code);
                dvLinkType.ChangeMode(DetailsViewMode.Edit);
                dvLinkType.DataSource = codeList;
                dvLinkType.DataBind();
            }
            if (e.CommandName == "DeleteLinkType")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Code.Delete(CodeID, ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE, Master.UserSession.CurrentUser);
                    loadLinkTypeList();
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        protected void gvLinkType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvLinkType.PageIndex = e.NewPageIndex;
            loadLinkTypeList();
        }

        protected void gvLinkType_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString().Equals(ViewState["LinkTypeSortField"].ToString()))
            {
                switch (ViewState["LinkTypeSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["LinkTypeSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["LinkTypeSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["LinkTypeSortField"] = e.SortExpression;
                ViewState["LinkTypeSortDirection"] = "ASC";
            }
            loadLinkTypeList();
            dvLinkType.ChangeMode(DetailsViewMode.Insert);
        }


        protected void dvLinkType_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Code code = new ECN_Framework_Entities.Communicator.Code();
                code.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                code.CodeType = "LinkType";
                code.CodeValue = ((TextBox)dvLinkType.FindControl("CodeValue")).Text;
                code.CodeDisplay = ((TextBox)dvLinkType.FindControl("CodeValue")).Text;
                code.SortOrder = 1;
                code.DisplayFlag = "Y";
                code.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Communicator.Code.Save(code, Master.UserSession.CurrentUser);
                loadLinkTypeList();
                dvLinkType.ChangeMode(DetailsViewMode.Insert);
                dvLinkType.DataBind();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }
        
        protected void dvLinkType_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            dvLinkType.ChangeMode(DetailsViewMode.Insert);
        }

        protected void dvLinkType_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                int CodeID = Convert.ToInt32(e.Keys[0].ToString());
                ECN_Framework_Entities.Communicator.Code code = ECN_Framework_BusinessLayer.Communicator.Code.GetByCodeID(CodeID, Master.UserSession.CurrentUser);
                code.CodeValue = ((TextBox)dvLinkType.FindControl("CodeValue")).Text;
                code.CodeDisplay = ((TextBox)dvLinkType.FindControl("CodeValue")).Text;
                code.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Communicator.Code.Save(code, Master.UserSession.CurrentUser);
                loadLinkTypeList();
                dvLinkType.ChangeMode(DetailsViewMode.Insert);
                dvLinkType.DataBind();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        #endregion

       
    }
}
