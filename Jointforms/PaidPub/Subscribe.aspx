<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subscribe.aspx.cs" Inherits="PaidPub.Subscribe" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Subscription Form</title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <!-- START Conversion Tracking -->

    <script language="javascript" src="http://www.ecn5.com/ecn.accounts/assets/channelID_1/js/Conversion.js">
    </script>

    <!-- END Conversion Tracking -->

    <script type="text/javascript">
<!--
        function onclickchange(ctrl, resetctrlid) {

            if (ctrl.checked)
                document.getElementById(resetctrlid).checked = false;
        }

        function ongridclickchange(ctrl) {
            var targetctrlID = "";

            if (ctrl.checked) {
                if (ctrl.id.indexOf("chkUnsubscribe") > 0)
                    targetctrlID = ctrl.id.replace(/chkUnsubscribe/, "chkRenew");
                else
                    targetctrlID = ctrl.id.replace(/chkRenew/, "chkUnsubscribe");

                document.getElementById(targetctrlID).checked = false;
            }
        }

// -->
    </script>

</head>
<body class="prototype">
    <form name="frmsub1" id="frmsub1" runat="server">
    <div id="container">
        <asp:PlaceHolder ID="banner" runat="server"></asp:PlaceHolder>
        <div id="innerContainer">
            <asp:Panel ID="pnlEmailAddress" runat="server" Visible="true">

                <script language="Javascript">
	<!--
                    ECNstepname = 'EnterEmailAddress';
                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	//-->
                </script>

                <center>
                    <label>
                        <asp:Label ID="lblDesc" runat="server" Width="100%"></asp:Label>
                    </label>
                    <br />
                    <br />
                    Email Address :
                    <asp:TextBox runat="server" ID="txtE" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ControlToValidate="txtE"
                        runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="120" CssClass="formButton"
                        OnClick="btnSubmit_Click" CausesValidation="true" />
                </center>
            </asp:Panel>
            <asp:Panel ID="pnlLogin" runat="server" Visible="false">

                <script language="Javascript">
	<!--
                    ECNstepname = 'Login';
                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	//-->
                </script>

                <div id="loginPanel" style="visibility: visible; width: 100%; text-align: center;"
                    runat="server">
                    <div class="eNewsDefault" style="text-align: left">
                        Please enter your password and click <i>Next</i>.
                    </div>
                    <br />
                    <asp:Login ID="loginCtrl" runat="server" BorderStyle="Solid" BackColor="#EFF3FB"
                        BorderWidth="1px" BorderColor="#B5C7DE" Font-Size="0.8em" Font-Names="Verdana"
                        UserNameLabelText="E-mail address:" FailureText="Wrong Password<br>[OR]<br>User does not exist."
                        RememberMeText="" UserNameRequiredErrorMessage="&laquo;&laquo; E-mail address is required to Login"
                        PasswordRequiredErrorMessage="&laquo;&laquo; Password is required to Login" OnLoggingIn="loginCtrl_LoggingIn"
                        OnAuthenticate="loginCtrl_Authenticate" BorderPadding="2" ForeColor="#333333"
                        Height="180px" Width="300px" DisplayRememberMe="False" PasswordRecoveryText="Forgot your password."
                        LoginButtonText="Next" PasswordRecoveryUrl="forgotpassword.aspx">
                        <TitleTextStyle Font-Bold="True" ForeColor="White" BackColor="#507CD1" Height="25px"
                            Font-Size="1.1em" HorizontalAlign="Center" VerticalAlign="Middle" Width="100%">
                        </TitleTextStyle>
                        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                        <TextBoxStyle Font-Size="0.8em" Width="155px" />
                        <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
                    </asp:Login>
                    <br />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlStep1" runat="server" Visible="false">

                <script language="Javascript">
	<!--
                    ECNstepname = 'SubscriptionSelection';
                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	//-->
                </script>

                <div style="padding-left: 2em; padding-right: 2em;">
                    <asp:Panel ID="pnlPromoDesc" runat="server">
                        <label>
                            <asp:Label ID="lblPromotionDesc" runat="server" ForeColor="#000000" Font-Bold="true"
                                Font-Size="Small"></asp:Label>
                        </label>
                        <br />
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="pnlCurrentSubscriptions" runat="server" Visible="false">
                        <div class="eNewsHeader">
                            <strong>Current Subscriptions :</strong></div>
                        <br>
                        <center>
                            <asp:GridView ID="gvSubscribed" runat="server" DataKeyNames="groupID" AutoGenerateColumns="False"
                                Width="80%" PageSize="20" CellPadding="4" BackColor="White" BorderColor="#CC9966"
                                BorderStyle="None" BorderWidth="1px">
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="groupname" HeaderText="Newsletter">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FreeOrPaid" HeaderText="Subscription Type">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" Width="30%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Renew" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRenew" onclick="ongridclickchange(this);" Visible='<%# DataBinder.Eval(Container.DataItem, "FREEorpaid").ToString().Trim()=="COMP" || DataBinder.Eval(Container.DataItem, "FREEorpaid").ToString().Trim()=="TRIAL"?false:true %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cancel" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkUnsubscribe" onclick="ongridclickchange(this);"
                                                Visible='<%# DataBinder.Eval(Container.DataItem, "FREEorpaid").ToString().Trim()=="COMP"  || DataBinder.Eval(Container.DataItem, "FREEorpaid").ToString().Trim()=="TRIAL" ?false:true %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                        </center>
                    </asp:Panel>
                    <br />
                    <label>
                        <asp:Label ID="lblenewsletter" runat="server" Width="100%"></asp:Label>
                    </label>
                    <br />
                    <br />
                    <asp:Repeater ID="rptCategory" runat="Server" OnItemDataBound="rptCategory_ItemDataBound">
                        <ItemTemplate>
                            <br />
                            <asp:Label ID="lblgroupIDs" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "groups") %>'></asp:Label>
                            <table cellpadding="2" cellspacing="0" border="0" width="625px" align="center">
                                <asp:Panel ID="pnlCategory" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "CategoryName")==""?false:true %>'>
                                    <tr style="background-color: #EDEDF5" width="625px">
                                        <td align="left" width="100%" style="padding: 6px 5px 6px 5px; color: black; font-weight: bold">
                                            <%# DataBinder.Eval(Container.DataItem, "CategoryName") %>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr bgcolor="#ffffff">
                                    <td align="left" width="625px">
                                        <asp:GridView ID="gvNewsletters" runat="server" AllowPaging="False" AllowSorting="True"
                                            ShowHeader="false" GridLines="None" Width="625px" AutoGenerateColumns="false"
                                            DataKeyNames="GroupID" CellPadding="15" CellSpacing="10">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Print" ItemStyle-HorizontalAlign="center"
                                                    ItemStyle-Width="5%" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Print" ItemStyle-HorizontalAlign="left"
                                                    ItemStyle-Width="25%" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <i><b>
                                                            <%# Eval("GroupName")%></b></i>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Print" ItemStyle-HorizontalAlign="left"
                                                    ItemStyle-Width="75%" ItemStyle-Wrap="true" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <%# Eval("GroupDescription")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlStep2" runat="server" Visible="false">

                <script language="Javascript">
	<!--
                    ECNstepname = 'Checkout';
                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	//-->
                </script>

                <br />
                <div id="divSubscriptionOption" runat="server" style="padding-left: 2em; padding-right: 2em;">
                    <font size="3" style="font-weight: bold; font-family: Arial;">Subscription Term</font>&nbsp;&nbsp;&nbsp;
                    <br>
                    <br>
                    <div style="padding-left: 2em; padding-right: 2em;">
                        <font size="2" style="font-weight: bold; font-family: Arial;">Select a Subscription
                            Option</font>&nbsp;&nbsp;&nbsp;
                        <asp:RadioButtonList ID="rbYears" runat="server" CellSpacing="5" CellPadding="5"
                            RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true"
                            OnSelectedIndexChanged="rbYears_SelectedIndexChanged">
                            <asp:ListItem Text="1 Year" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="2 Years" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3 Years" Value="3"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <br />
                </div>
                <div id="divTrialSub" runat="server" style="padding-left: 2em; padding-right: 2em;">
                    <font size="3" style="font-weight: bold; font-family: Arial;">Subscription Term : </font>&nbsp;&nbsp;&nbsp;
                        <font size="2" style="font-weight: bold; font-family: Arial;">7 day trial.</font>&nbsp;&nbsp;&nbsp;
                                        <br>
                    <br>

                </div>
                <font size="3" style="font-weight: bold; font-family: Arial; padding-left: 2em; padding-right: 2em;">
                    User Information</font>&nbsp;&nbsp;&nbsp;
                <br>
                <br />
                <div id="profile">
                    <p>
                        <label>
                            <font color="#000000">First Name:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtfirstname" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtfirstname"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Last Name:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtlastname" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtlastname"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Job Title:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txttitle" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txttitle"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Company:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtcompany" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtcompany"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Address:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtaddress" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="txtaddress"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Address 2:&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtaddress2" Width="200"></asp:TextBox>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">City:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtcity" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="txtcity"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">State/Province:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:DropDownList ID="drpstate" runat="server">
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
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"
                            InitialValue="" ControlToValidate="drpstate" Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p class="zip">
                        <label>
                            <font color="#000000">Postcode:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtzip" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="txtzip"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p class="country">
                        <label>
                            <font color="#000000">Country:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:DropDownList ID="drpcountry" runat="server">
                            <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                            <asp:ListItem Value="AL">Albania</asp:ListItem>
                            <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                            <asp:ListItem Value="AD">Andorra</asp:ListItem>
                            <asp:ListItem Value="AO">Angola</asp:ListItem>
                            <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                            <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                            <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                            <asp:ListItem Value="AR">Argentina</asp:ListItem>
                            <asp:ListItem Value="AM">Armenia</asp:ListItem>
                            <asp:ListItem Value="AW">Aruba</asp:ListItem>
                            <asp:ListItem Value="AU">Australia</asp:ListItem>
                            <asp:ListItem Value="AT">Austria</asp:ListItem>
                            <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                            <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                            <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                            <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                            <asp:ListItem Value="BB">Barbados</asp:ListItem>
                            <asp:ListItem Value="BY">Belarus</asp:ListItem>
                            <asp:ListItem Value="BE">Belgium</asp:ListItem>
                            <asp:ListItem Value="BZ">Belize</asp:ListItem>
                            <asp:ListItem Value="BJ">Benin</asp:ListItem>
                            <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                            <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                            <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                            <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                            <asp:ListItem Value="BW">Botswana</asp:ListItem>
                            <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                            <asp:ListItem Value="BR">Brazil</asp:ListItem>
                            <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                            <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                            <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                            <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                            <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                            <asp:ListItem Value="BI">Burundi</asp:ListItem>
                            <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                            <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                            <asp:ListItem Value="CA">Canada</asp:ListItem>
                            <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                            <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                            <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                            <asp:ListItem Value="TD">Chad</asp:ListItem>
                            <asp:ListItem Value="CL">Chile</asp:ListItem>
                            <asp:ListItem Value="CN">China</asp:ListItem>
                            <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                            <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                            <asp:ListItem Value="CO">Colombia</asp:ListItem>
                            <asp:ListItem Value="KM">Comoros</asp:ListItem>
                            <asp:ListItem Value="CG">Congo</asp:ListItem>
                            <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                            <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                            <asp:ListItem Value="CI">Cote D'ivoire</asp:ListItem>
                            <asp:ListItem Value="HR">Croatia</asp:ListItem>
                            <asp:ListItem Value="CU">Cuba</asp:ListItem>
                            <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                            <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                            <asp:ListItem Value="KP">Democratic People's Republic Of Korea</asp:ListItem>
                            <asp:ListItem Value="DK">Denmark</asp:ListItem>
                            <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                            <asp:ListItem Value="DM">Dominica</asp:ListItem>
                            <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                            <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                            <asp:ListItem Value="EG">Egypt</asp:ListItem>
                            <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                            <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                            <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                            <asp:ListItem Value="EE">Estonia</asp:ListItem>
                            <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                            <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                            <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                            <asp:ListItem Value="FM">Federated States Of Micronesia</asp:ListItem>
                            <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                            <asp:ListItem Value="FI">Finland</asp:ListItem>
                            <asp:ListItem Value="FR">France</asp:ListItem>
                            <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                            <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                            <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                            <asp:ListItem Value="GA">Gabon</asp:ListItem>
                            <asp:ListItem Value="GM">Gambia</asp:ListItem>
                            <asp:ListItem Value="GE">Georgia</asp:ListItem>
                            <asp:ListItem Value="DE">Germany</asp:ListItem>
                            <asp:ListItem Value="GH">Ghana</asp:ListItem>
                            <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                            <asp:ListItem Value="GR">Greece</asp:ListItem>
                            <asp:ListItem Value="GL">Greenland</asp:ListItem>
                            <asp:ListItem Value="GD">Grenada</asp:ListItem>
                            <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                            <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                            <asp:ListItem Value="GN">Guinea</asp:ListItem>
                            <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                            <asp:ListItem Value="GY">Guyana</asp:ListItem>
                            <asp:ListItem Value="HT">Haiti</asp:ListItem>
                            <asp:ListItem Value="HM">Heard Island And Mcdonald Islands</asp:ListItem>
                            <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                            <asp:ListItem Value="HN">Honduras</asp:ListItem>
                            <asp:ListItem Value="HK">Hong Kong</asp:ListItem>
                            <asp:ListItem Value="HU">Hungary</asp:ListItem>
                            <asp:ListItem Value="IS">Iceland</asp:ListItem>
                            <asp:ListItem Value="IN">India</asp:ListItem>
                            <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                            <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                            <asp:ListItem Value="IE">Ireland</asp:ListItem>
                            <asp:ListItem Value="IR">Islamic Republic Of Iran</asp:ListItem>
                            <asp:ListItem Value="IL">Israel</asp:ListItem>
                            <asp:ListItem Value="IT">Italy</asp:ListItem>
                            <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                            <asp:ListItem Value="JP">Japan</asp:ListItem>
                            <asp:ListItem Value="JO">Jordan</asp:ListItem>
                            <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                            <asp:ListItem Value="KE">Kenya</asp:ListItem>
                            <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                            <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                            <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                            <asp:ListItem Value="LA">Lao People's Democratic Republic</asp:ListItem>
                            <asp:ListItem Value="LV">Latvia</asp:ListItem>
                            <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                            <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                            <asp:ListItem Value="LR">Liberia</asp:ListItem>
                            <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>
                            <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                            <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                            <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                            <asp:ListItem Value="MO">Macao</asp:ListItem>
                            <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                            <asp:ListItem Value="MW">Malawi</asp:ListItem>
                            <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                            <asp:ListItem Value="MV">Maldives</asp:ListItem>
                            <asp:ListItem Value="ML">Mali</asp:ListItem>
                            <asp:ListItem Value="MT">Malta</asp:ListItem>
                            <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                            <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                            <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                            <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                            <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                            <asp:ListItem Value="MX">Mexico</asp:ListItem>
                            <asp:ListItem Value="MC">Monaco</asp:ListItem>
                            <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                            <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                            <asp:ListItem Value="MA">Morocco</asp:ListItem>
                            <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                            <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                            <asp:ListItem Value="NA">Namibia</asp:ListItem>
                            <asp:ListItem Value="NR">Nauru</asp:ListItem>
                            <asp:ListItem Value="NP">Nepal</asp:ListItem>
                            <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                            <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                            <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                            <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                            <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                            <asp:ListItem Value="NE">Niger</asp:ListItem>
                            <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                            <asp:ListItem Value="NU">Niue</asp:ListItem>
                            <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                            <asp:ListItem Value="NO">Norway</asp:ListItem>
                            <asp:ListItem Value="PS">Occupied Palestinian Territory</asp:ListItem>
                            <asp:ListItem Value="OM">Oman</asp:ListItem>
                            <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                            <asp:ListItem Value="PW">Palau</asp:ListItem>
                            <asp:ListItem Value="PA">Panama</asp:ListItem>
                            <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                            <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                            <asp:ListItem Value="PE">Peru</asp:ListItem>
                            <asp:ListItem Value="PH">Philippines</asp:ListItem>
                            <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                            <asp:ListItem Value="PL">Poland</asp:ListItem>
                            <asp:ListItem Value="PT">Portugal</asp:ListItem>
                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                            <asp:ListItem Value="QA">Qatar</asp:ListItem>
                            <asp:ListItem Value="MD">Republic Of</asp:ListItem>
                            <asp:ListItem Value="KR">Republic Of Korea</asp:ListItem>
                            <asp:ListItem Value="RE">Reunion</asp:ListItem>
                            <asp:ListItem Value="RO">Romania</asp:ListItem>
                            <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                            <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                            <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                            <asp:ListItem Value="KN">Saint Kitts And Nevis</asp:ListItem>
                            <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                            <asp:ListItem Value="PM">Saint Pierre And Miquelon</asp:ListItem>
                            <asp:ListItem Value="VC">Saint Vincent And The Grenadines</asp:ListItem>
                            <asp:ListItem Value="WS">Samoa</asp:ListItem>
                            <asp:ListItem Value="SM">San Marino</asp:ListItem>
                            <asp:ListItem Value="ST">Sao Tome And Principe</asp:ListItem>
                            <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                            <asp:ListItem Value="SN">Senegal</asp:ListItem>
                            <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                            <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                            <asp:ListItem Value="SG">Singapore</asp:ListItem>
                            <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                            <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                            <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                            <asp:ListItem Value="SO">Somalia</asp:ListItem>
                            <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                            <asp:ListItem Value="GS">South Georgia And The South Sandwich Islands</asp:ListItem>
                            <asp:ListItem Value="ES">Spain</asp:ListItem>
                            <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                            <asp:ListItem Value="SD">Sudan</asp:ListItem>
                            <asp:ListItem Value="SR">Suriname</asp:ListItem>
                            <asp:ListItem Value="SJ">Svalbard And Jan Mayen</asp:ListItem>
                            <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                            <asp:ListItem Value="SE">Sweden</asp:ListItem>
                            <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                            <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>
                            <asp:ListItem Value="TW">Taiwan</asp:ListItem>
                            <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                            <asp:ListItem Value="TH">Thailand</asp:ListItem>
                            <asp:ListItem Value="CD">The Democratic Republic Of The Congo</asp:ListItem>
                            <asp:ListItem Value="MK">The Former Yugoslav Republic Of Macedonia</asp:ListItem>
                            <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                            <asp:ListItem Value="TG">Togo</asp:ListItem>
                            <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                            <asp:ListItem Value="TO">Tonga</asp:ListItem>
                            <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                            <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                            <asp:ListItem Value="TR">Turkey</asp:ListItem>
                            <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                            <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                            <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                            <asp:ListItem Value="VI">U.S. Virgin Islands</asp:ListItem>
                            <asp:ListItem Value="UG">Uganda</asp:ListItem>
                            <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                            <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                            <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                            <asp:ListItem Value="TZ">United Republic Of Tanzania</asp:ListItem>
                            <asp:ListItem Selected="true" Value="US">United States</asp:ListItem>
                            <asp:ListItem Value="UM">United States Minor Outlying Islands</asp:ListItem>
                            <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                            <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                            <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                            <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                            <asp:ListItem Value="VN">Viet Nam</asp:ListItem>
                            <asp:ListItem Value="WF">Wallis and Futuna</asp:ListItem>
                            <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                            <asp:ListItem Value="YE">Yemen</asp:ListItem>
                            <asp:ListItem Value="YU">Yugoslavia</asp:ListItem>
                            <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                            <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="drpcountry"
                            InitialValue="" runat="server" ErrorMessage="*" Font-Size="X-Small"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Phone:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtphone" Width="200"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtphone"
                            runat="server" ErrorMessage="*" Font-Size="X-Small"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Fax:</font></label>
                        <asp:TextBox runat="server" ID="txtfax" Width="200"></asp:TextBox>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Email Address:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtemail" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                            ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="txtemail"
                            Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Password:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtPassword" Width="200" CssClass="formborder" TextMode="Password"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="txtPassword"
                            runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Confirm Password:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtCPassword" Width="200" CssClass="formborder" TextMode="Password"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ControlToValidate="txtCPassword"
                            runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage=">> Password does not match."
                            Type="String" ControlToValidate="txtCPassword" ControlToCompare="txtPassword"></asp:CompareValidator>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Promotion Code:&nbsp;</font></label>
                        <asp:TextBox runat="server" ID="txtpromo" Width="200" CssClass="formborder" ReadOnly></asp:TextBox>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Company Description:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:DropDownList ID="user_Business" runat="server" CssClass="formborder">
                            <asp:ListItem Value="">Your company's primary business activity...</asp:ListItem>
                            <asp:ListItem Value="01">&nbsp;&nbsp;Pharmaceutical manufacturer</asp:ListItem>
                            <asp:ListItem Value="11">&nbsp;&nbsp;Biotechnology company</asp:ListItem>
                            <asp:ListItem Value="15">&nbsp;&nbsp;Generic pharmaceutical
                                                                manufacturer</asp:ListItem>
                            <asp:ListItem Value="09">&nbsp;&nbsp;Medical equipment manufacturer</asp:ListItem>
                            <asp:ListItem Value="12">&nbsp;&nbsp;Contract research organization</asp:ListItem>
                            <asp:ListItem Value="13">&nbsp;&nbsp;Clinical study site/SMO</asp:ListItem>
                            <asp:ListItem Value="14">&nbsp;&nbsp;Clinical lab</asp:ListItem>
                            <asp:ListItem Value="03">&nbsp;&nbsp;Healthcare communications
                                                                company</asp:ListItem>
                            <asp:ListItem Value="04">&nbsp;&nbsp;Marketing services company</asp:ListItem>
                            <asp:ListItem Value="05">&nbsp;&nbsp;General business services
                                                                company</asp:ListItem>
                            <asp:ListItem Value="06">&nbsp;&nbsp;Hospital</asp:ListItem>
                            <asp:ListItem Value="07">&nbsp;&nbsp;Academic/University
                                                                research institution</asp:ListItem>
                            <asp:ListItem Value="08">&nbsp;&nbsp;Government agency</asp:ListItem>
                            <asp:ListItem Value="10">&nbsp;&nbsp;Data management company</asp:ListItem>
                             <asp:ListItem Value="40">&nbsp;&nbsp;Packaging Company</asp:ListItem>
                             <asp:ListItem Value="41">&nbsp;&nbsp;Executive Recruitment Agency</asp:ListItem>
                             <asp:ListItem Value="42">&nbsp;&nbsp;Venture Capital/Investment Firm</asp:ListItem>
                             <asp:ListItem Value="43">&nbsp;&nbsp;Consulting Firm</asp:ListItem>
                             
                            <asp:ListItem Value="44">&nbsp;&nbsp;Media Company</asp:ListItem>
                            
                            <asp:ListItem Value="99">&nbsp;&nbsp;Other support or service
                                                                company</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="user_Business"
                            InitialValue="" runat="server" ErrorMessage="*" Font-Size="X-Small"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            <font color="#000000">Primary Responsibility:</font><font color="#ff0000">*&nbsp;</font></label>
                        <asp:DropDownList ID="user_Responsibility" runat="server" CssClass="formborder">
                            <asp:ListItem Value="">Your primary area of responsibility...</asp:ListItem>
                            <asp:ListItem Value="09">&nbsp;&nbsp;R&D management</asp:ListItem>
                            <asp:ListItem Value="10">&nbsp;&nbsp;Senior management</asp:ListItem>
                            <asp:ListItem Value="34">&nbsp;&nbsp;Finance management</asp:ListItem>
                            <asp:ListItem Value="38">&nbsp;&nbsp;Business strategy</asp:ListItem>
                            <asp:ListItem Value="12">&nbsp;&nbsp;Product management</asp:ListItem>
                            <asp:ListItem Value="23">&nbsp;&nbsp;Marketing,advertising or promotion management</asp:ListItem>
                            <asp:ListItem Value="14">&nbsp;&nbsp;Sales management</asp:ListItem>
                            <asp:ListItem Value="15">&nbsp;&nbsp;Agency account management</asp:ListItem>
                            <asp:ListItem Value="19">&nbsp;&nbsp;Marketing services</asp:ListItem>
                            <asp:ListItem Value="20">&nbsp;&nbsp;Media management (incl. directors/planners)</asp:ListItem>
                            <asp:ListItem Value="24">&nbsp;&nbsp;Medical director/associate medical director</asp:ListItem>
                            <asp:ListItem Value="25">&nbsp;&nbsp;Clinical trials management</asp:ListItem>
                            <asp:ListItem Value="26">&nbsp;&nbsp;Clinical/drug research</asp:ListItem>
                            <asp:ListItem Value="28">&nbsp;&nbsp;Clinical monitoring/CRC/CRA</asp:ListItem>
                            <asp:ListItem Value="31">&nbsp;&nbsp;Clinical documentation preparation</asp:ListItem>
                            <asp:ListItem Value="27">&nbsp;&nbsp;Regulatory affairs</asp:ListItem>
                            <asp:ListItem Value="22">&nbsp;&nbsp;Quality control</asp:ListItem>
                            <asp:ListItem Value="33">&nbsp;&nbsp;Drug safety</asp:ListItem>
                            <asp:ListItem Value="32">&nbsp;&nbsp;Project management</asp:ListItem>
                            <asp:ListItem Value="29">&nbsp;&nbsp;Academic research or professor</asp:ListItem>
                            <asp:ListItem Value="30">&nbsp;&nbsp;Data management or analysis</asp:ListItem>
                            <asp:ListItem Value="35">&nbsp;&nbsp;Licensing</asp:ListItem>
                            <asp:ListItem Value="36">&nbsp;&nbsp;Manufacturing</asp:ListItem>
                            <asp:ListItem Value="37">&nbsp;&nbsp;IT management</asp:ListItem>
                            <asp:ListItem Value="99">&nbsp;&nbsp;Other functions</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="user_Responsibility"
                            runat="server" ErrorMessage="*" Font-Size="X-Small" InitialValue=""></asp:RequiredFieldValidator>
                    </p>
                </div>
                <p>
                    <center>
                        <asp:CheckBox ID="chkVerify" runat="server" Text="I have confirmed that the information above regarding my subscription is accurate  and that I have read and acknowledged the terms of use." /><font
                            color="#ff0000">*&nbsp;</font>
                        <br />
                        <br />
                        <font color="#ff0000">*&nbsp;</font> = Required Field
                    </center>
                </p>
                <br />
                <div style="padding-left: 2em; padding-right: 2em;">
                    <asp:GridView ID="gvPrice" runat="server" AllowPaging="False" AllowSorting="False"
                        GridLines="both" ShowHeader="true" Width="625px" AutoGenerateColumns="false"
                        DataKeyNames="GroupID" CellPadding="10" CellSpacing="0" OnRowDataBound="gvPrice_RowDataBound"
                        ShowFooter="True">
                        <Columns>
                            <asp:BoundField DataField="GroupName" HeaderStyle-Width="40%" HeaderText="Newsletter(s)"
                                ItemStyle-Width="40%" ItemStyle-Wrap="true" SortExpression="GroupName" FooterStyle-HorizontalAlign="Right"
                                FooterStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="x-Small">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Regular Price" HeaderStyle-Width="15%" FooterStyle-HorizontalAlign="center"
                                FooterStyle-Font-Bold="true" ItemStyle-ForeColor="#000000" FooterStyle-ForeColor="#000000"
                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" ItemStyle-Width="15%"
                                HeaderStyle-Font-Size="x-Small">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegularPrice" runat="server" Visible="true" Text='<%# String.Format("{0:C}", DataBinder.Eval(Container.DataItem, "regularprice"))%>'></asp:Label>
                                    <asp:Label ID="lblSavings" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "savings")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SAVINGS" ItemStyle-Width="15%" HeaderStyle-Width="15%"
                                FooterStyle-HorizontalAlign="center" FooterStyle-Font-Bold="true" ItemStyle-ForeColor="#840000"
                                FooterStyle-ForeColor="#840000" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center" HeaderStyle-Font-Size="x-Small" HeaderStyle-ForeColor="RED">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Your Price" ItemStyle-Width="15%" HeaderStyle-Width="15%"
                                FooterStyle-HorizontalAlign="center" FooterStyle-Font-Bold="true" ItemStyle-ForeColor="#000000"
                                FooterStyle-ForeColor="#000000" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center" HeaderStyle-Font-Size="x-Small">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div style="padding-left: 2em; padding-right: 2em; text-align: center">
                    <asp:Button ID="btnPrevious" Text="Revise newsletter selection" runat="server" OnClick="btnPrevious_Click"
                        CausesValidation="false" />
                </div>
                <asp:Panel ID="pnlCheckout" runat="server">
                    <div style="padding-left: 2em; padding-right: 2em;">
                        <asp:Label ID="lblBilling" runat="server" ForeColor="#000000" Font-Bold="true" Font-Size="Small">Billing Information</asp:Label>
                        <br />
                        <br />
                        <div style="padding-left: 3em;">
                            <table cellspacing="2" cellpadding="2" width="625px" border="0">
                                <tr>
                                    <td width="200px">
                                        Payment method<font color="#ff0000">*&nbsp;</font>
                                    </td>
                                    <td width="425px">
                                        <asp:DropDownList ID="drpCreditCard" runat="server">
                                            <asp:ListItem Value="">-- Select Card --</asp:ListItem>
                                            <asp:ListItem Value="MasterCard">Master Card</asp:ListItem>
                                            <asp:ListItem Value="Visa">Visa</asp:ListItem>
                                            <asp:ListItem Value="Amex">American Express</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfvdrpCreditCard" runat="server" Font-Size="xx-small"
                                            ControlToValidate="drpCreditCard" ErrorMessage="&laquo; required" Font-Italic="True"
                                            Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Credit card number<font color="#ff0000">*&nbsp;</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="50" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="rfvtxtCardNumber" runat="server" Font-Size="xx-small" ControlToValidate="txtCardNumber"
                                            ErrorMessage="&laquo; required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Expiration date<font color="#ff0000">*&nbsp;</font>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpMonth" runat="server">
                                            <asp:ListItem Value="01" Selected="True">Jan</asp:ListItem>
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
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpYear" runat="server">
                                            <asp:ListItem Value="08" Selected="True">2008</asp:ListItem>
                                            <asp:ListItem Value="09">2009</asp:ListItem>
                                            <asp:ListItem Value="10">2010</asp:ListItem>
                                            <asp:ListItem Value="11">2011</asp:ListItem>
                                            <asp:ListItem Value="12">2012</asp:ListItem>
                                            <asp:ListItem Value="13">2013</asp:ListItem>
                                            <asp:ListItem Value="14">2014</asp:ListItem>
                                            <asp:ListItem Value="15">2015</asp:ListItem>
                                            <asp:ListItem Value="16">2016</asp:ListItem>
                                            <asp:ListItem Value="17">2017</asp:ListItem>
                                            <asp:ListItem Value="18">2018</asp:ListItem>
                                            <asp:ListItem Value="19">2019</asp:ListItem>
                                            <asp:ListItem Value="20">2020</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Security Code<font size="1"> (on back of card)</font><font color="#ff0000">*&nbsp;</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcvNumber" runat="server" MaxLength="50" Width="250"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator14" runat="server" Font-Size="xx-small" ControlToValidate="txtcvNumber"
                                            ErrorMessage="&laquo; required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Name as it appears on card<font color="#ff0000">*&nbsp;</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCardHolderName" runat="server" MaxLength="100" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="rfvtxtCardHolderName" runat="server" Font-Size="xx-small" ControlToValidate="txtCardHolderName"
                                            ErrorMessage="&laquo; required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div style="padding-left: 3em;">
                            If you would like to pay by check, please print this and the previous pages, and
                            submit with payment to:<br />
                            <br />
                            Canon Pharma Group LLC<br />
                            Subscription Services<br />
                            828A Newtown-Yardley Road<br />
                            Newtown, PA 18940<br />
                            <br />
                            Checks should be payable to: Canon Pharma Group LLC<br />
                            <br />
                            I have reviewed the information above. Please process my subscription request.<br />
                            <br />
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <div id="divError" style="padding-left: 2em; padding-right: 2em; text-align: center;
                padding-bottom: 5px;">
                <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Font-Size="x-Small"
                    Font-Bold="true" BorderColor="Red" BorderStyle="Solid" BorderWidth="1" ForeColor="red"></asp:Label>
            </div>
            <div id="pnllogos" style="text-align: center">
                <asp:Button ID="btnRegister" Text="Register" runat="server" OnClick="btnRegister_Click" />
                &nbsp;
            </div>
            <br />
            <div id="footer" runat="server">
            </div>
            <br />
        </div>
    </div>
    </form>
</body>
</html>
