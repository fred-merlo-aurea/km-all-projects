<%@ register tagprefix="ecn" TagName="Log" src="../includes/LogWizardActivity.ascx"%>
<%@ Page  Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ChooseTemplate.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.ChooseTemplate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
<ecn:Log id="LogSiteActivity" runat="server" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="./style.css" type="text/css" rel="stylesheet">
			<LINK media="screen" href="lightbox.css" type="text/css" rel="stylesheet">
				<script src="../js/lightbox.js" type="text/javascript"></script>
				<script language="javascript">
			// Check if Template is selected or not
			function ValidateAll () {
				// Tell you if at least one value is selected or not
				var isSelected = false;
				// Get the Templates array 
				var rbl = document.forms[0].rblTemplates;
				var i = 0;
				for (i=0; i<rbl.length; i++) {
					if (rbl[i].checked) {
						isSelected = true;
						break;
					}
				}
				
				// if isSelected is true that means user has selected a template
				if (!isSelected) {
					alert ("Please choose a template before proceeding.");
					return false;
				}
				return true;
			}
				</script>
</HEAD>
	<body>
		<div align="center">
			<div style="BACKGROUND: url(../images/bg.gif) repeat-y; WIDTH: 752px">
				<table cellSpacing="0" cellPadding="0" width="752" border="0">
					<tr>
						<td colSpan="2"><IMG height="65" alt="Show Care" src="../images/header.gif" width="752"></td>
					</tr>
					<tr>
						<td vAlign="top" align="right" width="265"><IMG height="186" alt="Choose a template" src="../images/img_step1.gif" width="255"></td>
						<td vAlign="top" align="left" width="487">
							<p class="title">
								Welcome to the Showlead Email Wizard</p>
							<p class="rightpaddingfortext">
								You will now be guided through the process of creating a professional&nbsp;HTML 
								email to send to your leads. HTML-based email are proven to get much higher 
								response than traditional text emails.
								<BR>
								The&nbsp;wizard will walk you through each step of the process. Before you send 
								the final email, you will be able to preview the email to ensure quality.</p>
							<UL>
								<LI>
									<DIV class="blue_fonts"><strong>&nbsp;Choose a template from the thumbnails below.<BR>
										</strong>
									</DIV>
								<LI>
									<DIV class="blue_fonts"><strong>&nbsp;Click on the template thumbnail to preview the 
											template and message.
											<BR>
										</strong>
									</DIV>
								<LI>
									<DIV class="blue_fonts"><strong> &nbsp;In step 2 you can choose to use the message 
											provided or create your own message.</strong></DIV>
								</LI>
							</UL>
							<p>
							<div id="templatesDiv" style="BORDER-TOP: #cccccc 1px solid" align=left>
							<table  cellSpacing="0" cellPadding="2" width="100%" border="0">
								<tr>
									<td width="20%"><strong style="COLOR: #00008b"><b>List Name : </b></strong>&nbsp;</td>
									<td width="80%"><asp:label ID="lblLstName" Runat=server></asp:label></td>
								</tr>
								<tr>
									<td><strong style="COLOR: #00008b"><b>Records : </b></strong>&nbsp;</td>
									<td><asp:HyperLink id=lnkRecords runat="server" ></asp:HyperLink><br></td>
								</tr>
							</table>
							</div>
      <P></P>
						</td>
					</tr>
					<tr>
						<td align="center" colSpan="2">
						</td>
					</tr>
					<tr>
						<td align="center" colSpan="2"><br></td></tr>					
					<tr>
						<td align="center" colSpan="2"><strong class="blue_fonts"> Choose a Template: </strong>
							<IMG src="../images/img_arrowdown.gif" align="absMiddle"> <strong style="COLOR: #00008b">
								To select a template, check the radio button above the thumbnail.</strong>
						</td>
					</tr>
					<tr>
						<td align="center" colSpan="2">
							<form action="#" method="get" runat="server">
								<div id="templatesDiv" style="BORDER-TOP: #0099ff 1px solid; WIDTH: 720px; BORDER-BOTTOM: #0099ff 1px solid"
									runat="server">
									<table id="templatesTable" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
									</table>
								</div>
								<table cellSpacing="0" cellPadding="0" width="340" border="0">
									<tr>
										<td></td>
										<td></td>
										<td><BR>
											<input id="submit" onclick="return ValidateAll();" type="image" src="../images/btn_gostep2.gif"
												name="submit" runat="server"></td>
									</tr>
								</table>
							</form>
						</td>
					</tr>
					<tr>
						<td colSpan="2" align="center" height="22" class="footer">
							Need Help? Click here for <a href="http://www.showlead.com/help.aspx" style="COLOR:#ffff00" target="_blank">
								assistance.</a></td>
					</tr>
				</table>
			</div>
		</div>
	</body>
</HTML>
