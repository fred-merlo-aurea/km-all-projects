<%@ Page language="c#" AutoEventWireup="true" Inherits="ecn.communicator.contentmanager.ckeditor.dialog.dynamicTag" Codebehind="dynamicTag.aspx.cs" ValidateRequest="false"%>

<html>
  <head>

		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <base target="_self" />
		<style type="text/css">
		body, td, input, select, textarea, button
		{
			font-size: 11px;
			font-family: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
		}
		</style>
      
		<script src="common/fck_dialog_common.js" type="text/javascript"></script>
		<link href="common/fck_dialog_common.css" rel="stylesheet" type="text/css" />
		<script language="JavaScript" type="text/JavaScript">

        	function ok() {
        	    var result = document.getElementById("hfDynamicTag");


                //window opener for Chrome and Firefox
		        if (window.opener) {
        	        if (result.value != '-Select-') {
		                if (window.opener.setValue != undefined) {
		                    window.opener.setValue(result.value.toString());
		                }
		            }
        	        }
        	    
		        this.close();
			}

			function cancel() {
		        window.returnValue = "";
		        this.close();
		    }			
		</script>
          <title>
        Insert Dynamic Tag
        </title>
       
</head>
	<body>
         <form id="Form1" runat="server">
        <table>
            <tr>
                <td>
                    Dynamic Tag
                </td>
                <td>
                    <asp:DropDownList ID="drpDynamicTag" AppendDataBoundItems="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDynamicTag_OnSelectedIndexChanged" DataValueField="TagValue" DataTextField="Tag">
                        <asp:ListItem>-Select-</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td colspan="2" align="center">
                    <asp:HiddenField ID="hfDynamicTag" runat="server" Value="-Select-" />
                    	<input type=button onClick="ok();" value="OK">&nbsp;
					&nbsp;<input type="button" value="Cancel" onClick="cancel();" >
                </td>
            </tr>
        </table>
             </form>
	</body>
</html>
