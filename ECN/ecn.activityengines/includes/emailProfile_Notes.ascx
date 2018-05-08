<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="emailProfile_Notes.ascx.cs" Inherits="ecn.activityengines.includes.emailProfile_Notes" %>
<br />
<table cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td align="left" style="font-weight: bold; color: red"><asp:Label ID="messageLabel" runat="server" CssClass="errormsg" Font-Bold="True" Visible="False"></asp:Label></td>
    </tr>
    <tr>
        <td Width=100%>
            <asp:TextBox Width="99%" Rows="15" ID="profileNotes" runat="server" TextMode="MultiLine" style="FONT-FAMILY:Arial,Verdana; FONT-SIZE:10px;"></asp:TextBox>
        </td>
    </tr>
   </table> 