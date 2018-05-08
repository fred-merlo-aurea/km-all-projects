<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Site.Master" AutoEventWireup="true" CodeBehind="License.aspx.cs" Inherits="KMPS.MD.Administration.License" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>
<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
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
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">License</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="100%" BorderWidth="1">
                <table cellspacing="5" cellpadding="10" border="0" align="left" width="80%">
                    <tr>
                        <td align="left">Sales View License Count:&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtLicenseCount" runat="server" Text="0"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLicenseCount"
                                ErrorMessage="* Enter License Count" ValidationGroup="save"  ForeColor="Red" Font-Bold="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="ValLicenseCount" runat="server" ErrorMessage="* Enter valid Number" ValidationGroup="save" 
                                 ControlToValidate="txtLicenseCount" ValidationExpression="^[0-9]*$" Font-Bold="True" ForeColor="Red"></asp:RegularExpressionValidator>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr><td colspan="2" align="center"><asp:Label ID="lblMessage" runat="Server" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label></td></tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:HiddenField ID="hfConfigID" Value="0" runat="server" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"
                                ValidationGroup="save" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
