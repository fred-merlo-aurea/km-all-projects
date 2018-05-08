<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls.HtmlTextBox" Assembly="ActiveUp.WebControls.HtmlTextBox" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.creator.pages.contentdetail" CodeBehind="contentdetail.aspx.cs" MasterPageFile="~/Creator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000)
    </script>
    <style type="text/css">

    .cke_source 
    {
        white-space: pre-wrap !important;
    }
    </style>
    <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" valign="top">&nbsp;Content</td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>&nbsp;Title&nbsp;</td>
                <td colspan="2">
                    <asp:TextBox EnableViewState="true" ID="ContentTitle" runat="Server" size="50" CssClass="formfield"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="Server" ID="val_ContentTitle" ControlToValidate="ContentTitle"
                        ErrorMessage="ContentTitle is a required field." CssClass="errormsg" Display="Static"><-- Required</asp:RequiredFieldValidator>
                    <asp:Button ID="CreateAsNewTopButton" OnClick="CreateAsNewInitialize" runat="Server"
                        Text="Create as new Content" class="formbuttonsmall" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>&nbsp;Folder&nbsp;</td>
                <td colspan="2">
                    <asp:DropDownList EnableViewState="true" ID="folderID" runat="Server" DataValueField="FolderID"
                        DataTextField="FolderName" CssClass="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr size="1" color="#000000">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">&nbsp;&nbsp;</td>
                <td colspan="2" class="tableContent">
                    <asp:Panel ID="panelContentSource" runat="Server" Visible="true">
                        <%--<telerik:RadEditor ID="ContentSource" runat="server" Height="450px" Width="700px" EditModes="Html,Design,Preview" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" />--%>
                         <CKEditor:CKEditorControl ID="ContentSource" runat="server" Skin="kama" height="450" width="775"  BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
                    </asp:Panel>
                    <asp:Panel ID="panelContentURL" runat="Server" Visible="true">
                        URL: 
                        <asp:TextBox ID="ContentURL" runat="Server" EnableViewState="true" Columns="55"></asp:TextBox>
                    </asp:Panel>
                    <asp:Panel ID="panelContentFilePointer" runat="Server" Visible="false">
                        Path: 
                        <asp:TextBox ID="ContentFilePointer" runat="Server" EnableViewState="true" Columns="55"></asp:TextBox>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr size="1" color="#000000">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>&nbsp;Owner&nbsp;</td>
                <td colspan="2">
                    <asp:DropDownList EnableViewState="true" ID="UserID" runat="Server" DataValueField="UserID"
                        DataTextField="UserName" CssClass="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>&nbsp;Locked&nbsp;</td>
                <td colspan="2">
                    <asp:CheckBox EnableViewState="true" ID="LockedFlag" runat="Server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan='3' align="center">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="ContentID" runat="Server"></asp:TextBox>
                    <asp:Button ID="SaveButton" OnClick="CreateContent" Visible="true" Text="Create"
                        class="formbutton" runat="Server" />
                    <asp:Button ID="UpdateButton" OnClick="UpdateContent" Visible="false" Text="Update"
                        class="formbutton" runat="Server" />
                    <!--<asp:Button Id="CreateAsNewDownButton"  Visible="false" Text="Create As New" class="formbutton" runat="Server" />-->
                    <!-- if you make this active add the onClick event -->
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

