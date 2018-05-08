<%@ Page language="c#" Codebehind="referralProgram.aspx.cs" AutoEventWireup="True" Inherits="ecn.communicator.engines.referralProgram" MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellspacing="0" cellpadding="5" border='0'>
		<tr>
			<td>
				<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                    <table cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td id="errorTop"></td>
                        </tr>
                        <tr>
                            <td id="errorMiddle">
                                <table height="67" width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img style="padding: 0 0 0 15px;" src="http://images.ecn5.com/images/errorEx.jpg"/>
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
            </td>
		</tr>
	</table>     
</asp:Content>