<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.ReferralProgram" ValidateRequest="false"
    CodeBehind="ReferralProgram.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagName="radEditor" Src="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.ascx" TagPrefix="radEditor" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteSmartForm(theID) {
            if (confirm('Are you Sure?\n Selected smartForm permanently deleted.')) {
                window.location = "ReferralProgram.aspx?" + theID + "&action=delete";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <!-- Start Single Optin Panel-->
            <!-- RequiredFieldValidator is Turned OFF 'cos of the DO_SO_Button OnClick functionality wasn't working & it was poping the message that the fields are -->
            <!-- required instead of just redirecting to the DO_SO smartForm page. -->
            <asp:Panel ID="RP_panelFCKEditor" runat="Server" Visible="True">
                <tr>
                    <td class="tableHeader" colspan="2" align="center">
                        <!--Create New smartForms:&nbsp;&nbsp;-->
                        <asp:Button class="formbuttonsmall" ID="DO_SO_SmartFormButton" runat="Server" Text="Double Optin / Single Optin smartForms"
                            Enabled="false" OnClick="DO_SO_SmartFormButton_Click"></asp:Button>&nbsp;&nbsp;
                        <asp:Button class="formbuttonsmall" ID="RP_SmartFormButton" runat="Server" Text="Referral Program smartForm">
                        </asp:Button>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" width="782" colspan="2" height="24" align="left">
                        Referral Program
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" rowspan="3">
                        <asp:ListBox class="formtextfield" ID="RP_OptinFieldSelection" runat="Server" Visible="True"
                            SelectionMode="Multiple" Height="320px"></asp:ListBox>
                    </td>
                    <td class="tableHeader" valign="top" width="100%">
                        <table style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid; border-left: #c0c0c0 1px solid;
                            border-bottom: #c0c0c0 1px solid" cellspacing="1" cellpadding="1" width="100%"
                            align="left" border='0'>
                            <tr>
                                <td class="tableHeaderSmall" id="infoMsg" valign="middle" align="center" width="164"
                                    colspan="4" height="25">
                                    <font face="Verdana" color="orangered" size="2"><em>All Fields are Required</em></font>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="164" height="25">
                                    Program Name
                                </td>
                                <td colspan='3' height="25">
                                    <asp:TextBox class="formfield" ID="programName" runat="Server" Columns="41"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator id="requiredProgramName" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="programName">*</asp:RequiredFieldValidator>-->
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" valign="bottom" width="390" colspan="2" height="25">
                                    Referrer Response Information:
                                </td>
                                <td class="tableHeader" valign="bottom" width="50%" colspan="2" height="25">
                                    Referree Response Information:
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="164" height="20">
                                    Response From Email:
                                </td>
                                <td class="tableHeader" align="left" width="220" height="20">
                                    <asp:TextBox class="formfield" ID="RefererFromEmail" runat="Server" Columns="31"
                                        Width="208px"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator id="requiredFromEmail" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefererFromEmail">*</asp:RequiredFieldValidator>-->
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="164" height="20">
                                    Response Subject:
                                </td>
                                <td class="tableHeader" align="left" width="220" height="20">
                                    <asp:TextBox class="formfield" ID="RefererMsgSubject" runat="Server" Columns="31"
                                        Width="208px"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator id="requiredSubject" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefererMsgID">*</asp:RequiredFieldValidator>-->
                                </td>
                                <td class="tableHeaderSmall" valign="middle" height="20">
                                    Lead Subject:
                                </td>
                                <td class="tableHeader" height="20">
                                    <asp:TextBox class="formfield" ID="RefereeMsgSubject" runat="Server" Columns="31"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator id="requiredRefereeSubject" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefereeMsgSubject">*</asp:RequiredFieldValidator>-->
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="390" colspan="2" height="23">
                                    Response Message:
                                </td>
                                <td class="tableHeaderSmall" valign="middle" colspan="2" height="23">
                                    Response Message:
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" width="390" colspan="2" height="28">
                                    <asp:DropDownList class="formfield" ID="RefererMsgID" runat="Server">
                                    </asp:DropDownList>
                                    <!--<asp:RequiredFieldValidator id="requiredMsgID" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefererMsgID">*</asp:RequiredFieldValidator>-->
                                </td>
                                <td class="tableHeader" align='right' width="50%" colspan="2" height="24">
                                    <p align="left">
                                        <asp:DropDownList class="formfield" ID="RefereeMsgID" runat="Server">
                                        </asp:DropDownList>
                                        <!--<asp:RequiredFieldValidator id="requiredRefereeMsgID" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefereeMsgID">*</asp:RequiredFieldValidator>-->
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="164" height="20">
                                    Response Screen:
                                </td>
                                <td class="tableHeader" align='right' width="220" height="20">
                                    <p align="left">
                                        <asp:TextBox class="formfield" ID="RefererResponseScreen" runat="Server" Columns="31"
                                            Width="208px"></asp:TextBox>
                                        <!--<asp:RequiredFieldValidator id="requiredScreen" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="RefererResponseScreen">*</asp:RequiredFieldValidator>-->
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeaderSmall" valign="middle" width="164" height="20">
                                    Number of Referrees:
                                </td>
                                <td class="tableHeader" width="220" height="20">
                                    <asp:TextBox class="formfield" ID="SFFieldSet" runat="Server" Columns="5" Width="45px"></asp:TextBox>
                                    <!--<asp:RequiredFieldValidator id="requiredNumber" runat="Server" ErrorMessage="*" Font-Bold="True" ControlToValidate="SFFieldSet">*</asp:RequiredFieldValidator>-->
                                    <asp:RangeValidator ID="rangeValidatorNumber" runat="Server" ErrorMessage="Numbers Only!"
                                        ControlToValidate="SFFieldSet" Font-Italic="True" Font-Names="Verdana" Font-Size="8pt"
                                        MinimumValue="0" MaximumValue="10" Type="Integer">Numbers Only [1-10]</asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align='right' width="785" colspan="5" height="3">
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="center" width="100%" colspan="5">
                                    <asp:DataGrid ID="SmartFormGrid" runat="Server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                        Width="100%" CssClass="grid">
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                        <ItemStyle Height="17px"></ItemStyle>
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="ReferralProgramName" HeaderText="Referral Program Forms">
                                            </asp:BoundColumn>
                                            <asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Group / View Emails' border='0'&gt;"
                                                DataNavigateUrlField="ReferralProgramID" DataNavigateUrlFormatString="ReferralProgram.aspx?{0}">
                                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                            </asp:HyperLinkColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle Width="5%"></ItemStyle>
                                                <ItemTemplate>
                                                    <a href='javascript:deleteSmartForm("<%#DataBinder.Eval(Container.DataItem, "ReferralProgramID") %>");'>
                                                        <center>
                                                            <img src='/ecn.images/images/icon-delete1.gif' alt='Delete smartForm' border='0'></center>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <AU:PagerBuilder ID="GridPager" runat="Server" Width="100%" PageSize="3" ControlToPage="SmartFormGrid">
                                        <PagerStyle CssClass="gridpager"></PagerStyle>
                                    </AU:PagerBuilder>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" nowrap align="left" colspan="2" height="12">
                        Select the Fields you want to add to Single Optin smartForm
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" width="50%" colspan="2">
                        <asp:Button class="formbuttonsmall" ID="RP_RefreshHTML" runat="Server" Text="Rebuild smartForm"
                            OnClick="RefreshHTML_Click"></asp:Button>
                        <asp:Button class="formbuttonsmall" ID="RP_OptinHTMLSave" runat="Server" Text="Update smartForm"
                            Enabled="false" OnClick="SaveOptinHTML_Click"></asp:Button>
                        <asp:Button class="formbuttonsmall" ID="RP_OptinHTMLSaveNew" runat="Server" Text="Create New smartForm"
                            Enabled="true" OnClick="RP_OptinHTMLSaveNew_Click"></asp:Button>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td valign="bottom" align='right' width="782" colspan="2" height="240">
                        <%--<radEditor:radEditor ID="RP_HTMLCode" runat="server" />--%>
                        <CKEditor:CKEditorControl ID="RP_HTMLCode" runat="server" Skin="kama" Width="775"
                            Height="280" BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
                </tr>
                <tr>
                    <td id="panelTexBox" valign="top" align="left" width="782" colspan="2" height="100">
                        <asp:TextBox ID="TxtBx_HTMLCode" runat="Server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
            <!-- end DoubleOptin Panel-->
        </tbody>
    </table>
            <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
</asp:Content>
