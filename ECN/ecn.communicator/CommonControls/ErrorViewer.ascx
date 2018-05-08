<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ErrorViewer.ascx.cs" Inherits="ecn.communicator.CommonControls.ErrorViewer" %>

<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr><td id="errorTop"></td></tr>
        <tr>
            <td id="errorMiddle">
                <table height="67" width="80%">
                    <tr>
                        <td valign="top" align="center" width="20%">
                            <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                        </td>
                        <td valign="middle" align="left" width="80%" height="100%">
                            <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                            <asp:BulletedList ID="BulletRunTime" runat="Server" BulletStyle="Square"></asp:BulletedList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td id="errorBottom"></td></tr>
    </table>
</asp:PlaceHolder>