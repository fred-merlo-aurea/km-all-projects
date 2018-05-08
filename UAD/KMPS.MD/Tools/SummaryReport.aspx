<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="SummaryReport.aspx.cs" Inherits="KMPS.MD.Tools.SummaryReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="100%" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle" width="100%">
                            <table width="100%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </div>
            <table border="0" cellpadding="5" cellspacing="3" width="80%" align="center">
                <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                        <td valign="middle" align="left">
                            <b>Brand
                                <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                                <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                    AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                    DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvBrand" runat="server" ControlToValidate="drpBrand" 
                                    ErrorMessage="*" InitialValue="-1"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hfBrandID" runat="server" Value="0" /></td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td align="left" width="10%">
                        <asp:CheckBox ID="chkRefresh" runat="server" Text=" Refresh Report" Checked="false" />&nbsp;&nbsp;
                        <asp:Button ID="btnDownloadSummary" runat="server" Text="Download" CssClass="button"
                            OnClick="btnDownloadSummary_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GrdReports" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <a href='<%# Bind("FileName") %>' runat="server" id="Category" target="_blank">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FileName") %>'></asp:Label></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                            Width="100%" Height="100%" Visible="False" ShowPageNavigationControls="True">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
