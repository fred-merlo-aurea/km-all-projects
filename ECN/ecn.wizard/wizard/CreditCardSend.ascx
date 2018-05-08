<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CreditCardSend.ascx.cs" Inherits="ecn.wizard.wizard.CreditCardSend" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--content-->
<script language=javascript>
	function openwindow()
	{
		window.open("PricingChart.aspx",null, "height=200,width=400,status=no,toolbar=no,menubar=no,location=no,left=400, top=300");
	}

</script>

<div style="PADDING-RIGHT: 20px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px; HEIGHT: 350px">
	<div style="PADDING-RIGHT:0px; FONT-SIZE:14px; PADDING-TOP:0px" class="dashed_lines1">
		<table width="100%" cellpadding="0" cellspacing="0">
			<tr>
				<td align="left"><strong>Payment Information and Send.</strong></td>
				<td align="right"><img src="images/btn_view_pricing.gif" onclick="openwindow();" style="cursor:hand;"></td>
			</tr>
		</table>
	</div>
	<div style="PADDING-RIGHT:50px; PADDING-LEFT:20px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
		Thank you for purchasing this session of the Email Wizard.
		<br>
		The charge for your email message will be: <span style="COLOR:red">
			<asp:Label runat="server" ID="lblAmount" /></span>
		<br>
		<span style="FONT-SIZE:10px"><em>The charge will appear on your credit card statement 
				as Email Wizard.</em></span><br>
	</div>
	<div style="PADDING-RIGHT:0px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
		<table width="440" border="0" cellspacing="0" cellpadding="2">
			<tr>
				<td width="40%" align="right">Name as it appears on card :</td>
				<td width="60%"><input name="Name" type="text" id="Name" size="30" class="blue_border_box" runat="server"></td>
			</tr>
			<tr>
				<td align="right">Credit Card :</td>
				<td>
					<asp:DropDownList id="cardType" CssClass="blue_border_box" runat="server" onselectedindexchanged="DropDownList1_SelectedIndexChanged">
						<asp:ListItem Value="MasterCard" Selected="True">Master Card</asp:ListItem>
						<asp:ListItem Value="Visa">Visa</asp:ListItem>
						<asp:ListItem Value="Amex">American Express</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="right">Card Number :</td>
				<td><input name="cardNumber" type="text" id="cardNumber" size="30" class="blue_border_box"
						runat="server"></td>
			</tr>
			<tr>
				<td align="right">Expiration date :</td>
				<td><select name="month" id="month" class="blue_border_box" runat="server">
						<option value="01" selected>Jan</option>
						<option value="02">Feb</option>
						<option value="03">Mar</option>
						<option value="04">Apr</option>
						<option value="05">May</option>
						<option value="06">Jun</option>
						<option value="07">Jul</option>
						<option value="08">Aug</option>
						<option value="09">Sep</option>
						<option value="10">Oct</option>
						<option value="11">Nov</option>
						<option value="12">Dec</option>
					</select>&nbsp;&nbsp;&nbsp;
					<select name="year" id="year" class="blue_border_box" runat="server">
						<option value="06" selected>2006</option>
						<option value="07">2007</option>
						<option value="08">2008</option>
						<option value="09">2009</option>
						<option value="10">2010</option>
					</select>
				</td>
			</tr>
			<tr>
				<td align="right" valign="top">Card Verification Number :
				</td>
				<td width="302" valign="top">
					<input name="Name" type="text" id="cvNumber" size="7" maxlength="3" class="blue_border_box"
						runat="server">&nbsp;&nbsp;&nbsp;<img src="images/img_credit_card.gif" width="182" height="55" align="textTop"></td>
			</tr>
		</table>
	</div>
	<asp:label id="lblDebugMsg" runat="server" align="center"></asp:label>
	<div id="divMsg" runat="server" align="center" style="COLOR:#227622; PADDING-TOP:10px"></div>
</div>
