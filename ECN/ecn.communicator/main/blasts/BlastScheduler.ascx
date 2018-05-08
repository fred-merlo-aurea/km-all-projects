<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlastScheduler.ascx.cs"
    Inherits="ecn.communicator.main.blasts.BlastScheduler" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script language="javascript">
    function enabledtpicker() {
        $('#<%=txtStartTime.ClientID%>').timepicker({
            ampm: false,
            addSliderAccess: true,
            sliderAccessArgs: { touchonly: false },
            timeFormat: 'hh:mm:ss'
        });
        $('#<%=txtStartDate.ClientID%>').datepicker();
        $('#<%=txtEndDate.ClientID%>').datepicker();
    }
</script>
<style type="text/css">
    fieldset {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }

    .styled-select {
        width: 240px;
        background: transparent;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }

    .styled-text {
        width: 240px;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
</style>
<table id="TestWrapper" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlTestBlast">
    <tr>
        <td align="left" valign="middle" class="label" colspan="2">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 20%;">
                        <asp:Label ID="lblTestBlast" runat="server" Text="Is this a test blast?"></asp:Label>
                    </td>
                    <td align="left" width="15%" valign="middle" class="label">
                        <asp:RadioButtonList ID="rblTestBlast" runat="server" AutoPostBack="True" Width="150px"
                            OnSelectedIndexChanged="rblTestBlast_SelectedIndexChanged" RepeatDirection="Horizontal">
                            <asp:ListItem>Yes</asp:ListItem>
                            <asp:ListItem>No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width: 65%;">
                        <asp:Label ID="lblSocialMessage" Font-Size="Smaller" runat="server" Text="Results will not be generated for simple share and tracking features for test blasts" Visible="false" ForeColor="Blue" />
                    </td>
                </tr>
            </table>


        </td>

    </tr>

</asp:Panel>
        <asp:Panel ID="pnlTextBlast" runat="server" >
            
                <tr>
                    <td style="text-align: left; vertical-align: middle;" class="label" colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 20%;">
                                    <asp:Label ID="lblSendTextBlast" runat="server" Text="Also send Text version?" />
                                </td>
                                <td style="text-align: left; vertical-align: middle;" class="label">
                                    <asp:RadioButtonList ID="rblTextBlast" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" Width="150px">
                                        <asp:ListItem Value="true" Text="Yes" />
                                        <asp:ListItem Value="false" Text="No" Selected="True" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
        </asp:Panel>
<asp:Panel runat="server" ID="pnlEmailPreview">
    <tr>
        <td align="left" width="20%">&nbsp;
        </td>
        <td align="left" width="80%">
            <table id="Table11" cellspacing="0" cellpadding="2" width="100%" border="0">
                <tr>
                    <td align="left" width="20%">
                        <asp:CheckBox ID="cbEmailPreview" runat="server" AutoPostBack="false" Text="Email Preview"
                            ToolTip="An additional charge will be incurred for doing this."></asp:CheckBox>
                    </td>
                    <td align="left" width="80%">
                        <font color="#ff0000" size="1"><span style="padding-left: 8px; width: 350px">An additional
                                charge will be incurred for doing this</font>
                        <br />
                        <asp:Label ID="lbEmailPreviewResult" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</asp:Panel>
</table>
<table id="ScheduleWrapper" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlScheduleType">
        <tr>
            <td align="left" width="20%" valign="middle" class="label">
                <asp:Label ID="lblScheduleType" runat="server" Text="Schedule Type"></asp:Label>
            </td>
            <td align="left" width="80%" valign="middle" class="label">
                <asp:DropDownList ID="ddlScheduleType" runat="server" AutoPostBack="True" Width="300px"
                    class="styled-select" OnSelectedIndexChanged="ddlScheduleType_SelectedIndexChanged">
                    <asp:ListItem>--SELECT--</asp:ListItem>
                    <asp:ListItem>Send Now</asp:ListItem>
                    <asp:ListItem>Schedule One-Time</asp:ListItem>
                    <asp:ListItem>Schedule Recurring</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRecurrence">
        <tr>
            <td align="left" width="20%" valign="middle" class="label">
                <asp:Label ID="lblRecurrence" runat="server" Text="Recurrence"></asp:Label>
            </td>
            <td align="left" width="80%" valign="middle" class="label">
                <asp:DropDownList ID="ddlRecurrence" runat="server" AutoPostBack="True" Width="300px"
                    OnSelectedIndexChanged="ddlRecurrence_SelectedIndexChanged" class="styled-select">
                    <asp:ListItem Selected="True">Daily</asp:ListItem>
                    <asp:ListItem>Weekly</asp:ListItem>
                    <asp:ListItem>Monthly</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSplitType">
        <tr>
            <td align="left" width="20%" valign="middle" class="label">
                <asp:Label ID="lblSplitType" runat="server" Text="Split Type"></asp:Label>
            </td>
            <td align="left" width="80%" valign="middle" class="label">
                <asp:DropDownList ID="ddlSplitType" runat="server" AutoPostBack="True" Width="300px"
                    OnSelectedIndexChanged="ddlSplitType_SelectedIndexChanged" class="styled-select">
                    <asp:ListItem Selected="True">Single Day</asp:ListItem>
                    <asp:ListItem>Evenly Split</asp:ListItem>
                    <asp:ListItem>Manually Split</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table2" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlStart">
        <tr>
            <td align="left" width="20%" class="label"></td>
            <td align="left" width="80%">
                <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td align="left" valign="middle" width="10%" class="label">&nbsp;Start Date:
                        </td>
                        <td align="left" valign="middle" width="50%" class="label">
                            <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="false" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStartDate" ValidationGroup="formValidation" ControlToValidate="txtStartDate"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" valign="middle" width="10%" class="label">&nbsp;Time:
                        </td>
                        <td align="left" valign="middle" width="50%" class="label">
                            <asp:TextBox ID="txtStartTime" runat="server" class="styled-text"></asp:TextBox>
                            (CST)
                            <asp:RequiredFieldValidator ID="rfvStartTime" ValidationGroup="formValidation" ControlToValidate="txtStartTime"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEnd">
        <tr>
            <td align="left" width="20%" class="label"></td>
            <td align="left" width="80%">
                <table id="Table4" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td align="left" valign="middle" width="10%" class="label">&nbsp;End Date:
                        </td>
                        <td align="left" valign="middle" width="50%" class="label">
                            <asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="false" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEndDate" ValidationGroup="formValidation" ControlToValidate="txtEndDate"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table9" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlWeeks">
        <tr>
            <td align="left" valign="middle" width="20%" class="label">Every how many weeks&nbsp;
            </td>
            <td align="left" valign="middle" width="80%" class="label">
                <asp:TextBox ID="txtWeeks" runat="server" AutoPostBack="false" class="styled-text"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvWeeks" ValidationGroup="formValidation" ControlToValidate="txtWeeks"
                    runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvWeeks" runat="server" ValidationGroup="formValidation"
                    ControlToValidate="txtWeeks" MinimumValue="1" Type="Integer" MaximumValue="52"
                    CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table10" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlMonth">
        <tr>
            <td align="left" valign="middle" width="20%" class="label">What day of each month&nbsp;
            </td>
            <td align="left" valign="middle" width="80%" class="label">
                <asp:TextBox ID="txtMonth" runat="server" AutoPostBack="false" class="styled-text"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMonth" ValidationGroup="formValidation" ControlToValidate="txtMonth"
                    runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvMonth" runat="server" ValidationGroup="formValidation"
                    ControlToValidate="txtMonth" MinimumValue="1" Type="Integer" MaximumValue="31"
                    CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                <asp:CheckBox ID="cbLastDay" runat="server" AutoPostBack="True" OnCheckedChanged="cbLastDay_CheckedChanged"
                    Text="Last Day" />
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table5" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlNumberToSendType">
        <tr>
            <td align="left" valign="middle" width="20%" class="label">
                <asp:Label ID="lblNumToSendType" runat="server" Text="Number To Send Type"></asp:Label>
            </td>
            <td align="left" valign="middle" width="80%" class="label">
                <asp:DropDownList ID="ddlNumberToSendType" runat="server" Width="300px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlNumberToSendType_SelectedIndexChanged" class="styled-select">
                    <asp:ListItem Selected="True">ALL</asp:ListItem>
                    <asp:ListItem Selected="True">Percent</asp:ListItem>
                    <asp:ListItem>Number</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNumberToSend">
        <tr>
            <td align="left" valign="middle" width="20%" class="label">
                <asp:Label ID="lblNumberToSend" runat="server" Text="Number To Send"></asp:Label>
            </td>
            <td align="left" valign="middle" width="80%" class="label">
                <asp:TextBox ID="txtNumberToSend" runat="server" class="styled-text" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNumberToSend" ValidationGroup="formValidation"
                    ControlToValidate="txtNumberToSend" runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvNumberToSend" runat="server" ValidationGroup="formValidation"
                    ControlToValidate="txtNumberToSend" MinimumValue="1" Type="Integer" MaximumValue="1000000"
                    CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table6" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlDays" BorderStyle="None">
        <tr>
            <td align="left" valign="middle" width="20%" class="label">
                <asp:Label ID="lblDays" runat="server" Text="Days"></asp:Label>
            </td>
            <td align="left" width="80%">
                <table id="Table7" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay1" runat="server" Text="Sunday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="20%" class="label">
                            <asp:TextBox ID="txtDay1" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay1" ValidationGroup="formValidation" ControlToValidate="txtDay1"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay1" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay1"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RangeValidator>
                        </td>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay2" runat="server" Text="Monday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="20%" class="label">
                            <asp:TextBox ID="txtDay2" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay2" ValidationGroup="formValidation" ControlToValidate="txtDay2"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay2" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay2"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay3" runat="server" Text="Tuesday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="10%" class="label">
                            <asp:TextBox ID="txtDay3" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay3" ValidationGroup="formValidation" ControlToValidate="txtDay3"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay3" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay3"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay4" runat="server" Text="Wednesday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="20%" class="label">
                            <asp:TextBox ID="txtDay4" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay4" ValidationGroup="formValidation" ControlToValidate="txtDay4"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay4" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay4"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay5" runat="server" Text="Thursday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="20%" class="label">
                            <asp:TextBox ID="txtDay5" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay5" ValidationGroup="formValidation" ControlToValidate="txtDay5"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay5" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay5"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay6" runat="server" Text="Friday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="10%" class="label">
                            <asp:TextBox ID="txtDay6" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay6" ValidationGroup="formValidation" ControlToValidate="txtDay6"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay6" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay6"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="middle" width="15%" class="label">
                            <asp:CheckBox ID="cbxDay7" runat="server" Text="Saturday" AutoPostBack="True" OnCheckedChanged="Days_CheckedChanged" />
                        </td>
                        <td align="left" valign="middle" width="20%" class="label">
                            <asp:TextBox ID="txtDay7" runat="server" Width="80px" class="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDay7" ValidationGroup="formValidation" ControlToValidate="txtDay7"
                                runat="server" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Required></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDay7" runat="server" ValidationGroup="formValidation" ControlToValidate="txtDay7"
                                MinimumValue="1" Type="Integer" MaximumValue="1000000" CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                        </td>
                        <td align="left" valign="middle" width="15%">&nbsp;
                        </td>
                        <td align="left" valign="middle" width="20%">&nbsp;
                        </td>
                        <td align="left" valign="middle" width="15%">&nbsp;
                        </td>
                        <td align="left" valign="middle" width="10%">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table12" cellspacing="0" cellpadding="2" width="100%" border="0">
    <asp:Panel runat="server" ID="pnlErrorMessage">
        <tr>
            <td align="left" valign="middle" width="20%">
                <asp:Label ID="lblError" runat="server" Text="Schedule Errors" CssClass="errormsg"></asp:Label>
            </td>
            <td align="left" valign="middle" width="80%">
                <asp:BulletedList ID="blErrorMessage" runat="server" Width="100%" CssClass="errormsg" />
            </td>
        </tr>
    </asp:Panel>
</table>
<table id="Table8" cellspacing="0" cellpadding="2" width="100%" border="0">
    <tr>
        <td align="left" width="20%">&nbsp;
        </td>
        <td align="left" width="80%">
            <asp:Button ID="btnSetupBlast" runat="server" Text="Blast Now" Width="180px" ValidationGroup="formValidation"
                Visible="false" Enabled="false" />
        </td>
    </tr>
</table>
