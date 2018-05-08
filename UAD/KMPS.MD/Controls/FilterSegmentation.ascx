<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterSegmentation.ascx.cs" Inherits="KMPS.MD.Controls.FilterSegmentation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Venn" TagPrefix="uc" Src="~/Controls/FilterVennDiagram.ascx" %>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="overlay">
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10002; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<div id="divMessage" runat="Server" visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorMiddle">
                <table width="80%">
                    <tr>
                        <td valign="middle" align="left" width="80%" height="100%">
                            <asp:Label ID="lblMessage" runat="Server" Font-Bold="true" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="grdFilterSegmentation" />
        <asp:PostBackTrigger ControlID="grdFilterSegmentationCounts" />
        <asp:AsyncPostBackTrigger ControlID="btnFilterSegmentation" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnlFilterSegmentation" Visible="false">
            <table cellpadding="5" cellspacing="5" border="0" width="100%">
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="grdFilterSegmentation" runat="server" Width="100%" AutoGenerateColumns="False"
                            Height="100%" OnRowDataBound="grdFilterSegmentation_RowDataBound"
                            ShowFooter="true" AllowPaging="false" DataKeyNames="FilterNo">
                            <Columns>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFilterGroupName" runat="server" Text='<%# Eval("FilterGroupName") %>' Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblFilterGroupID" runat="server" Text='<%# Eval("FilterGroupID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBrandID" runat="server" Text='<%# Eval("BrandID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filters"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlResultHeader" runat="server" Height="28px" CssClass="collapsePanelHeader"
                                            BackColor="#eeeeee" ForeColor="Black">
                                            <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                                <div style="float: left;">
                                                </div>
                                                <div style="float: left; margin-left: 20px;">
                                                    <asp:Label ID="pnlResultLabel" runat="server">(Show Filters...)</asp:Label>
                                                </div>
                                                <div style="float: right; vertical-align: middle;">
                                                    <asp:ImageButton ID="pnlResultImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                        AlternateText="(Show Filters...)" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlResultBody" runat="server" CssClass="collapsePanel" Height="0"
                                            BorderColor="#eeeeee" BorderWidth="2">
                                            <div style="width: 1150px; overflow-x:auto;">
                                            <asp:GridView ShowHeader="false" ID="grdFilterValues" Width="100%" runat="server"
                                                AutoGenerateColumns="False" GridLines="Both">
                                                <Columns>
                                                    <asp:BoundField DataField="name" HeaderText="Column">
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="right" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            =
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="77%" ItemStyle-Width="77%" ItemStyle-HorizontalAlign="left"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFilterText" runat="server" Text='<%# Eval("Text") %>'></asp:Label>
                                                            <asp:Label ID="lblAdhocCondition" runat="server" Text='<%# Eval("name").ToString() == "Adhoc" ?  " - " + Eval("SearchCondition") + " - " + Eval("Values") : "" %>'></asp:Label>
                                                            <asp:Label ID="lblFilterValues" runat="server" Text='<%# Eval("Values") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblFiltername" runat="server" Text='<%# Eval("name") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblSearchCondition" runat="server" Text='<%# Eval("SearchCondition") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblFilterType" runat="server" Text='<%# Eval("FilterType") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpeDemo3" runat="Server" TargetControlID="pnlResultBody"
                                            ExpandControlID="pnlResultHeader" CollapseControlID="pnlResultHeader" Collapsed="true"
                                            TextLabelID="pnlResultLabel" ImageControlID="pnlResultImage" ExpandedText="(Hide Filters...)"
                                            CollapsedText="(Show Filters...)" ExpandedImage="~/images/collapse_blue.jpg"
                                            CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCounts" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="5" cellspacing="5" border="0" width="70%">
                            <tr>
                                <td width="5%" style="vertical-align: middle">
                                    <b>Operation</b>
                                </td>
                                <td style="vertical-align: middle" width="16%">
                                    <asp:RadioButtonList ID="rblSelectedFilterOperation" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Union" Text=" Union" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Intersect" Text=" Intersect"></asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td style="vertical-align: middle">
                                    <asp:RequiredFieldValidator ID="rvSelectedFilterOperation" runat="server" ControlToValidate="rblSelectedFilterOperation" ValidationGroup="load" Display="Dynamic"
                                        ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                    <td>
                        <table cellpadding="5" cellspacing="5" border="0" width="70%">
                            <tr>
                                <td width="5%" style="vertical-align: middle">
                                    <b>Operation</b>
                                </td>
                                <td style="vertical-align: middle" width="16%">
                                    <asp:RadioButtonList ID="rblSuppressedFilterOperation" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Union" Text=" Union" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Intersect" Text=" Intersect"></asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td style="vertical-align: middle">
                                    <asp:RequiredFieldValidator ID="rvSuppressedFilterOperation" runat="server" ControlToValidate="rblSuppressedFilterOperation" ValidationGroup="load" Display="Dynamic"
                                        ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="31%" align="center"><b>Available Filters - In</b>
                    </td>
                    <td width="5%"></td>
                    <td width="31%" align="center"><b>Available Filters - Not In(Optional)</b></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstSelectedFilters" runat="server" Rows="10" Style="text-transform: uppercase;"
                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="150px"
                            EnableViewState="True"></asp:ListBox>
                        <asp:RequiredFieldValidator ID="rvSelectedFilters" runat="server" ControlToValidate="lstSelectedFilters" Display="static" ValidationGroup="load"
                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                    <td>
                        <asp:ListBox ID="lstSuppressedFilters" runat="server" Rows="10" Style="text-transform: uppercase;"
                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="150px"></asp:ListBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnFilterSegmentation" runat="server" Text="Load Segmentation" CssClass="buttonMedium" OnClick="btnFilterSegmentation_Click" ValidationGroup="load" />
                        <asp:Button ID="Button1" runat="server" Text="Reset" CssClass="buttonMedium" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divErrMsg" runat="Server" visible="false">
                            <font color="red">
                                <asp:Label ID="lblErrMsg" runat="Server"></asp:Label></font>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top">
                        <asp:GridView ID="grdFilterSegmentationCounts" runat="server" Width="100%" AutoGenerateColumns="False"
                            Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="FilterViewNo"  OnRowDeleting="grdFilterSegmentationCounts_RowDeleting">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="FilterViewNo" HeaderStyle-Width="2%"
                                    ItemStyle-Width="2%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                    FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left"  Visible="false"/>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFilterViewName" runat="server" Text='<%# Eval("FilterViewName")%>' Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Data Segments Included" DataField="FilterDescription" HeaderStyle-Width="38%"
                                    ItemStyle-Width="38%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                    FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="8%" ItemStyle-Width="6%"
                                    ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                    FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFilterViewNo" runat="server" Value='<%# Eval("FilterViewNo") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfFilterViewName" runat="server" Value='<%# Eval("FilterViewName") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfFilterDescription" runat="server" Value='<%# Eval("FilterDescription") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfSelectedFilterNo" runat="server" Value='<%# Eval("SelectedFilterNo") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfSuppressedFilterNo" runat="server" Value='<%# Eval("SuppressedFilterNo") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfSelectedFilterOperation" runat="server" Value='<%# Eval("SelectedFilterOperation") %>' Visible="false"></asp:HiddenField>
                                        <asp:HiddenField ID="hfSuppressedFilterOperation" runat="server" Value='<%# Eval("SuppressedFilterOperation") %>' Visible="false"></asp:HiddenField>
                                        <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")  + "/" + "" + "/" + Eval("Count")%>'
                                            Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                            OnCommand="lnkCount_Command"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ButtonType="Link" Text="<img src='../images/icon-delete.gif' style='border:none;'>"
                                    CommandName="Delete" HeaderText="Remove" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                <asp:TemplateField HeaderText="Save" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                    ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelectFilter" runat="server" Text="" Checked="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                    ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                    FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                            <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                    <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                        PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                        <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                            <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                    Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>'
                                                    OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                    Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>'
                                                    OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                    Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>'
                                                    OnCommand="lnkGeoReport_Command" />
                                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                    CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""%>' OnCommand="lnkGeoMaps_Command" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                    <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                        PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                        <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + ""  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="text-align:right">
                            <br />
                            <asp:Button ID="btnSaveFilterSegmentation" runat="server" CssClass="buttonMedium" OnClick="btnOpenSaveFSPopup_Click" Text="Save Segmentation" visible="false"/>
                        </div>
                    </td>
                    <td width="25%" align="center" valign="top">
                        <uc:Venn runat="server" ID="ctrlVenn"></uc:Venn>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>