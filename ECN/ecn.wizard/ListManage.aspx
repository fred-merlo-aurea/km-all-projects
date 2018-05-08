<%@ Page language="c#" Codebehind="ListManage.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.ListManage" %>
<script language=javascript>
	function SetWorkSheet()
	{
		var rdolist = document.getElementsByName("rblFileType")

		//if (rdolist[1].checked)
		//{
		//	document.getElementById("txtSheetName").value = "";
		//}
		//else 
		if (rdolist[2].checked)
		{
			if(document.getElementById("txtSheetName").value == "")
			{
				document.getElementById("txtSheetName").value = "Sheet1";
			}
		} 
	}
	
	function openwindow()
	{
		window.open("PricingChart.aspx",null, "height=200,width=400,status=no,toolbar=no,menubar=no,location=no,left=400, top=300");
	}

</script>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="images/img_manage_list.gif"></td>
				<td width="564" valign="top" align="left" >
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px"><BR>
						<div class="dashed_lines1" style="FONT-SIZE: 14px">
							<table cellSpacing="0" cellPadding="0" width="100%">
								<tr>
									<td align="left"><strong>Manage Your Existing Email Lists.</strong></td>
									<td align="right">
									<asp:imagebutton id="btnPricing" ImageUrl="images/btn_view_pricing.gif" BorderWidth="0"
												Runat="server"></asp:imagebutton>
									</td>
								</tr>
							</table>
						</div>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 10px">Select 
							a list to manage.<br>
							<br>
							<asp:dropdownlist id="ddlList" runat="server" CssClass="blue_border_box"></asp:dropdownlist>
							<div style="PADDING-TOP: 5px"><br>
								<table cellSpacing="0" cellPadding="0">
									<tr>
										<td><asp:imagebutton id="btnViewEdit" ImageUrl="images/btn_view_edit_email_list.gif" BorderWidth="0"
												Runat="server"></asp:imagebutton>&nbsp;</td>
										<td><asp:imagebutton id="btnDelete" ImageUrl="images/btn_delete_list.gif" Runat="server"></asp:imagebutton></td>
									</tr>
								</table>
							</div>
						</div>
						<div class="dashed_lines1" style="PADDING-RIGHT: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 20px"><strong>Add 
								Emails to a List</strong>
						</div>
						<P><asp:label id="errlabel" runat="server" ForeColor="Red"></asp:label></P>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 10px">To protect you and to enhance your results, Knowledge Marketing and its Affiliated Partners expressly prohibit the use of purchased or rented lists with this communication tool. You must have prior permission to communicate via email with the persons you wish to contact utilizing this process as required by Federal Law. <!--To maximize your potential for growing your own internal list, or for more information please contact us 866-844-6275 or go to <a href="http://www.knowledgemarketing.com" target="_blank">www.knowledgemarketing.com</a>.-->
						</div>

						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 10px">Import 
							Data to my List From:<br>
							<input class="blue_border_box" id="fBrowse" type="file" name="fBrowse" runat="server">
						</div>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 90px; PADDING-BOTTOM: 0px; PADDING-TOP: 5px">
							<asp:radiobuttonlist id="rblFileType" runat="server">
								<asp:ListItem Value=".csv" Text="CSV"></asp:ListItem>
								<asp:ListItem Value=".xls" Text="Excel"></asp:ListItem>
							</asp:radiobuttonlist></div>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 5px">Worksheet 
							Name (Excel Only)<br>
							<asp:textbox id="txtSheetName" runat="server" CssClass="blue_border_box"></asp:textbox>&nbsp;<!--<asp:Button CssClass="blue_border_box" id="btnAddMails" runat="server" Text="Submit" Height="20px"></asp:Button>-->
						</div>
						<div style="PADDING-RIGHT: 130px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 5px">If 
							you are importing an Excel file, enter the name of the worksheet exactly as it 
							appears in your file.
						</div>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 20px"
							align="left"><asp:imagebutton id="btnImportData" ImageUrl="images/btn_import_data.gif" Runat="server"></asp:imagebutton></div>
						<br>
					</div>
				</td>
			</tr>
		</table>
	</form>
</center>
