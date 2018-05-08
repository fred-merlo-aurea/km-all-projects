<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sublogin.aspx.cs" Inherits="KMPS_JF.EventSolutions.sublogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Login</title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <div id="divcss" runat="server">
    </div>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="innerContainer">
            <div>
                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                <asp:Panel ID="pnlError" runat="server" Visible="true" Height="100px">
                    <br>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td id="errorTop">
                            </td>
                        </tr>
                        <tr>
                            <td id="errorMiddle">
                                <table height="67" width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img src="../images/errorEx.jpg" style="padding: 0 0 0 15px;" />
                                        </td>
                                        <td valign="middle" align="center" width="80%" height="100%">
                                            <asp:Label ID="lblError" runat="Server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
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
                </asp:Panel> 
                <asp:Panel ID="pnlSubmit" runat="server" Visible="true" Height="200px">               
                    <table width="100%" cellpadding="10" cellspacing="5" border="0">
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" valign="middle" class="label">
                                <asp:Label ID="lblEmailAddress" runat="server" Text="Email Address: "></asp:Label>
                                <asp:TextBox ID="txtEmailAddress" runat="server" Width="250px" CssClass="txtstyle"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmailAddress" ControlToValidate="txtEmailAddress" ErrorMessage="Email Address"
                                        runat="server" Display="Dynamic" Text="<%$ Resources:Resource, requiredfieldImage %>">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" ControlToValidate="txtEmailAddress"
                                    Display="Dynamic" ErrorMessage="<%$ Resources:Resource, emailaddressvalid %>"
                                    ValidationExpression="^([a-zA-Z0-9_\-\.+]+)@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$">
                                </asp:RegularExpressionValidator>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" valign="middle">
                                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, submit %>"
                                        OnClick="btnSubmit_Click" />
                            </td>
                        </tr>                        
                    </table> 
                </asp:Panel>               
                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
