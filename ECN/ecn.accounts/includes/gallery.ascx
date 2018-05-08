<%@ Reference Control="~/includes/uploader.ascx" %>
<%@ Control Language="c#" Inherits="ecn.accounts.includes.gallery" Codebehind="gallery.ascx.cs" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="uploader.ascx" %>
<asp:Panel ID="PanelTabs" Visible="true" Runat="server">
	<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
		<TR>
			<TD width="72"><asp:linkbutton id="tabBrowse" onclick="showBrowse" Text="browse" CssClass="tableContent" runat="server"></asp:linkbutton>&nbsp;</TD>
			<TD width="72"><asp:linkbutton id="tabUpload" onclick="showUpload" Text="upload" CssClass="tableContent" runat="server"></asp:linkbutton>&nbsp;</TD>
			<TD width="*">&nbsp;</TD>
			<TD width="72"><asp:linkbutton id="tabPreview" onclick="showPreview" Text="preview" CssClass="tableContent" runat="server" Visible="False"></asp:linkbutton>&nbsp;</TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:Panel ID="PanelBrowse" Visible="true" Runat="server">
	<TABLE cellSpacing="0" cellPadding="0"  width="100%" border="1">
		<TR>
			<TD align=center>
				<asp:TextBox id="imagepath" Runat="server" Visible="False"></asp:TextBox>
				<asp:TextBox id="imagesize" Runat="server" Visible="False">100</asp:TextBox>
				<ASP:DataList id="imagerepeater" runat="server" CellSpacing="0" CellPadding="4" 
				GridLines="Both" BorderWidth="1" BorderColor="black" 
				RepeatColumns="3" RepeatLayout="Table">
				<ItemTemplate>
					<table class="tableContent" align=center>
					<tr><td valign=center align=center height=<%=thumbnailSize%>>
					<asp:ImageButton Runat=server ID="BrowseImage" OnCommand="previewImage" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Path")%>' ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Image")%>'></asp:ImageButton>
					</td></tr>
					<tr><td valign=top align=center>
					<asp:LinkButton Runat="server" ID="BrowseImageLink" OnCommand="previewImage" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Path")%>' Text='<%#DataBinder.Eval(Container.DataItem,"FileName")%>'></asp:LinkButton>
					<br><%#DataBinder.Eval(Container.DataItem,"Size")%> - <%#DataBinder.Eval(Container.DataItem,"Date")%>
					</td></tr>
					</table>
				</ItemTemplate>
				</ASP:DataList>
			</TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:Panel ID="PanelPreview" Visible="false" Runat="server">
	<TABLE cellSpacing="0" cellPadding="0"  width="100%" border="1">
		<TR>
			<TD>
			<table width=100% cellpadding="10"><tr>
			<td colspan=2>
			<table width=100%><tr>
			<td width=33% class="tableContent" align=left>
			<asp:HyperLink Runat=server ID=fullsizeLink Text="full size image" Target=_blank></asp:HyperLink>
			</td>
			<td width=33% class="tableContent" align=center>
			<asp:LinkButton Runat="server" ID=deleteLink OnCommand="deleteImage" Text="delete image"></asp:LinkButton>
			</td>
			<td width=33% class="tableContent" align=right><a href="?imagefile=">back</a></td>
			</tr></table>
			</td>
			</tr>
			<tr>
			<td height=300 width=300 align=center valign=top class="tableContent">
			<asp:Image ID="ImagePreview" Runat="server"></asp:Image>
			</td>
			<td valign=top class="tableContent">
			<br>Name: <asp:Label ID="previewName" Runat=server Text="Name"></asp:Label>
			<br>Size: <asp:Label ID="previewSize" Runat=server Text="Size"></asp:Label>
			<br>Date: <asp:Label ID="previewDate" Runat=server Text="Date"></asp:Label>
			<br>Height: <asp:Label ID="previewHeight" Runat=server Text="Height"></asp:Label>
			<br>Width: <asp:Label ID="previewWidth" Runat=server Text="Width"></asp:Label>
			<br>Resolution: <asp:Label ID="previewResolution" Runat=server Text="Width"></asp:Label>
			</td></tr>
			</table>
			</TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:Panel ID="PanelUpload" Visible="false" Runat="server">
	<TABLE cellSpacing="0" cellPadding="0"  width="100%" border="1">
		<TR>
			<TD align=center>
			<br>
			<ecn:uploader id="pageuploader" runat="server" 
			uploadDirectory="e:\\http\\dev\\ECNblaster\\assets\\eblaster\\customers\\1\\images\\"></ecn:uploader>
			<br><br></TD>
		</TR>
	</TABLE>
</asp:Panel>
