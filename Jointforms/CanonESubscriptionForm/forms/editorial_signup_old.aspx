<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Canon Communications - Editorial Signup</title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript">

function showQuestions (publication) {
	var section = "#questions div." + publication;
	
	// this section checks if the question is visible  
	// & removes the quesion from it's original place and appends to end
	if ($(section).is(':hidden')) {
		var inSection = $(section).html(); 
		var inSection = '<div class="' + publication + '">' + inSection +  '</div';
		$(section).remove(); //remove from original position
		$('#questions').append(inSection);
		$(section).hide(); //hide element for binding effect	
	}
	//else
	//{
	//    $(section).hide(); //hide element for binding effect	
	//    return;
	//}
	//
	
	$(section).show('slow');
	$('#questions div').addClass('showLogos');
}


$(document).ready(function() {
	
	var defaultMagazine = $.jqURL.get("mag"); 
	
	//hide all questions by default
	$('#questions div').hide();
    
    if (defaultMagazine	!= undefined)
    {
       	var ccqsection = "#current div.ccq";
	    $(ccqsection).remove();

	    //show questions and hide logo for this publication
	    //var defaultQuestions = '#questions div.' + defaultMagazine;
	    //$(defaultQuestions).show();
    	
	    var section = "#questions div." + defaultMagazine;
	    var inSection = $(section).html(); 
	    var inSection = '<div class="' + defaultMagazine + '">' + inSection +  '</div';
	    $(section).remove(); //remove from original position
	    $('#current').append(inSection);
	    $(section).hide(); //hide element for binding effect	
    	
	    var defaultLogo = '#logos img.' + defaultMagazine;
	    $(defaultLogo).hide();
	    
	    
	    if (defaultMagazine == 'ivd' || defaultMagazine == 'mddi' || defaultMagazine == 'mpmn')
	    {
	        var memLogo = '#logos img.mem';
	        $(memLogo).hide();
	        var mxLogo = '#logos img.mx';
	        $(mxLogo).hide();
	    }
	    
	}
	else
	{

	}
	
    //adds click function to logo images and uses img class to show correponding questions
    $('#logos img').click(function() { 
	    showQuestions(this.className); 
    });
});


function validateForm() 
{
    var allOk = false;
	allOk = 
		(svValidator("Email", document.forms[0].e.value) && svValidator("First Name", document.forms[0].fn.value) && svValidator("Last Name", document.forms[0].ln.value) && 
		svValidator("Job Title", document.forms[0].t.value) && svValidator("Company", document.forms[0].compname.value) && svValidator("Address", document.forms[0].adr.value) && 
		svValidator("City", document.forms[0].city.value) && svValidator("State", document.forms[0].state.value) && 
		svValidator("Postcode", document.forms[0].zc.value));

	// Validate zip against state
	if (allOk) 
	{
        var x = document.forms[0].e.value;

        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(x))
        {	
	        alert('Invalid Email address');
	        allOk = false;
        }
    }
	return allOk;
}
    </script>

</head>
<body class="prototype">
    <div id="container">
        <div id="innerContainer">
            <div id="container-content">
                <div id="banner">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td align="center">

                                <script type="text/javascript">
	  	var pub = $.jqURL.get("mag");
		
		if (pub) {
			pub = pub.toUpperCase();
      		document.write('<img border="0" src="http://www.kmpsgroup.com/images/' + pub + '_sLogo.jpg" ALT="">');
		} else {
			document.write('<img border="0" src="http://www.kmpsgroup.com/images/canon_sLogo.jpg" ALT="">');
		}
                                </script>

                            </td>
                        </tr>
                    </table>
                </div>
                <form name="frmsub" id="frmsub" action="http://emailactivity.ecn5.com/engines/SO_subscribe.aspx"
                    onsubmit="javascript:return validateForm();">
                    <div id="current">
                        <div class="ccq" style="height: 7em;">
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).
                            </label>
                            <br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_CCQ" />
                                    </td>
                                    <td valign="top">
                                        <b><i>Consultants Corner Quarterly</i></b> - A quarterly newsletter providing insights
                                        into the manufacturing, engineering, regulatory, and marketing issues that confront
                                        the device industry.
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div id="profile">
                        <p>
                            <label>
                                * Email:</label>
                            <input name="e" size="30">
                        </p>
                        <p>
                            <label>
                                * First Name:</label>
                            <input name="fn" size="30">
                        </p>
                        <p>
                            <label>
                                * Last Name:</label>
                            <input name="ln" size="30">
                        </p>
                        <p>
                            <label>
                                * Job Title:</label>
                            <input name="t" size="30">
                        </p>
                        <p>
                            <label>
                                * Company:</label>
                            <input name="compname" size="30">
                        </p>
                        <p>
                            <label>
                                * Address:</label>
                            <input name="adr" size="30">
                        </p>
                        <p>
                            <label>
                                Address 2:</label>
                            <input name="adr2" size="30">
                        </p>
                        <p>
                            <label>
                                * City:</label>
                            <input name="city" size="30">
                        </p>
                        <p>
                            <label>
                                * State/Province:</label>
                            <select id="State" name="state">
                                <option value="" selected>Select a State</option>
                                <optgroup label="USA">
                                    <option value="AK">Alaska</option>
                                    <option value="AL">Alabama</option>
                                    <option value="AR">Arkansas</option>
                                    <option value="AZ">Arizona</option>
                                    <option value="CA">California</option>
                                    <option value="CO">Colorado</option>
                                    <option value="CT">Connecticut</option>
                                    <option value="DC">Washington D.C.</option>
                                    <option value="DE">Delaware</option>
                                    <option value="FL">Florida</option>
                                    <option value="GA">Georgia</option>
                                    <option value="HI">Hawaii</option>
                                    <option value="IA">Iowa</option>
                                    <option value="ID">Idaho</option>
                                    <option value="IL">Illinois</option>
                                    <option value="IN">Indiana</option>
                                    <option value="KS">Kansas</option>
                                    <option value="KY">Kentucky</option>
                                    <option value="LA">Louisiana</option>
                                    <option value="MA">Massachusetts</option>
                                    <option value="MD">Maryland</option>
                                    <option value="ME">Maine</option>
                                    <option value="MI">Michigan</option>
                                    <option value="MN">Minnesota</option>
                                    <option value="MO">Missourri</option>
                                    <option value="MS">Mississippi</option>
                                    <option value="MT">Montana</option>
                                    <option value="NC">North Carolina</option>
                                    <option value="ND">North Dakota</option>
                                    <option value="NE">Nebraska</option>
                                    <option value="NH">New Hampshire</option>
                                    <option value="NJ">New Jersey</option>
                                    <option value="NM">New Mexico</option>
                                    <option value="NV">Nevada</option>
                                    <option value="NY">New York</option>
                                    <option value="OH">Ohio</option>
                                    <option value="OK">Oklahoma</option>
                                    <option value="OR">Oregon</option>
                                    <option value="PA">Pennsylvania</option>
                                    <option value="PR">Puerto Rico</option>
                                    <option value="RI">Rhode Island</option>
                                    <option value="SC">South Carolina</option>
                                    <option value="SD">South Dakota</option>
                                    <option value="TN">Tennessee</option>
                                    <option value="TX">Texas</option>
                                    <option value="UT">Utah</option>
                                    <option value="VA">Virginia</option>
                                    <option value="VT">Vermont</option>
                                    <option value="WA">Washington</option>
                                    <option value="WI">Wisconsin</option>
                                    <option value="WV">West Virginia</option>
                                    <option value="WY">Wyoming</option>
                                </optgroup>
                                <optgroup label="Canada">
                                    <option value="AB">Alberta</option>
                                    <option value="BC">British Columbia</option>
                                    <option value="MB">Manitoba</option>
                                    <option value="NB">New Brunswick</option>
                                    <option value="NF">New Foundland</option>
                                    <option value="NS">Nova Scotia</option>
                                    <option value="ON">Ontario</option>
                                    <option value="PE">Prince Edward Island</option>
                                    <option value="QC">Quebec</option>
                                    <option value="SK">Saskatchewan</option>
                                    <option value="YT">Yukon Territories</option>
                                </optgroup>
                                <optgroup label="Foreign">
                                    <option value="OT">Other</option>
                                </optgroup>
                            </select>
                        </p>
                        <p class="zip">
                            <label>
                                * Postcode:</label>
                            <input name="zc" size="10">
                        </p>
                        <p class="country">
                            <label>
                                Country:</label>
                            <select name="ctry" id="country">
                                <option value="">--- Select Country --- </option>
                                <option value="USA">UNITED STATES OF AMERICA</option>
                                <option value="ALBANIA">ALBANIA</option>
                                <option value="ALGERIA">ALGERIA</option>
                                <option value="AMERICAN SAMOA">AMERICAN SAMOA</option>
                                <option value="ANDORRA">ANDORRA</option>
                                <option value="ANGOLA">ANGOLA</option>
                                <option value="ANTARCTICA">ANTARCTICA</option>
                                <option value="ANTIGUA">ANTIGUA</option>
                                <option value="ARGENTINA">ARGENTINA</option>
                                <option value="ARMENIA">ARMENIA</option>
                                <option value="ARUBA">ARUBA</option>
                                <option value="ASCENSION">ASCENSION</option>
                                <option value="AUSTRALIA">AUSTRALIA</option>
                                <option value="AUSTRIA">AUSTRIA</option>
                                <option value="AZERBAIJAN">AZERBAIJAN</option>
                                <option value="BAHAMAS">BAHAMAS</option>
                                <option value="BAHRAIN">BAHRAIN</option>
                                <option value="BANGLADESH">BANGLADESH</option>
                                <option value="BARBADOS">BARBADOS</option>
                                <option value="BELARUS">BELARUS</option>
                                <option value="BELGIUM">BELGIUM</option>
                                <option value="BELIZE">BELIZE</option>
                                <option value="BENIN">BENIN</option>
                                <option value="BERMUDA">BERMUDA</option>
                                <option value="BHUTAN">BHUTAN</option>
                                <option value="BOLIVIA">BOLIVIA</option>
                                <option value="BOTSWANA">BOTSWANA</option>
                                <option value="BOZNIA-HERZEGOVINA">BOZNIA-HERZEGOVINA</option>
                                <option value="BRAZIL">BRAZIL</option>
                                <option value="BRITISH VIRGIN ISLANDS">BRITISH VIRGIN ISLANDS</option>
                                <option value="BRITISH WEST INDIES">BRITISH WEST INDIES</option>
                                <option value="BRUNEI">BRUNEI</option>
                                <option value="BULGARIA">BULGARIA</option>
                                <option value="BURKINA FASO">BURKINA FASO</option>
                                <option value="BURMA">BURMA</option>
                                <option value="BURUNDI">BURUNDI</option>
                                <option value="CAMBODIA">CAMBODIA</option>
                                <option value="CAMEROON">CAMEROON</option>
                                <option value="CANADA">CANADA</option>
                                <option value="CANARY ISLANDS">CANARY ISLANDS</option>
                                <option value="CAPE VERDE ISLANDS">CAPE VERDE ISLANDS</option>
                                <option value="CAYMAN ISLANDS">CAYMAN ISLANDS</option>
                                <option value="CENTRAL AFRICAN REPUBLIC">CENTRAL AFRICAN REPUBLIC</option>
                                <option value="CHAD">CHAD</option>
                                <option value="CHANNEL ISLANDS">CHANNEL ISLANDS</option>
                                <option value="CHATHAM ISLAND">CHATHAM ISLAND</option>
                                <option value="CHILE">CHILE</option>
                                <option value="CHINA">CHINA</option>
                                <option value="CHRISTMAS ISLAND">CHRISTMAS ISLAND</option>
                                <option value="CIS">CIS</option>
                                <option value="COCOS ISLANDS">COCOS ISLANDS</option>
                                <option value="COLOMBIA">COLOMBIA</option>
                                <option value="COMOROS">COMOROS</option>
                                <option value="CONGO">CONGO</option>
                                <option value="COOK ISLANDS">COOK ISLANDS</option>
                                <option value="COSTA RICA">COSTA RICA</option>
                                <option value="CROATIA">CROATIA</option>
                                <option value="CUBA">CUBA</option>
                                <option value="CURACO">CURACO</option>
                                <option value="CYPRUS">CYPRUS</option>
                                <option value="CZECH REPUBLIC">CZECH REPUBLIC</option>
                                <option value="DENMARK">DENMARK</option>
                                <option value="DIEGO GARCIA">DIEGO GARCIA</option>
                                <option value="DJIBOUTI">DJIBOUTI</option>
                                <option value="DOMINICA">DOMINICA</option>
                                <option value="DOMINICIAN REPUBLIC">DOMINICIAN REPUBLIC</option>
                                <option value="EASTER ISLAND">EASTER ISLAND</option>
                                <option value="ECUADOR">ECUADOR</option>
                                <option value="EGYPT">EGYPT</option>
                                <option value="EL SALVADOR">EL SALVADOR</option>
                                <option value="EQUATORIAL GUINEA">EQUATORIAL GUINEA</option>
                                <option value="ERITREA">ERITREA</option>
                                <option value="ESTONIA">ESTONIA</option>
                                <option value="ETHIOPIA">ETHIOPIA</option>
                                <option value="FAEROE ISLANDS">FAEROE ISLANDS</option>
                                <option value="FALKLAND ISLANDS">FALKLAND ISLANDS</option>
                                <option value="FIJI">FIJI</option>
                                <option value="FINLAND">FINLAND</option>
                                <option value="FRANCE">FRANCE</option>
                                <option value="FRENCH ANTILLES">FRENCH ANTILLES</option>
                                <option value="FRENCH GUIANA">FRENCH GUIANA</option>
                                <option value="FRENCH POLYNESIA">FRENCH POLYNESIA</option>
                                <option value="FSM">FSM</option>
                                <option value="GABON">GABON</option>
                                <option value="GAMBIA">GAMBIA</option>
                                <option value="GEORGIA">GEORGIA</option>
                                <option value="GERMANY">GERMANY</option>
                                <option value="GHANA">GHANA</option>
                                <option value="GIBRALTAR">GIBRALTAR</option>
                                <option value="GINEA-BISSAU">GINEA-BISSAU</option>
                                <option value="GREECE">GREECE</option>
                                <option value="GREENLAND">GREENLAND</option>
                                <option value="GRENADA">GRENADA</option>
                                <option value="GRENADIN ISLANDS">GRENADIN ISLANDS</option>
                                <option value="GUADELOUPE">GUADELOUPE</option>
                                <option value="GUANTANAMO BAY">GUANTANAMO BAY</option>
                                <option value="GUATEMALA">GUATEMALA</option>
                                <option value="GUINEA">GUINEA</option>
                                <option value="GUYANA">GUYANA</option>
                                <option value="HAITI">HAITI</option>
                                <option value="HONDURAS">HONDURAS</option>
                                <option value="HONG KONG-SAR">HONG KONG-SAR</option>
                                <option value="HUNGARY">HUNGARY</option>
                                <option value="ICELAND">ICELAND</option>
                                <option value="INDIA">INDIA</option>
                                <option value="INDONESIA">INDONESIA</option>
                                <option value="IRAN">IRAN</option>
                                <option value="IRAQ">IRAQ</option>
                                <option value="IRELAND, REPUBLIC OF">IRELAND, REPUBLIC OF</option>
                                <option value="ISLE OF MAN">ISLE OF MAN</option>
                                <option value="ISRAEL">ISRAEL</option>
                                <option value="ITALY">ITALY</option>
                                <option value="IVORY COAST">IVORY COAST</option>
                                <option value="JAMAICA">JAMAICA</option>
                                <option value="JAPAN">JAPAN</option>
                                <option value="JORDAN">JORDAN</option>
                                <option value="KAZAKHSTAN">KAZAKHSTAN</option>
                                <option value="KENYA">KENYA</option>
                                <option value="KIRIBATI">KIRIBATI</option>
                                <option value="KUWAIT">KUWAIT</option>
                                <option value="KYRGYSTAN">KYRGYSTAN</option>
                                <option value="LAOS">LAOS</option>
                                <option value="LATVIA">LATVIA</option>
                                <option value="LEBANON">LEBANON</option>
                                <option value="LESOTHO">LESOTHO</option>
                                <option value="LIBERIA">LIBERIA</option>
                                <option value="LIBYA">LIBYA</option>
                                <option value="LIECHTENSTEIN">LIECHTENSTEIN</option>
                                <option value="LITHUANIA">LITHUANIA</option>
                                <option value="LUXEMBOURG">LUXEMBOURG</option>
                                <option value="MACAO">MACAO</option>
                                <option value="MACEDONIA">MACEDONIA</option>
                                <option value="MADAGASCAR">MADAGASCAR</option>
                                <option value="MALAWI">MALAWI</option>
                                <option value="MALAYSIA">MALAYSIA</option>
                                <option value="MALDIVES REPUBLIC">MALDIVES REPUBLIC</option>
                                <option value="MALI">MALI</option>
                                <option value="MALTA">MALTA</option>
                                <option value="MARSHALL ISLANDS">MARSHALL ISLANDS</option>
                                <option value="MARTINIQUE">MARTINIQUE</option>
                                <option value="MAURITANIA">MAURITANIA</option>
                                <option value="MAURITIUS">MAURITIUS</option>
                                <option value="MAYOTTE">MAYOTTE</option>
                                <option value="MEXICO">MEXICO</option>
                                <option value="MIDWAY ISLANDS">MIDWAY ISLANDS</option>
                                <option value="MIQUELON">MIQUELON</option>
                                <option value="MOLDOVA">MOLDOVA</option>
                                <option value="MONACO">MONACO</option>
                                <option value="MONGOLIA">MONGOLIA</option>
                                <option value="MONTSERRAT">MONTSERRAT</option>
                                <option value="MOROCCO">MOROCCO</option>
                                <option value="MOZAMBIQUE">MOZAMBIQUE</option>
                                <option value="NAMIBIA">NAMIBIA</option>
                                <option value="NAURU">NAURU</option>
                                <option value="NEPAL">NEPAL</option>
                                <option value="NETHERLANDS">NETHERLANDS</option>
                                <option value="NETHERLANDS ANTILLES">NETHERLANDS ANTILLES</option>
                                <option value="NEVIS">NEVIS</option>
                                <option value="NEW CALCEDONIA">NEW CALCEDONIA</option>
                                <option value="NEW GUINEA">NEW GUINEA</option>
                                <option value="NEW ZEALAND">NEW ZEALAND</option>
                                <option value="NICARAGUA">NICARAGUA</option>
                                <option value="NIGER">NIGER</option>
                                <option value="NIGERIA">NIGERIA</option>
                                <option value="NIUE">NIUE</option>
                                <option value="NORFOLK ISLAND">NORFOLK ISLAND</option>
                                <option value="NORTH KOREA">NORTH KOREA</option>
                                <option value="NORWAY">NORWAY</option>
                                <option value="OMAN">OMAN</option>
                                <option value="PAKISTAN">PAKISTAN</option>
                                <option value="PALAU">PALAU</option>
                                <option value="PANAMA">PANAMA</option>
                                <option value="PAPAU NEW GUINEA">PAPAU NEW GUINEA</option>
                                <option value="PARAGUAY">PARAGUAY</option>
                                <option value="PERU">PERU</option>
                                <option value="PHILIPPINES">PHILIPPINES</option>
                                <option value="POLAND">POLAND</option>
                                <option value="PORTUGAL">PORTUGAL</option>
                                <option value="PRINCIPE">PRINCIPE</option>
                                <option value="QATAR">QATAR</option>
                                <option value="REPUBLIC OF MACEDONIA">REPUBLIC OF MACEDONIA</option>
                                <option value="REUNION ISLANDS">REUNION ISLANDS</option>
                                <option value="ROMANIA">ROMANIA</option>
                                <option value="RUSSIAN FEDERATION">RUSSIAN FEDERATION</option>
                                <option value="RWANDA">RWANDA</option>
                                <option value="SAIPAN">SAIPAN</option>
                                <option value="SAN MARINO">SAN MARINO</option>
                                <option value="SAO TOME">SAO TOME</option>
                                <option value="SAUDI ARABIA">SAUDI ARABIA</option>
                                <option value="SENEGAL REPUBLIC">SENEGAL REPUBLIC</option>
                                <option value="SERBIA">SERBIA</option>
                                <option value="SEYCHELLES">SEYCHELLES</option>
                                <option value="SIERRA LEONE">SIERRA LEONE</option>
                                <option value="SINGAPORE">SINGAPORE</option>
                                <option value="SLOVAKIA">SLOVAKIA</option>
                                <option value="SLOVENIA">SLOVENIA</option>
                                <option value="SOLOMON ISLANDS">SOLOMON ISLANDS</option>
                                <option value="SOMALIA">SOMALIA</option>
                                <option value="SOUTH AFRICA">SOUTH AFRICA</option>
                                <option value="SOUTH KOREA">SOUTH KOREA</option>
                                <option value="SPAIN">SPAIN</option>
                                <option value="SRI LANKA">SRI LANKA</option>
                                <option value="ST HELENA">ST HELENA</option>
                                <option value="ST KITTS">ST KITTS</option>
                                <option value="ST LUCIA">ST LUCIA</option>
                                <option value="ST PIERRE">ST PIERRE</option>
                                <option value="ST VINCENT">ST VINCENT</option>
                                <option value="SUDAN">SUDAN</option>
                                <option value="SURINAME">SURINAME</option>
                                <option value="SWAZILAND">SWAZILAND</option>
                                <option value="SWEDEN">SWEDEN</option>
                                <option value="SWITZERLAND">SWITZERLAND</option>
                                <option value="SYRIA">SYRIA</option>
                                <option value="TAHITI">TAHITI</option>
                                <option value="TAIWAN ROC">TAIWAN ROC</option>
                                <option value="TAJIKISTAN">TAJIKISTAN</option>
                                <option value="TANZANIA">TANZANIA</option>
                                <option value="THAILAND">THAILAND</option>
                                <option value="TIBET">TIBET</option>
                                <option value="TOGO">TOGO</option>
                                <option value="TONGA">TONGA</option>
                                <option value="TRINIDAD &amp; TOBAGO">TRINIDAD &amp; TOBAGO</option>
                                <option value="TUNISIA">TUNISIA</option>
                                <option value="TURKEY">TURKEY</option>
                                <option value="TURKMENISTAN">TURKMENISTAN</option>
                                <option value="TURKS &amp; CAICOS ISLANDS">TURKS &amp; CAICOS ISLANDS</option>
                                <option value="TUVALU">TUVALU</option>
                                <option value="UGANDA">UGANDA</option>
                                <option value="UKRAINE">UKRAINE</option>
                                <option value="UNITED ARAB EMIRATES">UNITED ARAB EMIRATES</option>
                                <option value="UNITED KINGDOM">UNITED KINGDOM</option>
                                <option value="URUGUAY">URUGUAY</option>
                                <option value="UZBEKISTAN">UZBEKISTAN</option>
                                <option value="VANUATU">VANUATU</option>
                                <option value="VATICAN CITY">VATICAN CITY</option>
                                <option value="VENEZUELA">VENEZUELA</option>
                                <option value="VIETNAM">VIETNAM</option>
                                <option value="WAKE ISLAND">WAKE ISLAND</option>
                                <option value="WALLIS &amp; FUTUNA ISLANDS">WALLIS &amp; FUTUNA ISLANDS</option>
                                <option value="WESTERN SOMOA">WESTERN SOMOA</option>
                                <option value="YEMEN">YEMEN</option>
                                <option value="YUGOSLAVIA">YUGOSLAVIA</option>
                                <option value="ZAIRE">ZAIRE</option>
                                <option value="ZAMBIA">ZAMBIA</option>
                                <option value="ZIMBABWE">ZIMBABWE</option>
                            </select>
                        </p>
                        <p>
                            <label>
                                Phone:</label>
                            <input name="ph" size="30">
                        </p>
                        <p>
                            <label>
                                Fax:</label>
                            <input name="fax" size="30">
                        </p>
                    </div>
                    <br />
                    <div id="questions">
                        <div class="mddi" style="height: 16em;">
                            <img src="http://www.kmpsgroup.com/images/mddi_small.gif" />
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).
                            </label>
                            <br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MDDI_1" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MD&amp;DI E-Newsletter</i></b> - A monthly preview of the upcoming issue of
                                        <i>MD&amp;DI</i> magazine.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MDDI_2" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MD&amp;DI Field Reports</i></b> - A monthly look at the latest headlines,
                                        interviews, and more from the device industry's most trusted source.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MDDI_3" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MD&amp;DI Medtech Decisionmaker Reports</i></b> - A bimonthly report with
                                        a specific focus on key topics for the medical device industry.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MDDI_4" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MD&amp;DI Trade Show Advisor</i></b> (MD&amp;M West, MD&amp;M East, MD&amp;M
                                        Midwest and MD&amp;M Minneapolis) - Three newsletters are sent before each tradeshow,
                                        highlighting the area and previewing the upcoming show and exhibitors.
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="mpmn" style="height: 16em;">
                            <img src="http://www.kmpsgroup.com/images/mpmn_small.gif" />
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MPMN_1" /></td>
                                    <td valign="top">
                                        <b><i>MPMN Product Showcase</i></b> - A twice-monthly e-mail resource spotlighting
                                        the latest products from our sponsors.</td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MPMN_2" /></td>
                                    <td valign="top">
                                        <b><i>MPMN Technology Update</i></b> - A bimonthly newsletter addressing key topics
                                        related to medical device design and manufacturing through featured editorial and
                                        relevant product and service descriptions.</td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MPMN_3" /></td>
                                    <td valign="top">
                                        <b><i>MPMN Trade Show Advisor</i></b> (MD&M West, MD&M East, MD&M Midwest and MD&M
                                        Minneapolis) - Deployed three times prior to each MD&M event, this product provides
                                        in-depth coverage of exhibitors, product previews, and travel tips to ensure a successful
                                        show.</td>
                                </tr>
                            </table>
                        </div>
                        <div class="mx" style="height: 10em;">
                            <img src="http://www.kmpsgroup.com/images/mx_small.gif" />
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).
                            </label>
                            <br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td>
                                        <input type="checkbox" value="y" name="user_MX_1" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MX: Issues Update </i></b> - Medtech news and perspectives from the editors of <i>MX</i> (monthly).
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MX_2" />
                                    </td>
                                    <td valign="top">
                                        <b><i>BIOMEDevice Advisor</i></b> - Your preview to the upcoming BIOMEDevice Forum and Exposition (four weekly issues during the month preceding the event) .
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="mem">
                            <img src="http://www.kmpsgroup.com/images/mem_small.gif" />
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_MEM_1" />
                                    </td>
                                    <td valign="top">
                                        <b><i>MEM E-Newsletter</i></b> - A bimonthly newsletter keeping you connected with
                                        the medical electronics industry.
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ivd">
                            <img src="http://www.kmpsgroup.com/images/ivd_small.gif" />
                            <label>
                                TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                            <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_IVD_1" />
                                    </td>
                                    <td valign="top">
                                        <b><i>IVDT E-Newsletter</i></b> - A monthly preview of the upcoming issue of IVD Technology magazine.
                                    </td>
                                </tr>
                                 <tr>
                                    <td valign="top">
                                        <input type="checkbox" value="y" name="user_IVD_2" />
                                    </td>
                                    <td valign="top">
                                        <b><i>IVDT Product Showcase </i></b> - The latest products being offered by suppliers to the IVD industry.
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="logos">
                        <h4>
                            For more opportunities to subscribe to other digital products offered<br />
                            by Canon Communications, please click on the logos below.</h4>
                        <img src="http://www.kmpsgroup.com/images/mddi_small.gif" class="mddi" />
                        <img src="http://www.kmpsgroup.com/images/mpmn_small.gif" class="mpmn" />
                        <img src="http://www.kmpsgroup.com/images/mem_small.gif" class="mem" />
                        <img src="http://www.kmpsgroup.com/images/mx_small.gif" class="mx" />
                        <img src="http://www.kmpsgroup.com/images/ivd_small.gif" class="ivd" />
                    </div>
                    <input type="hidden" name="s" value="S" />
                    <input type="hidden" name="f" value="html" />
                    <input type="hidden" value="12911" name="g" />
                    <input type="hidden" value="1797" name="c" />
                    <input type="hidden" value="687" name="sfID" />
                    <input type="submit" value="Submit" id="submit" />
                    <input type="reset" value="Reset" />
                </form>
            </div>
            <div id="footer">
                <p>
                    * Indicates required fields for subscription</p>
                <p>
                    &copy; Copyright

                    <script type="text/javascript"> 
				var currentTime = new Date();
				var year = currentTime.getFullYear()
				document.write(year);
                    </script>

                    Canon Communications LLC</p>
            </div>
        </div>
    </div>
</body>
</html>
