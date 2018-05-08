<%@ Reference Control="~/includes/uploader.ascx" %>
<%@ Control Language="c#" Inherits="ecn.editor.includes.imageGallery" Codebehind="imageGallery.ascx.cs" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="uploader.ascx" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/overlib/overlib.js"></script>

<div id="overDiv" style="Z-INDEX:1000; VISIBILITY:hidden; POSITION:absolute"></div>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td>
			<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0" class="gradient">
				<TR>
					<td valign="middle" align="right" noWrap height="22"><asp:Panel ID="PanelTabs" Visible="true" style="DISPLAY:inline" Runat="server" Height="22"><IMG style="POSITION: relative; TOP: 3px" src="/ecn.images/images/browse_img.gif" border="0">
						  <asp:LinkButton ID="tabBrowse" OnClick="showBrowse" Text="" runat="server">Browse Images</asp:LinkButton>
						  &nbsp; |&nbsp; <IMG style="POSITION: relative; TOP: 3px" src="/ecn.images/images/upload_img.gif" border="0"> 
							<asp:linkbutton id="tabUpload" onclick="showUpload" Text="" runat="server">Upload Images</asp:linkbutton>&nbsp;
						</asp:Panel></td>
				</TR>
			</TABLE>
	  </td>
	</tr>
	<asp:Panel ID="PanelBrowse" Visible="true" Runat="server" BorderWidth="0">
		<TR>
			<TD class="offWhite">
				<TABLE style="BORDER-RIGHT: #b6bcc6 1px solid; BORDER-LEFT: #b6bcc6 1px solid;" cellSpacing="0" 
					cellPadding="0" width="100%" border="0">
					<TR>
						<TD style="PADDING:2px 7px;background:#fff;" vAlign="top" align="center" width="25%">
							<TABLE class="tableContent" cellSpacing="0" width="100%" align="left" border="0">
								<asp:Label id="FolderSrc" Runat="server" Text="Name"></asp:Label></TABLE>
						</TD>
						<TD style="BORDER-LEFT: #ccc 1px solid" vAlign="top" align="right" width="75%">
							<TABLE width="100%" border="0">
								<TR>
									<TD class="tablecontent" noWrap align="right" width="40%"><SPAN class="highLightOne">List 
											Images by:</SPAN>
									<TD class="tablecontent" style="PADDING-RIGHT: 5px" noWrap align="right" width="25%">
										<asp:RadioButtonList id="ImgListViewRB" runat="server" RepeatDirection="Horizontal"
											AutoPostback="true" Width="100%" onselectedindexchanged="ImgListViewRB_SelectedIndexChanged">
											<asp:ListItem Value="DETAILS" Selected><IMG src="/ecn.images/images/view_detail.gif" border="0" style="position:relative;top:3px;">&nbsp;Details</asp:ListItem>
											<asp:ListItem Value="THUMBNAILS"><IMG src="/ecn.images/images/view_thumbnails.gif" border="0" style="position:relative;top:3px;">&nbsp;Thumbnails</asp:ListItem>
										</asp:RadioButtonList></TD>
									<TD class="pageLinks" style="PADDING-RIGHT: 5px" noWrap width="20%"><!--<a href="#">50</a> | <a href="#">100</a> | <a href="#">150</a> | <a href="#" class="selected">200</a> images / page -->  <!--<div style="display:none"> &nbsp;with&nbsp;-->
										<asp:DropDownList class="formfield" id="ImagesToShowDR" runat="server" AutoPostback="true" visible="true"
											EnableViewState="true" onselectedindexchanged="ImagesToShowDR_SelectedIndexChanged">
											<asp:ListItem Value="50" Selected>50</asp:ListItem>
											<asp:ListItem Value="100">100</asp:ListItem>
											<asp:ListItem Value="150">150</asp:ListItem>
											<asp:ListItem Value="200">200</asp:ListItem>
										</asp:DropDownList>&nbsp;Images / page<!--</div>-->
									</TD>
								</TR>
								<asp:Panel id="ImageListGridPanel" Runat="server" Visible="False">
									<TR>
										<TD style="PADDING-bottom:10px;" align="center" colSpan="3">
											<asp:datagrid id="ImageListGrid" runat="server" CssClass="gridWizard" OnSortCommand="ImageList_Sort"
												AllowSorting="True" width="97%" BackColor="#ffffff" AutoGenerateColumns="False" AllowPaging="True" PagerStyle-PrevPageText="<"
												PagerStyle-NextPageText=">" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Right"
												PageSize="50">
												<ItemStyle HorizontalAlign="center"></ItemStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderStyle HorizontalAlign="Center" CssClass="gridheaderWizard"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="ImageKey" Visible="false"></asp:BoundColumn>
													<asp:TemplateColumn  HeaderText="INSERT" ItemStyle-Width="10%">
														<ItemTemplate>
															<a href="javascript:void(0)" onclick=JavaScript:getit('<%#DataBinder.Eval(Container.DataItem,"ImagePath")%>')><img src='/ecn.images/images/icon-add.gif' border=0 ></a>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn ItemStyle-Width="35%" DataField="ImageName" HeaderText="<h5>NAME</h5>"
														SortExpression="ImageName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
													<asp:BoundColumn ItemStyle-Width="15%" DataField="ImageSize" HeaderText="<h5>SIZE</h5>"
														SortExpression="ImageSizeRaw"></asp:BoundColumn>
													<asp:BoundColumn ItemStyle-Width="15%" DataField="ImageType" HeaderText="<h5>TYPE</h5>"
														SortExpression="ImageType"></asp:BoundColumn>
													<asp:BoundColumn ItemStyle-Width="25%" DataField="ImageDtModified" HeaderText="<h5>DATE MODIFIED</h5>"
														DataFormatString="{0:MM/dd/yyyy HH:mm:ss tt}" SortExpression="ImageDtModified"></asp:BoundColumn>
												</Columns>
												<AlternatingItemStyle CssClass="gridaltrowWizard" />
											</asp:datagrid></TD>
									</TR>
								</asp:Panel>
								<asp:Panel id="ImageListRepeaterPanel" Runat="server" Visible="False">
									<TR>
										<TD style="PADDING-RIGHT: 5px" align="right" colSpan="3">
											<ASP:DataList id="ImageListRepeater" runat="server" BorderWidth="0" RepeatLayout="Table" RepeatColumns="4"
												BorderColor="black" GridLines="none" CellPadding="0" CellSpacing="2">
												<ItemTemplate>
													<table class="tableContent" align="center" cellpadding="0" border=0 style="width:175px;">
														<tr>
															<td valign="middle" align="center" style="border:1px solid #cccccc">
																<div style="width:175px;border:0">
																	<table cellpadding=0 cellspacing=0 border="0" width="100%">
																		<tr>
																			<td height="110" width="175" align="center" colspan="2">
																				<%#DataBinder.Eval(Container.DataItem,"ImageDIV")%>
																				<asp:ImageButton Runat=server ID="BrowseImage" ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Image")%>'>
																				</asp:ImageButton>
																				</DIV>
																			</td>
																		</tr>
																		<tr>
																			<td width="155" align="center" class="tablecontent" noWrap><%#DataBinder.Eval(Container.DataItem,"ImageName")%>&nbsp;</td>
																			<td width="20" align="left"><a href="javascript:void(0)" onclick=JavaScript:getit('<%#DataBinder.Eval(Container.DataItem,"ImagePath")%>')><img src="/ecn.images/images/icon-add.gif" border=0 ></a></td>
																			
																		</tr>
																	</table>
																</div>
															</td>
														</tr>
													</table>
												</ItemTemplate>
											</ASP:DataList>
											<br><br>
											<AU:PagerBuilder id="ImageListRepeaterPager" Runat="server" Width="95%" ControlToPage="ImageListRepeater" onindexchanged="ImageListRepeaterPager_IndexChanged">
												<PagerStyle CssClass="gridpagerWizard"></PagerStyle>
											</AU:PagerBuilder><br></TD>
									</TR>
								</asp:Panel></TABLE>
							<asp:TextBox id="imagepath" Runat="server" Visible="False"></asp:TextBox>
							<asp:TextBox id="imagesize" Runat="server" Visible="False">100</asp:TextBox></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</asp:Panel>
	<asp:Panel ID="PanelUpload" Visible="false" Runat="server">
		<TR>
			<TD colSpan="2" class="offWhite">
				<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
					<TR>
						<TD class="grd_panel_borders" align="center">
							<ecn:uploader id="pageuploader" runat="server" uploadDirectory="e:\\http\\dev\\ECNblaster\\assets\\eblaster\\customers\\1\\images\\"></ecn:uploader><BR>
							<BR>
							<BR>
						</TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</asp:Panel>
	<tr>
		<td>
			<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0" class="gradient">
				<TR>
					<td valign="middle" align="right" noWrap height="22">
						<asp:Panel ID="PanelTabsBottom" Visible="true" style="DISPLAY:inline" Runat="server" Height="22"><IMG style="POSITION: relative; TOP: 3px" src="/ecn.images/images/browse_img.gif"
								border="0"> 
<asp:LinkButton id="tabBrowseBottom" onclick="showBrowse" Text="" runat="server">Browse Images</asp:LinkButton>&nbsp; 
            |&nbsp; <IMG style="POSITION: relative; TOP: 3px" src="/ecn.images/images/upload_img.gif"
								border="0"> 
<asp:linkbutton id="tabUploadBottom" onclick="showUpload" Text="" runat="server">Upload Images</asp:linkbutton>&nbsp; 
						</asp:Panel>
						&nbsp;&nbsp;</td>
				</TR>
			</TABLE>
		</td>
	</tr>
</table>
