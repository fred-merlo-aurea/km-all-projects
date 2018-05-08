<%@ Reference Control="~/includes/uploader.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageSelector.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.ImageSelector" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="~/includes/uploader.ascx" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>

<script type="text/javascript" src="/ecn.accounts/js/overlib/overlib.js"></script>



<div id="overDiv" style="z-index: 1000; visibility: hidden; position: absolute">
</div>
<table cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="gradient">
                <tr>
                    <td width="40%">
                    </td>
                    <td width="60%" valign="middle" align="right" nowrap height="22">
                        <asp:Panel ID="PanelTabs" Visible="true" Style="display: inline" runat="server" Height="22">
                            <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                                padding-top: 0px" align="right">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top" align="right" width="15">
                                            <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/browse_img.gif"
                                                border="0"></td>
                                        <td valign="bottom" width="118">
                                            <h3 class="blackBtn">
                                                <asp:LinkButton ID="tabBrowse" Style="padding-right: 0px; padding-left: 20px; padding-bottom: 0px;
                                                    cursor: pointer; padding-top: 0px" OnClick="showBrowse" Text="" runat="server">
													<span>Browse Images</span></asp:LinkButton></h3>
                                        </td>
                                        <td valign="top" align="right" width="15">
                                            <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/upload_img.gif"
                                                border="0"></td>
                                        <td valign="bottom" width="115">
                                            <h3 class="blackBtn">
                                                <asp:LinkButton ID="tabUpload" Style="padding-right: 0px; padding-left: 20px; padding-bottom: 0px;
                                                    cursor: pointer; padding-top: 0px" OnClick="showUpload" Text="" runat="server">
													<span>Upload Images</span></asp:LinkButton></h3>
                                        </td>
                                        <asp:Panel ID="tabDeleteImagesPanel" Style="display: inline" runat="server">
                                            <td valign="top" align="right" width="15">
                                                <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/delete.gif"
                                                    border="0"></td>
                                            <td valign="top" width="115">
                                                <h3 class="blackBtn">
                                                    <asp:LinkButton ID="tabDeleteImages" Style="padding-right: 0px; padding-left: 20px;
                                                        padding-bottom: 0px; cursor: pointer; padding-top: 0px" OnClick="deleteSelectedImages"
                                                        Text="" runat="server">
														<span>Delete Images</span></asp:LinkButton></h3>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="PanelBrowse" Visible="true" BackColor="White" runat="server" BorderWidth="0">
        <tr>
            <td class="offWhite">
                <table style="border-right: #b6bcc6 1px solid; border-left: #b6bcc6 1px solid" cellspacing="0"
                    cellpadding="0" width="100%" border="0">
                    <tr>
                        <td style="padding-right: 7px; padding-left: 7px; padding-bottom: 2px; padding-top: 2px"
                            valign="top" align="center" width="25%">
                            <table class="tableContent" cellspacing="0" width="100%" align="left" border="0">
                                <asp:Label ID="FolderSrc" runat="server" Text="Name"></asp:Label></table>
                        </td>
                        <td style="border-left: #ccc 1px solid" valign="top" align="right" width="75%">
                            <table width="100%" border="0">
                                <tr>
                                    <td class="tablecontent" nowrap align="right" width="40%">
                                        <span class="highLightOne">List Images by:</span>
                                        <td class="tablecontent" style="padding-right: 5px" nowrap align="right" width="25%">
                                            <asp:RadioButtonList ID="ImgListViewRB" runat="server" CssClass="tableContent" RepeatDirection="Horizontal"
                                                AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ImgListViewRB_SelectedIndexChanged">
                                                <asp:ListItem Value="DETAILS" Selected><IMG src="/ecn.images/images/view_detail.gif" border="0" style="position:relative;top:3px;">&nbsp;Details</asp:ListItem>
                                                <asp:ListItem Value="THUMBNAILS"><IMG src="/ecn.images/images/view_thumbnails.gif" border="0" style="position:relative;top:3px;">&nbsp;Thumbnails</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td class="pageLinks" style="padding-right: 5px" nowrap width="20%">
                                            <!--<a href="#">50</a> | <a href="#">100</a> | <a href="#">150</a> | <a href="#" class="selected">200</a> images / page -->
                                            <!--<div style="display:none"> &nbsp;with&nbsp;-->
                                            <asp:DropDownList class="formfield" ID="ImagesToShowDR" runat="server" AutoPostBack="true"
                                                Visible="true" EnableViewState="true" OnSelectedIndexChanged="ImagesToShowDR_SelectedIndexChanged">
                                                <asp:ListItem Value="50" Selected>50</asp:ListItem>
                                                <asp:ListItem Value="100">100</asp:ListItem>
                                                <asp:ListItem Value="150">150</asp:ListItem>
                                                <asp:ListItem Value="200">200</asp:ListItem>
                                            </asp:DropDownList>&nbsp;Images / page
                                            <!--</div>-->
                                        </td>
                                </tr>
                                <asp:Panel ID="ImageListGridPanel" runat="server" Visible="False">
                                    <tr>
                                        <td style="padding-right: 5px" align="right" colspan="3">
                                            <asp:DataGrid ID="ImageListGrid" runat="server" CssClass="gridWizard" OnSortCommand="ImageList_Sort"
                                                AllowSorting="True" Width="97%" AutoGenerateColumns="False" AllowPaging="True"
                                                PagerStyle-PrevPageText="<" PagerStyle-NextPageText=">" PagerStyle-Mode="NumericPages"
                                                PagerStyle-HorizontalAlign="Right" PageSize="50">
                                                <ItemStyle HorizontalAlign="center" Height="20"></ItemStyle>
                                                <ItemStyle HorizontalAlign="Center" Height="20px"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" BorderWidth="1" BorderColor="#A4A2A3" Height="23px"
                                                    CssClass="tableHeader gridHeaderWizard"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn ItemStyle-Width="0%" DataField="ImageKey" Visible="false"></asp:BoundColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                           <a href="javascript:void(0)" onclick="getit('<%#DataBinder.Eval(Container.DataItem,"ImagePath")%>')"><img src='/ecn.images/images/icon-add.gif' border=0 ></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn ItemStyle-Width="45%" DataField="ImageName" HeaderText="<sub><img src=/ecn.images/images/sort_btn.gif border=0 /></sub>&nbsp;NAME"
                                                        SortExpression="ImageName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="15%" DataField="ImageSize" HeaderText="<sub><img src=/ecn.images/images/sort_btn.gif border=0 /></sub>&nbsp;SIZE"
                                                        SortExpression="ImageSizeRaw"></asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="15%" DataField="ImageType" HeaderText="<sub><img src=/ecn.images/images/sort_btn.gif border=0 /></sub>&nbsp;TYPE"
                                                        SortExpression="ImageType"></asp:BoundColumn>
                                                    <asp:BoundColumn ItemStyle-Width="20%" DataField="ImageDtModified" HeaderText="<sub><img src=/ecn.images/images/sort_btn.gif border=0 /></sub>&nbsp;DATE MODIFIED"
                                                        DataFormatString="{0:MM/dd/yyyy HH:mm:ss tt}" SortExpression="ImageDtModified"></asp:BoundColumn>
                                                </Columns>
                                                <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                            </asp:DataGrid></td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="ImageListRepeaterPanel" runat="server" Visible="False">
                                    <tr>
                                        <td style="padding-right: 5px" align="right" colspan="3">
                                            <asp:DataList ID="ImageListRepeater" runat="server" BorderWidth="0" RepeatLayout="Table"
                                                RepeatColumns="4" GridLines="none" BorderColor="black" CellPadding="0" CellSpacing="0">
                                                <ItemTemplate>
                                                    <table class="tableContent" align="center" cellpadding="0" style="width: 175px;">
                                                        <tr>
                                                            <td valign="middle" align="center" style="width: 175px; border: 1px solid #cccccc">
                                                                <div style="width: 175px; border: 0">
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td height="110" width="175" align="center" colspan="2">
                                                                                <%#DataBinder.Eval(Container.DataItem,"ImageDIV")%>
                                                                                <asp:ImageButton runat="server" ID="BrowseImage" ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Image")%>'>
                                                                                </asp:ImageButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="155" align="center" class="tablecontent" nowrap>
                                                                <%#DataBinder.Eval(Container.DataItem,"ImageName")%>
                                                            </td>
                                                            <td width="20" valign="top" align="left">
                                                                <a href="javascript:void(0)" onclick="getit('<%#DataBinder.Eval(Container.DataItem,"ImageKey")%>')"><img src='/ecn.images/images/icon-add.gif' border=0 ></a>
                                                                <!--<asp:CheckBox ID="DLChkBxImages" runat="server"></asp:CheckBox>-->
                                                                <asp:TextBox ID="ImageKeyLbl" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"ImageKey")%>'>
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div> </td> </tr> </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <AU:PagerBuilder ID="ImageListRepeaterPager" runat="server" Width="95%" ControlToPage="ImageListRepeater"
                                                OnIndexChanged="ImageListRepeaterPager_IndexChanged">
                                                <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                            </AU:PagerBuilder>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                            <asp:TextBox ID="imagepath" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="imagesize" runat="server" Visible="False">100</asp:TextBox><br>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="SocialPanelUpload" BackColor="White" Visible="false" runat="server">
        <tr>
            <td colspan="2">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="grd_panel_borders" align="center">
                            <ecn:uploader ID="pageuploader" runat="server" uploadDirectory="e:\\http\\dev\\ECNblaster\\assets\\eblaster\\customers\\1\\images\\">
                            </ecn:uploader>
                            <br>
                            <br>
                            <br>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="gradient">
                <tr>
                    <td width="40%">
                    </td>
                    <td width="60%" valign="middle" align="right" nowrap height="22">
                        <asp:Panel ID="PanelTabsBottom" Visible="true" Style="display: inline" runat="server"
                            Height="22">
                            <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                                padding-top: 0px" align="right">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top" align="right" width="15">
                                            <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/browse_img.gif"
                                                border="0"></td>
                                        <td valign="top" width="118">
                                            <h3 class="blackBtn">
                                                <asp:LinkButton ID="tabBrowseBottom" Style="padding-right: 0px; padding-left: 20px;
                                                    padding-bottom: 0px; cursor: pointer; padding-top: 0px" OnClick="showBrowse"
                                                    Text="" runat="server">
													<span>Browse Images</span></asp:LinkButton></h3>
                                        </td>
                                        <td valign="top" align="right" width="15">
                                            <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/upload_img.gif"
                                                border="0"></td>
                                        <td valign="top" width="115">
                                            <h3 class="blackBtn">
                                                <asp:LinkButton ID="tabUploadBottom" Style="padding-right: 0px; padding-left: 20px;
                                                    padding-bottom: 0px; cursor: pointer; padding-top: 0px" OnClick="showUpload"
                                                    Text="" runat="server">
													<span>Upload Images</span></asp:LinkButton></h3>
                                        </td>
                                        <asp:Panel ID="tabDeleteImagesPanelBottom" Style="display: inline" runat="server">
                                            <td valign="top" align="right" width="15">
                                                <img style="left: 25px; position: relative; top: 4px" src="/ecn.images/images/delete.gif"
                                                    border="0"></td>
                                            <td valign="top" width="115">
                                                <h3 class="blackBtn">
                                                    <asp:LinkButton ID="tabDeleteImagesBottom" Style="padding-right: 0px; padding-left: 20px;
                                                        padding-bottom: 0px; cursor: pointer; padding-top: 0px" OnClick="deleteSelectedImages"
                                                        Text="" runat="server">
														<span>Delete Images</span></asp:LinkButton></h3>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
        
