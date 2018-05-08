<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="PublisherAdd.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.PublisherAdd"
    Title="KMPS Form Builder - Publisher" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:HiddenField ID="hfldControlSequence" Value="0" runat="server" />
                <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <asp:Menu ID="MnuPunlisher" Orientation="Horizontal" runat="server" OnMenuItemClick="MnuPunlisher_MenuItemClick"
                        CssClass="menuTabs" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab">
                        <Items>
                            <asp:MenuItem Text="Publication Details" Value="0" Selected="true"></asp:MenuItem>
                            <asp:MenuItem Text="Header & Footer" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Landing" Value="2"></asp:MenuItem>
                            <asp:MenuItem Text="Renewal Login" Value="3"></asp:MenuItem>
                            <asp:MenuItem Text="New/Renew" Value="4"></asp:MenuItem>
                            <asp:MenuItem Text="Free Pub Thx" Value="5"></asp:MenuItem>
                            <asp:MenuItem Text="CS & FAQ" Value="6"></asp:MenuItem>
                            <asp:MenuItem Text="Define Styles" Value="7"></asp:MenuItem>
                            <asp:MenuItem Text="Paid Pages Thx" Value="8"></asp:MenuItem>
                            <asp:MenuItem Text="Ext URL Post" Value="9"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                    <div style="background-color: #eeeeee; border-color: Black; border-style: solid; border-width: 1px">
                        <asp:MultiView ID="MultiViewPublisher" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                        <tr>
                                            <td class="labelbold" width="25%">Customer :
                                            </td>
                                            <td width="75%" colspan="2">
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px" DataTextField="CustomerName"
                                                    DataValueField="CustomerId">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCustomer"
                                                    ErrorMessage="*" InitialValue=""></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <%--                                        <tr>
                                            <td class="labelbold">
                                                Email Marketing Group :
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="200px" DataTextField="GroupName"
                                                    DataValueField="GroupID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ddlGroupRequiredFieldValidator" runat="server" ControlToValidate="ddlGroup"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="labelbold">Name :
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="TxtName" runat="server" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ReqFldTxtName" runat="server" ControlToValidate="TxtName"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Code :
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="TxtCode" runat="server" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ReqFldTxtCode" runat="server" ControlToValidate="TxtCode"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Publication Logo :
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="oFilePubLogo" runat="server" Width="200" Height="20px" />
                                                <asp:HiddenField ID="hfldPubLogo" runat="server" Value="0" />
                                            </td>
                                            <td align="center" valign="middle">
                                                <asp:Label ID="lblPubLogo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" height="75px" valign="top">Mailing Label:
                                            </td>
                                            <td height="75px" valign="top">
                                                <asp:FileUpload ID="FileUploadMailingLabel" runat="server" Width="200" Height="20px" />
                                                <asp:HiddenField ID="hfldMailingLabel" runat="server" Value="0" />
                                            </td>
                                            <td align="center" valign="top" height="75px">
                                                <asp:Label ID="lblMailingLabel" Text="<img src='../Images/pub-label.gif' width='250px' height='75px'/>"
                                                    runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Page Title :
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <b>Width:</b>
                                            </td>
                                            <td align="left" width="80%">
                                                <asp:DropDownList ID="drpWidth" runat="server">
                                                    <asp:ListItem Value="500px">500px</asp:ListItem>
                                                    <asp:ListItem Value="600px">600px</asp:ListItem>
                                                    <asp:ListItem Value="700px">700px</asp:ListItem>
                                                    <asp:ListItem Value="750px">750px</asp:ListItem>
                                                    <asp:ListItem Value="800px">800px</asp:ListItem>
                                                    <asp:ListItem Value="850px">850px</asp:ListItem>
                                                    <asp:ListItem Value="900px">900px</asp:ListItem>
                                                    <asp:ListItem Value="950px">950px</asp:ListItem>
                                                    <asp:ListItem Value="100%">Full Screen</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <b>Column Format:</b>
                                            </td>
                                            <td align="left" width="80%">
                                                <asp:DropDownList ID="drpColumnFormat" runat="server">
                                                    <asp:ListItem Value="2">2 Column</asp:ListItem>
                                                    <asp:ListItem Value="4">4 Column</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Is Active:
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="RdbLstIsActive" AutoPostBack="true" OnSelectedIndexChanged="RdbLstIsActive_SelectedIndexChanged"
                                                    runat="server" Width="200px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlRedirectorHTML" runat="server" Visible="true">
                                            <tr>
                                                <td class="labelbold">Redirector Link:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox Width="350" ID="txtRedirectorLink" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold" valign="top">Redirector HTML:
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadEditor runat="server" ID="RadEditorRedirectorHTML" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td class="labelbold">Has Paid Forms:
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="RdbLstHasPaid" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="RdbLstHasPaid_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlPaymentOptions" runat="server" Visible="false">
                                            <tr>
                                                <td class="labelbold" colspan="3">Do you want to handle payment processing externally (outside of form builder)?
                                                    <asp:RadioButtonList ID="rbProcessExternal" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="rbProcessExternal_SelectedIndexChanged" AutoPostBack="true"
                                                        RepeatLayout="Flow">
                                                        <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlProcessExternal" runat="server" Visible="false">
                                            <td class="labelbold">Payment Processing Page :
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtProcessExternalURL" runat="server" MaxLength="250" Width="400"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtProcessExternalURL"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPaymentGateway" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:RadioButtonList ID="rblstPaymentGateway" runat="server" AutoPostBack="true"
                                                        Font-Bold="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblstPaymentGateway_SelectedIndexChanged">
                                                        <asp:ListItem Text="Paypal" Value="Paypal" Selected="True" />
                                                        <asp:ListItem Text="Authorize.NET" Value="AuthorizeDotNet" />
                                                        <asp:ListItem Text="Paypal Redirect" Value="PaypalRedirect" />
                                                        <asp:ListItem Text="Paypal Payflow" Value="Paypalflow" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                                        ControlToValidate="rblstPaymentGateway"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPayPal" runat="server" Visible="false">
                                            <tr>
                                                <td class="labelbold" colspan="3">
                                                    <u>PayPal Account Information:</u>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlPayFlow" runat="server" Visible="false">
                                                <tr>
                                                    <td class="labelbold">Partner:
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TxtPayflowPartner" runat="server" MaxLength="50" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator11" runat="server" ControlToValidate="TxtPayflowPartner"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelbold">Merchant Login:
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TxtPayflowVendor" runat="server" MaxLength="50" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator12" runat="server" ControlToValidate="TxtPayflowVendor"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <tr>
                                                <td class="labelbold">User / Account:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TxtPayflowAccount" runat="server" MaxLength="50" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtPayflowAccount"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Password:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TxtPayflowPassword" runat="server" MaxLength="50" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtPayflowPassword"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlPayPalPro" runat="server" Visible="false">
                                                <tr>
                                                    <td class="labelbold">Signature:
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TxtPayflowSignature" runat="server" MaxLength="100" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtPayflowSignature"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr id="trPayflowPageStyle" runat="server">
                                                    <td class="labelbold">Page Style:

                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPayflowPageStyle" runat="server" MaxLength="50" Width="400" />
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAuthorizeNet" runat="server" Visible="false">
                                            <tr>
                                                <td class="labelbold" colspan="3">
                                                    <u>Authorize.net Account Information:</u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Authorize.net Account:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtAuthorizeDotNetAccount" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAuthorizeDotNetAccount"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Authorize.net Signature:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtAuthorizeDotNetSignature" runat="server" MaxLength="100" Width="400"></asp:TextBox><asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtAuthorizeDotNetSignature"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPaymentCoverPage" runat="server">
                                            <tr>
                                                <td>
                                                    <b>Paid Page Cover Image:</b>
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="fileUploadCoverImage" runat="server" Width="200" Height="20px" />
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td>
                                                <b>Check Subscriber Existence:</b>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblstCheckSubscriberExists" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="True" />
                                                    <asp:ListItem Text="No" Value="False" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Stop Repeat Emails:</b>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblstRepeatEmails" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="True" />
                                                    <asp:ListItem Text="No" Value="False" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View1" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold">Header HTML :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorHeaderHTML" Height="300px" Width="100%" AllowScripts="true" ContentFilters="None" ></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Footer HTML :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorFooterHTML" Height="300px" Width="100%" AllowScripts="true" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold" width="25%">Show Print :
                                            </td>
                                            <td width="75%">
                                                <asp:RadioButtonList ID="RdbLstShowPrint" runat="server" Width="200px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Show News Letters :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="RdbLstShowNewsLetters" runat="server" Width="200px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Show New Subscription Link :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="RdbLstShowNewSubscriptionLink" runat="server" Width="200px"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Show Renew Link :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="RdbLstShowRenewLink" runat="server" Width="200px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Show Customer Service Link :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="RdbLstShowCustomerServiceLink" runat="server" Width="200px"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Show Related Trade Shows :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="RdbLstShowRelatedTradeShows" runat="server" Width="200px"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="labelbold">Home Page Description :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorHomePageDesc" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="labelbold">Step 2 Description : (Email & Country Selection Page Desc)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorStep2PageDesc" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold" colspan="2">New Subscription Header :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorNewSubscriptionHeader" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" align="left" width="20%">New Subscription Link :
                                            </td>
                                            <td align="left" width="80%">
                                                <asp:TextBox ID="txtNewSubscriptionLink" runat="server" Width="400" />
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Display="Dynamic" ErrorMessage="*"
                                                    ControlToValidate="txtNewSubscriptionLink" runat="server"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" colspan="2">Manage Subscription Header :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorManageSubscriptionHeader" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" align="left" width="20%">Manage Subscription Link :
                                            </td>
                                            <td align="left" width="80%">
                                                <asp:TextBox ID="txtManageSubscriptionLink" runat="server" Width="400" />
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic" ErrorMessage="*"
                                                    ControlToValidate="txtManageSubscriptionLink" runat="server"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" colspan="2">Required Field : &nbsp;
                                                <asp:CustomValidator runat="server" ErrorMessage="*" ID="ValidateRequiredField" ControlToValidate="RadEditorRequiredFieldEditor"
                                                    ValidateEmptyText="true" OnServerValidate="ValidateRequiredField_ServerValidate" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorRequiredFieldEditor" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" colspan="2">Newsletter Header : &nbsp;
                                                <asp:CustomValidator runat="server" ErrorMessage="*" ID="ValidateNewsletterHeader"
                                                    ControlToValidate="RadEditorNewsletterHeader" ValidateEmptyText="true" OnServerValidate="ValidateNewsletterHeader_ServerValidate" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorNewsletterHeader" Height="300px" Width="100%"  AllowScripts="true" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold" width="20%">Disable Email/Password:
                                            </td>
                                            <td width="80%">
                                                <asp:RadioButtonList ID="rbDisableEmail" runat="server" Width="100px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="false"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" width="20%">Disable Password:
                                            </td>
                                            <td width="80%">
                                                <asp:RadioButtonList ID="rbDisablePassword" runat="server" Width="100px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="false"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" width="20%">Disable Subscription Login :
                                            </td>
                                            <td width="80%">
                                                <asp:RadioButtonList ID="rbDisableSubLogin" runat="server" Width="100px" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true" Selected="false"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold" width="20%">Login Verification :
                                            </td>
                                            <td width="80%">
                                                <asp:DropDownList ID="DdlLoginVerification" runat="server" Width="200px" AutoPostBack="false"
                                                    DataTextField="Names">
                                                    <asp:ListItem Selected="True" Value="Select">--- Select Verification ---</asp:ListItem>
                                                    <asp:ListItem Value="S">State</asp:ListItem>
                                                    <asp:ListItem Value="L">Last Name Initial</asp:ListItem>
                                                    <asp:ListItem Value="C">Country first letter</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqFldDdlLoginVerification" runat="server" ControlToValidate="DdlLoginVerification"
                                                    ErrorMessage="*" InitialValue="Select"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="labelbold">Login Page Description :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorLoginPageDesc" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="labelbold">Forgot Password Email  : <font style="font-size:x-small">(Add %%password%% codesnippet to append new/existing password)</font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorForgotPassword" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                        <tr>
                                            <td class="labelbold">New Page Description :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorNewPageDesc" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">Renew Page Description :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorRenewPageDesc" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View5" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold" width="20%">Thank You Page Link :
                                            </td>
                                            <td width="80%"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding-left: 10px">
                                                <asp:TextBox ID="TxtThankYouPageLink" runat="server" MaxLength="255" TextMode="MultiLine"
                                                    Rows="5" Width="100%" Font-Size="Small"></asp:TextBox><br />
                                                <font size="1" color="#325C78"><b>EMAILADDRESS</b>=%%e%%, <b>FIRSTNAME</b>=%%fn%%, <b>LASTNAME</b>=%%ln%%, <b>FULLNAME</b>=%%n%%, <b>COMPANY</b>=%%compname%%, <b>TITLE</b>=%%t%%,
                                                    <b>OCCUPATION</b>=%%occ%%,<br />
                                                    <b>ADDRESS</b>=%%adr%%, <b>ADDRESS2</b>=%%adr2%%, <b>CITY</b>=%%city%%, <b>STATE</b>=%%state%%,
                                                    <b>ZIP</b>=%%zc%%, <b>COUNTRY</b>=%%ctry%%, <b>VOICE</b>=%%ph%%, <b>MOBILE</b>=%%mph%%,
                                                    <br />
                                                    <b>FAX</b>=%%fax%%, <b>WEBSITE</b>=%%website%%, <b>AGE</b>=%%age%%, <b>INCOME</b>=%%income%%,
                                                    <b>GENDER</b>=%%gndr%%, <b>BIRTHDATE</b>=%%bdt%%<br />
                                                    <br />
                                                </font><font size="1" color="#325C78"><b>Example : http://www.domain.com?email=%%e%%&address=%%adr%%&zip=%%zc%%</b>
                                                </font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="labelbold">Thank You Page HTML :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorThankYouPageHTML" Height="300px" Width="100%"  AllowScripts="true" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View6" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td class="labelbold">Customer Service Page HTML :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorCustomerServiceHTML" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelbold">FAQ Page HTML :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorFAQHTML" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View7" runat="server">
                                <asp:Panel ID="pnlTemp" runat="server">
                                    <asp:TextBox ID="TextBox0" runat="server" AutoCompleteType="None" MaxLength="7" Style="float: left"></asp:TextBox>
                                    <asp:ImageButton runat="Server" ID="Image0" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                        AlternateText="Click to show color picker" />
                                    <asp:Panel ID="Sample0" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                        runat="server" />
                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image0" SampleControlID="Sample0"
                                        ID="ColorPickerExtender7" TargetControlID="TextBox0" PopupPosition="BottomLeft" />
                                </asp:Panel>
                                <div style="padding: 20px 20px 20px 25px">
                                    <asp:Panel ID="Panel1" runat="server" CssClass="collapsePanelHeader" Height="30px"
                                        Width="95%">
                                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                            <div style="float: left;">
                                                Page Settings
                                            </div>
                                            <div style="float: left; margin-left: 20px;">
                                                <asp:Label ID="Label1" runat="server">(Show ...)</asp:Label>
                                            </div>
                                            <div style="float: right; vertical-align: middle;">
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                    AlternateText="(Show Details...)" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="collapsePanel" Height="0" Width="95%">
                                        <br />
                                        <table width="100%" border="0" cellpadding="5" cellspacing="0" style="padding-left: 25px">
                                            <tr>
                                                <td class="labelbold">Page Background Color
                                                </td>
                                                <td valign="middle">
                                                    <asp:TextBox ID="txtPBGColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="ImageButton2" Style="float: left; margin: 0 3px"
                                                        ImageUrl="~/images/cp_button.png" AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample1" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="ImageButton2" SampleControlID="Sample1"
                                                        ID="ColorPickerExtender1" TargetControlID="txtPBGColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Form Background Color
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFBGColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="Image2" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                                        AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample2" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image2" SampleControlID="Sample2"
                                                        ID="ColorPickerExtender2" TargetControlID="txtFBGColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Form Border
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPageBorder" runat="server">
                                                        <asp:ListItem Text="Yes" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%; text-align: left" class="labelbold">Page Font Name
                                                </td>
                                                <td style="width: 70%; text-align: left">
                                                    <asp:DropDownList ID="ddlPageFont" runat="server" Width="100px">
                                                        <asp:ListItem Text="Arial" Value="Arial"></asp:ListItem>
                                                        <asp:ListItem Text="Courier New" Value="Courier New"></asp:ListItem>
                                                        <asp:ListItem Text="Georgia" Value="Georgia"></asp:ListItem>
                                                        <asp:ListItem Text="Trebuchet MS" Value="Trebuchet MS"></asp:ListItem>
                                                        <asp:ListItem Text="Times New Roman" Value="Times New Roman"></asp:ListItem>
                                                        <asp:ListItem Text="Tahoma" Value="Tahoma"></asp:ListItem>
                                                        <asp:ListItem Text="Verdana" Value="Verdana"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Page Font Size
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPageFontSize" runat="server" Width="100px">
                                                        <asp:ListItem Text="10 px" Value="10px"></asp:ListItem>
                                                        <asp:ListItem Text="11 px" Value="11px"></asp:ListItem>
                                                        <asp:ListItem Text="12 px" Value="12px"></asp:ListItem>
                                                        <asp:ListItem Text="13 px" Value="13px"></asp:ListItem>
                                                        <asp:ListItem Text="14 px" Value="14px"></asp:ListItem>
                                                        <asp:ListItem Text="15 px" Value="15px"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="Panel2"
                                        ExpandControlID="Panel1" CollapseControlID="Panel1" Collapsed="false" TextLabelID="Label1"
                                        ImageControlID="Image1" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                        ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                        SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                    <br />
                                    <br />
                                    <asp:Panel ID="Panel3" runat="server" CssClass="collapsePanelHeader" Height="30px"
                                        Width="95%">
                                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                            <div style="float: left;">
                                                Category Settings
                                            </div>
                                            <div style="float: left; margin-left: 20px;">
                                                <asp:Label ID="Label2" runat="server">(Show ...)</asp:Label>
                                            </div>
                                            <div style="float: right; vertical-align: middle;">
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                    AlternateText="(Show Details...)" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="collapsePanel" Height="0" Width="95%">
                                        <br />
                                        <table width="100%" border="0" cellpadding="5" cellspacing="0" style="padding-left: 25px">
                                            <tr>
                                                <td style="width: 30%; text-align: left" class="labelbold">Category Background Color
                                                </td>
                                                <td style="width: 70%; text-align: left">
                                                    <asp:TextBox ID="txtCBGColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="Image3" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                                        AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample3" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image3" SampleControlID="Sample3"
                                                        ID="ColorPickerExtender3" TargetControlID="txtCBGColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Category Font Size
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCatFontSize" runat="server" Width="100px">
                                                        <asp:ListItem Text="10 px" Value="10px"></asp:ListItem>
                                                        <asp:ListItem Text="11 px" Value="11px"></asp:ListItem>
                                                        <asp:ListItem Text="12 px" Value="12px"></asp:ListItem>
                                                        <asp:ListItem Text="13 px" Value="13px"></asp:ListItem>
                                                        <asp:ListItem Text="14 px" Value="14px"></asp:ListItem>
                                                        <asp:ListItem Text="15 px" Value="15px"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Category Font Color
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCFColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="Image4" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                                        AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample4" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image4" SampleControlID="Sample4"
                                                        ID="ColorPickerExtender4" TargetControlID="txtCFColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server"
                                        TargetControlID="Panel4" ExpandControlID="Panel3" CollapseControlID="Panel3"
                                        Collapsed="False" TextLabelID="Label2" ImageControlID="ImageButton3" ExpandedText="(Hide Details...)"
                                        CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                        CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                    <br />
                                    <br />
                                    <asp:Panel ID="Panel5" runat="server" CssClass="collapsePanelHeader" Height="30px"
                                        Width="95%">
                                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                            <div style="float: left;">
                                                Question Settings
                                            </div>
                                            <div style="float: left; margin-left: 20px;">
                                                <asp:Label ID="Label3" runat="server">(Show ...)</asp:Label>
                                            </div>
                                            <div style="float: right; vertical-align: middle;">
                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                    AlternateText="(Show Details...)" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="collapsePanel" Height="0" Width="95%">
                                        <table width="100%" border="0" cellpadding="5" cellspacing="0" style="padding-left: 25px">
                                            <tr>
                                                <td style="width: 30%; text-align: left" class="labelbold">Question Font Size
                                                </td>
                                                <td style="width: 70%; text-align: left">
                                                    <asp:DropDownList ID="ddlQFSize" runat="server" Width="100px">
                                                        <asp:ListItem Text="10 px" Value="10px"></asp:ListItem>
                                                        <asp:ListItem Text="11 px" Value="11px"></asp:ListItem>
                                                        <asp:ListItem Text="12 px" Value="12px"></asp:ListItem>
                                                        <asp:ListItem Text="13 px" Value="13px"></asp:ListItem>
                                                        <asp:ListItem Text="14 px" Value="14px"></asp:ListItem>
                                                        <asp:ListItem Text="15 px" Value="15px"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Question Font Color
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQFColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="Image5" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                                        AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample5" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image5" SampleControlID="Sample5"
                                                        ID="ColorPickerExtender5" TargetControlID="txtQFColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Question Font Bold
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlQFBold" runat="server">
                                                        <asp:ListItem Text="No" Value="normal"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="bold"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server"
                                        TargetControlID="Panel6" ExpandControlID="Panel5" CollapseControlID="Panel5"
                                        Collapsed="False" TextLabelID="Label3" ImageControlID="ImageButton4" ExpandedText="(Hide Details...)"
                                        CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                        CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                    <br />
                                    <br />
                                    <asp:Panel ID="Panel7" runat="server" CssClass="collapsePanelHeader" Height="30px"
                                        Width="95%">
                                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                            <div style="float: left;">
                                                Answer Settings
                                            </div>
                                            <div style="float: left; margin-left: 20px;">
                                                <asp:Label ID="Label4" runat="server">(Show ...)</asp:Label>
                                            </div>
                                            <div style="float: right; vertical-align: middle;">
                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                    AlternateText="(Show Details...)" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel8" runat="server" CssClass="collapsePanel" Height="0" Width="95%">
                                        <table width="100%" border="0" cellpadding="5" cellspacing="0" style="padding-left: 25px">
                                            <tr>
                                                <td style="width: 30%; text-align: left" class="labelbold">Answer Font Size
                                                </td>
                                                <td style="width: 70%; text-align: left">
                                                    <asp:DropDownList ID="ddlAFSize" runat="server" Width="100px">
                                                        <asp:ListItem Text="10 px" Value="10px"></asp:ListItem>
                                                        <asp:ListItem Text="11 px" Value="11px"></asp:ListItem>
                                                        <asp:ListItem Text="12 px" Value="12px"></asp:ListItem>
                                                        <asp:ListItem Text="13 px" Value="13px"></asp:ListItem>
                                                        <asp:ListItem Text="14 px" Value="14px"></asp:ListItem>
                                                        <asp:ListItem Text="15 px" Value="15px"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Answer Font Color
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFColor" runat="server" AutoCompleteType="None" MaxLength="7"
                                                        Style="float: left"></asp:TextBox>
                                                    <asp:ImageButton runat="Server" ID="Image6" Style="float: left; margin: 0 3px" ImageUrl="~/images/cp_button.png"
                                                        AlternateText="Click to show color picker" />
                                                    <asp:Panel ID="Sample6" Style="width: 36px; height: 18px; border: 1px solid #000; margin: 0 3px; float: left"
                                                        runat="server" />
                                                    <ajaxToolkit:ColorPickerExtender runat="server" PopupButtonID="Image6" SampleControlID="Sample6"
                                                        ID="ColorPickerExtender6" TargetControlID="txtAFColor" PopupPosition="BottomLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelbold">Answer Font Bold
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAFBold" runat="server">
                                                        <asp:ListItem Text="No" Value="normal"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="bold"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="Server"
                                        TargetControlID="Panel8" ExpandControlID="Panel7" CollapseControlID="Panel7"
                                        Collapsed="False" TextLabelID="Label4" ImageControlID="ImageButton5" ExpandedText="(Hide Details...)"
                                        CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                        CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                    <br />
                                </div>
                            </asp:View>
                            <asp:View ID="View8" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td>
                                                <b>From Name:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPaidFormFromName" runat="server" Width="300" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>From Email:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPaidPageFromEmail" runat="server" Width="300" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Subject:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPaidFormResponseEmailSubject" runat="server" Width="300" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <b>Show Shipping Address:</b>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblstShowShippingAddress" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="True" />
                                                    <asp:ListItem Text="No" Value="False" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Thank You Page Link:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPaidThankyouPageLink" runat="server" Width="500" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <b>Thank you Page HTML</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorPaidThankyouPage" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <b>Response Email HTML</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorPaidResponseEmail" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <b>COMP Response Email HTML</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <telerik:RadEditor runat="server" ID="RadEditorCompResponseEmail" Height="300px" Width="100%" ContentFilters="None"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="View9" runat="server">
                                <div style="padding: 20px 20px 20px 25px">
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td style="text-align: left; width: 20%">External Post URL
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:TextBox ID="txtPostURL" runat="server" MaxLength="200" Width="400px"></asp:TextBox>
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="text-align: left; width: 20%">Query String Details
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: left; width: 100%">Name
                                                                    <asp:TextBox ID="txtQSName" runat="server" MaxLength="50" Width="100px"></asp:TextBox>
                                                                    &nbsp;&nbsp; Value
                                                                  <asp:DropDownList ID="drpQSValue" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="drpQSValue_SelectedIndexChanged">
                                                                  </asp:DropDownList>
                                                                    &nbsp;&nbsp;
                                                                    <asp:TextBox ID="txtQSValue" runat="server" MaxLength="200" Width="100px" Visible="false"></asp:TextBox>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnAddHttpPostURL" runat="server" Text="Add" OnClick="btnAddHttpPostURL_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 100%">
                                                                    <asp:GridView ID="gvHttpPost" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                                                        Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="HttpPostParamsID"
                                                                        OnRowCommand="gvHttpPost_RowCommand">
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblHttpPostParamsID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.HttpPostParamsID") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblParamName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Value">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblParamValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamValue") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="CustomValue">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCustomValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomValue") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                                        CommandName="ParamDelete" OnClientClick="return confirm('Are you sure, you want to delete this parameter?')"
                                                                                        CausesValidation="false" CommandArgument='<%#Eval("HttpPostParamsID")%>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 100%">
                                                                    <asp:Label ID="lblHttpPostPreview" Text="" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                        <div style="text-align: center; padding: 5px 5px 5px 5px">
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div style="text-align: right; padding: 10px 10px 10px 10px">
                            <asp:Button ID="btnPrevious" Text="Previous" runat="server" OnClick="btnPrevious_Click"
                                CssClass="button" />
                            <asp:Button ID="btnNext" Text="Next" runat="server" OnClick="btnNext_Click" CssClass="button" />
                            <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click"
                                runat="server" CssClass="button" OnClientClick="return confirm('Are you sure you want to cancel?');" />
                            <asp:Button ID="btnFinish" Text="Save" runat="server" OnClick="btnFinish_Click" CssClass="button"
                                CausesValidation="true" />
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="sqldatasourceFinish" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
        SelectCommand="sp_PublisherSave" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="PubID" QueryStringField="PubId"
                Type="Int32" />
            <asp:ControlParameter ControlID="TxtName" DefaultValue="0" Name="PubName" PropertyName="Text"
                Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtCode" DefaultValue="0" Name="PubCode" PropertyName="Text"
                Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="ddlCustomer" DefaultValue="0" Name="ECNCustomerID"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="PubLogo" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="MailingLabel" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="ECNDefaultGroupID" Type="Int32" Direction="Input"
                ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="true" Name="IsDefaultGroupEnabled" Type="Boolean" />
            <asp:Parameter DefaultValue="0" Name="ECNSFID" Type="Int32" Direction="Input" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RdbLstIsActive" DefaultValue="false" Name="IsActive"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:Parameter DefaultValue="0" Name="CSS" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorHeaderHTML" DefaultValue="0" Name="HeaderHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorHomePageDesc" DefaultValue="0" Name="HomePageDesc"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorStep2PageDesc" DefaultValue="0" Name="Step2PageDesc"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="drpWidth" DefaultValue="700px" Name="Width" PropertyName="SelectedValue"
                Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="drpColumnFormat" DefaultValue="2" Name="ColumnFormat"
                PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorNewPageDesc" DefaultValue="0" Name="NewPageDesc"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorRenewPageDesc" DefaultValue="0" Name="RenewPageDesc"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorLoginPageDesc" DefaultValue="0" Name="LoginPageDesc"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorForgotPassword" DefaultValue="0" Name="ForgotPasswordHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="DdlLoginVerification" DefaultValue="0" Name="LoginVerfication"
                PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtThankYouPageLink" DefaultValue="0" Name="ThankYouPageLink"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorThankYouPageHTML" DefaultValue="0" Name="ThankYouPageHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorCustomerServiceHTML" DefaultValue="0" Name="CustomerServicePageHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorFAQHTML" DefaultValue="0" Name="FAQPageHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RdbLstShowNewSubscriptionLink" DefaultValue="false"
                Name="ShowNewSubLink" PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RdbLstShowRenewLink" DefaultValue="true" Name="ShowRenewSubLink"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RdbLstShowCustomerServiceLink" DefaultValue="false"
                Name="ShowCustomerServiceLink" PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RdbLstShowRelatedTradeShows" DefaultValue="false"
                Name="ShowTradeShowLink" PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="rblstRepeatEmails" DefaultValue="false"
                Name="RepeatEmails" PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RdbLstShowNewsLetters" DefaultValue="false" Name="ShowNewsletters"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RadEditorFooterHTML" DefaultValue="0" Name="FooterHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="AddedBy" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="ModifiedBy" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter DefaultValue="0" Name="iMode" Type="Int32" />
            <asp:ControlParameter ControlID="RdbLstHasPaid" DefaultValue="false" Name="HasPaid"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="TxtPayflowAccount" DefaultValue="" Name="PayflowAccount"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtPayflowPassword" DefaultValue="" Name="PayflowPassword"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtPayflowSignature" DefaultValue="" Name="PayflowSignature"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txtPayflowPageStyle" DefaultValue="" Name="PayflowPageStyle"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtPayflowPartner" DefaultValue="" Name="PayflowPartner"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="TxtPayflowVendor" DefaultValue="" Name="PayflowVendor"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="rbProcessExternal" DefaultValue="false" Name="ProcessExternal"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="txtProcessExternalURL" DefaultValue="" Name="ProcessExternalURL"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorNewSubscriptionHeader" DefaultValue="New Subscription"
                Name="NewSubscriptionHeader" PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txtNewSubscriptionLink" DefaultValue="New Subscription"
                Name="NewSubscriptionLink" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true" />
            <asp:ControlParameter ControlID="RadEditorManageSubscriptionHeader" DefaultValue=""
                Name="ManageSubscriptionHeader" PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txtManageSubscriptionLink" DefaultValue="Manage Subscription"
                Name="ManageSubscriptionLink" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true" />
            <asp:ControlParameter ControlID="RadEditorRequiredFieldEditor" DefaultValue="" Name="RequiredFieldHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorNewsletterHeader" DefaultValue="" Name="NewsletterHeaderHTML"
                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="rbDisableSubLogin" DefaultValue="false" Name="DisableSubcriberLogin"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="rbDisablePassword" DefaultValue="false" Name="DisablePassword"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="rbDisableEmail" DefaultValue="false" Name="DisableEmail"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RadEditorRedirectorHTML" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="RedirectorHTML" PropertyName="Content" Type="String" />
            <asp:ControlParameter ControlID="txtRedirectorLink" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="RedirectorLink" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtAuthorizeDotNetAccount" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="AuthorizeDotNetAccount" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtAuthorizeDotNetSignature" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="AuthorizeDotNetSignature" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="rblstPaymentGateway" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PaymentGateway" PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="fileUploadCoverImage" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="MagCoverImage" PropertyName="FileName" Type="String" />
            <asp:ControlParameter ControlID="rblstCheckSubscriberExists" DefaultValue="false"
                Name="CheckSubscriber" PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="RadEditorPaidThankyouPage" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PaidThankYouPageHTML" PropertyName="Content" Type="String" />
            <asp:ControlParameter ControlID="RadEditorPaidResponseEmail" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PaidResponseEmail" PropertyName="Content" Type="String" />
            <asp:ControlParameter ControlID="txtPaidPageFromEmail" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PaidPageFromEmail" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPaidFormResponseEmailSubject" DefaultValue=""
                ConvertEmptyStringToNull="false" Name="PaidFormResponseEmailSubject" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtPaidFormFromName" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PaidPageFromName" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="rblstShowShippingAddress" DefaultValue="false" Name="ShowShippingAddress"
                PropertyName="SelectedValue" Type="Boolean" />
            <asp:ControlParameter ControlID="txtPaidThankyouPageLink" DefaultValue="" Name="PaidPageThankyouLink"
                PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="RadEditorCompResponseEmail" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="CompResponseEmailHTML" PropertyName="Content" Type="String" />
            <asp:ControlParameter ControlID="txtPageTitle" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="PageTitle" PropertyName="Text" Type="String" />
            <asp:Parameter Name="URL" Type="String" DefaultValue="0" />
            <asp:Parameter Name="qsNameValue" Type="String" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
