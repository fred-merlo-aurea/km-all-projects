<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionManagement.aspx.cs" Inherits="ecn.activityengines.engines.SubscriptionManagement" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1" />

    <title>Subscription Management</title>
    <link rel="stylesheet" href="../App_Themes/reset.css" type="text/css" media="screen" />

    <link rel="stylesheet" href="../App_Themes/style.css" type="text/css" media="screen" />

    <link rel="stylesheet" href="../MasterPages/ECN_Controls.css" type="text/css" />
    <script src="../scripts/Modernizr.js"></script>
    <script src="../scripts/Respond.js"></script>
    <script src="../scripts/jquery-1.7.1.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" runat="server" />
        <header>
            <asp:Literal ID="litHeader" runat="server" />
        </header>

        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
            <table cellspacing="0" cellpadding="0" style="background-color: white; width: 100%; text-align: center;">
                <tr>
                    <td id="errorTop"></td>
                </tr>
                <tr>
                    <td id="errorMiddle">
                        <table style="height: 67px; width: 80%;">
                            <tr>
                                <td style="vertical-align: top; text-align: center; width: 20%;">
                                    <img style="padding: 0 0 0 15px;" src="http://images.ecn5.com/images/errorEx.jpg" />
                                </td>
                                <td style="vertical-align: middle; text-align: left; width: 80%; height: 100%;">
                                    <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id="errorBottom"></td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <asp:Panel ID="pnlThankYou" runat="server" Visible="false">
            <section>
                <article>
                    <asp:Label ID="lblThankYouHeading" CssClass="ECN-Label-Heading-Large" Text="Thank you for updating your subscriptions!" runat="server" />
                </article>
            </section>
        </asp:Panel>
        <asp:Panel ID="pnlContent" runat="server">

            <section>

                <asp:Label ID="lblTitle" CssClass="ECN-Label-Heading-Large" runat="server" />
                <br />
                <br />
                <!--Current groups  -->
                <article>
                    <asp:Label ID="lblCurrentHeading" CssClass="ECN-Label-Heading" runat="server">Current</asp:Label>
                    <br />

                    <asp:Panel ID="upCurrentGroups" runat="server">

                            <asp:GridView ID="gvCurrent" DataKeyNames="SubscriptionManagementGroupID" CellPadding="5" CellSpacing="5" OnRowDataBound="gvCurrent_RowDataBound" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="Subscribe" HeaderStyle-CssClass="ECN-Label" HeaderStyle-Font-Size="Small" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rbSubscribed" OnCheckedChanged="rbSubscribed_CheckedChanged" AutoPostBack="true" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unsubscribe" ItemStyle-Width="15%" HeaderStyle-Font-Size="Small" HeaderStyle-CssClass="ECN-Label" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rbUnsubscribed" OnCheckedChanged="rbUnsubscribed_CheckedChanged" Font-Color="Black" AutoPostBack="true" runat="server" />
                                            <asp:HiddenField ID="hfInitial" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="70%">
                                        <ItemTemplate>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblGroupName" Font-Size="Small" CssClass="ECN-Label" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GroupID" Visible="false" />
                                </Columns>
                            </asp:GridView>

                    </asp:Panel>
                </article>
                <!--available groups  -->
                <article>
                    <asp:Label ID="lblAvailableHeading" CssClass="ECN-Label-Heading" runat="server">Available</asp:Label>
                    <br />

                    <asp:Panel ID="pnlAvailable" runat="server">
                        <asp:GridView ID="gvAvailable" CssClass="gridview" DataKeyNames="SubscriptionManagementGroupID" CellPadding="5" CellSpacing="5" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="Subscribe" ItemStyle-Width="25%" HeaderStyle-CssClass="ECN-Label" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSubscribe" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="4%"></asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="71%" ItemStyle-Font-Size="Small" ItemStyle-CssClass="ECN-Label" ItemStyle-HorizontalAlign="Left" DataField="Label" />
                                <asp:BoundField DataField="SMGID" ItemStyle-Width="0%" Visible="false" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </article>
                <article>
                    <asp:Literal ID="lblMSMessage" runat="server" />
                    <asp:GridView ID="gvMasterSuppressed" CellPadding="5" CellSpacing="5" AutoGenerateColumns="false" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Label" ItemStyle-CssClass="ECN-Label" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                </article>
                <article>
                    <asp:Literal ID="lblReasonMessage" runat="server" />
                    <asp:DropDownList ID="ddlReason" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged" AutoPostBack="true" runat="server">
                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                        <asp:ListItem>Email frequency</asp:ListItem>
                        <asp:ListItem>Email volume</asp:ListItem>
                        <asp:ListItem>Content not relevant</asp:ListItem>
                        <asp:ListItem>Signed up for one-time email</asp:ListItem>
                        <asp:ListItem>Circumstances changed(moved, married, changed jobs, etc.)</asp:ListItem>
                        <asp:ListItem>Prefer to get information another way</asp:ListItem>
                        <asp:ListItem>Other(Please specify)</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                    <asp:TextBox ID="txtReason" runat="server" />
                    <asp:Label ID="lblReasonError" runat="server" ForeColor="Red" />
                </article>
            </section>
            <section>
                <!-- Submit -->
                <article style="width: 100%; text-align: center; padding-top: 10px;">
                    <asp:CheckBox ID="chkSendResponse" runat="server" CssClass="ECN-Label" Text="Send me a confirmation email" />
                    <br />
                    <br />
                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit Changes" />
                    <br />


                </article>
            </section>
        </asp:Panel>
        <footer>
            <asp:Literal ID="litFooter" runat="server"></asp:Literal>
        </footer>

    </form>
</body>
</html>
