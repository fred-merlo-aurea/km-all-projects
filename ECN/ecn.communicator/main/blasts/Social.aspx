<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.Social" CodeBehind="Social.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
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
    <table id="layoutWrapper1" cellspacing="0" cellpadding="0" width="100%" border='0'>
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
                    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                        <tr>
                            <td class="tableHeader" width="100%">
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr>
                                        <td width="20">
                                            <img src="/ecn.images/images/socialshare.png" />
                                        </td>
                                        <td valign="top" align="left" class="tableHeader" style="padding: 2px 0 0 0;">
                                            Subscriber Sharing
                                        </td>
                                        <td align="right" class="tableHeader" width="70%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr valign="top">
                                        <td width="20%">
                                            <ecnCustom:ecnGridView ID="SocialGrid" runat="Server" AutoGenerateColumns="False"
                                                Style="margin: 7px 0;" Width="100%" CssClass="gridWizard" datakeyfield="BlastID"
                                                DataKeyNames="ID" OnRowDataBound="SocialGrid_RowDataBound">
                                                <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Media" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="center"
                                                        ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <%--<asp:HyperLink ID="hlMediaReporting" runat="server" Text='<%# Eval("ReportImagePath").ToString() %>'
                                                            NavigateUrl='<%# Eval("IsBlastGroup").ToString().ToLower()=="y"?"/social.aspx?blastgroupID=" + Eval("ID"):"/social.aspx?blastID=" + Eval("ID") %>'></asp:HyperLink>--%>
                                                            <asp:HyperLink ID="hlMediaReporting" runat="server" Text='<%# Eval("ReportImagePath").ToString() %>'
                                                                NavigateUrl=''></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Share" HeaderText="Share" HeaderStyle-Width="40%" ItemStyle-Width="48%"
                                                        Visible="true" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Click" HeaderText="Previews" HeaderStyle-Width="40%" ItemStyle-Width="48%"
                                                        Visible="true" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                                    </asp:BoundField>
                                                </Columns>
                                            </ecnCustom:ecnGridView>
                                        </td>
                                        <td align="right">
                                            <asp:DropDownList ID="dropdownExportShares" runat="server" CssClass="formfield" Visible="false">
                                                <asp:ListItem Value="PDF">PDF</asp:ListItem>
                                                <asp:ListItem Value="Excel">Excel</asp:ListItem>
                                                <asp:ListItem Value="Word">Word</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="ltbnExportShares" runat="server" OnClick="ExportShares_Click" Text="Export"
                                                class="formbuttonsmall" Visible="false"></asp:Button>
                                            <rsweb:ReportViewer ID="ReportViewerShares" runat="server" ShowBackButton="False"
                                                ShowRefreshButton="False" Visible="false">
                                                <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_SocialShares.rdlc">
                                                </LocalReport>
                                            </rsweb:ReportViewer>
                                            <br />
                                            <asp:Label ID="LabelShares" runat="server"></asp:Label>
                                            <asp:Chart ID="chartShares" runat="server" BackColor="Transparent" Height="250px"
                                                Width="350px">
                                                <Series>
                                                    <asp:Series Name="Series1" ChartType="Pie" IsValueShownAsLabel="True" Palette="BrightPastel">
                                                    </asp:Series>
                                                </Series>
                                                <Titles>
                                                    <asp:Title Text="Shares" Docking="Top">
                                                    </asp:Title>
                                                </Titles>
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
                                        <td align="right">
                                            <asp:DropDownList ID="dropdownExportPreviews" runat="server" CssClass="formfield"
                                                Visible="false">
                                                <asp:ListItem Value="PDF">PDF</asp:ListItem>
                                                <asp:ListItem Value="Excel">Excel</asp:ListItem>
                                                <asp:ListItem Value="Word">Word</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="ltbnExportPreviews" runat="server" OnClick="ExportPreviews_Click"
                                                Text="Export" class="formbuttonsmall" Visible="false"></asp:Button>
                                            <rsweb:ReportViewer ID="ReportViewerPreviews" runat="server" ShowBackButton="False"
                                                ShowRefreshButton="False" Visible="false">
                                                <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_SocialPreviews.rdlc">
                                                </LocalReport>
                                            </rsweb:ReportViewer>
                                            <br />
                                            <asp:Label ID="LabelPreviews" runat="server"></asp:Label>
                                            <asp:Chart ID="chartPreviews" runat="server" BackColor="Transparent" Height="250px"
                                                Width="350px">
                                                <Series>
                                                    <asp:Series Name="Series1" ChartType="Pie" IsValueShownAsLabel="True" Palette="BrightPastel">
                                                    </asp:Series>
                                                </Series>
                                                <Titles>
                                                    <asp:Title Text="Previews Per Share" Docking="Top">
                                                    </asp:Title>
                                                </Titles>
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
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr>
                                        <td class="tableHeader" align="left">
                                            Filter Media types&nbsp;
                                            <asp:DropDownList EnableViewState="true" ID="ddlMediaType" runat="Server" class="formfield"
                                                Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMediaType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tableHeader" align="right">
                                            Download View&nbsp;&nbsp;
                                            <asp:DropDownList EnableViewState="true" ID="DownloadType" runat="Server" class="formfield"
                                                Visible="true">
                                                <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                                <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                                <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button class="formbuttonsmall" ID="DownloadButton" runat="Server" Visible="true"
                                                OnClick="DownloadButton_Click" Text="Download"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <%--<asp:datagrid id="PreviewsGrid" runat="Server" width="100%" autogeneratecolumns="False"
                                cssclass="gridWizard">
								<ItemStyle></ItemStyle>
								<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
								<FooterStyle CssClass="tableHeader1"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="BlastID" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn ItemStyle-Width="45%" DataField="DisplayName" HeaderText="Media" ItemStyle-HorizontalAlign="Left"
										HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
									<asp:BoundColumn ItemStyle-Width="45%" DataField="EmailAddress" HeaderText="Who" ItemStyle-HorizontalAlign="Left"
										HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
									<asp:BoundColumn ItemStyle-Width="10%" DataField="Click" HeaderText="Previews" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
								</Columns>
								<AlternatingItemStyle CssClass="gridaltrowWizard" />
							</asp:datagrid>
                            <AU:PagerBuilder ID="PreviewsPager" runat="Server" Width="100%" PageSize="50" 
                                OnIndexChanged="PreviewsPager_IndexChanged">
                                <PagerStyle CssClass="gridpager"></PagerStyle>
                            </AU:PagerBuilder>--%>
                                <ecnCustom:ecnGridView ID="PreviewsGrid" runat="Server" AutoGenerateColumns="False"
                                    onsortcommand="PreviewsGrid_sortCommand" OnRowDataBound="PreviewsGrid_RowDataBound"
                                    PageSize="15" Width="100%" CssClass="gridWizard" datakeyfield="BlastID" DataKeyNames="BlastID"
                                    AllowPaging="True" AllowSorting="true">
                                    <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                    <Columns>
                                        <asp:BoundField DataField="BlastID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="DisplayName" HeaderText="Media" SortExpression="DisplayName"
                                            HeaderStyle-Width="45%" ItemStyle-Width="45%" Visible="true" ItemStyle-HorizontalAlign="left"
                                            HeaderStyle-HorizontalAlign="left"></asp:BoundField>
                                        <asp:BoundField DataField="EmailAddress" HeaderText="Who" SortExpression="EmailAddress"
                                            HeaderStyle-Width="45%" ItemStyle-Width="45%" Visible="true" ItemStyle-HorizontalAlign="left"
                                            HeaderStyle-HorizontalAlign="left"></asp:BoundField>
                                        <asp:BoundField DataField="Click" HeaderText="Previews" SortExpression="Click" HeaderStyle-Width="10%"
                                            ItemStyle-Width="10%" Visible="true" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                    <PagerTemplate>
                                        <table cellpadding="0" border="0">
                                            <tr>
                                                <td align="left" class="label" width="31%">
                                                    Total Records:
                                                    <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                </td>
                                                <td align="left" class="label" width="25%">
                                                    Show Rows:
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PreviewsGrid_SelectedIndexChanged"
                                                        CssClass="formfield">
                                                        <asp:ListItem Value="5" />
                                                        <asp:ListItem Value="10" />
                                                        <asp:ListItem Value="15" />
                                                        <asp:ListItem Value="20" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right" class="label" width="100%">
                                                    Page
                                                    <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged"
                                                        class="formtextfield" Width="30px" />
                                                    of
                                                    <asp:Label ID="lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                    &nbsp;
                                                    <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                        CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                    <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                                                        class="formbuttonsmall" Text="Next >>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                </ecnCustom:ecnGridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
