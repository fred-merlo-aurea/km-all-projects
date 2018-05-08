<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="OmnitureCustomerSetup.aspx.cs" Inherits="ecn.communicator.main.Omniture.OmnitureCustomerSetup" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlNoAccess">
        <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
            <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlContent">

        <table style="width: 100%;">
            <tr>
                <td class="greyOutSide offWhite center label">
                    <br />
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
                                                <img style="padding: 0 0 0 15px;"
                                                    src="http://images.ecn5.com/images/errorEx.jpg"></td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="errorBottom"></td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label ID="lblTitle" Text="OMNITURE TRACKING CUSTOMER SETUP" runat="server" Font-Bold="true" Font-Size="Medium" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <table style="margin-left: 20px; margin-top: 20px; width: 100%;">
                        <tr>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="chkboxOverride" Text="Override Base Channel" runat="server" Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                <asp:Label ID="lblQueryName" Text="Query String Name" Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtQueryName" Width="150px" runat="server" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbeQueryName" runat="server" TargetControlID="txtQueryName" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="/%&#\^!@*$?<>{}|+=-_~" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDelimiter" Text="Delimiter" Font-Size="Small" runat="server" />

                            </td>
                            <td>
                                <asp:TextBox ID="txtDelimiter" Width="150px" runat="server" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbeDelimiter" runat="server" TargetControlID="txtDelimiter" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="/?&#\^=" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="height: 100%; width: 100%; border: 1px solid gray;" cellpadding="5" cellspacing="0">
                        <tr>
                            <td style="width: 35%; text-align: left; height: 40px;">
                                <asp:Label ID="lblFields" Text="Fields" Font-Bold="true" Font-Size="Medium" runat="server" />
                            </td>
                            <td style="width: 35%; text-align: center;">
                                <asp:Label ID="lblDefault" Text="Default Option" Font-Bold="true" Font-Size="Small" runat="server" />
                            </td>

                            <td style="width: 15%; border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:Label ID="lblRequired" Text="Is Required" Font-Bold="true" Font-Size="Small" runat="server" />
                            </td>
                            <td style="width: 15%; text-align: center;">
                                <asp:Label ID="lblCustom" Text="Allow Custom" Font-Bold="true" Font-Size="Small" runat="server" />
                            </td>

                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture1" Text="Omniture1" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni1" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault1" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni1" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni1" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture2" Text="Omniture2" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni2" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault2" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni2" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni2" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture3" Text="Omniture3" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni3" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault3" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni3" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni3" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture4" Text="Omniture4" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni4" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault4" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni4" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni4" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture5" Text="Omniture5" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni5" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault5" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni5" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni5" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture6" Text="Omniture6" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni6" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault6" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni6" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni6" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture7" Text="Omniture7" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni7" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault7" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni7" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni7" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture8" Text="Omniture8" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni8" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />

                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault8" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni8" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni8" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture9" Text="Omniture9" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni9" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault9" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni9" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni9" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 40px;">
                                <asp:Label ID="lblOmniture10" Text="Omniture10" runat="server" />
                                <asp:ImageButton ID="imgbtnOmni10" ImageUrl="~/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmniEdit_Click" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOmniDefault10" runat="server" Width="100%" AutoPostBack="false" />
                            </td>

                            <td style="border-right: 1px solid gray; border-left: 1px solid gray; text-align: center;">
                                <asp:RadioButtonList ID="rblReqOmni10" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="text-align: center;">
                                <asp:RadioButtonList ID="rblCustomOmni10" RepeatDirection="Horizontal" Width="100%" runat="server">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnSaveSettings" CssClass="formbutton" runat="server" Text="Save" OnClick="btnSaveSettings_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="pnlEditOmniture" ChildrenAsTriggers="true" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnOmniEditSave" />
                <asp:PostBackTrigger ControlID="btnOmniEditCancel" />
            </Triggers>
            <ContentTemplate>
                <table style="background-color: white;">
                    <tr>
                        <td>Field Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtOmniDisplayName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Parameter Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtOmniParamName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Parameter Value
                        </td>
                        <td>

                            <asp:TextBox ID="txtOmniParamOption" runat="server"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbeOmniParamOption" TargetControlID="txtOmniParamOption" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="/%&#\^!@*$?<>{}|+=-_~" runat="server" />
                            &nbsp;&nbsp;
               <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add Value' border='0'&gt;"
                   CausesValidation="false" ID="btnAddParamOption" OnClick="btnAddParamOption_Click"></asp:LinkButton>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label ID="lblEditError" runat="server" ForeColor="Red" Visible="false" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <div style="overflow: auto; height: 100px;">
                                <ecnCustom:ecnGridView ID="gvOmniParamOptions" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                    Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="LTPOID" CssClass="grid"
                                    OnRowCommand="gvOmniParamOptions_RowCommand">
                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true" HeaderText="Field Names" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParamName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DisplayName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true" HeaderText="Field Values" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Value") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                    CommandName="ValueDelete" OnClientClick="return confirm('Are you sure, you want to delete this value?')"
                                                    CausesValidation="false" CommandArgument='<%#Eval("LTPOID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </ecnCustom:ecnGridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td>Dynamic Fields
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chklstDynamicFields" RepeatDirection="Horizontal" RepeatColumns="2" runat="server">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnOmniEditSave" Text="Save" OnClick="btnOmniEditSave_Click" runat="server" />
                                    </td>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnOmniEditCancel" Text="Cancel" CausesValidation="false" UseSubmitBehavior="true" runat="server" />
                                    </td>
                                </tr>
                            </table>

                        </td>

                    </tr>
                </table>
            </ContentTemplate>


        </asp:UpdatePanel>
        <asp:Button ID="hfOmniEdit" style="display:none;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="modalPopupOmnitureConfig" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlEditOmniture" CancelControlID="btnOmniEditCancel" TargetControlID="hfOmniEdit">
        </ajaxToolkit:ModalPopupExtender>
        <KM:Message ID="kmMsg" runat="server" />

         <asp:Button ID="hfTemplateNotif" Style="display: none;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="mpeTemplateNotif" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="upTemplateNotif" CancelControlID="btnCancelTemplate" TargetControlID="hfTemplateNotif" />
        <asp:UpdatePanel ID="upTemplateNotif" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnConfirmTemplate" />
            </Triggers>
            <ContentTemplate>
                <div style="padding:10px;background-color: white;border-radius: 5px;">
                <table style="background-color: white;max-width:400px;font-size:12px;font-family:Arial;">
                    <tr>
                        <td valign="top" align="center" width="20%">
                            <img style="padding: 0 0 0 15px;" src="/ecn.images/images/warningEx.jpg">
                        </td>
                        <td valign="middle" align="left" width="80%" height="100%">
                            <asp:Label ID="lblTemplateMessage" runat="Server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <table style="width:100%;padding-top:10px;" >
                                <tr>
                                    <td style="width:50%;text-align:center;">
                                        <asp:Button ID="btnConfirmTemplate" OnClick="btnSaveSettings_Click" runat="server" Text="Ok" />
                                    </td>
                                    <td style="width:50%;text-align:center;">
                                        <asp:Button ID="btnCancelTemplate" runat="server" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>

                        </td>

                    </tr>
                </table>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
