<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PaidPub.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%;text-align:center; width:100%; vertical-align:middle;padding-top:200px;">
            <asp:Login ID="Login1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderStyle="Solid"
        BorderWidth="1px" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" InstructionText="&nbsp;" 
        Height="150px" Width="300px" 
        RememberMeSet="false" DisplayRememberMe="False" OnAuthenticate="Login1_Authenticate" OnLoggedIn="Login1_LoggedIn">
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
            Font-Names="Verdana" ForeColor="#284775" Width="100px" />
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <TextBoxStyle Width="150px" />
    </asp:Login>
    </div>
    </form>
</body>
</html>
