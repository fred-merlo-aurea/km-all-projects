<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassAlong.aspx.cs" Inherits="KMPS_JF.Forms.PassAlong"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/styles.css" rel="stylesheet" type="text/css">
    <div id="divcss" runat="server">
    </div>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script language="javascript" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/js/getConversionData.js"></script>

    <script language="Javascript" type="text/javascript">

        function ValidateAll() {

            var allOk = true;

            if (document.frmsub.user_PA1FNAME.value == "" && document.frmsub.user_PA1LNAME.value == "" && document.frmsub.user_PA1FUNCTION.value == "" && document.frmsub.user_PA1EMAIL.value == "" && document.frmsub.user_PA1FUNCTION.value == ""
                && document.frmsub.user_PA2FNAME.value == "" && document.frmsub.user_PA2LNAME.value == "" && document.frmsub.user_PA2FUNCTION.value == "" && document.frmsub.user_PA2EMAIL.value == "" && document.frmsub.user_PA2FUNCTION.value == ""
                && document.frmsub.user_PA3FNAME.value == "" && document.frmsub.user_PA3LNAME.value == "" && document.frmsub.user_PA3FUNCTION.value == "" && document.frmsub.user_PA3EMAIL.value == "" && document.frmsub.user_PA3FUNCTION.value == ""
                ) {
                allOk = false;
                alert('Please enter First Name, Last Name, Job Title and Email');
                document.frmsub.user_PA1FNAME.focus();
            }

            if (allOk) {
                if ((document.frmsub.user_PA1FNAME.value != "" || document.frmsub.user_PA1LNAME.value != "" || document.frmsub.user_PA1FUNCTION.value != "" || document.frmsub.user_PA1EMAIL.value != "") && (document.frmsub.user_PA1FNAME.value == "" || document.frmsub.user_PA1LNAME.value == "" || document.frmsub.user_PA1FUNCTION.value == "" || document.frmsub.user_PA1EMAIL.value == "")) {
                    allOk = false;
                    alert('Please enter First Name, Last Name, Job Title and Email');
                }

                if (allOk) {
                    if (document.frmsub.user_PA1FUNCTION.value == 'Z' && document.frmsub.user_PA1FUNCTXT.value == "") {
                        allOk = false;
                        alert("Please Specify the Job Title.");
                        document.frmsub.user_PA1FUNCTXT.focus();
                    }
                }

                if (allOk) {
                    if (document.frmsub.user_PA1FNAME.value != "" || document.frmsub.user_PA1LNAME.value != "" || document.frmsub.user_PA1FUNCTION.value != "") {
                        if (!ValidateEmail(document.frmsub.user_PA1EMAIL.value)) {
                            allOk = false;
                            alert("Invalid Email Address.");
                            document.frmsub.user_PA1EMAIL.focus();
                        }
                    }
                }
            }

            if (allOk) {
                if ((document.frmsub.user_PA2FNAME.value != "" || document.frmsub.user_PA2LNAME.value != "" || document.frmsub.user_PA2FUNCTION.value != "" || document.frmsub.user_PA2EMAIL.value != "") && (document.frmsub.user_PA2FNAME.value == "" || document.frmsub.user_PA2LNAME.value == "" || document.frmsub.user_PA2FUNCTION.value == "" || document.frmsub.user_PA2EMAIL.value == "")) {
                    allOk = false;
                    alert('Please enter First Name, Last Name, Job Title and Email');
                }

                if (allOk) {
                    if (document.frmsub.user_PA2FUNCTION.value == 'Z' && document.frmsub.user_PA2FUNCTXT.value == "") {
                        allOk = false;
                        alert("Please Specify the Job Title.");
                        document.frmsub.user_PA2FUNCTXT.focus();
                    }
                }

                if (allOk) {
                    if (document.frmsub.user_PA2FNAME.value != "" || document.frmsub.user_PA2LNAME.value != "" || document.frmsub.user_PA2FUNCTION.value != "") {
                        if (!ValidateEmail(document.frmsub.user_PA2EMAIL.value)) {
                            allOk = false;
                            alert("Invalid Email Address.");
                            document.frmsub.user_PA2EMAIL.focus();
                        }
                    }
                }
            }

            if (allOk) {
                if ((document.frmsub.user_PA3FNAME.value != "" || document.frmsub.user_PA3LNAME.value != "" || document.frmsub.user_PA3FUNCTION.value != "" || document.frmsub.user_PA3EMAIL.value != "") && (document.frmsub.user_PA3FNAME.value == "" || document.frmsub.user_PA3LNAME.value == "" || document.frmsub.user_PA3FUNCTION.value == "" || document.frmsub.user_PA3EMAIL.value == "")) {
                    allOk = false;
                    alert('Please enter First Name, Last Name, Job Title and Email');
                }

                if (allOk) {
                    if (document.frmsub.user_PA3FUNCTION.value == 'Z' && document.frmsub.user_PA3FUNCTXT.value == "") {
                        allOk = false;
                        alert("Please Specify the Job Title.");
                        document.frmsub.user_PA3FUNCTXT.focus();
                    }
                }

                if (allOk) {
                    if (document.frmsub.user_PA3FNAME.value != "" || document.frmsub.user_PA3LNAME.value != "" || document.frmsub.user_PA3FUNCTION.value != "") {
                        if (!ValidateEmail(document.frmsub.user_PA3EMAIL.value)) {
                            allOk = false;
                            alert("Invalid Email Address.");
                            document.frmsub.user_PA3EMAIL.focus();
                        }
                    }
                }
            }

            return allOk;
        }


        function ValidateEmail(email) {

            var emailRegxp = /^([\w]+)(.[\w]+)*@([\w]+)(.[\w]{2,3}){1,2}$/;
            if (emailRegxp.test(email) != true) {
                return false;
            }
            else {
                return true;
            }
        }
        
    </script>

</head>
<body>
    <form id="frmsub" name="frmsub" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ErrorContainer" visible="false" runat="server">
        <center>
            <table class="pageborder" cellpadding="0" cellspacing="0">
                <tr style="height: 100px" valign="middle">
                    <td width="10%">
                        &nbsp;<img src="../Images/ic-error.gif" />
                    </td>
                    <td width="90%" valign="middle" align="left">
                        <asp:Label ID="lblmessage" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="arial"
                            Font-Size="Large">
                       <p>The link used to reach this page is missing vital information for processing. <br /> Please reply to your originating email and request a new URL.</p>
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <div id="container" runat="server">
        <div id="innerContainer">
            <div>
                <div>
                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                </div>
                <div>
                <p>
                    <span class="label">Please let us know your industry colleagues who may not be receiving their own
                        personal subscriptions to
                        <asp:Label ID="lblPubName" Text="" runat="server" />.
                        Thank you.</span>
                </p>                     
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <p class="borderTop padTop">
                                <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                    <tr>
                                        <td align="right" width="20%">
                                            First Name:
                                        </td>
                                        <td align="left" width="80%">
                                            <asp:TextBox ID="user_PA1FNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Last Name:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="user_PA1LNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Job Title:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="user_PA1FUNCTION" runat="server" AutoPostBack="true" OnSelectedIndexChanged="user_PA1FUNCTION_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            If other, please describe:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="user_PA1FUNCTXT" Enabled="false" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="15%">
                                            Email:
                                        </td>
                                        <td align="left" width="85%">
                                            <asp:TextBox ID="user_PA1EMAIL" CssClass="txtstyle" runat="server"  />
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <p class="borderTop padTop">
                                <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                    <tr>
                                        <td align="right" width="20%">
                                            First Name:
                                        </td>
                                        <td align="left" width="80%">
                                            <asp:TextBox ID="user_PA2FNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Last Name:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="user_PA2LNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Job Title:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="user_PA2FUNCTION" runat="server" AutoPostBack="true" OnSelectedIndexChanged="user_PA2FUNCTION_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            If other, please describe:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="user_PA2FUNCTXT" Enabled="false" Width="150" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="20%">
                                            Email:
                                        </td>
                                        <td align="left" width="80%">
                                            <asp:TextBox ID="user_PA2EMAIL" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <p class="borderTop padTop">
                                <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                    <tr>
                                        <td align="right" width="20%">
                                            First Name:
                                        </td>
                                        <td align="left" width="80%">
                                            <asp:TextBox ID="user_PA3FNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Last Name:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="user_PA3LNAME" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Job Title:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="user_PA3FUNCTION" runat="server" AutoPostBack="true" OnSelectedIndexChanged="user_PA3FUNCTION_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            If other, please describe:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="user_PA3FUNCTXT" Enabled="false" Width="150" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="20%">
                                            Email:
                                        </td>
                                        <td align="left" width="80%">
                                            <asp:TextBox ID="user_PA3EMAIL" CssClass="txtstyle" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <p class="borderTop padTop">
                                <asp:Button ID="btnSubmit" Text="Submit" OnClientClick="return ValidateAll()" runat="server"
                                    OnClick="btnSubmit_Click" />
                                <input type="reset" id="reset" name="reset" value="Clear">
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>                  
                </div>
                <br />
                <br />
                <div style="text-align: center">
                    &nbsp;&nbsp;</div>
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
