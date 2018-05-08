<%@ Page Language="C#" EnableEventValidation="false" Theme="Default" AutoEventWireup="true" CodeBehind="subscribe.aspx.cs" Inherits="PaidPub.subscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>France Media Paid Subscription Form</title>
    <script src='https://www.google.com/recaptcha/api.js' type="text/javascript"></script>
        <script src="scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <link type="text/css" href="CSS/styles.css" rel="Stylesheet" />
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="subsScriptManager" runat="server">
        </asp:ScriptManager>
        <div id="container">
            <div id="innerContainer">
                <div id="container-content">
                    <div id="banner">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <img border="0" src="images/header.jpg" width="640px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
                        BorderWidth="1px">
                        <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.jpg'); background-repeat: no-repeat">
                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server">
                        <asp:UpdatePanel ID="grdUpdatePanel" runat="server">
                            <ContentTemplate>
                                <p>
                                    &nbsp;<asp:Panel ID="pnlCountry" runat="server" Visible="false">
                                        <div>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Country:" ID="lblFilterCoun" CssClass="label" runat="server" />&nbsp;
                                                    <asp:DropDownList ID="countryName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ListCountryChanged">
                                                    </asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdateProgress ID="udProgress" runat="server" AssociatedUpdatePanelID="grdUpdatePanel">
                                                            <ProgressTemplate>
                                                                <sub>
                                                                    <img border="0" src="./images/animated-loading-orange.gif" /></sub><span style="font-size: 10px; color: Black">Please wait...</span>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <p>
                                        <asp:HiddenField ID="hfProductCount" runat="server" Value="0" />
                                        <asp:GridView ID="grdMagazines" runat="server" AutoGenerateColumns="false" HorizontalAlign="Center"
                                            ShowFooter="true" SkinID="skitems" Width="100%" OnRowDataBound="grdMagazines_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Cover" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="lblImageID" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"coverImage") %>' Width="114px" Height="40px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="title" HeaderText="Magazine" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Term" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="drpQuantity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpQuantity_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblQuantity" runat="server" Text="1" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Print Edition" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" SortExpression="true">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"groupID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblCustID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"custID") %>'
                                                            Visible="false" />
                                                        <asp:Label ID="lblPubCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"pubcode") %>'
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkPrint" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chBoxPrint_CheckedChanged"
                                                            Visible="true" />
                                                        <asp:Label ID="lblUsPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uspriceone") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUsPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uspricetwo") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCanPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"canpriceone") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCanPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"canpricetwo") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblIntPrice1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"intpriceone") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblIntPrice2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"intpricetwo") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMessage" Font-Bold="true" runat="server" Text='' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrText" runat="server" CssClass="label" Font-Bold="true" Text="Total:" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Free Digital" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                    <ItemTemplate>
                                                        <a runat="server" href='<%# DataBinder.Eval(Container.DataItem,"jflink") %>'>Free Digital</a>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Price" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                    ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <span class="label">$<asp:Label ID="lblTotalAmount" Text="0.00" runat="server" /></span>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </p>
                                    <p class="borderBottom padBottom">
                                        <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td colspan="4">
                                                        <span class="label">* Indicates a required field. </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td colspan="4">
                                                        <font face="verdana,arial,helvetica"><b>Contact Information:</b></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">
                                                        <span class="label">* Email:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="email" runat="server" Width="130" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true"
                                                            runat="server" ControlToValidate="email" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="&lt;&lt; Invalid Email Address"
                                                            EnableClientScript="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ControlToValidate="email"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">
                                                        <span class="label">* First Name:</span>&nbsp;
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="first" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="first" />
                                                    </td>
                                                    <td width="15%" align="left">
                                                        <span class="label">* Last Name:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="last" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="last"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">
                                                        <span class="label">* Address:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="address1" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="address1"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="15%" align="left">
                                                        <span class="label">&nbsp;&nbsp;P.O.Box:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="address2" Width="130" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">
                                                        <span class="label">* Company:</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="company" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="company"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="15%" align="left">
                                                        <span class="label">* City:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="city" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="city"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">

                                                        <asp:Label ID="lblStateProv" runat="server" CssClass="label">* State:</asp:Label>

                                                    </td>
                                                    <td width="35%">
                                                        <asp:Panel ID="pnlState1" runat="server" Visible="false">
                                                            <asp:DropDownList Width="130" ID="state" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                                ControlToValidate="state"></asp:RequiredFieldValidator>

                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlState2" runat="server" Visible="false">
                                                            <asp:TextBox ID="txtState" Width="130" runat="server" />
                                                        </asp:Panel>

                                                    </td>
                                                    <td width="15%" align="left">
                                                        <span class="label">* Zip Code:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="zip" Width="130" runat="server" MaxLength="10" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="zip"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr valign="baseline">
                                                    <td width="15%" align="left">
                                                        <span class="label">* Phone Number:</span>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="phone" Width="130" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                                            ControlToValidate="phone"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="15%" align="left">
                                                        <span class="label">&nbsp;&nbsp;Fax Number:</span>&nbsp;
                                                    </td>
                                                    <td width="35%">
                                                        <asp:TextBox ID="fax" Width="130" runat="server" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <p class="borderBottom padBottom">
                            <table cellspacing="3" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <img src="https://www.ecn5.com/ecn.Images/images/testMyCampaign.gif" />&nbsp;<font
                                            face="verdana,arial,helvetica"><b>Payment Information:</b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%">
                                        <span class="label">* Name as it appears on the card:</span>
                                    </td>
                                    <td width="35%">
                                        <asp:TextBox ID="user_CardHolderName" runat="server" Width="130" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" EnableClientScript="true"
                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="user_CardHolderName"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label">* Credit Card Type:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="user_CCType" runat="server" Width="130">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="MasterCard"></asp:ListItem>
                                            <asp:ListItem Value="Visa"></asp:ListItem>
                                            <asp:ListItem Value="Amex"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_CCType"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label">* Card Number (No dashes):</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="user_CCNumber" runat="server" Width="130" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_CCNumber"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <span class="label">* Expiration Date:</span>
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList ID="user_Exp_Month" runat="server">
                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="01">Jan</asp:ListItem>
                                            <asp:ListItem Value="02">Feb</asp:ListItem>
                                            <asp:ListItem Value="03">Mar</asp:ListItem>
                                            <asp:ListItem Value="04">Apr</asp:ListItem>
                                            <asp:ListItem Value="05">May</asp:ListItem>
                                            <asp:ListItem Value="06">Jun</asp:ListItem>
                                            <asp:ListItem Value="07">Jul</asp:ListItem>
                                            <asp:ListItem Value="08">Aug</asp:ListItem>
                                            <asp:ListItem Value="09">Sep</asp:ListItem>
                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                            <asp:ListItem Value="12">Dec</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_Exp_Month" Display="Dynamic"></asp:RequiredFieldValidator><asp:DropDownList
                                            ID="user_Exp_Year" runat="server">
                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="16">2016</asp:ListItem>
                                            <asp:ListItem Value="17">2017</asp:ListItem>
                                            <asp:ListItem Value="18">2018</asp:ListItem>
                                            <asp:ListItem Value="19">2019</asp:ListItem>
                                            <asp:ListItem Value="20">2020</asp:ListItem>
                                            <asp:ListItem Value="21">2021</asp:ListItem>
                                            <asp:ListItem Value="22">2022</asp:ListItem>
                                            <asp:ListItem Value="23">2023</asp:ListItem>
                                            <asp:ListItem Value="24">2024</asp:ListItem>
                                            <asp:ListItem Value="25">2025</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="user_Exp_Year"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <span class="label">&nbsp;&nbsp;Card Verification Number:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="user_CCVerfication" runat="server" />
                                        &nbsp;&nbsp;&nbsp;<img src="https://www.ecn5.com/ecn.Images/images/img_credit_card.gif"
                                            width="182" height="55" align="textTop"><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18"
                                        runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="user_CCVerfication"></asp:RequiredFieldValidator>--%>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </p>
                        <br />
                        <p>
                            <center>
                                <table align="center" border="0">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <div class="g-recaptcha" data-sitekey="6LdH3AoUAAAAAL-QRSj-PeLIYrbckR_wTd48ub6l"></div>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="pnlButton" runat="server" Visible="true">
                                                <asp:Button ID="btnSecurePayment" Text="Submit" Width="110px" runat="server"
                                                    OnClick="btnSecurePayment_Click" CausesValidation="true" UseSubmitBehavior="false" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlButtonDisabled" runat="server" Visible="true">
                                                <asp:Button ID="btnSecurePaymentDisabled" Text="Processing...." Width="110px" runat="server"
                                                    Enabled="false" Style="visibility: hidden;" />
                                            </asp:Panel>
                                        </td>
                                        <td align="center" valign="top">
                                            <asp:Panel ID="pnlbtnReset" runat="server" Visible="true">
                                                <asp:Button ID="btnReset" Text="Reset" runat="server" Width="110px" CausesValidation="false"
                                                    OnClick="btnReset_Click" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlReset" runat="server" Visible="true">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                    </asp:Panel>
                </div>
                <!-- end container-content -->
                <div id="footer">
                    <table align="center">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <img src="images/footer.jpg" width="640px" height="100" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--end footer-->
            </div>
        </div>
        <div id="modalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10000; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
        </div>
        <div id="loadingModalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10019; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
        </div>
        <div id="loadingPanel" style="display: none; z-index: 10020; position: fixed; width: 200px; height: 200px; background-color: white;  left: 50%; top:50%;border-radius:5px;margin:-100px 0 0 -100px;">
            
                <div class='uil-default-css' style='transform: scale(0.46); margin-left:10px;'>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(0deg) translate(0,-60px); transform: rotate(0deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(30deg) translate(0,-60px); transform: rotate(30deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(60deg) translate(0,-60px); transform: rotate(60deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(90deg) translate(0,-60px); transform: rotate(90deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(120deg) translate(0,-60px); transform: rotate(120deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(150deg) translate(0,-60px); transform: rotate(150deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(180deg) translate(0,-60px); transform: rotate(180deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(210deg) translate(0,-60px); transform: rotate(210deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(240deg) translate(0,-60px); transform: rotate(240deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(270deg) translate(0,-60px); transform: rotate(270deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(300deg) translate(0,-60px); transform: rotate(300deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                    <div style='top: 80px; left: 93px; width: 14px; height: 40px; background: #559edb; -webkit-transform: rotate(330deg) translate(0,-60px); transform: rotate(330deg) translate(0,-60px); border-radius: 10px; position: absolute;'></div>
                </div>
                
            
            <span style=' text-align: center; font-family: Arial; font-size: 12px; color: #8C8989;'>Please wait…</span>
        </div>
    </form>
</body>
    <script language="javascript" type="text/javascript">
    var canSubmit = false;
    // OnClientClick="doubleSubmitCheck()" for btnSecurePayment control.
    $(document).ready(function(){
        $("#btnSecurePayment").click(function (event) {
            if(!canSubmit)
            {
                event.preventDefault();
            
                if (doubleSubmitCheck() == true)
                {
                    canSubmit = true;
                    $(this).trigger('click');
                }
            }
            else{

            }
            
        });
    });
    function doubleSubmitCheck() {

        var validation = Page_ClientValidate();

        if (validation) {
            
            $("#loadingModalBackgroundDiv").show();
            $("#loadingPanel").show();
            return true;
            <%--document.getElementById("<%= btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%= btnSecurePayment.ClientID %>").style.display = 'none';
            return true;--%>
        }
        else {
            return false;
        }
    }

    function hideConfirm() {
        $("#modalBackgroundDiv").hide();
        $("#pnlConfirmSubmit").hide();
    }

    function showLoading() {
        $("#loadingModalBackgroundDiv").show();
        $("#loadingPanel").show();
    }
</script>

</html>
