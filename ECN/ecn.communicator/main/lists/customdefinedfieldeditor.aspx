<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.customdefinedfieldeditor"
    CodeBehind="customdefinedfieldeditor.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
    </asp:PlaceHolder>
    <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" valign="middle" align='right' width="10%">Short Name
                </td>
                <td>
                    <asp:TextBox ID="short_name" runat="Server" Width="136px" class="formfield"></asp:TextBox>
                    &nbsp;<font size="-2" face='verdana' color="#000000">(unique name)<asp:RequiredFieldValidator
                        ID="rfvshortname" runat="Server" ErrorMessage="<< required" Font-Names="arial"
                        Font-Size="xx-small" ControlToValidate="short_name" ValidationGroup="newudfgroup"></asp:RequiredFieldValidator></font>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="middle" align='right' height="41">Long Description
                </td>
                <td height="41" valign="middle">
                    <asp:TextBox ID="long_name" runat="Server" Width="400px" Height="30px" class="formfield"
                        TextMode="multiline"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="middle" align='right' width="10%" nowrap>Is Public?
                </td>
                <td>
                    <asp:CheckBox ID="isPublicChkbox" runat="Server" Enabled="false"></asp:CheckBox>
                    &nbsp;<font size="-2" face='verdana' color="#000000">(Will accept only Yes / No as the
                        data value for this field.)</font>
                </td>
            </tr>
            <asp:Panel ID="pnlWholeDefaultValue" Visible="false" runat="server">
                <tr>
                    <td class="tableHeader" valign="middle" align="right" width="10%">Use Default Value?
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="chkUseDefaultValue" runat="server" AutoPostBack="true" OnCheckedChanged="chkUseDefaultValue_CheckedChanged" />
                    </td>
                </tr>
                <asp:Panel ID="pnlDefaultValue" runat="server" Visible="false">
                    <tr>
                        <td width="10%"></td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlDefaultType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDefaultType_SelectedIndexChanged">
                                            <asp:ListItem Text="Default Value" Value="default" />
                                            <asp:ListItem Text="System Value" Value="system" />
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDefaultValue" runat="server" Visible="false" />
                                        <asp:DropDownList ID="ddlSystemValues" runat="server" Visible="false">
                                            <asp:ListItem Text="Current Date" Value="CurrentDate" Selected="true" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>

                </asp:Panel>
            </asp:Panel>
            <tr>
                <td class="tableHeader" valign="top" align="center" colspan="2">
                    <asp:Button class="formbutton" ID="update_button" OnClick="update_button_Click" runat="Server" Text="Update Field"
                        ValidationGroup="newudfgroup"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
