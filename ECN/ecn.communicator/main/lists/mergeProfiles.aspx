<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mergeProfiles.aspx.cs" Inherits="ecn.communicator.main.lists.mergeProfiles" MasterPageFile="~/MasterPages/Communicator.Master"%>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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
    <table width="100%">
        <tr valign="top">
             <td align="center">
                 <asp:DetailsView ID="dvEmailAddressOld" runat="server" Height="50px" CssClass="grid" width="80%"></asp:DetailsView>
            </td>
            <td align="center">    
                  <asp:DetailsView ID="dvEmailAddressNew" runat="server" Height="50px" CssClass="grid" width="80%"></asp:DetailsView>
            </td>
        </tr>
        <tr valign="top" align="center">
            <td align="center">
                <asp:Button ID="btnprofileOld" runat="server" Text="Make this primary" OnClick="btnprofileOld_Click" />
            </td>
            <td align="center">
                <asp:Button ID="btnprofileNew" runat="server" Text="Make this primary" OnClick="btnprofileNew_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>
