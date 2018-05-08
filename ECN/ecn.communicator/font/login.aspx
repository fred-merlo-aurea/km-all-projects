<%@ Page Language="c#" Inherits="ecn.communicator.front.login" CodeBehind="login.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border='0' cellspacing="0" cellpadding="0">
        <tr>
            <td class="tableContent" valign="top" width="370" align="left">
                .communicator is a turn-key, permissions based, email marketing and communications
                software for businesses and organizations of all sizes.
                <br />
                <center>
                    <table width="340">
                        <tr>
                            <td class="tableContent" align="left">
                                <img src="/ecn.images/images/whyemailmarketing.gif">
                            </td>
                        </tr>
                        <tr>
                            <td class="tableContent" align="left">
                                <img src="/ecn.images/images/bullet.gif">Improve qualified opportunities and closing
                                ratios
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Email newsletters & promotions
                                <br />
                                <img src="/ecn.images/images/bullet.gif">10 times the response rate of direct mail
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Track click thrus and response rates
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Dynamic personalization of messaging
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="340">
                        <tr>
                            <td class="tableContent" align="left">
                                <img src="/ecn.images/images/whycommunicator.gif">
                            </td>
                        </tr>
                        <tr>
                            <td class="tableContent" align="left">
                                <img src="/ecn.images/images/bullet.gif">Industry leading features and functionality
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Flexible offerings designed to fit ‘YOUR’
                                business
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Dedicated customer relationship manager
                                <br />
                                <img src="/ecn.images/images/bullet.gif">24/7 tech support
                                <br />
                                <img src="/ecn.images/images/bullet.gif">No HTML skills required
                                <br />
                                <img src="/ecn.images/images/bullet.gif">No IT burden on your company
                                <br />
                                <img src="/ecn.images/images/bullet.gif">HTML and text auto-sensing technology
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Build your own surveys and sign up forms
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Triggered event auto-mailings<br />
                                (Birthday, warranty, anniversary, etc...)
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Powerful Viral Marketing features
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Real-time statistics reports
                                <br />
                                <img src="/ecn.images/images/bullet.gif">Professional services from our team of
                                designers, developers and managers
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="360">
                        <tr>
                            <td valign="top" class="tablecontent" align="left">
                                <hr>
                                <font face="verdana" color="darkblue" size="-2">Sign up for our free quarterly newsletter
                                to stay on top of tech news and Internet Marketing best practices &amp; strategies.
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <font face="verdana" color="darkblue" size="-1"><b>Email:
                                    <input id="EmailAddress" size="25" name="e"></b></font>
                                <input type="image" src="/ecn.images/images/email.gif">
                                <font face="verdana" color="darkblue" size="-2">
                                    <br />
                                    &nbsp;&nbsp;&nbsp;<input id="RadioHTML" type="radio" checked value="html" name="f">HTML
                                    &nbsp;<input id="RadioText" type="radio" value="text" name="f">
                                    Text </font>
                            </td>
                        </tr>
                    </table>
            </td>
            <td valign="top" align="left">
                <table cellpadding="0" cellspacing="0" border='0'>
                    <tr>
                        <td>
                            <a href="levelsmallbiz.aspx">
                                <img src="/ecn.images/images/level1.jpg" border='0'></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="levelenterprise.aspx">
                                <img src="/ecn.images/images/level2.jpg" border='0'></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="levelchannel.aspx">
                                <img src="/ecn.images/images/level3.jpg" border='0'></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
