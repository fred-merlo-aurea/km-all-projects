<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.creator.pagesmanager.eventseditor" CodeBehind="eventdetail.aspx.cs" MasterPageFile="~/Creator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
    <script language="C#" runat="Server">

    </script>
    <style type="text/css">

    .cke_source 
    {
        white-space: pre-wrap !important;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="808" border='0'
            height="96">
            <tbody>
                <tr>
                    <td class="tableHeader" align='right' width="13%" height="37">Event Type</td>
                    <td width="432" height="37" colspan="2">
                        <asp:DropDownList runat="Server" ID="EventTypeCode" Visible="true" EnableViewState="True"
                            CssClass='formfield' AutoPostBack="True" OnSelectedIndexChanged="EventTypeCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align='right' width="13%" height="30">Display on&nbsp;<br />
                        Site&nbsp;</td>
                    <td width="432" height="30" colspan="2">
                        <asp:CheckBox ID="DisplayFlag" runat="Server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align='right' width="13%" height="27">&nbsp;EventName&nbsp;</td>
                    <td width="432" height="27" colspan="2">
                        <asp:TextBox EnableViewState="true" ID="EventName" runat="Server" size="50" CssClass='formfield'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="val_EventName" runat="Server" ErrorMessage="Event Name is Required."
                            CssClass="errormsg" ControlToValidate="EventName"><-- Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </tbody>
        </table>
        <p>
            <asp:Panel ID="specialPanel" runat="Server" Width="816px" Height="256px">
                <table id="Table1" height="243" cellspacing="1" cellpadding="1" width="744" border='0'>
                    <tr>
                        <td class="tableHeader" align='right' width="99" colspan="1" height="33" rowspan="1">Timing</td>
                        <td colspan='3' height="33">
                            <asp:TextBox ID="Times" runat="Server" CssClass="formfield" Size="50"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tableHeader" align='right' width="99" height="31">Location</td>
                        <td colspan='3' height="31">
                            <asp:TextBox ID="Location" runat="Server" CssClass="formfield" Size="50"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tableHeader" align='right' width="99">Dates</td>
                        <td class="tableHeader" valign="middle" align="center" width="214">
                            <p>From</p>
                            <p>
                                <asp:Calendar ID="StartDate" runat="Server" CssClass="formfield" BackColor="Transparent" Width="200px"
                                    Font-Size="6.5pt" PrevMonthText="«" NextMonthText="»" DayHeaderStyle-BackColor="#FECE2E" BorderColor="#000000"
                                    CellPadding="1">
                                    <DayHeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Cornsilk"></DayHeaderStyle>
                                    <TitleStyle Font-Bold="True"></TitleStyle>
                                    <OtherMonthDayStyle ForeColor="DarkKhaki"></OtherMonthDayStyle>
                                </asp:Calendar>
                            </p>
                        </td>
                        <td class="tableHeader" valign="middle" align="center">
                            <p>To</p>
                            <p>
                                <asp:Calendar ID="EndDate" runat="Server" CssClass="formfield" Width="200px" Font-Size="6.5pt"
                                    PrevMonthText="«" NextMonthText="»" DayHeaderStyle-BackColor="#FECE2E" BorderColor="#000000"
                                    CellPadding="1">
                                    <TodayDayStyle BorderStyle="None"></TodayDayStyle>
                                    <DayHeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Cornsilk"></DayHeaderStyle>
                                    <TitleStyle Font-Bold="True"></TitleStyle>
                                    <OtherMonthDayStyle ForeColor="DarkKhaki"></OtherMonthDayStyle>
                                </asp:Calendar>
                            </p>
                        </td>
                        <td valign="middle" align="center">
                            <asp:Label ID="val_FromToDates" runat="Server" CssClass="errormsg">From Date Precedes To Date</asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </p>
        <p>
            <table id="Table2" height="371" cellspacing="1" cellpadding="1" width="752" border='0'>
                <tr>
                    <td class="tableHeader" width="572" height="32">
                        <p align="justify">
                            Description
                        </p>
                    </td>
                    <td height="32"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--<telerik:RadEditor ID="Description" runat="server" Height="450px" Width="700px" EditModes="Html,Design,Preview" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" />--%>
                        <CKEditor:CKEditorControl ID="Description" runat="server" Skin="kama" BasePath="/ecn.editor/ckeditor/"     Width="775" Height="250"></CKEditor:CKEditorControl>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" colspan="2">&nbsp;
                    <hr color="#000000" size="1">
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="center" colspan="2">
                        <asp:TextBox ID="EventID" runat="Server" EnableViewState="true" Visible="false"></asp:TextBox>
                        <asp:Button class="formbutton" ID="SaveButton" OnClick="CreateEvent" runat="Server"
                            Visible="true" Text="Create"></asp:Button>
                        <asp:Button class="formbutton" ID="UpdateButton" OnClick="UpdateEvent" runat="Server"
                            Visible="false" Text="Update"></asp:Button>
                    </td>
                </tr>
            </table>
        </p>
        <p>
            &nbsp;
        </p>
</asp:Content>


