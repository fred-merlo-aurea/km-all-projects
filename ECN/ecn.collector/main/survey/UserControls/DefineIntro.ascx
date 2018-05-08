
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Control Language="c#" Inherits="ecn.collector.main.survey.UserControls.DefineIntro" Codebehind="DefineIntro.ascx.cs" %>
 <style type="text/css">

    .cke_source 
    {
        white-space: pre-wrap !important;
    }
    </style>
<table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tr>
        <td class="headingTwo" style="padding-left: 20px;">
            Introduction Page HTML Editor:</td>
    </tr>
    <tr>
        <td align="center">
          <CKEditor:CKEditorControl ID="IntroHTML" runat="server" Skin="kama" BasePath="/ecn.editor/ckeditor/" Width="800" Height="350" ToolbarSet="Basic"></CKEditor:CKEditorControl>
        </td>
    </tr>
    <tr>
        <td class="headingTwo" style="padding: 20px 0 0 20px;">
            Thank You Page HTML Editor:</td>
    </tr>
    <tr>
        <td align="center">
          <CKEditor:CKEditorControl ID="ThankYouHTML" runat="server" Skin="kama" BasePath="/ecn.editor/ckeditor/" Width="800" Height="350" ToolbarSet="Basic"></CKEditor:CKEditorControl>
        </td>
    </tr>
</table>
