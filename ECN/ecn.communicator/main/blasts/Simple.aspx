<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.Simple" CodeBehind="Simple.aspx.cs"
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
                                            Simple Sharing
                                        </td>
                                        <td align="right" class="tableHeader" width="70%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <br />                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width:100%;">
                                   <tr>
                                       <td style="text-align:center;">
                                           <asp:GridView ID="gvCharts" runat="server" GridLines="None" OnRowDataBound="gvCharts_RowDataBound" AutoGenerateColumns="false">
                                               <Columns>
                                                   <asp:TemplateField>
                                                       <ItemTemplate>
                                                           <asp:Chart ID="SocialChart" BackImageTransparentColor="White" runat="server" />
                                                           <asp:Image ID="imgNoResults" runat="server" Visible="false" ImageUrl="/ecn.images/images/NoDataWhite.png" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                           </asp:GridView>
                                       </td>
                                   </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

