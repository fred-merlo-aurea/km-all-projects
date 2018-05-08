<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.reportsGraphical"
    CodeBehind="reportsGraphical.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="gradient">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" width="100%" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="DataPanel1" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="&nbsp;&nbsp;REPORT OVERVIEW" Collapsed="False" ExpandText="Click to display Contents List"
                            CollapseText="Click to hide Contents List" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                            ExpandImageUrl="/ecn.images/images/collapse2.gif">
                            <table width="100%">
                                <tr>
                                    <td width="50%" valign="top">
                                        <table cellpadding="1" width="100%" border='0'>
                                            <tr>
                                                <td colspan="2" height="16">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="28%" align='right' valign="top" class="formLabel">
                                                    Email Title:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo" align="left">
                                                    <asp:Label ID="EmailSubject" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' valign="top" class="formLabel">
                                                    Email From:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="EmailFrom" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" valign="top" align='right'>
                                                    Group:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="GroupTo" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" valign="top" align='right'>
                                                    Filter:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="Filter" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" valign="top" align='right'>
                                                    Message:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="Campaign" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" valign="top" align='right'>
                                                    Send Time:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="SendTime" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" valign="top" align='right'>
                                                    Finish Time:
                                                </td>
                                                <td width="5">
                                                    &nbsp;
                                                </td>
                                                <td class="dataTwo">
                                                    <asp:Label ID="FinishTime" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="50%" align='right' valign="top">
                                        <table width="100%" border='0'>
                                            <tr>
                                                <td class="tableContent" colspan="2" align="left">
                                                    <b>Message ROI</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableContent" align="center" bgcolor='#C4C5C7' width="60%">
                                                    <b>PERFORMANCE</b>
                                                </td>
                                                <td class="tableContent" align="center" bgcolor='#C4C5C7' width="40%">
                                                    <b>TOTAL COST</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableContent"  align="left">
                                                    <dotnet:Chart ID="ROI_Chart" runat="Server" Width="280" Height="150" />
                                                </td>
                                                <td align='right' valign="top">
                                                    <table border='0' width="100%">
                                                        <tr>
                                                            <td align='right' class="formLabel" width="65%">
                                                                Emails Sent:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                <asp:Label ID="ROI_EmailsSentLbl" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align='right' class="formLabel">
                                                                Responses [Opens]:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                <asp:Label ID="ROI_TotalResponse" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align='right' class="formLabel">
                                                                Conversions:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                <asp:Label ID="ROI_TotalConversion" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align='right' class="formLabel">
                                                                Cost / Response:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                $<asp:Label ID="ROI_PerResponse" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align='right' class="formLabel">
                                                                Cost / Conversion:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                $<asp:Label ID="ROI_PerClick" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <!--<tr>
														<td class="tableContent">Setup &amp; Fees:</td>
														<td class="tableContent" align='right'>$<asp:label id="ROI_SetupFees" runat="Server"></asp:label></td>
													</tr>-->
                                                        <tr>
                                                            <td align='right' class="formLabel">
                                                                Investment Cost:
                                                            </td>
                                                            <td width="5">
                                                                &nbsp;
                                                            </td>
                                                            <td class="dataTwo" align="left">
                                                                $<asp:Label ID="ROI_TotalInvestment" runat="Server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="10" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="Datapanel2" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="&nbsp;&nbsp;REPORT DETAIL" Collapsed="False" ExpandText="Click to display Contents List"
                            CollapseText="Click to hide Contents List" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                            ExpandImageUrl="/ecn.images/images/collapse2.gif">
                            <table cellpadding="2" width="100%" border='0' style="margin: 10px 0 0 0;">
                                <tr>
                                    <td class="formLabel" align='right' width="20%">
                                        Delivery Rate:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <asp:Label ID="Successful" runat="Server"></asp:Label>
                                    </td>
                                    <td width='3%' align='right' class="dataTwo">
                                        <asp:Label ID="SuccessfulPercentage" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" align='right' width="20%">
                                        Resends:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <span class="dataTwo" style="padding: 0 0 0 8px;">
                                            <asp:Label ID="ResendsUnique" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                    <td class="dataTwo" width="20%">
                                        <asp:Label ID="ResendsPercentage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align='right' width="20%">
                                        Opens:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <asp:Label ID="OpensUnique" runat="Server"></asp:Label>
                                    </td>
                                    <td class="dataTwo" width="3% align='right'">
                                        <asp:Label ID="OpensPercentage" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" align='right' width="20%">
                                        Bounces:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <span class="dataTwo" style="padding: 0 0 0 8px;">
                                            <asp:Label ID="BouncesUnique" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                    <td class="dataTwo" width="20%">
                                        <asp:Label ID="BouncesPercentage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align='right' width="20%">
                                        Clicks:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <asp:Label ID="ClicksUnique" runat="Server"></asp:Label>
                                    </td>
                                    <td class="dataTwo" width="3% align='right'">
                                        <asp:Label ID="ClicksPercentage" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" align='right' width="20%">
                                        Hard Bounces:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <span class="dataTwo" style="padding: 0 0 0 8px;">
                                            <asp:Label ID="HardBouncesUnique" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                    <td class="dataTwo" width="20%">
                                        <asp:Label ID="HardBouncesPercentage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align='right' width="20%">
                                        Unsubscribes:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <asp:Label ID="SubscribesUnique" runat="Server"></asp:Label>
                                    </td>
                                    <td class="dataTwo" width="3% align='right'">
                                        <asp:Label ID="SubscribesPercentage" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" align='right' width="20%">
                                        Soft Bounces:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <span class="dataTwo" style="padding: 0 0 0 8px;">
                                            <asp:Label ID="SoftBouncesUnique" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                    <td class="dataTwo" width="20%">
                                        <asp:Label ID="SoftBouncesPercentage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align='right' width="20%">
                                        Forward to Friend:
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <asp:Label ID="ForwardsUnique" runat="Server"></asp:Label>
                                    </td>
                                    <td class="dataTwo" width="3% align='right'">
                                        <asp:Label ID="ForwardsPercentage" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" align='right' width="20%">
                                        <span class="formLabel">Other Bounces:</span>
                                    </td>
                                    <td class="dataTwo" align="left" width="10%" style="padding: 0 0 0 8px;">
                                        <span class="dataTwo" style="padding: 0 0 0 8px;">
                                            <asp:Label ID="UnknownBouncesUnique" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                    <td class="dataTwo" width="20%">
                                        <span class="dataTwo">
                                            <asp:Label ID="UnknownBouncesPercentage" runat="Server"></asp:Label>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="10" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="Datapanel4" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="&nbsp;&nbsp;RESPONSE DETAIL" Collapsed="False" ExpandText="Click to display Contents List"
                            CollapseText="Click to hide Contents List" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                            ExpandImageUrl="/ecn.images/images/collapse2.gif">
                            <table cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td class="tableContent" align="middle" colspan='3'>
                                        <br />
                                        <dotnet:Chart ID="ResponseDetailChart" runat="Server" Width="675%" Height="400" />
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="10" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="Datapanel6" Style="z-index: 101" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                            CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Contents List"
                            ExpandText="Click to display Contents List" Collapsed="False" TitleText="&nbsp;&nbsp;CLICKS DETAIL"
                            AllowTitleExpandCollapse="True">
                            <table cellspacing="0" cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td valign="top" width="100%">
                                        <div align='right' style="padding: 10px 0;">
                                            <asp:DropDownList ID="ClickSelectionDD" runat="Server" CssClass="formfield" OnSelectedIndexChanged="ClickSelectionDD_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="TOP 10" Selected="True">Top 10 Clicks</asp:ListItem>
                                                <asp:ListItem Value="TOP 20">Top 20 Clicks</asp:ListItem>
                                                <asp:ListItem Value="TOP 30">Top 30 Clicks</asp:ListItem>
                                                <asp:ListItem Value="TOP 40">Top 40 Clicks</asp:ListItem>
                                                <asp:ListItem Value="TOP 50">Top 50 Clicks</asp:ListItem>
                                                <asp:ListItem Value="">All Clicks</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:DataGrid ID="ClicksGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="gridWizard">
                                            <ItemStyle></ItemStyle>
                                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                            <Columns>
                                                <asp:BoundColumn HeaderText="Total Clicks" DataField="ClickCount" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundColumn HeaderText="Unique Clicks" DataField="distinctClickCount" ItemStyle-Width="12%"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundColumn HeaderText="URL / Link Alias" DataField="NewActionValue" ItemStyle-Width="70%"
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <asp:TemplateColumn HeaderText="Download" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href="clicks.aspx?BlastID=<%# getBlastID().ToString() %>&action=report&actionURL=<%# DataBinder.Eval(Container.DataItem, "NewActionValue")%>">
                                                            Report</a>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <AU:PagerBuilder ID="ClicksPager" runat="Server" Width="100%" PageSize="10" ControlToPage="ClicksGrid">
                                            <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                        </AU:PagerBuilder>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="10" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="Datapanel3" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="&nbsp;&nbsp;BOUNCE DETAIL" Collapsed="False" ExpandText="Click to display Contents List"
                            CollapseText="Click to hide Contents List" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                            ExpandImageUrl="/ecn.images/images/collapse2.gif">
                            <table cellspacing="0" cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td class="tableContent" align="left" width="50%">
                                        <b>Hard Bounces</b>
                                    </td>
                                    <td class="tableContent" align="left" width="50%">
                                        <b>Soft Bounces</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="50%">
                                        <asp:DataGrid ID="HardBounceGrid" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False"
                                            Width="100%">
                                            <ItemStyle></ItemStyle>
                                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                            <Columns>
                                                <asp:BoundColumn ItemStyle-Width="75%" DataField="ActionNotes" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderText=""></asp:BoundColumn>
                                                <asp:BoundColumn ItemStyle-Width="25%" DataField="BounceCount" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderText=""></asp:BoundColumn>
                                            </Columns>
                                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                        </asp:DataGrid>
                                    </td>
                                    <td valign="top" width="50%">
                                        <asp:DataGrid ID="SoftBounceGrid" runat="Server" AutoGenerateColumns="False" CssClass="gridWizard"
                                            Width="100%">
                                            <ItemStyle></ItemStyle>
                                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                            <Columns>
                                                <asp:BoundColumn ItemStyle-Width="75%" DataField="ActionNotes" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderText=""></asp:BoundColumn>
                                                <asp:BoundColumn ItemStyle-Width="25%" DataField="BounceCount" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderText=""></asp:BoundColumn>
                                            </Columns>
                                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="10" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="greySidesB offWhite blastLinksTwo">
                    <div class="moveUp">
                        <cpanel:DataPanel ID="Datapanel5" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="&nbsp;&nbsp;PROJECTED COSTS" Collapsed="False" ExpandText="Click to display Contents List"
                            CollapseText="Click to hide Contents List" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                            ExpandImageUrl="/ecn.images/images/collapse2.gif">
                            <table cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td class="tableContent" align="left" width="50%">
                                        <b>Setup &amp; Fees</b>
                                    </td>
                                    <td class="tableContent" align="left" width="50%">
                                        <b>Message Fees</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableContent" align="left">
                                        <table style="margin: 0 0 10px 50px;">
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Setup Cost:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="SetupSetupCostLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Outbound Costs:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="OutboundCostLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Inbound Costs:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="InboundCostLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Design Costs:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="DesignCostLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Other Costs:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="OtherCostLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="tableContent" align="left">
                                        <table style="padding: 0 0 0 50px;">
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Total Emails sent:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    <asp:Label ID="EmailsSentLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Per Email Charge:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="PerEmailChargeLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Total Responses:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    <asp:Label ID="ResponsesLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Cost per Response:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="PerResponseLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' class="formLabel">
                                                    Cost per Click:
                                                </td>
                                                <td width="5">
                                                </td>
                                                <td class="dataTwo" align="left" width="70">
                                                    $<asp:Label ID="PerClickLbl" runat="Server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableContent" bgcolor="#cc0000">
                                        <b><font color="#ffffff" style="padding-left: 25px;">Total Setup &amp; Fees: &nbsp;&nbsp;&nbsp;&nbsp;$<asp:Label
                                            ID="TotalSetupLbl" runat="Server"></asp:Label></font></b>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <br />
</asp:Content>
