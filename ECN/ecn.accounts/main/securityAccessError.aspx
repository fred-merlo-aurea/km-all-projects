<%@ Page Language="c#" AutoEventWireup="true"  Inherits="ecn.accounts.securityAccessError" CodeBehind="securityAccessError.aspx.cs"
    Title="error" MasterPageFile="~/MasterPages/Accounts.Master"%>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">      
    <br />
    <table id="Table1" style="width: 800px; height: 96px" cellspacing="1" cellpadding="1"
        width="800" border="1">
        <tr>
            <td style="height: 23px" class="errormsg">
                <asp:Label ID="lblMsgTitle" runat="Server" Font-Bold="True" Font-Names="Verdana"
                    Font-Size="13px" ForeColor="Black">
					    <br />&nbsp;	SECURITY VIOLATION Occured !!
					    <br /><br />
					    &nbsp;You are in this page because you are not Authorized to view this information.<br />&nbsp;Please contact your User Administrator to resolve the problem.<br /><br /></asp:Label></td>
        </tr>
    </table>
    <br />
</asp:content>
