<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Subscribe.aspx.cs"
    Inherits="KMPS_JF.PaidForms.Subscribe" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Paid Subscription Form</title>
    <style type="text/css">
        .style1 {
            height: 34px;
        }
    </style>
    <div id="divcss" runat="server">
    </div>

    <script src="../scripts/jQuery/jquery-1.4.3.js" type="text/javascript"></script>

    <script src="../scripts/Validation.js" type="text/javascript"></script>

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
                    <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
                        BorderWidth="1px">
                        <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('../images/errorEx.jpg'); background-repeat: no-repeat">
                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlPaidForm" runat="server">
                        <p>
                            &nbsp;<asp:Panel ID="pnlCountry" runat="server" Visible="false">
                                <div>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td style="padding-bottom: 10px;">
                                                <span class="label">Country:</span>&nbsp;
                                            <asp:DropDownList ID="drpBillingCountry" runat="server" Width="200" AutoPostBack="true"
                                                OnSelectedIndexChanged="drpBillingCountry_SelectedIndexChanged" />
                                                <br />
                                            </td>
                                            <td align="right">
                                                <asp:UpdateProgress ID="udProgress" runat="server">
                                                    <ProgressTemplate>
                                                        <sub>
                                                            <img border="0" src="../Images/animated-loading-orange.gif" /></sub><span style="font-size: 10px; color: Black">Please wait...</span>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <p>
                                <asp:UpdatePanel ID="grdUpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdMagazines" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                            HorizontalAlign="Center" ShowFooter="true" SkinID="skitems" Width="100%" OnRowDataBound="grdMagazines_RowDataBound">
                                            <EmptyDataTemplate>
                                                No Products available for this publication
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:ImageField DataImageUrlField="coverImage" HeaderText="Cover" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%" Height="50px" />
                                                </asp:ImageField>
                                                <asp:TemplateField HeaderText="Title" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"title") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Term" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTerm" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Term") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Term" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="drpTerm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpTerm_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Print Edition" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriceText" runat="server" Text="Please check the box to order your subscription.<br/><br/>" />
                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"groupID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblCustID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"custID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblPubCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"pubcode") %>'
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkPrint" runat="server" AutoPostBack="true" OnCheckedChanged="chBoxPrint_CheckedChanged"
                                                            Visible="true" />
                                                        <asp:Label ID="lblUsPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"usprice1") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUsPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"usprice2") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUsPrice3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"usprice3") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCanPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"canprice1") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCanPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"canprice2") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCanPrice3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"canprice3") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblIntPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"intprice1") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblIntPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"intprice2") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblIntPrice3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"intprice3") %>'
                                                            Visible="false"></asp:Label>

                                                        <asp:Label ID="lblMessage" Font-Bold="true" runat="server" Text='' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Price" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <span class="label">$<asp:Label ID="lblTotalAmount" Text="0.00" runat="server" /></span>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </p>
                            <p>
                                <br />
                                <span class="label">* Indicates a required field.</span>
                            </p>
                            <p>
                                <table border="0" width="100%">
                                    <asp:Panel ID="pnlContactInfo" runat="server" Visible="true">
                                        <tr>
                                            <td>
                                                <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="2">&nbsp;
                                                            </td>
                                                        </tr>
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
                                                            <td width="30%" align="left">
                                                                <span class="label">* Email:</span>
                                                            </td>
                                                            <td width="70%">
                                                                <asp:TextBox ID="email" runat="server" Width="250" AutoPostBack="true" OnTextChanged="email_TextChanged" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true"
                                                                    runat="server" ControlToValidate="email" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="&lt;&lt; Invalid Email Address"
                                                                    EnableClientScript="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                    ControlToValidate="email" Display="Dynamic" Font-Bold="True" Font-Size="11px"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="label">* First Name:</span>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="first" Width="250" runat="server" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                    ControlToValidate="first" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="label">* Last Name:</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="last" Width="250" runat="server" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                    ControlToValidate="last"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="label">* Company:</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="company" Width="250" runat="server" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                    ControlToValidate="company" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="label">* Phone Number:</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="phone" Width="150" runat="server" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                    ControlToValidate="phone"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="label">&nbsp;&nbsp;Fax Number:</span>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="fax" Width="150" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <asp:Panel ID="pnlCoverImage" runat="server" Visible="false">
                                                <td valign="middle" align="center">
                                                    <asp:PlaceHolder ID="phlMagCoverImage" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </asp:Panel>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="pnlUpdateAddress" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <asp:Panel ID="pnlBillingAddress" runat="server" Visible="true">
                                                                <td valign="top">
                                                                    <table border="0" cellspacing="5" width="100%" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="2" style="padding-bottom: 10px;">
                                                                                <span class="label"><font face="verdana,arial,helvetica" color="black"><b>Billing Address</b></font></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="label">Address1:</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBillingAddress1" runat="server" Width="200" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtBillingAddress1"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtBillingCity"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="drpBillingState"
                                                                                        Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtBillingZip"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="label">Country:</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="drpBillingCountry_1" Enabled="false" runat="server" Width="200"
                                                                                    AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlShippingAddress" runat="server" Visible="true">
                                                                <td width="50%" valign="top">
                                                                    <table border="0" cellspacing="5" width="100%" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="2" style="padding-bottom: 10px;">
                                                                                <font face="verdana,arial,helvetica" color="black"><b>Shipping Address</b></font>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="label">Address1:</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtShippingAddress1" runat="server" Width="200" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="txtShippingAddress1"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ControlToValidate="txtShippingCity"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" ControlToValidate="drpShippingState"
                                                                                        Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" ControlToValidate="txtShippingZip"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="label">Country:</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="drpShippingCountry" runat="server" Width="200" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="drpShippingCountry_SelectedIndexChanged" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" ControlToValidate="drpShippingCountry"
                                                                                    Font-Size="12pt" runat="server" Display="Dynamic" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </asp:Panel>
                                                        </tr>
                                                        <asp:Panel ID="pnlBillingToShipping" runat="server" Visible="true">
                                                            <tr>
                                                                <td colspan="2" align="center" style="padding-top: 5px;">
                                                                    <asp:Button ID="btnCopyBillingtoShipping" runat="server" Text="Copy Billing to Shipping -->"
                                                                        CausesValidation="false" OnClick="btnCopyBillingtoShipping_Click" />
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <p class="padBottom">
                                <table cellspacing="3" cellpadding="5" width="100%" border="0">
                                    <tr>
                                        <td colspan="2">
                                            <img src="https://www.ecn5.com/ecn.Images/images/testMyCampaign.gif" />&nbsp; <font
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
                                        <td width="70%">
                                            <asp:TextBox ID="user_CardHolderName" runat="server" Width="250" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" EnableClientScript="true"
                                                ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;" ControlToValidate="user_CardHolderName"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="label">* Credit Card Type:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="user_CCType" runat="server">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
                                                <asp:ListItem Value="Visa">Visa</asp:ListItem>
                                                <asp:ListItem Value="Amex">American Express</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                ControlToValidate="user_CCType"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="label">* Card Number (No dashes):</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="user_CCNumber" runat="server" Width="250" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_Exp_Month" Display="Dynamic"></asp:RequiredFieldValidator><asp:DropDownList
                                            ID="user_Exp_Year" runat="server">
                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="&lt;img src=&quot;../images/required_field.jpg&quot;&gt;"
                                                ControlToValidate="user_Exp_Year"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <span class="label">* Card Verification Number:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="user_CCVerfication" runat="server" />
                                            &nbsp;&nbsp;&nbsp;<img src="../Images/CVN.jpg" width="250" height="100" align="textTop">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <br />
                            <p>
                                <center>
                                    <table align="center" border="0">
                                        <tr>
                                            <td align="center"  colspan="2">
                                                <div class="g-recaptcha" data-sitekey="6LdH3AoUAAAAAL-QRSj-PeLIYrbckR_wTd48ub6l"></div>
                                                 <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="pnlButton" runat="server" Visible="true">
                                                    <asp:Button ID="btnSecurePayment" Text="Process Payment" Width="250" runat="server"
                                                        OnClick="btnSecurePayment_Click" CausesValidation="true" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlButtonDisabled" runat="server" Visible="true">
                                                    <asp:Button ID="btnSecurePaymentDisabled" Text="Processing...." Width="250" runat="server"
                                                        Enabled="false" Style="visibility: hidden;" />
                                                </asp:Panel>
                                                <asp:CustomValidator ID="cvDoubleSubmit" runat="server" ClientValidationFunction="doubleSubmitCheck"
                                                    ErrorMessage=""></asp:CustomValidator>
                                            </td>
                                            <td align="center" valign="top">
                                                <asp:Panel ID="pnlbtnReset" runat="server" Visible="true">
                                                    <asp:Button ID="btnReset" Text="Reset" runat="server" Width="80px" CausesValidation="false"
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
                                    <asp:Button ID="btnCancelProductEmail" runat="server" Text="OK" Width="50" CausesValidation="false" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <!-- end container-content -->
                <div id="footer" style="text-align: center">
                    <table align="center" border="0" width="100%">
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
        <br />
        <br />
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
        var company = document.getElementById("<%=company.ClientID%>");
        var phone = document.getElementById("<%=phone.ClientID%>");
        var fax = document.getElementById("<%=fax.ClientID%>");

        IsFieldEmpty(args, email);
        args.IsValid = ValidateEmailAddress(email.value);
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
        IsFieldEmpty(args, shipZip);
        IsFieldEmpty(args, shipCountry);

        //validate credit card information
        var cardName = document.getElementById("<%=user_CardHolderName.ClientID%>");
        var cardType = document.getElementById("<%=user_CCType.ID%>");
        var cardNo = document.getElementById("<%=user_CCNumber.ClientID%>");
        var expDate_Month = document.getElementById("<%=user_Exp_Month.ClientID%>");
        var expDate_Year = document.getElementById("<%=user_Exp_Year.ClientID%>");

        IsFieldEmpty(args, cardName);
        IsFieldEmpty(args, cardType);
        IsFieldEmpty(args, cardNo);
        IsFieldEmpty(args, expDate_Month);
        IsFieldEmpty(args, expDate_Year);
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
            document.getElementById("<%=btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%=btnSecurePayment.ClientID %>").style.display = 'none';
            document.getElementById("<%=btnSecurePaymentDisabled.ClientID %>").style.visibility = 'visible';
            document.getElementById("<%=btnReset.ClientID %>").style.visibility = 'hidden';
        }
    }

    function ValidateBillingStateZip(source, args) {

        var country = $("#drpBillingCountry");
        var stateObj = $("#drpBillingState");
        var zipObj = $("#txtBillingZip");

        if (country.val() == 205) {
            ValidateStateZip(source, args, country.val(), stateObj.val(), zipObj.val());

            if (args.IsValid == false)
                alert("Invalid Billing State / Zip Code Combination");
        }
    }

    function ValidateShippingStateZip(source, args) {

        var country = $("#drpShippingCountry");
        var stateObj = $("#drpShippingState");
        var zipObj = $("#txtShippingZip");

        if (country.val() == 205) {
            ValidateStateZip(source, args, country.val(), stateObj.val(), zipObj.val());

            if (args.IsValid == false)
                alert("Invalid Shipping State / Zip Code Combination");
        }
    }

</script>

</html>
