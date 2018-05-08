<%@ Control Language="c#" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardGroup"
    CodeBehind="WizardGroup.ascx.cs" AutoEventWireup="true" %>
<%@ Register Src="~/main/ECNWizard/Group/groupExplorer.ascx" TagName="groupExplorer"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" />
<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>

<table cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td style="padding-left: 30px; padding-right: 30px" colspan="2">
         <uc1:groupExplorer ID="groupExplorer1" runat="server" />
        </td>
    </tr>
</table>