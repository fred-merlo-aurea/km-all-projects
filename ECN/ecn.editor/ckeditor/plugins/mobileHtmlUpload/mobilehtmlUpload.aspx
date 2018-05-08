<%@ Page language="c#" AutoEventWireup="false" Inherits="ecn.communicator.ckeditor.dialog.mobilehtmlUpload" Codebehind="mobilehtmlUpload.aspx.cs" ValidateRequest="false" %>

<html>
  <head>
        <base target="_self" />
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

		    function ok() {
		        var result = document.getElementById('<%= hfContentSourceMobile.ClientID %>');
		        
		        if (window.opener) {
		            
		            if (window.opener.setUploadContentMobile != undefined) {
		                window.opener.setUploadContentMobile(result.value.toString());
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
        HTML Upload
        </title>
       
</head>
	<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
        <form runat="server">
	<table>
        <tr>
            <td>
                 File 
            </td>
            <td colspan="2">
               <asp:FileUpload ID="fileUpload1" runat="server" />
                <br />
            </td>
        </tr>
		<tr>
            <td>
                  
            </td>
			<td align="right" valign="top">
                <asp:Button ID="btnUpload" runat="server" Text="Submit" OnClick="btnUpload_Click"/>				
			</td>
            <td>                
                <input type="button" value="Cancel" onClick="cancel();" >
            </td>
		</tr>				
		</table>
            <input id="hfContentSourceMobile" type="hidden"  runat="server" value=""/>
          <asp:TextBox CssClass="dataOne" ID="htmlContentSource" runat="server" Visible="False" Text=""></asp:TextBox>
            </form>
	</body>
</html>
