<%@ Page Language="c#" Inherits="ecn.publisher.main.Edition.Preview" CodeBehind="Preview.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Welcome to ECN Digital Editions</title>
    <link href="http://www.ecndigitaledition.com/<%= DETheme%>/stylesheet.css" rel="stylesheet"
        type="text/css" />

    <script language="javascript" src="http://www.ecndigitaledition.com/Scripts/ed.js"></script>

    <script language="javascript" src="http://www.ecndigitaledition.com/Scripts/utils.js"></script>

    <script language="javascript" src="http://www.ecndigitaledition.com/Scripts/xmldom.js"></script>

    <script language="JavaScript" src="http://www.ecndigitaledition.com/Scripts/jsTree.js"></script>

    <script language="JavaScript" src="http://www.ecndigitaledition.com/Scripts/browser.js"></script>

    <style type='text/css'>
        * html, * html body
        {
            overflow-y: hidden;
            height: 100%;
        }
    </style>
</head>
<body id="bdy" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" onresize="window.setTimeout('sizeFrame(true)', 500);"
    onkeypress="return CheckKey(event);">
    <form method="post" runat="Server" height="100%" id="Form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" enablepagemethods="true" />
        <asp:Panel ID="pnlEdition" runat="Server" Visible="False">
            <asp:Label ID="lblEditionID" runat="Server"></asp:Label>
            <asp:Label ID="lblIsSecured" runat="server"></asp:Label>
            <div id="hLine">
            </div>
            <div id="frameContent" style="display: block; width: 100%; height: 34px;" class="lg_bkgnd">
                <table id="Table11" cellspacing="0" cellpadding="0" border='0'>
                    <tr height="34">
                        <td align="left" valign="top" width="150">
                            <div style="padding: 0; margin: 0; width: 150px; overflow: hidden; text-align: center;">
                                <asp:HyperLink ID="hlLogo" runat="Server"></asp:HyperLink></div>
                        </td>
                        <td align="left" width="7">
                            <div class="spacer" style="margin-left: 4px">
                            </div>
                        </td>
                        <td valign="middle" id="pageViews">
                            <div>
                                <a href="javascript:ShowThumbnails();" id="thumbnails" title="Thumbnail View" style="margin: 4px 0 0 4px;">
                                    Show Thumbnails</a>
                                <asp:PlaceHolder ID="nav1Page" runat="Server"><a href="javascript:resetPage(true)"
                                    id="singlePageView" title="Single page view" style="margin: 4px 0 2px 2px;">Single
                                    Page View</a> </asp:PlaceHolder>
                                <asp:PlaceHolder ID="nav2Page" runat="Server"><a href="javascript:resetPage(true)"
                                    id="twoPageView" title="Two page view" style="margin: 4px 0 2px 2px;">Two page view</a>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="nav" runat="Server"><a href="javascript:Edition.setpref(1);"
                                    id="singlePageView" title="Single page view" style="margin: 4px 0 2px 2px;">Single
                                    page view</a> <a href="javascript:Edition.setpref(2);" id="twoPageView" title="Two page view"
                                        style="margin: 4px 0 2px 2px;">Two page</a> </asp:PlaceHolder>
                            </div>
                        </td>
                        <td valign="middle" align="left">
                            <div class="spacer" style="margin: 0">
                            </div>
                        </td>
                        <td style="padding-right: 5px" valign="middle" align="left" width="110" class="firstLast">
                            <div id="arrowButtons" class="highlightOff">
                                <a id="first" href="javascript:Edition.Nav(Edition.Page.First);" title="First Page">
                                    First Page</a> <a id="previous" href="javascript:Edition.Nav(Edition.Page.Previous);"
                                        title="Previous Page">Previous Page</a> <a id="next" href="javascript:Edition.Nav(Edition.Page.Next);"
                                            title="Next Page">Next Page</a> <a id="last" href="javascript:Edition.Nav(Edition.Page.Last);"
                                                title="Last Page">Last Page</a></div>
                        </td>
                        <td class="text" width="13">
                            P.
                        </td>
                        <td nowrap width="70">
                            <asp:TextBox ID="txtpageno" MaxLength="3" runat="Server"></asp:TextBox>&nbsp;/
                            <asp:Label ID="lblTotalPages" runat="Server"></asp:Label>
                        </td>
                        <td align="center" width="26">
                            <a href="javascript:btngo();">
                                <img height="12" alt="Go to page" src="http://www.ecndigitaledition.com/images/Navigation_sm5_GO.gif"
                                    width="22" border='0'></a>
                        </td>
                        <td valign="middle" align="left" width="15">
                            <div class="spacer" style="margin-left: 10px">
                            </div>
                        </td>
                        <td width="110" align="center">
                            <div style="width: 110px;">
                                <div id="zoomOutLine" class="highlightOff">
                                    <div class="container">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td valign="middle" align="center" width="25" height="33" class="plusMinusZoom">
                                                    <a href="javascript:Edition.zoom(0);" class="minusZoom" title="Zoom out" style="margin-left: 3px;">
                                                        Zoom out</a>
                                                </td>
                                                <td valign="middle" align="center" width="51">
                                                    <img id="zb" src="http://www.ecndigitaledition.com/images/zoom1.gif" usemap="#Map5"
                                                        border='0'>
                                                </td>
                                                <td valign="middle" align="center" width="25" class="plusMinusZoom">
                                                    <a href="javascript:Edition.zoom(1);" class="plusZoom" title="Zoom in">Zoom in</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td valign="middle" width="17" align="center">
                            <div class="spacer" style="margin: 0 8px">
                            </div>
                        </td>
                        <td valign="middle" align="center" width="25" class="fitPage">
                            <div id="fitToPage" class="highlightOff">
                                <a href="javascript:Edition.zoombar(getdefaultsize());">Fit Page</a></div>
                        </td>
                        <td valign="middle" align="left" width="20">
                            <div class="spacer" style="margin-left: 8px">
                            </div>
                        </td>
                        <td valign="middle" align="left" width="120" class="emailPrint">
                            <div class="highlightOff" id="printEmailCell">
                                <a onclick="javascript:alert('Disabled in Preview mode');" id="print" title="Print Options">
                                    Print</a> <a onclick="javascript:alert('Disabled in Preview mode');" id="emailBtn"
                                        title="Email to a Friend">Email</a> <a target="_blank" id="download" title="Download"
                                            runat="server">Download</a></div>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table cellpadding="0" cellspacing="0" width="100%" border='0' height="100%">
                    <tr>
                        <td valign="bottom" style="background: url(images/tiler_184.gif) top left repeat-y;"
                            width="35">
                            <div id="wholeScroll">
                                <!-- :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: -->
                                <!-- JUST TOOLBAR -->
                                <div id="divTOOLS">
                                    <div id="expandIcon">
                                    <a href="javascript:void(0);" class="openClose" style="display: none;" id="openBtn"
                                        onclick="javascript:expandSideBar();" title="open">
                                    </a>
                                    </div>     
                                        
                                    <div id="closeIcon">    
                                    <a href="javascript:void(0);" class="openClose" style="display: block;" id="closeBtn"
                                        onclick="javascript:closeSideBar();" title="close">
                                    </a>
                                    </div>    
                                        
                                    <div class="toolButtons">
                                        <div id="tInfo" class="unders">
                                            <a href="javascript:void(0);" onclick="setwhichBtn('infoBtn');expandSideBar();initSubscForm();"
                                                title="Information">Information</a></div>
                                        <div id="divOverFiller" style="display: none;">
                                        </div>
                                        <div id="divOverInfo" class="overs" style="display: block;">
                                            
                                            <a href="javascript:void(0);" onclick="javascript:closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/<%= DETheme%>/information_on.gif" border='0'
                                                    title="Information">
                                            </a>
                                        </div>
                                        
                                        <div id="tPages" class="unders">
                                            <a href="javascript:void(0);" onclick="createSPThumbnails();setwhichBtn('pagesBtn');expandSideBar();"
                                                title="Pages">Pages</a></div>
                                        <div id="divOverPages" class="overs" style="display: none;">
                                            <a href="javascript:void(0);" onclick="closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/<%= DETheme%>/pages_on.gif" border='0' title="Pages"></a></div>
                                        <div id="tTableOfContents" class="unders">
                                            <asp:PlaceHolder ID="pltoc" runat="Server"><a href="javascript:void(0);" onclick="setwhichBtn('tableOfContentsBtn');expandSideBar();"
                                                title="Table of Contents">Table of Contents</a></asp:PlaceHolder>
                                        </div>
                                        <div id="divOverTableOfContents" class="overs" style="display: none;">
                                            <a href="javascript:void(0);" onclick="closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/<%= DETheme%>/tableOfContents_on.gif" border='0'
                                                    title="Table of Contents"></a></div>
                                        <div id="tSearch" class="unders">
                                            <a href="javascript:void(0);" name="aSearch" onclick="setwhichBtn('searchBtn');expandSideBar();focusSearch();"
                                                title="Search">Search</a></div>
                                        <div id="divOverSearch" class="overs" style="display: none;">
                                            <a href="javascript:void(0);" onclick="closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/<%= DETheme%>/search_on.gif" border='0' title="Search"></a></div>
                                        <div id="tLinks" class="unders">
                                            <a href="javascript:void(0);" onclick="setwhichBtn('linksBtn');expandSideBar();"
                                                title="Links">Links</a></div>
                                        <div id="divOverLinks" class="overs" style="display: none;">
                                            <a href="javascript:void(0);" onclick="closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/<%= DETheme%>/links_on.gif" border='0' title="Links"></a></div>
                                        <div id="tBackIssues" style="display: none" class="unders">
                                            <asp:PlaceHolder ID="plbackissues" runat="Server"><a href="javascript:void(0);" onclick="setwhichBtn('backIssuesBtn');expandSideBar();"
                                                title="Back Issues">Back Issues</a></asp:PlaceHolder>
                                        </div>
                                        <div id="divOverBackIssues" class="overs" style="display: none;">
                                            <a href="javascript:void(0);" onclick="closeSideBar();">
                                                <img src="http://www.ecndigitaledition.com/images/backIssues_on.gif" border='0' title="Back Issues"></a></div>
                                    </div>
                                    <!-- END toolButtons -->
                                </div>
                                <!-- END divTOOLS -->
                                <div id="divEMPTY" style="display: none;">
                                </div>
                                <div id="divTABS" style="display: block;">
                                    <!--<div id="minimize"><a href="javascript:void(0);"><img src="http://www.ecndigitaledition.com/images/minimize_13.gif" title="Minimize" onClick="closeSideBar();"></a></div>-->
                                    <!-- HEADERS ABOVE SCROLL AREA-->
                                    <div id="aboveContainer">
                                        <div id="abvInfo" class="aboveScroll">
                                            Information</div>
                                        <div id="abvPages" class="aboveScroll">
                                            Pages</div>
                                        <div id="abvTOC" class="aboveScroll">
                                            Table of Contents</div>
                                        <div id="abvSearch" class="aboveScroll">
                                            Search For:<br />
                                            <input id="txtsearch" style="font-size: 10px; width: 120px" type="text" maxlength="25"
                                                name="txtsearch">
                                            &nbsp;<a href="javascript:search();"><img src="http://www.ecndigitaledition.com/images/search-go.gif"
                                                border='0'></a>
                                        </div>
                                        <div id="abvLinks" class="aboveScroll">
                                            Links</div>
                                        <div id="abvBackIssues" class="aboveScroll">
                                            Back Issues</div>
                                    </div>
                                    <!-- END SCROLL HEADERS -->
                                    <div id="infoMenu" style="display: none;">
                                        <asp:PlaceHolder ID="phSubscribe" runat="Server" Visible="False"><a href="javascript:infoSwitch('Subscribe');initSubscForm()"
                                            class="infoBtn" id="infoLinkSubscribe"><span>&raquo;</span> Subscribe Today</a>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID='phContact' runat="server"></asp:PlaceHolder>
                                        <a href="javascript:infoSwitch('NavInst');" id="infoLinkNavInst"
                                                class="infoBtn selected"><span>&raquo;</span> Navigation Instructions</a>
                                    </div>
                                    <!--<div id="Scrollbar-Container"> <img src="http://www.ecndigitaledition.com/images/uparrow_59.gif" class="Scrollbar-Up" /> <img src="http://www.ecndigitaledition.com/images/arrow_down_73.gif" class="Scrollbar-Down" />
                                <div class="Scrollbar-Track"> <img src="http://www.ecndigitaledition.com/images/handle_82.gif" class="Scrollbar-Handle" /> </div>
                            </div>-->
                                    <div id="Scroller-1">
                                        <div class="Scroller-Container">
                                            <div id="divinformation" style="display: block;">
                                                <div id="infoDivSubscribe" style="display: none;">
                                                    <!-- SUBSCRIBE TODDAY FORM DIV -->
                                                    <div id="subscFormContainer">
                                                        <p>
                                                            <label>
                                                                * First Name:<span class="alert" id="fnameAlert">(Required)</span></label>
                                                            <input id="fname" type="text" size="25" class="formField">
                                                            <label>
                                                                * Last Name:<span class="alert" id="lnameAlert">(Required)</span></label>
                                                            <input id="lname" type="text" size="25" class="formField">
                                                            <label>
                                                                * Email:<span class="alert" id="emailAlert">(Required)</span><span class="alert"
                                                                    id="emailInvalidAlert">Invalid Email</span></label>
                                                            <input id="email" type="text" size="25" class="formField">
                                                            <label>
                                                                Phone:</label>
                                                            <input id="phone" type="text" size="25" class="formField">
                                                            <label>
                                                                Fax:</label>
                                                            <input id="fax" type="text" size="25" class="formField">
                                                            <label>
                                                                Country:</label>
                                                            <select class="formField" id="country" onchange="countryChange();">
                                                                <option value="">&lt;Select&gt;</option>
                                                                <option value="United States">United States</option>
                                                                <option value="Afghanistan">Afghanistan</option>
                                                                <option value="Albania">Albania</option>
                                                                <option value="Algeria">Algeria</option>
                                                                <option value="American Samoa">American Samoa</option>
                                                                <option value="Andorra">Andorra</option>
                                                                <option value="Angola">Angola</option>
                                                                <option value="Anguilla">Anguilla</option>
                                                                <option value="Antarctica">Antarctica</option>
                                                                <option value="Antigua">Antigua</option>
                                                                <option value="Argentina">Argentina</option>
                                                                <option value="Armenia">Armenia</option>
                                                                <option value="Aruba">Aruba</option>
                                                                <option value="Australia">Australia</option>
                                                                <option value="Austria">Austria</option>
                                                                <option value="Azerbaijan">Azerbaijan</option>
                                                                <option value="Bahamas">Bahamas</option>
                                                                <option value="Bahrain">Bahrain</option>
                                                                <option value="Bangladesh">Bangladesh</option>
                                                                <option value="Barbados">Barbados</option>
                                                                <option value="Barbuda">Barbuda</option>
                                                                <option value="Belarus">Belarus</option>
                                                                <option value="Belgium">Belgium</option>
                                                                <option value="Belize">Belize</option>
                                                                <option value="Benin">Benin</option>
                                                                <option value="Bermuda">Bermuda</option>
                                                                <option value="Bhutan">Bhutan</option>
                                                                <option value="Bolivia">Bolivia</option>
                                                                <option value="Bosnia & Herzegowina">Bosnia & Herzegowina</option>
                                                                <option value="Botswana">Botswana</option>
                                                                <option value="Bouvet Island">Bouvet Island</option>
                                                                <option value="Brazil">Brazil</option>
                                                                <option value="British Indian Ocean Terr">British Indian Ocean Terr.</option>
                                                                <option value="Brunei Darussalam">Brunei Darussalam</option>
                                                                <option value="Bulgaria">Bulgaria</option>
                                                                <option value="Burkina Faso">Burkina Faso</option>
                                                                <option value="Burma (Myanmar)">Burma (Myanmar)</option>
                                                                <option value="Burundi">Burundi</option>
                                                                <option value="Cambodia">Cambodia</option>
                                                                <option value="Cameroon">Cameroon</option>
                                                                <option value="Canada">Canada</option>
                                                                <option value="Cape Verde">Cape Verde</option>
                                                                <option value="Cayman Islands">Cayman Islands</option>
                                                                <option value="Central African Rep">Central African Rep</option>
                                                                <option value="Chad">Chad</option>
                                                                <option value="Chile">Chile</option>
                                                                <option value="China">China</option>
                                                                <option value="Christmas Island">Christmas Island</option>
                                                                <option value="Cocos (Keeling) Isles">Cocos (Keeling) Isles</option>
                                                                <option value="Colombia">Colombia</option>
                                                                <option value="Comoros">Comoros</option>
                                                                <option value="Congo">Congo</option>
                                                                <option value="Congo, The Democratic Rep">Congo,The Democratic Rep</option>
                                                                <option value="Cook Islands">Cook Islands</option>
                                                                <option value="Costa Rica">Costa Rica</option>
                                                                <option value="Croatia">Croatia</option>
                                                                <option value="Cuba">Cuba</option>
                                                                <option value="Cyprus">Cyprus</option>
                                                                <option value="Czech Republic">Czech Republic</option>
                                                                <option value="Denmark">Denmark</option>
                                                                <option value="Djibouti">Djibouti</option>
                                                                <option value="Dominica">Dominica</option>
                                                                <option value="Dominican Republic">Dominican Republic</option>
                                                                <option value="East Timor">East Timor</option>
                                                                <option value="Ecuador">Ecuador</option>
                                                                <option value="Egypt">Egypt</option>
                                                                <option value="El Salvador">El Salvador</option>
                                                                <option value="Equatorial Guinea">Equatorial Guinea</option>
                                                                <option value="Eritrea">Eritrea</option>
                                                                <option value="Estonia">Estonia</option>
                                                                <option value="Ethiopia">Ethiopia</option>
                                                                <option value="Falkland Islands (Malvinas)">Falkland Islands (Malvinas)</option>
                                                                <option value="Faroe Islands">Faroe Islands</option>
                                                                <option value="Fiji">Fiji</option>
                                                                <option value="Finland">Finland</option>
                                                                <option value="France">France</option>
                                                                <option value="France, Metro">France, Metro</option>
                                                                <option value="French Guiana">French Guiana</option>
                                                                <option value="French Polynesia">French Polynesia</option>
                                                                <option value="French Southern Terr.">French Southern Terr.</option>
                                                                <option value="Gabon">Gabon</option>
                                                                <option value="Gambia">Gambia</option>
                                                                <option value="Georgia">Georgia</option>
                                                                <option value="Germany">Germany</option>
                                                                <option value="Ghana">Ghana</option>
                                                                <option value="Gibraltar">Gibraltar</option>
                                                                <option value="Greece ">Greece</option>
                                                                <option value="Greenland">Greenland</option>
                                                                <option value="Grenada">Grenada</option>
                                                                <option value="Guadeloupe">Guadeloupe</option>
                                                                <option value="Guam">Guam</option>
                                                                <option value="Guatemala">Guatemala</option>
                                                                <option value="Guinea">Guinea</option>
                                                                <option value="Guinea-Bissau">Guinea-Bissau</option>
                                                                <option value="Guyana">Guyana</option>
                                                                <option value="Haiti">Haiti</option>
                                                                <option value="Heard Ad Mc Donald Isles">Heard And Mc Donald Isles</option>
                                                                <option value="Holy See (Vatican)">Holy See (Vatican)</option>
                                                                <option value="Honduras">Honduras</option>
                                                                <option value="Hong Kong">Hong Kong</option>
                                                                <option value="Hungary">Hungary</option>
                                                                <option value="Iceland">Iceland</option>
                                                                <option value="India">India</option>
                                                                <option value="Indonesia">Indonesia</option>
                                                                <option value="Iran">Iran</option>
                                                                <option value="Iraq">Iraq</option>
                                                                <option value="Ireland">Ireland</option>
                                                                <option value="Israel">Israel</option>
                                                                <option value="Italy">Italy</option>
                                                                <option value="Ivory Coast (Cote D'Ioire)">Ivory Coast (Cote D'Ivoire)</option>
                                                                <option value="Japan">Japan</option>
                                                                <option value="Jordan">Jordan</option>
                                                                <option value="Kazakhstan">Kazakhstan</option>
                                                                <option value="Kenya">Kenya</option>
                                                                <option value="Kiribati">Kiribati</option>
                                                                <option value="Korea Dem People's Rep">Korea Dem People's Rep</option>
                                                                <option value="Korea Republic Of">Korea Republic Of</option>
                                                                <option value="Kuwait">Kuwait</option>
                                                                <option value="Kyrgyzstan">Kyrgyzstan</option>
                                                                <option value="Laos">Laos</option>
                                                                <option value="Latvia">Latvia</option>
                                                                <option value="Lebanon">Lebanon</option>
                                                                <option value="Lesotho">Lesotho</option>
                                                                <option value="Liberia">Liberia</option>
                                                                <option value="Libya">Libya</option>
                                                                <option value="Liechtenstein">Liechtenstein</option>
                                                                <option value="Lithuania">Lithuania</option>
                                                                <option value="Luxembourg">Luxembourg</option>
                                                                <option value="Macau">Macau</option>
                                                                <option value="Macedonia(Republic of)">Macedonia(Republic of)</option>
                                                                <option value="Madagascar">Madagascar</option>
                                                                <option value="Malawi">Malawi</option>
                                                                <option value="Malaysia">Malaysia</option>
                                                                <option value="Maldives">Maldives</option>
                                                                <option value="Mali">Mali</option>
                                                                <option value="Malta">Malta</option>
                                                                <option value="Marshall Islan Martiniqueds">Marshall Islan Martiniqueds</option>
                                                                <option value="Martinique">Martinique</option>
                                                                <option value="Mauritania">Mauritania</option>
                                                                <option value="Mauritius">Mauritius</option>
                                                                <option value="Mayotte">Mayotte</option>
                                                                <option value="Mexico">Mexico</option>
                                                                <option value="Micronesia Fed States">Micronesia Fed States</option>
                                                                <option value="Moldova Rep">Moldova Rep</option>
                                                                <option value="Monaco">Monaco</option>
                                                                <option value="Mongolia">Mongolia</option>
                                                                <option value="Montserrat">Montserrat</option>
                                                                <option value="Morocco">Morocco</option>
                                                                <option value="Mozambique">Mozambique</option>
                                                                <option value="Namibia">Namibia</option>
                                                                <option value="Nauru">Nauru</option>
                                                                <option value="Nepal">Nepal</option>
                                                                <option value="Netherlands">Netherlands</option>
                                                                <option value="Netherlands Antilles">Netherlands Antilles</option>
                                                                <option value="New Caledonia">New Caledonia</option>
                                                                <option value="New Zealand">New Zealand</option>
                                                                <option value="Nicaragua">Nicaragua</option>
                                                                <option value="Niger">Niger</option>
                                                                <option value="Nigeria">Nigeria</option>
                                                                <option value="Niue">Niue</option>
                                                                <option value="Norfolk Island">Norfolk Island</option>
                                                                <option value="Northern Mariana Isles">Northern Mariana Isles</option>
                                                                <option value="Norway">Norway</option>
                                                                <option value="Oman">Oman</option>
                                                                <option value="Pakistan">Pakistan</option>
                                                                <option value="Palau">Palau</option>
                                                                <option value="Panama">Panama</option>
                                                                <option value="Papua New Guinea">Papua New Guinea</option>
                                                                <option value="Paraguay">Paraguay</option>
                                                                <option value="Peru">Peru</option>
                                                                <option value="Philippines">Philippines</option>
                                                                <option value="Pitcairn">Pitcairn</option>
                                                                <option value="Poland">Poland</option>
                                                                <option value="Portugal">Portugal</option>
                                                                <option value="Puerto Rico">Puerto Rico</option>
                                                                <option value="Qatar">Qatar</option>
                                                                <option value="Reunion">Reunion</option>
                                                                <option value="Romania">Romania</option>
                                                                <option value="Russian Federation">Russian Federation</option>
                                                                <option value="Rwanda">Rwanda</option>
                                                                <option value="Saint Kitts & Nevis">Saint Kitts & Nevis</option>
                                                                <option value="Saint Lucia">Saint Lucia</option>
                                                                <option value="Saint Vincent & Grenadines ">Saint Vincent & Grenadines</option>
                                                                <option value="Samoa">Samoa</option>
                                                                <option value="San Marino">San Marino</option>
                                                                <option value="Sao Tome & Principe">Sao Tome & Principe</option>
                                                                <option value="Saudi Arabia">Saudi Arabia</option>
                                                                <option value="Senegal">Senegal</option>
                                                                <option value="Seychelles">Seychelles</option>
                                                                <option value="Sierra Leona">Sierra Leona</option>
                                                                <option value="Singapore">Singapore</option>
                                                                <option value="Slovakia (Slovak Rep)">Slovakia (Slovak Rep)</option>
                                                                <option value="Slovenia">Slovenia</option>
                                                                <option value="Solomon Islands">Solomon Islands</option>
                                                                <option value="Somalia">Somalia</option>
                                                                <option value="South Africa">South Africa</option>
                                                                <option value="Georgia & Sandwich Isles">Georgia & Sandwich Isles</option>
                                                                <option value="ESPSpain">ESPSpain</option>
                                                                <option value="Sri Lanka">Sri Lanka</option>
                                                                <option value="St. Helena">St. Helena</option>
                                                                <option value="St. Pierre & Miquelon">St. Pierre & Miquelon</option>
                                                                <option value="Sudan">Sudan</option>
                                                                <option value="Suriname">Suriname</option>
                                                                <option value="Svalbard & Jan Mayen Island">Svalbard & Jan Mayen Islands</option>
                                                                <option value="Swaziland">Swaziland</option>
                                                                <option value="Sweden">Sweden</option>
                                                                <option value="Switzerland">Switzerland</option>
                                                                <option value="Syria">Syria</option>
                                                                <option value="Taiwan">Taiwan</option>
                                                                <option value="Tajikistan">Tajikistan</option>
                                                                <option value="Tanzania">Tanzania</option>
                                                                <option value="Thailand">Thailand</option>
                                                                <option value="Togo">Togo</option>
                                                                <option value="Tokelau">Tokelau</option>
                                                                <option value="Tonga">Tonga</option>
                                                                <option value="Trinidad & Tobago">Trinidad & Tobago</option>
                                                                <option value="Tunisia">Tunisia</option>
                                                                <option value="Turkey">Turkey</option>
                                                                <option value="Turkmenistan">Turkmenistan</option>
                                                                <option value="Turks & Caicos Islands">Turks & Caicos Islands</option>
                                                                <option value="Tuvalu">Tuvalu</option>
                                                                <option value="Uganda">Uganda</option>
                                                                <option value="Ukraine">Ukraine</option>
                                                                <option value="United Arab Emirates">United Arab Emirates</option>
                                                                <option value="United Kingdom">United Kingdom</option>
                                                                <option value="Minor Outlying Isles">Minor Outlying Isles</option>
                                                                <option value="Uruguay">Uruguay</option>
                                                                <option value="Uzbekistan">Uzbekistan</option>
                                                                <option value="Vanuatu">Vanuatu</option>
                                                                <option value="Venezuela">Venezuela</option>
                                                                <option value="Viet Nam">Viet Nam</option>
                                                                <option value="Virgin Isles (British)">Virgin Isles (British)</option>
                                                                <option value="Virgin Isles (U.S.)">Virgin Isles (U.S.)</option>
                                                                <option value="Wallis & Futuna Islands ">Wallis & Futuna Islands</option>
                                                                <option value="Western Sahara">Western Sahara</option>
                                                                <option value="Yemen">Yemen</option>
                                                                <option value="Yugoslavia">Yugoslavia</option>
                                                                <option value="Zambia">Zambia</option>
                                                                <option value="Zimbabwe">Zimbabwe</option>
                                                                <option value="Other">Other</option>
                                                            </select>
                                                            <span id="addressContainer" style="display: none;">
                                                                <label>
                                                                    Address:</label>
                                                                <input id="stAddress" type="text" size="25" class="formField">
                                                                <label>
                                                                    City:</label>
                                                                <input id="city" type="text" size="25" class="formField">
                                                                <span id="stateContainer">
                                                                    <label>
                                                                        State:</label>
                                                                    <select name="state" id="state" class="formField">
                                                                        <option value="AL">AL</option>
                                                                        <option value="AK">AK</option>
                                                                        <option value="AZ">AZ</option>
                                                                        <option value="AR">AR</option>
                                                                        <option value="CA">CA</option>
                                                                        <option value="CO">CO</option>
                                                                        <option value="CT">CT</option>
                                                                        <option value="DE">DE</option>
                                                                        <option value="DC">DC</option>
                                                                        <option value="FL">FL</option>
                                                                        <option value="GA">GA</option>
                                                                        <option value="HI">HI</option>
                                                                        <option value="ID">ID</option>
                                                                        <option value="IL">IL</option>
                                                                        <option value="IN">IN</option>
                                                                        <option value="IA">IA</option>
                                                                        <option value="KS">KS</option>
                                                                        <option value="KY">KY</option>
                                                                        <option value="LA">LA</option>
                                                                        <option value="ME">ME</option>
                                                                        <option value="MD">MD</option>
                                                                        <option value="MA">MA</option>
                                                                        <option value="MI">MI</option>
                                                                        <option value="MN">MN</option>
                                                                        <option value="MS">MS</option>
                                                                        <option value="MO">MO</option>
                                                                        <option value="MT">MT</option>
                                                                        <option value="NE">NE</option>
                                                                        <option value="NV">NV</option>
                                                                        <option value="NH">NH</option>
                                                                        <option value="NJ">NJ</option>
                                                                        <option value="NM">NM</option>
                                                                        <option value="NY">NY</option>
                                                                        <option value="NC">NC</option>
                                                                        <option value="ND">ND</option>
                                                                        <option value="OH">OH</option>
                                                                        <option value="OK">OK</option>
                                                                        <option value="OR">OR</option>
                                                                        <option value="PA">PA</option>
                                                                        <option value="RI">RI</option>
                                                                        <option value="SC">SC</option>
                                                                        <option value="SD">SD</option>
                                                                        <option value="TN">TN</option>
                                                                        <option value="TX">TX</option>
                                                                        <option value="UT">UT</option>
                                                                        <option value="VT">VT</option>
                                                                        <option value="VA">VA</option>
                                                                        <option value="WA">WA</option>
                                                                        <option value="WV">WV</option>
                                                                        <option value="WI">WI</option>
                                                                        <option value="WY">WY</option>
                                                                    </select>
                                                                </span>
                                                                <label id="zipLabel">
                                                                    Zip:</label>
                                                                <input id="zc" type="text" size="25" class="formField">
                                                            </span>
                                                        </p>
                                                        <p style="text-align: center;">
                                                            <a href="javascript:void(0)" onclick="javascript:alert('Disabled in Preview mode');">
                                                                <img src="http://www.ecndigitaledition.com/images/submit-button.gif" border='0' /></a></p>
                                                        <p style="font-size: 10px; text-align: right; padding-top: 5px; margin-right: 9px;
                                                            border-top: 1px #ccc solid;">
                                                            <a href="javascript:processSubForm('reset');" style="color: #ccc;">Reset Form</a></p>
                                                    </div>
                                                    <!-- END subscFormContainer -->
                                                    <div id="subscFormThankYou" style="display: none;">
                                                        <p>
                                                            <strong>Thank you for your subscription.</strong></p>
                                                        <p>
                                                            A subscription confirmation has been sent to the email address you provided. Please
                                                            check your email and click the specified link to complete the subscription process.</p>
                                                    </div>
                                                </div>
                                                <div id="infoDivContact" style="display: none;">
                                                    <p>
                                                        Email:<br />
                                                        <asp:HyperLink ID="lnkContactEmail" runat="Server"></asp:HyperLink>
                                                    </p>
                                                    <asp:Panel ID="pnlContactPhone" Visible="False" runat="Server">
                                                        <p>
                                                            Customer Service:<br />
                                                            <asp:Label ID="lblContactPhone" runat="Server"></asp:Label>
                                                        </p>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlContactAddress" Visible="False" runat="Server">
                                                        <p>
                                                            Address:<br />
                                                            <asp:Label ID="lblContactAddress1" runat="Server"></asp:Label><br />
                                                            <asp:Label ID="lblContactAddress2" runat="Server"></asp:Label>
                                                        </p>
                                                    </asp:Panel>
                                                    <p style="text-align: center;">
                                                        <asp:Image runat="Server" ID="imgContactLogo" Visible="false"></asp:Image>
                                                    </p>
                                                </div>
                                                <div id="infoDivNavInst" style="display: block;">
                                                    <div class="infoTable">
                                                        <div>
                                                            <div>
                                                                <table border='0' cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:alert('Disabled in Preview Mode');">
                                                                                    <img src="http://www.ecndigitaledition.com/images/printInfo.gif" alt="Print Options"
                                                                                        title="Print Options"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Print Options</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:alert('Disabled in Preview Mode');">
                                                                                    <img src="http://www.ecndigitaledition.com/images/emailInfo.gif" alt="Email to a Friend"
                                                                                        title="Email to a Friend"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Email This Edition Link To A Friend</p>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="infoTable">
                                                        <div>
                                                            <div>
                                                                <table border='0' cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:ShowThumbnails();">
                                                                                    <img src="http://www.ecndigitaledition.com/images/thumbInfo.gif" alt="Thumbnail View"
                                                                                        title="Thumbnail View"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Thumbnail View</p>
                                                                        </td>
                                                                    </tr>
                                                                    <asp:PlaceHolder ID="plsbSinglePg" runat="Server">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <a href="javascript:Edition.setpref(1)">
                                                                                        <img src="http://www.ecndigitaledition.com/images/onePageInfo.gif" alt="Single Page View"
                                                                                            title="Single Page View"></a></div>
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    Single Page View</p>
                                                                            </td>
                                                                        </tr>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="plsbDoublePg" runat="Server">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <a href="javascript:Edition.setpref(2)">
                                                                                        <img src="http://www.ecndigitaledition.com/images/twoPageInfo.gif" alt="Facing Page View"
                                                                                            title="Facing Page View"></a></div>
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    Facing Page View</p>
                                                                            </td>
                                                                        </tr>
                                                                    </asp:PlaceHolder>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="infoTable">
                                                        <div>
                                                            <div>
                                                                <table border='0' cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:Edition.Nav(Edition.Page.First);">
                                                                                    <img src="http://www.ecndigitaledition.com/images/firstInfo.gif" alt="First Page"
                                                                                        title="First Page"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                First Page</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:Edition.Nav(Edition.Page.Previous);">
                                                                                    <img src="http://www.ecndigitaledition.com/images/prevInfo.gif" alt="Previous Page"
                                                                                        title="Previous Page"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Previous Page</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:Edition.Nav(Edition.Page.Next);">
                                                                                    <img src="http://www.ecndigitaledition.com/images/nextInfo.gif" alt="Next Page" title="Next Page"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Next Page</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <a href="javascript:Edition.Nav(Edition.Page.Last);">
                                                                                    <img src="http://www.ecndigitaledition.com/images/lastInfo.gif" alt="Last Page" title="Last Page"></a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Last Page</p>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="infoTable">
                                                        <div>
                                                            <div>
                                                                <table border='0' cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div id="zoomBarInfo">
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td valign="middle" align='right' width="40" height="33" class="plusMinusZoomII">
                                                                                            <a href="javascript:Edition.zoom(0);" class="minusZoom" title="Zoom out">Zoom out</a>
                                                                                        </td>
                                                                                        <td valign="middle" align="center" width="51">
                                                                                            <img id="zb2" src="http://www.ecndigitaledition.com/images/zoomA1.gif" usemap="#Map5"
                                                                                                border='0'>
                                                                                        </td>
                                                                                        <td valign="middle" align="left" width="40" class="plusMinusZoomII">
                                                                                            <a href="javascript:Edition.zoom(1);" class="plusZoom" title="Zoom in">Zoom in</a>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <p>
                                                                                    (-) Zoom Out / (+) Zoom In</p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="65">
                                                                            <div class="fitPage">
                                                                                <a style="position: static; margin: 0;" href="javascript:Edition.zoombar(getdefaultsize());">
                                                                                    Fit Page</a></div>
                                                                        </td>
                                                                        <td>
                                                                            <p>
                                                                                Fit Page View to Fill Your Screen</p>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:HyperLink ID="hlpowered" runat="server" ImageUrl="http://www.ecndigitaledition.com/images/powered-by-KM.png"
                                                        NavigateUrl="http://www.knowledgemarketing.com" Target="_blank"></asp:HyperLink>
                                                </div>
                                            </div>
                                            <div id="divpages" class="pageWithImg" style="display: none;">
                                            </div>
                                            <div id="divtableOfContents" style="display: none;">
                                                <div class="innerDivText" class="topMarg">
                                                    <div id="treeContainer" style="padding-left: 10px">
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divsearch" style="display: none;">
                                                <div id="divsearchcontent" class="topMarg">
                                                </div>
                                            </div>
                                            <div id="divlinks" style="display: none;">
                                                <div id="divlinkcontent" class="topMarg">
                                                </div>
                                            </div>
                                            <div id="divbackIssues" style="display: none;">
                                                <!-- p tag is inside of repeater -->
                                                <asp:Repeater ID="rptBackIssues" runat="Server">
                                                    <ItemTemplate>
                                                        <p>
                                                            <a href="Magazine.aspx?eID=<%# DataBinder.Eval(Container, "DataItem.EditionID")%>">
                                                                <%# DataBinder.Eval(Container, "DataItem.editionName")%>
                                                            </a>
                                                        </p>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end wholeScroll -->
                                <!-- end divTABS -->
                                <!-- :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: -->
                            </div>
                        </td>
                        <td valign="top">
                            <div id="mainContent" style="width: 100%; overflow: scroll; padding: 0; margin: 0;">
                                <table cellpadding="0" cellspacing="0" height="100%" width="100%">
                                    <tr>
                                        <td valign="middle">
                                            <table style="padding: 0; margin: 0;" cellspacing="0" cellpadding="0" width="100%"
                                                height="100%" border='0'>
                                                <tr>
                                                    <td align="center" width="100%" colspan='3' valign="top">
                                                        <div id="divThumb" style="display: none;" align="center">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='3' valign="middle">
                                                        <div style="position: relative">
                                                           <a style="visibility: hidden;" id="olay" align="center">&nbsp;</a>
                                                        </div>
                                                        <div id="divpage" style="white-space: nowrap;" align="center">
                                                            <table style="" cellspacing="0" cellpadding="0" align="center" border='0'>
                                                                <tr>
                                                                    <td id="td1" align="center">
                                                                        <img src="http://www.ecndigitaledition.com/images/transparentpixel.gif" border='0'>
                                                                    </td>
                                                                    <td id="td2" align="center">
                                                                        <img style="display: block" src="http://www.ecndigitaledition.com/images/transparentpixel.gif"
                                                                            border='0'>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <map name="Map3">
                    <area shape="RECT" coords="61,4,92,26" href="javascript:Edition.setpref(2);">
                    <area shape="RECT" coords="37,3,57,26" href="javascript:Edition.setpref(1);">
                    <area shape="RECT" coords="8,3,33,27" href="javascript:ShowThumbnails();">
                </map>
                <map name="Map5">
                    <area shape="RECT" coords="0,0,4,16" href="javascript:Edition.zoombar(0);">
                    <area shape="RECT" coords="6,0,10,17" href="javascript:Edition.zoombar(1);">
                    <area shape="RECT" coords="12,0,16,17" href="javascript:Edition.zoombar(2);">
                    <area shape="RECT" coords="18,0,22,16" href="javascript:Edition.zoombar(3);">
                    <area shape="RECT" coords="24,0,28,17" href="javascript:Edition.zoombar(4);">
                    <area shape="RECT" coords="30,0,34,17" href="javascript:Edition.zoombar(5);">
                    <area shape="RECT" coords="36,0,40,17" href="javascript:Edition.zoombar(6);">
                </map>

                <script type="text/javascript">

				sizeFrame();
				setwhichBtn('infoBtn');
				expandSideBar();
				
				var AjCalls = PageMethods;
				var UpdateDB = true;

                </script>

            </div>
        </asp:Panel>
    </form>

    <script type="text/javascript">
        BrowserDetect.init();
        if (BrowserDetect.browser=="Safari"){
          document.body.style.position="fixed";
        }
    </script>

</body>
</html>
