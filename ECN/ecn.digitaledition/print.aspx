<%@ Page language="c#" Inherits="ecn.digitaledition.print" Codebehind="print.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Your Digital Edition</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="themes/default/stylesheet.css" rel="stylesheet" type="text/css">
	</HEAD>
	<body style="BACKGROUND-COLOR:#ffffff">
		<form id="Form1" method="post" runat="server">
			<asp:label id="lbltotalpages" Visible="False" Runat="server"></asp:label>
			<asp:label id="imgPath" Visible="False" Runat="server"></asp:label><asp:panel id="pnlprint" Visible="True" Runat="server">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD align="center" height="50"><BR>
							<BR>
							<BR>
							<BR>
							<BR>
							<BR>
							<IMG height="35" alt="Print Center" src="themes/blue/print.jpg" width="40"></TD>
					</TR>
					<TR>
						<TD class="print_text" style="PADDING-BOTTOM: 10px; PADDING-TOP: 15px" align="center">Please 
							select one the print options below.
							<BR>
							<HR style="COLOR: #999999">
						</TD>
					</TR>
					<TR>
						<TD align="center">
							<TABLE cellSpacing="1" cellPadding="1" width="30%" border="0">
								<TR>
									<TD height="30">
										<asp:RadioButton id="rdCurrent" runat="server" Checked="True" GroupName="grpPrint" CssClass="print_text"
											Text="Current Page(s) : "></asp:RadioButton></TD>
								</TR>
								<TR>
									<TD height="30">
										<asp:RadioButton id="rdRange" runat="server" GroupName="grpPrint" CssClass="print_text" Text="Pages : "></asp:RadioButton>&nbsp;
										<asp:DropDownList id="drpStart" runat="server"></asp:DropDownList>&nbsp;to&nbsp;
										<asp:DropDownList id="drpEnd" runat="server"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD height="30">
										<asp:RadioButton id="rdAll" runat="server" GroupName="grpPrint" CssClass="print_text" Text="All : "></asp:RadioButton></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblMessage" Runat="server" Visible="False" Font-Size="x-small" ForeColor="red"></asp:Label></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD vAlign="middle" align="center">
							<HR style="COLOR: #999999">
							<asp:Button id="Button1" runat="server" Text="Print" onclick="Button1_Click"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel><asp:panel id="pnlImage" Visible="True" Runat="server">
				<asp:Repeater id="rptImage" runat="server">
					<ItemTemplate>
						<p align="center">
							<asp:Image Runat="server" ID="imgThumbnail" ImageUrl='<%# DataBinder.Eval(Container, "DataItem.imgpath") %>'>
							</asp:Image></p>
					</ItemTemplate>
				</asp:Repeater>
			</asp:panel></form>
	</body>
</HTML>
