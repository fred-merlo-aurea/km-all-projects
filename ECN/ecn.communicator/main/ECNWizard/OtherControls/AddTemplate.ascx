<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddTemplate.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.AddTemplate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer" TagPrefix="ecn" %>
<%@ Register Src="~/main/ECNWizard/Group/filtergrid.ascx" TagName="filterGrid" TagPrefix="uc1" %>
<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .modalPopupFull {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
        overflow: auto;
    }

    .modalPopupLayoutExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .modalPopupGroupExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .modalPopupImport {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 60%;
        overflow: auto;
    }

    .buttonMedium {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }

    .TransparentGrayBackground {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }

    fieldset {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }

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

    fieldset {
    margin: 0.5em 0px;
    padding: 0.0px 0.5em 0px 0.5em;
    border: 1px solid #ccc;
    -webkit-border-radius: 8px;
    -moz-border-radius: 8px;
    border-radius: 8px;
}

    fieldset p {
        margin: 2px 12px 10px 10px;
    }

    fieldset.login label, fieldset.register label, fieldset.changePassword label {
        display: block;
    }

    fieldset label.inline {
        display: inline;
    }

legend {
    font-size: 18px;
    font-weight: 600;
    padding: 2px 4px 8px 4px;
    font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    color: #000;
}
</style>
<br />
<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="false">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
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
<asp:UpdatePanel ID="upMain" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div style="text-align: left;">
            <asp:Label ID="lblCampaignItemTemplate" runat="server" Text="Add Campaign Item Template" Font-Bold="true" Font-Size="Large"></asp:Label>
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
            <table style="width:100%; border:0px">
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Campaign Item Template Name</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Name" Font-Size="Medium" style="margin-right:10px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTemplateName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Campaign Name</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblselectcamp" runat="server" Text="Select Campaign" Font-Size="Medium" style="margin-right:10px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpdownCampaign" Height="22px" width="350px" runat="server" CssClass="styled-select" >
                                            
                                       </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Groups</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px; width: 100%;">
                                <tr valign="top" align="left">
                                    <td style="width:33%;">
                                        <asp:Label ID="Label1" runat="server" Text="Group" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td style="width:33%;">
                                        <asp:Label ID="Label6" runat="server" Text="Suppression Group" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td style="width:33%;">&nbsp;&nbsp;</td>
                                </tr>
                                <tr valign="top" align="left">
                                    <td style="width:33%;">
                                        <asp:Panel ID="pnlGroups" runat="server">
                                            <table style="padding-left: 30px; width: 280px;">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkSelectGroup" runat="server" OnClick="lnkSelectGroup_Click">Click Here</asp:LinkButton>
                                                        to Select Groups
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSelectGroupName" runat="server" Text="No Groups Selected"></asp:Label>
                                                        <asp:GridView ID="gvSelectedGroups" runat="server" OnRowDataBound="gvSelectedGroups_RowDataBound" OnRowCommand="gvSelectedGroups_RowCommand" 
                                                            GridLines="None" AllowSorting="false" AutoGenerateColumns="false" Width="100%" AllowPaging="false" ShowFooter="false" ShowHeader="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="GroupID" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGroupName" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" OnClientClick="return confirm('Are you sure you want to delete this Group from the list?')" 
                                                                            ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegroup" />
                                                                        </td>
                                                                        </tr>
                                                                        <tr style="border-bottom: 1px solid gray;">
                                                                            <td colspan="5">
                                                                                <uc1:filterGrid ID="fgGroupFilterGrid" SuppressOrSelect="select" IsTestBlast="false" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width:33%;">
                                        <asp:Panel ID="pnlSuppression" runat="server">
                                            <table style="padding-left: 30px; width: 280px;">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkSelectSuppressionGroup" runat="server" OnClick="lnkSelectSuppressionGroup_Click">Click Here</asp:LinkButton>
                                                        to Select Groups for Suppression
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSuppressionGroupName" runat="server" Text="No Groups Selected"></asp:Label>
                                                        <asp:GridView ID="gvSuppressionGroup" runat="server" OnRowDataBound="gvSupression_RowDataBound" OnRowCommand="gvSuppressionGroup_RowCommand" 
                                                            GridLines="None" AllowSorting="false" AutoGenerateColumns="false" Width="100%" AllowPaging="false" ShowFooter="false" ShowHeader="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="GroupID" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGroupName" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" OnClientClick="return confirm('Are you sure you want to delete this Group from the list?')" 
                                                                            ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegroup" />
                                                                        </td>
                                                                        </tr>
                                                                        <tr style="border-bottom: 1px solid gray;">
                                                                            <td colspan="5">
                                                                                <uc1:filterGrid ID="fgSuppressionFilterGrid" SuppressOrSelect="suppress" IsTestBlast="false" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width:33%;">&nbsp;&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Message</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr>
                                    <td class="formLabel">
                                        <asp:ImageButton ID="imgSelectLayoutA" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutTrigger_Click" Visible="true" />
                                        <asp:Label ID="lblSelectedLayoutTrigger" runat="server" Text="-No Message Selected-" Font-Size="11px" />
                                        <asp:ImageButton ID="imgCleanSelectedLayout" runat="server" OnClientClick="return confirm('Are you sure you want to delete this Message?')" 
                                            ImageUrl="/ecn.images/images/icon-delete1.gif" CausesValidation="false" OnClick="imgCleanSelectedLayout_Click" Visible="false" />
                                        <asp:HiddenField ID="hfSelectedLayoutTrigger" runat="server" Value="" />
                                        <asp:HiddenField ID="hfWhichLayout" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Envelope Information</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Envelope" Font-Size="Medium"></asp:Label><br />
                                        <br />
                                        <table>                                            
                                            <tr>
                                                <td>From Email
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromEmail" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Reply To
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReplyTo" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>From Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromName" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Subject
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Fields Information</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px; width: 100%;">
                                <tr valign="top" align="left">
                                    <td style="width:33%;">
                                        <asp:Panel ID="pnlBlastFields" runat="server">
                                            <asp:Label ID="Label3" runat="server" Text="Blast Fields" Font-Size="Medium"></asp:Label><br />
                                            <br />
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBlastField1" runat="server" Text="Field1"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBlastField1" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBlastField2" runat="server" Text="Field2"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBlastField2" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBlastField3" runat="server" Text="Field3"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBlastField3" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBlastField4" runat="server" Text="Field4"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBlastField4" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBlastField5" runat="server" Text="Field5"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBlastField5" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>                                    
                                    <td style="width:33%;">
                                        <asp:Panel ID="pnlOmniture" runat="server" Width="100%" Visible="false">
                                            <asp:Label ID="lblOmniture" Text="Omniture Fields" Font-Size="Medium" runat="server" />
                                            <br />
                                            <br />
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture1" Text="Omniture1" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture1" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture2" Text="Omniture2" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture2" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture3" Text="Omniture3" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture3" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture4" Text="Omniture4" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture4" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture5" Text="Omniture5" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture5" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture6" Text="Omniture6" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture6" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture7" Text="Omniture7" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture7" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture8" Text="Omniture8" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture8" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture9" Text="Omniture9" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture9" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOmniture10" Text="Omniture10" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOmniture10" Height="22px" Width="150px" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width:33%;">&nbsp;&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Opt-out Preference</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px; width: 100%;">
                                <tr>
                                    <td style="width:33%;">
                                        <asp:CheckBox ID="chkOptOutMasterSuppression" runat="server" Text="Add to Master Suppression" OnCheckedChanged="chkOptOutMasterSuppression_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%;">
                                        <table>
                                            <tr valign="middle">
                                                <td>
                                                    <asp:CheckBox ID="chkOptOutSpecificGroup" runat="server" Text="Opt-out from Specific Groups" OnCheckedChanged="chkOptOutSpecificGroup_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td align="left"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="pnlOptOutSpecificGroups" runat="server" UpdateMode="Conditional" Visible="false">
                                                        <ContentTemplate>
                                                            <table style="padding-left: 30px">
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton ID="lnkSelectOptOutGroups" runat="server" OnClick="lnkSelectOptOutGroups_Click">Click Here</asp:LinkButton>
                                                                        to Select Groups for Opt-out
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="gvOptOutGroups" runat="server" OnRowDataBound="gvOptOutGroups_RowDataBound" OnRowCommand="gvOptOutGroups_RowCommand" 
                                                                            GridLines="None" AllowSorting="false" AutoGenerateColumns="false" Width="100%" AllowPaging="false" ShowFooter="false" ShowHeader="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="GroupID" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGroupName" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" OnClientClick="return confirm('Are you sure you want to delete this Group from the list?')" 
                                                                                            ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegroup" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>                    
                </tr>
            </table>
            <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />

            <asp:HiddenField ID="hfSuppressGroupID" runat="server" Value="0" />
        </div>
        <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
        <asp:Button ID="hfLayoutExplorer"  style="display:none;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="mpeLayoutExplorer" PopupControlID="pnlLayoutExplorer" BackgroundCssClass="modalBackground" TargetControlID="hfLayoutExplorer" runat="server" />
        <asp:UpdatePanel ID="pnlLayoutExplorer" Style="display:none;" CssClass="modalPopupLayoutExplorer" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
                <table style="background-color: white;">
                    <tr>
                        <td>
                            <ecn:layoutExplorer ID="layoutExplorer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnCloseLayoutExplorer" runat="server" OnClick="btnCloseLayoutExplorer_Click" Text="Close" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="hfAddFilter" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="mpeAddFilter" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlAddFilter" TargetControlID="hfAddFilter" />
<asp:Panel runat="server" ID="pnlAddFilter" Width="500px" Height="300px" CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="pnlFilterConfig" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upProgressFilterEditControl3" CssClass="overlay" runat="server">
                <asp:Panel ID="upProgressFilterEditControlP3" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="pnlFilterConfig" UpdateMode="Conditional" style="width: 100%; height: 100%;" runat="server">
        <ContentTemplate>
            <table style="background-color: white; width: 100%; max-width: 98%; height: 100%; max-height: 98%; margin: auto;">
                <tr>
                    <td>
                        <asp:Panel ID="pnlCustomFilter" Visible="false" Width="100%" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <span>Select Filter(s)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="height: 150px; overflow: auto;">
                                            <asp:ListBox ID="lbAvailableFilters" Width="100%" Height="100%" runat="server" SelectionMode="Multiple" AutoPostBack="false" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCancelFilter" runat="server" OnClick="btnCancelFilter_Click" Text="Cancel" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSaveFilter" runat="server" OnClick="btnSaveFilter_Click" CommandName="savefilter" Text="Save" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
