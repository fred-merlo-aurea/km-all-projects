<%@ Page language="c#" AutoEventWireup="false" Inherits="ecn.communicator.contentmanager.feditor.dialog.codeSnippets" Codebehind="codeSnippets.aspx.cs" %>

<HTML>
  <HEAD>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type=text/css>
		body, td, input, select, textarea, button
		{
			font-size: 11px;
			font-family: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
		}
		</style>
		<script src="common/fck_dialog_common.js" type="text/javascript"></script>
		<link href="common/fck_dialog_common.css" rel="stylesheet" type="text/css" />
		<script language="JavaScript" type="text/JavaScript">
			var oEditor		= window.parent.InnerDialogLoaded() ;
			var FCK			= oEditor.FCK ;
			var FCKLang		= oEditor.FCKLang ;
			var FCKConfig	= oEditor.FCKConfig ;
			var FCKDebug	= oEditor.FCKDebug ;
		
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
					oEditor.FCK.InsertHtml( codeSnippet.value) ;
				}
				parent.closeWnd() ;
			}

			function cancel() {
				//window.returnValue = null ;
				oEditor.FCK.InsertHtml(null) ;
			}			
		</script>
</HEAD>
	<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
		<table cellspacing="0" cellpadding="0" width="100%" border="0" ID="Table2">
        <tr>
			<td align=left bgcolor="#FF6600"><div style="padding:2px;"><img src="warning.gif"> <span style="color:#000">Selecting User Defined Fields [UDFs] from different 
            Groups for the same Campaign will cause Campaign not to work correctly.<br>Please choose UDFs from the Group you are trying to send 
            this Campaign</span></div></td>
		</tr>
		<TR>
			<TD vAlign=top style="padding-top:5px"><b>Select&nbsp;Group</b></TD></TR>
		<form id="BlastForm" Runat="Server">	
		<TR>
		    <TD vAlign=top><asp:dropdownlist class="formfield" id="Groups" runat="server" DataTextField="GroupName"	DataValueField="GroupID" AutoPostBack=True></asp:dropdownlist></TD>
		</TR>
		</form>	
		<TR><TD height=8></TD>
		</TR>
		<tr>
			<td valign="top">
				<b>Select&nbsp;Code&nbsp;Snippet</b><br>
				<select onChange="updateSelection()" style="WIDTH: 100%" name="selectSnippet" id=selectSnippet>
				<% Response.Write(CodeSnippets); %>
				</select>
				<input name="codeSnippet" id="codeSnippet" type="hidden" >
			</td>
		</tr>
		<TR><TD height=7></TD></TR>		
		<tr>
			<td align=right height=26 valign=top>
					<input type=button onClick="ok();" value="OK">&nbsp;
					&nbsp;<input type="button" value="Cancel" onClick="parent.Cancel();" >
				</td>
		</tr>
				
		</table>
	</body>
</HTML>
