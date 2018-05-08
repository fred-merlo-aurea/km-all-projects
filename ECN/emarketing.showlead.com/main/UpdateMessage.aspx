<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="UpdateMessage.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.UpdateMessage" validateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="./style.css" type="text/css" rel="stylesheet">
			<script language="javascript">
			function hideAdvEditing() {
				var dadv = document.getElementById("dadv");
				dadv.style.display = "none";
				return false;
			}
			
			function toggleAdvEditing () {
				var dadv = document.getElementById("dadv");
				if (dadv.style.display == "none") {
					dadv.style.display = "inline";
				} else {
					dadv.style.display = "none";
				}
				return false;
			}
			</script>
</HEAD>
	<body onload="return hideAdvEditing();">
		<div align="center">
			<div style="BACKGROUND: url(../images/bg.gif) repeat-y; WIDTH: 752px">
				<form id="Form1" runat="server">
					<table cellSpacing="0" cellPadding="0" width="752" border="0">
						<TBODY>
							<tr>
								<td align="left" colSpan="2"><IMG height="65" alt="Show Care" src="../images/header.gif" width="752">
								</td>
							</tr>
							<tr>
								<td vAlign="top" align="right" width="265"><IMG height="186" alt="Update Message" src="../images/img_step2.gif" width="255"></td>
								<td vAlign="top" align="left" width="487"><br>
									<p class="rightpaddingfortext">Please complete all the provided fields before you 
										continue&nbsp;to step 3.
										<BR>
										All fields are required with the exception of phone number.</p>
									<div class="subtitle">Please Enter a Message Name.</div>
									<table cellSpacing="0" cellPadding="2" width="480" border="0">
										<tr>
											<td align="right" width="130">Message Name:
											</td>
											<td><input id="msgTitle" type="text" size="40" name="msgTitle" runat="server">
												<asp:requiredfieldvalidator id="rfvMessageTitle" runat="server" ErrorMessage="«Required" ControlToValidate="msgTitle"></asp:requiredfieldvalidator></td>
										</tr>
									</table>
									<div class="subtitle">Email Information (these will appear in the email the 
										recipient receives).</div>
									<table cellSpacing="0" cellPadding="2" width="480" border="0">
										<tr>
											<td align="right" width="130">From Email Address :
											</td>
											<td><input id="email" type="text" size="40" name="email" runat="server"></td>
											<td><asp:requiredfieldvalidator id="rfvEmail" runat="server" ErrorMessage="«Required" ControlToValidate="email"></asp:requiredfieldvalidator><br>
												<asp:regularexpressionvalidator id="valEmailAddress" runat="server" ErrorMessage="«Not Valid" ControlToValidate="email"
													ValidationExpression=".*@.*\..*" Display="Static"></asp:regularexpressionvalidator></td>
										</tr>
										<tr>
											<td align="right" width="130">From Name :</td>
											<td><input id="name" type="text" size="40" name="name" runat="server"></td>
											<td><asp:requiredfieldvalidator id="rfvHeaderFromName" runat="server" ErrorMessage="«Required" ControlToValidate="name"></asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="right" width="130">Email Subject :
											</td>
											<td><input id="emailSubject" type="text" size="40" name="emailSubject" runat="server"></td>
											<td><asp:requiredfieldvalidator id="rfvEmailSubject" runat="server" ErrorMessage="«Required" ControlToValidate="emailSubject"></asp:requiredfieldvalidator></td>
										</tr>
									</table>
									<div class="subtitle">Content Information (this info. will appear in the text of 
										the message).
									</div>
									<table cellSpacing="0" cellPadding="2" width="480" border="0">
										<tr>
											<td align="right" width="130"><strong>Salutation</strong>
											</td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td align="right" width="130">First Name Only :
											</td>
											<td><input id="Radio1" type="radio" CHECKED value="firstname" name="salutation" runat="server"></td>
										</tr>
										<tr>
											<td align="right" width="130">First and Last Name :
											</td>
											<td><input id="Radio2" type="radio" value="firstlast" name="salutation" runat="server"></td>
										</tr>
									</table>
									<div class="subtitle">Footer Information.</div>
									<table cellSpacing="0" cellPadding="2" width="480" border="0">
										<tr>
											<td align="right" width="130">From Name :
											</td>
											<td><input id="footerName" type="text" size="40" name="footerName" runat="server">
												<asp:requiredfieldvalidator id="rfvFooterName" runat="server" ErrorMessage="«Required" ControlToValidate="footerName"></asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="right" width="130">Title :
											</td>
											<td><input id="footerTitle" type="text" size="40" name="footerTitle" runat="server">
												<asp:requiredfieldvalidator id="rfvFooterTitle" runat="server" ErrorMessage="«Required" ControlToValidate="footerTitle"></asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="right" width="130">Company Name :
											</td>
											<td><input id="footerCompany" type="text" size="40" name="footerCompany" runat="server">
												<asp:requiredfieldvalidator id="rfvCompany" runat="server" ErrorMessage="«Required" ControlToValidate="footerCompany"></asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="right" width="130">Phone Number :
											</td>
											<td><input id="footerPhone" type="text" size="40" name="footerPhone" runat="server"></td>
										</tr>
									</table>
									<asp:panel id="pnlCustomHeader" Visible="False" Runat="server">
      <DIV class=subtitle>Header Information.</DIV>
      <TABLE cellSpacing=0 cellPadding=2 width=480 border=0>
        <TR>
          <TD colSpan=2>
            <DIV style="FONT-WEIGHT: bold; FONT-SIZE: 12px; COLOR: red">For 
            better results, your image should not be more than 650px (pixels) 
            wide.</DIV></TD></TR>
        <TR>
          <TD align=right width=130>Upload New Logo : </TD>
          <TD><INPUT id=fHeaderImg type=file name=fHeaderImg 
        runat="server"></TD></TR>
        <TR>
          <TD align=right width=130>Selected Logo: </TD>
          <TD><INPUT id=HeaderImg type=text size=40 name=HeaderImg 
            runat="server"></TD></TR></TABLE>
									</asp:panel></td>
							</tr>
							<tr>
								<td align="left" colSpan="2">
									<div style="MARGIN: 0px 50px"><asp:label id="lblMsg" Runat="server" Font-Size="10pt" Font-Names="Verdana" ForeColor="Red"></asp:label>
										<div class="subtitle" style="WIDTH: 650px">Advance Editing <font color="#ff0000">(optional)</font>.
										</div>
										You can choose to edit or replace the existing message from the template with a 
										message of your own.<br>
										<strong>Click the "Edit Message" button to activate advanced editing options.</strong>
										<p align="center"><input id="btnEdit" onclick="return toggleAdvEditing();" type="image" alt="edit message"
												src="../images/btn_editmessage.gif" name="btnEdit"></p>
										<div id="dadv">
											<table cellSpacing="0" cellPadding="2" width="100%" border="0">
												<tr>
													<td></td>
												</tr>
												<tr>
													<td>
														<table cellSpacing="0" cellPadding="0" width="100%" border="0">
															<tr>
																<td width="15"><IMG height="23" src="../images/Header_LeftEnd_g.png" width="15"></td>
																<td bgColor="#d1f0b3">&nbsp;</td>
																<td width="15"><IMG height="23" src="../images/Header_RightEnd_g.png" width="15"></td>
															</tr>
														</table>
														<table cellSpacing="0" cellPadding="5" width="100%" border="0">
															<tr>
																<td bgColor="#d1f0b3" colSpan="3">
																	<table cellSpacing="0" cellPadding="0" border="0">
																		<tr>
																			<td colSpan="3"></td>
																		</tr>
																		<tr>
																			<td>
																				<P class="rightpaddingfortext" align="left"><strong>Editing Instructions : </strong>
																				</P>
																				<UL>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">Use the editor below to modify the 
																							text of the message.</DIV>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">Type your revisions just as you would 
																							in any text editor.</DIV>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">Put your cursor on the icons to 
																							determine their function.</DIV>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">To insert a link (for example, to a 
																							page on your website), type in a name for the link, highlight the name, and 
																							click on the <sub><IMG src="../main/editor/images/toolbar/default/button.link.gif"></sub>
																							icon. In the box that appears, use the drop down to select the URL type, type 
																							in the URL and click on save.</DIV>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">To delete a link, highlight the link 
																							and click the <sub><IMG src="../main/editor/images/toolbar/default/button.unlink.gif"></sub>
																							icon</DIV>
																					<LI>
																						<DIV class="rightpaddingfortext" align="left">If you are copying from the Word 
																							document, this editor needs to edit the copied text. Copy the text from the 
																							Word, click on the spot where you want to insert the text, then click on the <FONT face="Arial" color="navy" size="2">
																								<SPAN style="FONT-SIZE: 10pt; COLOR: navy; FONT-FAMILY: Arial">
																									<IMG id="_x0000_i1027" height="21" src="../main/editor/images/toolbar/default/button.pasteword.gif"
																										width="21" align="baseline" border="0"></SPAN></FONT>&nbsp;&nbsp;icon 
																							to edit and paste the text.
																						</DIV>
																					</LI>
																				</UL>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
														<table cellSpacing="0" cellPadding="0" width="100%" border="0">
															<tr>
																<td width="15"><IMG height="23" src="../images/Footer_LeftEnd_g.png" width="15"></td>
																<td bgColor="#d1f0b3">&nbsp;</td>
																<td width="15"><IMG height="23" src="../images/Footer_RightEnd_g.png" width="15"></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td align="left"><IMG height="28" src="../images/img_editorheader.gif" width="290"></td>
												</tr>
												<tr>
													<td>
                                                      <CKEditor:CKEditorControl ID="contents"  width="650" height="450" runat="server" Skin="kama" BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl> 
                                                    </td>
                                                </tr>
											</table>
										</div>
										<div style="MARGIN-TOP: 20px" align="center">
											<table cellSpacing="0" cellPadding="0" width="340" border="0">
												<tr>
													<td><asp:imagebutton id="btnBack" runat="server" ImageUrl="../images/btn_goback.gif" CommandArgument="Step2"
															CommandName="BackFrom" CausesValidation="False"></asp:imagebutton></td>
													<td><IMG height="41" src="../images/line_separator.gif" width="8"></td>
													<td><asp:imagebutton id="btnNext" runat="server" ImageUrl="../images/btn_gotopreview.gif"></asp:imagebutton></td>
												</tr>
											</table>
										</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="footer" align="center" colSpan="2" height="22">Need Help? Click here for <a style="COLOR: #ffff00" href="http://www.showlead.com/help.aspx" target="_blank">
										assistance.</a></td>
							</tr>
						</TBODY></table>
				</form>
			</div>
		</div>
	</body>
</HTML>
