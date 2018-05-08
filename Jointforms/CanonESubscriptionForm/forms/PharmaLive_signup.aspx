<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" Codebehind="PharmaLive_signup.aspx.cs"
    Inherits="CanonESubscriptionForm.forms.PharmaLive_signup" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <title>PharmaLive.com eNewsletter Series</title>
    <link rel="STYLESHEET" type="text/css" href="http://www.pharmalive.com/css/pl.css.cfm?">

<script language="javascript">
    document.location.href = "http://eforms.kmpsgroup.com/paidpub/pharmalive_signup.aspx";
</script>

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

    <script src="http://www.pharmalive.com/includes/javascript/tooltip.js" language="JavaScript1.2"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript">
<!--
function newImage(arg) {
	if (document.images) {
		rslt = new Image();
		rslt.src = arg;
		return rslt;
	}
}
function changeImages() {
	if (document.images && (preloadFlag == true)) {
		for (var i=0; i<changeImages.arguments.length; i+=2) {
			document[changeImages.arguments[i]].src = changeImages.arguments[i+1];
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
// -->
    </script>

    <script type="text/javascript">

function validateForm() 
{
    var allOk = false;
	allOk = 
		(svValidator("Email Address", document.forms[1].txtemail.value) && svValidator("First Name", document.forms[1].txtfirstname.value) && svValidator("Last Name", document.forms[1].txtlastname.value) && 
		svValidator("Company", document.forms[1].txtcompany.value) && svValidator("Job Title", document.forms[1].txttitle.value) && svValidator("Telephone", document.forms[1].txtphone.value) &&
		svValidator("Address", document.forms[1].txtaddress.value) && 
		svValidator("City", document.forms[1].txtcity.value) && svValidator("State", document.forms[1].drpstate.value) && 
		svValidator("Zip", document.forms[1].txtzip.value) && svValidator("Country", document.forms[1].drpcountry.value) &&
		svValidator("Company Description", document.forms[1].user_Business.value) && svValidator("Primary Responsibility", document.forms[1].user_Responsibility.value));

	// Validate zip against state
	if (allOk) 
	{
		if (!document.forms[1].ChkVerify.checked)
		{
		    alert('Please select the checkbox to confirm all the information is accurate.');
		    return false;
		}

        var x = document.forms[1].txtemail.value;

        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(x))
        {	
	        alert('Invalid Email address');
	        allOk = false;
        }
    }
	if (allOk)
    {

       document.forms[2].e.value = document.forms[1].txtemail.value;
       document.forms[2].fn.value = document.forms[1].txtfirstname.value;
       document.forms[2].ln.value = document.forms[1].txtlastname.value;
       document.forms[2].t.value = document.forms[1].txttitle.value;
       document.forms[2].compname.value = document.forms[1].txtcompany.value;
       document.forms[2].adr.value = document.forms[1].txtaddress.value;
       document.forms[2].adr2.value = document.forms[1].txtaddress2.value;
       document.forms[2].city.value = document.forms[1].txtcity.value;
       document.forms[2].state.value = document.forms[1].drpstate.value;
       document.forms[2].zc.value = document.forms[1].txtzip.value;
       document.forms[2].ctry.value = document.forms[1].drpcountry.value;
       document.forms[2].ph.value = document.forms[1].txtphone.value;
       document.forms[2].fax.value = document.forms[1].txtfax.value;
       document.forms[2].user_business.value = document.forms[1].user_Business.value;
       document.forms[2].user_responsibility.value = document.forms[1].user_Responsibility.value;
       document.forms[2].user_Effort_Code.value = document.forms[1].txtpromo.value;
       
        var d = new Date();
        
        var curr_date = d.getDate();
        var curr_month = d.getMonth();
        curr_month++;
        var curr_year = d.getFullYear();

        document.forms[2].user_Verification_Date.value = curr_month + "/" + curr_date + "/" + curr_year;
             
       for(i=0; i<document.forms[1].elements.length; i++)
       {
           if ((document.forms[1].elements[i].name.substring(0,5) == "user_" || document.forms[1].elements[i].name.substring(0,2) == "g_") && document.forms[1].elements[i].type =="checkbox")
           {
               var dynInput = document.createElement("input");
                dynInput.setAttribute("type", "hidden");
                dynInput.setAttribute("id", document.forms[1].elements[i].name);
                dynInput.setAttribute("name", document.forms[1].elements[i].name);

               if (document.forms[1].elements[i].checked)
                    dynInput.setAttribute("value", "y");
               else
                    dynInput.setAttribute("value", "");
                    
               document.forms[2].appendChild(dynInput);
           }
        }
       document.forms[2].submit();
       return false;
    }
    else
        return false;
}

    </script>

</head>
<body onload="preloadImages();">
    <div align="center">
        <div class="pageBorder">
            <table width="974" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="background-color: #FFFFFF;">
                <tr>
                    <td align="left" background="http://www.pharmalive.com/images/TILE_banner_area.gif"
                        style="background-image: url('http://www.pharmalive.com/images/TILE_banner_area.gif')"
                        width="619">
                        <a href="http://www.pharmalive.com/" title="Pharmalive - The Pulse of the Pharmaceutical Industry">
                            <img src="http://www.pharmalive.com/images/logo.gif" width="314" height="100" alt="Pharmalive - The Pulse of the Pharmaceutical Industry"
                                border="0"></a></td>
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
				   if( document.all ) { 
				     browser.type = IE;
				   }
					function downloadtoolbar(p)
					{
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
                    <td colspan="2">
                        <img src="../images/pharmalive_header.gif" border="0"></td>
                </tr>
            </table>
            <form name="frmmain" id="frmmain" runat="server">
                <input type="hidden" id="hidState" runat="server" />
                <input type="hidden" id="hidResponsibility" runat="server" />
                <input type="hidden" id="hidBusiness" runat="server" />
                <input type="hidden" id="user_subdate" runat="server" />
                <table width="752" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="background-color: #FFFFFF;">
                    <tr>
                        <td valign="top" align="left">
                            <div class="eNewsHeader">
                                PharmaLive and Therapeutics Daily eNewsletters</div>
                            <br>
                            <div class="eNewsDefault">
                                Subscribe now to PharmaLive and Therapeutics Daily eNewsletters for quick-to-read
                                insight into events affecting the industry and your business. Our editors compile
                                only relevant news and categorize that information by business and therapeutic area.
                                Our editors also provide a brief summary of each article, as well as a historical
                                perspective, and a link to the complete and related articles – giving you the industry
                                intelligence you need in a format that doesn’t take long to read.
                                <br />
                                <br />
                                To begin receiving this valuable information, select the newsletters you wish to
                                receive from the list below, provide the required demographic information, and click
                                Register.
                                <br />
                                <br />
                                <center>
                                    <table width="680" border="0">
                                        <tr>
                                            <td valign="top" align="center" height="11" width="20">
                                                <input type="checkbox" value="Y" name="g_14092" id="g_14092" runat="server"></td>
                                            <td valign="top" align="left" style="padding-left: 5px; height: 22px;">
                                                <strong><span style="color: #900000">Daily Advantage:</span></strong> <span class="eNewsBody"
                                                    style="color: #000000">concise report of the day’s most important and impactful
                                                    news </span>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                            </div>
                            <center>
                                <table width="680" border="0">
                                    <tr>
                                        <td class="eNewsCurrent" style="width: 677px; text-align: left">
                                            Weekly business news categories</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 677px">
                                            <table class="body" align="left" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top" align="center" width="20" style="height: 22px">
                                                        <input type="checkbox" value="Y" name="g_14094" id="g_14094" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px; height: 22px;">
                                                        <span class="eNewsName">Product Marketing:</span> <span class="eNewsBody">launches,
                                                            withdrawals, joint promotion, and marketing trends</span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14098" id="g_14098" runat="server"></td>
                                                    <td valign="top" style="padding-left: 5px" align="left">
                                                        <span class="eNewsName"><span class="eNewsName">Products, Deals, and Launches:</span>
                                                        </span><span class="eNewsBody">covers collaborations, research, licensing, supply, and
                                                            distribution agreements, and product launches and acquisitions</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14095" id="g_14095" runat="server"></td>
                                                    <td valign="top" style="padding-left: 5px" align="left">
                                                        <span class="eNewsName"><span class="eNewsName">Drugs in Development:</span></span>
                                                        <span class="eNewsBody">covering compounds in the pipeline</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14096" id="g_14096" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Drug Approvals and Filings:</span> </span>
                                                        <span class="eNewsBody"><span class="eNewsBody">covering potential medicines that have
                                                            been filed or approved in the past week throughout the world</span> </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14097" id="g_14097" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Trial Results:</span></span> <span
                                                            class="eNewsBody"><span class="eNewsBody">covering compounds through all phases of clinical
                                                                trials</span> </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14093" id="g_14093" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">PharmaBlog Review:</span></span>
                                                        <span class="eNewsBody"><span class="eNewsBody">insight into blog postings and industry
                                                            issues</span> </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eNewsCurrent" style="text-align: left; width: 677px;">
                                            Weekly therapeutic newsletters cover product development, clinical trials, and sales
                                            and marketing, categorized by:</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 677px">
                                            <table class="body" align="left" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top" align="center" width="20" style="height: 22px">
                                                        <input type="checkbox" value="Y" name="g_14100" id="g_14100" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px; height: 22px;">
                                                        <span class="eNewsName">Oncology</span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14102" id="g_14102" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Cardiovascular</span></span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14103" id="g_14103" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Pain & Inflammation</span></span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14099" id="g_14099" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Central Nervous System</span></span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14101" id="g_14101" runat="server"></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><span class="eNewsName">Infectious Disease</span></span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eNewsCurrent" style="text-align: left; width: 677px;">
                                            Advance Newsletters provide a preview of the top news stories found in the upcoming
                                            edition of the magazine. Newsletters include:</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 677px">
                                            <table class="body" align="left" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14104" id="g_14104" runat="server" /></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><em>Med Ad News</em> Advance</span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_14105" id="g_14105" runat="server" /></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><em>R&amp;D Directions</em> Advance</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eNewsCurrent" style="text-align: left; width: 677px;">
                                           eBulletins provide insight into select topics affecting pharmaceutical research, marketing, and sales. eBulletin newsletters include:</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 677px">
                                            <table class="body" align="left" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_16201" id="g_16201" runat="server" /></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><em>Med Ad News eBulletin:</em></span> <span class="eNewsBody"
                                                    style="color: #000000"> focuses on issues affecting the marketing and sales of medicines</span></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" height="11" width="20">
                                                        <input type="checkbox" value="Y" name="g_16585" id="g_16585" runat="server" /></td>
                                                    <td valign="top" align="left" style="padding-left: 5px">
                                                        <span class="eNewsName"><em>R&amp;D Directions eBulletin:</em></span> <span class="eNewsBody"
                                                    style="color: #000000">  focuses on issues affecting the research and development of medicines</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellspacing="4" cellpadding="0" width="680">
                                    <tr>
                                        <td class="eNewsCurrent" style="text-align: left">
                                            Information</td>
                                    </tr>
                                </table>
                                <table width="680" border="0">
                                    <tr>
                                        <td align="left" class="input_caption" width="80">
                                            First Name: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtfirstname" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Last Name: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtlastname" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Title: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txttitle" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Company Name: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtcompany" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Address 1: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtaddress" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Address 2:</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtaddress2" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            City: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtcity" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            State: *</td>
                                        <td align="left">
                                            <asp:DropDownList ID="drpstate" runat="server" CssClass="formborder">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Zip: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtzip" Width="200" CssClass="formborder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Country: *</td>
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Telephone: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtphone" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Fax:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtfax" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Email Address: *</td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtemail" Width="200" CssClass="formborder"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption">
                                            Promotion Code:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtpromo" Width="200" CssClass="formborder" ReadOnly></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption" style="line-height: 12px;">
                                            Company Description:*</td>
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
                                                <asp:ListItem Value="99">&nbsp;&nbsp;Other support or service
                                                        company</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="input_caption" style="line-height: 12px;">
                                            Primary Responsibility:*</td>
                                        <td align="left">
                                            <asp:DropDownList ID="user_Responsibility" runat="server" CssClass="formborder">
                                                <asp:ListItem Value="">Your primary area of responsibility...</asp:ListItem>
                                                <asp:ListItem Value="09">&nbsp;&nbsp;R&D management</asp:ListItem>
                                                <asp:ListItem Value="10">&nbsp;&nbsp;Senior management</asp:ListItem>
                                                <asp:ListItem Value="34">&nbsp;&nbsp;Finance management</asp:ListItem>
                                                <asp:ListItem Value="38">&nbsp;&nbsp;Business strategy</asp:ListItem>
                                                <asp:ListItem Value="12">&nbsp;&nbsp;Product management</asp:ListItem>
                                                <asp:ListItem Value="23">&nbsp;&nbsp;Marketing,advertising or promotion management</asp:ListItem>
                                                <asp:ListItem Value="14">&nbsp;&nbsp;Sales management</asp:ListItem>
                                                <asp:ListItem Value="15">&nbsp;&nbsp;Agency account management</asp:ListItem>
                                                <asp:ListItem Value="19">&nbsp;&nbsp;Marketing services</asp:ListItem>
                                                <asp:ListItem Value="20">&nbsp;&nbsp;Media management (incl. directors/planners)</asp:ListItem>
                                                <asp:ListItem Value="24">&nbsp;&nbsp;Medical director/associate medical director</asp:ListItem>
                                                <asp:ListItem Value="25">&nbsp;&nbsp;Clinical trials management</asp:ListItem>
                                                <asp:ListItem Value="26">&nbsp;&nbsp;Clinical/drug research</asp:ListItem>
                                                <asp:ListItem Value="28">&nbsp;&nbsp;Clinical monitoring/CRC/CRA</asp:ListItem>
                                                <asp:ListItem Value="31">&nbsp;&nbsp;Clinical documentation preparation</asp:ListItem>
                                                <asp:ListItem Value="27">&nbsp;&nbsp;Regulatory affairs</asp:ListItem>
                                                <asp:ListItem Value="22">&nbsp;&nbsp;Quality control</asp:ListItem>
                                                <asp:ListItem Value="33">&nbsp;&nbsp;Drug safety</asp:ListItem>
                                                <asp:ListItem Value="32">&nbsp;&nbsp;Project management</asp:ListItem>
                                                <asp:ListItem Value="29">&nbsp;&nbsp;Academic research or professor</asp:ListItem>
                                                <asp:ListItem Value="30">&nbsp;&nbsp;Data management or analysis</asp:ListItem>
                                                <asp:ListItem Value="35">&nbsp;&nbsp;Licensing</asp:ListItem>
                                                <asp:ListItem Value="36">&nbsp;&nbsp;Manufacturing</asp:ListItem>
                                                <asp:ListItem Value="37">&nbsp;&nbsp;IT management</asp:ListItem>
                                                <asp:ListItem Value="99">&nbsp;&nbsp;Other functions</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" align="left" class="input_caption">
                                            <br />
                                            To change your email address, send an email to <a href="mailto:pharmalive@kmpsgroup.com">pharmalive@kmpsgroup.com</a>.
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="input_caption">
                                            <br />
                                            <input type="checkbox" name="ChkVerify" />
                                            I have confirmed that information above regarding my subscription is accurate.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <br>
                                            <asp:Button ID="btnSubmit" runat="server" Text=" Register " Width="120" CssClass="formButton" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            * = Required Field</td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
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
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
    <form name="frmsub" id="frmsub" action="http://emailactivity.ecn5.com/engines/SO_multigroup_subscribe.aspx">
        <input type="hidden" name="e" value="" />
        <input type="hidden" name="fn" value="" />
        <input type="hidden" name="ln" value="" />
        <input type="hidden" name="t" value="" />
        <input type="hidden" name="compname" value="" />
        <input type="hidden" name="adr" value="" />
        <input type="hidden" name="adr2" value="" />
        <input type="hidden" name="city" value="" />
        <input type="hidden" name="state" value="" />
        <input type="hidden" name="zc" value="" />
        <input type="hidden" name="ctry" value="" />
        <input type="hidden" name="ph" value="" />
        <input type="hidden" name="fax" value="" />
        <input type="hidden" name="s" value="S" />
        <input type="hidden" name="f" value="html" />
        <input type="hidden" name="sfID" value="741" />
        <input type="hidden" name="SFmode" value="manage" />
        <input type="hidden" name="user_business" value="" />
        <input type="hidden" name="user_responsibility" value="" />
        <input type="hidden" name="user_Effort_Code" value="" />
        <input type="hidden" name="user_Verification_Date" value="" />
    </form>
</body>
</html>
