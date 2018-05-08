<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Site.Master" AutoEventWireup="true" CodeBehind="ActivityHistory.aspx.cs" Inherits="KMPS.MD.Administration.ActivityHistory" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="Images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="600" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
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
            <table cellspacing="5" cellpadding="10" border="0">
                <tr>
                <td align="left" width="8%">
                    From Date :<br />
                    <asp:TextBox ID="txtSDate" runat="server" Width="75"></asp:TextBox><ajaxToolkit:CalendarExtender
                        ID="CalendarExtender1" runat="server" CssClass="MyCalendar" TargetControlID="txtSDate"
                        Format="MM/dd/yyyy">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td align="left" width="8%">
                    To Date :<br />
                    <asp:TextBox ID="txtEDate" runat="server" Width="75"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="MyCalendar"
                        TargetControlID="txtEDate" Format="MM/dd/yyyy">
                    </ajaxToolkit:CalendarExtender>
                </td>
                    <td align="left" valign="bottom"  width="20%">User Name :<br />
                        <asp:DropDownList ID="drpUsers" runat="server" CssClass="formfield" DataTextField="UserName"
                            DataValueField="UserID" Width="200px">
                        </asp:DropDownList>
                    </td>
                <td align="left" valign="bottom" width="61%">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />
                </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvActivityHistory" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gvActivityHistory_Sorting"
                EnableModelValidation="True" DataKeyNames="UserTrackingID" OnPageIndexChanging="gvActivityHistory_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName">
                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity">
                        <HeaderStyle HorizontalAlign="left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ActivityDateTime" HeaderText="DateTime" SortExpression="ActivityDateTime">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="IPAddress" HeaderText="IPAddress" SortExpression="IPAddress" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Platform" HeaderText="Platform" SortExpression="Platform" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Client" HeaderText="Browser/Device" SortExpression="Client">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
