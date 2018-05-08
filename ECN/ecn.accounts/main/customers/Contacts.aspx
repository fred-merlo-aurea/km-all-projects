<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="ecn.accounts.main.customers.Contacts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Contacts</title>
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet_default.css"
        type="text/css">
    <script language="javascript" type="text/javascript">
        function closewindow() {
            if (window.opener && !window.opener.closed) {
                window.opener.location.reload();
            }
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="section">
        <table width="100%" border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td colspan="2">
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="674" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
                            </tr>
                            <tr>
                                <td id="errorMiddle">
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="errorBottom">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    First Name :
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbFirstName" runat="server" Width="150" MaxLength="50" CssClass="label10" />&nbsp;<asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" ControlToValidate="tbFirstName" ErrorMessage="First Name is required. "
                        Display="Dynamic" InitialValue="" Width="100%" runat="server">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    Last Name :
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbLastName" runat="server" Width="150" MaxLength="50" CssClass="label10" />&nbsp;<asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2" ControlToValidate="tbLastName" ErrorMessage="LastName is required. "
                        Display="Dynamic" InitialValue="" Width="100%" runat="server">*</asp:RequiredFieldValidator>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    Email:
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbEmail" runat="server" Width="200" MaxLength="50" CssClass="label10" />
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    Phone:
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbPhone" runat="server" Width="100" MaxLength="20" CssClass="label10" />
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    Mobile:
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbMobile" runat="server" Width="100" MaxLength="20" CssClass="label10" />
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    Address:
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbAddress" runat="server" Width="200" MaxLength="100" CssClass="label10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbAddress2" runat="server" Width="150" MaxLength="50" CssClass="label10" />
                </td>
            </tr>
            <tr>
                <td class="formLabel">
                    City:
                </td>
                <td class="dataOne">
                    <asp:TextBox ID="tbCity" runat="server" Width="100" MaxLength="50" />&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlState" runat="server" Width="120" CssClass="label10">
                        <asp:ListItem Value="" Text="-- Select State --"></asp:ListItem>
                        <asp:ListItem Value="AK" Text="Alaska"></asp:ListItem>
                        <asp:ListItem Value="AL" Text="Alabama"></asp:ListItem>
                        <asp:ListItem Value="AR" Text="Arkansas"></asp:ListItem>
                        <asp:ListItem Value="AZ" Text="Arizona"></asp:ListItem>
                        <asp:ListItem Value="CA" Text="California"></asp:ListItem>
                        <asp:ListItem Value="CO" Text="Colorado"></asp:ListItem>
                        <asp:ListItem Value="CT" Text="Connecticut"></asp:ListItem>
                        <asp:ListItem Value="DC" Text="Washington D.C."></asp:ListItem>
                        <asp:ListItem Value="DE" Text="Delaware"></asp:ListItem>
                        <asp:ListItem Value="FL" Text="Florida"></asp:ListItem>
                        <asp:ListItem Value="GA" Text="Georgia"></asp:ListItem>
                        <asp:ListItem Value="HI" Text="Hawaii"></asp:ListItem>
                        <asp:ListItem Value="IA" Text="Iowa"></asp:ListItem>
                        <asp:ListItem Value="ID" Text="Idaho"></asp:ListItem>
                        <asp:ListItem Value="IL" Text="Illinois"></asp:ListItem>
                        <asp:ListItem Value="IN" Text="Indiana"></asp:ListItem>
                        <asp:ListItem Value="KS" Text="Kansas"></asp:ListItem>
                        <asp:ListItem Value="KY" Text="Kentucky"></asp:ListItem>
                        <asp:ListItem Value="LA" Text="Louisiana"></asp:ListItem>
                        <asp:ListItem Value="MA" Text="Massachusetts"></asp:ListItem>
                        <asp:ListItem Value="MD" Text="Maryland"></asp:ListItem>
                        <asp:ListItem Value="ME" Text="Maine"></asp:ListItem>
                        <asp:ListItem Value="MI" Text="Michigan"></asp:ListItem>
                        <asp:ListItem Value="MN" Text="Minnesota"></asp:ListItem>
                        <asp:ListItem Value="MO" Text="Missourri"></asp:ListItem>
                        <asp:ListItem Value="MS" Text="Mississippi"></asp:ListItem>
                        <asp:ListItem Value="MT" Text="Montana"></asp:ListItem>
                        <asp:ListItem Value="NC" Text="North Carolina"></asp:ListItem>
                        <asp:ListItem Value="ND" Text="North Dakota"></asp:ListItem>
                        <asp:ListItem Value="NE" Text="Nebraska"></asp:ListItem>
                        <asp:ListItem Value="NH" Text="New Hampshire"></asp:ListItem>
                        <asp:ListItem Value="NJ" Text="New Jersey"></asp:ListItem>
                        <asp:ListItem Value="NM" Text="New Mexico"></asp:ListItem>
                        <asp:ListItem Value="NV" Text="Nevada"></asp:ListItem>
                        <asp:ListItem Value="NY" Text="New York"></asp:ListItem>
                        <asp:ListItem Value="OH" Text="Ohio"></asp:ListItem>
                        <asp:ListItem Value="OK" Text="Oklahoma"></asp:ListItem>
                        <asp:ListItem Value="OR" Text="Oregon"></asp:ListItem>
                        <asp:ListItem Value="PA" Text="Pennsylvania"></asp:ListItem>
                        <asp:ListItem Value="PR" Text="Puerto Rico"></asp:ListItem>
                        <asp:ListItem Value="RI" Text="Rhode Island"></asp:ListItem>
                        <asp:ListItem Value="SC" Text="South Carolina"></asp:ListItem>
                        <asp:ListItem Value="SD" Text="South Dakota"></asp:ListItem>
                        <asp:ListItem Value="TN" Text="Tennessee"></asp:ListItem>
                        <asp:ListItem Value="TX" Text="Texas"></asp:ListItem>
                        <asp:ListItem Value="UT" Text="Utah"></asp:ListItem>
                        <asp:ListItem Value="VA" Text="Virginia"></asp:ListItem>
                        <asp:ListItem Value="VT" Text="Vermont"></asp:ListItem>
                        <asp:ListItem Value="WA" Text="Washington"></asp:ListItem>
                        <asp:ListItem Value="WI" Text="Wisconsin"></asp:ListItem>
                        <asp:ListItem Value="WV" Text="West Virginia"></asp:ListItem>
                        <asp:ListItem Value="WY" Text="Wyoming"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tbZip" runat="server" Width="100" MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="100" />
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
