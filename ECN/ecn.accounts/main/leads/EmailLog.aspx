<%@ Page Language="c#" Inherits="ecn.accounts.main.leads.EmailLog" Codebehind="EmailLog.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>EmailLog</title>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <asp:DataGrid ID="dgdEmailLog" Style="z-index: 101; left: 8px; position: absolute;
            top: 8px" runat="Server" AutoGenerateColumns="False" Width="100%" CssClass="grid">
            <HeaderStyle CssClass="gridheader" />
            <ItemStyle />
            <Columns>
                <asp:BoundColumn DataField="EmailID" HeaderText="EmailID" HeaderStyle-Width="100"></asp:BoundColumn>
                <asp:BoundColumn DataField="ActionTypeCode" HeaderText="Action" HeaderStyle-Width="100">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ActionDate" HeaderText="Date" HeaderStyle-Width="200"></asp:BoundColumn>
                <asp:BoundColumn DataField="ActionValue" HeaderText="Action Value" HeaderStyle-Width="100">
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </form>
</body>
</html>
