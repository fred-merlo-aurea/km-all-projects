<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addGroup.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.addGroup" %>
<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<br />
<table>
    <tr>
        <td class="tablecontent" valign="top" align="left" height="24" colspan='3'>
            <b>Create Group</b>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='right'>
            <span class="label">Name&nbsp;</span>
        </td>
        <td colspan="2" align="left">
            <asp:TextBox ID="GroupName" CssClass="formfield" EnableViewState="true" runat="Server"
                Columns="40" MaxLength="50" class="formfield" Width="272"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" align='right' valign="top">
            <span class="label">Description&nbsp;</span>
        </td>
        <td colspan="2" align="left">
            <asp:TextBox ID="GroupDescription" runat="Server" Wrap="true" Columns="50" Rows="3"
                TextMode="multiline" Enabled="true" class="formfield" Width="272" Height="60"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" align='right'>
            &nbsp;<span class="label">Folder&nbsp;</span>
        </td>
        <td colspan="2" align="left">
            <asp:DropDownList EnableViewState="true" CssClass="formfield" Style="border-right: #999999 1px solid;
                border-top: #999999 1px solid; border-left: #999999 1px solid; border-bottom: #999999 1px solid"
                ID="drpFolder" runat="Server" DataValueField="FolderID" DataTextField="FolderName"
                Width="272">
            </asp:DropDownList>
        </td>
    </tr>
    <asp:Panel ID="SeedListPanel" runat="Server" Visible="false">
        <tr>
            <td class="tableHeader" align='right'>
                &nbsp;<span class="label">Is this Seed List&nbsp;</span>
            </td>
            <td colspan="2" align="left">
                <asp:RadioButtonList ID="rbSeedList" class="formLabel" runat="Server" RepeatDirection="horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                    <asp:ListItem Value="True">Yes</asp:ListItem>
                </asp:RadioButtonList>
             
            </td>
        </tr>
        <tr>
        <td colspan="3" ><font class="formLabel" style="color: Gray">(ECN will automatically send
                    a copy of the blasts to all the emails in the Seed list group. )</font>
        </td>
        </tr>
    </asp:Panel>
    <tr>
        <td align='right' class="tableHeader">
            <asp:Label ID="lblIsPublic" runat="Server" class="tableHeader" Visible="false" Width="64px"> Is Public</asp:Label>
            &nbsp;
        </td>
        <td>
            <asp:CheckBox ID="PublicFolder" runat="Server" Visible="false"></asp:CheckBox>
        </td>
    </tr>
</table>
