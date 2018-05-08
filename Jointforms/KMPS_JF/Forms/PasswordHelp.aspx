<%@ Page Language="C#" AutoEventWireup="true" Theme="Default" CodeBehind="PasswordHelp.aspx.cs"
    Inherits="KMPS_JF.Forms.PasswordHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password Help</title>
    <style type="text/css">
        .style1
        {
            height: 71px;
        }
        .buttonMedium
        {
            height: 26px;
        }
    </style>
</head>
<body style="background: white url(../images/spacer.gif);">
    <form id="form1" runat="server">
    <div>
        <jf:boxpanel id="BoxPanel2" runat="server" width="100%" height="250px" title="Password Help">
            <asp:Panel ID="pnlstep1" runat="server" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <h3>
                                What is the problem you are experiencing?</h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:RadioButton ID="rbforgot" runat="server" GroupName="group1" Checked="true" />
                            I forgot my password<br />
                            <br />
                            <asp:RadioButton ID="rbreset" runat="server" GroupName="group1" />
                            My password doesn&#39;t work</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage2" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button" OnClick="btnNext_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlstep2" runat="server" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">
                            <h3>
                                Please enter your Email address.</h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtemailaddress" runat="server" Width="250"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtemailaddress"
                                ErrorMessage="<< Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:Button ID="btnFinish" runat="server" Text="Finish" CssClass="buttonMedium" OnClick="btnFinish_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </jf:boxpanel>
    </div>
    </form>
</body>
</html>
