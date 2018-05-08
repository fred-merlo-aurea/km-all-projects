<%@ Page language="c#" Codebehind="CarsUnTypedXml.aspx.cs" AutoEventWireup="True" Inherits="UsefulControlsTest.CarsUnTypedXml" EnableEventValidation="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat="server">
		<title>Useful Controls</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="Styles.css" type="text/css" rel="stylesheet">
		<style type="text/css">
			/****************************
			We can override the settings in the linked
			Style Sheet locally, without affecting any of
			the other pages.
			****************************/
			.ccblcategory{
				font-size: 10.5pt;
				font-family: Verdana, Geneva, Sans-Serif;
				font-weight: 900;
				padding-top: 4px;
				background-color : #3399FF;
				color: #ffffff;
				text-align: left;
				padding: 4px 0px 4px 10px;
			}
			TD{
				background-color: #ffffcc;
			}
			input.ccblcheckbox{
				margin-left: 4px;
			}
			TABLE.ccbltable{
				border: 1px solid #666666;
				margin-left: 20px;
			}
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h1>Useful Controls</h1>
			<p><asp:label id="Label1" runat="server">This page loads the data from an XML file without a schema into an un-typed DataSet. <br>Click the button below to test the checkbox values.</asp:label></p>
			<JF:categorizedcheckboxlist AutoPostBack=true id="CategorizedCheckBoxList1" runat="server" columns="2" rowcssclass="ccblrow" checkboxcssclass="ccblcheckbox"
				categorycssclass="ccblcategory" textcssclass="ccbltext" tablecssclass="ccbltable" sharedtable="true" datatextcolumn="Model"
				datavaluecolumn="CarModelPK" datacategorycolumn="Make"></JF:categorizedcheckboxlist>
			<p><asp:checkbox id="chkShowList" text="Show CategorizedCheckBoxList on PostBack" checked="True"
					runat="server"></asp:checkbox></p>
			<p><asp:button id="btnTestValues" runat="server" text="Test Checkbox Values" onclick="btnTestValues_Click"></asp:button></p>
			<p>&nbsp;</p>
			<p><a href="CarsUnTypedXml.aspx">reset</a></p>
			<asp:DropDownList ID="drp1" runat="server" AutoPostBack="true">
			    <asp:ListItem Text="1" Value="1"></asp:ListItem>
			    <asp:ListItem Text="2" Value="2"></asp:ListItem>
			</asp:DropDownList>
		</form>
	</body>
</html>
