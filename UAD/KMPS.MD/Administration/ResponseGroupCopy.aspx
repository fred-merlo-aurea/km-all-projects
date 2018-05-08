<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="ResponseGroupCopy.aspx.cs" Inherits="KMPS.MD.Administration.ResponseGroupCopy" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function ConfirmSubmit() {

            Page_ClientValidate();

            if (Page_IsValid) {

                return confirm('This action will delete all the codesheet and codesheet reference for the selected pubs. Are you sure want to continue ?');

            }

            return Page_IsValid;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="text-align: right">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                        Processing....
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
                                            alt="" />
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
                <br />
            </div>
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Response Group Copy</asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="380px" BorderWidth="1">
                <table cellspacing="5" cellpadding="5" border="0" width="100%">
                    <tr>
                        <td width="35%">
                            <b>From</b>
                        </td>
                        <td colspan="2">
                            <b>To</b>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="35%">
                            <table cellspacing="0" cellpadding="5" border="0">
                                <tr>
                                    <td>
                                        Product :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpPubsFrom" runat="server" AutoPostBack="True" DataTextField="PubName"
                                            DataValueField="PubID" OnSelectedIndexChanged="drpPubsFrom_SelectedIndexChanged" CausesValidation="false"
                                            Width="300px" onchange="Page_BlockSubmit = false;">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqPubsFrom" runat="server" ControlToValidate="drpPubsFrom" InitialValue="0"
                                            ErrorMessage="* required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ResponseGroup :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpResponseGroupFrom" runat="server" DataTextField="DisplayName"
                                            DataValueField="ResponseGroupID" Width="300px" onchange="Page_BlockSubmit = false;">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqResponseGroupFrom" runat="server" ControlToValidate="drpResponseGroupFrom"  InitialValue="0"
                                            ErrorMessage="* required"  Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top"  width="5%"  align="right">
                            Product :
                        </td>
                        <td>
                            <asp:ListBox ID="lstPubs" runat="server" Rows="15" Style="text-transform: uppercase"
                                SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial" Width="500px"
                                DataTextField="PubName" DataValueField="PubID" onchange="Page_BlockSubmit = false;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqlstPubs" runat="server" ControlToValidate="lstPubs"
                                            ErrorMessage="* required" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnCopy" runat="server" Text="COPY" OnClick="btnCopy_Click" CssClass="button"
                                 OnClientClick="javascript:return ConfirmSubmit()"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" colspan="3">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
