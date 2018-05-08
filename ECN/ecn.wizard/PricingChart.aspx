<%@ Page language="c#" Codebehind="PricingChart.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.PricingChart" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>PricingChart</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
  </HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			
			<div class="dashed_lines1" style="PADDING-RIGHT: 0px; FONT-SIZE: 14px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px"><strong>Pricing Chart:</strong></div>
			<br>
			<center>
			<asp:datagrid id="dgPricing" AutoGenerateColumns="False" Width="350px" Runat="server" AllowPaging=False AllowSorting=False BackColor="white">
				<AlternatingItemStyle Font-Size="12px" BackColor="#EDF0F5"></AlternatingItemStyle>
				<ItemStyle Font-Size="12px"></ItemStyle>
				<HeaderStyle Font-Size="12px" HorizontalAlign="Center" ForeColor="White" BackColor="#A6A6A6"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="EmailCount" HeaderText="# of Emails" HeaderStyle-HorizontalAlign=Left>
						<HeaderStyle ForeColor="White"></HeaderStyle>
						<ItemStyle HorizontalAlign="left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="BaseFee" HeaderText="Base Price" DataFormatString="{0:C}"> 
						<HeaderStyle ForeColor="White"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>					
					<asp:BoundColumn DataField="Cost" HeaderText="+ Cost/Email" DataFormatString="{0:C4}">
						<HeaderStyle ForeColor="White"></HeaderStyle>
						<ItemStyle HorizontalAlign="center"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</asp:datagrid>
			<br>
			<div align="center"><IMG src="images/btn_close.gif" onclick="window.close();" style="cursor:hand;"></div>
			</center>
			
		</form>
	</body>
</HTML>
