<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="BlastStatus.aspx.cs" Inherits="ecn.accounts.main.reports.BlastStatus" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        div.New .rpRootGroup {
            border-width: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
            <br />
            <table cellpadding="0" cellspacing="0" width="100%" border='0'>
                <tr>
                    <td width="100%" style="margin-left: 960px">
                        <br />
                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" Style="text-align: left" AutoPostBack="true">
                            <ajaxToolkit:TabPanel ID="TabBlastSearch" runat="server" TabIndex="0" HeaderText="Blast Search">
                                <headertemplate>
                                    <span><b>Blast Search</b></span>
                                </headertemplate>
                                <contenttemplate>
                                    <br />
                                    <table cellpadding="5" cellspacing="5" width="100%" border='0'>
                                        <tr>
                                            <td align='right' class="formLabel"><b>BlastID :</b>&nbsp;
                                                <asp:TextBox ID="txtBlastID" runat="server" ValidationGroup="BlastSearch" class="formfield"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="rfvtxtBlastID"
                                                    runat="server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> Only numbers allowed"
                                                    Font-Size="XX-Small" ControlToValidate="txtBlastID" ValidationExpression="[0-9]*"
                                                    Font-Overline="False" ValidationGroup="BlastSearch" Display="Dynamic"></asp:RegularExpressionValidator>
                                                &nbsp;
                                                <asp:Button ID="btnSubmit" runat="Server" Text="Show Report" CssClass="formfield"
                                                    OnClick="btnSubmit_Click" ValidationGroup="BlastSearch"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hfBlastID" runat="server" />
                                                <asp:HiddenField ID="hfBlastEngineID" runat="server" />
                                                <asp:HiddenField ID="hfBlastFinishTime" runat="server" />
                                                <asp:HiddenField ID="hfCustomerID" runat="server" />
                                                <asp:HiddenField ID="hfGroupID" runat="server" />
                                                <telerik:RadGrid AutoGenerateColumns="False" ID="rgBlastSearch" runat="server" Width="920px" Visible="false">
                                                    <ClientSettings>
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" ScrollHeight="100px" />
                                                    </ClientSettings>
                                                    <MasterTableView DataKeyNames="BlastID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="">
                                                        <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                        <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderText="BlastEngineID" DataField="BlastEngineID" UniqueName="BlastEngineID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="BlastID" DataField="BlastID" UniqueName="BlastID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="CampaignItemName" DataField="CampaignItemName" UniqueName="CampaignItemName" />
                                                            <telerik:GridBoundColumn HeaderText="EmailSubject" DataField="EmailSubject" UniqueName="EmailSubject" />
                                                            <telerik:GridBoundColumn HeaderText="EmailFrom" DataField="EmailFrom" UniqueName="EmailFrom" />
                                                            <telerik:GridBoundColumn HeaderText="CustomerID" DataField="CustomerID" UniqueName="CustomerID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="CustomerName" DataField="CustomerName" UniqueName="CustomerName" />
                                                            <telerik:GridBoundColumn HeaderText="GroupID" DataField="GroupID" UniqueName="GroupID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="GroupName" DataField="GroupName" UniqueName="GroupName" />
                                                            <telerik:GridBoundColumn HeaderText="StatusCode" DataField="StatusCode" UniqueName="StatusCode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="SendTime" DataField="SendTime" UniqueName="SendTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="StartTime" DataField="StartTime" UniqueName="StartTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="FinishTime" DataField="FinishTime" UniqueName="FinishTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <telerik:GridBoundColumn HeaderText="SendTotal" DataField="SendTotal" UniqueName="SendTotal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                        <asp:PlaceHolder ID="phAdditionalData" runat="server" Visible="false">

                                            <tr>
                                                <td>
                                                    <table cellpadding="2" cellspacing="2" width="100%" border='0'>
                                                        <asp:PlaceHolder ID="plSendInformation" runat="server" Visible="false">
                                                            <tr>
                                                                <td style="font-size: 12px">
                                                                    <asp:CheckBox ID="cbGetBlastReport" runat="server" />
                                                                    Get Blast Report
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:CheckBox ID="cbGetCurrentCounts" runat="server" />
                                                                Get Current Count
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:CheckBox ID="cbGetPreviousBlastInfo" runat="server" />
                                                                Show Previous Blast Info For Engine
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:CheckBox ID="cbGetSubscribersToSend" runat="server" />
                                                                Get Subscribers to Send
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:CheckBox ID="cbShowErrors" runat="server" />Show Last 5 Errors
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:Button ID="btnAdditionalData" runat="server" Text="Get Additional Information" OnClick="btnAdditionalData_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="plReset" runat="server" Visible="false">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset to Pending" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <asp:Panel ID="plBlastReport" runat="server" Style="padding-left: 30px;" Height="100%" Font-Size="Smaller" Width="95%" Visible="false">
                                            <tr>
                                                <td valign="middle" class="headingOne"><b>Blast Report</b>
                                                    <asp:HyperLink ID="hlBlastReport" runat="server" Target="_blank" Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Survey Reporting' border='0'>" />
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td>
                                                <telerik:RadPanelBar ID="RadPanelBar1" runat="server" Width="100%" Visible="false" CssClass="New">
                                                    <Items>
                                                        <telerik:RadPanelItem Text="Current Counts" Value="rpiCurrentCounts" Visible="false" CssClass="headingOne" Font-Bold="True" Expanded="True" BorderStyle="None">
                                                            <Items>
                                                                <telerik:RadPanelItem Value="rpitemCurrentCounts" runat="server" Expanded="True">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="2" cellspacing="2" width="90%" border='0'>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgCurrentCounts" runat="server" Width="30%">
                                                                                        <MasterTableView Font-Bold="False">
                                                                                            <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn HeaderText="ActiontypeCode" DataField="ActiontypeCode" UniqueName="ActiontypeCode" HeaderStyle-Width="10px" />
                                                                                                <telerik:GridBoundColumn HeaderText="DistinctCount" DataField="DistinctCount" UniqueName="DistinctCount" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="Total" DataField="Total" UniqueName="Total" HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelItem>
                                                            </Items>
                                                        </telerik:RadPanelItem>
                                                        <telerik:RadPanelItem Text="Previous Blast Info" Value="rpiPreviousBlastInfo" Visible="false" CssClass="headingOne" Font-Bold="True" Expanded="True" BorderStyle="None">
                                                            <Items>
                                                                <telerik:RadPanelItem Value="rpitemPreviousBlastInfo" runat="server" Expanded="True">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="2" cellspacing="2" width="90%" border='0'>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgPreviousBlastInfo" runat="server" Width="920px" Visible="false">
                                                                                        <ClientSettings>
                                                                                            <Scrolling AllowScroll="True" UseStaticHeaders="true" ScrollHeight="100px" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="BlastID" Width="100%">
                                                                                            <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn HeaderText="BlastEngineID" DataField="BlastEngineID" UniqueName="BlastEngineID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="BlastID" DataField="BlastID" UniqueName="BlastID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="CampaignItemName" DataField="CampaignItemName" UniqueName="CampaignItemName" />
                                                                                                <telerik:GridBoundColumn HeaderText="EmailSubject" DataField="EmailSubject" UniqueName="EmailSubject" />
                                                                                                <telerik:GridBoundColumn HeaderText="EmailFrom" DataField="EmailFrom" UniqueName="EmailFrom" />
                                                                                                <telerik:GridBoundColumn HeaderText="CustomerID" DataField="CustomerID" UniqueName="CustomerID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="CustomerName" DataField="CustomerName" UniqueName="CustomerName" />
                                                                                                <telerik:GridBoundColumn HeaderText="GroupID" DataField="GroupID" UniqueName="GroupID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="GroupName" DataField="GroupName" UniqueName="GroupName" />
                                                                                                <telerik:GridBoundColumn HeaderText="StatusCode" DataField="StatusCode" UniqueName="StatusCode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="SendTime" DataField="SendTime" UniqueName="SendTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="StartTime" DataField="StartTime" UniqueName="StartTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="FinishTime" DataField="FinishTime" UniqueName="FinishTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                                <telerik:GridBoundColumn HeaderText="SendTotal" DataField="SendTotal" UniqueName="SendTotal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelItem>
                                                            </Items>
                                                        </telerik:RadPanelItem>
                                                        <telerik:RadPanelItem Text="Subscribers to Send" Value="rpiSubscribers" Visible="false" CssClass="headingOne" Font-Bold="True" Expanded="True" BorderStyle="None">
                                                            <Items>
                                                                <telerik:RadPanelItem Value="rpitemSubscribers" runat="server" Expanded="True">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="2" cellspacing="2" width="90%" border='0'>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgSubscribersToSend" runat="server" Width="920px" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" OnNeedDataSource="rgSubscribersToSend_NeedDataSource">
                                                                                        <ClientSettings>
                                                                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="EmailID" Width="100%">
                                                                                            <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageSizes="100" />
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn HeaderText="EmailAddress" DataField="EmailAddress" UniqueName="EmailAddress" />
                                                                                                <telerik:GridTemplateColumn UniqueName="Edit" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HyperLink runat="Server" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit / View Email Profile Information' border='0'>" 
                                                                                                            NavigateUrl='<%# "/ecn.communicator/main/lists/emaileditor.aspx?EmailID=" + DataBinder.Eval(Container, "DataItem.EmailID") + "&GroupID=" + DataBinder.Eval(Container, "DataItem.GroupID") %>'
                                                                                                            ID="Hyperlink1" NAME="Hyperlink1" Target="_blank">
                                                                                                        </asp:HyperLink>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelItem>
                                                            </Items>
                                                        </telerik:RadPanelItem>
                                                        <telerik:RadPanelItem Text="Errors" Value="rpiErrors" Visible="false" CssClass="headingOne" Font-Bold="True" Expanded="True" BorderStyle="None">
                                                            <Items>
                                                                <telerik:RadPanelItem Value="rpitemErrors" runat="server" Expanded="True">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="2" cellspacing="2" width="90%" border='0'>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgErrors" runat="server" Width="920px">
                                                                                        <ClientSettings>
                                                                                            <Scrolling AllowScroll="True" UseStaticHeaders="true" ScrollHeight="500px" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="LogID" Width="100%">
                                                                                            <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn HeaderText="Source Method" DataField="SourceMethod" UniqueName="SourceMethod" SortExpression="SourceMethod" ItemStyle-VerticalAlign="Top" />
                                                                                                <telerik:GridBoundColumn HeaderText="Exception" DataField="Exception" UniqueName="Exception" SortExpression="Exception" ItemStyle-VerticalAlign="Top" />
                                                                                                <telerik:GridBoundColumn HeaderText="Log Note" DataField="LogNote" UniqueName="LogNote" SortExpression="LogNote" ItemStyle-VerticalAlign="Top" />
                                                                                                <telerik:GridBoundColumn HeaderText="LogAddedDate" DataField="LogAddedDate" UniqueName="LogAddedDate" SortExpression="LogAddedDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-VerticalAlign="Top" />
                                                                                                <telerik:GridBoundColumn HeaderText="LogAddedTime" DataField="LogAddedTime" UniqueName="LogAddedTime" SortExpression="LogAddedTime" ItemStyle-VerticalAlign="Top" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelItem>
                                                            </Items>
                                                        </telerik:RadPanelItem>
                                                    </Items>
                                                </telerik:RadPanelBar>
                                            </td>
                                        </tr>
                                    </table>
                                </contenttemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabBlastEngineStatus" runat="server" TabIndex="1" HeaderText="Blast Engine Status">
                                <headertemplate>
                                    <span><b>Blast Engine Status</b></span>
                                </headertemplate>
                                <contenttemplate>
                                    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
                                        AssociatedUpdatePanelID="update1" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                                                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                                                    <div>
                                                        <center>
                                                            <br />
                                                            <br />
                                                            <b>Processing...</b><br />
                                                            <br />
                                                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                                                            <br />
                                                            <br />
                                                            <br />
                                                        </center>
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <div>
                                        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="60000">
                                        </asp:Timer>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <table cellpadding="5" cellspacing="5" width="100%" border='0'>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server" EnableViewState="false">
                                                            <asp:Label ID="lbTimeRefreshed" runat="server" Text="Grid will refresh after every 1 minute." CssClass="formLabel"></asp:Label><br />
                                                            <telerik:RadGrid AutoGenerateColumns="False" ID="rgBlastEngineStatus" runat="server" OnItemDataBound="rgBlastEngineStatus_ItemDataBound" Width="920px" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" OnNeedDataSource="rgBlastEngineStatus_NeedDataSource">
                                                                <ClientSettings>
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                                                </ClientSettings>
                                                                <MasterTableView DataKeyNames="BlastEngineID" Width="100%" EnableViewState="true">
                                                                    <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageSizes="20" />
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="StatusIndicator">
                                                                            <ItemTemplate>
                                                                                <img alt="" src="/ecn.images/images/StatusRed.png" id="imgStatus" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn HeaderText="BlastEngineName" DataField="BlastEngineName" UniqueName="BlastEngineName" />
                                                                        <telerik:GridBoundColumn HeaderText="Status" DataField="StatusCode" UniqueName="StatusCode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridBoundColumn HeaderText="BlastID" DataField="BlastID" UniqueName="BlastID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridBoundColumn HeaderText="EmailSubject" DataField="EmailSubject" UniqueName="EmailSubject" />
                                                                        <telerik:GridBoundColumn HeaderText="CustomerName(CustomerID)" DataField="Customer" UniqueName="Customer" />
                                                                        <telerik:GridBoundColumn HeaderText="SendTime" DataField="SendTime" UniqueName="SendTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridBoundColumn HeaderText="StartTime" DataField="StartTime" UniqueName="StartTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridBoundColumn HeaderText="LastUpdatedTime" DataField="LastUpdatedTime" UniqueName="LastUpdatedTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridBoundColumn HeaderText="SendTotal" DataField="SendTotal" UniqueName="SendTotal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false" />
                                                                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false" />
                                                                        <telerik:GridTemplateColumn UniqueName="RadialGauge" AllowFiltering="false" HeaderText="AlreadySent" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <telerik:RadRadialGauge ID="RadRadialGauge" Width="150" Height="150" Scale-Max="10000" runat="server" Pointer-Value='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "AlreadySent"))%>' Visible="false">
                                                                                </telerik:RadRadialGauge>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </contenttemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
