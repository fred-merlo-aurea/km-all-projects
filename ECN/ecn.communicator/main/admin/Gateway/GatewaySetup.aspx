<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="GatewaySetup.aspx.cs" Inherits="ecn.communicator.main.admin.Gateway.GatewaySetup" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="~/includes/uploader.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $("#<%= DtTime_SingleDate.ClientID %>").datepicker({
                    defaultDate: "+1w",
                    changeMonth: true,
                    numberOfMonths: 3,
                    onSelect: function (selectedDate) {
                        $("#<%= DtTime_SingleDate.ClientID %>").datepicker();
                    }
                });
                }
        });
    </script>
    <style type="text/css">
        .overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel4" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <asp:Panel runat="server" ID="pnlNoAccess" Visible="false">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSettings">
                <br />
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2" style="text-align: left;">
                            <asp:Label ID="lblHeading" runat="server" Text="Gateway Setup" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left;">
                            <table style="margin-left: 10px; width: 100%;">
                                <tr>
                                    <td style="text-align: right; width: 10%;">Name:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtGatewayName" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvGatewayName" runat="server" ControlToValidate="txtGatewayName" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">Group:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="txtGatewayGroup" runat="server"></asp:Label>

                                        <asp:ImageButton ID="imgbtnSelectGroup" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" OnClick="imgbtnSelectGroup_Click" />
                                        <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                        <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">PubCode:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtGatewayPubCode" runat="server" />
                                        <asp:RequiredFieldValidator ID="revGatewayPubCode" runat="server" ControlToValidate="txtGatewayPubCode" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">TypeCode:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtGatewayTypeCode" runat="server" />
                                        <asp:RequiredFieldValidator ID="revGatewayTypeCode" runat="server" ControlToValidate="txtGatewayTypeCode" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">Header:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtGatewayHeader" runat="server" Width="80%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">Footer:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtGatewayFooter" runat="server" Width="80%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; vertical-align: top;">
                            <asp:Panel ID="pnlPassword" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: left;" colspan="2">
                                            <asp:Label ID="lblForgotPassword" runat="server" Text="Forgot Password" Font-Bold="true" Font-Size="Medium" />
                                        </td>
                                    </tr>

                                    <tr id="trForgotPasswordText" runat="server">
                                        <td style="width: 10%; text-align: right;">Text
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtForgotPasswordText" runat="server" />
                                            <asp:RequiredFieldValidator ID="revForgotPassword" runat="server" ControlToValidate="txtForgotPasswordText" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right;">
                                            <table>
                                                <tr>
                                                    <td>Make Field Visible &nbsp; 
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblForgotPasswordVisible" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblForgotPasswordVisible_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                            <asp:ListItem Selected="True" Value="true" Text="Yes" />
                                                            <asp:ListItem Value="false" Text="No" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>

                        <td style="width: 50%; text-align: left; vertical-align: top;">
                            <asp:Panel ID="pnlSignup" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: left;" colspan="2">
                                            <asp:Label ID="lblSignup" runat="server" Text="Signup" Font-Bold="true" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr id="trSignupText" runat="server">
                                        <td style="text-align: right; width: 10%;">Text
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtSignupText" runat="server" />
                                            <asp:RequiredFieldValidator ID="revSignupText" runat="server" ControlToValidate="txtSignupText" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                        </td>
                                    </tr>
                                    <tr id="trSignupURL" runat="server">
                                        <td style="text-align: right;">URL
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtSignupURL" runat="server" />
                                            <asp:RequiredFieldValidator ID="revSignupURL" runat="server" ControlToValidate="txtSignupURL" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right;">
                                            <table>
                                                <tr>
                                                    <td>Make Field Visible&nbsp;
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:RadioButtonList ID="rblSignupVisible" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSignupVisible_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                            <asp:ListItem Selected="True" Value="true" Text="Yes" />
                                                            <asp:ListItem Value="false" Text="No" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <asp:Label ID="lblSubmit" runat="server" Text="Submit" Font-Bold="true" Font-Size="Medium" />
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 10%;">Text
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtSubmitText" runat="server" />
                                        <asp:RequiredFieldValidator ID="revSubmitText" runat="server" ControlToValidate="txtSubmitText" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align: top;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <asp:Label ID="lblStyling" runat="server" Text="Styling" Font-Bold="true" Font-Size="Medium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top; width: 10%;"></td>
                                    <td style="text-align: left;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblStyling" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="rblStyling_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="Use KM Standard" Value="default" Selected="True" />
                                                        <asp:ListItem Text="Link to External CSS" Value="external" />
                                                        <asp:ListItem Text="Upload CSS" Value="upload" />
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="vertical-align: top; text-align: left; padding-top: 10px;">
                                                    <asp:Button ID="hlKMDefault" runat="server" OnClick="hlKMDefault_Click" Text="Download" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Panel ID="pnlCSS" runat="server">
                                            <asp:Label ID="lblStyleSelector" runat="server" />
                                            <asp:TextBox ID="txtStyle" runat="server" />
                                            <asp:RequiredFieldValidator ID="revStyle" runat="server" ControlToValidate="txtStyle" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                            &nbsp;

                                            <asp:ImageButton ID="imgbtnUploadCSS" runat="server" OnClick="imgbtnUploadCSS_Click" Style="vertical-align: middle;" ImageUrl="~/images/ecn-icon-gear-small.png" />

                                            <asp:HiddenField ID="hfFilePath" runat="server" Value="" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left; vertical-align: top;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblConfirmation" runat="server" Text="Confirmation Page" Font-Bold="true" Font-Size="Medium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblConfirmationPage" runat="server" OnSelectedIndexChanged="rblConfirmationPage_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Vertical">
                                                        <asp:ListItem Selected="True" Text="Show Page" Value="page" />
                                                        <asp:ListItem Text="Show Page with Redirect URL" Value="pagewithredirect" />
                                                        <asp:ListItem Text="Show Page and Auto-Redirect" Value="pagewithautoredirect" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <table>
                                                        <tr>
                                                            <td style="text-align: right; width: 15%;">Message
                                                            </td>
                                                            <td style="width: 80%; text-align: left;">
                                                                <asp:TextBox ID="txtConfirmationMessage" runat="server" />
                                                                <asp:RequiredFieldValidator ID="revConfirmationMessage" runat="server" ControlToValidate="txtConfirmationMessage" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Panel ID="pnlRedirect" runat="server">
                                                        <table style="width: 24%;">
                                                            <tr>
                                                                <td style="text-align: right;">Text
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtConfirmationText" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="revConfirmationText" runat="server" ControlToValidate="txtConfirmationText" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; width: 50%;">URL
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtConfirmationRedirectURL" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="rfvConfirmationRedirectURL" runat="server" ControlToValidate="txtConfirmationRedirectURL" ErrorMessage="Required" Display="Dynamic" ValidationGroup="Save" />
                                                                    <asp:RegularExpressionValidator ID="revConfirmationRedirectURL" runat="server" ControlToValidate="txtConfirmationRedirectURL" ErrorMessage="Invalid" Display="Dynamic" ValidationGroup="Save" ValidationExpression="^((https|http):\/\/([\-a-zA-Z0-9-]+\.)([a-zA-Z0-9-]+\.).{2,3}.*)" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Panel ID="pnlAutoRedirect" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>Delay Before Redirect
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlConfirmationRedirectDelay" runat="server">
                                                                        <asp:ListItem Value="0" Text="Automatic" />
                                                                        <asp:ListItem Value="5" Text="5 seconds" Selected="True" />
                                                                        <asp:ListItem Value="10" Text="10 seconds" />
                                                                        <asp:ListItem Value="15" Text="15 seconds" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>

                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left;">
                            <table style="width: 50%;">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <asp:Label ID="lblLoginOrCapture" runat="server" Text="Login Or Capture" Font-Bold="true" Font-Size="Medium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top; width: 15%;">
                                        <asp:RadioButtonList ID="rblLoginOrCapture" runat="server" OnSelectedIndexChanged="rblLoginOrCapture_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Vertical">
                                            <asp:ListItem Text="Capture" Value="capture" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Login" Value="login" />
                                        </asp:RadioButtonList>

                                    </td>
                                    <td style="text-align: left; vertical-align: top; width: 85%;">
                                        <asp:ImageButton ID="imgbtnAddCaptureField" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" OnClick="imgbtnAddCaptureField_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; margin-left: 10px;" colspan="2">
                                        <asp:Panel ID="pnlLogin" runat="server">
                                            <table style="width: 100%;">
                                                <tr style="height: 30px; vertical-align: middle;">
                                                    <td>
                                                        <asp:CheckBox ID="chkLoginValidateEmail" runat="server" Enabled="false" Checked="true" Text="Validate Email Address" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 30px; vertical-align: middle;">
                                                    <td>
                                                        <asp:CheckBox ID="chkLoginValidatePassword" runat="server" Text="Validate Password" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 30px; vertical-align: middle;">
                                                    <td>
                                                        <asp:CheckBox ID="chkLoginValidateCustom" runat="server" Text="Validate Custom" />&nbsp;
                                                        <asp:ImageButton ID="imgbtnAddCustom" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" OnClick="imgbtnAddCustom_Click" />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:GridView ID="gvCustomValidate" Width="80%" runat="server" AutoGenerateColumns="false" OnRowCommand="gvCustomValidate_RowCommand" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="IsStatic" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Is Static" />
                                                                <asp:BoundField DataField="Label" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Label" />
                                                                <asp:BoundField DataField="Field" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="UDF" />
                                                                <asp:BoundField DataField="Comparator" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Comparator" />
                                                                <asp:BoundField DataField="Value" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Value" />
                                                                <asp:TemplateField ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDeleteCustom" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' CommandName="deletecustom" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlCapture" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="text-align: right; width: 20%;">Email Address
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:CheckBox ID="chkCaptureEmail" runat="server" Text="" Enabled="false" Checked="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="gvCaptureCustom" Width="60%" runat="server" AutoGenerateColumns="false" OnRowCommand="gvCaptureCustom_RowCommand" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="IsStatic" HeaderText="Is Static" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="Label" HeaderText="Label" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="Field" HeaderText="UDF" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="Value" HeaderText="Value" ItemStyle-HorizontalAlign="Center" />

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDeleteCaptureCustom" runat="server" CommandName="deletecustom" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' ImageUrl="/ecn.images/images/icon-delete1.gif" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <asp:Button ID="btnSaveGateway" runat="server" Text="Save" ValidationGroup="Save" OnClick="btnSaveGateway_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="hfCustomValidate" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeCustomValidate" runat="server" TargetControlID="hfCustomValidate" PopupControlID="pnlCustomValidateRule" BackgroundCssClass="modalBackground" />
    <asp:UpdatePanel ID="pnlCustomValidateRule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="background-color: white; width: 500px; height: 350px;">
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="chkValidateIsStatic" runat="server" Checked="true" AutoPostBack="true" Text="Is Static" OnCheckedChanged="chkValidateIsStatic_CheckedChanged" />
                    </td>
                </tr>
                <asp:Panel ID="pnlNonStaticValidate" Visible="false" runat="server">
                    <tr>

                        <td style="text-align: right;">Label
                        </td>
                        <td>
                            <asp:TextBox ID="txtValidateNonStaticLabel" runat="server" />
                        </td>

                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlStaticValidate" runat="server" Visible="true">
                    <tr>

                        <td style="text-align: right;">Field
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlCustomField" runat="server">
                            </asp:DropDownList>
                        </td>

                    </tr>
                </asp:Panel>
                <tr>
                    <td style="text-align: right;">Field Type
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlCustomFieldType" OnSelectedIndexChanged="ddlCustomFieldType_SelectedIndexChanged" AutoPostBack="true" runat="server">
                            <asp:ListItem Selected="True" Value=""></asp:ListItem>
                            <asp:ListItem Value="VARCHAR(500)">String</asp:ListItem>
                            <asp:ListItem Value="INT">Number</asp:ListItem>
                            <asp:ListItem Value="DATETIME">Date [MM/DD/YYYY]</asp:ListItem>
                            <asp:ListItem Value="DECIMAL(11,2)">Money $$</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">Comparator
                    </td>
                    <td style="text-align: left;" colspan="2">
                        <asp:CheckBox ID="chkCustomCompNot" runat="server" Text="NOT" />&nbsp;
                        <asp:DropDownList ID="ddlComparator" OnSelectedIndexChanged="ddlComparator_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td style="text-align: right; width: 20%;">Value
                    </td>
                    <td style="text-align: left; width: 35%;">
                        <asp:Panel ID="Default_CompareValuePanel" runat="Server" Visible="true">
                            <asp:TextBox ID="txtCustomValue" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="DtTime_CompareValuePanel" runat="Server" Visible="false">
                            <table>
                                <tr id="trSingleDate" runat="server">
                                    <td align="right"></td>
                                    <td>
                                        <asp:TextBox class="formfield" ID="DtTime_SingleDate" runat="Server" Columns="15"
                                            MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>

                    </td>
                    <td style="text-align: left; width: 45%;">
                        <asp:ImageButton ID="imgbtnAddLoginValidator" runat="server" ImageUrl="/ecn.images/images/icon-add.gif" OnClick="imgbtnAddLoginValidator_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="ErrorLabel" runat="Server" Visible="false" CssClass="errormsg"></asp:Label><br />
                        <asp:CustomValidator ID="CompValueNumberValidator" runat="Server" CssClass="errormsg"
                            ErrorMessage="" ControlToValidate="txtCustomValue" ClientValidationFunction="IsNumber"
                            Enabled="false">ERROR: Compare Value that you have entered cannot be converted to Number. Only Numbers are Allowed. Please Correct !</asp:CustomValidator><br />
                        <asp:CustomValidator ID="DtTime_Value1Validator" runat="Server" CssClass="errormsg"
                            ErrorMessage="" ControlToValidate="DtTime_SingleDate" ClientValidationFunction="isValidDateFrom"
                            Enabled="true">ERROR: 'Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:CustomValidator><br />

                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div style="height: 100px; overflow: auto;">
                            <asp:GridView ID="gvCustomValidateValues" runat="server" Width="100%" AutoGenerateColumns="false" OnRowCommand="gvCustomValidateValues_RowCommand" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="IsStatic" HeaderText="Is Static" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Label" HeaderText="Label" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Field" HeaderText="UDF" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Comparator" HeaderText="Comparator" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Value" HeaderText="Value" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />

                                    <asp:TemplateField ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDeleteCaptureValue" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.ID") %>' CommandName="deletecustomvalue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSaveCustomValidator" runat="server" OnClick="btnSaveCustomValidator_Click" Text="Save" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCloseCustomValidator" runat="server" OnClick="btnCloseCustomValidator_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>

                    </td>

                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfCaptureCustom" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeCaptureCustom" runat="server" TargetControlID="hfCaptureCustom" PopupControlID="pnlCaptureCustom" BackgroundCssClass="modalBackground" />
    <asp:UpdatePanel ID="pnlCaptureCustom" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="background-color: white; width: 300px; height: 200px;">
                <tr>
                    <td style="width: 20%;"></td>
                    <td style="width: 80%; text-align: left;">
                        <asp:CheckBox ID="chkCaptureIsStatic" runat="server" Text="Is Static" Checked="true" AutoPostBack="true" OnCheckedChanged="chkCaptureIsStatic_CheckedChanged" />
                    </td>
                </tr>

                <tr>
                    <td style="text-align: right;">UDFs:
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlUDF" runat="server" />
                        &nbsp;<asp:ImageButton ID="imgbtnAddCapture" runat="server" OnClick="imgbtnAddCapture_Click" ValidationGroup="StaticValue" ImageUrl="/ecn.images/images/icon-add.gif" />
                    </td>
                </tr>

                <asp:Panel ID="pnlCaptureNonStatic" Visible="false" runat="server">
                    <tr>
                        <td style="text-align: right;">Label:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCaptureLabel" runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlCaptureStatic" Visible="true" runat="server">
                    <tr>
                        <td style="text-align: right;">Static value:
                        </td>
                        <td style="text-align: left;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtStaticValue" runat="server" />&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvStaticValue" ControlToValidate="txtStaticValue" ErrorMessage="*" runat="server" ValidationGroup="StaticValue" Display="Dynamic" />
                                    </td>

                                </tr>
                            </table>


                        </td>

                    </tr>
                </asp:Panel>
                <tr>
                    <td colspan="2">
                        <div style="height: 100px; overflow: auto;">
                            <asp:GridView ID="gvCaptureValues" runat="server" Width="90%" AutoGenerateColumns="false" OnRowCommand="gvCaptureValues_RowCommand" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="IsStatic" HeaderText="Is Static" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Label" HeaderText="Label" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Field" HeaderText="UDF" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Value" HeaderText="Value" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDeleteCaptureValue" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.ID") %>' CommandName="deletecapturevalue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSaveCaptureFields" runat="server" Text="Save" OnClick="btnSaveCaptureFields_Click" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCancelCaptureFields" runat="server" Text="Cancel" OnClick="btnCancelCaptureFields_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="pnlSelectGroup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="background-color: white;">
                <tr>
                    <td>
                        <uc1:groupsLookup ID="groupExplorer" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnCloseGroupExplorer" Visible="false" runat="server" Text="Close" OnClick="btnCloseGroupExplorer_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="hfUpload" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeUploadControl" runat="server" PopupControlID="upUpload" BackgroundCssClass="modalBackground" TargetControlID="hfUpload" />
    <asp:UpdatePanel ID="upUpload" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="uploader" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="gatewayUpload" runat="server">
                <table style="background-color: white;">
                    <tr>
                        <td style="width: 50%;">
                            <ecn:uploader ID="uploader" runat="server" />

                        </td>
                        <td style="width: 50%;">
                            <asp:DataGrid ID="dgFiles" runat="server" Width="100%" OnItemDataBound="dgFiles_ItemDataBound" AutoGenerateColumns="false" CssClass="grid" Height="100%"
                                AllowPaging="true" PageSize="15">
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <PagerStyle Mode="NumericPages" HorizontalAlign="Right"></PagerStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="FileName" ItemStyle-Width="80%" HeaderText="File" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                    <asp:TemplateColumn ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="imgbtnDelete_Click" ImageUrl="/ecn.images/images/icon-cancel.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnSelect" runat="server" OnClick="imgbtnSelect_Click" ImageUrl="/ecn.images/images/icon-add.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center; vertical-align: top;">
                            <asp:Button ID="btnCancelUpload" runat="server" OnClick="btnCancelUpload_Click" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible; position: absolute; top: -500px"
        name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>
