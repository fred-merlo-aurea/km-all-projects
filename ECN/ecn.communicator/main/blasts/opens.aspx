<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.opens" CodeBehind="opens.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px;
        padding-top: 0px" align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" align="left">
                    <table cellspacing="0" cellpadding="0" border='0' width="100%">
                        <tr>
                            <td class="wizTabs" valign="bottom">
                                <asp:LinkButton ID="btnActiveOpens" runat="Server" Text="<span>Active Opens</span>"
                                    OnClick="btnActiveOpens_Click"></asp:LinkButton>
                            </td>                          
                             <td class="wizTabs" valign="bottom">
                                <asp:LinkButton ID="btnAllOpens" runat="Server" Text="<span>All Opens</span>"
                                    OnClick="btnAllOpens_Click"></asp:LinkButton>
                            </td>
                              <td class="wizTabs" valign="bottom">
                                <asp:LinkButton ID="btnOpensbyTime" runat="Server" Text="<span>Opens by Time</span>"
                                    OnClick="btnOpensbyTime_Click"></asp:LinkButton>
                            </td>
                            <td class="wizTabs" valign="bottom">
                                <asp:LinkButton ID="btnBrowserStats" runat="Server" Text="<span>Browser Stats</span>"
                                    OnClick="btnBrowserStats_Click"></asp:LinkButton>
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
                <td class="gradient" valign="middle" align='right'>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="offWhite greySidesB" width="100%" align="center">
                    <br />
                    <asp:PlaceHolder ID="phActiveGrid" runat="Server" Visible="true">
                        <asp:DataGrid ID="ActiveGrid" runat="Server" Width="95%" AutoGenerateColumns="False"
                            CssClass="gridWizard">
                            <HeaderStyle CssClass="gridheaderWizard" />
                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                            <Columns>
                                <asp:BoundColumn ItemStyle-Width="25%" DataField="ActionCount" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Count"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="75%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phAllOpens" runat="Server" Visible="false">
                        <asp:Panel ID="DownloadPanel" runat="Server" Visible="true" CssClass="tableHeader"
                            Height="35px" Style="margin-top: 10px; text-align: right; width: 95%">
                            Download&nbsp;
                            <asp:DropDownList EnableViewState="true" ID="OpensTypeRBList" runat="Server" class="formfield"
                                Visible="true">
                                <asp:ListItem Selected="true" Value="all">All Open</asp:ListItem>
                                <asp:ListItem Value="unique">Unique Open</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;email addresses as&nbsp;
                            <asp:DropDownList EnableViewState="true" ID="DownloadType" runat="Server" class="formfield"
                                Visible="true">
                                <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" runat="Server" Visible="true"
                                Text="Download" OnClick="downloadOpenEmails"></asp:Button>&nbsp;
                        </asp:Panel>
                        <asp:DataGrid ID="OpensGrid" runat="Server" Width="95%" AutoGenerateColumns="False"
                            CssClass="gridWizard" PageSize="50">
                            <HeaderStyle CssClass="gridheaderWizard" />
                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                            <FooterStyle CssClass="tableHeader1" />
                            <Columns>
                                <asp:BoundColumn ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
                                    DataField="ActionDate" HeaderText="Open Time"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="28%" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn ItemStyle-Width="53%" DataField="ActionValue" HeaderText="Info"  ItemStyle-HorizontalAlign="Left">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="OpensPager" runat="Server" Width="95%" PageSize="50" OnIndexChanged="OpensPager_IndexChanged">
                            <PagerStyle CssClass="gridpager"></PagerStyle>
                        </AU:PagerBuilder>
                    </asp:PlaceHolder>
                    <br />
                    <asp:PlaceHolder ID="phBrowserStats" runat="Server" Visible="false">
                        <div align="center">
                            <asp:Label ID="phBrowserStatsTitle" runat="server" Text="Email Client Usage" Font-Bold="True"
                                Font-Size="Medium" />
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr valign="top" align="right">
                                        <td>
                                            Format:
                                            <asp:DropDownList ID="dropdownExport" runat="server">
                                                <asp:ListItem>PDF</asp:ListItem>
                                                <asp:ListItem>Excel</asp:ListItem>
                                                <asp:ListItem>Word</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="ltbnExport" runat="server" OnClick="Export_Click">Export</asp:LinkButton>
                                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="False" ShowRefreshButton="False"
                                                Visible="false">
                                                <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_BlastOpens.rdlc">
                                                </LocalReport>
                                            </rsweb:ReportViewer>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Chart ID="chartBrowserStats" runat="server" BackColor="Transparent" Height="450px"
                                                Width="600px" AntiAliasing="Graphics">
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
                                        </td>
                                    </tr>
                                    </table>

                                    <table  width="80%">
                                    <tr valign="top">
                                        <td align="left">
                                            <br />
                                            <br />
                                            <asp:Label ID="lblTotalOpens" runat="server" Font-Bold="True" Font-Size="Medium" />
                                            <br />
                                            <br />
                                            <asp:DataList ID="dlPlatform" runat="server" OnItemDataBound="dlPlatform_ItemDataBound"
                                                Font-Size="Small" RepeatDirection="Horizontal" Width="100%" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    Platform:
                                                    <asp:Label ID="lblPlatform" runat="server" Text='<%# Eval("PlatformName") %>' Font-Bold="True"
                                                        Font-Size="Small" />
                                                    <br />
                                                    Opens:
                                                    <asp:Label ID="lblOpens" runat="server" Text='<%# Eval("Opens") %>' Font-Bold="True"
                                                        Font-Size="X-Small" />
                                                    <br />
                                                    Usage:
                                                    <asp:Label ID="lblUsage" runat="server" Text='<%# Eval("Usage") %>' Font-Bold="True"
                                                        Font-Size="X-Small" /><br />
                                                    <br />
                                                    <asp:GridView ID="gvEmailClients" runat="server" ForeColor="Black" ShowHeader="true"
                                                        Font-Size="X-Small" CssClass="gridWizard" OnRowDataBound="gvEmailClients_RowDataBound"
                                                        AutoGenerateColumns="false">
                                                        <FooterStyle BackColor="#CCCCCC" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Client">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%#Eval("EmailClientName") %>' runat="server" ID="gvlblEmailClientName"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Opens">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%#Eval("Opens") %>' runat="server" ID="gvlblOpens"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Usage">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%#Eval("Usage") %>' runat="server" ID="gvlblUsage"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="gridheaderWizard" />
                                                        <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#808080" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#383838" />
                                                    </asp:GridView>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ltbnExport" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:PlaceHolder>

                      <asp:PlaceHolder ID="phOpensGrid" runat="Server" Visible="false">
                        <div align="center">
                            <asp:Label ID="lblOpensByTime" runat="server" Text="Opens By Time" Font-Bold="True"
                                Font-Size="Medium" /><br />

                                <div align="right">
                                 Format:
                                            <asp:DropDownList ID="drpExportOpensByTime" runat="server">
                                                <asp:ListItem>PDF</asp:ListItem>
                                                <asp:ListItem>Excel</asp:ListItem>
                                                <asp:ListItem>Word</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="lnkbtnOpenbyTimeExport" runat="server" OnClick="OpenbyTimeExport_Click">Export</asp:LinkButton>
                                            <rsweb:ReportViewer ID="ReportViewer2" runat="server" ShowBackButton="False" ShowRefreshButton="False"
                                                Visible="false">
                                                <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_BlastOpensByTime.rdlc">
                                                </LocalReport>
                                            </rsweb:ReportViewer>
                                </div>

                             <asp:Chart ID="chtOpensbyTime" runat="server"   BackColor="Transparent"  AntiAliasing="Graphics">                            
                            </asp:Chart> 
                        
                                <asp:GridView ID="gvOpens" runat="server" ForeColor="Black" ShowHeader="true"
                                    Font-Size="X-Small" CssClass="gridWizard"
                                    AutoGenerateColumns="false" Width="60%">
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Hour Range">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("Hour") %>' runat="server" ID="gvlblHour"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Opens">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("Opens") %>' runat="server" ID="gvlblOpens"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OpensPerc">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("OpensPerc") %>' runat="server" ID="gvlblOpensPerc"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheaderWizard" />
                                    <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                </asp:GridView>
                        </div>
                    </asp:PlaceHolder>
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
