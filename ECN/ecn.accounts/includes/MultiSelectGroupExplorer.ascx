<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiSelectGroupExplorer.ascx.cs" Inherits="ecn.accounts.includes.MultiSelectGroupExplorer" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/includes/folderSystem.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>

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
        overflow: auto;
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
</style>
<script type="text/javascript">

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
        <asp:Panel ID="pnlGroup" runat="server" BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px">
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0' bgcolor="#ffffff">
                <asp:Panel ID="pnlGroupSearch" runat="Server">
                    <tr>
                        <td valign="top">
                            <table cellpadding="0" cellspacing="0" border='0' width="100%">
                                <tr>
                                    <td></td>
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
                                    <asp:ListItem Value="like"> contains </asp:ListItem>
                                    <asp:ListItem Value="equals"> equals </asp:ListItem>
                                    <asp:ListItem Value="starts"> starts with </asp:ListItem>
                                    <asp:ListItem Value="ends"> ends with </asp:ListItem>
                                </asp:DropDownList>
                                            &nbsp;
                                <asp:TextBox class="formtextfield" ID="SearchCriteria" runat="Server" EnableViewState="False"
                                    Columns="16"></asp:TextBox>&nbsp;
                                            &nbsp;
                                    <asp:DropDownList ID="ddlArchiveFilter" runat="server">
                                        <asp:ListItem Text="Active" Value="active" Selected="True" />
                                        <asp:ListItem Text="Archived" Value="archived" />
                                        <asp:ListItem Text="All" Value="all" />
                                    </asp:DropDownList>
                                            &nbsp;
                                            <asp:CheckBox ID="chkAllFolders" runat="server" Text="All Folders" AutoPostBack="false" />
                                            &nbsp;
                                <asp:Button class="formbuttonsmall" ID="SearchButton"
                                    runat="Server" EnableViewState="False" Width="75" Visible="true" OnClick="SearchButton_Click" Text="Search"></asp:Button>
                                            &nbsp; &nbsp;
                                 
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
                                                <asp:HyperLink ID="hlFolders" runat="server" Text="Folders" NavigateUrl="~/main/folders/folderseditor.aspx" />
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
                                <td style="border: solid 1px #CCCCCC; border-top: none; max-width: 57%; width: 57%;" valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                            <ecnCustom:ecnGridView ID="GroupsGrid" runat="Server" Width="100%" CssClass="grid"
                                        AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="GroupsGrid_Command"
                                        OnPageIndexChanging="GroupsGrid_PageIndexChanging" DataKeyNames="GroupID" PageSize="15"
                                        OnRowDataBound="GroupsGrid_RowDataBound">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>

                                            <asp:TemplateField HeaderText="Folder Name" ItemStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFolderName" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" ItemStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
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
                                                <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageGroup_TextChanged" class="formtextfield" Width="30px" />
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
                                <asp:Panel runat="server" ID="pnlSelectedGroup">
                                    <td width="30%">
                                        <table width="100%">
                                            <tr class="gridheader">
                                                <td>Selected Groups
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="SelectedGroupID" runat="server" Value="0" />
                                                    <div style="max-height: 400px; overflow-y: auto;">
                                                        <ecnCustom:ecnGridView ID="gvSelectedGroups" AllowSorting="true" GridLines="None" RowStyle-BorderStyle="None"
                                                            OnRowDataBound="gvSelectedGroups_RowDataBound" AllowPaging="false" runat="Server"
                                                            HorizontalAlign="Center" OnRowCommand="gvSelectedGroups_Command" AutoGenerateColumns="False"
                                                            Width="100%" DataKeyNames="GroupID">
                                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                            <%--<AlternatingRowStyle CssClass="gridaltrow" BorderStyle="None" />--%>
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
                                                                <asp:TemplateField HeaderText=""
                                                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Remove Group' border='0'&gt;"
                                                                            CausesValidation="false" ID="SelectGroupBtn" CommandName="RemoveGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </ecnCustom:ecnGridView>
                                                    </div>
                                                    <br />
                                                    <br />
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









