<%@ Page Language="c#" Inherits="ecn.accounts.Engines.ScheduleDemo" CodeBehind="ScheduleDemo.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Schedule Demo</title>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
    <div style="font-size: 11px; font-family: Arial, Helvetica, sans-serif" align="center">
        <img height="54" alt="" src="/ecn.images/customers/142/images/email-header.gif" width="564">
        <div style="width: 564px" align="center">
            <img height="151" alt="" src="/ecn.images/customers/142/images/reachout-sun.jpg"
                width="564"></div>
        <div style="border-right: #cccccc 1px dotted; padding-right: 15px; border-top: #000000 1px solid;
            padding-left: 15px; border-left: #cccccc 1px dotted; width: 564px" align="left">
            <br />
            <div style="padding-left: 50px">
                <asp:Label ID="lblMessage" runat="Server"></asp:Label>
                <br />
                Please choose a demo date (Central Time):<br />
                <asp:DropDownList ID="ddlDemoDates" runat="Server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblErrorMessage" runat="Server" Visible="False" ForeColor="Red"></asp:Label>
                <br />
                <asp:Button ID="btnSchduleDemo" runat="Server" Text="Schedule Now" OnClick="btnSchduleDemo_Click">
                </asp:Button>
                <asp:Button ID="btnDecline" runat="Server" Width="96px" Text="Decline" OnClick="btnDecline_Click">
                </asp:Button></div>
            <br />
            <br />
            <br />
            <div align="center">
                <br />
                <div style="border-top: #cccccc 1px solid; width: 85%">
                </div>
                <table cellspacing="0" cellpadding="10" width="90%" border='0'>
                    <tbody>
                        <tr valign="top">
                            <td width="33%" valign="top">
                                <div style="font-size: 16pt; color: #8da6c9; font-family: Arial Black, Arial, Helvetica, sans-serif"
                                    align="left">
                                    Cheaper.<br />
                                    <div style="font-size: 11px; color: #666; font-family: Arial, Helvetica, sans-serif;
                                        text-align: justify">
                                        Receive top notch service, support, and solutions at prices that rival all of our
                                        competition. Schedule a demo today and see for yourself.
                                    </div>
                                </div>
                            </td>
                            <td width="33%">
                                <div style="font-size: 16pt; color: #8da6c9; font-family: Arial Black, Arial, Helvetica, sans-serif"
                                    align="left">
                                    Faster.<br />
                                    <div style="font-size: 11px; color: #666; font-family: Arial, Helvetica, sans-serif;
                                        text-align: justify">
                                        Our intuitive interfaces make our industry leading propritary Internet marketing
                                        solutions some of the easiest to use in the world.
                                    </div>
                                </div>
                            </td>
                            <td width="33%">
                                <div style="font-size: 16pt; color: #8da6c9; font-family: Arial Black, Arial, Helvetica, sans-serif"
                                    align="left">
                                    Smarter.<br />
                                    <div style="font-size: 11px; color: #666; font-family: Arial, Helvetica, sans-serif;
                                        text-align: justify">
                                        We've built our solutions from the ground up. They're based off of user feedback
                                        and experiences dating back to the beginning of the Internet itself.
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <br />
                <br />
            </div>
        </div>
        <img height="54" alt="" src="/ecn.images/customers/142/images/email-footer.gif" width="564">
    </div>
    </form>
</body>
</html>
