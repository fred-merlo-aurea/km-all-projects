<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="filters.aspx.cs" Inherits="ecn.communicator.listsmanager.filtersplus"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteFilter(theID) {
            if (confirm('Are you Sure?\n Selected Filter will be permanently deleted.')) {
                window.location = "filtersplus.aspx?FilterID=" + theID + "&action=deleteFilter";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="upMain" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
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
            <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
                <tbody>
                    <tr>
                        <td class="tableHeader" align="right" valign="middle">
                            <%--				    <asp:RequiredFieldValidator id="val_FilterName" runat="Server" CssClass="errormsg" ControlToValidate="txtFilterName"
						ErrorMessage="Filter name is a required field." display="Static">Required --> </asp:RequiredFieldValidator>
					<asp:textbox class="formfield" id="txtFilterName" runat="Server" EnableViewState="true" columns="25"></asp:textbox>&nbsp;					
					<asp:DropDownList ID="ddlGroupCompareType" runat="server">
                        <asp:ListItem>ANY</asp:ListItem>
                        <asp:ListItem>ALL</asp:ListItem>
                    </asp:DropDownList>--%>
                    &nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr class="tableHeader" align="left">
                        <td class="label" align="left" valign="middle">List of Filters for Group: <i>
                            <asp:Label ID="GroupNameDisplay" runat="Server" Visible="true" CssClass="label" Text=""></asp:Label></i>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlArchiveFilter" runat="server" OnSelectedIndexChanged="ddlArchiveFilter_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Active" Value="active" Selected="True" />
                                <asp:ListItem Text="Archived" Value="archived"  />
                                <asp:ListItem Text="All" Value="all" />
                            </asp:DropDownList>
                        </td>
                        <td class="tableHeader" align="right" valign="middle">
                            <asp:Button class="formbuttonsmall" ID="btnAddFilter" runat="Server" Visible="true"
                                Text="Create new Filter" OnClick="btnAddFilter_Click" Width="180px"></asp:Button>
                        </td>
                        <td>
                            <asp:Button CssClass="formbuttonsmall" ID="btnCopyFilter" runat="server" Visible="true" Text="Copy Filter" OnClick="btnCopyFilter_Click" Width="180px" />&nbsp;&nbsp;<br />
                        </td>
                    </tr>
                    <tr class="tableHeader" align="left">
                        <td colspan="4">
                            <ecnCustom:ecnGridView ID="FilterGrid" runat="Server" Width="100%" CssClass="grid"
                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" ondeletecommand="FilterGrid_Command"
                                onsortcommand="FilterGrid_sortCommand" DataKeyNames="FilterID, GroupID" PageSize="15"
                                OnRowDeleting="FilterGrid_RowDeleting" OnRowDataBound="FilterGrid_RowDataBound"
                                OnPageIndexChanging="FilterGrid_PageIndexChanging"
                                OnSorting="FilterGrid_Sorting">
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <Columns>
                                    <asp:BoundField DataField="FilterName" HeaderText="Filter Name" SortExpression="FilterName"
                                        HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                                    <asp:BoundField DataField="CreateDate" HeaderText="Date Created" HeaderStyle-Width="20%"
                                        SortExpression="CreateDate" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="5%" HeaderStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <a href='filtersplusedit.aspx?GroupID=<%# DataBinder.Eval(Container.DataItem, "GroupID") %>&amp;FilterID=<%# DataBinder.Eval(Container.DataItem, "FilterID") %>'>
                                                <center>
                                                    <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Filter' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Filter' border='0'&gt;"
                                                CausesValidation="false" ID="DeleteContentBtn" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this filter?')"
                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterID") %>'></asp:LinkButton>
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
                                <PagerTemplate>
                                    <table cellpadding="0" border="0">
                                        <tr>
                                            <td align="left" class="label" width="31%">Total Records:
                                        <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                            </td>
                                            <td align="left" class="label" width="25%">Show Rows:
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FilterGrid_SelectedIndexChanged"
                                            CssClass="formfield">
                                            <asp:ListItem Value="5" />
                                            <asp:ListItem Value="10" />
                                            <asp:ListItem Value="15" />
                                            <asp:ListItem Value="20" />
                                        </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td align="right" class="label" width="100%">Page
                                        <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged"
                                            class="formtextfield" Width="30px" />
                                                of
                                        <asp:Label ID="lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                &nbsp;
                                        <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                            CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                <asp:Button ID="btnNextC" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                                                    class="formbuttonsmall" Text="Next >>" />
                                            </td>
                                        </tr>
                                    </table>
                                </PagerTemplate>
                            </ecnCustom:ecnGridView>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Button ID="hfCopyFilter" runat="server" style="display:none;" />
            <ajaxToolkit:ModalPopupExtender ID="mpeCopyFilter" runat="server" CancelControlID="btnClose" BackgroundCssClass="modalBackground" TargetControlID="hfCopyFilter" PopupControlID="upCopyFilter" />

            <asp:Panel ID="upCopyFilter" runat="server">

                <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" BehaviorID="RoundedCornersBehavior2"
                    TargetControlID="pnlPopupDimensionsRound2" Radius="6" Corners="All" />
                <asp:Panel ID="pnlPopupDimensionsRound2" runat="server" Width="490px" CssClass="modalPopup2">
                    <table width="100%" border="0" cellpadding="0" cellspacing="5" class="greySidesLtB" style="background-color: white;">
                        <tr>
                            <td class="gradientTwo addPage" colspan="2" style="border-right: none;">&nbsp;<span style="font-size: 12px; font-weight: bold"> Copy Filter </span>
                            </td>
                        </tr>
                        <asp:Panel ID="pnlCopyUDFMsg" runat="server" Visible="false">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblCopyUDFMsg" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td class="formLabel" valign="middle" align="right">Source Group &nbsp;
                            </td>
                            <td class="formLabel" valign="middle" align="left">
                                <asp:DropDownList ID="drpSourceGroup" runat="server" DataValueField="GroupID" DataTextField="GroupName"
                                    CssClass="formfield" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drpSourceGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvTransGroupName" runat="Server" ErrorMessage="<< required"
                                    Font-Names="arial" Font-Size="xx-small" ControlToValidate="drpSourceGroup" ValidationGroup="valdrp"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;" colspan="2">
                                <div>
                                    <ecnCustom:ecnGridView ID="gvFilters" runat="server" Visible="false" Width="100%" CssClass="grid" DataKeyNames="FilterID" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="FilterID" Visible="false" HeaderText="" ItemStyle-HorizontalAlign="left"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCopyFilter" runat="server" Checked="true"></asp:CheckBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FilterName" HeaderText="Filter Name" />

                                        </Columns>
                                    </ecnCustom:ecnGridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="label" align="middle" valign="middle" colspan="2">
                                <asp:Button ID="btnCopy" runat="server" class="formbuttonsmall" OnClick="btnCopy_Click"
                                    Text="Copy" ValidationGroup="valdrp" Width="90px" />
                                &nbsp; &nbsp;
                                    <asp:Button runat="server" Text="Cancel" ID="btnClose" class="formbuttonsmall" Width="90px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
