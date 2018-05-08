<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlastCalendarView.aspx.cs"
    Inherits="ecn.communicator.main.blasts.BlastCalendarView" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
<%--    <script src="http://cdn.jquerytools.org/1.2.5/full/jquery.tools.min.js"></script>--%>

    <script>

        // execute your scripts when the DOM is ready. this is a good habit
        $(function () {
            // select all desired input fields and attach tooltips to them

            $("#TabContainer1_TabPanel3_cal a[title]").tooltip({

                // place tooltip on the right edge
                position: "center right",

                // a little tweaking of the position
                offset: [-2, 10],

                // use the built-in fadeIn/fadeOut effect
                effect: "fade",

                // custom opacity setting
                opacity: 0.7

            });

            $("#TabContainer1_TabPanel2 a[title]").tooltip({

                // place tooltip on the right edge
                position: "center right",

                // a little tweaking of the position
                offset: [-2, 10],

                // use the built-in fadeIn/fadeOut effect
                effect: "fade",

                // custom opacity setting
                opacity: 0.7

            });

            $("#TabContainer1_TabPanel1 a[title]").tooltip({

                // place tooltip on the right edge
                position: "center right",

                // a little tweaking of the position
                offset: [-2, 10],

                // use the built-in fadeIn/fadeOut effect      
                effect: "fade",

                // custom opacity setting
                opacity: 0.7

            });

        });
    </script>
     <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

    <script type="text/javascript" >
    
        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {
            $('.subject').each(function () {
                var initialString = $(this).html();
                initialString = initialString.replace(/'/g, "\\'");
                initialString = initialString.replace(/\r?\n|\r/g, ' ');
                initialString = twemoji.parse(eval("\'" + initialString + "\'"), { size: "16x16" });

                //if (!initialString.includes('<img')) {
                //    initialString = initialString.substr(0, 30);
                //}

                var regSplit = new RegExp("(<img.*?\/?>)");

                var imgSplit = new Array();

                imgSplit = initialString.split(regSplit);
                var textFullSplit = new Array();

                for (var i = 0; i < imgSplit.length; i++) {
                    var current = imgSplit[i];
                    if (current.includes('<img')) {
                        //currentindex is image add to finalSplit
                        textFullSplit.push(current);
                    }
                    else {
                        //currentindex is plain text, loop through each char and add to final split
                        for (var j = 0; j < current.length; j++) {
                            textFullSplit.push(current.charAt(j));
                        }
                    }
                }

                var finalText = "";

                if (initialString.length > 0) {
                    if (textFullSplit.length > 30) {
                        for (var i = 0; i < 30; i++) {
                            finalText = finalText.concat(textFullSplit[i]);
                        }
                        finalText = finalText + '...';
                    }
                
                }
                else {
                    finalText = initialString;
                }


                $(this).html(finalText);
            })
        }
        </script>
    <style>
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
        }
        fieldset
        {
            margin: 0.5em 0px;
            padding: 0.0px 0.5em 0px 0.5em;
            border: 1px solid #ccc;
        }
        fieldset p
        {
            margin: 2px 12px 10px 10px;
        }
        fieldset.login label, fieldset.register label, fieldset.changePassword label
        {
            display: block;
        }
        fieldset label.inline
        {
            display: inline;
        }
        legend
        {
            font-size: 1.5em;
            font-weight: 600;
            padding: 2px 4px 8px 4px;
            font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" , Arial, Helvetica, Verdana, sans-serif;
        }
        .TransparentGrayBackground
        {
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
        .overlay
        {
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
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
        .loader
        {
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
        * html .loader
        {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
        .modalBackground
        {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .popupbody
        {
            /*background:#fffff url(images/blank.gif) repeat-x top;*/
            z-index: 101;
            background-color: #FFFFFF;
            font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
        }
        .dailyview
        {
            border-color: Gray;
            border-style: solid;
            border-width: thin 1px;
        }
        .tooltip
        {
            font-family: "Lucida Grande" , "bitstream vera sans" , "trebuchet ms" ,sans-serif,verdana;
            background-color: #000;
            border: 1px solid #fff;
            padding: 10px 15px;
            width: 200px;
            display: none;
            color: #fff;
            text-align: left;
            font-size: 12px; /* outline radius for mozilla/firefox only */
            -moz-box-shadow: 0 0 10px #000;
            -webkit-box-shadow: 0 0 10px #000;
        }
        #headerTab1 a:hover
        {
            text-decoration: none;
        }
        #headerTab2 a:hover
        {
            text-decoration: none;
        }
        #headerTab3 a:hover
        {
            text-decoration: none;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<%--<asp:updateprogress id="uProgress" runat="server" displayafter="10" visible="true"
    associatedupdatepanelid="UpdatePanel1" dynamiclayout="true">   
     <ProgressTemplate>
     <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
     <asp:Panel ID="Panel2" CssClass="loader" runat="server">
     <div> <br /><br />
     <b>Processing...</b><br /><br />
     <img src="http://images.ecn5.com/images/loading.gif" /><br /><br /><br /><br />     
     </div> 
     </asp:Panel> 
     </asp:Panel> 
     </ProgressTemplate>
</asp:updateprogress>--%>
<div>
    <table class="subSecBr-1px" id="MyAppointmentTable" cellspacing="0" cellpadding="0"
        width="100%">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" align="right">
                    <tr>
                        <td style="background-color: Black" width="20px;">
                            &nbsp;
                        </td>
                        <td>
                            <b>SENT</b>
                        </td>
                        <td style="background-color: #78797B" width="20px;">
                            &nbsp;
                        </td>
                        <td>
                            <b>PENDING</b>
                        </td>
                        <td style="background-color: Green" width="20px;">
                            &nbsp;
                        </td>
                        <td>
                            <b>ACTIVE</b>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px;">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="label" valign="middle" align="left">
                            Subject
                        </td>
                        <td class="label" valign="middle" align="left">
                            Group
                        </td>
                        <td class="label" valign="middle" align="left">
                            User
                        </td>
                        <td class="label" valign="middle" align="left">
                            Campaign
                        </td>
                        <td class="label" valign="middle" align="left">
                            Blast Type
                        </td>
                        <td class="label" valign="middle" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align="left">
                            <asp:textbox class="formtextfield" id="txtSubjectSearch" CssClass="subject" runat="Server" enableviewstate="False"
                                columns="16"></asp:textbox>
                        </td>
                        <td class="label" valign="middle" align="left">
                            <asp:textbox class="formtextfield" id="txtGroupSearch" runat="Server" enableviewstate="False"
                                columns="16"></asp:textbox>
                        </td>
                        <td class="label" valign="middle" align="left">
                            <asp:dropdownlist id="SentUserID" runat="Server" datatextfield="UserName" autopostback="false"
                                datavaluefield="UserID" cssclass="formfield" onselectedindexchanged="SentUserID_SelectedIndexChanged">
                                    </asp:dropdownlist>
                        </td>
                        <td class="label" valign="middle" align="left">
                            <asp:dropdownlist class="formfield" id="drpCampaigns" runat="Server" datatextfield="CampaignName"
                                autopostback="false" datavaluefield="CampaignID" visible="false">
                                    </asp:dropdownlist>
                        </td>
                        <td class="label" valign="middle" align="left">
                            <asp:dropdownlist id="BlastTypeDD" runat="Server" autopostback="false" cssclass="formfield">
                                        <asp:ListItem Value="N" Selected="True">Live Blasts</asp:ListItem>
                                        <asp:ListItem Value="Y">Test Blasts</asp:ListItem>
                                    </asp:dropdownlist>
                        </td>
                        <td class="label" valign="middle" align="left">
                            <asp:button class="formbuttonsmall" id="btnSearch" onclick="btnSearch_Click" runat="Server"
                                enableviewstate="False" width="75" visible="true" text="Search"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="label" valign="middle" align="misddle">
                            <asp:label id="ErrorLabel" runat="Server" text="" visible="false" cssclass="errormsg"></asp:label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" style="padding-top: 0px;">
                &nbsp;
            </td>
        </tr>
    </table>
    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="2" Style="text-align: left;" AutoPostBack="true" OnActiveTabChanged="TabContainer1_ActiveTabChanged">
        <cc1:TabPanel ID="TabPanel2" runat="server" TabIndex="0" HeaderText="Daily">
        <HeaderTemplate>
            <span><b>Daily</b></span>
        </HeaderTemplate>
           <%-- <HeaderTemplate>
                <div id="headerTab1" style="height: 150px; text-align: center; width: 200px">
                   <asp:linkbutton id="lnkDaily" runat="server" text="Daily" causesvalidation="False" enabled="true"
                        font-bold="true" onclick="lnkDaily_Click">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                                Daily
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </asp:linkbutton>
                </div>
            </HeaderTemplate>--%>
            <ContentTemplate>
                <div style="text-align: right">
                    <table width="100%">
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="12%" align="left">
                                <span><b style="font-family: Arial; font-size: 12px;">Select Date:</b></span>
                            </td>
                            <td align="left" width="15%">
                                <asp:textbox id="txtDailyCalendar" runat="server" />
                                <cc1:CalendarExtender ID="calDaily" runat="server" TargetControlID="txtDailyCalendar" />
                            </td>
                            <td align="left" width="77%" valign="top">
                                <asp:imagebutton runat="server" id="btnDailyBlast" imageurl="/ecn.communicator/images/arrowright.gif"
                                    onclick="btnDailyBlast_Click" alternatetext="Get Daily Blast">
                            </asp:imagebutton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                
                                <asp:xml id="Xml1" runat="server"></asp:xml>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" ID="TabPanel1" TabIndex="1" HeaderText="Weekly" Font-Bold="true">
         <HeaderTemplate>
            <span><b>Weekly</b></span>
        </HeaderTemplate>
           <%-- <HeaderTemplate>
                <div id="headerTab3" style="height: 150px; text-align: center; width: 200px">
                    <asp:linkbutton id="lnkWeekly" runat="server" text="" causesvalidation="False" enabled="true"
                        onclick="lnkWeekly_Click" font-bold="true">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                            Weekly
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                        </asp:linkbutton>
                </div>
            </HeaderTemplate>--%>
            <ContentTemplate>
                <br />
                <asp:xml id="XmlWeekly" runat="server"></asp:xml>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" ID="TabPanel3" TabIndex="2" HeaderText="Monthly">
         <HeaderTemplate>
            <span><b>Monthly</b></span>
        </HeaderTemplate>
            <%--<HeaderTemplate>
                <div id="headerTab2" style="height: 150px; text-align: center; width: 200px">
                    <asp:linkbutton id="hlnkMonthly" runat="server" causesvalidation="False" enabled="true"        
                        font-bold="true" onclick="hlnkMonthly_Click">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                            Monthly
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;           
                         </asp:linkbutton>
                </div>
            </HeaderTemplate>--%>
            <ContentTemplate>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" style="padding-right: 10px; padding-bottom: 10px;">
                            <div style="width: 100%">
                                <asp:radiobuttonlist id="rbBlastCalType" font-names="Arial" runat="server" font-size="10px"
                                    repeatdirection="Horizontal" autopostback="true" font-bold="true" onselectedindexchanged="rbBlastCalType_SelectedIndexChanged">               
                                    <asp:ListItem Text="SUMMARY" Value="sum" Selected="True" />  
                                    <asp:ListItem Text="DETAILS" Value="det" />                   
                                    </asp:radiobuttonlist>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="cal" runat="server" style="width: 100%">
                                <asp:calendar id="MonthCalender" runat="server" enableviewstate="False" visible="true"
                                    onselectionchanged="MonthCalender_SelectionChanged" onvisiblemonthchanged="MonthCalender_OnVisibleMonthChanged"
                                    height="250px" backcolor="White" forecolor="Black" bordercolor="#CDCDCD" font-names="Verdana"
                                    titleformat="MonthYear" showtitle="true" shownextprevmonth="true" font-size="9pt"
                                    daynameformat="Full" selectmonthtext="&amp;gt;" prevmonthtext="&lt;img src='../../images/calendar-previous.png' /&gt;"
                                    nextmonthtext="&lt;img src='../../images/calendar-next.png' /&gt;" borderwidth="1px"
                                    width="98%" cellpadding="4" ondayrender="MonthCalender_DayRender">
                                <TodayDayStyle ForeColor="Black" BackColor="White"></TodayDayStyle>
                                <DayStyle Font-Size="Smaller" Font-Bold="True" HorizontalAlign="Left" Height="50px"
                                    BorderWidth="1px" BorderStyle="Solid" BorderColor="#CDCDCD" Width="14%" VerticalAlign="Top"
                                    BackColor="white"></DayStyle>
                                <NextPrevStyle HorizontalAlign="Left" Font-Size="8pt" Font-Names="Arial" Font-Bold="True" BorderStyle="None" >                          
                                </NextPrevStyle>                    
                                <DayHeaderStyle Font-Size="8pt" Font-Bold="True" HorizontalAlign="Left" Height="5px"
                                    ForeColor="gray" BorderStyle="None" BorderColor="White" Width="14%" VerticalAlign="Middle"
                                    BackColor="#CDCDCD"></DayHeaderStyle>
                                <SelectedDayStyle ForeColor="White" BackColor="White"></SelectedDayStyle>
                                <TitleStyle Font-Size="14px" Font-Names="Arial" Font-Bold="True" Height="26px" ForeColor="Black" BorderColor="White"     
                                    BackColor="White"></TitleStyle>                 
                                <OtherMonthDayStyle ForeColor="White"></OtherMonthDayStyle>        
                            </asp:calendar>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</div>
<br />
<br />
</asp:content>
