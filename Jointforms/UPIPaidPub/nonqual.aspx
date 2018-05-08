<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nonqual.aspx.cs" Inherits="wattpaidpub.Nonqual" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Non Qual</title>
    <link href="css/styles.css" rel="stylesheet" type="text/css">
    <div id="divcss" runat="server">
    </div>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="innerContainer">
            <div>
                <div>
                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                </div>
                <br />
                <br />
                <div>
                    <asp:Panel ID="pnlNonQualResponse" Visible="false" runat="server">
                        Thank you for completing the subscription form.<br />
                        <br />
                        Based on marking "<asp:Label Font-Bold="true" ID="lblValue" runat="server" />" for
                        "<asp:Label Font-Bold="true" ID="lblQuestion" runat="server" />", this does not
                        qualify you for a free subscription. You can go to:
                        <br />
                        <br />
                        "Back to Form" and review the Primary List again<br />
                        "Pay Here" and enter your credit card information on our secure server<br />
                        <asp:Label Font-Bold="true" ID="lblMagazineName" runat="server" Visible="false" />
                    </asp:Panel>
                    <asp:Panel ID="pnlNonQualCountry" Visible="false" runat="server">
                        <p>
                            Thank you for your interest in subscribing to
                            <asp:Label ID="lblPubName" runat="server" Text="" />.</p>
                        <p>
                            At this time we are not offering free subscriptions in
                            <asp:Label ID="lblCountryName" runat="server" Font-Bold="true" Text="" />, the country
                            you indicated on your subscription form.
                        </p>
                        <p>
                            If you would like to subscribe please click on the "Pay Here" button below to enter
                            your credit card information<br />
                            on our secure payment page.
                        </p>                      
                    </asp:Panel>
                </div>
                <br />
                <br />
                <div style="text-align: center">
                    <asp:Button ID="btnSubscribe"
                            runat="server" Text="Pay Here" OnClick="btnSubscribe_Click" />&nbsp;<asp:Button ID="btnHomePage"
                                runat="server" Text="" Visible="false" />
                </div>
                <br />
                <br />
                <div>
                    <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
