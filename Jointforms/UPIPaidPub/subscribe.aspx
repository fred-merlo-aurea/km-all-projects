<%@ Page Language="C#" EnableEventValidation="false" Theme="Default" AutoEventWireup="true"
    CodeBehind="subscribe.aspx.cs" Inherits="upipaidpub.subscribe" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>UPI Paid Subscription Form</title>
        
    <script src="scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <link type="text/css" href="CSS/styles.css" rel="Stylesheet" />
    <script src="scripts/Validation.js" type="text/javascript"></script>

    <script src='https://www.google.com/recaptcha/api.js' type="text/javascript"></script>

    <style>
        .modalBackground {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .popupbody {
            /*background:#fffff url(images/blank.gif) repeat-x top;*/
            z-index: 101;
            background-color: #FFFFFF;
            font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="subsScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="phErrorTop" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
            BorderWidth="1px">
            <div style="padding-left: 65px; padding-right: 15px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.png'); background-repeat: no-repeat">
                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblErrorMessageTop" runat="Server" ForeColor="Red"></asp:Label>
            </div>
        </asp:Panel>
        <div id="container" runat="server">
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
                    <asp:Panel runat="server">
                        <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
                            BorderWidth="1px">
                            <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.png'); background-repeat: no-repeat">
                                &nbsp;&nbsp;<asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                            </div>
                        </asp:Panel>
                        <p>
                            &nbsp;<asp:Panel ID="pnlCountry" runat="server" Visible="false">
                                <div>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="right" colspan="2">
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
                                &nbsp;<asp:UpdatePanel ID="grdUpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdMagazines" runat="server" AutoGenerateColumns="false" HorizontalAlign="Center"
                                            ShowFooter="true" SkinID="skitems" OnRowDataBound="grdMagazines_RowDataBound"
                                            OnRowCreated="grdMagazines_RowCreated">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:ImageField DataImageUrlField="coverImage" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" BorderColor="#FDFCC4" />
                                                </asp:ImageField>
                                                <asp:BoundField DataField="title" HeaderText="Description" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" BorderColor="#FDFCC4" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Persons" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="drpQuantity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpQuantity_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblQuantity" runat="server" Text="1" Visible="false" />
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" BorderColor="#FDFCC4" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="22%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"groupID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblCustID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"custID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblPubCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"pubcode") %>'
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkPrint" runat="server" AutoPostBack="true" Checked="true" Enabled="false"
                                                            OnCheckedChanged="chBoxPrint_CheckedChanged" Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"showprintcheckBox")) %>' />
                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"oneyearprice") %>'
                                                            Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"showprintcheckBox")) %>' />
                                                        <asp:HiddenField ID="hdDescription" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"description") %>' />
                                                        <asp:HiddenField ID="hdTitle" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"title") %>' />
                                                        <asp:Label ID="lblMessage" Font-Bold="true" runat="server" Text='' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="40%" />
                                                    <FooterTemplate>
                                                        <span class="label">
                                                            <asp:Label ID="lblDiscountPromoCode" runat="server" Text="Promotional Discount" Visible="false" />
                                                            <br />
                                                            <br />
                                                            Total Price&nbsp;</span>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOneYearPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"oneyearprice") %>'
                                                            Visible="true"></asp:Label>
                                                        <asp:Label ID="lblTwoYearPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"twoyearprice") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblThreeYearPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"threeyearprice") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblFourYearPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"fouryearprice") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblFiveYearPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"fiveyearprice") %>'
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <span></span><span></span><span class="label">
                                                            <asp:Label ID="lblDiscountPromocodePrice" runat="server" Text="$0.00" Font-Bold="false"
                                                                Visible="false" />
                                                        </span>
                                                        <br />
                                                        <br />
                                                        <span class="label">$<asp:Label ID="lblTotalAmount" Text="0.00" runat="server" Visible="true" /></span>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <p>
                                </p>
                                <p class="padBottom">
                                    <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                        <tbody>
                                            <tr>
                                                <td style="padding-bottom: 5px;">
                                                    <span class="label">* Indicates a required field. </span>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlContactInfo" runat="server" Visible="false">
                                                <tr valign="top">
                                                    <td colspan="2">
                                                        <font face="verdana,arial,helvetica"><b>Contact Information:</b></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="30%">
                                                        <span class="label">* Email:</span>
                                                    </td>
                                                    <td width="70%">
                                                        <asp:TextBox ID="email" runat="server" Width="250" OnTextChanged="email_TextChanged" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="email"
                                                            Display="Dynamic" EnableClientScript="true" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email"
                                                            Display="Dynamic" EnableClientScript="true" ErrorMessage="&lt;&lt; Invalid Email Address"
                                                            Font-Bold="True" Font-Size="11px" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label">* First Name:</span>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="first" runat="server" Width="250" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="first"
                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label">* Last Name:</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="last" runat="server" Width="250" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="last"
                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label">* Phone Number:</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="phone" runat="server" Width="150" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="phone"
                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label">&nbsp;&nbsp;Fax Number:</span>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="fax" runat="server" Width="150" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label">&nbsp;&nbsp;Company:</span>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCompany" runat="server" Width="150" />
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                        </tbody>
                                    </table>
                                    <br />
                                    <asp:UpdatePanel ID="pnlUpdateAddress" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <asp:Panel ID="pnlBillingAddress" runat="server" Visible="false">
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                                                <tr>
                                                                    <td colspan="2" style="padding-bottom: 10px;">
                                                                        <span class="label"><font color="black" face="verdana,arial,helvetica"><b>Billing Address
                                                                        (for credit card)</b></font></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Address1:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBillingAddress1" runat="server" Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBillingAddress1"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Address2:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBillingAddress2" runat="server" Width="200" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">City:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBillingCity" runat="server" Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBillingCity"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <asp:Panel ID="pnlBillingState" runat="server">
                                                                    <tr>
                                                                        <td>
                                                                            <span class="label">State/Province:</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpBillingState" runat="server" Width="200" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="drpBillingState"
                                                                                ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlBillingStateInt" runat="server" Visible="false">
                                                                    <tr>
                                                                        <td>
                                                                            <span class="label">State/Province:</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBillingStateInt" runat="server" Width="150" />
                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Zip/Postal Code:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBillingZip" runat="server" Width="100" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtBillingZip"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Country:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="drpBillingCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBillingCountry_SelectedIndexChanged"
                                                                            Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="drpBillingCountry"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlShippingAddress" runat="server" Visible="false">
                                                        <td width="50%">
                                                            <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                                                <tr>
                                                                    <td colspan="2" style="padding-bottom: 10px;">
                                                                        <span class="label"><font color="black" face="verdana,arial,helvetica"><b>Shipping Address</b></font></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Address1:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtShippingAddress1" runat="server" Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtShippingAddress1"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Address2:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtShippingAddress2" runat="server" Width="200" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">City:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtShippingCity" runat="server" Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtShippingCity"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <asp:Panel ID="pnlShippingStateInt" runat="server" Visible="false">
                                                                    <tr>
                                                                        <td>
                                                                            <span class="label">State/Province:</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtShippingStateInt" runat="server" Width="150" />
                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlShippingState" runat="server">
                                                                    <tr>
                                                                        <td>
                                                                            <span class="label">State/Province:</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpShippingState" runat="server" Width="200" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="drpShippingState"
                                                                                ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Zip/Postal Code:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtShippingZip" runat="server" Width="100" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtShippingZip"
                                                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" Font-Size="12pt"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="label">Country:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="drpShippingCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpShippingCountry_SelectedIndexChanged"
                                                                            Width="200" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="drpShippingCountry"
                                                                            ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </asp:Panel>
                                                </tr>
                                                <asp:Panel ID="pnlBillingToShipping" runat="server" Visible="false">
                                                    <tr>
                                                        <td align="center" colspan="2" style="padding-top: 5px;">
                                                            <asp:Button ID="btnCopyBillingtoShipping" runat="server" CausesValidation="false"
                                                                OnClick="btnCopyBillingtoShipping_Click" Text="Copy Billing to Shipping --&gt;" />
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <p>
                                    </p>
                                    <p>
                                        <table border="0" cellpadding="5" cellspacing="3" width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <img src="https://www.ecn5.com/ecn.Images/images/testMyCampaign.gif" />&nbsp; <font
                                                        face="verdana,arial,helvetica"><b>Payment Information</b></font>
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
                                                <td width="70%">
                                                    <asp:TextBox ID="user_CardHolderName" runat="server" Width="250" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="user_CardHolderName"
                                                        EnableClientScript="true" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                    <span class="label">*&nbsp;Zip on Card Bill:</span>
                                                </td>
                                                <td width="70%">
                                                    <asp:TextBox ID="txtZipCardBill" runat="server" Width="200" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtZipCardBill"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="label">* Credit Card Type:</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="user_CCType" runat="server">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="MasterCard"></asp:ListItem>
                                                        <asp:ListItem Value="Visa"></asp:ListItem>
                                                        <asp:ListItem Value="Amex"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="user_CCType"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="label">* Card Number (No dashes):</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="user_CCNumber" runat="server" Width="250" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="user_CCNumber"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle">
                                                    <span class="label">* Expiration Date:</span>
                                                </td>
                                                <td valign="middle">
                                                    <asp:DropDownList ID="user_Exp_Month" runat="server">
                                                        <asp:ListItem Selected="True" Value=""></asp:ListItem>
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
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="user_Exp_Month"
                                                    Display="Dynamic" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="user_Exp_Year" runat="server">
                                                        <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                                        <asp:ListItem Value="16">2016</asp:ListItem>
                                                        <asp:ListItem Value="17">2017</asp:ListItem>
                                                        <asp:ListItem Value="18">2018</asp:ListItem>
                                                        <asp:ListItem Value="19">2019</asp:ListItem>
                                                        <asp:ListItem Value="20">2020</asp:ListItem>
                                                        <asp:ListItem Value="21">2021</asp:ListItem>
                                                        <asp:ListItem Value="22">2022</asp:ListItem>
                                                        <asp:ListItem Value="23">2023</asp:ListItem>
                                                        <asp:ListItem Value="24">2024</asp:ListItem>
                                                        <asp:ListItem Value="25">2025</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="user_Exp_Year"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <span class="label">* Card Verification Number:</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="user_CCVerfication" runat="server" />
                                                    &nbsp;&nbsp;&nbsp;<img align="textTop" height="75" src="images/CVN.jpg" width="250">
                                                    &nbsp;
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="user_CCVerfication"
                                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"></asp:RequiredFieldValidator>
                                                    </img>
                                                </td>
                                            </tr>
                                        </table>
                                    </p>
                                    <br />
                                    <p>
                                        <center>
                                            <table align="center" border="0">
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <div class="g-recaptcha" data-sitekey="6LdH3AoUAAAAAL-QRSj-PeLIYrbckR_wTd48ub6l"></div>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Panel ID="pnlButton" runat="server" Visible="true">
                                                            <asp:Button ID="btnSecurePayment" runat="server" OnClick="btnSecurePayment_Click"
                                                                Text="Process Payment" Width="250" CausesValidation="true" UseSubmitBehavior="false" />
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlButtonDisabled" runat="server" Visible="true">
                                                            <asp:Button ID="btnSecurePaymentDisabled" runat="server" Enabled="false" Style="visibility: hidden;"
                                                                Text="Processing...." Width="250" />
                                                        </asp:Panel>
                                                        <asp:CustomValidator ID="cvDoubleSubmit" runat="server" ClientValidationFunction="doubleSubmitCheck"
                                                            ErrorMessage=""></asp:CustomValidator>
                                                    </td>
                                                    <td align="center" valign="top">
                                                        <asp:Panel ID="pnlbtnReset" runat="server" Visible="true">
                                                            <asp:Button ID="btnReset" runat="server" CausesValidation="false" OnClick="btnReset_Click"
                                                                Text="Reset" Width="80px" />
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
                                    </p>
                                </p>
                            </p>
                    </asp:Panel>
                    <ajaxToolkit:RoundedCornersExtender ID="rounderCorneremailValidation" runat="server"
                        Corners="All" Radius="10" BorderColor="Black" TargetControlID="pnlEmailValidationPopup" />
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderEmailValidation" runat="server"
                        BackgroundCssClass="modalBackground" PopupControlID="pnlEmailValidationPopup"
                        TargetControlID="btnAddEmailValidation" CancelControlID="btnCancelProductEmail" />
                    <asp:Button ID="btnAddEmailValidation" Text="Add" runat="server" Style="display: none;"
                        CausesValidation="false" UseSubmitBehavior="true" />
                    <asp:Panel ID="pnlEmailValidationPopup" runat="server" CssClass="popupbody" Width="700px"
                        Height="130px" Style="display: none;">
                        <table border="0" align="center" style="padding-top: 20px;">
                            <tr>
                                <td style="padding: 5px 5px 5px 5px">
                                    <asp:Label ID="lblEmailValidationText" runat="server" Font-Bold="true" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 15px;">
                                    <asp:Button ID="btnCancelProductEmail" runat="server" Text="OK" Width="50" CausesValidation="false"
                                        UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <!-- end container-content -->
                <div id="footer" runat="server" align="center">
                </div>
                <!--end footer-->
            </div>
        </div>
        <div id="modalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10000; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
        </div>
        <div id="loadingModalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10019; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
        </div>
        <div id="loadingPanel" style="display: none; z-index: 10020; position: fixed; width: 200px; height: 200px; background-color: white;  left: 50%; top:50%;border-radius:5px;margin:-100px 0 0 -100px;">
            
                <div class='uil-default-css' style='transform: scale(0.46); margin-left:10px;'>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(0deg) translate(0,-60px); transform: rotate(0deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(30deg) translate(0,-60px); transform: rotate(30deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(60deg) translate(0,-60px); transform: rotate(60deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(90deg) translate(0,-60px); transform: rotate(90deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(120deg) translate(0,-60px); transform: rotate(120deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(150deg) translate(0,-60px); transform: rotate(150deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(180deg) translate(0,-60px); transform: rotate(180deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(210deg) translate(0,-60px); transform: rotate(210deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(240deg) translate(0,-60px); transform: rotate(240deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(270deg) translate(0,-60px); transform: rotate(270deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(300deg) translate(0,-60px); transform: rotate(300deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(330deg) translate(0,-60px); transform: rotate(330deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                </div>
                
            
            <span style=' text-align: center; font-family: Arial; font-size: 12px; color: #8C8989;'>Please wait…</span>
        </div>
    </form>
</body>

<script language="javascript" type="text/javascript">

    function Trim(str) {
        if (str != null) {
            return str.replace(/^\s+|\s+$/g, "");
        }
    }

    function IsFieldEmpty(args, field) {
        if (args.IsValid == true && field != null && Trim(field.value).length == 0) {
            args.IsValid = false;
        }
    }

    function ValidateFields(args) {

        //Validate contact information               
        var email = document.getElementById('<%=email.ClientID%>');
        var first = document.getElementById("<%=first.ClientID%>");
        var last = document.getElementById("<%=last.ClientID%>");
        var company = document.getElementById("<%=txtCompany.ClientID%>");
        var phone = document.getElementById("<%=phone.ClientID%>");
        var fax = document.getElementById("<%=fax.ClientID%>");

        IsFieldEmpty(args, email);

        if (email != null) {
            args.IsValid = ValidateEmailAddress(email.value);
        }

        IsFieldEmpty(args, first);
        IsFieldEmpty(args, last);
        IsFieldEmpty(args, company);
        IsFieldEmpty(args, phone);

        //validate billing address
        var billAddress1 = document.getElementById("<%=txtBillingAddress1.ClientID%>");
        var billCity = document.getElementById("<%=txtBillingCity.ClientID%>");
        var billState = document.getElementById("<%=drpBillingState.ClientID%>");
        var billZip = document.getElementById("<%=txtBillingZip.ClientID%>");
        var billCountry = document.getElementById("<%=drpBillingCountry.ClientID%>");

        IsFieldEmpty(args, billAddress1);
        IsFieldEmpty(args, billCity);
        IsFieldEmpty(args, billState);
        IsFieldEmpty(args, billZip);
        IsFieldEmpty(args, billCountry);

        //validate shipping address
        var shipAddress1 = document.getElementById("<%=txtShippingAddress1.ClientID%>");
        var shipCity = document.getElementById("<%=txtShippingCity.ClientID%>");
        var shipState = document.getElementById("<%=drpShippingState.ClientID%>");
        var shipZip = document.getElementById("<%=txtShippingZip.ClientID%>");
        var shipCountry = document.getElementById("<%=drpShippingCountry.ClientID%>");

        IsFieldEmpty(args, shipAddress1);
        IsFieldEmpty(args, shipCity);
        IsFieldEmpty(args, shipState);
        IsFieldEmpty(args, shipZip);
        IsFieldEmpty(args, shipCountry);

        //validate credit card information
        var cardName = document.getElementById("<%=user_CardHolderName.ClientID%>");
        var cardType = document.getElementById("<%=user_CCType.ID%>");
        var cardNo = document.getElementById("<%=user_CCNumber.ClientID%>");
        var expDate_Month = document.getElementById("<%=user_Exp_Month.ClientID%>");
        var expDate_Year = document.getElementById("<%=user_Exp_Year.ClientID%>");
        var zipCardBill = document.getElementById("<%=txtZipCardBill.ClientID%>");
        var cardverification = document.getElementById("<%=user_CCVerfication.ClientID%>");

        IsFieldEmpty(args, cardName);
        IsFieldEmpty(args, cardType);
        IsFieldEmpty(args, cardNo);
        IsFieldEmpty(args, expDate_Month);
        IsFieldEmpty(args, expDate_Year);
        IsFieldEmpty(args, zipCardBill);
        IsFieldEmpty(args, cardverification);
    }

    function doubleSubmitCheck(source, args) {

        if (args.IsValid) {
            ValidateFields(args);

            if (args.IsValid) {
                ValidateBillingStateZip(source, args);
            }

            if (args.IsValid) {
                ValidateShippingStateZip(source, args);
            }
        }

        if (args.IsValid) {
            $("#loadingModalBackgroundDiv").show();
            $("#loadingPanel").show();
            return true;
            <%--document.getElementById("<%=btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%=btnSecurePayment.ClientID %>").style.display = 'none';
            document.getElementById("<%=btnSecurePaymentDisabled.ClientID %>").style.visibility = 'visible';
            document.getElementById("<%=btnReset.ClientID %>").style.visibility = 'hidden';--%>
        }
        else{
            return false;
        }
    }

    function ValidateBillingStateZip(source, args) {

        var country = $("#drpBillingCountry");
        var stateObj = $("#drpBillingState");
        var zipObj = $("#txtBillingZip");

        if (stateObj.val() != null && zipObj.val() != null && Trim(zipObj.val()).length > 0 && Trim(stateObj.val()).length > 0) {
            ValidateStateZip(source, args, country.val(), stateObj.val(), zipObj.val());

            if (args.IsValid == false)
                alert("Invalid Billing State / Zip Code Combination");
        }
    }

    function ValidateShippingStateZip(source, args) {

        var country = $("#drpShippingCountry");
        var stateObj = $("#drpShippingState");
        var zipObj = $("#txtShippingZip");

        if (stateObj.val() != null && zipObj.val() != null && Trim(zipObj.val()).length > 0 && Trim(stateObj.val()).length > 0) {
            ValidateStateZip(source, args, country.val(), stateObj.val(), zipObj.val());

            if (args.IsValid == false)
                alert("Invalid Shipping State / Zip Code Combination");
        }
    }

</script>

     <script language="javascript" type="text/javascript">
    var canSubmit = false;
    // OnClientClick="doubleSubmitCheck()" for btnSecurePayment control.
    //$(document).ready(function(){
    //    $("#btnSecurePayment").click(function (event) {
    //        if(!canSubmit)
    //        {
    //            event.preventDefault();
            
    //            if (doubleSubmitCheck() == true)
    //            {
    //                canSubmit = true;
    //                $(this).trigger('click');
    //            }
    //        }
    //        else{

    //        }
            
    //    });
    //});
   <%-- function doubleSubmitCheck() {

        var validation = Page_ClientValidate();

        if (validation) {
            
            $("#loadingModalBackgroundDiv").show();
            $("#loadingPanel").show();
            return true;
            <%--document.getElementById("<%= btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%= btnSecurePayment.ClientID %>").style.display = 'none';
            return true;
        }
        else {
            return false;
        }
    }--%>

    function hideConfirm() {
        $("#modalBackgroundDiv").hide();
        $("#pnlConfirmSubmit").hide();
    }

    function showLoading() {
        $("#loadingModalBackgroundDiv").show();
        $("#loadingPanel").show();
    }
</script>

</html>
