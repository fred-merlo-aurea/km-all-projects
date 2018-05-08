<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Inherits="CanonESubscriptionForm.forms.EditorialformHandler" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Canon Communications - Editorial Form Signup</title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

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
	
	$(section).show('slow');
	$('#questions div').addClass('showLogos');
}

$(document).ready(function()
{
    var defaultMagazine = $.jqURL.get("mag");


    //hide all questions by default
    $('#questions div').hide();

    if (defaultMagazine != undefined)
    {
        defaultMagazine = defaultMagazine.toLowerCase();
        //var ccqsection = "#current div.ccq";
        //$(ccqsection).remove();    	

        var section = "#questions div." + defaultMagazine;
        var inSection = $(section).html();
        var logo = "";
        if (inSection != null)
        {
                var inSection = '<div class="' + defaultMagazine + '">' + inSection + '</div';
                $(section).remove(); //remove from original position
                $('#current').append(inSection);
                $(section).hide(); //hide element for binding effect	

                var defaultLogo = '#logos img.' + defaultMagazine;
                $(defaultLogo).hide();

            if (defaultMagazine == 'mtp')
            {
                logo = '#logos img.emd';
                $(logo).hide();
                logo = '#logos img.cmdm';
                $(logo).hide();
                logo = '#logos img.mdt';
                $(logo).hide();
                logo = '#logos img.mx';
                $(logo).hide();

            }
            else if (defaultMagazine == 'mx')
            {
                logo = '#logos img.emd';
                $(logo).hide();
                logo = '#logos img.cmdm';
                $(logo).hide();
                logo = '#logos img.mdt';
                $(logo).hide();

            }
            else if (defaultMagazine == 'emd' || defaultMagazine == 'cmdm' || defaultMagazine == 'mdt')
            {
                logo = '#logos img.mx';
                $(logo).hide();
                logo = '#logos img.mtp';
                $(logo).hide();
            }
        }
    }
    else
    {
    }

    //adds click function to logo images and uses img class to show correponding questions
    $('#logos img').click(function()
    {
        showQuestions(this.className);
    });
});

function hidepubs(p)
{
}

function validateEmail()
{
    var allOk = false;
    allOk = svValidator("txtEmail", document.forms[0].txtEmail.value);
    
    // Validate zip against state
	if (allOk) 
	{
        var x = document.forms[0].txtEmail.value;

        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(x))
        {	
	        alert('Invalid Email address');
	        allOk = false;
        }
    }
            
    if (!allOk)
        document.forms[0].txtEmail.focus();
    
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
                <form name="frmsub1" id="frmsub1" runat="server">
                    <br />
                    <div id="current">
                        <asp:Panel ID="pnlCurrent" runat="server" Visible="false">
                        </asp:Panel>
                    </div>
                    <div id="profile">
                        <asp:Panel ID="pnlText1" Visible="true" runat="server">
                            <p>
                                <label>
                                </label>
                                Please enter your email address to begin:</p>
                        </asp:Panel>
                        <p>
                            <label>
                                * Email:</label>
                            <asp:TextBox runat="server" ID="txtEmail" Width="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=">> requried"
                                ControlToValidate="txtEmail" Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            <asp:Button ID="btnEmailClick" runat="server" Visible="true" Text="Search for my profile"
                                OnClick="btnEmailClick_Click" Width="150" /><br />
                        </p>
                        <asp:Panel ID="pnlText2" Visible="true" runat="server">
                            <p>
                                <label>
                                </label>
                                Note: If your email address is in our database- you can update your account.<br />
                                <label>
                                </label>
                                If new - you can register on the next page.<br /><br />
                                <label>
                                </label>
                                 <script type="text/javascript">
                                     var pub = $.jqURL.get("mag");

                                     if (pub) {
                                         pub = pub.toUpperCase();

                                         if (pub == 'EMD')
                                             document.write('<font style="font-weight:bold; text-decoration:none"><a href="http://www.kmpsgroup.com/subforms/EMDM_home.htm">Click here to sign up for EMDM print/digital magazine</a></font>');
                                         else if (pub != 'MX')
                                             document.write('<font style="font-weight:bold; text-decoration:none"><a href="http://www.kmpsgroup.com/subforms/' + pub + '_home.htm">Click here to sign up for ' + pub + ' print/digital magazine</a></font>');
                                     }                         
            </script>
                                </p>
                               
                        </asp:Panel>
                        <asp:Panel ID="pnlProfile" Visible="false" runat="server">
                            <p>
                                <label>
                                    * First Name:</label><asp:TextBox runat="server" ID="fn" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage=">> requried" ControlToValidate="fn"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * Last Name:</label><asp:TextBox runat="server" ID="ln" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator3" runat="server" ErrorMessage=">> requried" ControlToValidate="ln"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * Job Title:</label><asp:TextBox runat="server" ID="t" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator4" runat="server" ErrorMessage=">> requried" ControlToValidate="t"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * Company:</label><asp:TextBox runat="server" ID="compname" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator5" runat="server" ErrorMessage=">> requried" ControlToValidate="compname"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * Address:</label><asp:TextBox runat="server" ID="adr" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator6" runat="server" ErrorMessage=">> requried" ControlToValidate="adr"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    Address 2:</label><asp:TextBox runat="server" ID="adr2" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * City:</label><asp:TextBox runat="server" ID="city" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator7" runat="server" ErrorMessage=">> requried" ControlToValidate="city"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * State/Province:</label>
                                <asp:DropDownList ID="state" runat="server">
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage=">> requried"
                                    InitialValue="" ControlToValidate="state" Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p class="zip">
                                <label>
                                    * Postcode:</label><asp:TextBox runat="server" ID="zc" Width="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator8" runat="server" ErrorMessage=">> requried" ControlToValidate="zc"
                                        Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p class="country">
                                <label>
                                    Country:</label>
                                <asp:DropDownList ID="ctry" runat="server">
                                    <asp:ListItem Value="">--- Select Country --- </asp:ListItem>
                                    <asp:ListItem Value="USA">UNITED STATES OF AMERICA</asp:ListItem>
                                    <asp:ListItem Value="ALBANIA">ALBANIA</asp:ListItem>
                                    <asp:ListItem Value="ALGERIA">ALGERIA</asp:ListItem>
                                    <asp:ListItem Value="AMERICAN SAMOA">AMERICAN SAMOA</asp:ListItem>
                                    <asp:ListItem Value="ANDORRA">ANDORRA</asp:ListItem>
                                    <asp:ListItem Value="ANGOLA">ANGOLA</asp:ListItem>
                                    <asp:ListItem Value="ANTARCTICA">ANTARCTICA</asp:ListItem>
                                    <asp:ListItem Value="ANTIGUA">ANTIGUA</asp:ListItem>
                                    <asp:ListItem Value="ARGENTINA">ARGENTINA</asp:ListItem>
                                    <asp:ListItem Value="ARMENIA">ARMENIA</asp:ListItem>
                                    <asp:ListItem Value="ARUBA">ARUBA</asp:ListItem>
                                    <asp:ListItem Value="ASCENSION">ASCENSION</asp:ListItem>
                                    <asp:ListItem Value="AUSTRALIA">AUSTRALIA</asp:ListItem>
                                    <asp:ListItem Value="AUSTRIA">AUSTRIA</asp:ListItem>
                                    <asp:ListItem Value="AZERBAIJAN">AZERBAIJAN</asp:ListItem>
                                    <asp:ListItem Value="BAHAMAS">BAHAMAS</asp:ListItem>
                                    <asp:ListItem Value="BAHRAIN">BAHRAIN</asp:ListItem>
                                    <asp:ListItem Value="BANGLADESH">BANGLADESH</asp:ListItem>
                                    <asp:ListItem Value="BARBADOS">BARBADOS</asp:ListItem>
                                    <asp:ListItem Value="BELARUS">BELARUS</asp:ListItem>
                                    <asp:ListItem Value="BELGIUM">BELGIUM</asp:ListItem>
                                    <asp:ListItem Value="BELIZE">BELIZE</asp:ListItem>
                                    <asp:ListItem Value="BENIN">BENIN</asp:ListItem>
                                    <asp:ListItem Value="BERMUDA">BERMUDA</asp:ListItem>
                                    <asp:ListItem Value="BHUTAN">BHUTAN</asp:ListItem>
                                    <asp:ListItem Value="BOLIVIA">BOLIVIA</asp:ListItem>
                                    <asp:ListItem Value="BOTSWANA">BOTSWANA</asp:ListItem>
                                    <asp:ListItem Value="BOZNIA-HERZEGOVINA">BOZNIA-HERZEGOVINA</asp:ListItem>
                                    <asp:ListItem Value="BRAZIL">BRAZIL</asp:ListItem>
                                    <asp:ListItem Value="BRITISH VIRGIN ISLANDS">BRITISH VIRGIN ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="BRITISH WEST INDIES">BRITISH WEST INDIES</asp:ListItem>
                                    <asp:ListItem Value="BRUNEI">BRUNEI</asp:ListItem>
                                    <asp:ListItem Value="BULGARIA">BULGARIA</asp:ListItem>
                                    <asp:ListItem Value="BURKINA FASO">BURKINA FASO</asp:ListItem>
                                    <asp:ListItem Value="BURMA">BURMA</asp:ListItem>
                                    <asp:ListItem Value="BURUNDI">BURUNDI</asp:ListItem>
                                    <asp:ListItem Value="CAMBODIA">CAMBODIA</asp:ListItem>
                                    <asp:ListItem Value="CAMEROON">CAMEROON</asp:ListItem>
                                    <asp:ListItem Value="CANADA">CANADA</asp:ListItem>
                                    <asp:ListItem Value="CANARY ISLANDS">CANARY ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="CAPE VERDE ISLANDS">CAPE VERDE ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="CAYMAN ISLANDS">CAYMAN ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="CENTRAL AFRICAN REPUBLIC">CENTRAL AFRICAN REPUBLIC</asp:ListItem>
                                    <asp:ListItem Value="CHAD">CHAD</asp:ListItem>
                                    <asp:ListItem Value="CHANNEL ISLANDS">CHANNEL ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="CHATHAM ISLAND">CHATHAM ISLAND</asp:ListItem>
                                    <asp:ListItem Value="CHILE">CHILE</asp:ListItem>
                                    <asp:ListItem Value="CHINA">CHINA</asp:ListItem>
                                    <asp:ListItem Value="CHRISTMAS ISLAND">CHRISTMAS ISLAND</asp:ListItem>
                                    <asp:ListItem Value="CIS">CIS</asp:ListItem>
                                    <asp:ListItem Value="COCOS ISLANDS">COCOS ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="COLOMBIA">COLOMBIA</asp:ListItem>
                                    <asp:ListItem Value="COMOROS">COMOROS</asp:ListItem>
                                    <asp:ListItem Value="CONGO">CONGO</asp:ListItem>
                                    <asp:ListItem Value="COOK ISLANDS">COOK ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="COSTA RICA">COSTA RICA</asp:ListItem>
                                    <asp:ListItem Value="CROATIA">CROATIA</asp:ListItem>
                                    <asp:ListItem Value="CUBA">CUBA</asp:ListItem>
                                    <asp:ListItem Value="CURACO">CURACO</asp:ListItem>
                                    <asp:ListItem Value="CYPRUS">CYPRUS</asp:ListItem>
                                    <asp:ListItem Value="CZECH REPUBLIC">CZECH REPUBLIC</asp:ListItem>
                                    <asp:ListItem Value="DENMARK">DENMARK</asp:ListItem>
                                    <asp:ListItem Value="DIEGO GARCIA">DIEGO GARCIA</asp:ListItem>
                                    <asp:ListItem Value="DJIBOUTI">DJIBOUTI</asp:ListItem>
                                    <asp:ListItem Value="DOMINICA">DOMINICA</asp:ListItem>
                                    <asp:ListItem Value="DOMINICIAN REPUBLIC">DOMINICIAN REPUBLIC</asp:ListItem>
                                    <asp:ListItem Value="EASTER ISLAND">EASTER ISLAND</asp:ListItem>
                                    <asp:ListItem Value="ECUADOR">ECUADOR</asp:ListItem>
                                    <asp:ListItem Value="EGYPT">EGYPT</asp:ListItem>
                                    <asp:ListItem Value="EL SALVADOR">EL SALVADOR</asp:ListItem>
                                    <asp:ListItem Value="EQUATORIAL GUINEA">EQUATORIAL GUINEA</asp:ListItem>
                                    <asp:ListItem Value="ERITREA">ERITREA</asp:ListItem>
                                    <asp:ListItem Value="ESTONIA">ESTONIA</asp:ListItem>
                                    <asp:ListItem Value="ETHIOPIA">ETHIOPIA</asp:ListItem>
                                    <asp:ListItem Value="FAEROE ISLANDS">FAEROE ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="FALKLAND ISLANDS">FALKLAND ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="FIJI">FIJI</asp:ListItem>
                                    <asp:ListItem Value="FINLAND">FINLAND</asp:ListItem>
                                    <asp:ListItem Value="FRANCE">FRANCE</asp:ListItem>
                                    <asp:ListItem Value="FRENCH ANTILLES">FRENCH ANTILLES</asp:ListItem>
                                    <asp:ListItem Value="FRENCH GUIANA">FRENCH GUIANA</asp:ListItem>
                                    <asp:ListItem Value="FRENCH POLYNESIA">FRENCH POLYNESIA</asp:ListItem>
                                    <asp:ListItem Value="FSM">FSM</asp:ListItem>
                                    <asp:ListItem Value="GABON">GABON</asp:ListItem>
                                    <asp:ListItem Value="GAMBIA">GAMBIA</asp:ListItem>
                                    <asp:ListItem Value="GEORGIA">GEORGIA</asp:ListItem>
                                    <asp:ListItem Value="GERMANY">GERMANY</asp:ListItem>
                                    <asp:ListItem Value="GHANA">GHANA</asp:ListItem>
                                    <asp:ListItem Value="GIBRALTAR">GIBRALTAR</asp:ListItem>
                                    <asp:ListItem Value="GINEA-BISSAU">GINEA-BISSAU</asp:ListItem>
                                    <asp:ListItem Value="GREECE">GREECE</asp:ListItem>
                                    <asp:ListItem Value="GREENLAND">GREENLAND</asp:ListItem>
                                    <asp:ListItem Value="GRENADA">GRENADA</asp:ListItem>
                                    <asp:ListItem Value="GRENADIN ISLANDS">GRENADIN ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="GUADELOUPE">GUADELOUPE</asp:ListItem>
                                    <asp:ListItem Value="GUANTANAMO BAY">GUANTANAMO BAY</asp:ListItem>
                                    <asp:ListItem Value="GUATEMALA">GUATEMALA</asp:ListItem>
                                    <asp:ListItem Value="GUINEA">GUINEA</asp:ListItem>
                                    <asp:ListItem Value="GUYANA">GUYANA</asp:ListItem>
                                    <asp:ListItem Value="HAITI">HAITI</asp:ListItem>
                                    <asp:ListItem Value="HONDURAS">HONDURAS</asp:ListItem>
                                    <asp:ListItem Value="HONG KONG-SAR">HONG KONG-SAR</asp:ListItem>
                                    <asp:ListItem Value="HUNGARY">HUNGARY</asp:ListItem>
                                    <asp:ListItem Value="ICELAND">ICELAND</asp:ListItem>
                                    <asp:ListItem Value="INDIA">INDIA</asp:ListItem>
                                    <asp:ListItem Value="INDONESIA">INDONESIA</asp:ListItem>
                                    <asp:ListItem Value="IRAN">IRAN</asp:ListItem>
                                    <asp:ListItem Value="IRAQ">IRAQ</asp:ListItem>
                                    <asp:ListItem Value="IRELAND, REPUBLIC OF">IRELAND, REPUBLIC OF</asp:ListItem>
                                    <asp:ListItem Value="ISLE OF MAN">ISLE OF MAN</asp:ListItem>
                                    <asp:ListItem Value="ISRAEL">ISRAEL</asp:ListItem>
                                    <asp:ListItem Value="ITALY">ITALY</asp:ListItem>
                                    <asp:ListItem Value="IVORY COAST">IVORY COAST</asp:ListItem>
                                    <asp:ListItem Value="JAMAICA">JAMAICA</asp:ListItem>
                                    <asp:ListItem Value="JAPAN">JAPAN</asp:ListItem>
                                    <asp:ListItem Value="JORDAN">JORDAN</asp:ListItem>
                                    <asp:ListItem Value="KAZAKHSTAN">KAZAKHSTAN</asp:ListItem>
                                    <asp:ListItem Value="KENYA">KENYA</asp:ListItem>
                                    <asp:ListItem Value="KIRIBATI">KIRIBATI</asp:ListItem>
                                    <asp:ListItem Value="KUWAIT">KUWAIT</asp:ListItem>
                                    <asp:ListItem Value="KYRGYSTAN">KYRGYSTAN</asp:ListItem>
                                    <asp:ListItem Value="LAOS">LAOS</asp:ListItem>
                                    <asp:ListItem Value="LATVIA">LATVIA</asp:ListItem>
                                    <asp:ListItem Value="LEBANON">LEBANON</asp:ListItem>
                                    <asp:ListItem Value="LESOTHO">LESOTHO</asp:ListItem>
                                    <asp:ListItem Value="LIBERIA">LIBERIA</asp:ListItem>
                                    <asp:ListItem Value="LIBYA">LIBYA</asp:ListItem>
                                    <asp:ListItem Value="LIECHTENSTEIN">LIECHTENSTEIN</asp:ListItem>
                                    <asp:ListItem Value="LITHUANIA">LITHUANIA</asp:ListItem>
                                    <asp:ListItem Value="LUXEMBOURG">LUXEMBOURG</asp:ListItem>
                                    <asp:ListItem Value="MACAO">MACAO</asp:ListItem>
                                    <asp:ListItem Value="MACEDONIA">MACEDONIA</asp:ListItem>
                                    <asp:ListItem Value="MADAGASCAR">MADAGASCAR</asp:ListItem>
                                    <asp:ListItem Value="MALAWI">MALAWI</asp:ListItem>
                                    <asp:ListItem Value="MALAYSIA">MALAYSIA</asp:ListItem>
                                    <asp:ListItem Value="MALDIVES REPUBLIC">MALDIVES REPUBLIC</asp:ListItem>
                                    <asp:ListItem Value="MALI">MALI</asp:ListItem>
                                    <asp:ListItem Value="MALTA">MALTA</asp:ListItem>
                                    <asp:ListItem Value="MARSHALL ISLANDS">MARSHALL ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="MARTINIQUE">MARTINIQUE</asp:ListItem>
                                    <asp:ListItem Value="MAURITANIA">MAURITANIA</asp:ListItem>
                                    <asp:ListItem Value="MAURITIUS">MAURITIUS</asp:ListItem>
                                    <asp:ListItem Value="MAYOTTE">MAYOTTE</asp:ListItem>
                                    <asp:ListItem Value="MEXICO">MEXICO</asp:ListItem>
                                    <asp:ListItem Value="MIDWAY ISLANDS">MIDWAY ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="MIQUELON">MIQUELON</asp:ListItem>
                                    <asp:ListItem Value="MOLDOVA">MOLDOVA</asp:ListItem>
                                    <asp:ListItem Value="MONACO">MONACO</asp:ListItem>
                                    <asp:ListItem Value="MONGOLIA">MONGOLIA</asp:ListItem>
                                    <asp:ListItem Value="MONTSERRAT">MONTSERRAT</asp:ListItem>
                                    <asp:ListItem Value="MOROCCO">MOROCCO</asp:ListItem>
                                    <asp:ListItem Value="MOZAMBIQUE">MOZAMBIQUE</asp:ListItem>
                                    <asp:ListItem Value="NAMIBIA">NAMIBIA</asp:ListItem>
                                    <asp:ListItem Value="NAURU">NAURU</asp:ListItem>
                                    <asp:ListItem Value="NEPAL">NEPAL</asp:ListItem>
                                    <asp:ListItem Value="NETHERLANDS">NETHERLANDS</asp:ListItem>
                                    <asp:ListItem Value="NETHERLANDS ANTILLES">NETHERLANDS ANTILLES</asp:ListItem>
                                    <asp:ListItem Value="NEVIS">NEVIS</asp:ListItem>
                                    <asp:ListItem Value="NEW CALCEDONIA">NEW CALCEDONIA</asp:ListItem>
                                    <asp:ListItem Value="NEW GUINEA">NEW GUINEA</asp:ListItem>
                                    <asp:ListItem Value="NEW ZEALAND">NEW ZEALAND</asp:ListItem>
                                    <asp:ListItem Value="NICARAGUA">NICARAGUA</asp:ListItem>
                                    <asp:ListItem Value="NIGER">NIGER</asp:ListItem>
                                    <asp:ListItem Value="NIGERIA">NIGERIA</asp:ListItem>
                                    <asp:ListItem Value="NIUE">NIUE</asp:ListItem>
                                    <asp:ListItem Value="NORFOLK ISLAND">NORFOLK ISLAND</asp:ListItem>
                                    <asp:ListItem Value="NORTH KOREA">NORTH KOREA</asp:ListItem>
                                    <asp:ListItem Value="NORWAY">NORWAY</asp:ListItem>
                                    <asp:ListItem Value="OMAN">OMAN</asp:ListItem>
                                    <asp:ListItem Value="PAKISTAN">PAKISTAN</asp:ListItem>
                                    <asp:ListItem Value="PALAU">PALAU</asp:ListItem>
                                    <asp:ListItem Value="PANAMA">PANAMA</asp:ListItem>
                                    <asp:ListItem Value="PAPAU NEW GUINEA">PAPAU NEW GUINEA</asp:ListItem>
                                    <asp:ListItem Value="PARAGUAY">PARAGUAY</asp:ListItem>
                                    <asp:ListItem Value="PERU">PERU</asp:ListItem>
                                    <asp:ListItem Value="PHILIPPINES">PHILIPPINES</asp:ListItem>
                                    <asp:ListItem Value="POLAND">POLAND</asp:ListItem>
                                    <asp:ListItem Value="PORTUGAL">PORTUGAL</asp:ListItem>
                                    <asp:ListItem Value="PRINCIPE">PRINCIPE</asp:ListItem>
                                    <asp:ListItem Value="QATAR">QATAR</asp:ListItem>
                                    <asp:ListItem Value="REPUBLIC OF MACEDONIA">REPUBLIC OF MACEDONIA</asp:ListItem>
                                    <asp:ListItem Value="REUNION ISLANDS">REUNION ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="ROMANIA">ROMANIA</asp:ListItem>
                                    <asp:ListItem Value="RUSSIAN FEDERATION">RUSSIAN FEDERATION</asp:ListItem>
                                    <asp:ListItem Value="RWANDA">RWANDA</asp:ListItem>
                                    <asp:ListItem Value="SAIPAN">SAIPAN</asp:ListItem>
                                    <asp:ListItem Value="SAN MARINO">SAN MARINO</asp:ListItem>
                                    <asp:ListItem Value="SAO TOME">SAO TOME</asp:ListItem>
                                    <asp:ListItem Value="SAUDI ARABIA">SAUDI ARABIA</asp:ListItem>
                                    <asp:ListItem Value="SENEGAL REPUBLIC">SENEGAL REPUBLIC</asp:ListItem>
                                    <asp:ListItem Value="SERBIA">SERBIA</asp:ListItem>
                                    <asp:ListItem Value="SEYCHELLES">SEYCHELLES</asp:ListItem>
                                    <asp:ListItem Value="SIERRA LEONE">SIERRA LEONE</asp:ListItem>
                                    <asp:ListItem Value="SINGAPORE">SINGAPORE</asp:ListItem>
                                    <asp:ListItem Value="SLOVAKIA">SLOVAKIA</asp:ListItem>
                                    <asp:ListItem Value="SLOVENIA">SLOVENIA</asp:ListItem>
                                    <asp:ListItem Value="SOLOMON ISLANDS">SOLOMON ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="SOMALIA">SOMALIA</asp:ListItem>
                                    <asp:ListItem Value="SOUTH AFRICA">SOUTH AFRICA</asp:ListItem>
                                    <asp:ListItem Value="SOUTH KOREA">SOUTH KOREA</asp:ListItem>
                                    <asp:ListItem Value="SPAIN">SPAIN</asp:ListItem>
                                    <asp:ListItem Value="SRI LANKA">SRI LANKA</asp:ListItem>
                                    <asp:ListItem Value="ST HELENA">ST HELENA</asp:ListItem>
                                    <asp:ListItem Value="ST KITTS">ST KITTS</asp:ListItem>
                                    <asp:ListItem Value="ST LUCIA">ST LUCIA</asp:ListItem>
                                    <asp:ListItem Value="ST PIERRE">ST PIERRE</asp:ListItem>
                                    <asp:ListItem Value="ST VINCENT">ST VINCENT</asp:ListItem>
                                    <asp:ListItem Value="SUDAN">SUDAN</asp:ListItem>
                                    <asp:ListItem Value="SURINAME">SURINAME</asp:ListItem>
                                    <asp:ListItem Value="SWAZILAND">SWAZILAND</asp:ListItem>
                                    <asp:ListItem Value="SWEDEN">SWEDEN</asp:ListItem>
                                    <asp:ListItem Value="SWITZERLAND">SWITZERLAND</asp:ListItem>
                                    <asp:ListItem Value="SYRIA">SYRIA</asp:ListItem>
                                    <asp:ListItem Value="TAHITI">TAHITI</asp:ListItem>
                                    <asp:ListItem Value="TAIWAN ROC">TAIWAN ROC</asp:ListItem>
                                    <asp:ListItem Value="TAJIKISTAN">TAJIKISTAN</asp:ListItem>
                                    <asp:ListItem Value="TANZANIA">TANZANIA</asp:ListItem>
                                    <asp:ListItem Value="THAILAND">THAILAND</asp:ListItem>
                                    <asp:ListItem Value="TIBET">TIBET</asp:ListItem>
                                    <asp:ListItem Value="TOGO">TOGO</asp:ListItem>
                                    <asp:ListItem Value="TONGA">TONGA</asp:ListItem>
                                    <asp:ListItem Value="TRINIDAD &amp; TOBAGO">TRINIDAD &amp; TOBAGO</asp:ListItem>
                                    <asp:ListItem Value="TUNISIA">TUNISIA</asp:ListItem>
                                    <asp:ListItem Value="TURKEY">TURKEY</asp:ListItem>
                                    <asp:ListItem Value="TURKMENISTAN">TURKMENISTAN</asp:ListItem>
                                    <asp:ListItem Value="TURKS &amp; CAICOS ISLANDS">TURKS &amp; CAICOS ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="TUVALU">TUVALU</asp:ListItem>
                                    <asp:ListItem Value="UGANDA">UGANDA</asp:ListItem>
                                    <asp:ListItem Value="UKRAINE">UKRAINE</asp:ListItem>
                                    <asp:ListItem Value="UNITED ARAB EMIRATES">UNITED ARAB EMIRATES</asp:ListItem>
                                    <asp:ListItem Value="UNITED KINGDOM">UNITED KINGDOM</asp:ListItem>
                                    <asp:ListItem Value="URUGUAY">URUGUAY</asp:ListItem>
                                    <asp:ListItem Value="UZBEKISTAN">UZBEKISTAN</asp:ListItem>
                                    <asp:ListItem Value="VANUATU">VANUATU</asp:ListItem>
                                    <asp:ListItem Value="VATICAN CITY">VATICAN CITY</asp:ListItem>
                                    <asp:ListItem Value="VENEZUELA">VENEZUELA</asp:ListItem>
                                    <asp:ListItem Value="VIETNAM">VIETNAM</asp:ListItem>
                                    <asp:ListItem Value="WAKE ISLAND">WAKE ISLAND</asp:ListItem>
                                    <asp:ListItem Value="WALLIS &amp; FUTUNA ISLANDS">WALLIS &amp; FUTUNA ISLANDS</asp:ListItem>
                                    <asp:ListItem Value="WESTERN SOMOA">WESTERN SOMOA</asp:ListItem>
                                    <asp:ListItem Value="YEMEN">YEMEN</asp:ListItem>
                                    <asp:ListItem Value="YUGOSLAVIA">YUGOSLAVIA</asp:ListItem>
                                    <asp:ListItem Value="ZAIRE">ZAIRE</asp:ListItem>
                                    <asp:ListItem Value="ZAMBIA">ZAMBIA</asp:ListItem>
                                    <asp:ListItem Value="ZIMBABWE">ZIMBABWE</asp:ListItem>
                                </asp:DropDownList>
                            </p>
                            <p>
                                <label>
                                    Phone:</label><asp:TextBox runat="server" ID="ph" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    Fax:</label><asp:TextBox runat="server" ID="fax" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * What is your Type of Business?</label>
                                <asp:DropDownList ID="user_business" runat="server">
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value='A'>Medical Mfg</asp:ListItem>
                                    <asp:ListItem Value='B'>IVD Mfg</asp:ListItem>
                                    <asp:ListItem Value='C'>Pharmaceutical Mfg</asp:ListItem>
                                    <asp:ListItem Value='D'>Biotech Company</asp:ListItem>
                                    <asp:ListItem Value='E'>Plastics Mfg</asp:ListItem>
                                    <asp:ListItem Value='F'>Packaging</asp:ListItem>
                                    <asp:ListItem Value='G'>Electronics</asp:ListItem>
                                    <asp:ListItem Value='H'>Appliance Mfg</asp:ListItem>
                                    <asp:ListItem Value='I'>Food/Beverage Mfg</asp:ListItem>
                                    <asp:ListItem Value='J'>Vitamin/Supplement Mfg</asp:ListItem>
                                    <asp:ListItem Value='K'>Other Mfg</asp:ListItem>
                                    <asp:ListItem Value='L'>PROCESSOR - Injection Molding</asp:ListItem>
                                    <asp:ListItem Value='M'>PROCESSOR - Extrusion</asp:ListItem>
                                    <asp:ListItem Value='N'>PROCESSOR - Blow Molding</asp:ListItem>
                                    <asp:ListItem Value='O'>PROCESSOR - Thermoforming</asp:ListItem>
                                    <asp:ListItem Value='P'>PROCESSOR - Powder/Bulk Solids</asp:ListItem>
                                    <asp:ListItem Value='Q'>PROCESSOR - Other</asp:ListItem>
                                    <asp:ListItem Value='R'>Mold/Die Making</asp:ListItem>
                                    <asp:ListItem Value='S'>Contract Manufacturer</asp:ListItem>
                                    <asp:ListItem Value='T'>Ancillary Products/Services</asp:ListItem>
                                    <asp:ListItem Value='U'>Government/Academic</asp:ListItem>
                                    <asp:ListItem Value='V'>Consultant</asp:ListItem>
                                    <asp:ListItem Value='W'>Distributor/Mfr rep</asp:ListItem>
                                    <asp:ListItem Value='Z'>Other</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage=">> requried"
                                    ControlToValidate="user_business" InitialValue="" Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                            <p>
                                <label>
                                    * What is your Job Function?</label>
                                <asp:DropDownList ID="user_function" runat="server">
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value="A">Gen/Corp Mgmt</asp:ListItem>
                                    <asp:ListItem Value="B">Design/Project/Process Engineering</asp:ListItem>
                                    <asp:ListItem Value="C">Engineering, Other</asp:ListItem>
                                    <asp:ListItem Value="D">Package Design</asp:ListItem>
                                    <asp:ListItem Value="E">QA/QC</asp:ListItem>
                                    <asp:ListItem Value="F">R&D</asp:ListItem>
                                    <asp:ListItem Value="G">Purchasing</asp:ListItem>
                                    <asp:ListItem Value="H">Sales</asp:ListItem>
                                    <asp:ListItem Value="I">Marketing</asp:ListItem>
                                    <asp:ListItem Value="J">Plant/Production/Manufacturing</asp:ListItem>
                                    <asp:ListItem Value="K">Regulatory</asp:ListItem>
                                    <asp:ListItem Value="L">Tech Services</asp:ListItem>
                                    <asp:ListItem Value="M">Consultant</asp:ListItem>
                                    <asp:ListItem Value="Z">Other</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage=">> requried"
                                    InitialValue="" ControlToValidate="user_function" Font-Size="X-Small"></asp:RequiredFieldValidator>&nbsp;
                            </p>
                        </asp:Panel>
                    </div>
                    <br />
                    <div id="questions" style="text-align: center">
                        <asp:Panel ID="pnlquestions" runat="server" Visible="false">
                           
                            <div class="mx" style="visibility:hidden;height: 10em;">
                                <img src="http://www.kmpsgroup.com/images/mx_small.gif" />
                                <label>
                                    TO SUBSCRIBE, please choose your options below (check all that apply).
                                </label>
                                <br />
                                <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" name="g_17876" id="g_17876" runat="server" value="y" />
                                        </td>
                                        <td valign="top">
                                            <b><i>MX: Issues Update</i></b> - is a monthly e-newsletter serving the business
                                            needs of executive-level medical technology decision makers around the globe, providing
                                            medtech news and perspectives from the editors of <i>MX</i>. With a focus on timely information
                                            and complementary features about current business trends and policy issues associated
                                            with running a medical technology business, <i>MX: Issues Update</i> is especially designed
                                            to meet the needs of busy executives in medical technology companies worldwide.
                                            Key areas of coverage encompass the full spectrum of medtech business activities from
                                            executive recruitment all the way to product distribution including market intelligence,
                                            corporate funding, intellectual property management, business planning, information
                                            technologies, sales and advertising and a whole lot more. <i>MX: Issues Update</i> reaches
                                            an audience of more than 15,000 medical technology CEOs, presidents, CFOs, and other 
                                            executives plus hundreds of leaders in the medtech investment and financial communities.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="mtp"  style="visibility:hidden">
                                <img src="http://www.kmpsgroup.com/images/mtp_small.gif" />
                                <label>
                                    TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                                <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                                           
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" name="g_18512" id="g_18512" runat="server" value="y" />
                                        </td>
                                        <td valign="top">
                                            <b><i>Med-Tech Precision E-Newsletter </i></b>- A bi-monthly update on the latest
                                            news in the orthopedic, cardiological, and surgical precision manufacturing sectors.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" name="g_22546" id="g_22546" runat="server" value="y" />
                                        </td>
                                        <td valign="top">
                                            <b><i>MTP Tech Update </i></b>- The MTP Tech Update focuses on the latest cutting edge news in three key areas of medical device manufacturing - orthopedics, cardiological, and surgical.
                                        </td>
                                    </tr>
                                             
                                </table>
                            </div>
                            <div class="emd"  style="visibility:hidden">
                                <img src="http://www.kmpsgroup.com/images/emd_small.gif" />
                                <label>
                                    TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                                <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_17868" id="g_17868" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b>medtech<i>insider</i><i> Weekly Edition</i></b> - News and commentary culled from the medtech<i>insider</i>.com blog.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_18587" id="g_18587" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b><i>European Med-Tech News E-Newsletter</i></b> - A preview of the upcoming issues of European Medical Device Manufacturer and Medical Device Technology magazines. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_17877" id="g_17877" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b><i>Critical Products</i></b> - Sponsored product and service announcements distributed
                                            three times a year.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_24185" id="g_24185" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b>medtech<i>insider</i><i> 14-t&auml;glicher Newsletter</i></b> - Ausgew&auml;hlte Nachrichten und Kommentare aus dem Blog medtech<i>insider</i>.de.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="cmdm"  style="visibility:hidden">
                                <img src="http://www.kmpsgroup.com/images/CMDM_small.jpg" />
                                <label>
                                    TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                                <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_17875" id="g_17875" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b><i>CMDM E-Newsletter</i></b> - Published five times a year, the Chinese-language
                                            CMDM e-newsletter makes available peer-reviewed technical articles and technology
                                            and industry news to China's medical device industry.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_17873" id="g_17873" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b><i>CMDM Product Showcase</i></b> - Sponsored product and service announcements
                                            distributed three times a year.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="mdt"  style="visibility:hidden">
                                <img src="http://www.kmpsgroup.com/images/mdt_small.gif" />
                                <label>
                                    TO SUBSCRIBE, please choose your options below (check all that apply).</label><br />
                                <table width="100%" cellpadding="5" cellspacing="1" border="0">
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="g_17867" id="g_17867" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <b><i>Technology Update</i></b> - Distributed three times a year, each Technology
                                            Update provides in-depth information on a specific product and service category
                                            relevant to medical manufacturing.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Font-Size="x-Small" Font-Bold=true
                            ForeColor="red"></asp:Label>
                    </div>
                    <asp:Panel ID="pnllogos" Visible="false" runat="server">
                        <div id="logos">
                            <h4>
                                For more opportunities to subscribe to other digital products offered<br />
                                by Canon Communications, please click on the logos below.
                            </h4>
                            <img src="http://www.kmpsgroup.com/images/mx_small.gif" class="mx" />
                            <img src="http://www.kmpsgroup.com/images/mtp_small.gif" class="mtp" />
                            <img src="http://www.kmpsgroup.com/images/emd_small.gif" class="emd" />
                            <img src="http://www.kmpsgroup.com/images/cmdm_small.jpg" class="cmdm" />
                            <img src="http://www.kmpsgroup.com/images/mdt_small.gif" class="mdt" />
                        </div>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        <input type="reset" value="Reset" />
                    </asp:Panel>
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
