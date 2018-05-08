<%@ Page language="c#" Inherits="ecn.activityengines.linkfromwithtopic" Codebehind="linkfromwithtopic.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
		<link rel="stylesheet" href="/ecn.images/images//stylesheet_default.css" type="text/css">
		<script language=javascript>
			function getobj(id) {
				if (document.all && !document.getElementById) 
					obj = eval('document.all.' + id);
				else if (document.layers) 
					obj = eval('document.' + id);
				else if (document.getElementById) 
					obj = document.getElementById(id);

				return obj;
			}
			
			function sendEmailNow(toEmail){
				var cc = getobj("cc").value;
				var bcc = getobj("bcc").value;
				var subject = getobj("subject").value;
				var body = getobj("body").value;
				
				if(cc.length == 0){ cc = " "; }
				if(bcc.length == 0){ bcc = " "; }				
				if(subject.length == 0){ subject = " "; }
				if(body.length == 0){ body = " "; }	
								
				var sendMail = "mailto:" + toEmail +
										"?cc=" + cc +
										"&amp;bcc=" + bcc +
										"&amp;subject=" + escape(subject) +
										"&amp;body=" + escape(body);

				window.location = sendMail;
				window.close();
			}
		</script>
	</head>
	<body>
		<asp:panel id="errorMsgPanel" runat=server visible=false>
			<table width=50% align=left border=0 cellpadding=5>
				<tr>
					<td style="font-family:arial; font-size:12px">
					We're sorry - the page you have requested does not exist.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensivitity (‘a’ and ‘A’ are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br><br><br>Sincerely,<br>Customer Support
					</td>
				</tr>
			</table>	
		</asp:Panel>
		<asp:panel id="mailSenderFormPanel" runat="Server" visible=false>
			<form id=mailSenderForm name=mailSenderForm>
				<table width=50% align="center" border='0' cellpadding="5">
					<tr>
						<td align='right' class="tableHeader">To :</td><td style="font-family:arial; font-size:12px">&nbsp;<%= linkToUSE.Replace("mailto:", "")%></td>
					</tr>
					<tr>
						<td align='right' class="tableHeader">[optional] CC :</td><td>&nbsp;<input type=text size=30 class=formfield name=cc id=cc></td>
					</tr>
					<tr>
						<td align='right' class="tableHeader">[optional] BCC :</td><td>&nbsp;<input type=text size=30 class=formfield name=bcc id=bcc></td>
					</tr>
					<tr>
						<td align='right' class="tableHeader">Subject :</td><td>&nbsp;<input type=text size=30 class=formfield name=subject id=subject></td>
					</tr>
					<tr>
						<td align='right' valign="top" class="tableHeader">Body :</td><td>&nbsp;<Textarea cols=50 rows=10 class=formfield name=body id=body></Textarea></td>
					</tr>		
					<tr>
						<td align="center" style="font-family:arial; font-size:12px" class="tableHeader" colspan=2><input type=button name="sendButton" value="Create Email" class=formfield onclick="sendEmailNow('<%= linkToUSE.Replace("mailto:","") %>')"></td>
					</tr>												
				</table>
			</form>
		</asp:panel>
	</body>
</html>
