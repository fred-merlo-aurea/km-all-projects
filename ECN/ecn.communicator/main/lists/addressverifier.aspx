<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.addressverifier.addressloader"
    CodeBehind="addressverifier.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="../../includes/uploader.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                                    <img style="padding: 0 0 0 15px;"
                                                        src="/ecn.images/images/errorEx.jpg"></td>
                                                <td valign="middle" align="left" width="80%" height="100%">
                                                    <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="errorBottom"></td>
                                </tr>
                            </table>
                            <br />
                        </asp:PlaceHolder>
    <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
    <br />
    <table id="layoutWrapper" cellspacing="1" cellpadding="2" width="100%" border='0'>
        <tbody>
            <tr>
                <!--added width attribute - anthony 9-180-06 1133-->
                <td class="tableHeader" valign="top" align='right' width="122px">
                    <span class="label">&nbsp;Group&nbsp;</span>
                </td>
                <td valign="top" align="left">
                    <div>
                        <asp:RadioButton ID="rbGroupChoice1" runat="server" Text="" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged" Checked="True"/>
                        <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
                        <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>
                        <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                        <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                        <!--<asp:DropDownList Style="width: 133px" ID="GroupID" EnableViewState="true" runat="Server"
                            DataTextField="GroupName" DataValueField="GroupID" class="formfield">
                        </asp:DropDownList>-->
                    </div>
                    <div style="padding-bottom: 8px">
                            <asp:RadioButton ID="rbGroupChoice2" runat="server" Text="Master Suppression Group" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    <span class="label">&nbsp;Validation Type&nbsp;</span>
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList Style="width: 133px" ID="ValidationLevel" EnableViewState="true"
                        runat="Server" class="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    <span class="label">&nbsp;Results&nbsp;</span>
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="ShowResponse" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <!--left aligned cell and added padding-left -anthony 9-18-06 1133 -->
                <td>
                </td>
                <td class="tableHeader" colspan="2" align="left" valign="bottom" height="40">
                    <asp:Button ID="VerifyButton" OnClick="VerifyEmails" runat="Server" Width="133px"
                        Text="Check Format" class="formbutton" Visible="true" />
                </td>
            </tr>
        </tbody>
    </table>
    <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
</asp:Content>
