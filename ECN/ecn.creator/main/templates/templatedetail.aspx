<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.creator.templatemanager.templateeditor" CodeBehind="templatedetail.aspx.cs" MasterPageFile="~/Creator.Master" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls.HtmlTextBox" Assembly="ActiveUp.WebControls.HtmlTextBox" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" align='right'>TemplateName</td>
                <td>
                    <asp:TextBox runat="Server" ID="TemplateName" Visible="True" EnableViewState="True"></asp:TextBox>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">&nbsp;HeaderCode&nbsp;</td>
                <td colspan="2">
                    <asp:TextBox ID="HeaderCode" runat="Server" Wrap="false" Columns="50" Rows="10" TextMode="multiline"></asp:TextBox>
                    <AU:Editor ID="HeaderSource" runat="Server" PersistText="true" EnableViewState="true"
                        Template="paragraph,bold,italic,underline,separator,image,link,table,rule,specialchars,codesnippets,separator,fontface,fontsize,fontcolor,separator,orderedlist,unorderedlist,alignleft,aligncenter,alignright,alignjustify,separator,strikethrough,superscript,subscript,separator,codecleaner"
                        TextareaColumns="72" TextareaRows="10" TextareaCssClass="formfield" Width="600"
                        Height="300">
                    </AU:Editor>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">&nbsp;SourceCode&nbsp;</td>
                <td colspan="2">
                    <asp:TextBox ID="SourceCode" runat="Server" Wrap="false" Columns="50" Rows="10" TextMode="multiline"></asp:TextBox>
                    <AU:Editor ID="FooterSource" runat="Server" PersistText="true" EnableViewState="true"
                        Template="paragraph,bold,italic,underline,separator,image,link,table,rule,specialchars,codesnippets,separator,fontface,fontsize,fontcolor,separator,orderedlist,unorderedlist,alignleft,aligncenter,alignright,alignjustify,separator,strikethrough,superscript,subscript,separator,FindAndReplace,codecleaner"
                        TextareaColumns="72" TextareaRows="10" TextareaCssClass="formfield" Width="600"
                        Height="300">
                    </AU:Editor>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="tableHeader" colspan='3' align="middle">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="TemplateID" runat="Server"></asp:TextBox>
                    <asp:Button ID="SaveButton" OnClick="CreateTemplate" Visible="true" Text="Create"
                        class="formbutton" runat="Server" />
                    <asp:Button ID="UpdateButton" OnClick="UpdateTemplate" Visible="false" Text="Update"
                        class="formbutton" runat="Server" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

