<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="testGroupExplorer.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.testGroupExplorer" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/includes/folderSystem.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Src="~/main/ECNWizard/Group/addSubscribers.ascx" TagName="addSubscribers" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Group/addGroup.ascx" TagName="addGroup" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Group/filters.ascx" TagName="filters" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Group/importSubscribers.ascx" TagName="importSubscribers" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Group/filtergrid.ascx" TagName="filterGrid" TagPrefix="uc1" %>
<style type="text/css">
    .accordionHeader {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        color: white;
        font-weight: bold;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }

    .accordionHeaderSelected {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        font-weight: bold;
        color: white;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }

    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .modalPopupFull {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
    }

    .modalPopup {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
    }

    .modalPopupImport {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 60%;
        overflow: auto;
    }

    .style1 {
        width: 100%;
    }

    .buttonMedium {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }

    .TransparentGrayBackground {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }

    .overlay {
        position: fixed;
        z-index: 99;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }

    * html .overlay {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }

    .loader {
        z-index: 100;
        position: fixed;
        width: 120px;
        margin-left: -60px;
        background-color: #F4F3E1;
        font-size: x-small;
        color: black;
        border: solid 2px Black;
        top: 40%;
        left: 50%;
    }

    * html .loader {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
</style>
<script type="text/javascript">
    function deleteGroup(theID) {
        if (confirm('Are you Sure?\n Selected Group and all associated emails will be permanently deleted.')) {
            window.location = "default.aspx?GroupID=" + theID + "&action=delete";
        }
    }
    function deleteSegment(theID) {
        if (confirm('Are you Sure?\n Selected Segment and all associated profiles will be permanently deleted.')) {
            window.location = "default.aspx?SegmentID=" + theID + "&action=delete";
        }
    }
    function MM_goToURL() {
        var i, args = MM_goToURL.arguments; document.MM_returnValue = false;
        for (i = 0; i < (args.length - 1) ; i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
    }
</script>

<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upMain" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
            <table cellspacing="0" cellpadding="0" width="674" align="center">
                <tr>
                    <td id="errorTop"></td>
                </tr>
                <tr>
                    <td id="errorMiddle">
                        <table width="80%">
                            <tr>
                                <td valign="top" align="center" width="20%">
                                    <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                </td>
                                <td valign="middle" align="left" width="80%" height="100%">
                                    <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id="errorBottom"></td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <asp:Panel ID="pnlGroup" runat="server">
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <asp:Panel ID="pnlGroupSearch" runat="Server">
                    <tr>
                        <td valign="top">
                            <table cellpadding="0" cellspacing="0" border='0' width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Panel ID="FilterPanel" runat="Server" Visible="true">
                                            <asp:DropDownList class="formfield" ID="SearchTypeDR" runat="Server" Visible="true"
                                                EnableViewState="False" Width="100">
                                                <asp:ListItem Value="Group" Selected="True">Group</asp:ListItem>
                                                <asp:ListItem Value="Profile">Profile in Group</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                <asp:DropDownList class="formfield" ID="SearchGrpsDR" runat="Server" Visible="true"
                                    EnableViewState="False" Width="125">
                                    <asp:ListItem Value="starts"> value starting with </asp:ListItem>
                                    <asp:ListItem Value="like"> value containing any </asp:ListItem>
                                    <asp:ListItem Value="ends"> value ending with </asp:ListItem>
                                    <asp:ListItem Value="equals"> value equals </asp:ListItem>
                                </asp:DropDownList>
                                            &nbsp;
                                <asp:TextBox class="formtextfield" ID="SearchCriteria" runat="Server" EnableViewState="False"
                                    Columns="16"></asp:TextBox>&nbsp;
                                <asp:Button class="formbuttonsmall" ID="SearchButton"
                                    runat="Server" EnableViewState="False" OnClick="SearchButton_Click" Width="75" Visible="true" Text="Search"></asp:Button>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td>

                        <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                            <tr valign="top">
                                <td width="12%" valign="top">
                                    <table class="grid" cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">
                                        <tr class="gridheader">
                                            <th align="left" scope="col">
                                                <a href="javascript:void" onclick="MM_goToURL('parent','../folders/folderseditor.aspx');return document.MM_returnValue">
                                                    <b>Folders</b></a>
                                            </th>
                                        </tr>
                                    </table>
                                    <div style="overflow: auto; width: 200px; height: 400px; border: solid 1px #CCCCCC; border-top: none;">
                                        <ecn:FolderSys ID="GroupFolder" runat="Server"></ecn:FolderSys>
                                        <br />
                                    </div>
                                </td>
                                <td width="1%">&nbsp;
                                </td>
                                <td style="border: solid 1px #CCCCCC; border-top: none;" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <ecnCustom:ecnGridView ID="GroupsGrid" runat="Server" Width="100%" CssClass="grid"
                                                    AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="GroupsGrid_Command"
                                                    OnPageIndexChanging="GroupsGrid_PageIndexChanging" DataKeyNames="GroupID" PageSize="15"
                                                    OnRowDataBound="GroupsGrid_RowDataBound">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ItemStyle-Width="30%"
                                                            ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                        <asp:BoundField DataField="Subscribers" HeaderText="Subscribers" HeaderStyle-HorizontalAlign="center"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" Visible="false"></asp:BoundField>
                                                        <asp:HyperLinkField ItemStyle-Width="7%" HeaderStyle-Width="7%" HeaderText="Edit" Visible="false"
                                                            HeaderStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit Group / View Emails' border='0'>"
                                                            DataNavigateUrlFields="GroupID" DataNavigateUrlFormatString="~/main/lists/groupeditor.aspx?GroupID={0}"
                                                            ItemStyle-HorizontalAlign="center"></asp:HyperLinkField>
                                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="center" Visible="false">
                                                            <ItemStyle Width="3%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Group' border='0'&gt;"
                                                                    CausesValidation="false" ID="DeleteGroupBtn" CommandName="DeleteGroup" OnClientClick="return confirm('Are you sure you want to delete this group?');"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Filters" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                            Visible="false" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-groupFilter.gif alt='Add Filter' border='0'&gt;"
                                                                    CausesValidation="false" ID="AddFilterBtn" CommandName="AddFilter" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                            Visible="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/add_icon.jpg alt='Select Group' border='0'&gt;"
                                                                    CausesValidation="false" ID="SelectGroupBtn" CommandName="SelectGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />

                                                </ecnCustom:ecnGridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlPager" runat="server" Visible="false">
                                                    <table cellpadding="0" border="0" width="100%">
                                                        <tr>
                                                            <td class="label" style="width:33%;">
                                                                
                                                                    Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                                </td>
                                                            <td style="width:33%;">
                                                                
                                                                    Show Rows:
                                                                    
                                                                    <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GroupGrid_SelectedIndexChanged" CssClass="formfield">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            <td style="width:34%;">
                                                                
                                                                    Page
                                                <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageContent_TextChanged" class="formtextfield" Width="30px" />
                                                                    of
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                                                    
                                                <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Prev" />
                                                                    <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" />
                                                                

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>

                                        </tr>
                                    </table>

                                </td>
                                <td>&nbsp&nbsp
                                </td>
                                <asp:Panel runat="server" ID="pnlSelectedGroup" Visible="true">
                                    <td width="40%">
                                        <table width="100%">
                                            <tr class="gridheader">
                                                <td>Selected Groups
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hfLicenseExceed" runat="server" Value="" />
                                                    <asp:HiddenField ID="SelectedGroupID" runat="server" Value="0" />
                                                    <asp:Label ID="lblEmptyGrid_Selected" runat="server" Text="No Groups Selected"></asp:Label>
                                                    <ecnCustom:ecnGridView ID="gvSelectedGroups" AllowSorting="true"
                                                        OnRowDataBound="gvSelectedGroups_RowDataBound" AllowPaging="false" runat="Server" RowStyle-BorderStyle="None"
                                                        HorizontalAlign="Center" OnRowCommand="gvSelectedGroups_Command" AutoGenerateColumns="False"
                                                        Width="100%" DataKeyNames="GroupID" GridLines="None">
                                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                        <%--<AlternatingRowStyle CssClass="gridaltrow" />--%>
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="GroupID"
                                                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Group Name"
                                                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGroupName" Font-Size="Small" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ADD" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnAddSubs" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" Text="ADD" CommandName="addsubs" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IMPORT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnImportSubs" runat="server" ImageUrl="/ecn.images/images/icon-add.gif" Text="IMPORT" CommandName="importsubs" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText=""
                                                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-cancel.gif alt='Remove Group' border='0'&gt;"
                                                                        CausesValidation="false" ID="SelectGroupBtn" CommandName="RemoveGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr valign="top" style="top: 10px; border-bottom: 1px solid gray;">
                                                                    <td colspan="5" align="left">
                                                                        <table style="width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <uc1:filterGrid ID="ucFilterGrid" SuppressOrSelect="testselect" IsTestBlast="true" runat="server" />
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </ecnCustom:ecnGridView>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr class="gridheader">
                                                <td>Test Blast Details
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                    <table width="100%">
                                                        <tr>
                                                            <td class="dataOne" width="100%">
                                                                <table class="tablecontent" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <b>Test Blast License: </b><b>
                                                                                <asp:Label ID="BlastLicensed" runat="Server"></asp:Label>
                                                                            </b>
                                                                        </td>
                                                                        <td>
                                                                            <b>This Blast: </b><b>
                                                                                <asp:Label ID="BlastThis" runat="Server"></asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="btnShowPopup5" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupFilterEdit" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlFilterEdit" TargetControlID="btnShowPopup5">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlFilterEdit" CssClass="modalPopup" Width="600">
    <asp:UpdateProgress ID="upProgressFilterEdit" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="UpdatePanel6" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upProgressFilterEditP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upProgressFilterEditP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <uc1:filters ID="filterEdit1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: center">
                        <asp:HiddenField ID="hfShowSelect" runat="server" Value="0" />
                        <asp:Button runat="server" Text="Close" ID="btnFilterEdit_Close" CssClass="formfield"
                            OnClick="FilterEdit_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupImport" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlImport" TargetControlID="btnShowPopup2">
</ajaxToolkit:ModalPopupExtender>

<asp:Panel runat="server" ID="pnlImport" CssClass="modalPopupImport">

    <uc1:importSubscribers ID="importSubscribers1" runat="server" />
    <table align="center" class="style1">
        <tr>
            <td style="text-align: center">
                <asp:Button runat="server" Text="Close" ID="btnImport" CssClass="formfield"
                    OnClick="ImportGroup_Hide"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupAddSubscribers" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlAddSubscribers" TargetControlID="btnShowPopup1">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlAddSubscribers" CssClass="modalPopup">
    <asp:UpdateProgress ID="upAddSubscribersProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upAddSubscribersProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upAddSubscribersProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <uc1:addSubscribers ID="addSubscribers1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnAddSubscribers" CssClass="formfield"
                            OnClick="AddSubscribers_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnClose" OnClick="AddSubscribers_Close" CssClass="formfield"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupAddGroup" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlAddGroup" TargetControlID="btnShowPopup3">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlAddGroup" CssClass="modalPopup">
    <asp:UpdateProgress ID="upAddGroupProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upAddGroupProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upAddGroupProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <uc1:addGroup ID="addGroup1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnAddGroup" CssClass="formfield"
                            OnClick="AddGroup_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnAddGroup_Close" OnClick="AddGroup_Close" CssClass="formfield"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="hfAddFilter" runat="server" style="display:none;"/>
<ajaxToolkit:ModalPopupExtender ID="mpeAddFilter" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlAddFilter" TargetControlID="hfAddFilter" />
<asp:Panel runat="server" ID="pnlAddFilter" Width="500px" Height="300px" CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="pnlFilterConfig" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upProgressFilterEditControl3" CssClass="overlay" runat="server">
                <asp:Panel ID="upProgressFilterEditControlP3" CssClass="loader" runat="server">
                    <div>
                        <center>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlFilterConfig" UpdateMode="Conditional" style="width: 100%; height: 100%;" runat="server">
        <ContentTemplate>
            <table style="background-color: white; width: 100%; max-width: 98%; height: 100%; max-height: 98%; margin: auto;">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblFilterType" runat="server" OnSelectedIndexChanged="rblFilterType_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                            <asp:ListItem Text="SMART" Value="smart" Selected="True" />
                            <asp:ListItem Text="CUSTOM" Value="custom" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlSmartSegment" Width="100%" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlSmartSegment" runat="server" AutoPostBack="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRefBlastError" runat="server" ForeColor="Red" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstboxBlast" runat="server" Width="90%" SelectionMode="Multiple" />
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlCustomFilter" Visible="false" Width="100%" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="imgbtnCreateFilter" runat="server" CommandName="createcustomfilter" OnClick="imgbtnCreateFilter_Click" ImageUrl="/ecn.images/images/icon-add.gif" />
                                        <asp:Label ID="lblCreateFilter" Text="Add New Filter" runat="server" />

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="height: 150px; overflow: auto;">
                                            <asp:ListBox ID="lbAvailableFilters" Width="100%" runat="server" SelectionMode="Multiple" AutoPostBack="false" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCancelFilter" runat="server" OnClick="btnCancelFilter_Click" Text="Cancel" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSaveFilter" runat="server" OnClick="btnSaveFilter_Click" CommandName="savefilter" Text="Save" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Panel>
