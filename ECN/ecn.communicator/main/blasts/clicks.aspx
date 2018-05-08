<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.clicks_main" CodeBehind="clicks.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#content [href]').each(function () {
                if ($(this).attr("clickvalue") != null) {
                    $(this).qtip(
       {
           content: $(this).attr("clickvalue"),
           show: {
               when: false,
               ready: true
           },
           hide: true,
           style: {
               name: 'red',
               tip: 'topLeft'
           }
       });
                }
            });



        });
    </script>

    <script type="text/javascript" src="/ecn.communicator/scripts/jquery.qtip-1.0.0-rc3.min.js"></script>

    <style type='text/css'>
        .ui-datepicker-trigger {
            position: relative;
            vertical-align: middle;
            padding-left: 5px;
        }
    </style>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtstartDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                numberOfMonths: 1,
                showButtonPanel: true
            });
            $("#<%=txtendDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                numberOfMonths: 1,
                showButtonPanel: true
            });
        });
    </script>


    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px; padding-top: 0px"
        align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" align="left">
                    <table cellspacing="0" cellpadding="0" border='0' width="100%">
                        <tr>
                            <td valign="bottom" class="wizTabs">
                                <asp:LinkButton ID="btnTopClicks" runat="Server" Text="<span>Top Clicks</span>" OnClick="btnTopClicks_Click"></asp:LinkButton>
                            </td>
                            <td valign="bottom" class="wizTabs">
                                <asp:LinkButton ID="btnTopVisitors" runat="Server" Text="<span>Top Visitors</span>"
                                    OnClick="btnTopVisitors_Click"></asp:LinkButton>
                            </td>
                            <td valign="bottom" class="wizTabs">
                                <asp:LinkButton ID="btnClicksbyTime" runat="Server" Text="<span>Clicks by Time</span>"
                                    OnClick="btnClicksbyTime_Click"></asp:LinkButton>
                            </td>
                            <td valign="bottom" class="wizTabs"> 
                                <asp:LinkButton ID="btnClicksHeatMap" runat="Server" Text="<span>Clicks HeatMap</span>" OnClick="btnClicksHeatMap_Click"></asp:LinkButton>
                            </td>
                            <td width="100%" align='right' class="homeButton" style="height: 40px" valign="top">
                                <asp:LinkButton ID="btnHome" runat="Server" Text="<span>Report Summary Page</span>"
                                    OnClick="btnHome_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gradient" valign="middle" align='right'>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="offWhite greySidesB" width="100%" align="center">
                    <asp:PlaceHolder ID="phTopClicks" runat="Server" Visible="true">
                        <div style="padding: 5px 0 10px 0; text-align: left; vertical-align: middle; width: 95%">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 5%;">
                                        <asp:Label ID="lblFilter" runat="server" Text="Filter" Font-Bold="true" Font-Size="Small" />
                                    </td>
                                    <td style="width: 60%;">
                                        <asp:DropDownList ID="ClickSelectionDD" runat="Server" CssClass="formfield" OnSelectedIndexChanged="ClickSelectionDD_SelectedIndexChanged"
                                            AutoPostBack="true" Visible="true">
                                            <asp:ListItem Value="TOP 10" Selected="True">Top 10 Clicks</asp:ListItem>
                                            <asp:ListItem Value="TOP 20">Top 20 Clicks</asp:ListItem>
                                            <asp:ListItem Value="TOP 30">Top 30 Clicks</asp:ListItem>
                                            <asp:ListItem Value="TOP 40">Top 40 Clicks</asp:ListItem>
                                            <asp:ListItem Value="TOP 50">Top 50 Clicks</asp:ListItem>
                                            <asp:ListItem Value="">All Clicks</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:Label ID="lblDownload" Font-Bold="true" Font-Size="Small" Text="Download Current View" runat="server" />
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:DropDownList ID="ddlDownloadView" Width="100%" runat="server">
                                            <asp:ListItem Text="Summary" Value="summary" Selected="True" />
                                            <asp:ListItem Text="Detail" Value="detail" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%;">
                                        <asp:Button ID="btnDownloadView" class="formbuttonsmall" runat="server" Text="Download" OnClick="btnDownloadView_Click" />
                                    </td>
                                </tr>
                            </table>


                        </div>
                        <asp:DataGrid ID="TopGrid" runat="Server" Width="95%" AllowSorting="true" AutoGenerateColumns="False"
                            CssClass="gridWizard" OnSortCommand="TopGrid_Sort">
                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                            <Columns>
                                <asp:BoundColumn HeaderText ="Total Clicks" DataField="ClickCount" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" SortExpression="ClickCount">
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Total Clicks" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" SortExpression="ClickCount">
                                    <ItemTemplate>
                                        <a href="clickslinks.aspx<%# getClicksLinkURL().ToString() %>&link=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem, "NewActionValue").ToString()).ToString()%>">
                                            <%# DataBinder.Eval(Container.DataItem, "ClickCount")%> 
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="Unique Clicks" DataField="distinctClickCount" ItemStyle-Width="12%"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="distinctClickCount" />
                                <asp:TemplateColumn HeaderText="URL / Link Alias" ItemStyle-Width="70%" SortExpression="SmallLink"
                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <a href="<%# DataBinder.Eval(Container.DataItem, "NewActionValue")%>" target='_blank'>
                                            <%# DataBinder.Eval(Container.DataItem, "SmallLink")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Download" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <a href="clicks.aspx?BlastID=<%# getBlastID().ToString() %>&action=report&actionURL=<%# Server.UrlEncode(DataBinder.Eval(Container.DataItem, "NewActionValue").ToString())%>">Report</a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="TopClicksPager" runat="Server" Width="95%" PageSize="10" ControlToPage="TopGrid"
                            OnIndexChanged="TopClicksPager_IndexChanged">
                            <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                        </AU:PagerBuilder>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phTopVisitors" runat="Server" Visible="true">
                        <div style="padding-top: 10px">
                            <asp:DataGrid ID="EmailsGrid" runat="Server" Width="95%" CssClass="gridWizard" AutoGenerateColumns="False"
                                PageSize="10" AllowSorting="true" OnSortCommand="EmailsGrid_Sort">
                                <ItemStyle CssClass="tableContentSmall"></ItemStyle>
                                <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" DataField="ClickCount"
                                        HeaderStyle-HorizontalAlign="center" HeaderText="Click Count" SortExpression="ClickCount"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="75%" SortExpression="EmailAddress"
                                        ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                                <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <AlternatingItemStyle CssClass="gridaltrowWizard" />
                            </asp:DataGrid>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phClicksbyTime" runat="Server" Visible="false">
                        <asp:Panel ID="DownloadPanel" runat="Server" Visible="true" CssClass="tableHeader"
                            Height="35px" Style="margin-top: 10px; text-align: right; width: 95%">
                            <table>
                                <tr>
                                    <td width="200">
                                        <asp:Label ID="lblDateRangeError" Width="200" runat="server" Text="Please enter a valid date range" Style="color: red;" Visible="false"></asp:Label>
                                    </td>
                                    <td width="75px">
                                        <asp:Label ID="lblClicksByTimeRange" runat="server" Text="Set Range" Font-Bold="true" Font-Size="Small" Visible="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstartDate" runat="Server" Width="60" CssClass="formfield" OnTextChanged="txtStartDate_textChanged" AutoPostBack="true"></asp:TextBox>
                                        &nbsp;
                                        <asp:TextBox ID="txtendDate" runat="Server" Width="60" CssClass="formfield" OnTextChanged="txtEndDate_textChanged" AutoPostBack="true"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList EnableViewState="true" ID="ddlClicksType" runat="Server" class="formfield"
                                            Visible="true" OnSelectedIndexChanged="ddlCLicksType_indexChanged" AutoPostBack="true">
                                            <asp:ListItem Selected="true" Value="all">All Clicks</asp:ListItem>
                                            <asp:ListItem Value="unique">Unique Clicks</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="100px">&nbsp; Download as&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList class="formfield" ID="DownloadType" runat="Server" Visible="true"
                                            EnableViewState="true">
                                            <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                            <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                            <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" OnClick="downloadClickEmails"
                                            runat="Server" Text="Download" Visible="true" Enabled="false"></asp:Button>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:DataGrid ID="ClicksGrid" runat="Server" Width="95%" AutoGenerateColumns="False"
                            PageSize="20" CssClass="gridWizard" AllowSorting="true" OnSortCommand="ClicksGrid_Sort">
                            <HeaderStyle CssClass="gridheaderWizard" />
                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                            <FooterStyle CssClass="tableHeader1" />
                            <Columns>
                                <asp:BoundColumn ItemStyle-Width="23%" ItemStyle-HorizontalAlign="Center" DataField="ClickTime"
                                    HeaderStyle-HorizontalAlign="center" HeaderText="Click Time" SortExpression="ClickTime"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="25%" SortExpression="EmailAddress"
                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="URL / Link Alias" ItemStyle-Width="52%" SortExpression="SmallLink"
                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <a href="<%# DataBinder.Eval(Container.DataItem, "FullLink")%>" target='_blank'>
                                            <%# DataBinder.Eval(Container.DataItem, "SmallLink")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="ClicksPager" runat="Server" Width="95%" PageSize="50" ControlToPage="ClicksGrid"
                            OnIndexChanged="ClicksPager_IndexChanged">
                            <PagerStyle CssClass="gridpager"></PagerStyle>
                        </AU:PagerBuilder>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phClicksHeatMap" runat="Server" Visible="false">
                        <div id="content">
                            <asp:Label ID="LabelPreview" runat="Server" Text="Label"></asp:Label>
                        </div>
                    </asp:PlaceHolder>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
    </rsweb:ReportViewer>
</asp:Content>
