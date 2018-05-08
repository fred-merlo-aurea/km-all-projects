<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KMPS.MD.Main.Default" MasterPageFile="../MasterPages/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="cr" %>



<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   <%-- <div>
        <asp:Panel ID="GlobalFiltersPanel" runat="server" BackColor="LightGray" Width="100%">
            <table width="100%" cellpadding="5px" cellspacing="5px">
                <tr>
                    <td align="left" valign="top" style="width: 15%">
                        <asp:Panel ID="whichFilterPanel" HorizontalAlign="Left" runat="server" BackColor="Silver">
                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Medium">Choose your Filters:</asp:Label>
                            <asp:CheckBoxList ID="whichfilters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="displayFilter">
                                <asp:ListItem Text="Pubs/Tradeshow" Value="Pubs/Tradeshow"></asp:ListItem>
                                <asp:ListItem Text="Category" Value="Category"></asp:ListItem>
                                <asp:ListItem Text="Category Codes" Value="Category Codes"></asp:ListItem>
                                <asp:ListItem Text="Transaction" Value="Transaction"></asp:ListItem>
                                <asp:ListItem Text="Qualification Source" Value="Qualification Source"></asp:ListItem>
                                <asp:ListItem Text="Contact Fields" Value="Contact Fields"></asp:ListItem>
                                <asp:ListItem Text="Other Fields" Value="Other Fields"></asp:ListItem>
                                <asp:ListItem Text="Ad Hoc" Value="Ad Hoc"></asp:ListItem>
                                <asp:ListItem Text="Date Range" Value="Date Range"></asp:ListItem>
                                <asp:ListItem Text="GeoCode" Value="GeoCode"></asp:ListItem>
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:RoundedCornersExtender ID="rceFilterPanel" runat="server" TargetControlID="whichFilterPanel"
                            Color="Silver" Radius="5" Corners="All" />
                    </td>
                    <td align="left" valign="top">
                        <asp:Table ID="filterTable" runat="server" EnableViewState="false">
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="ButtonPanel" HorizontalAlign="Center" runat="server">
                <asp:RadioButton Checked="true" ID="consensusRadio1" runat="server" Text="Record Consensus"
                    GroupName="consensusRadio" />
                <asp:RadioButton ID="consensusRadio2" runat="server" Text="Company Consensus" GroupName="consensusRadio" />
                <br />
                <asp:DropDownList ID="drpReportFor" runat="server" CssClass="formfield" DataValueField="ResponseGroup"
                    DataTextField="ResponseGroup">
                </asp:DropDownList>
                <asp:Button ID="btnRecalculatecounts" Text="Apply Filters" runat="server" CssClass="button"
                    OnClick="btnReload_Click"></asp:Button>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnReset" Text="Reset Filters" runat="server" CssClass="button" OnClick="btnReset_Click">
                </asp:Button>
                &nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="jttest" runat="server" Visible="false"></asp:TextBox>
            </asp:Panel>
        </asp:Panel>
        <asp:RoundedCornersExtender ID="rce" runat="server" TargetControlID="GlobalFiltersPanel"
            Color="LightGray" Radius="6" Corners="All" />
        <br />
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" align="left" colspan="3">
                    <table style="border-right: #dde4e8 2px solid; border-top: #dde4e8 2px solid; border-left: #dde4e8 2px solid;
                        border-bottom: #dde4e8 2px solid" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr class="gridheader">
                            <td align="left">
                                <span style="font-weight: bold; font-size: 8pt; color: #ffffff; font-style: normal;
                                    font-family: Arial">Report</span>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="drpExport" Width="100" runat="server">
                                    <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                                    <asp:ListItem Value="xls">Excel</asp:ListItem>
                                    <asp:ListItem Value="doc">Word</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click"
                                    Visible="true"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btndownloaddetails" runat="server" Text="Download Details" OnClick="btndownloaddetails_Click"
                                    Visible="false"></asp:Button>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                <cr:CrystalReportViewer ID="crv" runat="server" Width="100%" SeparatePages="False"
                                    EnableDrillDown="False" DisplayToolbar="False">
                                </cr:CrystalReportViewer>
                            </td>
                        </tr>
                    </table>
                    <br>
                </td>
            </tr>
        </table>
    </div>--%>
</asp:Content>

