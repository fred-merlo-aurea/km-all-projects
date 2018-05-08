<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Site.Master" AutoEventWireup="true" CodeBehind="ReportGroups.aspx.cs" Inherits="KMPS.MD.Administration.ReportGroups" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="90%" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
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
            </div>
            Product :
            <asp:DropDownList ID="drpPubs" runat="server" AutoPostBack="true"
                DataTextField="PubName" DataValueField="PubID"
                OnSelectedIndexChanged="drpPubs_SelectedIndexChanged" Width="300px">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <br />
            <br />
            Response Group :
            <asp:DropDownList ID="drpResponseGroup" runat="server" AutoPostBack="true"
                DataTextField="DisplayName" DataValueField="ResponseGroupID"
                OnSelectedIndexChanged="drpResponseGroup_SelectedIndexChanged" Width="300px">
            </asp:DropDownList>
            <br />
            <br />
            <asp:GridView ID="gvReportGroup" runat="server" AutoGenerateColumns="False" DataKeyNames="ReportGroupID"
                EnableModelValidation="True" AllowSorting="True" OnSorting="gvReportGroup_Sorting"
                AllowPaging="True" OnPageIndexChanging="gvReportGroup_PageIndexChanging" OnRowCommand="gvReportGroup_RowCommand" EmptyDataText="No Records">
                <Columns>
                    <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="ResponseGroupName"
                        ItemStyle-HorizontalAlign="left">
                        <HeaderStyle HorizontalAlign="Left" Width="55%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" SortExpression="DisplayOrder"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="50%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:ButtonField HeaderStyle-Width="5%" ItemStyle-Width="5%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
            <br />

            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Report Group</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="100%" BorderWidth="1">
                <table cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td align="right">
                            DisplayName :
                        </td>
                        <td>
                            <asp:HiddenField ID="hfReportGroupID" runat="server" Value="0" />
                            <asp:HiddenField ID="hfSortOrder" runat="server" Value="0" />
                            <asp:TextBox ID="txtDisplayName" runat="server" Width="148px" ValidationGroup="save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvDisplayName" runat="server" ControlToValidate="txtDisplayName"
                                ErrorMessage="*" ValidationGroup="save"  ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click" CssClass="button" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
