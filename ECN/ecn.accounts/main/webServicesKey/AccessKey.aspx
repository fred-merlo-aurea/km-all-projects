<%@ Page Language="c#" Inherits="ecn.accounts.webServicesKey.AcessKey" CodeBehind="AccessKey.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script language="javascript" type="text/javascript">
        function deleteUser(theID) {
            if (confirm('Are you Sure?\nSelected User will be permanently deleted.')) {
                window.location = "userdetail.aspx?UserID=" + theID + "&action=delete";
            }
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<table border='0' cellpadding="5" cellspacing="0">
        <tr>
            <td>
                <asp:button class="formbutton" id="AccessKeyButton" runat="Server" text="Get Access Key"
                    onclick="AccessKeyButton_Click"></asp:button>
            </td>
        </tr>
        <asp:panel id="AccessKeyPanel" runat="Server" visible="false" cssclass="smallBold">
			<tr>
				<td valign="bottom" height="30" class=tableContent>
					<span class=errormsg >ecn.WebServices Access Key:</span>&nbsp;&nbsp;&nbsp;&nbsp;
					<span style="FONT-FAMILY: Verdana; FONT-SIZE:13px"><asp:label id="msglabel" runat="Server"></asp:label></span><br />
					<span style="COLOR: red"><b>NOTE:</b><br />
					Please save this KEY in a safe place.<br />
					Use this key on every method calls to ECN thru Web Services.<br /><br />
					Illegal use of this KEY would gain access to your ECN account through Web Servces.</span></td>
			</tr>
		</asp:panel>
    </table>
     <asp:HiddenField runat="server" ID="hdHelpTitle" Value="" />
     <asp:HiddenField runat="server" ID="hdHelpContent" Value="<img align='right' src=/ecn.images/images/icousers.gif>Access key Help" />" />    
</asp:content>
