<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DownloadTemplateSetup.aspx.cs" Inherits="KMPS.MD.Tools.DownloadTemplateSetup" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>
<%@ Register TagName="DownloadCase" TagPrefix="dc" Src="~/Controls/DownloadCase.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="overlay">
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10002; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DownloadEditCase" EventName="CausePostBack" />
    </Triggers>
        <ContentTemplate>
            <center>
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                        <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                            <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 40px;">
                                <td valign="middle">
                                    <b>Brand
                                    <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                                    <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                        DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <div style="border: solid 1px #5783BD">
                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgDownloadTemplate" runat="server" Visible="true" OnNeedDataSource="rgDownloadTemplate_NeedDataSource" AllowPaging="True" AllowSorting="false">
                                        <MasterTableView DataKeyNames="DownloadTemplateID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="" AllowAutomaticUpdates="false" PageSize="25" PagerStyle-Mode="NextPrevAndNumeric" PagerStyle-AlwaysVisible="true">
                                            <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                            <Columns>
                                                <telerik:GridBoundColumn HeaderText="Template Name" DataField="DownloadTemplateName" UniqueName="DownloadTemplateName" SortExpression="DownloadTemplateName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                <telerik:GridTemplateColumn HeaderText="View Type" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" SortExpression="PubID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblViewType" runat="server" Text='<%# Convert.ToInt32(Eval("PubID")) > 0 ? "Product View" : "Consensus/Recency View" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderText="Product" DataField="PubName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" SortExpression="PubName" />
                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditDownloadTemplate"
                                                            CommandArgument='<%# Eval("DownloadTemplateID") %>' OnCommand="lnk_Command"
                                                            Font-Bold="true"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                                            CommandArgument='<%# Eval("DownloadTemplateID") %>' OnCommand="lnk_Command"
                                                            Font-Bold="true"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <ecn:BoxPanel ID="bpAddTemplates" runat="server" Title="Template Setup" Layout="table"
                        HorizontalAlign="Left" Width="100%">
                        <div id="div1" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="100%" align="center">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" align="center" width="5%">
                                                    <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="95%" height="100%">
                                                    <asp:Label ID="lblTemplateError" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table width="100%" border="0" cellpadding="5" cellspacing="5" height="100%">
                            <tr>
                                <td width="8%" align="right"><b>Template Name</b></td>
                                <td align="left" width="92%">
                                    <asp:TextBox ID="txtDownloadTemplateName" runat="server" MaxLength="100" Width="250"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqFldTxtName" runat="server" ControlToValidate="txtDownloadTemplateName"
                                        ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfDownloadTemplateID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfProductID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfCreatedUserID" runat="server" />
                                    <asp:HiddenField ID="hfCreatedDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblViewType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblViewType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="ConsensusView" Text="Consensus/Recency View" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="ProductView" Text="Product View"></asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <asp:Panel ID="pnlProduct" runat="server" Visible="false">
                                <tr>
                                    <td align="right"><b>Product</b></td>
                                    <td align="left">
                                        <asp:DropDownList ID="drpProduct" runat="server" Font-Names="Arial" Font-Size="x-small"
                                            AutoPostBack="true" Style="text-transform: uppercase" DataTextField="PubName"
                                            DataValueField="PubID" Width="150px" OnSelectedIndexChanged="drpProduct_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="2">
                                    <table width="50%" border="0" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td align="center" width="15%"><b>Available Fields</b></td>
                                            <td width="5%"></td>
                                            <td align="center" width="15%">
                                                    <b>Selected Fields</b> &nbsp;&nbsp;<asp:Button ID="btnEditCase" runat="server" CssClass="button" OnClick="btnEditCase_Click" Text="Edit Case" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSourceFields" runat="server" Rows="10"
                                                    SelectionMode="Multiple" Width="350px" Height="300px"
                                                    EnableViewState="True"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                                <br>
                                                <br>
                                                <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click"
                                                    Text="<<" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lstDestFields" runat="server" Rows="10"
                                                    SelectionMode="Multiple" Width="350px" Height="300px"
                                                    DataTextField="DisplayName"
                                                    DataValueField="ColumnValue"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" />
                                                <br>
                                                <br>
                                                <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                                    Text="Move Down" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" ValidationGroup="Save" UseSubmitBehavior="False" />
                                                &nbsp;&nbsp; 
                                                <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel" ValidationGroup="Cancel" UseSubmitBehavior="False" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ecn:BoxPanel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>
            <dc:DownloadCase runat="server" ID="DownloadEditCase" Visible="false"></dc:DownloadCase>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
