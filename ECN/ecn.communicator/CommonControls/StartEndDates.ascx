<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StartEndDates.ascx.cs" Inherits="ecn.communicator.CommonControls.StartEndDates" %>
<%--<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>--%>


<%--<%@ Register tagPrefix="KmDates" tagName="DateSelector" src="~/CommonControls/StartEndDates.ascx" %>--%>

<style>.errorClass { border: 1px solid red; }</style>

<script type="text/javascript">
    $(document).ready(function () {
        $("input[type=\"submit\"]").click(function (event) {
            event.preventDefault();
            alert("Handler for .submit() called.");
            $("#btnCheckDates").click();
            //$("form").submit();
        });
    });
</script>

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

<div style="float: left;text-align: right; width: 150px" class="rowSpacer">
    <label class="tableHeader" style="padding-right: 8px">Start Date:</label><asp:textbox ID="txtStartDate" runat="server" Width="65" CssClass="formfield" MaxLength="10"></asp:textbox>
    <%--<br/><label runat="server" style="text-align: left">Invalid Date</label>--%>
</div>
<div style="float: left; padding-left: 8px">
    <img onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%= txtStartDate.ClientID%>'),document.getElementById('<%= txtStartDate.ClientID%>')); return false;"
        src="http://images.ecn5.com/Images/icon-calendar.gif" align="absMiddle">
        &nbsp;&nbsp;
        <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
            position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
            frameborder="0" width="174" scrolling="no" height="189"></iframe>
        <br />
</div>
<div style="float: left;text-align: right; width: 150px" class="rowSpacer">
    <label class="tableHeader" style="padding-right: 8px">End Date:</label><asp:textbox ID="txtEndDate" runat="server" Width="65" CssClass="formfield" MaxLength="10"></asp:textbox>
    <%--<br/><label runat="server" style="text-align: left">Invalid Date</label>--%>
</div>
<div style="float: left; padding-left: 8px">
    <img onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%= txtEndDate.ClientID%>'),document.getElementById('<%= txtEndDate.ClientID%>')); return false;"
        src="http://images.ecn5.com/Images/icon-calendar.gif" align="absMiddle">
</div>
<div style="float: left; padding-left: 26px">
    <asp:Button ID="btnCheckDates" runat="server" OnClick="btnCheckDates_Click"/>
</div>
<div style="clear: both"></div>