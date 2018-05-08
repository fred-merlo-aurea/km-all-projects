<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="socialshare.aspx.cs" Inherits="ecn.communicator.contentmanager.feditor.dialog.socialshare" %>

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
		    var oEditor = window.parent.InnerDialogLoaded();
		    var FCK = oEditor.FCK;
		    var FCKLang = oEditor.FCKLang;
		    var FCKConfig = oEditor.FCKConfig;
		    var FCKDebug = oEditor.FCKDebug;

		    function ok() {
		        var finalMedia = "";
		        var fb = getobj("cbxFaceBook");
		        if (fb.checked) {
		            finalMedia = finalMedia + "|FaceBook|";
		        }
		        var tw = getobj("cbxTwitter");
		        if (tw.checked) {
		            finalMedia = finalMedia + "|Twitter|";
		        }
		        var li = getobj("cbxLinkedIn");
		        if (li.checked) {
		            finalMedia = finalMedia + "|LinkedIn|";
		        }
		        if (finalMedia.length == 0) {
		            cancel();
		            return;
		        } else {
		            oEditor.FCK.InsertHtml(finalMedia);
		        }
		        parent.closeWnd();
		    }

		    function cancel() {
		        //window.returnValue = null ;
		        oEditor.FCK.InsertHtml(null);
		    }

		    function getobj(id) {
		        if (document.all && !document.getElementById)
		            obj = eval('document.all.' + id);
		        else if (document.layers)
		            obj = eval('document.' + id);
		        else if (document.getElementById)
		            obj = document.getElementById(id);

		        return obj;
		    }		
		</script>
</HEAD>
	<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
		<table cellspacing="0" cellpadding="0" width="100%" border="0" ID="Table2">
        <tr>
			<td align=left bgcolor="#FF6600"><div style="padding:2px;"><img src="warning.gif"> <span style="color:#000">Use content without User Defined Fields [UDFs] as all UDFs will be 
            redacted</span></div></td>
		</tr>
		<TR>
			<TD vAlign=top style="padding-top:5px"><b>Select&nbsp;Media</b></TD></TR>
		<form id="BlastForm" Runat="Server">	
		<TR>
		    <TD vAlign=top>
                <asp:Image ID="imgFaceBook" runat="server" ImageUrl="facebook.jpg"/>
                <asp:CheckBox ID="cbxFaceBook" runat="server" Text="FaceBook" CssClass="formfield" />
            </TD>
		</TR>
        <TR>
		    <TD vAlign=top>
                <asp:Image ID="imgTwitter" runat="server" ImageUrl="twitter.jpg"/>
                <asp:CheckBox ID="cbxTwitter" runat="server" Text="Twitter" CssClass="formfield" />
            </TD>
		</TR>
        <TR>
		    <TD vAlign=top>
                <asp:Image ID="imgLinkedIn" runat="server" ImageUrl="linkedin.jpg"/>
                <asp:CheckBox ID="cbxLinkedIn" runat="server" Text="LinkedIn" CssClass="formfield" />
            </TD>
		</TR>
		</form>	
		<TR><TD height=8></TD>
		</TR>
		
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
