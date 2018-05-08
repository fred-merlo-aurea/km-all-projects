<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetupEdition.aspx.cs" Inherits="ecn.publisher.main.Edition.SetupEdition"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        function reuploadconfirm() {
            if (document.getElementById("pnlUploadedFile") != null) {
                if (document.getElementById("lbluploadedfile").innerHTML != "")
                    return confirm('Reuploading the PDF file will overwrite the existing Digital Edition\nand all the reporting data associated with the current edition will be deleted.\n\n Do you want to Continue?');
                else
                    return true;
            }
        }
//       $(document).ready(function () {
//          $("#divmenu").attr("z-index", 200);
//            $("#divmenu").attr('style', 'position:fixed');
//        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 10px 0; padding: 0;" align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" align="left" style="width: 1018px">
                    <table cellspacing="0" cellpadding="0" border='0'>
                        <tr>
                            <td class="steps tabNav" valign="bottom">
                                <asp:ImageButton ID="ibStep1" runat="Server" CommandArgument="1" CausesValidation="True"
                                    OnCommand="ibStep_Command"></asp:ImageButton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:ImageButton ID="ibStep2" runat="Server" CommandArgument="2" CausesValidation="True"
                                    OnCommand="ibStep_Command"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" align='right' valign="middle" style="width: 1018px">
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:LinkButton ID="btnNext1" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;"
                                OnClick="btnNext_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnPrevious1" CssClass="btnbgGray" runat="Server" CausesValidation="False"
                                Text="&laquo;&nbsp;Previous" OnClick="btnPrevious_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnCancel1" CssClass="btnbgRed" runat="Server" CausesValidation="False"
                                Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                        </li>
                    </ul>
            </tr>
            <tr>
                <td class="greyOutSide offWhite center label" style="width: 1018px">
                    <br />
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="674" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
                            </tr>
                            <tr>
                                <td id="errorMiddle">
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="errorBottom">
                                </td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td class=" greyOutSide offWhite center label" style="width: 1018px">
                    <asp:Panel ID="pnl1" runat="server" Visible="true">
                        <div class="section">
                            <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td valign="middle" align="center" width="5%">
                                        <img src="/ecn.images/images/sendMyCampaign.gif" />
                                    </td>
                                    <td class="headingOne" width="95%">
                                        Edition Details&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="layoutWrapper" cellspacing="0" cellpadding="5" width="800" border="0">
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right" width="150">
                                        <font color="red">*</font> Publication :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlPublicationList" runat="Server" CssClass="formfield" DataValueField="PublicationID"
                                            DataTextField="PublicationName">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="Server" Font-Bold="True"
                                            Font-Italic="True" ErrorMessage=">> required" ControlToValidate="ddlPublicationList"
                                            Font-Size="xx-small"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        <font color="red">*</font> Edition&nbsp;Name :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbEditionName" runat="Server" Width="250" CssClass="formfield"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv1" runat="Server" Font-Bold="True" Font-Italic="True"
                                            ErrorMessage=">> required" ControlToValidate="tbEditionName" Font-Size="xx-small"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlEditionType" runat="server" Visible="false">
                                    <tr>
                                        <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                            Edition&nbsp;Type :&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlEditionType" runat="Server" Width="125px" CssClass="label10"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlEditionType_SelectedIndexChanged">
                                                <asp:ListItem Value="" Selected="True">----- Select Type -----</asp:ListItem>
                                                <asp:ListItem Value="Coupon">Coupon</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlCouponType" runat="server" Visible="false">
                                        <tr>
                                            <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                                <font color="red">*</font> Select List:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlList" runat="Server" Width="250px" CssClass="label10" DataValueField="groupID"
                                                    DataTextField="groupname">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlList" runat="Server" Font-Bold="True" Font-Italic="True"
                                                    ErrorMessage=">> required" ControlToValidate="ddlList" Font-Size="xx-small" InitialValue=""></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </asp:Panel>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Activation Date :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbActivationDate" runat="Server" Width="75" CssClass="formfield"></asp:TextBox>
                                        <asp:RangeValidator runat="server" ID="rngActivationDate" ControlToValidate="tbActivationDate"
                                            Type="Date" MinimumValue="01/01/2001" MaximumValue="12/31/2025" ErrorMessage=">> Please enter a valid date (mm/dd/yyyy)"
                                            Font-Bold="True" Font-Italic="True" Font-Size="xx-small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        De-Activation Date :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbDeActivationDate" runat="Server" Width="75" CssClass="formfield"></asp:TextBox>
                                        <asp:RangeValidator runat="server" ID="rngDeActivationDate" ControlToValidate="tbDeActivationDate"
                                            Type="Date" MinimumValue="01/01/2001" MaximumValue="12/31/2025" ErrorMessage=">> Please enter a valid date (mm/dd/yyyy)"
                                            Font-Bold="True" Font-Italic="True" Font-Size="xx-small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Status :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlStatus" runat="Server" CssClass="formfield" Width="100px">
                                            <asp:ListItem Value="Active" Selected="True">Active</asp:ListItem>
                                            <asp:ListItem Value="Inactive">InActive</asp:ListItem>
                                            <asp:ListItem Value="Archieve">Archive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Enable Security :&nbsp;
                                    </td>
                                    <td align="left" nowrap valign="top">
                                        <asp:RadioButtonList ID="rbSecured" class="formLabel" runat="Server" RepeatDirection="horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                        </asp:RadioButtonList>
                                        &nbsp;&nbsp;<font class="formLabel" style="color: Gray">(If Yes, Login is required to
                                            view this Digital Edition)</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 10px" class="formLabel" valign="middle" align="left">
                                        &nbsp;
                                    </td>
                                    <td class="formLabel" valign="middle" align="left" style="color: Gray">
                                        If you are making your digital edition secure (requiring a username and password
                                        to access the publication), you will need to do these additional steps:<br />
                                        <br />
                                        1) Add two User Defined Fields "Username" and "PWD" to the publication Group (Publication
                                        Name followed by " Subscribers" in group listing)<br />
                                        <br />
                                        2) Make sure "Username" and "PWD" are fields in your list, and be sure to map those
                                        fields when you are uploading your list to the group.<br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnl2" runat="server" Visible="true">
                        <table border="0" cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td width="5%">
                                </td>
                                <td width="15%">
                                </td>
                                <td width="80%">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle">
                                    <img src="/ecn.images/images/sendMyCampaign.gif">
                                </td>
                                <td class="headingOne" colspan="2">
                                    Upload PDF
                                </td>
                            </tr>
                            <asp:Panel ID="pnlEdit" runat="server">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="left">
                                        Current PDF File :&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFileName" class="formLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="left">
                                        Total Pages :&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalPages" class="formLabel" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td style="padding-right: 20px" class="formLabel" valign="middle" align="left" nowrap>
                                    <asp:Label ID="lblUploadLabel" class="formLabel" runat="server"></asp:Label>
                                    :&nbsp;&nbsp;
                                </td>
                                <td valign="bottom">
                                    <table cellpaddding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td valign="middle">
                                                <input class="blue_border_box" id="FileUpload1" type="file" size="60" name="FileUpload1"
                                                    runat="Server">
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="ibSubmit" Text="Submit" ImageUrl="~/images/uploadbutton.gif"
                                                    AlternateText="Upload now" OnClick="ibSubmit_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" valign="middle">
                                                <asp:Label ID="lblnote" Font-Size="xx-small" ForeColor="red" Font-Bold="true" runat="server">Do Not Click Finish button until the upload is complete.</asp:Label><br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:Panel ID="pnlUploadedFile" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="padding-right: 20px" class="formLabel" valign="top" align="left" nowrap>
                                        Uploaded File :&nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lbluploadedfile" Font-Size="x-small" ForeColor="green" Font-Bold="true"
                                            runat="server"></asp:Label><br />
                                        <asp:Label ID="lblnote2" Font-Size="xx-small" ForeColor="green" Font-Bold="true"
                                            runat="server">Click Finish button to convert this PDF to Digital Edition.</asp:Label><br />
                                        <asp:Label ID="lblreuploadmessage" Font-Size="xx-small" ForeColor="red" Font-Bold="true"
                                            runat="server">Reuploading the PDF file will overwrite the existing Digital Edition <BR />and all the reporting data associated with the current edition will be deleted.</asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </table>
                    </asp:Panel>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" valign="middle" align='right' style="width: 1018px">
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:LinkButton ID="btnNext2" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;"
                                OnClick="btnNext_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnPrevious2" CssClass="btnbgGray" runat="Server" CausesValidation="False"
                                Text="&laquo;&nbsp;Previous" OnClick="btnPrevious_Click"></asp:LinkButton>
                            <li>
                                <asp:LinkButton ID="btnCancel2" CssClass="btnbgRed" runat="Server" CausesValidation="False"
                                    Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                            </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
