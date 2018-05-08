<%@ Page Language="c#" Inherits="ecn.accounts.exceptionManager.exception" CodeBehind="exception.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border='0' cellspacing="0" cellpadding="0">
        <tr>
            <td class="tableContent" valign="top" align="center">
                <font color="#FF0000" face="verdana" size="3"><b><u>ACCESS VIOLATION</u></b><br />
                    <br />
                    The page you are trying to access DOES NOT EXIST !!</font>
            </td>
        </tr>
    </table>
</asp:Content>
