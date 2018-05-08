<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true"
    CodeBehind="Campaign.aspx.cs" Inherits="KMPS.MD.Main.Campaign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register TagName="Download" TagPrefix="CC1" Src="~/Controls/DownloadPanel.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ModalWindow {
            border: solid1px#c0c0c0;
            background: #ffffff;
            padding: 0px10px10px10px;
            position: absolute;
            top: -1000px;
        }

        .modalPopup {
            background-color: transparent;
            padding: 1em 6px;
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
        }
    </style>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
        <ContentTemplate>
            <center>
                <asp:Label ID="lblCampaignID" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblCampaignFilterID" runat="server" Visible="false"></asp:Label>
                <CC1:Download runat="server" ID="DownloadPanel1" Visible="false" ViewType="ConsensusView"></CC1:Download>
                <br />
                <div style="width: 50%; text-align: center;">
                    <div id="divErrorMsg" runat="Server" visible="false" style="width: 674px">
                        <table cellspacing="0" cellpadding="0" width="674px" align="center">
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
                                                <asp:Label ID="lblErrorMsg" runat="Server" ForeColor="Red"></asp:Label>
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
                    <div id="div2" style="width: 50%; float: left; text-align: left; vertical-align:middle; height:30px; line-height: 30px;">
                        <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                            <b>Brand
                            <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                            <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                        </asp:Panel>
                    </div>
                    <div id="div3" style="width: 34%; float: right; padding-left: 1px; text-align: right; vertical-align: middle; height:30px; line-height: 30px;">
                        <asp:Panel ID="pnlExport" runat="server">
                            <asp:DropDownList ID="drpExport" Width="100px" runat="server">
                                <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                                <asp:ListItem Value="xls">Excel</asp:ListItem>
                                <asp:ListItem Value="doc">Word</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnDownload" runat="server" Text="Export" OnClick="btnDownload_Click"
                                CssClass="buttonMedium" Visible="true"></asp:Button>
                            &nbsp;&nbsp;
                        </asp:Panel>
                    </div>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                        Width="1000px" Visible="False">
                    </rsweb:ReportViewer>

                    </br></br>
                 <asp:Panel ID="pnlCampaign" runat="server">
                     <asp:GridView ID="gvCampaign" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                         DataKeyNames="CampaignID" OnRowDataBound="gvCampaign_RowDataBound" RowStyle-BackColor="#EBEBEB"
                         OnRowDeleting="gvCampaign_RowDeleting" OnRowCommand="gvCampaign_RowCommand" OnPageIndexChanging="gvCampaign_PageIndexChanging">
                         <Columns>
                             <asp:TemplateField HeaderText="" HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="right"
                                 ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right">
                                 <ItemTemplate>
                                     <asp:CheckBox ID="chkSelectDownload" runat="server" Text="" Checked="false" />
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:BoundField DataField="CampaignID" Visible="false" HeaderText="" />
                             <asp:BoundField DataField="CampaignName" HeaderText="Campaign Name" HeaderStyle-HorizontalAlign="Left"
                                 ItemStyle-HorizontalAlign="Left">
                                 <HeaderStyle HorizontalAlign="Left" Width="78%" />
                             </asp:BoundField>
                             <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="8%" ItemStyle-Width="8%"
                                 ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                 FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                 <ItemTemplate>
                                     <asp:LinkButton ID="lnkdownloadAll" runat="server" CommandName="download" CommandArgument='<%# Eval("CampaignID") + "/" + Eval("Count") %>'
                                         Text='<%# Eval("Count") %>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                         OnCommand="lnkdownloadAll_Command"></asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="22%" ItemStyle-Width="22%"
                                 HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                 <ItemTemplate>
                                     <asp:LinkButton runat="Server" Text="&lt;img src=../images/icon-delete.gif alt='Delete Content' border='0'&gt;"
                                         CausesValidation="false" ID="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CampaignID") %>'
                                         OnClientClick="return confirm('Are you sure you want to delete?')"></asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField>
                                 <ItemTemplate>
                                     <tr>
                                         <td colspan="100%" align="left">
                                             <div id="div<%# Eval("CampaignID") %>" style="display: block; position: relative; left: 9px; overflow: auto; width: 99%">
                                                 <asp:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlBody"
                                                     ExpandControlID="pnlHeader" CollapseControlID="pnlHeader" Collapsed="true" TextLabelID="pnlLabel"
                                                     ImageControlID="pnlImage" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                     ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                     SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                 <asp:Panel ID="pnlHeader" runat="server" CssClass="collapsePanelHeader" Height="28px" BorderColor="#5783BD">
                                                     <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                                         <div style="float: left;">
                                                             Filters
                                                         </div>
                                                         <div style="float: left; margin-left: 20px;">
                                                             <asp:Label ID="pnlLabel" runat="server">(Show Details...)</asp:Label>
                                                         </div>
                                                         <div style="float: right; vertical-align: middle;">
                                                             <asp:ImageButton ID="pnlImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                                 AlternateText="(Show Details...)" />
                                                         </div>
                                                     </div>
                                                 </asp:Panel>
                                                 <asp:Panel ID="pnlBody" runat="server" CssClass="collapsePanel" Height="0" BorderColor="#5783BD"
                                                     BorderWidth="1">
                                                     <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                                         <tr>
                                                             <td width="50%" valign="top">
                                                                 <asp:GridView ID="gvCampaignFilter" runat="server" AllowPaging="false" AllowSorting="True"
                                                                     AutoGenerateColumns="False" DataKeyNames="CampaignFilterID" OnRowCommand="gvCampaignFilter_RowCommand" 
                                                                     OnRowDeleting="gvCampaignFilter_RowDeleting" OnPageIndexChanging="gvCampaignFilter_PageIndexChanging">
                                                                     <Columns>
                                                                         <asp:BoundField DataField="FilterName" HeaderText="Filter Name" HeaderStyle-HorizontalAlign="Left">
                                                                             <HeaderStyle HorizontalAlign="Left" Width="61%" />
                                                                         </asp:BoundField>
                                                                         <asp:BoundField DataField="PromoCode" HeaderText="PromoCode" HeaderStyle-HorizontalAlign="Left">
                                                                             <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                                         </asp:BoundField>
                                                                         <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                             ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                             FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="right">
                                                                             <ItemTemplate>
                                                                                 <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" CommandArgument='<%# Eval("CampaignID") + "/" + Eval("CampaignFilterID") + "/" + Eval("Count") %>'
                                                                                     Text='<%# Eval("Count") %>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                     OnCommand="lnkdownload_Command"></asp:LinkButton>
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="13%" ItemStyle-Width="13%"
                                                                             HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                             <ItemTemplate>
                                                                                 <asp:LinkButton runat="Server" Text="&lt;img src=../images/icon-delete.gif alt='Delete Content' border='0'&gt;"
                                                                                     CausesValidation="false" ID="lnkCFDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CampaignFilterID") %>'
                                                                                     OnClientClick="return confirm('Are you sure you want to delete?')"></asp:LinkButton>
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                     </Columns>
                                                                 </asp:GridView>
                                                             </td>
                                                         </tr>
                                                     </table>
                                                 </asp:Panel>
                                             </div>
                                         </td>
                                     </tr>
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                 </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
