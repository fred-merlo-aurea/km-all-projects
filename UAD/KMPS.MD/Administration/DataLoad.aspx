<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="DataLoad.aspx.cs" Inherits="KMPS.MD.Administration.DataLoad" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" AsyncPostBackTimeout="3600" EnablePartialRendering="true"></asp:ScriptManager>
    <asp:Timer ID="timer" runat="server" OnTick="timer_Tick" Enabled="false" Interval="2000"></asp:Timer>
    <div id="divError" runat="Server" visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
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
                <td id="errorBottom"></td>
            </tr>
        </table>
        <br />
    </div>
    <table cellpadding="5" cellspacing="5" border="1px" width="100%" style="display: none;">
        <tr>
            <td align="right" valign="top">Latest Refresh Status 
            </td>
            <td align="left" valign="top">
                <asp:Label ID="lblStatus" runat="Server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">Start Time: 
            </td>
            <td align="left" valign="top">
                <asp:Label ID="lblStartTime" runat="Server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">End Time: 
            </td>
            <td align="left" valign="top">
                <asp:Label ID="lblEndTime" runat="Server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">Message/Status: 
            </td>
            <td align="left" valign="top">
                <asp:Label ID="lblMessage" runat="Server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <table cellpadding="25" cellspacing="25">
        <tr>
            <td>
                <asp:Panel ID="pnlPrd" runat="server" DefaultButton="btnPrd">
                    <table cellpadding="3px" cellspacing="3px">
                        <tr>
                            <th colspan="2">Prod to Refresh</th>
                        </tr>
                        <tr>
                            <td>Select Database:</td>
                            <td>
                                <asp:DropDownList ID="ddlPrd" runat="server" AutoPostBack="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnPrd" runat="server" Text="Start" CssClass="button" CausesValidation="false" OnClick="btnPrd_Click" />
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="5px" cellspacing="5px">
                        <tr>
                            <th colspan="4">
                                <p style="text-align: left;">
                                    This is a 4 step process.
                                    <br />
                                    Progress for each step will be displayed.
                                    <br />
                                    Steps 2, 3 can't start until step 1 is done.
                                    <br />
                                    Steps 2, 3 can run simultaneously.
                                    <br />
                                    Step 4 can't start until 1, 2, 3 are done.
                                </p>
                            </th>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Literal ID="litPrd1" runat="server" Text="Step 1 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litPrd2" runat="server" Text="Step 2 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litPrd3" runat="server" Text="Step 3 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litPrd4" runat="server" Text="Step 4 not started"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlRefresh" runat="server">
                    <table cellpadding="3px" cellspacing="3px">
                        <tr>
                            <th colspan="2">Refresh to Prod</th>
                        </tr>
                        <tr>
                            <td>Select Database:</td>
                            <td>
                                <asp:DropDownList ID="ddlRefresh" runat="server" AutoPostBack="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnRefresh" runat="server" Text="Start" CssClass="button" CausesValidation="false" OnClick="btnRefresh_Click" />
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="5px" cellspacing="5px">
                        <tr>
                            <th colspan="4">
                                <p style="text-align: left;">
                                    This is a 4 step process.
                                    <br />
                                    Progress for each step will be displayed.
                                    <br />
                                    Steps 2, 3 can't start until step 1 is done.
                                    <br />
                                    Steps 2, 3 can run simultaneously.
                                    <br />
                                    Step 4 can't start until 1, 2, 3 are done.
                                </p>
                            </th>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Literal ID="litRef1" runat="server" Text="Step 1 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litRef2" runat="server" Text="Step 2 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litRef3" runat="server" Text="Step 3 not started"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:Literal ID="litRef4" runat="server" Text="Step 4 not started"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>


</asp:Content>
