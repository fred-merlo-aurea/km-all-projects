<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  EnableEventValidation="false" Theme="Default" AutoEventWireup="true" CodeBehind="subscribe.aspx.cs" Inherits="PaidPub.subscribe" EnableViewState="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>SourceMedia Paid Subscription Form</title>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="subsScriptManager" runat="server">
        </asp:ScriptManager>
		 <div id="container">
            <div id="innerContainer">
                <div id="container-content">
                    <div id="banner">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid" BorderWidth="1px">
                        <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.jpg'); background-repeat: no-repeat">
                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                        </div>
                    </asp:Panel>
					<asp:Panel runat="server" ID="pnlSubscriptionPageDisplay" Visible="False">
       
                    <asp:Panel runat="server">
                        <asp:UpdatePanel ID="grdUpdatePanel" runat="server">
                            <ContentTemplate>
                                <p>
                                   <p class="borderBottom padBottom">
                                    <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                        <tbody>
                                           <tr>
                                                <td colspan="4">
                                                    <span class="label">* Indicates a required field. </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="25%">&nbsp;
                                                </td>
                                                <td width="75%">&nbsp;
                                                </td>

                                            </tr>
                                            <tr valign="top">
                                                <td colspan="2">
                                                    <font face="verdana,arial,helvetica"><b>Subscriber Information:</b></font><br />
                                                    <br />
                                                </td>
                                            </tr>
											
											
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">* Email:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtemail" runat="server" Style="width: 250px" MaxLength="100" TabIndex="1" AutoPostBack="True" OnTextChanged="txtemail_OnTextChanged" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtemail" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="&lt;&lt; Invalid Email Address"
                                                        EnableClientScript="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        ControlToValidate="txtemail"></asp:RegularExpressionValidator>
                                                </td>

                                            </tr>
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">* First Name:</span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfirstname" Style="width: 250px" MaxLength="50" runat="server" TabIndex="2" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtfirstname" />
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">* Last Name:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtlastname" Style="width: 250px" MaxLength="50" runat="server" TabIndex="3" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtlastname"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">&nbsp;&nbsp;Company:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtcompany" Style="width: 250px" MaxLength="75" runat="server" TabIndex="4" />
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">* Phone Number:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtphone" Style="width: 150px" MaxLength="20" runat="server" TabIndex="5" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtphone"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td align="left">
                                                    <span class="label">&nbsp;&nbsp;Fax Number:</span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="fax" Style="width: 150px" MaxLength="20" runat="server" TabIndex="6" />
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <br />
                                    <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                        <tbody>
                                            <tr valign="baseline">
                                                <td colspan="4" align="center">
                                                    <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td align="center">
                                                                <font face="verdana,arial,helvetica"><b>Billing Address</b></font>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Button ID="btnCopyAddres" Text="Copy Address" Width="110px" runat="server"
                                                                    OnClick="btnCopyAddres_Click" CausesValidation="false" /></td>
                                                            <td align="center">
                                                                <font face="verdana,arial,helvetica"><b>Shipping Address</b></font>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                            <tr valign="baseline">
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td width="12%">
                                                    <span class="label">* Address:</span>
                                                </td>
                                                <td width="38%">
                                                    <asp:TextBox ID="txtBillingAddress" runat="server" Style="width: 200px" MaxLength="100" TabIndex="7" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" EnableClientScript="true"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="txtBillingAddress"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="12%">
                                                    <span class="label">* Address:</span>
                                                </td>
                                                <td width="38%">
                                                    <asp:TextBox ID="txtShippingAddress" Style="width: 200px" MaxLength="100" runat="server" TabIndex="12" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtShippingAddress"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td>
                                                    <span class="label">&nbsp;&nbsp;Address 2:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBillingAddress2" runat="server" Style="width: 200px" MaxLength="100" TabIndex="8" />
                                                </td>
                                                <td align="left">
                                                    <span class="label">&nbsp;&nbsp;Address 2:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShippingAddress2" Style="width: 200px" MaxLength="100" runat="server" TabIndex="13" />
                                                </td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td>
                                                    <span class="label">* City:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBillingCity" runat="server" Style="width: 150px" MaxLength="50" TabIndex="9" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtBillingCity"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <span class="label">* City:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShippingCity" Style="width: 150px" MaxLength="50" runat="server" TabIndex="14" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtShippingCity"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td>
                                                    <asp:Label ID="lblBillingState" runat="server" CssClass="label">* State:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="panelDropDownBillingState" runat="server">
                                                          <asp:DropDownList Style="width: 150px" ID="drpBillingState" runat="server" TabIndex="10"  AutoPostBack="True" OnSelectedIndexChanged="stateChange" >
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="drpBillingState"></asp:RequiredFieldValidator>

                                                    </asp:Panel>
                                                    <asp:Panel ID="panelTextBoxBillingState" runat="server" Visible="false">
                                                        <asp:TextBox ID="txtBillingState" runat="server" Style="width: 150px" MaxLength="50" TabIndex="10" />
                                                    </asp:Panel>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStateProv" runat="server" CssClass="label">* State:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="panelDropDownShippingState" runat="server">
                                                        <asp:DropDownList Style="width: 150px" ID="drpShippingState" runat="server" TabIndex="15" AutoPostBack="True" OnSelectedIndexChanged="stateChange">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="drpShippingState"></asp:RequiredFieldValidator>

                                                    </asp:Panel>
                                                    <asp:Panel ID="panelTextBoxShippingState" runat="server" Visible="false">
                                                        <asp:TextBox ID="txtShippingState" Style="width: 150px" MaxLength="50" runat="server" TabIndex="15" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr valign="baseline">
                                                <td>
                                                    <span class="label">* Zip:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBillingZip" runat="server" Style="width: 150px" MaxLength="10" TabIndex="11" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtBillingZip"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <span class="label">* Zip Code:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShippingZip" runat="server" Style="width: 150px" MaxLength="10" TabIndex="16" AutoPostBack="True" OnTextChanged="ZipTextChanged" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                        ControlToValidate="txtShippingZip"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </p>
                                    <p class="borderBottom padBottom">
                                       &nbsp;<asp:Panel ID="pnlCountry" runat="server" Visible="false">
                                        <div>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Country:" ID="lblFilterCoun" CssClass="label" runat="server" />&nbsp;
                                                        <asp:DropDownList ID="drpCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ListCountryChanged"></asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdateProgress ID="udProgress" runat="server" AssociatedUpdatePanelID="grdUpdatePanel">
                                                            <ProgressTemplate>
                                                                <sub>
                                                                    <img border="0" src="./images/animated-loading-orange.gif" /></sub><span style="font-size: 10px; color: Black">Please wait...</span>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <p>
                                        <asp:HiddenField ID="hfProductCount" runat="server" Value="0" />

                                        <table cellspacing="0" cellpadding="0" border="1" class="grid" style="width: 100%; border-collapse: collapse;">
                                            <tr>
                                                <th align="center">Cover</th>
                                                <th align="center">title</th>
                                                <th align="center">Term</th>
                                                <th align="center">Price</th>
                                                <th align="center">Taxes</th>
                                                <th align="center">Total</th>

                                            </tr>
                                            <asp:PlaceHolder ID="plStandard" runat="server">
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <asp:Image ID="lblStandardCoverImage" runat="server" ImageUrl="" orderWidth="0px" Width="114px" />
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:Label ID="lblStandardTitle" runat="server" Text="Standard" />
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:DropDownList ID="drpStandardTerm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ListStandardTermChanged" EnableViewState="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:RadioButton ID="rbStandardPrint" runat="server" AutoPostBack="true" Checked="false"
                                                            Visible="true" OnCheckedChanged="rbStandardCheckChanged" />
                                                        <asp:Label ID="lblStandardPrice" runat="server" Text=""
                                                            Visible="true"></asp:Label>
                                                    </td>
													 <td align="center" valign="middle">
                                                        <asp:Label ID="lblStandardTaxes" runat="server" Text=""
                                                            Visible="<%#IsStandardVisible%>"></asp:Label>
                                                    </td>
													 <td align="center" valign="middle">
                                                        <asp:Label ID="lbltotalStandards" runat="server" Text=""
                                                            Visible="<%#IsStandardVisible%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="plPremium" runat="server">
                                                <tr class="gridAltColor">
                                                    <td align="center" valign="middle">
                                                        <asp:Image ID="lblPremiumCoverImage" runat="server" ImageUrl="" BorderWidth="0px" Width="114px" />
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:Label ID="lblPremiumTitle" runat="server" Text="Premium" />
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:DropDownList ID="drpPremiumTerm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ListPremiumTermChanged"  EnableViewState="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" valign="middle">
                                                        <asp:RadioButton ID="rbPremiumPrint" runat="server" AutoPostBack="true" Checked="false"
                                                            Visible="true" OnCheckedChanged="rbPremimCheckChanged" />
                                                        <asp:Label ID="lblPremiumPrice" runat="server" Text=""
                                                            Visible="true"></asp:Label>
                                                    </td>
													 <td align="center" valign="middle">
                                                        <asp:Label ID="lblPremiumTaxes" runat="server" Text=""
                                                            Visible="<%#IsPremiumVisible%>"></asp:Label>
                                                    </td>
													 <td align="center" valign="middle">
                                                        <asp:Label ID="lbltotalPremiums" runat="server" Text=""
                                                            Visible="<%#IsPremiumVisible%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                        </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <p class="borderBottom padBottom">
                            <table cellspacing="3" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <img src="https://www.ecn5.com/ecn.Images/images/testMyCampaign.gif" />&nbsp;<font
                                            face="verdana,arial,helvetica"><b>Payment Information:</b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%">
                                        <span class="label">* Name as it appears on the card:</span>
                                    </td>
                                    <td width="35%">
                                        <asp:TextBox ID="user_CardHolderName" runat="server" Width="130" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" EnableClientScript="true"
                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="user_CardHolderName"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label">* Credit Card Type:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="user_CCType" runat="server" Width="130">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="MasterCard"></asp:ListItem>
                                            <asp:ListItem Value="Visa"></asp:ListItem>
                                            <asp:ListItem Value="Amex"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_CCType"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label">* Card Number (No dashes):</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="user_CCNumber" runat="server" Width="130" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_CCNumber"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <span class="label">* Expiration Date:</span>
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList ID="user_Exp_Month" runat="server">
                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="01">Jan</asp:ListItem>
                                            <asp:ListItem Value="02">Feb</asp:ListItem>
                                            <asp:ListItem Value="03">Mar</asp:ListItem>
                                            <asp:ListItem Value="04">Apr</asp:ListItem>
                                            <asp:ListItem Value="05">May</asp:ListItem>
                                            <asp:ListItem Value="06">Jun</asp:ListItem>
                                            <asp:ListItem Value="07">Jul</asp:ListItem>
                                            <asp:ListItem Value="08">Aug</asp:ListItem>
                                            <asp:ListItem Value="09">Sep</asp:ListItem>
                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                            <asp:ListItem Value="12">Dec</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_Exp_Month" Display="Dynamic"></asp:RequiredFieldValidator><asp:DropDownList
                                            ID="user_Exp_Year" runat="server">
                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="16">2016</asp:ListItem>
                                            <asp:ListItem Value="17">2017</asp:ListItem>
                                            <asp:ListItem Value="18">2018</asp:ListItem>
                                            <asp:ListItem Value="19">2019</asp:ListItem>
                                            <asp:ListItem Value="20">2020</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_Exp_Year"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <span class="label">&nbsp;&nbsp;Card Verification Number:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="user_CCVerfication" runat="server" />
                                        &nbsp;&nbsp;&nbsp;<img src="https://www.ecn5.com/ecn.Images/images/img_credit_card.gif"
                                            width="182" height="55" align="textTop"><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18"
                                        runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="user_CCVerfication"></asp:RequiredFieldValidator>--%>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </p>
                        <p>
                            <center>
                                <table align="center" border="0">
                                    <tr>
                                        <td align="center">	                                       
                                            <asp:Panel ID="pnlButton" runat="server" Visible="true">                                          
												  <asp:Button ID="btnSecurePayment" Text="Submit" Width="110px" runat="server"
                                                    OnClick="btnSecurePayment_Click" OnClientClick="doubleSubmitCheck()"/>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlButtonDisabled" runat="server" Visible="true">
                                                <asp:Button ID="btnSecurePaymentDisabled" Text="Processing...." Width="110px" runat="server"
                                                    Enabled="false" Style="visibility: hidden;" PostBackUrl="~/thankyou.aspx" />
                                            </asp:Panel>
                                        </td>
                                        <td align="center" valign="top">
                                            <asp:Panel ID="pnlbtnReset" runat="server" Visible="true">
                                                <asp:Button ID="btnReset" Text="Reset" runat="server" Width="110px" CausesValidation="false"
                                                    OnClick="btnReset_Click" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlReset" runat="server" Visible="true">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                    </asp:Panel>
				
						</asp:Panel>
						<asp:Label runat="server" ID="lblResult"></asp:Label>
					<asp:Label runat="server" ID="lblResponse"></asp:Label>
                </div>
                <!-- end container-content -->
                <div id="footer">
                    <table align="center" style="width:100%">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--end footer-->
            </div>
        </div>
	</form>
</body>

<script language="javascript" type="text/javascript">
    function doubleSubmitCheck() {
        var validation = Page_ClientValidate();

        if (validation) {
            document.getElementById("<%= btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%= btnSecurePayment.ClientID %>").style.display = 'none';
            document.getElementById("<%= btnSecurePaymentDisabled.ClientID %>").style.visibility = 'visible';
            document.getElementById("<%= btnReset.ClientID %>").style.visibility = 'hidden';
            return true;
        }
        else {
            return false;
        }
    }
</script>

</html>
