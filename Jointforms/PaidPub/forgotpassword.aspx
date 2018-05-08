<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpassword.aspx.cs"
    Inherits="PaidPub.forgotpassword" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <title>PharmaLive.com eNewsletter Series</title>
    <link rel="STYLESHEET" type="text/css" href="http://www.pharmalive.com/css/pl.css.cfm?">

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

    <script src="http://www.pharmalive.com/includes/javascript/tooltip.js" language="JavaScript1.2"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

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
    </style>
</head>
<body>
    <div align="center">
        <div class="pageBorder">
            <table width="752" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="background-color: #FFFFFF;">
                <tr>
                    <td align="left" background="http://www.pharmalive.com/images/TILE_banner_area.gif"
                        style="background-image: url('http://www.pharmalive.com/images/TILE_banner_area.gif')"
                        width="354">
                        <a href="http://www.pharmalive.com/" title="Pharmalive - The Pulse of the Pharmaceutical Industry">
                            <img src="http://www.pharmalive.com/images/logo.gif" width="314" height="100" alt="Pharmalive - The Pulse of the Pharmaceutical Industry"
                                border="0"></a>
                    </td>
                    <td valign="top" background="http://www.pharmalive.com/images/TILE_banner_area.gif"
                        style="background-image: url('http://www.pharmalive.com/images/TILE_banner_area.gif');
                        padding-right: 10px;">

                        <script type="text/javascript">
                            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            * determine browser type
                            *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                            var IE = "IE";
                            var browser = new Object();
                            browser.type = "notIE";
                            if (document.all) {
                                browser.type = IE;
                            }
                            function downloadtoolbar(p) {
                                document.location.href = 'http://www.pharmalivesearch.com' + "/page.do?action=" + encodeURIComponent(p);
                            }
                        </script>

                        <table width="398" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="top" align="left">
                                    <form name="input" action="http://www.pharmalivesearch.com/PhL/EUISearch.do" method="get"
                                    style="margin: 0; padding: 0;">
                                    <div class="login" style="float: right; z-index: 10; position: relative;">
                                        <a href="http://www.pharmalive.com/login/?t=lo" title="Log In">
                                            <img src="http://www.pharmalive.com/images/BTN_login.gif" alt="Log In" width="106"
                                                height="26" border="0"></a></div>
                                    <div style="z-index: 0; float: left; width: 400px; position: absolute; top: 20px;"
                                        align="left">
                                        <span style="color: #fFFf00; font: 16px/20px verdana;"><em><strong>NEW!</strong></em></span><br>
                                        <span style="color: #fFFf00; font: 12px/14px arial bold; margin-right: 40px;">The Pharma
                                            Industry</span><br>
                                        <div style="color: #fFFf00; font: 12px/14px arial bold; padding-top: 3px;">
                                            Search Engine:
                                            <input type="text" name="qgeneral">
                                            <input type="submit" value="Search" style="background-color: #357ABB; border: 1px solid #e9fffc;
                                                color: #e9fffc; font-family: verdana,arial,helvetica,sans-serif; font-size: 0.75em;
                                                font-style: italic; font-weight: bold;">&nbsp;&nbsp;<img src="http://www.pharmalive.com/images/ui/searchhelpIcon.gif"
                                                    alt="">&nbsp;<a href="http://www.pharmalive.com/content/searchHelp.cfm" style="font: 11px arial;
                                                        color: #ddffff;"><em>How it Works</em></a><br>
                                            <a style="font: 11px arial; color: #ddffff; padding-top: 8px;" href="javascript:if (browser.type==IE) downloadtoolbar('ietoolbar'); else downloadtoolbar('fftoolbar');">
                                                Download the PharmaLiveSearch.com toolbar</a>
                                        </div>
                                    </div>
                                    </form>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="752" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="17" align="left">
                                    <img id="NAV_primary_top" src="http://www.pharmalive.com/images/NAV_primary_top.gif"
                                        width="752" height="11" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <img id="NAV_primary_left_cap" src="http://www.pharmalive.com/images/NAV_primary_left_cap.gif"
                                        width="9" height="28" alt="" />
                                </td>
                                <td align="left">
                                    <a href="http://www.pharmalive.com/" onmouseover="changeImages('BTN_news', 'http://www.pharmalive.com/images/BTN_news-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_news', 'http://www.pharmalive.com/images/BTN_news.gif'); return true;"
                                        title="Today&#39;s News">
                                        <img name="BTN_news" id="BTN_news" src="http://www.pharmalive.com/images/BTN_news.gif"
                                            width="40" height="28" border="0" alt="Today&#39;s News" /></a>
                                </td>
                                <td align="left">
                                    <img id="NAV_primary_SP7" src="http://www.pharmalive.com/images/NAV_primary_SP2.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td align="left">
                                    <a href="http://blog.pharmalive.com" onmouseover="changeImages('BTN_blog', 'http://www.pharmalive.com/images/BTN_blog-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_blog', 'http://www.pharmalive.com/images/BTN_blog.gif'); return true;"
                                        title="Pharma Blog Review">
                                        <img name="BTN_blog" id="BTN_blog" src="http://www.pharmalive.com/images/BTN_blog.gif"
                                            width="40" height="28" border="0" alt="Pharma Blog Review" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP1" src="http://www.pharmalive.com/images/NAV_primary_SP1.gif"
                                        width="6" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/magazines/medad/" onmouseover="changeImages('BTN_med_ad_news', 'http://www.pharmalive.com/images/BTN_med_ad_news-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_med_ad_news', 'http://www.pharmalive.com/images/BTN_med_ad_news.gif'); return true;">
                                        <img name="BTN_med_ad_news" id="BTN_med_ad_news" src="http://www.pharmalive.com/images/BTN_med_ad_news.gif"
                                            width="91" height="28" border="0" alt="Med Ad News" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP2" src="http://www.pharmalive.com/images/NAV_primary_SP2.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/magazines/randd/" onmouseover="changeImages('BTN_RnD_directions', 'http://www.pharmalive.com/images/BTN_RnD_directions-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_RnD_directions', 'http://www.pharmalive.com/images/BTN_RnD_directions.gif'); return true;">
                                        <img name="BTN_RnD_directions" id="BTN_RnD_directions" src="http://www.pharmalive.com/images/BTN_RnD_directions.gif"
                                            width="101" height="28" border="0" alt="R &#38; D Directions" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP3" src="http://www.pharmalive.com/images/NAV_primary_SP3.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/special_reports/" onmouseover="changeImages('BTN_special_reports', 'http://www.pharmalive.com/images/BTN_special_reports-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_special_reports', 'http://www.pharmalive.com/images/BTN_special_reports.gif'); return true;">
                                        <img name="BTN_special_reports" id="BTN_special_reports" src="http://www.pharmalive.com/images/BTN_special_reports.gif"
                                            width="106" height="28" border="0" alt="Special Reports" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP4" src="http://www.pharmalive.com/images/NAV_primary_SP4.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/content/databases/" onmouseover="changeImages('BTN_information_tools', 'http://www.pharmalive.com/images/BTN_information_tools-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_information_tools', 'http://www.pharmalive.com/images/BTN_information_tools.gif'); return true;">
                                        <img name="BTN_information_tools" id="BTN_information_tools" src="http://www.pharmalive.com/images/BTN_information_tools.gif"
                                            width="125" height="28" border="0" alt="Information Tools" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP5" src="http://www.pharmalive.com/images/NAV_primary_SP5.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/enewsletters/newsletters.cfm" onmouseover="changeImages('BTN_enewsletters', 'http://www.pharmalive.com/images/BTN_enewsletters-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_enewsletters', 'http://www.pharmalive.com/images/BTN_enewsletters.gif'); return true;">
                                        <img name="BTN_enewsletters" id="BTN_enewsletters" src="http://www.pharmalive.com/images/BTN_enewsletters.gif"
                                            width="94" height="28" border="0" alt="eNewsletters" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_SP6" src="http://www.pharmalive.com/images/NAV_primary_SP6.gif"
                                        width="7" height="28" alt="" />
                                </td>
                                <td>
                                    <a href="http://www.pharmalive.com/content/conferences/" onmouseover="changeImages('BTN_conferences', 'http://www.pharmalive.com/images/BTN_conferences-over.gif'); return true;"
                                        onmouseout="changeImages('BTN_conferences', 'http://www.pharmalive.com/images/BTN_conferences.gif'); return true;">
                                        <img name="BTN_conferences" id="BTN_conferences" src="http://www.pharmalive.com/images/BTN_conferences.gif"
                                            width="90" height="28" border="0" alt="Conferences" /></a>
                                </td>
                                <td>
                                    <img id="NAV_primary_right_cap" src="http://www.pharmalive.com/images/NAV_primary_right_cap.gif"
                                        width="8" height="28" alt="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top" style="height: 24px;">

                        <script language="javascript" src="http://www.pharmalive.com/includes/javascript/PL_dropdown_STATIC.js"></script>

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <img src="images/pharmalive_header.gif" border="0">
                    </td>
                </tr>
            </table>
            <form name="frmmain" id="frmmain" runat="server">
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
            <div align="left" class="eNewsDefault">
                Please enter your first name, last name, and e-mail address in the fields below.
                <br />
                We will send your password to the e-mail address you provide below.
                <br /><br />
                <table width="100%" border="0">
                    <tr>
                        <td align="left" class="input_caption" width="20%">
                            First Name:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtFirstName" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFirstName"
                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="input_caption">
                            Last Name :
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtLastName" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtLastName"
                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="input_caption">
                            Email Address :
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtE" Width="200" CssClass="formborder"></asp:TextBox>&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtE"
                                runat="server" ErrorMessage=">> required" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                 <br />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="120" CssClass="formButton"
                    OnClick="btnSubmit_Click" CausesValidation="true" />
                <br />
                 <asp:label ID="lblSuccessmessage" runat="server" ForeColor="Green" Visible = false Font-Bold=true Font-Size=Small></asp:label>
                 <br /> 
                <span class="eNewsDefault">If you require assistance, please call Subscriber Services
                    at 888-364-3544, extension 9814. Thank you.</span>
            </div>
            <table width="752" border="0" cellspacing="0" cellpadding="0" style="margin-top: 18px;
                padding-bottom: 24px; background-color: #FFFFFF;">
                <tr>
                    <td class="footer" align="left">
                        <a href="http://www.pharmalive.com/content/engel/about.cfm" class="footer">about us</a>
                        | <a href="http://www.pharmalive.com/content/advertise/advertise.cfm" class="footer">
                            advertise</a> | <a href="http://www.pharmalive.com/content/engel/contact.cfm" class="footer">
                                contact us</a> | <a href="http://www.pharmalive.com/" class="footer">home</a><br>
                        <a href="http://www.pharmalive.com/Content/policy/terms.cfm" class="footer">terms of
                            use</a> | <a href="http://www.pharmalive.com/Content/policy/privacy.cfm" class="footer">
                                privacy</a> | <a href="http://www.pharmalive.com/Content/policy/copyright.cfm" class="footer">
                                    copyright</a> | <a href="http://www.pharmalive.com/content/sitemap/sitemap.cfm" class="footer">
                                        site map</a>
                    </td>
                    <td align="right" valign="top">
                        &copy;2008 Canon Communications Pharmaceutical Media Group
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>
</body>
</html>
