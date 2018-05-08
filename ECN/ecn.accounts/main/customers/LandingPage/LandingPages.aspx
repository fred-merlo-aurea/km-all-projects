<%--<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.LandingPages" ValidateRequest="false"
    CodeBehind="LandingPages.aspx.cs" MasterPageFile="../../../MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register src="Unsubscribe.ascx" tagname="Unsubscribe" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'
        align="left">
        <tbody>
            <tr>
                <td colspan="2">
                    <br />
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
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>            
            <tr>
                <td width="30%">&nbsp</td>
                <td width="70%">&nbsp</td>
            </tr>
            <tr>
                <td class="tableHeader" style="padding-top: 10px;" align="center" colspan="2">
                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border='0' align="left">
                        <tr>
                            <td class="tableHeader" valign="top" align='right' width="50%">
                                Base Channel: 
                            </td>
                            <td align="left" width="50%">
                                <asp:DropDownList ID="ddlBaseChannel" runat="server" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" DataTextField="BaseChannelName" DataValueField="BaseChannelID"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="top" align='right'>
                                Customer: 
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCustomer" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" DataTextField="CustomerName" DataValueField="CustomerID"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="top" align='right'>
                                Page: 
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPage" runat="server" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged" DataTextField="Name" DataValueField="LPID"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td class="tableHeader" style="padding-top: 10px;" align="center" colspan="2">
                    <uc1:Unsubscribe ID="Unsubscribe1" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
--%>
