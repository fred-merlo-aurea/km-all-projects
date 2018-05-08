<%@ Page Language="c#" Inherits="ecn.activityengines.ManageSubscriptions" CodeBehind="ManageSubscriptions.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head>
    <title>Manage my subscription</title>
    <link rel="stylesheet" href="http://static.ak.connect.facebook.com/css/fb_connect.css"
        type="text/css" />
    <style type="text/css">
        .pghdr
        {
            font-weight: bold;
            font-size: 20px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .schdr
        {
            font-weight: bold;
            font-size: 14px;
            font-family: Arial, Helvetica, sans-serif;
            background-color: #eeeeee;
            border: solid 1px #CCCCCC;
        }
        .label
        {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .labelsm
        {
            font-size: 10px;
            font-family: Arial, Helvetica, sans-serif;
        }
    </style>
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet_default.css"
        type="text/css">
</head>
<body text="#000000" vlink="#ffffff" alink="#ffffff" link="#ffffff" bgcolor="#ffffff"
    leftmargin="0" topmargin="10px" marginwidth="0" marginheight="0">
    <form id="frmManager" runat="Server">
    <center>
        <asp:Label ID="MyHeader" runat="Server" EnableViewState="True"></asp:Label>
        <table id="Table1" style="font-family: Arial, Helvetica, sans-serif" cellspacing="0" cellpadding="2" width="700"
            height="200" border='0'>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlManange" runat="Server" Visible="False">
                        <table cellspacing="0" cellpadding="0" width="700" border='0'>
                            <tr>
                                <td valign="middle" align="left" class="gridheader">
                                    <h2 class="pghdr" style="padding-top: 10px" style="text-align: center">
                                        My Profile & Preferences</h2>
                                    <table cellspacing="0" cellpadding="5" width="100%" border='0' align="left">
                                        <tr>
                                            <td class="schdr" colspan='6'>
                                                <div style="float: left; vertical-align: middle; padding-top: 5px">
                                                    Profile Information:</div>
                                                <div style="float: right">
                                                    <fb:login-button onlogin="javascript:loadform();" autologoutlink="false">
                                                    </fb:login-button>

                                                    <script language="javascript" type="text/javascript" src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php"></script>

                                                    <script type="text/javascript">
                                                        FB_RequireFeatures(["XFBML"], function() {

                                                            //FB.Facebook.init("7af353aecf71fabc0a2ba041b7149b41", "xd_receiver.htm"); // developer setup
                                                            FB.init("cce5dd19f522c6a9849c2a19437e96cd", "xd_receiver.htm"); // server setup
                                                        });

                                                        function loadform() {

                                                            var usr = new Array(1);

                                                            usr[0] = FB.Facebook.apiClient.get_session().uid;

                                                            FB.Facebook.apiClient.users_getInfo(usr, ['first_name', 'last_name', 'name', 'birthday', 'pic', 'current_location', 'sex', 'work_history'], function(uinfo, ex) {
                                                                //window.alert("User Info :" + uinfo);
                                                                //window.alert(uinfo[0]['first_name'] + " / " + uinfo[0]['last_name'] + " / " + uinfo[0]['birthday'] + " / " + uinfo[0]['pic'] + " / " + uinfo[0]['current_location']);
                                                                //window.alert("Exception :" + ex);

                                                                //populate fields.
                                                                document.getElementById("txtFirstname").value = uinfo[0]['first_name'];
                                                                document.getElementById("txtLastName").value = uinfo[0]['last_name'];

                                                                if (uinfo[0]['birthday'] != '') {
                                                                    var bday = new Date(uinfo[0]['birthday']);
                                                                    document.getElementById("txtDOB").value = bday.getMonth() + 1 + '/' + bday.getDate() + '/' + bday.getFullYear();
                                                                }

                                                                //alert(uinfo[0]['current_location']['city'] + ' / ' + uinfo[0]['current_location']['state'] + ' / ' + uinfo[0]['current_location']['zip']);

                                                                if (typeof (uinfo[0]['current_location']['city']) != 'undefined') {
                                                                    document.getElementById("txtCity").value = uinfo[0]['current_location']['city'];
                                                                }

                                                                if (typeof (uinfo[0]['current_location']['state']) != 'undefined') {
                                                                    document.getElementById("txtState").value = uinfo[0]['current_location']['state'];
                                                                }

                                                                if (typeof (uinfo[0]['current_location']['zip']) != 'undefined') {
                                                                    document.getElementById("txtZip").value = uinfo[0]['current_location']['zip'];
                                                                }

                                                                if (typeof (uinfo[0]['current_location']['country']) != 'undefined') {
                                                                    document.getElementById("txtCountry").value = uinfo[0]['current_location']['country'];
                                                                }

 if (typeof (uinfo[0]['work_history'][0]) != 'undefined')
 { 
                                                                if (typeof (uinfo[0]['work_history'][0]['company_name']) != 'undefined') {
                                                                    document.getElementById("txtCompany").value = uinfo[0]['work_history'][0]['company_name'];
                                                                }

                                                                if (typeof (uinfo[0]['work_history'][0]['position']) != 'undefined') {
                                                                    document.getElementById("txtJobTitle").value = uinfo[0]['work_history'][0]['position'];
                                                                }
}
                                                                if (typeof (uinfo[0]['sex']) != 'undefined') {

                                                                    if (uinfo[0]['sex'] == 'male') {
                                                                        document.getElementById("rbGender_0").checked = true;
                                                                    }
                                                                    if (uinfo[0]['sex'] == 'female') {
                                                                        document.getElementById("rbGender_1").checked = true;
                                                                    }
                                                                }
                                                            });
                                                        }

                                                    </script>

                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="height:40px">
                                            <td width="5%">
                                                &nbsp;
                                            </td>
                                            <td class="label" width="15%">
                                                <b>Email Address:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="Server" CssClass="label" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td colspan="3" align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label" width="15%">
                                                <b>First Name:</b>
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtFirstname" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td class="label" width="15%">
                                                <b>Last Name:</b>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td width="5%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label">
                                                <b>Job Title:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtJobTitle" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td class="label">
                                                <b>Company:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCompany" runat="server" MaxLength="100"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label">
                                                <b>Phone:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="15"></asp:TextBox>
                                            </td>
                                            <td class="label">
                                                <b>Mobile:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobile" runat="server" MaxLength="15"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label" nowrap="nowrap">
                                                <b>Date of Birth:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOB" runat="server" Width="75" MaxLength="10"></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Invalid date."
                                                    ControlToValidate="txtDOB" Type="Date" MinimumValue="01/01/1900" MaximumValue="12/31/2050"></asp:RangeValidator>
                                            </td>
                                            <td class="label" nowrap="nowrap">
                                                <b>Gender:</b>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList CssClass="label" ID="rbGender" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td><td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label">
                                                <b>Address:</b>
                                            </td>
                                            <td colspan='5'>
                                                <asp:TextBox ID="txtAddress" runat="server" Width="250" MaxLength="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label">
                                                <b>City:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td class="label">
                                                <b>State:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtState" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="label">
                                                <b>Zip:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtZip" runat="server" Width="75" MaxLength="5"></asp:TextBox>
                                            </td>
                                            <td class="label">
                                                <b>Country:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountry" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='6' height="5">
                                                <br />
                                            </td>
                                        </tr>
                                        <asp:PlaceHolder ID="plList" runat="Server">
                                            <tr>
                                                <td class="schdr" colspan='6'>
                                                    <asp:Label ID="lbllistHeader" runat="Server"></asp:Label>:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelsm" valign="bottom" align='right' colspan='6' height="10">
                                                    * If you wish to unsubscribe, UnCheck the check box under Subscribed Column.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='6'>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='6' height="5">
                                                    <asp:DataGrid ID="dgSubscriptionGrid" runat="Server" DataKeyField="EmailGroupID"
                                                        AutoGenerateColumns="False" Width="100%" CssClass="grid" GridLines="None">
                                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                        <ItemStyle></ItemStyle>
                                                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn ItemStyle-Width="30%" DataField="GroupName" HeaderText="NewsLetter"
                                                                ItemStyle-Font-Bold="True"></asp:BoundColumn>
                                                            <asp:BoundColumn ItemStyle-Width="40%" DataField="Description" HeaderText="Description">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Subscribed" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chksubscription" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsSubscribed"))%>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="HTML" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="rbHTML" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isHTML"))%>'>
                                                                    </asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Text" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="rbText" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isText"))%>'>
                                                                    </asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid><br />
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="plEmail" runat="Server">
                                            <tr>
                                                <td class="schdr" colspan='6'>
                                                    <asp:Label ID="lblemailHeader" runat="Server"></asp:Label>:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='6'>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='6' height="5">
                                                    <asp:DataList ID="dtSubscriptionGrid" runat="Server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                        Width="100%" GridLines="both" CellPadding="5" CellSpacing="0" DataKeyField="groupID"
                                                        CssClass="grid">
                                                        <ItemStyle Font-Size="11px" Font-Bold="True" VerticalAlign="Top"></ItemStyle>
                                                        <ItemTemplate>
                                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "GroupName"))%>
                                                            <br />
                                                            <asp:CheckBoxList ID="cbList" runat="Server" Font-Size="10px">
                                                            </asp:CheckBoxList>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <tr>
                                            <td align="center" colspan='6'>
                                                <asp:Button ID="btnSubmit" runat="Server" Text="Click here to submit your changes"
                                                    OnClick="btnSubmit_Click"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr valign="top">
                <td align="center" style="padding-top: 15px; padding-bottom: 15px">
                    <asp:Label ID="lbMessage" runat="Server" Font-Bold="True" ForeColor="red"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="MyFooter" runat="Server" EnableViewState="True"></asp:Label></center>
    </form>
</body>
</html>
