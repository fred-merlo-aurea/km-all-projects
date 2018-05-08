<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true"
    CodeBehind="userdetail.aspx.cs" Inherits="ecn.accounts.usersmanager.userdetail" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register TagPrefix="vs" Namespace="Vladsm.Web.UI.WebControls" Assembly="GroupRadioButton" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagName="MSGroupExplorer" Src="~/includes/MultiSelectGroupExplorer.ascx" TagPrefix="ecn2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function validateDefaultCheckBoxes() {
            alert("Default User Group Access is SET / CHANGED for this user.\nAnything done by this user will be defaulted to the User Group that is assigned now. ");
        }
    </script>

    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_Controls.css" type="text/css" />
    <style type="text/css">
        .aspBtn {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
            background-color: #ededed;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            color: black;
            font-family: arial;
            font-size: 9px;
            font-weight: bold;
            padding: 2px 10px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }

            .aspBtn:hover {
                background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
                background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
                background-color: #dfdfdf;
            }

            .aspBtn:active {
                position: relative;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="5" Visible="true"
        AssociatedUpdatePanelID="upMain" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="http://images.ecn5.com/images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
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
                                        <asp:Label ID="lblErrorMessagePhError" runat="Server"></asp:Label>
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

            <asp:Label ID="lblErrorMessage" runat="Server" Visible="false" CssClass="errormsg"></asp:Label>


            <table id="layoutWrapper" cellspacing="0" cellpadding="1" width="100%" border="0"
                style="padding-left: 20px; padding-right: 20px; padding-top: 10px; padding-bottom: 20px;">
                <tbody>
                    <tr>
                        <asp:Label ID="lblcontentTitle" runat="server" Text="" />
                    </tr>
                    <tr>
                        <td class="tableHeader capitalize" colspan="4" align="left">&nbsp;User Details
                        </td>
                    </tr>
                    <tr>
                        <td align='right' width="15%" class="label">Email Address&nbsp;
                        </td>
                        <td align="left" width="30%" class="label">
                            <asp:TextBox ID="txtEmailAddress" class="formfield" runat="Server" Columns="50" TabIndex="1"
                                Width="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="val_txtEmailAddress" runat="Server"
                                CssClass="errormsg" Display="Static" ErrorMessage="Required"
                                ControlToValidate="txtEmailAddress"></asp:RequiredFieldValidator>
                        </td>
                        <td class="label" align='right' width="15%">User Name&nbsp;
                        </td>
                        <td align="left" width="50%" class="label">
                            <asp:TextBox ID="txtUserName" runat="server" class="formfield" TabIndex="5" />
                            <asp:RequiredFieldValidator ID="val_txtUserName" runat="Server"
                                CssClass="errormsg" Display="Static" ErrorMessage="Required"
                                ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="val_txtUserNameSyntax" runat="server" ControlToValidate="txtUserName" CssClass="errormsg"
                                ErrorMessage="Remove blank spaces" ValidationExpression="^[^\s].+[^\s]$" />
                        </td>
                    </tr>
                    <tr>
                        <td align='right' width="15%" class="label">First Name&nbsp;
                        </td>
                        <td align="left" width="30%" class="label">
                            <asp:TextBox ID="txtFirstName" class="formfield" runat="Server" Columns="30" TabIndex="2"
                                Width="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="val_txtFirstName" runat="Server"
                                CssClass="errormsg" Display="Static" ErrorMessage="Required"
                                ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                        </td>
                        <td class="label" align='right' width="10%">Password&nbsp;
                        </td>
                        <td align="left" width="45%" class="label">
                            <asp:TextBox ID="txtPassword" runat="server" Enabled="false" CssClass="formfield" />
                            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" />
                            <asp:Label ID="lblEmailSent" runat="server" Text="Email Sent" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right'>Last Name&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLastName" runat="server" class="formfield" TabIndex="3" Columns="30" Width="200" />
                            <asp:RequiredFieldValidator ID="val_txtLastName" runat="Server"
                                CssClass="errormsg" Display="Static" ErrorMessage="Required"
                                ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                        </td>
                        <td class="label" align="right">Access Key&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="txtAccessKey" class="label" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right'>Phone&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPhone" runat="server" class="formfield" Columns="30" TabIndex="4" Width="200" />

                        </td>
                        <td class="label" align="right">User Status&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Active" Value="active" Selected="True" />
                                <asp:ListItem Text="Disabled" Value="disabled" />
                                <asp:ListItem Text="Locked" Value="locked" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trKMStaff" runat="server">
                        <td></td>
                        <td></td>
                        <td class="label" align="right">Is KM Staff?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblKMStaff" RepeatDirection="Horizontal" runat="server">
                                <asp:ListItem Text="Yes" Value="yes" />
                                <asp:ListItem Text="No" Value="no" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <%--<tr id="trSysAdmin" runat="server">
                        <td></td>
                        <td></td>
                        <td align="right" class="label">Is System Admin?&nbsp;
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblSysAdmin" runat="server" OnSelectedIndexChanged="rblSysAdmin_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="yes" />
                                <asp:ListItem Text="No" Value="no" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>

                    <tr>
                        <td class="tableHeader capitalize" align="left" colspan="3" style="border-bottom: #000000 1px solid">
                            <br />
                            &nbsp;User Roles&nbsp;
                        </td>

                        <td colspan="2" style="border-bottom: #000000 1px solid; text-align: center;">
                            <asp:Button ID="btnAddRole" runat="server" CausesValidation="true" OnClick="btnAddRole_Click" Text="Add New Role" />

                        </td>

                    </tr>
                    <tr>
                        <td class="label" align='center' valign="top" style="padding-top: 10px" colspan="4">
                            <asp:GridView ID="gvUserRoles" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvUserRoles_RowDataBound" OnRowCommand="gvUserRoles_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="BaseChannel" HeaderText="BaseChannel" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Customer" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" />
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID") %>' />
                                            <asp:HiddenField ID="hfBaseChannel" runat="server" Value='<%# Eval("BaseChannel") %>' />
                                            <asp:HiddenField ID="hfBaseChannelID" runat="server" Value='<%# Eval("BaseChannelID") %>' />
                                            <asp:HiddenField ID="hfCustomer" runat="server" Value='<%# Eval("Customer") %>' />
                                            <asp:HiddenField ID="hfCustomerID" runat="server" Value='<%# Eval("CustomerID") %>' />
                                            <asp:HiddenField ID="hfRole" runat="server" Value='<%# Eval("Role") %>' />
                                            <asp:HiddenField ID="hfSecurityGroupID" runat="server" Value='<%# Eval("SecurityGroupID") %>' />
                                            <asp:HiddenField ID="hfInactiveReason" runat="server" Value='<%# Eval("InactiveReason") %>' />
                                            <asp:HiddenField ID="hfIsBCAdmin" runat="server" Value='<%# Eval("IsBCAdmin") %>'/>
                                            <asp:HiddenField ID="hfIsCAdmin" runat="server" Value='<%# Eval("IsCAdmin") %>'/>
                                            <asp:HiddenField ID="hfIsActive" runat="server" Value='<%# Eval("IsActive") %>'/>
                                            <asp:HiddenField ID="hfIsDeleted" runat="server" Value='<%# Eval("IsDeleted") %>'/>
                                            <asp:HiddenField ID="hfDisplay" runat="server" Value='<%# Eval("Display") %>'/>
                                            <asp:HiddenField ID="hfDoHardDelete" runat="server" Value='<%# Eval("DoHardDelete") %>'/>
                                            <asp:HiddenField ID="hfIsChannelRole" runat="server" Value='<%# Eval("IsChannelRole") %>'/>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleStatus" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Deactivate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deleterole" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Restrict Groups" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnRestrict" runat="server" CausesValidation="true" CommandName="restrict" ImageUrl="~/images/ecn-icon-gear-small.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" align="center" colspan="4">
                            <br />
                            <asp:TextBox ID="txtUserID" runat="Server" EnableViewState="true" Visible="false"></asp:TextBox>
                            <asp:Button class="formbutton" ID="btnSave" runat="Server" Text="Save" Visible="true" OnClick="Save"></asp:Button>
                            <br />
                            <p style="color:red;">Note: Clicking on the Save button will save your changes and send an email invitation to the user to accept the role changes.</p>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:Button ID="hfUserPerms" runat="server" style="display:none;"/>
            <ajax:ModalPopupExtender ID="mpeUserPerms" BackgroundCssClass="ECN-ModalBackground" TargetControlID="hfUserPerms" PopupControlID="pnlUserPermsP" runat="server"></ajax:ModalPopupExtender>
            <asp:Panel ID="pnlUserPermsP" Height="300" Width="400" CssClass="ECN-ModalPopup" runat="server">
                <asp:UpdatePanel ID="pnlUserPerms" UpdateMode="Conditional" runat="server">

                    <ContentTemplate>
                        <table style="background-color: white;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblUserPermsHeader" runat="server" Text="USER PERMISSIONS" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr id="trBaseChannel" runat="server">
                                <td style="vertical-align: top; text-align: right;">
                                    <asp:Label ID="lblBaseChannel" runat="server" Text="BaseChannel" />
                                </td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlBaseChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr id="trBaseChannelError" runat="server" style="text-align: left;" visible="false">
                                            <td>
                                                <asp:Label ID="lblBCRequired" runat="server" Text="Required" ForeColor="Red" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblIsBCAdmin" runat="server" Text="Is BaseChannel Admin?" />
                                                <asp:RadioButtonList ID="rblIsBCAdmin" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblIsBCAdmin_SelectedIndexChanged" runat="server">
                                                    <asp:ListItem Text="Yes" Value="yes" />
                                                    <asp:ListItem Text="No" Value="no" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="trIsBCRole" runat="server" style="text-align:left;" visible="true">
                                            <td>
                                                <asp:Label ID="lblIsBCRole" runat="server" Text="Is BaseChannel Role?" />
                                                <asp:RadioButtonList ID="rblBCRoles" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblBCRoles_SelectedIndexChanged">
                                                    <asp:ListItem Text="Yes" Value="yes" />
                                                    <asp:ListItem Text="No" Value="no" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        

                                    </table>

                                </td>
                            </tr>
                            <tr id="trCustomer" runat="server">
                                <td style="vertical-align: top; text-align: right;">
                                    <asp:Label ID="lblCustomer" runat="server" Text="Customer" />
                                </td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trCustomerError" runat="server" style="text-align: left;" visible="false">

                                            <td>
                                                <asp:Label ID="lblCustomerRequired" runat="server" Text="Required" ForeColor="Red" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCADMIN" runat="server" Text="Is Customer Admin?" />
                                                <asp:RadioButtonList ID="rblIsCAdmin" runat="server" OnSelectedIndexChanged="rblIsCAdmin_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="yes" />
                                                    <asp:ListItem Text="No" Value="no" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                            <tr id="trRole" runat="server">
                                <td style="text-align: right;">
                                    <asp:Label ID="lblRole" runat="server" Text="Role" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRole" Width="90%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trBCRole" runat="server">
                                <td style="text-align:right;">
                                    <asp:Label ID="lblBCRole" runat="server" Text="Role" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBCRoles" Width="90%" runat="server" /><br />
                                    <asp:Label ID="lblBCRoleError" runat="server" Text="Required" Visible="false" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="text-align: center; width: 50%;">
                                                <asp:Button ID="btnSaveRole" runat="server" Text="OK" CausesValidation="false" OnClick="btnSaveRole_Click" />
                                            </td>
                                            <td style="text-align: center; width: 50%;">
                                                <asp:Button ID="btnCancelAddRole" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancelAddRole_Click" />
                                            </td>
                                        </tr>

                                    </table>

                                </td>

                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Button ID="hfRestrictGroups" runat="server" style="display:none;" />
            <ajax:ModalPopupExtender ID="mpeRestrictGroups" runat="server" TargetControlID="hfRestrictGroups" BackgroundCssClass="ECN-ModalBackground" PopupControlID="pnlRestrictGroupsP" />
            <asp:Panel ID="pnlRestrictGroupsP" Width="800px" BorderStyle="Solid" BorderColor="Gray" BorderWidth="2px" runat="server">
                <asp:UpdatePanel ID="pnlRestrictGroups" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table style="background-color: white; width: 100%; height: 100%;">
                            <tr>
                                <td colspan="2">
                                    <ecn2:MSGroupExplorer ID="groupExplorer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSaveRestrictGroups" runat="server" Text="OK" OnClick="btnSaveRestrictGroups_Click" CausesValidation="false" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCancelRestrictGroups" runat="server" OnClick="btnCancelRestrictGroups_Click" CausesValidation="false" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
