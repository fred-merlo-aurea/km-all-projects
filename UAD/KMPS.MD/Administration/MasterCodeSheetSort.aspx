﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Site.Master" AutoEventWireup="true"
    CodeBehind="MasterCodeSheetSort.aspx.cs" Inherits="KMPS.MD.Administration.MasterCodeSheetSort" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                        <asp:Label ID="lblpnlHeader" runat="Server">Code Sheet Sort Order</asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="500px" BorderWidth="1">
                <table cellspacing="5" cellpadding="5" border="0">
                    <tr>
                        <td valign="top" align="right">
                            Master Groups :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpMasterGroups" runat="server" AutoPostBack="True" DataTextField="DisplayName"
                                DataValueField="MasterGroupID" Width="300px" OnSelectedIndexChanged="drpMasterGroups_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Master Code Sheet :</td>
                        <td colspan="2"><asp:ImageButton ID="ibSort" runat="server" ImageUrl="~/images/ic-sort.png" OnClick="ibSort_Click" /></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                        </td>
                        <td>
                            <asp:ListBox ID="lstSourceFields" runat="server" Rows="25" Style="text-transform: uppercase;"
                                SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="300px"
                                DataTextField="MasterDesc" DataValueField="MasterID"></asp:ListBox>
                        </td>
                        <td>
                            <td>
                                <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" />
                                <br>
                                <br>
                                <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                    Text="Move Down" />
                            </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
