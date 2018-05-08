<%@ Control Language="c#" Inherits="ecn.communicator.includes.EventTimerEditor" Codebehind="EventTimerEditor.ascx.cs" %>
<table width="100%">
    <tr>
        <td>
            <table id="tblEventTimer" cellpadding="0" class="tablecontent" style="border-right: #ffeecc thin solid;
                border-top: #ffeecc thin solid; border-left: #ffeecc thin solid; width: 920px;
                border-bottom: #ffeecc thin solid; height: 179px; background-color: #fffeff">
                <thead class="tableHeader" align="left">
                    Advanced Event Schedule Set Up
                </thead>
                <tr>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Monday</td>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Tuesday</td>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Wednesday</td>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Thursday</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdoMondaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtMondayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtMondayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtMondaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoTuesdaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtTuesdayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtTuesdayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtTuesdaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoWednesdaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtWednesdayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtWednesdayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtWednesdaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoThursdaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtThursdayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtThursdayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtThursdaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdoMondayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlMonday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday" Selected="True">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoTuesdayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlTuesday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday" Selected="True">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoWednesdayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlWednesday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday" Selected="True">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoThursdayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlThursday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday" Selected="True">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="8" style="border-top: #eeeeee 5px solid">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Friday</td>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Saturday</td>
                    <td colspan="2" style="border-right: #ffeecc thin solid; border-top: #ffeecc thin solid;
                        border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid; background-color: #fcf8e9" align="left">
                        Sunday</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdoFridaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtFridayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtFridayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtFridaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoSaturdaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtSaturdayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtSaturdayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtSaturdaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoSundaySendTime" runat="server" Text="Send at:" Checked="True"
                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:TextBox ID="txtSundayHour" runat="server" Width="24px">10</asp:TextBox>:
                        <asp:TextBox ID="txtSundayMinute" runat="server" Width="24px">0</asp:TextBox>:
                        <asp:TextBox ID="txtSundaySecond" runat="server" Width="24px">0</asp:TextBox></td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButton ID="rdoFridayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <tdv>
                        <asp:DropDownList ID="ddlFriday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday" Selected="True">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="Friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoSaturdayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSaturday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday" Selected="True">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left">
                        <asp:RadioButton ID="rdoSundayNextSendDay" runat="server" Text="Go to next" AutoPostBack="True"
                            OnCheckedChanged="RadioButton_CheckedChanged"></asp:RadioButton></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSunday" runat="server" Width="88px" Enabled="False">
                            <asp:ListItem Value="monday" Selected="True">Monday</asp:ListItem>
                            <asp:ListItem Value="tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="friday">Friday</asp:ListItem>
                            <asp:ListItem Value="saturday">Saturday</asp:ListItem>
                            <asp:ListItem Value="sunday">Sunday</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="left">
            <p>
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="errormsg"></asp:Label><br>
                <asp:CheckBox ID="chkShowTestWindow" AutoPostBack="True" Text="Show schedule test panel"
                    runat="server" CssClass="tablecontent" OnCheckedChanged="chkShowTestWindow_CheckedChanged">
                </asp:CheckBox></p>
        </td>
    </tr>
    <tr>
        <td style="width: 107px" align="left">
            <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click"></asp:Button></td>
    </tr>
    <tr>
        <td valign="bottom">
            <asp:Table ID="tblTestPanel" runat="server" Style="border-right: #ffeecc thin solid;
                border-top: #ffeecc thin solid; border-left: #ffeecc thin solid; border-bottom: #ffeecc thin solid;
                background-color: #fffeff" Visible="False">
                <asp:TableRow CssClass="tablecontent">
                </asp:TableRow>
                <asp:TableRow CssClass="tablecontent">
                    <asp:TableCell HorizontalAlign="left">
                        <asp:Label runat="server" ID="lblToday"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="tablecontent">
                    <asp:TableCell HorizontalAlign="left">
                        And if a triggered blast is scheduled after
                        <asp:TextBox runat="server" Width="20px" ID="txtAfter">1</asp:TextBox>
                        days.</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="tablecontent">
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Button runat="server" ID="btnTest" Text="Apply Rule" OnClick="btnTest_OnClick">
                        </asp:Button>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="tablecontent">
                    <asp:TableCell HorizontalAlign="left">
                        <asp:Label runat="server" ID="lblResult"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
</table>
