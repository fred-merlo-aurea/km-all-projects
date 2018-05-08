<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="layoutExplorer.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Content.layoutExplorer" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/includes/folderSystem.ascx" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<br />

<script>
    $(function () {
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
            <td id="errorTop"></td>
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
            <td id="errorBottom"></td>
        </tr>
    </table>
</asp:PlaceHolder>
<br />

<asp:UpdatePanel ID="pnlMessage" runat="server" UpdateMode="Conditional" Style="border: solid 1px #ccc">
    <ContentTemplate>
        <asp:Panel ID="pnlTitleLayout" runat="server">
            <table width="100%" cellpadding="0" cellspacing="0" style="height: 30px;" border="0">
                <tr class="gridheader">
                    <td align="left" valign="middle" style="font-size: 14px;" width="50%">&nbsp;&nbsp;&nbsp;&nbsp;Message&nbsp;&nbsp;
                    </td>
                    <td valign="middle" align="right" style="font-size: 14px;" width="50%">
                        <asp:Panel runat="server" ID="pnlSelectedLayout" Visible="false">
                            Selected Message:
                    <asp:Label ID="lblSelectedLayoutID" runat="server" Text="None"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border="0" bgcolor="#ffffff">
            <tr height="40px">
                <td align="" class="label" valign="middle">Name to Search :
                <asp:TextBox class="formtextfield" ID="LayoutSearchCriteria" Width="200px" runat="Server"
                    EnableViewState="False" Columns="16">
                </asp:TextBox>&nbsp;&nbsp; Users :
                <asp:DropDownList ID="LayoutUserID" AutoPostBack="false" runat="Server" DataValueField="UserID"
                    DataTextField="UserName" CssClass="formfield">
                </asp:DropDownList>
                    <asp:CheckBox ID="cbxAllFoldersLayout" runat="server" Text="All Folders"></asp:CheckBox>
                    <asp:DropDownList ID="ddlArchiveFilter" runat="server">
                        <asp:ListItem Text="Active" Value="active" Selected="True" />
                        <asp:ListItem Text="Archived" Value="archived" />
                        <asp:ListItem Text="All" Value="all" />
                    </asp:DropDownList>
                    <asp:Button class="formbuttonsmall" ID="LayoutSearchButton" runat="Server" OnClick="LayoutSearchButton_Click" EnableViewState="False"
                        Width="75" Visible="true" Text="Search"></asp:Button>
                    <asp:Button class="formbuttonsmall" ID="LayoutClearButton" OnClick="LayoutClearButton_Click"
                        runat="Server" EnableViewState="False" Width="75" Visible="true" Text="Clear Search"></asp:Button>
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
                                            <asp:HyperLink ID="hlFolders" runat="server" Text="Folders" NavigateUrl="~/main/folders/folderseditor.aspx" />
                                        </th>
                                    </tr>
                                </table>
                                <div style="border: solid 1px #CCCCCC; border-top: none; height: 450px; overflow: auto; width: 200px;">
                                    <ecn:FolderSys ID="LayoutFolderID" runat="Server"></ecn:FolderSys>
                                    <br />
                                </div>
                            </td>
                            <td width="1%">&nbsp;
                            </td>
                            <td width="85%" style="border: solid 1px #CCCCCC; border-top: none;" valign="top">

                                <ecnCustom:ecnGridView ID="LayoutsGrid" runat="Server" Width="100%" CssClass="grid"
                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" OnRowCommand="LayoutsGrid_Command"
                                    onsortcommand="LayoutsGrid_sortCommand" OnRowDataBound="LayoutsGrid_RowDataBound" DataKeyNames="LayoutID" PageSize="15">
                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20%" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FolderName" HeaderText="Folder Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFolderName" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HtmlEncode="False" ItemStyle-Width="38%" HeaderStyle-Width="38%" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="LayoutName" DataField="LayoutName" HeaderText="Name"></asp:BoundField>
                                        <asp:BoundField ItemStyle-Width="10%" HeaderStyle-Width="10%" SortExpression="CreatedDate"
                                            DataField="CreatedDate" HeaderText="Created Date" HeaderStyle-HorizontalAlign="center"
                                            ItemStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="10%" HeaderStyle-Width="10%" SortExpression="UpdatedDate"
                                            DataField="UpdatedDate" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="center"
                                            ItemStyle-HorizontalAlign="center"></asp:BoundField>
                                        <asp:BoundField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="7%" ItemStyle-Width="7%"
                                            DataField="SlotsTotal" HeaderText="Slots"></asp:BoundField>
                                        <asp:TemplateField HeaderText="ROI Calc." HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="7%"
                                            ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <center>
                                                <a href="#" onclick=" window.open('layoutCostEditor.aspx?LayoutID=<%# DataBinder.Eval(Container.DataItem, "LayoutID") %>', 'ROI_Cost', 'left=10,top=10,height=400,width=555,resizable=no,scrollbar=no,status=no'); ">
                                                    <sub>
                                                        <img src="/ecn.images/images/icon-linkROI.gif" border="0" alt="View / Setup ROI Values">
                                                    </sub>
                                                </a>
                                            </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:HyperLinkField HeaderText="Link Tracking" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-linkTracking.gif alt='Setup Conversion Tracking links' border='0'>"
                                            DataNavigateUrlFields="LayoutID" DataNavigateUrlFormatString="~/main/content/conversionTrackingLinks.aspx?layoutID={0}"></asp:HyperLinkField>
                                        <asp:HyperLinkField HeaderText="Preview Html" HeaderStyle-Width="7%" ItemStyle-Width="7%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Message as HTML' border='0'>"
                                            DataNavigateUrlFields="LayoutID" Target="_blank" DataNavigateUrlFormatString="~/main/content/preview.aspx?LayoutID={0}"></asp:HyperLinkField>
                                        <asp:HyperLinkField HeaderText="Preview Text" HeaderStyle-Width="7%" ItemStyle-Width="7%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-TEXT.gif alt='Preview Message as TEXT' border='0'>"
                                            DataNavigateUrlFields="LayoutID" Target="_blank" DataNavigateUrlFormatString="~/main/content/preview.aspx?LayoutID={0}&Format=text"></asp:HyperLinkField>
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="4%" ItemStyle-Width="4%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                <a href="layouteditor.aspx?LayoutID=<%# DataBinder.Eval(Container.DataItem, "LayoutID") %>&amp;action=edit">
                                                    <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CreatedUserID")).Equals("0") ? "" : "<img src=/ecn.images/images/icon-edits1.gif alt='Edit Message' border='0'>" %>
                                                </a>
                                            </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Layout' border='0'&gt;"
                                                    CausesValidation="false" ID="DeleteLayoutBtn" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LayoutID") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%" Visible="false"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Select Message' border='0'&gt;"
                                                    CausesValidation="false" ID="SelectLayoutBtn" CommandName="SelectLayout" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LayoutID") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Archived" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsArchived" runat="server" OnCheckedChanged="chkIsArchived_CheckedChanged" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                </ecnCustom:ecnGridView>
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
                                                <div style="float: left; padding-right: 114px">
                                                    Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                </div>
                                                <div style="float: left;">
                                                    Show Rows:
                                                <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LayoutsGrid_SelectedIndexChanged" CssClass="formfield">
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
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                                    &nbsp;
                                                <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Previous" />
                                                    <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" />
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
    </ContentTemplate>
</asp:UpdatePanel>
