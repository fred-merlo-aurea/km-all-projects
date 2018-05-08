<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="CreditCardSend.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.CreditCardSend" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="./style.css" type="text/css" rel="stylesheet">
			<script language="javascript">
		function setcvwidth() {
			//alert(document.forms[0].cardType.selectedIndex);
			if (document.forms[0].cardType.selectedIndex == 2) {
				document.forms[0].cvNumber.maxLength = 4;
				document.forms[0].cvNumber.value = "";
			} else {
				document.forms[0].cvNumber.maxLength = 3;
				document.forms[0].cvNumber.value = "";
			}
			//alert (document.forms[0].cvNumber.maxLength);
		}
		
		function validate() {
			if (document.forms[0].cardType.selectedIndex == 2) {
				if (document.forms[0].cvNumber.value.length != 4) { 
					alert ("Amex requires 4 digits in CV Number");
					return false;
				}
			} else {
				if (document.forms[0].cvNumber.value.length != 3) {
					alert ("Master and Visa requires 3 digits of CV Number");
					return false;
				}
			}
			return true;
		}
			</script> 
	</HEAD>
	<body>
		<form action="#" runat="server" ID="Form1">
			<div align="center">
				<div style="BACKGROUND: url(../images/bg.gif) repeat-y; WIDTH: 752px">
					<table cellSpacing="0" cellPadding="0" width="752" border="0">
						<tr>
							<td colSpan="2"><IMG height="65" alt="Show Care" src="../images/header.gif" width="752"></td>
						</tr>
						<tr>
							<td vAlign="top" align="right" width="265"><IMG height="186" src="../images/img_step4.gif" width="255"></td>
							<td vAlign="top" align="left" width="487">
								<p class="title">Payment Information and Send!</p>
								<table cellSpacing="0" cellPadding="2" width="480" border="0">
									<tr>
										<td class="rightpaddingfortext" align="left" colSpan="2">Your email is ready to 
											send!.<BR>
											The fee for this email campaign is <b>$<asp:label id="lblAmount" Runat="server" Width="20px" Font-Size="Small"></asp:label> (US)</b> To 
											proceed with this purchase and to send your email to the selected recipients, 
											please the details below.
											<br>
											<I>The charges will appear on your next credit card statement as "Knowledge 
												Marketing"</I>
										</td>
									</tr>
									<TR>
										<TD align="right" colspan="2"></TD>
									</tr>
									<TR>
										<asp:PlaceHolder ID="plCoupon" Runat="server" Visible="true"> 
										<TD align="right" width="33%" valign="middle">
											Coupon Code :
											</TD>
											<TD align="left" width="67%" valign="middle"> 
												<asp:TextBox id="txtCouponcode" runat="server"></asp:TextBox>&nbsp; 
												<asp:LinkButton Text="Validate" runat="server" CausesValidation="false"
                                                    onclick="Validate_Coupon_Click" PostBackUrl="~/main/CreditCardSend.aspx" ToolTip="Click here to check coupon code validity" Font-Italic="True"></asp:LinkButton>
                                           </td>
										   </asp:PlaceHolder> 
									</tr>									
									<asp:PlaceHolder ID="plCreditcard" Runat="server" Visible="false">
										<TR>
											<TD align="right" width="150">Name as it appears on card :
											</TD>
											<TD width="302"><INPUT id="Name" type="text" size="35" name="Name" runat="server">
												<asp:requiredfieldvalidator id="rfvCardName" runat="server" ControlToValidate="Name" ErrorMessage="«Required"></asp:requiredfieldvalidator></TD>
										</TR>
										<TR>
											<TD align="right" width="150">Credit Card :
											</TD>
											<TD width="302">
												<asp:DropDownList id="cardType" runat="server" name="cardType" OnChange="javascript:setcvwidth()">
													<asp:ListItem Selected Value="MasterCard">Master Card</asp:ListItem>
													<asp:ListItem Value="Visa">Visa</asp:ListItem>
													<asp:ListItem Value="Amex">American Express</asp:ListItem>
												</asp:DropDownList>
												<asp:requiredfieldvalidator id="rfvCardType" runat="server" ControlToValidate="cardType" ErrorMessage="«Required"></asp:requiredfieldvalidator></TD>
										</TR>
										<TR>
											<TD align="right" width="150">Card Number :
											</TD>
											<TD width="302"><INPUT id="cardNumber" type="text" size="35" name="cardNumber" runat="server">
												<asp:requiredfieldvalidator id="rfvCardNumber" runat="server" ControlToValidate="cardNumber" ErrorMessage="«Required"></asp:requiredfieldvalidator></TD>
										</TR>
										<TR>
											<TD align="right" width="150">Expiration date :
											</TD>
											<TD width="302"><SELECT id="month" name="month" runat="server">
													<OPTION value="01" selected>Jan</OPTION>
													<OPTION value="02">Feb</OPTION>
													<OPTION value="03">Mar</OPTION>
													<OPTION value="04">Apr</OPTION>
													<OPTION value="05">May</OPTION>
													<OPTION value="06">Jun</OPTION>
													<OPTION value="07">Jul</OPTION>
													<OPTION value="08">Aug</OPTION>
													<OPTION value="09">Sep</OPTION>
													<OPTION value="10">Oct</OPTION>
													<OPTION value="11">Nov</OPTION>
													<OPTION value="12">Dec</OPTION>
												</SELECT>&nbsp;&nbsp;&nbsp;
												<asp:requiredfieldvalidator id="rfvCardMonth" runat="server" ControlToValidate="month" ErrorMessage="«Required"></asp:requiredfieldvalidator><SELECT id="year" name="year" runat="server">
													<OPTION value="10" selected>2010</OPTION>
													<OPTION value="11">2011</OPTION>
													<OPTION value="12">2012</OPTION>
													<OPTION value="13">2013</OPTION>
													<OPTION value="14">2014</OPTION>
												</SELECT>
												<asp:requiredfieldvalidator id="rfvCardYear" runat="server" ControlToValidate="year" ErrorMessage="«Required"></asp:requiredfieldvalidator></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right" width="150">Card Verification Number :
											</TD>
											<TD vAlign="top" width="302"><INPUT id="cvNumber" type="text" maxLength="3" size="7" name="cvNumber" runat="server">&nbsp;
												<asp:requiredfieldvalidator id="rfvCVNumber" runat="server" ControlToValidate="cvNumber" ErrorMessage="«Required"></asp:requiredfieldvalidator>&nbsp;&nbsp;
												<IMG height="55" src="../images/credit_card.gif" width="182" align="textTop"></TD>
										</TR>
									</asp:PlaceHolder>
								</table>
							</td>
						</tr>
						<tr>
							<td align="center" valign="top" colSpan="2"> 
								<div style="MARGIN: 20px 0px" align="center"><asp:imagebutton OnMouseDown="return validate();" id="btnSubmit" runat="server" ImageUrl="../images/btn_submit.gif"></asp:imagebutton>&nbsp;
								</div>
								<div id="smsg" runat="server">
									<div style="WIDTH: 705px"><IMG src="../images/step4_header.gif"></div>
									<div style="BACKGROUND: url(../images/step4_bg.gif) repeat-y; WIDTH: 705px" runat="server">
										<!--receipt content goes here-->
										<div id="divMsg" style="PADDING-RIGHT: 10px; PADDING-LEFT: 10px; FONT-SIZE: 16px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; POSITION: relative"
											runat="server"></div> 
									</div>
									<div style="WIDTH: 705px"><IMG src="../images/step4_footer.gif"></div>
								</div>
								<div style="MARGIN: 20px 0px" align="center">&nbsp;</div>
								</td>
						</tr>
						<tr>
							<td class="footer" align="center" colSpan="2" height="22">Need Help? Click here for <A style="COLOR: #ffff00" href="http://www.showlead.com/help.aspx" target="_blank">
									assistance.</A></td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>
