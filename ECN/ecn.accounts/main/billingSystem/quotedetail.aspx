<%@ Reference Page="~/includes/login.aspx" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ Register TagPrefix="uc1" TagName="AnnualQuoteItemEditor" Src="../../includes/AnnualQuoteItemEditor.ascx" %>
<%@ Register TagPrefix="uc1" TagName="QuoteItemEditor" Src="../../includes/QuoteItemEditor.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EmailQuoteItemEditor" Src="../../includes/EmailQuoteItemEditor.ascx" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.quotedetail" CodeBehind="quotedetail.aspx.cs"
    Title="quotedetail" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="uc1" TagName="EmailNotes" Src="../../includes/EmailNotes.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type='text/css'>
        TABLE.Panel
        {
            border-right: #ee8800 1px solid;
            border-top: #ee8800 1px solid;
            font-size: 13px;
            border-left: #ee8800 1px solid;
            color: #000000;
            border-bottom: #ee8800 1px solid;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        .SubPanelTitle
        {
            font-weight: bold;
            font-size: 13px;
            color: #000000;
            border-bottom: black 1px solid;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-color: #fcf8e9;
        }
        .tableHeader
        {
            font-weight: bold;
            font-size: 13px;
            color: #000000;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        .tableContent
        {
            font-weight: normal;
            font-size: 11px;
            border-left-color: silver;
            border-bottom-color: silver;
            color: #000000;
            border-top-color: silver;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            border-right-color: silver;
        }
        .tableContentAlt1
        {
            font-size: 11px;
            color: #000000;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-color: #fcf8e9;
        }
        .sectionHeader
        {
            font-weight: bold;
            font-size: 13px;
            color: white;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-color: #6699cc;
        }
        .subSectionHeader
        {
            font-weight: bold;
            font-size: 11px;
            color: #000000;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-color: #ccffff;
        }
        .errormsg
        {
            font-weight: bold;
            font-size: 13px;
            color: #ff0000;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-color: #fcf8e9;
        }
        CustomerCreditItem
        {
            color: red;
            text-decoration: line-through;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table height="67" width="80%">
                        <tr>
                            <td valign="top" align="center" width="20%">
                                <img style="padding: 0 0 0 15px;"
                                    src="/ecn.images/images/errorEx.jpg"></td>
                            <td valign="middle" align="left" width="80%" height="100%">
                                <asp:Label ID="lblErrorMessageNew" runat="Server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder>

    <table class="tableContent" border='0' style="padding-left: 20px; padding-right: 20px;
        padding-top: 20px; padding-bottom: 20px;">
        <tr>
            <td valign="middle" align="left">
                <span style="font-size: 16pt; color: #ff6600">Create New Quote/Test Account(QTA)</span>
            </td>
            <td align='right' style="color: red; font-family: Verdana">
                Bill Type:
                <asp:DropDownList ID="ddlBillType" runat="Server" CssClass="formfield">
                    <asp:ListItem Value="CreditCard">Credit Card</asp:ListItem>
                    <asp:ListItem Value="Invoice" Selected="True">Invoice</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp; <font color='#FF0000' size="2"><b>STATUS:&nbsp;<asp:Label ID="lblStatus"
                    runat="Server"></asp:Label></b></font>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" CssClass="errormsg" Visible="False" runat="Server"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid;
        border-bottom: 1px solid; padding-left: 20px; padding-right: 20px; padding-top: 20px;
        padding-bottom: 20px;" cellpadding="1" bgcolor="#f4f4f4" border='0' class="tableContent"
        width="100%">
        <tr bgcolor="#f4f4f4">
            <td valign="Top" width="20%">
                <table class="tableContent" width="100%">
                    <tr>
                        <td>
                            Channel Partner:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList CssClass="formfield" ID="ddlChannels" runat="Server" Width="180"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlChannels_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Customer:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlCustomers" runat="Server" Width="180" AutoPostBack="True"
                                CssClass="formfield" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border-right: gray 1px solid; border-left: gray 1px solid" valign="top"
                width="60%">
                <table class="tableContent" border='0'>
                    <tr>
                        <td style="width: 25%" colspan="2">
                            First Name:
                        </td>
                        <td style="width: 25%" colspan="2">
                            Last Name:
                        </td>
                        <td style="width: 50%" colspan="2">
                            Email Address:
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 23%">
                            <asp:TextBox ID="txtFirstName" runat="Server" Width="130px" CssClass="formfield"></asp:TextBox>
                        </td>
                        <td style="width: 2%">
                            <asp:RequiredFieldValidator ID="FirstNameValidator" runat="Server" ErrorMessage="You must fill in First Name."
                                ControlToValidate="txtFirstName">*</asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 23%">
                            <asp:TextBox ID="txtLastName" runat="Server" Width="130px" CssClass="formfield"></asp:TextBox>
                        </td>
                        <td style="width: 2%">
                            <asp:RequiredFieldValidator ID="LastNameValidator" runat="Server" ErrorMessage="You must fill in Last Name."
                                ControlToValidate="txtLastName">*</asp:RequiredFieldValidator>
                        </td>
                        <td width="48%">
                            <asp:TextBox ID="txtEmail" runat="Server" Width="150px" CssClass="formfield"></asp:TextBox>
                        </td>
                        <td width="2%">
                            <asp:RequiredFieldValidator ID="EmailValidator" runat="Server" ErrorMessage="You must fill in Email."
                                ControlToValidate="txtEmail">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%" colspan="2">
                            Phone:
                        </td>
                        <td style="width: 25%" colspan="2">
                            Fax:
                        </td>
                        <td style="width: 50%" colspan="2">
                            Company Name:
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            <asp:TextBox CssClass="formfield" ID="txtPhone" runat="Server" Width="130px"></asp:TextBox>
                        </td>
                        <td style="width: 2px">
                            <asp:RequiredFieldValidator ID="valPhone" runat="Server" ErrorMessage="You must fill in Phone."
                                ControlToValidate="txtPhone">*</asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 99px" colspan="2">
                            <asp:TextBox CssClass="formfield" ID="txtFax" runat="Server" Width="130px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox CssClass="formfield" ID="txtCompanyName" runat="Server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 2px">
                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="Server" ErrorMessage="You must fill in Company."
                                ControlToValidate="txtCompanyName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="middle" width="20%">
                <table class="tableContent" border='0' width="100%">
                    <tr>
                        <td width="100%" align="Left" colspan="2">
                            Quote Created By
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblQCreatedBy" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <!--
                        <tr>
                            <td width="100%" align="Left" colspan="2">Account Manager</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DropDownList CssClass="formfield" ID="ddlAccountManager" runat="Server" Width="129px">
                                </asp:DropDownList></td>
                        </tr>
                        -->
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" border='0' cellpadding="0" cellspacing="0" style="padding-left: 20px;
        padding-right: 20px; padding-top: 20px; padding-bottom: 20px;">
        <tr>
            <td valign="top" width="20%">
                <uc1:annualquoteitemeditor id="ucAnnualTechEditor" runat="Server"></uc1:annualquoteitemeditor>
                <br />
                <uc1:emailnotes id="ucEmailNotes" runat="Server"></uc1:emailnotes>
            </td>
            <td valign="top" align="left" width="80%" style="padding-left: 7px">
                <table class="tableContent" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationMsg" runat="Server" CssClass="errormsg" Width="552px">
                            </asp:ValidationSummary>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="100%">
                            <uc1:emailquoteitemeditor id="ucEmailOptionEditor" title="&nbsp;Email Usages" runat="Server">
                            </uc1:emailquoteitemeditor>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="100%">
                            <uc1:quoteitemeditor id="ucOptionEditor" title="&nbsp;ECN Options" runat="Server">
                            </uc1:quoteitemeditor>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                                border-bottom: black 1px solid" cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td class="tableHeader" style="border-right: black 1px solid; border-top: black 1px solid;
                                        border-bottom: black 1px solid" align='right' width="40%">
                                        &nbsp;
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-top: black 1px solid;
                                        border-bottom: black 1px solid" align='right' width="15%">
                                        One Time Fees
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-top: black 1px solid;
                                        border-bottom: black 1px solid" align='right' width="15%">
                                        Monthly Fees
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-top: black 1px solid;
                                        border-bottom: black 1px solid" align='right' width="15%">
                                        Quarterly Fees
                                    </td>
                                    <td class="tableContent" style="border-top: black 1px solid; border-bottom: black 1px solid"
                                        align='right' width="15%">
                                        Annual Fees
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" style="border-right: black 1px solid; border-bottom: black 1px solid;
                                        height: 17px" align='right'>
                                        Total&nbsp;
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid;
                                        height: 17px" align='right'>
                                        <asp:Label ID="lblOneTimeFees" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid;
                                        height: 17px" align='right'>
                                        <asp:Label ID="lblMonthlyFees" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid;
                                        height: 17px" align='right'>
                                        <asp:Label ID="lblQuarterlyFees" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-bottom: black 1px solid; height: 17px" align='right'>
                                        <asp:Label ID="lblAnnualFees" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" style="border-right: black 1px solid; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblDiscount" runat="Server">Discount</asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblOneTimeSaving" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblMonthlySaving" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblQuarterlySaving" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="color: red; border-bottom: black 1px solid" align='right'>
                                        <asp:Label ID="lblAnnualSaving" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" style="border-right: black 1px solid; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblNetAmount" runat="Server">Net Amount</asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblOneTimeNetAmount" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblMonthlyNetAmount" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-right: black 1px solid; border-bottom: black 1px solid"
                                        align='right'>
                                        <asp:Label ID="lblQuarterlyNetAmount" runat="Server"></asp:Label>
                                    </td>
                                    <td class="tableContent" style="border-bottom: black 1px solid" align='right'>
                                        <asp:Label ID="lblAnnualNetAmount" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p>
                               
                                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="Server" Text="Submit">
                                </asp:Button></p>
                            <p style="text-align: left">
                                <asp:Label ID="lblPreview" runat="Server" Visible="False" Width="394px">Email Preview:</asp:Label><asp:TextBox
                                    CssClass="formfield" ID="txtEmailPreview" runat="Server" Visible="False" Width="100%"
                                    ReadOnly="True" Height="208px" TextMode="MultiLine"></asp:TextBox><br />
                                <br />
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
