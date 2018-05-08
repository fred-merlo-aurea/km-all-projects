<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PharmaLive_Signup.aspx.cs"
    Inherits="PaidPub.PharmaLive_Signup" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <title>PharmaLive.com eNewsletter Series</title>
    <link rel="STYLESHEET" type="text/css" href="http://www.pharmalive.com/css/pl.css.cfm?">

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

    <script src="http://www.pharmalive.com/includes/javascript/tooltip.js" language="JavaScript1.2"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <!-- START Conversion Tracking -->

    <script language="javascript" src="http://www.ecn5.com/ecn.accounts/assets/channelID_1/js/Conversion.js">
    </script>

    <!-- END Conversion Tracking -->

    <script type="text/javascript">

        function onclickchange(ctrl, resetctrlid) {

            if (ctrl.checked)
                document.getElementById(resetctrlid).checked = false;
        }

        function ongridclickchange(ctrl) {
            var targetctrlID = "";

            if (ctrl.checked) {
                if (ctrl.id.indexOf("chkUnsubscribe") > 0)
                    targetctrlID = ctrl.id.replace(/chkUnsubscribe/, "chkRenew");
                else
                    targetctrlID = ctrl.id.replace(/chkRenew/, "chkUnsubscribe");

                document.getElementById(targetctrlID).checked = false;
            }
        }


        function newImage(arg) {
            if (document.images) {
                rslt = new Image();
                rslt.src = arg;
                return rslt;
            }
        }
        function changeImages() {
            if (document.images && (preloadFlag == true)) {
                for (var i = 0; i < changeImages.arguments.length; i += 2) {
                    document[changeImages.arguments[i]].src = changeImages.arguments[i + 1];
                }
            }
        }
        var preloadFlag = false;
        function preloadImages() {
            if (document.images) {
                BTN_todays_news_over = newImage("http://www.pharmalive.com/images/BTN_todays_news-over.gif");
                BTN_med_ad_news_over = newImage("http://www.pharmalive.com/images/BTN_med_ad_news-over.gif");
                BTN_RnD_directions_over = newImage("http://www.pharmalive.com/images/BTN_RnD_directions-over.gif");
                BTN_information_tools_over = newImage("http://www.pharmalive.com/images/BTN_information_tools-over.gif");
                BTN_enewsletters_over = newImage("http://www.pharmalive.com/images/BTN_enewsletters-over.gif");
                BTN_conferences_over = newImage("http://www.pharmalive.com/images/BTN_conferences-over.gif");
                BTN_special_reports_over = newImage("http://www.pharmalive.com/images/BTN_special_reports-over.gif");
                preloadFlag = true;
            }
        }
    </script>

    <style>
        .eNewsName
        {
            font-family: verdana,arial,helvetica;
            font-size: 11px;
            font-weight: bold;
            color: #000000;
        }
        .eNewsName1
        {
            font-family: verdana,arial,helvetica;
            font-size: 11px;
            font-weight: bold;
            color: #900000;
        }
        #errorTop
        {
            background: url(images/errorRed_01.gif) top center no-repeat;
            width: 674px;
            height: 9px;
        }
        #errorMiddle
        {
            background: url(images/errorRed_02.gif) top center repeat-y;
        }
        #errorMiddle table
        {
            margin: 0 15px 0 0;
            font: bold 12px Arial, Helvetica, sans-serif;
            color: #a40000;
        }
        #errorBottom
        {
            background: url(images/errorRed_03.gif) top center no-repeat;
            width: 674px;
            height: 10px;
        }
        .subbox
        {
            border-width: 0px 0px 0px 0px;
            padding: 5px 5px 5px 5px;
            vertical-align: top;
            horizontal-align: center;
            border-style: solid solid solid solid;
            border-color: black black black black;
        }
        .subtext
        {
            padding: 0px 50px 5px 0px;
        }
        .outersubbox
        {
            vertical-align: top;
        }
        .eNewsHeader
        {
            padding: 0px 10px 0px 10px;
        }
        .eNewsDefault
        {
            padding: 0px 10px 0px 10px;
        }
        .button
        {
            font-family: trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
            width: 100px;
            background: url(images/button.gif) no-repeat left top;
            border: 0;
            font-weight: bold;
            color: #ffffff;
            height: 20px;
            cursor: pointer;
        }
        .style2
        {
            font-family: verdana,arial,helvetica;
            font-size: 11px;
            color: #444444;
            line-height: 14px;
            width: 357px;
            height: 161px;
        }
        .style4
        {
            height: 20px;
            width: 88%;
        }
        .style5
        {
            height: 25px;
            width: 88%;
        }
        .style6
        {
            height: 8px;
        }
    </style>
</head>
<body onload="preloadImages();">
    <div align="center">
        <div class="pageBorder">
            <table width="974" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="background-color: #FFFFFF;">
                <tr>
                    <td width="619" align="left" background="http://www.pharmalive.com/images/TILE_banner_area.gif"
                        style="background-image: url('http://www.pharmalive.com/images//TILE_banner_area.gif')">
                        <a href="http://www.pharmalive.com" title="Pharmalive - The Pulse of the Pharmaceutical Industry">
                            <img src="http://www.pharmalive.com/images//logo.gif" width="314" height="100" alt="Pharmalive - The Pulse of the Pharmaceutical Industry"
                                border="0"></a>
                    </td>
                    <td valign="top" background="http://www.pharmalive.com/images//TILE_banner_area.gif"
                        style="background-image: url('http://www.pharmalive.com/images//TILE_banner_area.gif');
                        padding-right: 10px;">
                        <!-- START login and Search Box -->
                        <table width="355" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="top">
                                    <div class="login" style="float: right; z-index: 10; position: relative;">
                                        <a href="http://www.pharmalive.com/login/?t=lo&from=%2F" title="Log In">
                                            <img src="http://www.pharmalive.com/images/BTN_login.gif" alt="Log In" width="106"
                                                height="26" border="0"></a>
                                        <img src="http://www.pharmalive.com/images//blank.gif.cfm?pdlmo=1" height="1" width="1"
                                            onload="if(this.src.match(/pdlmo=1$/)){this.src='http://www.pharmalive.com/images//blank.gif.cfm?pdlmo=1&i=0e30e2d6a64a70557741&s=' + screen.width + 'x' + screen.height + '&b=' + screen.colorDepth;}">
                                    </div>
                                    <br>
                                    <br>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <div align="left">
                                        <form method="post" name="search" action="http://www.pharmalive.com/search_engine/index.cfm"
                                        style="margin: 0; padding: 0;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <span style="color: #FFFFFF; font: 12px/14px arial bold; padding-top: 3px;">Search Criteria:</span>
                                                </td>
                                                <td>
                                                    <span style="color: #FFFFFF; font: 12px/14px arial bold; padding-top: 3px;">Search In:</span>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="text" name="criteria" value="keyword (s)" size="20" style="border: 1px solid #CECECE;
                                                        width: 150px; height: 15px; font-size: 11 px; font-family: verdana,arial,helvetica,sans-serif;"
                                                        onfocus="this.value='';">
                                                </td>
                                                <td>
                                                    <select name="search_by" size="1" style="border: 1px solid #CECECE; height: 19px;
                                                        font-size: 11px; font-family: verdana,arial,helvetica,sans-serif;">
                                                        <option value="news" selected="selected">-- select --</option>
                                                        <option value="news">news archive</option>
                                                        <option value="medad">Med Ad News</option>
                                                        <option value="randd">R&amp;D Directions</option>
                                                        <option value="all">entire archive</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <input type="submit" value="Search" style="background-color: #357ABB; border: 1px solid #e9fffc;
                                                        color: #e9fffc; font-family: verdana,arial,helvetica,sans-serif; font-size: 1.0em;
                                                        font-style: italic; font-weight: bold;">
                                                </td>
                                            </tr>
                                        </table>
                                        </form>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table id="primary_nav" width="950" height="38" border="0" cellpadding="0" cellspacing="0"
                            align="center">
                            <tr>
                                <td colspan="21" align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space.jpg" width="950" height="12"
                                        alt="">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-02.jpg" width="9" height="26"
                                        alt="">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com" title="Today&#39;s News">
                                        <img src="http://www.pharmalive.com/images/09up/pl-news-over.jpg" width="40" height="26"
                                            alt="" border="0" id="BTN_news" name="BTN_news"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-04.jpg" width="8" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/ate/" onmouseover="changeImages('BTN_ate', 'http://www.pharmalive.com/images/09up/pl-ate-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_ate', 'http://www.pharmalive.com/images/09up/pl-ate-up.jpg'); return true;"
                                        title="Ask The Experts">
                                        <img src="http://www.pharmalive.com/images/09up/pl-ate-up.jpg" width="100" height="26"
                                            alt="" border="0" id="BTN_ate" name="BTN_ate"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-06.jpg" width="10" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://whitepapers.pharmalive.com" onmouseover="changeImages('BTN_whitep', 'http://www.pharmalive.com/images/09up/pl-wp-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_whitep', 'http://www.pharmalive.com/images/09up/pl-wp-up.jpg'); return true;"
                                        title="Pharmaceutical Whitepapers">
                                        <img src="http://www.pharmalive.com/images/09up/pl-wp-up.jpg" width="80" height="26"
                                            alt="" border="0" id="BTN_whitep" name="BTN_whitep"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-08.jpg" width="8" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/blogs/" onmouseover="changeImages('BTN_blogs', 'http://www.pharmalive.com/images/09up/pl-blogs-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_blogs', 'http://www.pharmalive.com/images/09up/pl-blogs-up.jpg'); return true;"
                                        title="Pharmalive Pharmaceutical Blogs">
                                        <img src="http://www.pharmalive.com/images/09up/pl-blogs-up.jpg" width="40" height="26"
                                            alt="" border="0" id="BTN_blogs" name="BTN_blogs"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-10.jpg" width="6" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/magazines/medad/" onmouseover="changeImages('BTN_medad', 'http://www.pharmalive.com/images/09up/pl-medad-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_medad', 'http://www.pharmalive.com/images/09up/pl-medad-up.jpg'); return true;"
                                        title="Med Ad News">
                                        <img src="http://www.pharmalive.com/images/09up/pl-medad-up.jpg" width="91" height="26"
                                            alt="" border="0" id="BTN_medad" name="BTN_medad"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-12.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/magazines/randd/" onmouseover="changeImages('BTN_randd', 'http://www.pharmalive.com/images/09up/pl-rndd-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_randd', 'http://www.pharmalive.com/images/09up/pl-rndd-up.jpg'); return true;"
                                        title="R &#38; D Directions">
                                        <img src="http://www.pharmalive.com/images/09up/pl-rndd-up.jpg" width="101" height="26"
                                            alt="" border="0" id="BTN_randd" name="BTN_randd"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-14.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/special_reports/" onmouseover="changeImages('BTN_specialrep', 'http://www.pharmalive.com/images/09up/pl-specialrep-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_specialrep', 'http://www.pharmalive.com/images/09up/pl-specialrep-up.jpg'); return true;"
                                        title="Special Reports">
                                        <img src="http://www.pharmalive.com/images/09up/pl-specialrep-up.jpg" width="106"
                                            height="26" alt="" border="0" id="BTN_specialrep" name="BTN_specialrep"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-16.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/content/databases/" onmouseover="changeImages('BTN_infotools', 'http://www.pharmalive.com/images/09up/pl-infotools-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_infotools', 'http://www.pharmalive.com/images/09up/pl-infotools-up.jpg'); return true;"
                                        title="Information Tools">
                                        <img src="http://www.pharmalive.com/images/09up/pl-infotools-up.jpg" width="125"
                                            height="26" alt="" border="0" id="BTN_infotools" name="BTN_infotools"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-18.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/enewsletters/newsletters.cfm" onmouseover="changeImages('BTN_enews', 'http://www.pharmalive.com/images/09up/pl-enews-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_enews', 'http://www.pharmalive.com/images/09up/pl-enews-up.jpg'); return true;"
                                        title="eNewsletters">
                                        <img src="http://www.pharmalive.com/images/09up/pl-enews-up.jpg" width="94" height="26"
                                            alt="" border="0" id="BTN_enews" name="BTN_enews"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-20.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                                <td align="left" valign="top">
                                    <a href="http://www.pharmalive.com/content/conferences/" onmouseover="changeImages('BTN_confs', 'http://www.pharmalive.com/images/09up/pl-confs-over.jpg'); return true;"
                                        onmouseout="changeImages('BTN_confs', 'http://www.pharmalive.com/images/09up/pl-confs-up.jpg'); return true;"
                                        title="Conferences">
                                        <img src="http://www.pharmalive.com/images/09up/pl-confs-up.jpg" width="90" height="26"
                                            alt="Conferences" border="0" id="BTN_confs" name="BTN_confs"></a>
                                </td>
                                <td align="left" valign="top">
                                    <img src="http://www.pharmalive.com/images/09up/pl-nav-space-22.jpg" width="7" height="26"
                                        alt="" border="0">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top" style="height: 24px;">

                        <script language="javascript" src="PL_dropdown.js"></script>

                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <img src="images/pharmalive_header.png" border="0">
                    </td>
                </tr>
            </table>
            <form name="frmmain" id="frmmain" runat="server">
            <asp:Label ID="lblcustomerID" runat="server" Text="2069" Visible="false"></asp:Label>
            <asp:Label ID="lblsfID" runat="server" Text="741" Visible="false"></asp:Label>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="100%" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table height="67" width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <asp:Image ID="EcnImage1" ImageUrl="images/errorEx.jpg" runat="server" Style="padding: 0 0 0 15px;" />
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
                <br />
            </asp:PlaceHolder>
            <asp:Panel ID="pnlEmailAddress" runat="server" Visible="true" HorizontalAlign="Left">
                <!-- START Conversion Tracking -->

                <script language="Javascript">
	<!--
                    ECNstepname = 'EnterEmailAddress';
                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	//-->
                </script>

                <!-- END Conversion Tracking -->
                <div class="eNewsHeader">
                    PharmaLive/TherapeuticsDaily/Pharmalot eNewsletters – Subscribe Now – For FREE!
                    <br />
                </div>
                <div class="eNewsBody" style="margin-left: 10px">
                    <p>
                        <span class="input_caption">Get quick to read insight into events impacting pharmaceutical
                            and medical device business, marketing, and product development. Our editors search
                            for the most relevant information and provide busy professionals like you with insight
                            into how this information will impact your business.</span>
                    </p>
                    <table width="742" cellpadding="0" cellspacing="0" style="height: 167px">
                        <tr>
                            <td colspan="2">
                                <font size="2" color="black"><b>Subscribers receive:</b></font>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="2">
                                <ul style="list-style-type: disc">
                                    <li>Briefs of the latest industry events, including:</li>
                                    <ul style="list-style-type: disc">
                                        <li>M&A activity</li>
                                        <li>Licensing and partnership deals</li>
                                        <li>Marketing initiatives</li>
                                        <li>Financial results</li>
                                        <li>FDA/EMEA rulings</li>
                                        <li>Legislative activity</li>
                                        <li>Clinical trial results</li>
                                        <li>Manufacturing initiatives/issues</li>
                                    </ul>
                                    <br />
                                    <li>Access to full articles for expanded coverage</li>
                                    <li>Breaking News alerts</li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <span class="eNewsBody">We offer a variety of daily, weekly, and monthly newsletters
                            to select from.</span> <span class="eNewsName">You choose the format that best fits
                                your information needs.</span></p>
                    <p>
                        <span class="eNewsName">To start your FREE subscription, enter your email address below
                            and click <i>Continue</i>.</span></p>
                    <p>
                        <span class="eNewsName">Already a subscriber?</span><span class="eNewsBody"> Enter your
                            e-mail address below and click <i>Continue</i> to access your account.</span>
                    </p>
                </div>
                &nbsp;&nbsp;&nbsp;&nbsp;<span class="eNewsName">Email Address:</span>&nbsp;
                <asp:TextBox runat="server" ID="txtE" Width="200" CssClass="formborder"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSubmit" runat="server" Text="Continue" CssClass="button" Width="80"
                    OnClick="btnSubmit_Click" CausesValidation="true" ValidationGroup="EmailAddress" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtE"
                    runat="server" ErrorMessage=">> required" Font-Size="X-Small" Display="Dynamic"
                    ValidationGroup="EmailAddress"></asp:RequiredFieldValidator>
                <br />
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlInfo" runat="server" Visible="false" HorizontalAlign="Left">
                <!-- START Conversion Tracking -->
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    style="background-color: #FFFFFF;">
                    <tr>
                        <td valign="top" align="left">
                            <asp:Panel ID="pnlStep1" runat="server" Visible="false">

                                <script language="Javascript">

                                    //                                    ECNstepname = 'SubscriptionSelection'; 
                                    //                                    document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
     
                                </script>

                                <asp:Panel ID="pnlCurrentSubscriptions" runat="server" Visible="true">
                                    <table width="100%" height="100%" align="left" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="height: 10px;">
                                                <hr style="color: #eeeeee" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="style6">
                                                <span style="font-size: 12pt" class="eNewsHeader"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div style="padding-left: 2em; padding-top: 1em; padding-right: 1em; vertical-align: top;">
                                                    <table width="900px" border="1" align="left" style="border: 1px solid gray; border-collapse: collapse;
                                                        vertical-align: top">
                                                        <%--<asp:Panel ID="pnl_14092" runat="server" Visible="true">--%>
                                                        <tr>
                                                            <td align="left" style="background-color: #990000; font-weight: bold; font-size: 11pt;
                                                                color: #FFFFCC;" class="style4">
                                                                &nbsp;Your Newsletter Subscription(s) :
                                                            </td>
                                                            <td width="15%" style="text-align: center; background-color: #990000; font-weight: bold;
                                                                font-size: 11pt; color: #FFFFCC;">
                                                                Subscribe/<br />
                                                                Unsubscribe
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; background-color: #EDEDF5;" colspan="2" align="left">
                                                                <span style="font-family: verdana,arial,helvetica; font-size: 13px; font-weight: bold;
                                                                    color: #900000; height: 20px; padding: 7px 7px 7px 5px;">Daily</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px; border-right: 1px solid #c8c8c8; border-collapse: collapse"
                                                                class="style5" width="85%">
                                                                <span class="eNewsName"><i>PharmaLive</i> Daily Advantage:</span><span class="eNewsBody">&nbsp;concise
                                                                    report of the day's most important and impactful news </span>
                                                            </td>
                                                            <td valign="top" align="center" height="11" width="15%">
                                                                <input type="checkbox" value="Y" name="g_14092_free" id="g_14092_free" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px;" class="style5">
                                                                <span class="eNewsName"><i>Pharmalot</i> Daily:&nbsp;</span><span class="eNewsBody">editor’s
                                                                    insight and industry-insider commentary on issues, trends, and events impacting
                                                                    everything pharma</span>
                                                            </td>
                                                            <td valign="top" align="center" height="11">
                                                                <input type="checkbox" value="Y" name="g_40065_free" id="g_40065_free" runat="server" />
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td valign="top" align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px;" class="style5">
                                                                <span class="eNewsName"><i>Med Ad News</i> Daily:&nbsp;</span><span class="eNewsBody">covers top headline news, key marketing strategies, and healthcare twitter feeds to bring you a concise summary of trends, events, and issues that have everyone talking</span>
                                                            </td>
                                                            <td valign="top" align="center" height="11">
                                                                <input type="checkbox" value="Y" name="g_154060_free" id="g_154060_free" runat="server" />
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td style="text-align: left; background-color: #EDEDF5;" colspan="2">
                                                                <span style="font-family: verdana,arial,helvetica; font-size: 13px; font-weight: bold;
                                                                    color: #900000; height: 20px; padding: 7px 7px 7px 5px;">Weekly</span>
                                                            </td>
                                                        </tr>
                                                     
                                                      
                                                        <tr>
                                                            <td align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px;" valign="top" class="style5">
                                                                <span class="eNewsName"><em>R&amp;D </em>Pharma Business Connect:</span> <span class="eNewsBody">
                                                                    provides insight into deal and investment strategies including development agreements,
                                                                    milestone payments, share offerings, and M&A activity</span>
                                                            </td>
                                                            <td align="center" height="11" valign="top" width="10%">
                                                                <input id="g_69224_free" runat="server" name="g_69224_free" type="checkbox" value="Y" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; width: 677px; background-color: #EDEDF5;" colspan="2">
                                                                <span style="font-family: verdana,arial,helvetica; font-size: 13px; font-weight: bold;
                                                                    color: #900000; height: 20px; padding: 7px 7px 7px 5px;">Monthly</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px;" valign="top" class="style5">
                                                                <span class="eNewsName"><em>R&amp;D Directions</em> eBulletin:</span> <span class="eNewsBody">
                                                                    focuses on issues affecting the research and development of medicines</span>
                                                            </td>
                                                            <td align="center" height="11" valign="top" width="10%">
                                                                <input id="g_16585_free" runat="server" name="g_16585_free" type="checkbox" value="Y" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-left: 25px; padding-bottom: 5px; padding-top: 5px;
                                                                padding-right: 5px;" valign="top" class="style5">
                                                                <span class="eNewsName">Pharma Webcast Roundup:</span> <span class="eNewsBody">
                                                                    A compilation of our pharma webcasts, brought to you by <i>Med Ad News, Pharmalot, and R&amp;D Directions</i></span>
                                                            </td>
                                                            <td align="center" height="11" valign="top" width="10%">
                                                                <input id="g_121238_free" runat="server" name="g_121238_free" type="checkbox" value="Y" />
                                                            </td>
                                                        </tr>
                                                      </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlStep2" runat="server" Visible="false" HorizontalAlign="Left">

                                <script language="Javascript">

                                    //                                ECNstepname = 'Checkout';
                                    //                                document.write("<img src='" + PostConversionData() + "' height=1 width=1 border=0>");
	
                                </script>

                                <table width="100%" align="left">
                                    <tr>
                                        <td>
                                            <div class="eNewsHeader">
                                                <span style="font-size: 12pt">Account Information:</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="padding-left: 2em;">
                                                <table width="100%" border="0" align="left">
                                                    <tr>
                                                        <td align="left" class="input_caption" width="20%">
                                                            First Name:*
                                                        </td>
                                                        <td align="left" width="80%">
                                                            <asp:TextBox runat="server" ID="txtfirstname" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="rfqfirstname" ControlToValidate="txtfirstname" runat="server"
                                                                ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Last Name:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtlastname" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtlastname"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Title:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txttitle" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txttitle"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Company Name:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtcompany" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Address 1:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtaddress" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtaddress"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Address 2:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtaddress2" Width="200" CssClass="formborder"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            City:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtcity" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtcity"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            State:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="drpstate" runat="server" CssClass="formborder">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="drpstate"
                                                                InitialValue="" runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Zip:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtzip" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtzip"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Country:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="drpcountry" runat="server">
                                                                <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                                <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                                <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                                <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                                <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                                <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                                <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                                <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                                <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                                <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                                <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                                <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                                <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                                <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                                <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                                <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                                <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                                <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                                <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                                <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                                <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                                <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                                <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                                <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                                <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                                <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                                <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                                <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                                <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                                <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                                <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                                <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                                <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                                <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                                <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                                <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                                <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                                <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                                <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                                <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                                <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                                <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                                <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                                <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                                <asp:ListItem Value="CN">China</asp:ListItem>
                                                                <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                                <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                                <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                                <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                                <asp:ListItem Value="CG">Congo</asp:ListItem>
                                                                <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                                <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                                <asp:ListItem Value="CI">Cote D'ivoire</asp:ListItem>
                                                                <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                                <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                                <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                                <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                                <asp:ListItem Value="KP">Democratic People's Republic Of Korea</asp:ListItem>
                                                                <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                                <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                                <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                                <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                                <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                                <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                                <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                                <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                                <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                                <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                                <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                                <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                                <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                                <asp:ListItem Value="FM">Federated States Of Micronesia</asp:ListItem>
                                                                <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                                <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                                <asp:ListItem Value="FR">France</asp:ListItem>
                                                                <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                                <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                                <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                                <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                                <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                                <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                                <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                                <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                                <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                                <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                                <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                                <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                                <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                                <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                                <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                                <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                                <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                                <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                                <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                                <asp:ListItem Value="HM">Heard Island And Mcdonald Islands</asp:ListItem>
                                                                <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                                <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                                <asp:ListItem Value="HK">Hong Kong</asp:ListItem>
                                                                <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                                <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                                <asp:ListItem Value="IN">India</asp:ListItem>
                                                                <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                                <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                                <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                                <asp:ListItem Value="IR">Islamic Republic Of Iran</asp:ListItem>
                                                                <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                                <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                                <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                                <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                                <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                                <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                                <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                                <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                                <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                                <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                                <asp:ListItem Value="LA">Lao People's Democratic Republic</asp:ListItem>
                                                                <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                                <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                                <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                                <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                                <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>
                                                                <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                                <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                                <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                                <asp:ListItem Value="MO">Macao</asp:ListItem>
                                                                <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                                <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                                <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                                <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                                <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                                <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                                <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                                <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                                <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                                <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                                <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                                <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                                <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                                <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                                <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                                <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                                <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                                <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                                <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                                <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                                <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                                <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                                <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                                <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                                <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                                <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                                <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                                <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                                <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                                <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                                <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                                <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                                <asp:ListItem Value="PS">Occupied Palestinian Territory</asp:ListItem>
                                                                <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                                <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                                <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                                <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                                <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                                <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                                <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                                <asp:ListItem Value="PH">Philippines</asp:ListItem>
                                                                <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                                <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                                <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                                <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                                <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                                <asp:ListItem Value="MD">Republic Of</asp:ListItem>
                                                                <asp:ListItem Value="KR">Republic Of Korea</asp:ListItem>
                                                                <asp:ListItem Value="RE">Reunion</asp:ListItem>
                                                                <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                                <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                                <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                                <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                                <asp:ListItem Value="KN">Saint Kitts And Nevis</asp:ListItem>
                                                                <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                                <asp:ListItem Value="PM">Saint Pierre And Miquelon</asp:ListItem>
                                                                <asp:ListItem Value="VC">Saint Vincent And The Grenadines</asp:ListItem>
                                                                <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                                <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                                <asp:ListItem Value="ST">Sao Tome And Principe</asp:ListItem>
                                                                <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                                <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                                <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                                <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                                <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                                <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                                <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                                <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                                <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                                <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                                <asp:ListItem Value="GS">South Georgia And The South Sandwich Islands</asp:ListItem>
                                                                <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                                <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                                <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                                <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                                <asp:ListItem Value="SJ">Svalbard And Jan Mayen</asp:ListItem>
                                                                <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                                <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                                <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                                <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>
                                                                <asp:ListItem Value="TW">Taiwan</asp:ListItem>
                                                                <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                                <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                                <asp:ListItem Value="CD">The Democratic Republic Of The Congo</asp:ListItem>
                                                                <asp:ListItem Value="MK">The Former Yugoslav Republic Of Macedonia</asp:ListItem>
                                                                <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                                <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                                <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                                <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                                <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                                <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                                <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                                <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                                <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                                <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                                <asp:ListItem Value="VI">U.S. Virgin Islands</asp:ListItem>
                                                                <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                                <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                                <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                                <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                                <asp:ListItem Value="TZ">United Republic Of Tanzania</asp:ListItem>
                                                                <asp:ListItem Selected="true" Value="US">United States</asp:ListItem>
                                                                <asp:ListItem Value="UM">United States Minor Outlying Islands</asp:ListItem>
                                                                <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                                <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                                <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                                <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                                                                <asp:ListItem Value="VN">Viet Nam</asp:ListItem>
                                                                <asp:ListItem Value="WF">Wallis and Futuna</asp:ListItem>
                                                                <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                                <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                                <asp:ListItem Value="YU">Yugoslavia</asp:ListItem>
                                                                <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                                <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="drpcountry"
                                                                InitialValue="" runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Telephone:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtphone" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtphone"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Fax:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtfax" Width="200" CssClass="formborder"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Email Address:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtemail" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtemail"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            Promotion Code:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtpromo" Width="200" CssClass="formborder" ReadOnly></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption" style="line-height: 12px;">
                                                            Company Description:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="user_Business" runat="server" CssClass="formborder">
                                                                <asp:ListItem Value="">Your company's primary business activity...</asp:ListItem>
                                                                <asp:ListItem Value="01">&nbsp;&nbsp;Pharmaceutical manufacturer</asp:ListItem>
                                                                <asp:ListItem Value="11">&nbsp;&nbsp;Biotechnology company</asp:ListItem>
                                                                <asp:ListItem Value="15">&nbsp;&nbsp;Generic pharmaceutical
                                                                            manufacturer</asp:ListItem>
                                                                <asp:ListItem Value="09">&nbsp;&nbsp;Medical equipment manufacturer</asp:ListItem>
                                                                <asp:ListItem Value="12">&nbsp;&nbsp;Contract research organization</asp:ListItem>
                                                                <asp:ListItem Value="13">&nbsp;&nbsp;Clinical study site/SMO</asp:ListItem>
                                                                <asp:ListItem Value="14">&nbsp;&nbsp;Clinical lab</asp:ListItem>
                                                                <asp:ListItem Value="03">&nbsp;&nbsp;Healthcare communications
                                                                            company</asp:ListItem>
                                                                <asp:ListItem Value="04">&nbsp;&nbsp;Marketing services company</asp:ListItem>
                                                                <asp:ListItem Value="05">&nbsp;&nbsp;General business services
                                                                            company</asp:ListItem>
                                                                <asp:ListItem Value="06">&nbsp;&nbsp;Hospital</asp:ListItem>
                                                                <asp:ListItem Value="07">&nbsp;&nbsp;Academic/University
                                                                            research institution</asp:ListItem>
                                                                <asp:ListItem Value="08">&nbsp;&nbsp;Government agency</asp:ListItem>
                                                                <asp:ListItem Value="10">&nbsp;&nbsp;Data management company</asp:ListItem>
                                                                <asp:ListItem Value="40">&nbsp;&nbsp;Packaging company</asp:ListItem>
                                                                <asp:ListItem Value="41">&nbsp;&nbsp;Executive recruitment agency</asp:ListItem>
                                                                <asp:ListItem Value="42">&nbsp;&nbsp;Venture capital/financial investment</asp:ListItem>
                                                                <asp:ListItem Value="43">&nbsp;&nbsp;Consulting firm</asp:ListItem>
                                                                <asp:ListItem Value="44">&nbsp;&nbsp;Media company</asp:ListItem>
                                                                <asp:ListItem Value="45">&nbsp;&nbsp;Legal firm</asp:ListItem>
                                                                <asp:ListItem Value="99">&nbsp;&nbsp;Other support or service
                                                                            company</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="user_Business"
                                                                InitialValue="" runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="input_caption" style="line-height: 12px;">
                                                            Primary Responsibility:*
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="user_Responsibility" runat="server" CssClass="formborder">
                                                                <asp:ListItem Value="">Your primary area of responsibility...</asp:ListItem>
                                                                <asp:ListItem Value="09">&nbsp;&nbsp;R&D management</asp:ListItem>
                                                                <asp:ListItem Value="10">&nbsp;&nbsp;Senior management</asp:ListItem>
                                                                <asp:ListItem Value="34">&nbsp;&nbsp;Finance management</asp:ListItem>
                                                                <asp:ListItem Value="38">&nbsp;&nbsp;Business strategy</asp:ListItem>
                                                                <asp:ListItem Value="12">&nbsp;&nbsp;Product management</asp:ListItem>
                                                                <asp:ListItem Value="23">&nbsp;&nbsp;Marketing, advertising, or promotion management</asp:ListItem>
                                                                <asp:ListItem Value="14">&nbsp;&nbsp;Sales management</asp:ListItem>
                                                                <asp:ListItem Value="15">&nbsp;&nbsp;Agency account management</asp:ListItem>
                                                                <asp:ListItem Value="19">&nbsp;&nbsp;Marketing services</asp:ListItem>
                                                                <asp:ListItem Value="20">&nbsp;&nbsp;Media management (incl. directors/planners)</asp:ListItem>
                                                                <asp:ListItem Value="24">&nbsp;&nbsp;Medical director/associate medical director</asp:ListItem>
                                                                <asp:ListItem Value="25">&nbsp;&nbsp;Clinical trials management</asp:ListItem>
                                                                <asp:ListItem Value="26">&nbsp;&nbsp;Clinical/drug research</asp:ListItem>
                                                                <asp:ListItem Value="28">&nbsp;&nbsp;Clinical monitoring/CRC/CRA</asp:ListItem>
                                                                <asp:ListItem Value="31">&nbsp;&nbsp;Clinical document preparation</asp:ListItem>
                                                                <asp:ListItem Value="27">&nbsp;&nbsp;Regulatory affairs</asp:ListItem>
                                                                <asp:ListItem Value="22">&nbsp;&nbsp;Quality control</asp:ListItem>
                                                                <asp:ListItem Value="33">&nbsp;&nbsp;Drug safety</asp:ListItem>
                                                                <asp:ListItem Value="32">&nbsp;&nbsp;Project management</asp:ListItem>
                                                                <asp:ListItem Value="29">&nbsp;&nbsp;Academic research or professor</asp:ListItem>
                                                                <asp:ListItem Value="30">&nbsp;&nbsp;Data management or analysis</asp:ListItem>
                                                                <asp:ListItem Value="35">&nbsp;&nbsp;Licensing</asp:ListItem>
                                                                <asp:ListItem Value="36">&nbsp;&nbsp;Manufacturing</asp:ListItem>
                                                                <asp:ListItem Value="37">&nbsp;&nbsp;IT management</asp:ListItem>
                                                                <asp:ListItem Value="39">&nbsp;&nbsp;Legal professional</asp:ListItem>
                                                                <asp:ListItem Value="99">&nbsp;&nbsp;Other functions</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="user_Responsibility"
                                                                runat="server" ErrorMessage=">> required" Font-Size="X-Small" InitialValue=""></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left" class="input_caption">
                                                            <br />
                                                            To change your email address, send an email to <a href="mailto:pharmalive@kmpsgroup.com">
                                                                pharmalive@kmpsgroup.com</a>.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="eNewsHeader">
                                                <span style="font-size: 12pt">Subscription Agreement:</span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="padding-left: 2em;">
                                                <table width="100%" border="0" align="left">
                                                    <tr>
                                                        <td align="left" class="input_caption">
                                                            <asp:CheckBox ID="chkVerify" runat="server" />
                                                            I have confirmed that the information above regarding my subscription is accurate
                                                            and that I have read and acknowledged the terms of use.*
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="padding-left: 2em;">
                                <table width="100%" cellpadding="0" border="0" align="center" width="100%">
                                    <tr>
                                        <td>
                                            &nbsp; * = Required Field
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnNext" runat="server" Text="Submit" Width="80" Visible="true" CssClass="button"
                                                OnClick="btnNext_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table width="752" border="0" cellspacing="0" cellpadding="0" style="margin-top: 18px;
                padding-bottom: 24px; background-color: #FFFFFF;">
                <tr>
                    <td class="footer" align="left">
                        <a href="http://www.pharmalive.com/content/engel/about.cfm" target="_blank" class="footer">
                            about us | <a href="http://www.pharmalive.com/content/advertise/advertise.cfm" target="_blank"
                                class="footer">advertise</a> | <a href="http://www.pharmalive.com/content/engel/contact.cfm"
                                    target="_blank" class="footer">contact us</a> | <a href="http://www.pharmalive.com/"
                                        target="_blank" class="footer">home</a><br>
                            <a href="http://www.pharmalive.com/content/newsletters/xtra/terms.cfm" target="_blank"
                                class="footer">terms of use</a> | <a href="http://www.pharmalive.com/Content/policy/privacy.cfm"
                                    target="_blank" class="footer">privacy</a> | <a href="http://www.pharmalive.com/Content/policy/copyright.cfm"
                                        target="_blank" class="footer">copyright</a> | <a href="http://www.pharmalive.com/content/sitemap/sitemap.cfm"
                                            target="_blank" class="footer">site map</a>
                    </td>
                    <td align="right" valign="top">
                        &copy;UBM Canon
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>
</body>
</html>
