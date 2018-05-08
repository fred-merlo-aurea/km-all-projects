<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.creator.headerfootermanager.headerfootereditor" CodeBehind="headerfooterdetail.aspx.cs" MasterPageFile="~/Creator.Master" %>

<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
    <style type="text/css">

    .cke_source 
    {
    white-space: pre-wrap !important;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" align="left" colspan="4" bgcolor='#eeeeee' width="100%">
                    <cpanel:DataPanel ID="DataPanel1" runat="Server" ExpandImageUrl="../../../ecn.images/images/collapse2.gif"
                        CollapseImageUrl="../../../ecn.images/images/collapse2.gif" CollapseText="Click to hide Header Details"
                        ExpandText="Click to display Header Details" Collapsed="False" TitleText="View / Edit Header Details"
                        AllowTitleExpandCollapse="True">
                        <table width="100%" border='0' bgcolor="#FFFFFF">
                            <tr>
                                <td class="label" align='right' width="10%">HeaderFooter Name</td>
                                <td>
                                    <asp:TextBox runat="Server" ID="HeaderFooterName" Visible="True" EnableViewState="True"
                                        Columns="36" CssClass="formfield" />
                                    <asp:RequiredFieldValidator runat="Server" ID="val_HeaderFooterName" ControlToValidate="HeaderFooterName"
                                        ErrorMessage="HeaderFooter Name is Required" CssClass="errormsg" Display="static"><-- Required</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right'>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="PageHeaderStartTags" runat="Server" Wrap="false" Columns="106" Rows="3"
                                        TextMode="multiline" ReadOnly CssClass="formfield" />
                                </td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>SearchEngine Keywords</td>
                                <td>
                                    <asp:TextBox ID="Keywords" runat="Server" Wrap="false" Columns="106" Rows="3" TextMode="multiline"
                                        CssClass="formfield" />
                                </td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>Java Script</td>
                                <td>
                                    <asp:TextBox ID="JavaScriptCode" runat="Server" Wrap="false" Columns="106" Rows="5"
                                        TextMode="multiline" CssClass="formfield" />
                                </td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>Style Sheet</td>
                                <td>
                                    <asp:TextBox ID="StyleSheet" runat="Server" Wrap="false" Columns="106" Rows="5" TextMode="multiline"
                                        CssClass="formfield" />
                                </td>
                            </tr>
                        </table>
                    </cpanel:DataPanel>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan="4" height="5"></td>
            </tr>
            <tr>
                <td colspan="4" bgcolor='#eeeeee'>
                    <cpanel:DataPanel ID="Datapanel2" runat="Server" ExpandImageUrl="../../../ecn.images/images/collapse2.gif"
                        CollapseImageUrl="../../../ecn.images/images/collapse2.gif" CollapseText="Click to hide Header Code"
                        ExpandText="Click to display / Edit Header Code" Collapsed="False" TitleText="View / Edit Header Code"
                        AllowTitleExpandCollapse="True">
                        <table bgcolor="#FFFFFF" width="100%">
                            <tr>
                                <td class="label" align='right' valign="top">&nbsp;Header Code&nbsp;</td>
                                <td>
                                    <%--<telerik:RadEditor ID="HeaderCode" runat="server" Height="450px" Width="700px" EditModes="Html,Design,Preview" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" />
                                  --%>
                                      <CKEditor:CKEditorControl ID="HeaderCode" runat="server" Skin="kama"  Height="350" Width="775"  BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>--%>
                                    

                                </td>
                            </tr>
                        </table>
                    </cpanel:DataPanel>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan="4" height="5"></td>
            </tr>
            <tr>
                <td colspan="4" bgcolor='#eeeeee'>
                    <cpanel:DataPanel ID="Datapanel3" runat="Server" ExpandImageUrl="../../../ecn.images/images/collapse2.gif"
                        CollapseImageUrl="../../../ecn.images/images/collapse2.gif" CollapseText="Click to hide Footer Code"
                        ExpandText="Click to display / Edit Footer Code" Collapsed="False" TitleText="View / Edit Footer Code"
                        AllowTitleExpandCollapse="True">
                        <table bgcolor="#FFFFFF" width="100%" border='0'>
                            <tr>
                                <td class="label" align='right' valign="top">&nbsp;Footer Code&nbsp;</td>
                                <td>
                                <%--    <telerik:RadEditor ID="FooterCode" runat="server" Height="450px" Width="700px" EditModes="Html,Design,Preview" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" />
                                --%>
                                        <CKEditor:CKEditorControl ID="FooterCode"  Height="350" Width="775"  runat="server" Skin="kama" BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>

                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right' valign="top"></td>
                                <td class="tableHeader" align="left">
                                    <asp:TextBox ID="FooterEndTags" runat="Server" Wrap="false" Columns="106" Rows="3"
                                        TextMode="multiline" ReadOnly CssClass="formfield" />
                                </td>
                            </tr>
                        </table>
                    </cpanel:DataPanel>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan="4" height="5"></td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr size="1" color="#000000">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan="4" align="middle">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="HeaderFooterID" runat="Server"></asp:TextBox>
                    <asp:Button ID="SaveButton" OnClick="CreateHeaderFooter" Visible="true" Text="Create"
                        class="formbutton" runat="Server" />
                    <asp:Button ID="UpdateButton" OnClick="UpdateHeaderFooter" Visible="false" Text="Update"
                        class="formbutton" runat="Server" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

