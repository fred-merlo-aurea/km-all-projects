<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Entrepreneur_signup.aspx.cs"
    Inherits="CanonESubscriptionForm.forms.Entrepreneur_signup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="ctl00_ctl00_Head1">
    <title>Entrepreneur Newsletter Opt-in</title>
    <meta content="" name="description" />
    <meta content="" name="keywords" />
    <meta name="Robots" content="index,follow" />
    <meta name="googlebot" content="index,follow" />
    <meta name="verify-v1" content="VytPswCXjUV52BPf9/JrqfKb64cLEpXc4JqfxtlVlME=" />
    <link href="http://www.entrepreneur.com/css/newglobal.css" type="text/css" rel="stylesheet" />
    <link href="http://www.entrepreneur.com/css/home.css" type="text/css" rel="stylesheet" />
    <script src="http://www.entrepreneur.com/scripts/MiscFunctions.js" type="text/javascript" language="javascript"></script>
    <script src="http://www.entrepreneur.com/scripts/SearchFunctions.js" type="text/javascript" language="javascript"></script>
	<script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript" language="JavaScript">
var AAMB1 = "";
var AAMB2 = "";
var AAMB3 = "";
var aamRnd = Math.round(Math.random() * 100000000);
adserver = "http://atlas.entrepreneur.com/bservers";
site = "/site=Entrepreneur.com";
target = "/area=homepage";
allAdTags = "/AAMALL/acc_random=" + aamRnd + "/pageid=" + aamRnd;
ad1 = "/AAMB1/" + site + target + "/aamsz=Leaderboard";
ad2 = "/AAMB2/" + site + target + "/aamsz=Island";
document.write('<SCR' + 'IPT SRC="' + adserver + allAdTags + ad1 + ad2 + '?" type="text/JavaScript" language="JavaScript">');
document.write('</SCR' + 'IPT>');
    </script>
<script type="text/javascript">

function validateForm() 
{
    var allOk = false;
	allOk = 
		(svValidator("Email Address", document.forms[0].txtemail.value) && svValidator("First Name", document.forms[0].txtfirstname.value) && svValidator("Last Name", document.forms[0].txtlastname.value) && 
		svValidator("Zip", document.forms[0].txtzip.value));

	if (allOk) 
	{
        var x = document.forms[0].txtemail.value;

        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(x))
        {	
	        alert('Invalid Email address');
	        allOk = false;
        }
    }
	if (allOk)
    {

       document.forms[1].e.value = document.forms[0].txtemail.value;
       document.forms[1].fn.value = document.forms[0].txtfirstname.value;
       document.forms[1].ln.value = document.forms[0].txtlastname.value;
       document.forms[1].zc.value = document.forms[0].txtzip.value;
       
       for(i=0; i<document.forms[0].elements.length; i++)
       {
           if (document.forms[0].elements[i].name.substring(0,2) == "g_" && document.forms[0].elements[i].type =="checkbox")
           {
               var dynInput = document.createElement("input");
                dynInput.setAttribute("type", "hidden");
                dynInput.setAttribute("id", document.forms[0].elements[i].name);
                dynInput.setAttribute("name", document.forms[0].elements[i].name);

               if (document.forms[0].elements[i].checked)
                    dynInput.setAttribute("value", "y");
               else
                    dynInput.setAttribute("value", "");
                    
               document.forms[1].appendChild(dynInput);
           }
        }
       document.forms[1].submit();
       return false;
    }
    else
        return false;
}

    </script>
</head>
<body>
<div style="Z-INDEX: -100; RIGHT: 0px; POSITION: absolute; TOP: 0px">
<script language="JavaScript" src="http://www.entrepreneur.com/Common/s_code.js">
<!--
//-->
</script>
<script language="JavaScript">
<!-- 
s.pageName="newsletter reopt"
s.channel="newsletter reopt"
s.prop2="" 
s.prop4=document.location.href;
/* E-commerce Variables */
s.events="event5"
/************* DO NOT ALTER ANYTHING BELOW THIS LINE ! **************/ var s_code=s.t();if(s_code)document.write(s_code)//-->
</script>
<!-- End SiteCatalyst code version: H.2. -->
</div>
    <div id="holder">
        
<div id="header">
	<div style="width:325px; float:left;">
	<a title="Business &amp; Small Business Home" href="/"><img id="Img1" alt="Business &amp; Small Business Home" src="http://www.entrepreneur.com/graphics/entlogo.gif" style="margin-top:15px;" width="315" height="53" /></a>
	</div>
	<div style="padding:5px 0 0 0; text-align:right; margin:0;">
	<a title="Business Opportunities" href="/business-opportunities/index.html">Franchises for Sale</a>|<a href="http://www.womenentrepreneur.com/" target="_blank">Women Entrepreneur</a>|<a href="https://www.entrepreneur.com/subs/order/?Key=ABG" target="_blank">Subscribe</a>|<a href="/newsletters/signup.html">Newsletters</a>|<a href="http://www.entrepreneur.com/offers/coreg/2.htm">Special Offers</a>
	</div>

	<div style="padding-top:10px;" align="right"> 
	
<script type="text/javascript" language="javascript">
    function OnFocus(elementId)
    { 
      document.getElementById(elementId).className = "gSrchNoBgr";
    }

    function OnBlur(elementId)
    {
       var textValue = document.getElementById(elementId).value;
       if (textValue.length == 0)
       {
          document.getElementById(elementId).className = "gSrchBgr";
       }
       else
          document.getElementById(elementId).className = "gSrchNoBgr";
    }
</script>

<div id="ctl00_ctl00_NewHeader1_gsearchnew_plSearch" onkeypress="return fireDefaultButton(event,'ctl00_ctl00_NewHeader1_gsearchnew__btnSearch');">
	
	<span>Search</span>
    <input type="hidden" name="ctl00$ctl00$NewHeader1$gsearchnew$cx" id="ctl00_ctl00_NewHeader1_gsearchnew_cx" value="013574105172325703311:xhkof7qpqp" />
    <input type="hidden" name="ctl00$ctl00$NewHeader1$gsearchnew$cof" id="ctl00_ctl00_NewHeader1_gsearchnew_cof" value="FORID:9" />
    <input name="ctl00$ctl00$NewHeader1$gsearchnew$q" type="text" maxlength="512" id="ctl00_ctl00_NewHeader1_gsearchnew_q" class="gSrchBgr" size="48" onfocus="OnFocus('ctl00_ctl00_NewHeader1_gsearchnew_q')" onblur="OnBlur('ctl00_ctl00_NewHeader1_gsearchnew_q')" />
    <input type="image" name="ctl00$ctl00$NewHeader1$gsearchnew$_btnSearch" id="ctl00_ctl00_NewHeader1_gsearchnew__btnSearch" align="absmiddle" src="/graphics/search.gif" style="border-width:0px;padding-left:5px;" />

	

</div>
  
	</div>
</div>
        



<div id="topnav">
<ul>
	<li class="nolt"></li>
	<li class="press"><a class='press' href='/' target='_self' title='Business &amp; Small Business Home'>Home</a></li><li class=""><a  href='/ask/index.html' target='_self' title='Business Help and How-To Guides'>Ask Entrepreneur</a></li><li class=""><a  href='/grow/index.html' target='_self' title='Grow Your Business'>Grow Your Biz</a></li><li class=""><a  href='/businessideas/index.html' target='_self' title='Business Ideas'>Business Ideas</a></li><li class=""><a  href='/franchiseopportunities/index.html' target='_self' title='Franchises and Business Opportunities'>Franchises &amp; Opportunities</a></li><li class=""><a  href='/video/index.html' target='_self' title='Video'>Video</a></li><li class="ms">

<a class="ms" href='/tools/index.html'>Tools &amp; Services</a>
<!-- Fly Out Menu -->
<ul class='mshover'>
<li>
<div class='msbody' style='margin:0 15px 0 0;'>
<b style='font-size:11px; color:#c33;'>Get more from Office Live Small Business</b><br />
<img src='/graphics/startupideas/orange-bullet.gif' align='left' /><a href='http://atlas.entrepreneur.com/accipiter/adclick/CID=00001ae90000000000000000/relocate=http://clk.atdmt.com/MRT/go/ntrpraub1580000013mrt/direct/01/' target='_blank' class='scard'>Take your business online</a>
<img src='/graphics/startupideas/orange-bullet.gif' align='left' /><a href='http://atlas.entrepreneur.com/accipiter/adclick/CID=00001ae90000000000000000/relocate=http://clk.atdmt.com/MRT/go/ntrpraub1580000021mrt/direct/01/' target='_blank' class='scard'>Attract new customers</a>
<img src='/graphics/startupideas/orange-bullet.gif' align='left' /><a href='http://atlas.entrepreneur.com/accipiter/adclick/CID=00001ae90000000000000000/relocate=http://clk.atdmt.com/MRT/go/ntrpraub1580000012mrt/direct/01/' target='_blank' class='scard'>Stay on top of your business</a>
<div style='padding:0 5px 0 0;'>

<script language='javascript' type='text/javascript'>
document.write('<SCR');
document.write('IPT SRC="http://atlas.entrepreneur.com/jserver/acc_random=' + aamRnd + site + target + '/pos=MSTOOLS/aamsz=88x31' + '/pageid=' + aamRnd + '">');
document.write('</SCR');
document.write('IPT>');
</script>
<script language='javascript' type='text/javascript'>
document.write('<SCR');
document.write('IPT SRC="http://atlas.entrepreneur.com/jserver/acc_random=' + aamRnd + site + target + '/pos=MSOL_Nav1x1/aamsz=1x1' + '/pageid=' + aamRnd + '">');
document.write('</SCR');
document.write('IPT>');
</script>
</div>
</div>
</li>
</ul>
</li>
<li class="">
<a class="" target='_blank' href='http://econnect.entrepreneur.com/?cam=connect&cid=topnav'>Community</a>
</li>
  
	<li class="nort"></li>
</ul>    

</div>


        <div id="ctl00_ctl00_BannerAdHome" class="bannerhome">
            <script type="text/javascript" language="JavaScript"> document.writeln(AAMB1); </script>

        </div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="1" width="100%">
                <tr>
                    <td valign="top" style="padding: 0 0px 0 0;">
                        <form name="frmmain" id="frmmain" runat="server">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                style="background-color: #f2f1f1;">
                                <tr>
                                    <td valign="top" align="left">
                                        <center>
                                            <table width="400" border="0">
                                                <tr>
                                                    <td colspan="2" style="text-align: left" class="blogtitle">
                                                        <h2>
                                                            Entrepreneur Newsletters</h2>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: left">
                                                        Sign up for any of the Entrepreneur newsletters that best fit your needs by entering
                                                        in your information and clicking the checkbox next to the Enewsletter(s) you are
                                                        interested in.<br>
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="input_caption" width="120">
                                                        Email Address: *</td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtemail" Width="200" CssClass="formborder"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
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
                                                        Zip: *</td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtzip" Width="200" CssClass="formborder"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <br />
                                                        <table class="body" align="left" cellspacing="0" cellpadding=5 border="0" width="100%">
                                                            <tr>
                                                                <td valign="top" align="center" width="20" style="height: 22px">
                                                                    <input type="checkbox" value="Y" name="g_15242" id="g_15242" runat="server"></td>
                                                                <td valign="top" align="left" style="padding-left: 5px; height: 22px;">
                                                                    <span class="oppOn">Starting a Business</span><br />
                                                                    <span class="displayrank">Get the information you need to get your business idea off
                                                                        the ground.</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" height="11" width="20">
                                                                    <input type="checkbox" value="Y" name="g_15239" id="g_15239" runat="server"></td>
                                                                <td valign="top" style="padding-left: 5px" align="left">
                                                                    <span class="oppOn">Sales & Marketing:</span>
                                                                    <br />
                                                                    <span class="displayrank">From management to money, get information on the topics that
                                                                        are most pressing to established entrepreneurs. </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" height="11" width="20">
                                                                    <input type="checkbox" value="Y" name="g_15229" id="g_15229" runat="server"></td>
                                                                <td valign="top" style="padding-left: 5px" align="left">
                                                                    <span class="oppOn">Growing a Business:</span><br />
                                                                    <span class="displayrank" >From management to money, get information on the topics that
                                                                        are most pressing to established entrepreneurs.</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" height="11" width="20">
                                                                    <input type="checkbox" value="Y" name="g_17409" id="g_17409" runat="server"></td>
                                                                <td valign="top" align="left" style="padding-left: 5px">
                                                                    <span class="oppOn">Online Business:</span><br />
                                                                    <span class="displayrank">Find advice and insight on growing your business online--or
                                                                        starting your own online enterprise</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" height="11" width="20">
                                                                    <input type="checkbox" value="Y" name="g_15226" id="g_15226" runat="server"></td>
                                                                <td valign="top" align="left" style="padding-left: 5px">
                                                                    <span class="oppOn">Franchise News:</span><br />
                                                                    <span class="displayrank">Interested in buying a franchise? We get the scoop from franchising
                                                                        insiders to give you a monthly look at the latest trends, news and ideas.</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" height="11" width="20">
                                                                    <input type="checkbox" value="Y" name="g_17408" id="g_17408" runat="server"></td>
                                                                <td valign="top" align="left" style="padding-left: 5px">
                                                                    <span class="oppOn">Business Book Sampler:</span><br />
                                                                    <span class="displayrank">Your monthly guide to what's new from Entrepreneur Press.
                                                                        Read exclusive excerpts, author interviews and more.</span>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                            </table>
                        </form>
                    </td>
                </tr>
            </table>

            <script type="text/javascript" language="JavaScript"> document.writeln(AAMB2); </script>

        </div>

        <script language="javascript" type="text/javascript"> <!-- 
var AAMB11 = ""; 
var AAMB12 = ""; 
var AAMB13 = ""; 
var AAMB14 = ""; 
var AAMB15 = ""; 
//--> 
<!--   Hide from old browsers 
// Individual tags for each ad request - increment the adx variable name and the AAMBx parameter. 
ad11 = "/AAMB11" + site + target + "/pos=TL1/aamsz=textlink";
ad12 = "/AAMB12" + site + target + "/pos=TL2/aamsz=textlink";
ad13 = "/AAMB13" + site + target + "/pos=TL3/aamsz=textlink";
ad14 = "/AAMB14" + site + target + "/pos=TL4/aamsz=textlink";
ad15 = "/AAMB15" + site + target + "/pos=TL5/aamsz=textlink";
// bserver ad call - insert the adx variables 
document.write('<SCR' + 'IPT SRC="' + adserver + allAdTags + ad11 + ad12 + ad13 + ad14 + ad15 + '?" type="text/JavaScript" language="JavaScript">');
document.write('</SCR' + 'IPT>'); 
// End Hide --> 
        </script>

        <div id="sponsoredlinks">
            <div class="subhead">
                Sponsored Links</div>
            <ul>

                <script type="text/javascript" language="JavaScript"> document.writeln(AAMB11); </script>

                <script type="text/javascript" language="JavaScript"> document.writeln(AAMB12); </script>

                <script type="text/javascript" language="JavaScript"> document.writeln(AAMB13); </script>

                <script type="text/javascript" language="JavaScript"> document.writeln(AAMB14); </script>

                <script type="text/javascript" language="JavaScript"> document.writeln(AAMB15); </script>

            </ul>
            <div id="footer">
                <a title="Business" href="/">Small Business - Home</a> | <a title="Small Business Search"
                    href="http://www.smallbizsearch.com/" target="_blank">Small Business Search</a>
                | <a href="http://www.womenentrepreneur.com/" target="_blank">Women Entrepreneurs</a>
                | <a title="Business Startup Guides" href="http://www.smallbizbooks.com/cgi-bin/SmallBizBooks/index.html?cam=Ecom&amp;cid=Footer&amp;size=TL"
                    target="_blank">Business Startup Guides</a> | <a title="Business Bookstore" href="http://www.entrepreneurpress.com/?cam=Ecom&amp;cid=Footer&amp;size=TL"
                        target="_blank">Business Bookstore</a> | <a title="Negocio y Pequeña Empresa" href="http://www.entrepreneurenespanol.com/?cam=Ecom&amp;cid=Footer&amp;size=TL"
                            target="_blank">En Español</a> | <a title="Entrepreneur Magazine Subscriptions" href="https://www.entrepreneur.com/subs/order/?Key=ABR"
                                target="_blank">Magazine Subscriptions</a><br />
                <a title="Contact Entrepreneur.com" href="/contact-us/index.html">Contact Us</a>
                | <a title="Entrepreneur.com Help" href="/help/index.html">Help</a> | <a title="Business Newsletters"
                    href="/newsletters/signup.html">Newsletters</a> | <a title="Entrepreneur.com Affiliate Programs"
                        href="/affiliatesprograms/index.html">Affiliate Programs</a> | <a title="Entrepreneur Advertising"
                            href="/mediakit/index.htm" target="_blank">Advertising Info</a> | <a title="Press Releaese"
                                href="/pressrelease/index.html">Press Releases</a> | <a title="Special Offers" href="/offers/coreg/2.htm">
                                    Special Offers</a> | <a title="Entrepreneur.com RSS Feeds" href="/feeds/index.html">
                                        RSS Feeds</a> | <a href="/sitemap/index.html">Site Map</a>
                | <a title="Entrepreneur Magazine Reprints" href="/reprintspermissions.html">Reprints
                    &amp; Permissions</a>
                <br />
                <div style="margin-top: 5px;">
                    Copyright © <span id="ctl00_ctl00_footer_lblYear">2008</span> Entrepreneur.com,
                    Inc. All rights reserved. <a href="/privacypolicy/index.html">Privacy Policy</a>
                </div>
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
            <input type="hidden" name="sfID" value="813" />
            <input type="hidden" name="SFmode" value="manage" />
        </form>
</body>
</html>
