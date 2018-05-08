<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnsubscribeReasonReport.ascx.cs" Inherits="ecn.communicator.main.Reports.ReportSettingsControls.UnsubscribeReasonReport" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<table id="idMain" cellspacing="0" cellpadding="0" style="padding: 10px;" width="100%" align="center" border="0">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        Search By
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSearchBy" runat="server">
                            <asp:ListItem Text="--Select--" Value=""/>
                            <asp:ListItem Text="Group Name" Value="group"/>
                            <asp:ListItem Text="Email Subject" Value="emailsubject"/>
                            <asp:ListItem Text="Campaign Item" Value="campaignitem"/>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Search Text
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearchCriteria" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFTPURL" Text="FTP URL" runat="server" CssClass="label"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFTPURL" runat="server"/>
                        <asp:RegularExpressionValidator ID="revFTPURL" runat="server" ValidationExpression="^(ftp|ftps|sftp)://.+$" ControlToValidate="txtFTPURL" ErrorMessage="Invalid URL" ForeColor="Red"/>
                        <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtFTPURL" ErrorMessage="Required" />   
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFTPUsername" Text="Username" runat="server" CssClass="label"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFTPUsername" runat="server"/>
                        <asp:RequiredFieldValidator ID="rfvURLname" runat="server" ControlToValidate="txtFTPUsername" ErrorMessage="Required" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFTPPassword" Text="Password" runat="server" CssClass="label"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFTPPassword" runat="server"/>
                        <asp:RequiredFieldValidator ID="rfvURLpassword" runat="server" ControlToValidate="txtFTPPassword" ErrorMessage="Required" />
                    </td>
                </tr>

            </table>
            <br/>
            <br/>
        </td>
    </tr>


    <tr>
        <td style="width: 100%;">
            <ecnCustom:ecnGridView ID="gvReasonDetails" CssClass="gridWizard" Width="100%" runat="server" AutoGenerateColumns="false">
                <FooterStyle CssClass="tableHeader1"/>
                <AlternatingRowStyle CssClass="gridaltrowWizard"></AlternatingRowStyle>
                <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="CampaignItemName" ItemStyle-Width="20%" HeaderText="Campaign Item"/>
                    <asp:BoundField DataField="EmailSubject" ItemStyle-Width="20%" HeaderText="Email Subject"/>
                    <asp:BoundField DataField="GroupName" ItemStyle-Width="10%" HeaderText="Group"/>
                    <asp:BoundField DataField="EmailAddress" ItemStyle-Width="20%" HeaderText="Email"/>
                    <asp:BoundField DataField="UnsubscribeTime" ItemStyle-Width="10%" HeaderText="Time"/>
                    <asp:BoundField DataField="SelectedReason" ItemStyle-Width="20%" HeaderText="Reason"/>
                </Columns>
            </ecnCustom:ecnGridView>
            <rsweb:ReportViewer ID="report" Width="100%" Visible="false" runat="server"/>
        </td>
    </tr>
</table>