<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Page language="c#" Codebehind="list.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.list" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>list</title>
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid id="dgEmailList" runat="server" width="100%" CellPadding="2" AutoGenerateColumns="False"
				BackColor="white">
				<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
				<ItemStyle Font-Size="10pt"></ItemStyle>
				<HeaderStyle Font-Size="11pt" HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
				<Columns>
					<asp:BoundColumn ItemStyle-Width="20%" DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="left">
									<HeaderStyle ForeColor="White"></HeaderStyle>
									<ItemStyle Font-Size="10pt" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn ItemStyle-Width="20%" DataField="lastName" HeaderText="Last Name">
									<HeaderStyle ForeColor="White"></HeaderStyle>
									<ItemStyle Font-Size="10pt" HorizontalAlign="Left"></ItemStyle>
					
					</asp:BoundColumn>
					<asp:BoundColumn ItemStyle-Width="60%" DataField="EmailAddress" HeaderText="EmailAddress" ItemStyle-HorizontalAlign="left">
									<HeaderStyle ForeColor="White"></HeaderStyle>
									<ItemStyle Font-Size="10pt" HorizontalAlign="Left"></ItemStyle>
					
					</asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
			<AU:PagerBuilder id="EmailsPager" Runat="server" ControlToPage="dgEmailList" PageSize="50" Width="100%" >
<PagerStyle Font-Size="X-Small"> 
</PagerStyle>
			</AU:PagerBuilder>
		</form>
	</body>
</HTML>
