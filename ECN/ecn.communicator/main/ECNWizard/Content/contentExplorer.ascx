<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="contentExplorer.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Content.contentExplorer" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/includes/folderSystem.ascx" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<br/>

<script>
    function MM_goToURL() { //v3.0
        var i, args = MM_goToURL.arguments;
        document.MM_returnValue = false;
        for (i = 0; i < (args.length - 1) ; i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
    }
</script>

<script>
    $(function() {
        var focusedElementId = "";
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            var fe = document.activeElement;
            if (fe != null) {
                focusedElementId = fe.id;
            } else {
                focusedElementId = "";
            }
        });

        prm.add_endRequest(function () {
            if (focusedElementId !== "") {
                $("#" + focusedElementId).focus();
            }
        });
    });
</script>

<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
        </tr>
        <tr>
            <td id="errorMiddle">
                <table height="67" width="80%">
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<br/>
<asp:Panel ID="pnlTitleContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" style="height: 30px;" border="0">
        <tr class="gridheader">
            <td align="left" valign="middle" style="font-size: 14px;" align="left" width="50%">
                &nbsp;&nbsp;&nbsp;&nbsp;Content&nbsp;&nbsp;
            </td>
            <td valign="middle" align="right" style="font-size: 12px;" width="50%">
                <asp:Panel runat="server" ID="pnlSelectedContent" Visible="false">
                    Selected Content: <asp:Label ID="lblSelectedContentID" runat="server" Text="None"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlContent" runat="server" Style="border: solid 1px #ccc">
    <table width="100%" cellspacing="1" cellpadding="1" border="0" bgcolor="#FFFFFF">
        <tr height="40px">
            <td align="right" class="label" valign="middle">
                <table width="100%" cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td align="left">
                            Title to Search
                        </td>
                        <td align="left">
                            Users
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox class="formtextfield" ID="ContentSearchCriteria" runat="Server" EnableViewState="False"
                                         Columns="16" Width="265px">
                            </asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ContentUserID" AutoPostBack="false" runat="Server" DataValueField="UserID"
                                              DataTextField="UserName" CssClass="formfield">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxAllFoldersContent" runat="server" Text="All Folders"></asp:CheckBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlArchiveFilter" runat="server">
                                <asp:ListItem Text="Active" Value="active" Selected="True" />
                                <asp:ListItem Text="Archived" Value="archived" />
                                <asp:ListItem Text="All" Value="all" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button class="formbuttonsmall" ID="ContentSearchButton" runat="Server" OnClick="ContentSearchButton_Click" EnableViewState="False"
                                        Width="75" Visible="true" Text="Search">
                            </asp:Button>
                            <asp:Button class="formbuttonsmall" ID="ContentClearButton" OnClick="ContentClearButton_Click"
                                        runat="Server" EnableViewState="False" Width="75" Visible="true" Text="Clear Search"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="#ffffff">
                <table style="height: 100%; width: 100%;" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td width="14%" valign="top">
                            <table class="grid" cellspacing="0" cellpadding="4" border="0" style="border-collapse: collapse; width: 100%;">
                                <tr class="gridheader">
                                    <th align="left" scope="col">
                                        <a href="javascript:void" onclick=" MM_goToURL('parent', '../../main/folders/folderseditor.aspx');return document.MM_returnValue; ">
                                            <b>Folders</b>
                                        </a>
                                    </th>
                                </tr>
                            </table>
                            <div style="border: solid 1px #CCCCCC; border-top: none; height: 450px; overflow: auto; width: 200px;">
                                <ecn:foldersys id="ContentFolderID" runat="Server"></ecn:foldersys>
                                <br/>
                            </div>
                        </td>
                        <td width="1%">
                            &nbsp;
                        </td>
                        <td width="85%" style="border: solid 1px #CCCCCC; border-top: none;" valign="top">
                            <ecncustom:ecngridview id="ContentGrid" runat="Server" width="100%" cssclass="grid"
                                                   autogeneratecolumns="False" allowpaging="True" allowsorting="true" OnRowCommand="ContentGrid_Command"
                                                   onsortcommand="ContentGrid_sortCommand" OnRowDataBound="ContentGrid_RowDataBound" datakeynames="ContentID" pagesize="15">
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="38%" HeaderStyle-Width="38%" ItemStyle-HorizontalAlign="Left"
                                                    SortExpression="FolderName" HeaderText="Folder Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFolderName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HtmlEncode="False" ItemStyle-Width="38%" HeaderStyle-Width="38%" ItemStyle-HorizontalAlign="Left"
                                                    SortExpression="ContentTitle" DataField="ContentTitle" HeaderText="Title">
                                    </asp:BoundField>
                                    <asp:BoundField ItemStyle-Width="10%" HeaderStyle-Width="10%" SortExpression="CreatedDate" DataField="CreatedDate"
                                         HeaderText="Created Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField ItemStyle-Width="10%" HeaderStyle-Width="10%" SortExpression="UpdatedDate"
                                                    DataField="UpdatedDate" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="center"
                                                    ItemStyle-HorizontalAlign="center">
                                    </asp:BoundField>
                                    <asp:BoundField ItemStyle-Width="7%" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="center"
                                                    HeaderStyle-HorizontalAlign="center" DataField="ContentTypeCode" HeaderText="Type">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Validated" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsValidated" runat="server" OnCheckedChanged="chkIsValidated_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Content" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <center>
                                                &nbsp;<img src='/ecn.images/images/<%# DataBinder.Eval(Container.DataItem, "LockedFlag").Equals("N") ? "icon-nolock.gif' alt='Content NOT Locked'" : "icon-lock.gif' alt='Content Locked'" %>'
                                                           border="0">&nbsp;
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:HyperLinkField ItemStyle-Width="7%" HeaderStyle-Width="7%" HeaderText="Link Alias"
                                                        HeaderStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-linkAlias2.gif alt='Edit / View Link alias' border='0'>"
                                                        DataNavigateUrlFields="ContentID" DataNavigateUrlFormatString="~/main/content/linkAlias.aspx?ContentID={0}"
                                                        ItemStyle-HorizontalAlign="center">
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField ItemStyle-Width="7%" HeaderStyle-Width="7%" HeaderText="Preview Html"
                                                        HeaderStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Content as HTML' border='0'>"
                                                        DataNavigateUrlFields="ContentID" DataNavigateUrlFormatString="~/main/content/contentPreview.aspx?ContentID={0}&amp;type=html"
                                                        Target="_blank" ItemStyle-HorizontalAlign="center">
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField ItemStyle-Width="7%" HeaderStyle-Width="7%" HeaderText="Preview Text"
                                                        HeaderStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-TEXT.gif alt='Preview Content as TEXT' border='0'>"
                                                        DataNavigateUrlFields="ContentID" DataNavigateUrlFormatString="~/main/content/contentPreview.aspx?ContentID={0}&amp;type=text"
                                                        Target="_blank" ItemStyle-HorizontalAlign="center">
                                    </asp:HyperLinkField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="5%" HeaderStyle-Width="5%"
                                                       HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <a href='contenteditor.aspx?ContentID=<%# DataBinder.Eval(Container.DataItem, "ContentIDPlus") %>&amp;action=Edit'
                                               id="editLink">
                                                <center>
                                                    <img src="/ecn.images/images/icon-edits1.gif" alt="Edit Content" border="0">
                                                </center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                       HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                                            CausesValidation="false" ID="DeleteContentBtn" CommandName="DeleteContent" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ContentID") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%" Visible="false"
                                                       HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Select Content' border='0'&gt;"
                                                            CausesValidation="false" ID="SelectContentBtn" CommandName="SelectContent" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ContentID") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Archived" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsArchived" runat="server" OnCheckedChanged="chkIsArchived_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle CssClass="gridaltrow" />
                            </ecncustom:ecngridview>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td align="right">
                            <asp:Panel ID="pnlPager" runat="server" Visible="false">
                                <table cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td class="label">
                                            <div style="float: left; padding-right:114px">
                                                Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />    
                                            </div>
                                            <div style="float: left;">
                                                Show Rows:
                                                <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ContentGrid_SelectedIndexChanged" CssClass="formfield">
                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                </asp:DropDownList>    
                                            </div>
                                            <div style="float: right">
                                                Page
                                                <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageContent_TextChanged" class="formtextfield" Width="30px" />
                                                of
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label"/>
                                                &nbsp;
                                                <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Previous"/>
                                                <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>"/>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>