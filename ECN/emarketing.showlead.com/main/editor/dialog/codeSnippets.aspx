<%@ Page language="c#" AutoEventWireup="false" CodeBehind="codeSnippets.aspx.cs" Inherits="ecn.communicator.contentmanager.editor.dialog.codeSnippets" %>

<HTML>
  <HEAD>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="../css/fck_dialog.css">
		<script language="JavaScript">
			function updateSelection(){
				var selected = selectSnippet.value;
				codeSnippet.value = selected;
			}

			function ok()	{
				var selected = selectSnippet.value;
				if ( selected.length == 0 ) {
					cancel() ;
					return ;
				}else {
					window.returnValue = codeSnippet.value;
				}
				window.close() ;
			}

			function cancel() {
				window.returnValue = null ;
				window.close() ;
			}			
		</script>
</HEAD>
	<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
		<table cellspacing="0" cellpadding="0" width="98%" border="0" ID="Table2">
		<TR>
			<TD vAlign=top>Select&nbsp;Group</TD></TR>
		<form id="BlastForm" Runat="Server">	
		<TR>
		    <TD vAlign=top><asp:dropdownlist class="formfield" id="Groups" runat="server" DataTextField="GroupName"	DataValueField="GroupID" AutoPostBack=True></asp:dropdownlist></TD>
		</TR>
		</form>	
		<TR><TD height=7></TD></TR>
		<tr>
			<td valign="top"
				<font face=verdana size=1>Select&nbsp;Code&nbsp;Snippet</FONT><br>
				<select onchange="updateSelection()" style="WIDTH: 100%" name="selectSnippet" id=selectSnippet>
				<% Response.Write(CodeSnippets); %>
				</select>
				<input name="codeSnippet" type="hidden" >
			</td>
		</tr>
		<TR><TD height=7></TD></TR>		
		<tr>
			<td align=right height=26 valign=top>
					<input type=button fckLang="DlgBtnOK" onClick="ok();" value="OK">&nbsp;
					&nbsp;<input type="button" fckLang="DlgBtnCancel" value="Cancel" onClick="cancel();" >
				</td>
		</tr>
		<tr>
			<td align=left bgcolor=#000000>
				<table cellpadding=1><tr><td valign=top><img src="warning10X10.gif"></td><td><font color=#ffff33>Selecting User Defined Fields [UDFs] from different Groups for the same Campaign will cause Campaign not to work correctly.<br>Please choose UDFs from the Group you are trying to send this Campaign</font></td></tr></table>
			</td>
		</tr>		
		</table>
	</body>
</HTML>
