<%@ Page language="c#" AutoEventWireUp="True" CodeBehind="emailAddressCheck.aspx.cs" Inherits="ecn.communicator.main.lists.emailAddressCheck" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ECN RealTime Email Address Validator</title>
	</HEAD>
	<body>
		<form id="SimpleValidate_cs" method="post" runat="server">
			<span class="exampleheading">Validate an Email Address</span>
			<br>
			<br>
			&nbsp; <input class="flatinput" id="txtEmail" style="WIDTH: 360px; HEIGHT: 22px" type="text" size="54"
				value="test@aspNetEmail.com" name="txtEmail" runat="server">&nbsp; <input id="validateBtn" type="submit" value="Validate!" name="validateBtn" runat="server" onserverclick="validateBtn_ServerClick">
			<asp:placeholder id="phResults" Runat="server">
				<P><b>Results</b><BR>
					<asp:literal id="litResults" Runat="server"></asp:literal></P>
				<P><b>Validation Log</b><br>
					<asp:Literal id="litLog" Runat="server"></asp:Literal>
				<P><b>MX Records</b><br>
					<asp:literal id="litMxRecords" Runat="server"></asp:literal></P>
				<P><b>MX log</b><br>
				<asp:textbox id="txtLog" Runat="server" TextMode=MultiLine Rows="15" Columns="200"></asp:textbox></P>
				
			</asp:placeholder></form>
	</body>
</HTML>
