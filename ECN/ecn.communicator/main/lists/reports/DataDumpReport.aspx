<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="DataDumpReport.aspx.cs" Inherits="ecn.communicator.main.lists.reports.DataDumpReport" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    	 <style type='text/css'>
        .ui-datepicker-trigger { position: relative; vertical-align:middle; padding-left:5px; }
    </style>
	
	<script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtstartDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=txtendDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });
    </script>

    <asp:UpdatePanel ID="upMain" ChildrenAsTriggers="true" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>
            <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td colspan="4">
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
                                                        src="/ecn.images/images/errorEx.jpg"></td>
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
                            <br />
                        </asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr class="gradient">
                                <td width="50%" valign="middle" align="left" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Group Attribute Report
                                </td>
                                <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                    
                                    <asp:ListItem Value="xls" Selected="True">XLS</asp:ListItem>
                                    <asp:ListItem Value="xlsdata">XLSDATA</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" valign="top" colspan="2" align="left">
                                    <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                        style="margin: 10px 0px">
                                        <tr>
                                            <td width="30%" align='right'>
                                                <b>Group / List&nbsp;:&nbsp;</b>
                                                <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectGroup_Click" Visible="true" />
                                            </td>
                                            <td width="70%">


                                                <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                                <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'></td>
                                            <td>
                                                <b>Up to 1 year of statistics history is available.</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <b>Selected Groups</b>
                                            </td>
                                            <td>
                                                <asp:GridView ID="gvSelectedGroups" runat="server" GridLines="None" DataKeyNames="GroupID" OnRowCommand="gvSelectedGroups_RowCommand" OnRowDataBound="gvSelectedGroups_RowDataBound" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="GroupName" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CausesValidation="false" CommandName="deletegroup" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%" align='right'>
                                                <b>Start Date&nbsp;:&nbsp;</b>
                                            </td>
                                            <td width="70%">
                                                <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                                &nbsp;<asp:RequiredFieldValidator
                                                        ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                                        ErrorMessage="<< required" Font-Italic="True" ValidationGroup="Date" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                        <%--<asp:RangeValidator ID="rvStateDate" runat="server" ControlToValidate="txtstartDate"
                                            Font-Italic="True" Font-Bold="True" Font-Size="XX-Small"></asp:RangeValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'>
                                                <b>End Date&nbsp;:&nbsp;</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtendDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                                &nbsp;<asp:RequiredFieldValidator
                                                        ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                                        ErrorMessage="<< required" Font-Italic="True" ValidationGroup="Date" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                        <%--<asp:RangeValidator ID="rvEndDate" runat="server" ControlToValidate="txtendDate"
                                            Font-Italic="True" Font-Bold="True" Font-Size="XX-Small"></asp:RangeValidator>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <asp:Button ID="btnReport" runat="Server" Text="Show Report" ValidationGroup="Date" CssClass="formfield"
                                        OnClick="btnReport_Click"></asp:Button>
                                    <br>
                                    <br>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                                    </rsweb:ReportViewer>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="gradient" colspan="2">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
