<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Preview.ascx.cs" Inherits="ecn.wizard.wizard.Preview" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--content-->
<div style="PADDING-RIGHT:20px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
	<div style="PADDING-RIGHT:0px; FONT-SIZE:14px; PADDING-BOTTOM:0px; PADDING-TOP:0px" class="dashed_lines1"><strong>Preview:</strong></div>
	<div style="PADDING-RIGHT:20px; PADDING-LEFT:10px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
		Please review your email carefully. If you are satisfied, continue on to step 
		4. If you would like to make changes/ corrections; please use the "Previous" button on the bottom left to go back.
	</div>
</div>
<!--eof content--></TD></TR> 
<!--text editor row-->
<tr>
	<td colspan="2">
		<div align="center" style="PADDING-RIGHT: 0px; MARGIN-TOP: 10px; PADDING-LEFT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px">
			<!--editor box-->
			<div align="center" style="WIDTH:580px">
				<asp:Label id="lblError" runat="server" Font-Names="Arial" Font-Bold="True" ForeColor="red"></asp:Label>
				<asp:Label id="previewLbl" runat="server" Font-Names="Arial"></asp:Label>
			</div>
			<!--eof editor box-->
		</div>
	</td>
</tr>
