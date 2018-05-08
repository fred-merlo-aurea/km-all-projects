<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="ViewBlastLinks.aspx.cs" Inherits="ecn.communicator.main.blasts.ViewBlastLinks" %>
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
    <table style="width:100%;padding:10px;" >
        <tr>
            <td>
                <asp:RadioButtonList ID="rblBlastLinks" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblBlastLinks_SelectedIndexChanged" >
                    <asp:ListItem Text="Get Blast Links" Value="get" Selected="True" />
                    <asp:ListItem Text="Lookup Blast Links" Value="lookup" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlGetBlastLinks" runat="server">
                    <table style="padding:10px; width:60%;">
                        <tr>
                            <td style="width:10%;">
                                <asp:Label ID="lblGetBlastID" Text="Blast ID:" runat="server" />
                            </td>
                            <td style="text-align:left;width:90%;">
                                <asp:TextBox ID="txtGetBlastID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top:10px;">
                               <asp:GridView ID="gvGetBlastLinks" AutoGenerateColumns="false" Visible="false" runat="server">
                                   <Columns>
                                       <asp:BoundField DataField="LinkURL" HeaderText="Link URL" />
                                   </Columns>
                               </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlLookupBlastLinks" runat="server" Visible="false">
                    <table style="padding:10px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblLookupBlastLinks" runat="server" Text="Link from Email:" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLookupBlastLink" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlLookupResults" Visible="false" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastID" runat="server" Text="Blast ID:" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastIDResult" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEmailID" runat="server" Text="Email ID:" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmailIDResult" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLinkID" runat="server" Text="Blast Link ID:" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLinkIDResult" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUniqueLinkID" runat="server" Text="Unique Link ID:" />

                                            </td>
                                            <td>
                                                <asp:Label ID="lblUniqueLinkIDResult" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOLink" runat="server" Text="Original Link:" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOLinkResult" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblELink" runat="server" Text="Email Link:" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblELinkResult" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
