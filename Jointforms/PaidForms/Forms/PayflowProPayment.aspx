<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayflowProPayment.aspx.cs" Inherits="PaidForms.Forms.PayPalPayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="subsScriptManager" runat="server">
        </asp:ScriptManager>
        <div id="container">
            <div id="innerContainer">
                <div id="container-content">
                    <div id="banner">
                        <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                    </div>
                    <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
                        BorderWidth="1px">
                        <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.jpg'); background-repeat: no-repeat">
                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <asp:HyperLink ID="hlPaidFormLink" runat="server" Text="Click here " /><asp:Label ID="lblPaidFormLink" runat="server" Text="to go back to the form and try to submit again." />
                        </div>
                    </asp:Panel>
                    <asp:Panel id="pnlContent" runat="server">
                        <table>
                            <tr>
                                <td>
                                     <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <br />
                </div>
                <!-- end container-content -->
                <div id="footer">
                    <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                </div>
                <!--end footer-->
            </div>
        </div>
    </form>
</body>
</html>
