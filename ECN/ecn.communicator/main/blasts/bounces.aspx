<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.bounces_main" CodeBehind="bounces.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tr>
            <td colspan="2" align='right' class="homeButton" style="height: 40px" valign="top">
                <asp:LinkButton ID="btnHome" runat="Server" Text="<span>Report Summary Page</span>"
                    OnClick="btnHome_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="gradient">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" class="offWhite greySides" style="padding: 0 5px; border-bottom: 1px #A4A2A3 solid;">
                <div class="moveUp">
                    <table id="layoutWrapper1" cellspacing="1" cellpadding="1" width="100%" border='0'>
                        <tr>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr>
                                        <td width="20">
                                            <img src="/ecn.images/images/bounces_icon.gif" />
                                        </td>
                                        <td valign="top" align="left" class="tableHeader" style="padding: 2px 0 0 0;">
                                            Bounces
                                        </td>
                                        <td align="right" class="tableHeader" width="70%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr valign="top">
                                        <td width="60%" align="left">
                                            <asp:DataGrid ID="BounceTypesGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="gridWizard">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundColumn ItemStyle-Width="70%" DataField="ACTIONVALUE" HeaderText="Bounce Type"
                                                        HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="15%" DataField="UNIQUEBounces" HeaderText="Unique Bounces">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="15%" DataField="TOTALBounces" HeaderText="Total Bounces">
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                            </asp:DataGrid>
                                            <br />
                                            <input type="BUTTON" value="View Error Codes" class="formbuttonsmall" onclick="window.open('smtperrorcodes.htm')"/>
                                        </td>
                                        <td align="right">
                                            <asp:DropDownList ID="dropdownExport" runat="server" CssClass="formfield">
                                                <asp:ListItem Value="PDF">PDF</asp:ListItem>
                                                <asp:ListItem Value="Excel">Excel</asp:ListItem>
                                                <asp:ListItem Value="Word">Word</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="ltbnExport" runat="server" OnClick="Export_Click" Text="Export" class="formbuttonsmall">
                                            </asp:Button>
                                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="False" ShowRefreshButton="False"
                                                Visible="false">
                                                <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_BlastsBounces.rdlc">
                                                </LocalReport>
                                            </rsweb:ReportViewer>
                                            <br />
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            <asp:Chart ID="chartBounceTypes" runat="server" BackColor="Transparent" Height="250px"
                                                Width="350px">
                                                <Series>
                                                    <asp:Series Name="Series1" ChartType="Pie" IsValueShownAsLabel="True" Palette="BrightPastel">
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1" BackSecondaryColor="White" BackColor="Transparent"
                                                        ShadowColor="Transparent">
                                                        <AxisX>
                                                            <MajorGrid Enabled="False" />
                                                        </AxisX>
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                            </asp:Chart>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="Panel1" runat="server" style="text-align:left"> 
                                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                    Width="100%" >
                                    <asp:TabPanel runat="server" HeaderText="By Domain" ID="TabPanel1">
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="dropdownView" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvDomains" EventName="sorting" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <br />
                                                    <table width="100%">
                                                        <tr align="right">
                                                            <td>
                                                                <asp:DropDownList ID="dropdownView" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropdownView_SelectedIndexChanged"
                                                                    CssClass="formfield">
                                                                    <asp:ListItem Value="10">Top 10</asp:ListItem>
                                                                    <asp:ListItem Value="30">Top 30</asp:ListItem>
                                                                    <asp:ListItem Value="50">Top 50</asp:ListItem>
                                                                    <asp:ListItem Value="100">Top 100</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="gvDomains" CssClass="gridWizard" Width="100%" OnSorting="gvDomains_Sorting"
                                                                    AutoGenerateColumns="false" runat="server" GridLines="None" AllowSorting="true"
                                                                    OnRowDataBound="gvDomains_RowDataBound">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Domain" HeaderText="By Domain" ReadOnly="True" SortExpression="Domain"
                                                                            HeaderStyle-Width="30%" ItemStyle-Width="30%">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="TotalBounces" HeaderText="TotalBounces" ReadOnly="True"
                                                                            SortExpression="TotalBounces" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Hard" HeaderText="Hard" ReadOnly="True" SortExpression="Hard"
                                                                            HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Soft" HeaderText="Soft" ReadOnly="True" SortExpression="Soft"
                                                                            HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Other" HeaderText="Other" ReadOnly="True" SortExpression="Other"
                                                                            HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="MessagesSent" HeaderText="MessagesSent" ReadOnly="True"
                                                                            SortExpression="MessagesSent" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="PercBounced" HeaderText="PercBounced" ReadOnly="True"
                                                                            SortExpression="PercBounced" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="gridheaderWizard" />
                                                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel runat="server" HeaderText="Hard Bounces" ID="TabPanel2">
                                        <ContentTemplate>
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="UnsubBouncesButton" OnClick="UnsubscribeBounces" runat="Server" Text="Unsubscribe Hard Bounces"
                                                                    class="formbuttonsmall" Visible="true" />
                                                            </td>
                                                            <td class="tableHeader" align="right">
                                                                <asp:Panel ID="DownloadPanelHard" runat="Server" Visible="true">
                                                                    Download Current View&nbsp;&nbsp;
                                                                    <asp:DropDownList EnableViewState="true" ID="DownloadTypeHard" runat="Server" class="formfield"
                                                                        Visible="true">
                                                                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                                                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                                                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:Button class="formbuttonsmall" ID="DownloadButtonHard" runat="Server" Visible="true"
                                                                        Text="Download" OnClick="downloadBouncedEmails_Hard"></asp:Button>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:DataGrid ID="BouncesGridHard" runat="Server" Width="100%" AutoGenerateColumns="False" CssClass="gridWizard">
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="EmailID" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="17%" DataField="ActionDate" HeaderText="Bounce Time">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="41%">
                                                                <ItemTemplate>
                                                                    <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                                                        <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn ItemStyle-Width="10%" DataField="ActionValue" HeaderText="Bounce Type"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="30%" DataField="ActionNotes" HeaderText="Bounce Message">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                                    </asp:DataGrid>
                                                    <AU:PagerBuilder ID="BouncesPagerHard" runat="Server" Width="100%" PageSize="50"
                                                        OnIndexChanged="BouncesPagerHard_IndexChanged">
                                                        <PagerStyle CssClass="gridpager"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="DownloadButtonHard" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel runat="server" HeaderText="Soft Bounces" ID="TabPanel3">
                                        <ContentTemplate>
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="ResendSoftBouncesButton" OnClick="ResendSoftBounces" runat="Server"
                                                                    Text="Resend Soft Bounces" class="formbuttonsmall" Visible="true" />
                                                                &nbsp;
                                                            </td>
                                                            <td class="tableHeader" align="right">
                                                                <asp:Panel ID="DownloadPanelSoft" runat="Server" Visible="true">
                                                                    Download Current View&nbsp;&nbsp;
                                                                    <asp:DropDownList EnableViewState="true" ID="DownloadTypeSoft" runat="Server" class="formfield"
                                                                        Visible="true">
                                                                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                                                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                                                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:Button class="formbuttonsmall" ID="DownloadButtonSoft" runat="Server" Visible="true"
                                                                        Text="Download" OnClick="downloadBouncedEmails_Soft"></asp:Button>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:DataGrid ID="BouncesGridSoft" runat="Server" Width="100%" AutoGenerateColumns="False" CssClass="gridWizard">
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="EmailID" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="17%" DataField="ActionDate" HeaderText="Bounce Time">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="41%">
                                                                <ItemTemplate>
                                                                    <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                                                        <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn ItemStyle-Width="10%" DataField="ActionValue" HeaderText="Bounce Type"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="30%" DataField="ActionNotes" HeaderText="Bounce Message">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                                    </asp:DataGrid>
                                                    <AU:PagerBuilder ID="BouncesPagerSoft" runat="Server" Width="100%" PageSize="50"
                                                        OnIndexChanged="BouncesPagerSoft_IndexChanged">
                                                        <PagerStyle CssClass="gridpager"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="DownloadButtonSoft" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel runat="server" HeaderText="Other Bounces" ID="TabPanel4">
                                        <ContentTemplate>
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="tableHeader" align="left">
                                                                Filter Bounces types&nbsp;<asp:DropDownList EnableViewState="true" ID="BounceType"
                                                                    runat="Server" class="formfield" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="BounceTypeOthers_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="tableHeader" align="right">
                                                                <asp:Panel ID="DownloadPanelOthers" runat="Server" Visible="true">
                                                                    Download Current View&nbsp;&nbsp;
                                                                    <asp:DropDownList EnableViewState="true" ID="DownloadTypeOthers" runat="Server" class="formfield"
                                                                        Visible="true">
                                                                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                                                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                                                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:Button class="formbuttonsmall" ID="DownloadButtonOthers" runat="Server" Visible="true"
                                                                        Text="Download" OnClick="downloadBouncedEmails_Others"></asp:Button>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:DataGrid ID="BouncesGridOthers" runat="Server" Width="100%" AutoGenerateColumns="False"
                                                         CssClass="gridWizard">
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="EmailID" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="17%" DataField="ActionDate" HeaderText="Bounce Time">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="41%">
                                                                <ItemTemplate>
                                                                    <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                                                        <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn ItemStyle-Width="10%" DataField="ActionValue" HeaderText="Bounce Type"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="30%" DataField="ActionNotes" HeaderText="Bounce Message">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                                    </asp:DataGrid>
                                                    <AU:PagerBuilder ID="BouncesPagerOthers" runat="Server" Width="100%" PageSize="50"
                                                        OnIndexChanged="BouncesPagerOthers_IndexChanged">
                                                        <PagerStyle CssClass="gridpager"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="DownloadButtonOthers" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                </asp:TabContainer>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
