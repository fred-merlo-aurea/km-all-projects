<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Codebehind="CSC_RFQ.aspx.cs"
    Inherits="CanonESubscriptionForm.forms.CSC_RFQ" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CSC - RFQ</title>
    <link href="http://www.powderbulk.com/App_Themes/Basic/StyleSheet.css" rel="stylesheet"
        type="text/css" />
    <style>
        .style3 { vertical-align:middle;font-size: 8pt; font-weight: normal; color: #444444;margin-bottom:4px}
        .style4 { vertical-align:middle;font-size: 9pt; font-weight: bold; color: #444444; padding-left:5px;padding-right:5px;}
        label
        {
            font-size:9pt;
            color: #444444;
        }
    </style>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript">

   function validateForm() 
{

    var allOk = false;
	allOk = 
		(
		svValidator("First Name", document.forms[0].txtFirstName.value) && 
		svValidator("Last Name", document.forms[0].txtLastName.value) && 
		svValidator("Company", document.forms[0].txtCompany.value)  &&
		svValidator("Address", document.forms[0].txtAddress.value) && 
		svValidator("City", document.forms[0].txtCity.value) &&
		svValidator("State", document.forms[0].drpState.value) && 
		svValidator("Zip", document.forms[0].txtZip.value) &&
		 svValidator("Country", document.forms[0].txtCountry.value) &&
		 svValidator("Phone", document.forms[0].txtPhone.value) &&
		svValidator("Email Address", document.forms[0].txtEmail.value));

	if (allOk) 
	{
        var x = document.forms[0].txtEmail.value;

        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(x))
        {	
	        alert('Invalid Email address');
	        allOk = false;
        }
    }
	if (allOk)
    {
       return true;
    }
    else
        return false;
}

    </script>

</head>
<body style="text-align: left;">
    <form name="frmsub1" id="frmsub1" runat="server">
        <asp:Panel ID="pnlError" Visible="false" runat="server">
            <asp:Label ID="lblErrMessage" runat="server" Font-Bold="true" ForeColor="red"></asp:Label><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlCategories" runat="server" Visible="true" Width="100%">
            <span id="ctl00_lblDevoted" class="style3">The PBE RFQ program will allow you the opportunity
                to request a quote from one or multiple industry suppliers by equipment type, the
                specific supplier, and your exact needs – all in a single request.</span>
            <br />
            <br />
            <span class="style3" style="width: 100%">Please select all of the equipment catagories
                from those shown below for which you would like to receive a
                <br />
                quote(s).</span>
            <br />
            <asp:CheckBoxList ID="chkCategories" runat="server" ForeColor="#415067" RepeatColumns="2"
                RepeatDirection="Vertical">
            </asp:CheckBoxList>
        </asp:Panel>
        <asp:Panel ID="pnlAdvertisers" runat="server">
            <div style="height: 25em;">
                <span class="style3">Please select the equipment supplier(s) of interest for
                    <asp:Label ID="lblCategory" runat="Server" Font-Bold="true" Font-Italic="true" Font-Size="x-Small"></asp:Label>
                    from the list below for which you would like to receive a quote. </span>
                <br />
                <br />
                <asp:CheckBoxList ID="chkAdvertisers" runat="server" ForeColor="#415067">
                </asp:CheckBoxList>
                <br />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlSubscriptionForm" runat="server" Visible="false">
            <p>
                <span class="style3">Please understand that this is a generic form. Some information
                    requested may not be necessary for all equipment; at the same time, it may not be
                    fully complete for all vendors. It’s a good place to begin dialogues with PBE advertisers.</span>
            </p>
            <br />
            <div id="profile">
                <table border="0" cellpadding="4" cellspacing="2">
                    <tr>
                        <td width="25%" class="style3">
                            First Name <font color="red">*</font></td>
                        <td width="75%">
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Last Name <font color="red">*</font></td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Job Title</td>
                        <td>
                            <asp:TextBox ID="txtJobTitle" runat="server" Width="300px" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Company <font color="red">*</font></td>
                        <td>
                            <asp:TextBox ID="txtCompany" runat="server" Width="300px" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Address <font color="red">*</font></td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" Width="300px" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            City <font color="red">*</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            State/Province <font color="red">*</font>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpState" runat="server" CssClass="style3">
                               
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Zip/ Postal Code <font color="red">*</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtZip" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Country <font color="red">*</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Phone # <font color="red">*</font></td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Fax #</td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Cell Phone #</td>
                        <td>
                            <asp:TextBox ID="txtCellPhone" runat="server" CssClass="style3"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Email <font color="red">*</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="250px" CssClass="style3"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlSubscriptionForm2" runat="server" Visible="false">
            <p>
                <span class="style3"></span>
            </p>
            <br />
            <table border="0" cellpadding="3" cellspacing="3">
                <tr>
                    <td colspan="2" class="style3">
                        Material made or to be Produced? (Please be Specific)</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtMaterialProduced" runat="server" CssClass="style3" Width="350px"
                            MaxLength="250"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style3" width="25%" >
                        Material Weight</td>
                    <td class="style3" width="75%" >
                        <asp:TextBox ID="txtMaterialWeightCF" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;
                        per cubic foot <font class="style4">(or)</font>
                        <asp:TextBox ID="txtMaterialWeightCM" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;per
                        cubic meter</td>
                </tr>
                <tr>
                    <td colspan="2" class="style3">
                        Total weight amount of material needed to be made, or moved, or processed, or stored?
                        (Hourly, daily or weekly.)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtTotalWeight" runat="server" CssClass="style3" Width="350px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style3">
                        Material temperature</td>
                    <td>
                        <asp:TextBox ID="txtMaterialTemp" runat="server" CssClass="style3"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" class="style3">
                        Material characteristics:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBoxList ID="chkMatericalChar" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                            Width="100%" Font-Names="verdana" Font-Size="XX-Small">
                            <asp:ListItem>Powder</asp:ListItem>
                            <asp:ListItem>Free Flowing</asp:ListItem>
                            <asp:ListItem>Friable</asp:ListItem>
                            <asp:ListItem>Flake</asp:ListItem>
                            <asp:ListItem>Sticky</asp:ListItem>
                            <asp:ListItem>Toxic</asp:ListItem>
                            <asp:ListItem>Fibrous</asp:ListItem>
                            <asp:ListItem>Dusty</asp:ListItem>
                            <asp:ListItem>Granular</asp:ListItem>
                            <asp:ListItem>Pellet</asp:ListItem>
                            <asp:ListItem>Sluggish</asp:ListItem>
                            <asp:ListItem>Lumpy</asp:ListItem>
                            <asp:ListItem>Abrasive</asp:ListItem>
                            <asp:ListItem>Explosive</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:CheckBoxList></td>
                </tr>
                <tr>
                    <td class="style3" align="right">
                        (Specify Other)
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtOther" runat="server" CssClass="style3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table border="0" cellpadding="5" cellspacing="2">
                            <tr>
                                <td class="style3" width="26%">
                                    Bulk Density (if known)&nbsp;</td>
                                <td class="style3">
                                    <asp:TextBox ID="txtBulkDensitylbs" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;lbs
                                </td>
                                <td class="style3">
                                    <font class="style4">(or)</font>
                                </td>
                                <td class="style3" colspan="3">
                                    <asp:TextBox ID="txtBulkDensitykgs" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;kgs
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Particle Size (if known)&nbsp;</td>
                                <td class="style3">
                                    <asp:TextBox ID="txtParticleSizeM" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;microns
                                </td>
                                <td class="style3">
                                    <font class="style4">(or)</font>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="txtParticuleSizeI" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;inches
                                </td>
                                <td class="style3">
                                    <font class="style4">(or)</font>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="txtParticuleSizeMS" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;mesh
                                    size
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Moisture content</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtMoistureContent" runat="server" CssClass="style3" Width="60"></asp:TextBox>&nbsp;%</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="style3">
                        What other equipment will the equipment you are considering purchasing interact
                        with?</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtEquimentcomments" runat="server" Columns="60" CssClass="style3"
                            Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlThankYou" runat="server" Visible="false">
            <br />
            <br />
            <br />
            <asp:Label ID="lblThankyou" runat="Server" Font-Bold="true" ForeColor="#333333">Thank you for your
                                                request. This information will be forwarded to the vendors you have indicated.</asp:Label>
            <br />
            <br />
        </asp:Panel>
        <div style="text-align: center">
            <asp:Button ID="btnback" runat="server" Text="Back" OnClick="btnback_Click" />&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />&nbsp;&nbsp;&nbsp;<asp:Button
                    ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_Click" />
        </div>
        <br />
    </form>
</body>
</html>
