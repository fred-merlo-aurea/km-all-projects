<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ReportSetup.aspx.cs" Inherits="KMPS.MD.Tools.ReportSetup" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000009; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="80%" align="center">
                        <tr>
                            <td id="errorTop"></td>
                        </tr>
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="../Images/errorEx.jpg" runat="server"
                                                alt="" />
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
                    <br />
                </div>
                <br />
                <div style="width: 90%; text-align: left;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                                    <b>Brand
                                    <asp:DropDownList ID="ddlBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                        DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgCrossTabReport" runat="server" Visible="true" OnNeedDataSource="rgCrossTabReport_NeedDataSource" AllowPaging="True" AllowSorting="True" OnRowDataBound="rgCrossTabReport_RowDataBound">
                        <MasterTableView DataKeyNames="CrossTabReportID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="" AllowAutomaticUpdates="false" PageSize="25" PagerStyle-Mode="NextPrevAndNumeric" PagerStyle-AlwaysVisible="true">
                            <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Report Name" DataField="CrossTabReportName" SortExpression="CrossTabReportName" UniqueName="CrossTabReportName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                <telerik:GridTemplateColumn HeaderText="View Type" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" SortExpression="View_Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewType" runat="server" Text='<%#  Eval("View_Type").ToString() == "ConsensusView" ?  " Consensus/Recency View" : Eval("View_Type").ToString().Replace("View", " View")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="Product" DataField="PubName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" SortExpression="PubName" />
                                <telerik:GridBoundColumn HeaderText="Row" DataField="RowDisplayName" UniqueName="RowDisplayName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" SortExpression="RowDisplayName" />
                                <telerik:GridBoundColumn HeaderText="Column" DataField="ColumnDisplayName" UniqueName="ColumnDisplayName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" SortExpression="ColumnDisplayName" />
                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditCrossTab"
                                            CommandArgument='<%# Eval("CrossTabReportID") %>' OnCommand="lnk_Command"
                                            Font-Bold="true"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                            CommandArgument='<%# Eval("CrossTabReportID") %>' OnCommand="lnk_Command"
                                            Font-Bold="true"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server">Add CrossTab Report</asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="100%" BorderWidth="1">
                        <table cellspacing="5" cellpadding="5" border="0">
                            <tr>
                                <td align="right">Report Name :
                                </td>
                                <td>
                                    <asp:HiddenField ID="hfCrossTabReportID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfPubID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfCreatedUserID" runat="server" Value="" />
                                    <asp:HiddenField ID="hfCreatedDate" runat="server" Value="" />
                                    <asp:TextBox ID="txtCrossTabReportName" runat="server" Width="148px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqfldName" runat="server" ControlToValidate="txtCrossTabReportName"
                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Filter Type :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlViewType" runat="server" OnSelectedIndexChanged="ddlViewType_SelectedIndexChanged" AutoPostBack="true" Width="200px">
                                        <asp:ListItem Text="Consensus/Recency View" Value="ConsensusView"></asp:ListItem>
                                        <asp:ListItem Text="ProductView" Value="ProductView"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <asp:PlaceHolder ID="phProduct" runat="server" Visible="false">
                                <tr>
                                    <td align="right">Product :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server" DataTextField="PubName" DataValueField="PubID" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td align="right">Row :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRowDimension" runat="server" Width="200px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRowDimension"
                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Column :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlColumnDimension" runat="server" Width="200px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlRowDimension"
                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                                    <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                        CssClass="button" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblMessage" Visible="false" runat="Server"></asp:Label></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
