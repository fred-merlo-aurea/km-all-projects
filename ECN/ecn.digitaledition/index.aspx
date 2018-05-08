<%@ Page language="c#" Inherits="ecn.digitaledition.index" Codebehind="index.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
"http://www.w3.org/TR/html4/loose.dtd">
<HTML>
	<HEAD>
		<title>Welcome to ECN DigitalEdition</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/stylesheet.css"
			type="text/css" rel="stylesheet">
		</LINK>
		<link href="Images/stylesheet.css" rel="stylesheet" type="text/css">
		<style type="text/css">
BODY { FONT-SIZE: 12px; FONT-FAMILY: Arial, Helvetica, sans-serif;color:#ffffff; background-color:#737F92; }
		</style>
		<script language="JavaScript" type="text/JavaScript">
			<!--
			function MM_swapImgRestore() { //v3.0
			var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
			}

			function MM_preloadImages() { //v3.0
			var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
				var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
				if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
			}

			function MM_findObj(n, d) { //v4.01
			var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
				d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
			if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
			for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
			if(!x && d.getElementById) x=d.getElementById(n); return x;
			}

			function MM_swapImage() { //v3.0
			var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
			if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
			}
			//-->
		
		function openwnd(link)
		{
			var width = screen.width - 10;
			var height = screen.height - 50;
			var win = window.open(link,"win1","status=yes,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes,left=0,top=0,width="+width+',height='+ height);
			win.focus();
			return false;
		}

		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0"
		onLoad="MM_preloadImages('http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/video_b_01.gif','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/charity_b_01.gif','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/publisher_b_01.gif','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/retail_b_01.gif','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/km_logo2.jpg','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/tradeshow_b_01.gif')">
		<form id="Form1" method="post" runat="server">
			<CENTER>
				<TABLE cellSpacing="0" cellPadding="0" width="750" border="0">
					<TR>
						<TD></TD>
					</TR>
					<TR>
						<TD class="loginTop" style="background-color:#737F92;PADDING-RIGHT: 20px; PADDING-LEFT: 0px; PADDING-BOTTOM: 5px; PADDING-TOP: 5px"
							vAlign="top" align="right"><A class="loginTop" href="#contact_us">CONTACT US</A>&nbsp; 
							|&nbsp; <A class="loginTop" href="http://ecn5.com/">Log into ECN</A>
						</TD>
					</TR>
					<TR>
						<TD vAlign="top"><IMG height="175" src="images/publisher_header.jpg" width="750" border="0"></TD>
					</TR>
					<TR>
						<TD>
							<TABLE cellSpacing="0" cellPadding="0" bgColor="#ffffff" border="0">
								<TR>
									<TD align="center" width="205"><A onmouseover="MM_swapImage('km_logo','','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/km_logo2.jpg',1)"
											onmouseout="MM_swapImgRestore()" href="http://knowledgemarketing.com/" target="_blank"><IMG height="16" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/km_logo.jpg"
												width="176" border="0" name="km_logo"></A></TD>
									<td width="218">&nbsp;</td>
									<TD align="center" width="109"><IMG height="19" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/publisher_y_01.gif"
											width="108"></TD>
									<TD align="center" width="109"><A onmouseover="MM_swapImage('charity','','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/charity_b_01.gif',1)"
											onmouseout="MM_swapImgRestore()" href="http://ecncharity.com/"><IMG height="19" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/charity_g_01.gif"
												width="108" border="0" name="charity"></A></TD>
									<TD align="center" width="109"><A onmouseover="MM_swapImage('tradeshow','','http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/tradeshow_b_01.gif',1)"
											onmouseout="MM_swapImgRestore()" href="http://www.ecntradeshow.com/" target="_blank"><IMG height="19" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/tradeshowl_g_01.gif"
												width="108" border="0" name="tradeshow"></A></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD bgColor="#a9936c" height="7"></TD>
					</TR>
					<TR>
						<TD bgColor="#000000" height="14"></TD>
					</TR>
					<TR>
						<TD bgcolor="black">
							<asp:PlaceHolder ID="pnllist" Runat="server">
								<TABLE cellSpacing="0" cellPadding="0" width="720" border="0">
									<TR>
										<TD style="FONT-SIZE: x-large; COLOR: red; FONT-FAMILY: arial"><A class="blue" href="http://www.ecndigitalEdition.com/">ECN 
												DigitalEdition Home</A><BR>
											<BR>
											<asp:Label id="lbldisplay" Visible="false" runat="server"></asp:Label></TD>
									</TR>
									<TR>
										<TD align="center">
											<asp:DataList id="DataList1" runat="server" ItemStyle-HorizontalAlign="Center" CellSpacing="5"
												CellPadding="5" RepeatLayout="Table" RepeatColumns="4" RepeatDirection="Horizontal">
												<itemtemplate>
														<a href="javascript:void(0);" onClick="javascript:openwnd('Magazine.aspx?eID=<%# DataBinder.Eval(Container.DataItem, "EditionID") %>');">
														<img src="<%# DataBinder.Eval(Container.DataItem, "thumbview") %>" alt="<%# DataBinder.Eval(Container.DataItem, "EditionName") %>" border="0" /></a><br>
														<br>
														<font style="FONT-SIZE: x-small; COLOR: white; FONT-FAMILY: arial">
														<%# DataBinder.Eval(Container.DataItem, "EditionName") %>
													</font>&nbsp;
											  </itemtemplate>
											</asp:DataList>
										</TD>
									</TR>
								</TABLE>
							</asp:PlaceHolder>
							<asp:PlaceHolder ID="pnllogin" Runat="server">
								<BR>
								<div align="center">
								<TABLE cellSpacing="0" cellPadding="3" width="100%" border="0">
									<TR>
										<TD vAlign="middle" align="left">
											<DIV style="WIDTH: 100%; BORDER-BOTTOM: #ccc 1px solid; text-indent:15px;">Login</DIV>
										</TD>
									</TR>
								</TABLE>
								<TABLE cellSpacing="0" cellPadding="3" width="600" border="0">
									<TR>
										<TD colSpan="2">&nbsp;</TD>
									</TR>
									<TR>
										<TD align="right" width="200"><B>Username:</B>&nbsp;</TD>
										<TD align="left">
											<asp:textbox id="txtLogin" runat="server" CssClass="formSize" MaxLength="50"></asp:textbox>
										</TD>
									</TR>									
									<TR>
										<TD align="right"><B>Password:</B>&nbsp;</TD>
										<TD align="left">
											<asp:textbox id="txtpassword" runat="server" CssClass="formSize" MaxLength="50" TextMode="Password"></asp:textbox>
										</TD>
									</TR>
									<TR>
										<TD>&nbsp;</TD>
										<td align="left"><asp:Label id="lblErrorMessage" Visible="false" runat="server" Font-Bold="true" ForeColor="red"
												Font-Size="small"></asp:Label></td>
									</TR>
									<TR>
										<TD align="center" colSpan="2">
											<asp:imagebutton id="ImageButton1" Runat="server" ImageUrl="images/login2.gif"></asp:imagebutton></TD>
									</TR>
								</TABLE>
								</div>
								<BR>
							</asp:PlaceHolder>
						</TD>
					</TR>
					<TR>
						<TD bgColor="#000000">
							<TABLE cellSpacing="0" cellPadding="0" width="750" border="0">
								<TR bgColor="#737f92">
									<TD align="left" colSpan="3" height="3"></TD>
								</TR>
								<TR>
									<TD align="left" width="289"><IMG height="97" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/template1_25.gif"
											width="289" useMap="#Map2" border="0"></TD>
									<TD width="280"><A name="contact_us"></A></TD>
									<TD align="right" width="181"><IMG height="97" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/ecn_products/images/template1_27.jpg"
											width="181"></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
			  </TABLE>
				<MAP name="Map2">
					<AREA shape="RECT" coords="33,26,247,52" href="http://knowledgemarketing.com/">
				</MAP>
			</CENTER>
		</form>
	</body>
</HTML>
