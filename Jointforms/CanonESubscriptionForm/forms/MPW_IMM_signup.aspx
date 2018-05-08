<%@ Page EnableEventValidation="false" AutoEventWireup="True" Inherits="CanonESubscriptionForm.forms.Handler" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Canon Communications - MPW and IMM Editorial Signup</title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script type="text/javascript" src="../js/jquery-1.2.1.js"></script>

    <script type="text/javascript" src="../js/jGet.js"></script>

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <script type="text/javascript">

function validateEmail()
{
    var allOk = false;
    allOk = svValidator("txtemail", document.forms[0].txtemail.value);
    
    // Validate zip against state
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
            
    if (!allOk)
        document.forms[0].txtemail.focus();
    
	return allOk;
}

function validateForm() 
{
    var allOk = false;
	allOk = 
		(svValidator("Email", document.forms[0].txtemail.value) && svValidator("First Name", document.forms[0].txtfirstname.value) && svValidator("Last Name", document.forms[0].txtlastname.value) && 
		svValidator("Job Title", document.forms[0].txttitle.value) && svValidator("Company", document.forms[0].txtcompany.value) && svValidator("Address", document.forms[0].txtaddress.value) && 
		svValidator("City", document.forms[0].txtcity.value) && svValidator("State", document.forms[0].drpstate.value) && 
		svValidator("Postcode", document.forms[0].txtzip.value));

	// Validate zip against state
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
       document.forms[1].t.value = document.forms[0].txttitle.value;
       document.forms[1].compname.value = document.forms[0].txtcompany.value;
       document.forms[1].adr.value = document.forms[0].txtaddress.value;
       document.forms[1].adr2.value = document.forms[0].txtaddress2.value;
       document.forms[1].city.value = document.forms[0].txtcity.value;
       document.forms[1].state.value = document.forms[0].drpstate.value;
       document.forms[1].zc.value = document.forms[0].txtzip.value;
       document.forms[1].ctry.value = document.forms[0].drpcountry.value;
       document.forms[1].ph.value = document.forms[0].txtphone.value;
       document.forms[1].fax.value = document.forms[0].txtfax.value;
       
       alert( document.forms[1].ctry.value);
       
       for(i=0; i<document.forms[0].elements.length; i++)
       {
           if (document.forms[0].elements[i].name.substring(0,5) == "user_" && document.forms[0].elements[i].type =="checkbox")
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
<body class="prototype">
    <div id="container">
        <div id="innerContainer">
            <div id="container-content">
                <div id="banner">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td align="left">
                                <img border="0" src="http://www.kmpsgroup.com/images/MPW_sheader.gif" alt="">
                            </td>
                            <td align="center">
                                <h2>
                                    Digital Product Subscription Form</h2>
                            </td>
                            <td align="right">
                                <img border="0" src="http://www.kmpsgroup.com/images/IMM_sheader.gif" alt="">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <form name="frmsub1" id="frmsub1" runat="server">
                    <br />
                    <div id="current">
                        <asp:Panel ID="pnlCurrent" runat="server" Visible="false">
                            <div class="MPW" style="height: 20em;">
                                <label>
                                    <font size="4" color="black"><u>Modern Plastics Worldwide</u></font> - TO SUBSCRIBE,
                                    please choose your options below (check all that apply).
                                </label>
                                <br />
                                <table width="100%" cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_1" id="user_MPW_1" runat="server" />
                                        </td>
                                        <td valign="top"><b><i>E-Weekly</i></b> - A weekly update of global breaking news from
                                            the plastics industry. </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_3" id="user_MPW_3" runat="server" />
                                        </td>
                                        <td valign="top"><b><i>E-Product Showcase</i></b> - Monthly bulletin featuring a wide
                                            range of new products and technology. </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_4" id="user_MPW_4" runat="server" />
                                        </td>
                                        <td valign="top"><b><i>Greening of an Industry</i></b> - Extensive coverage of the latest
                                            "green" trends in the industry. </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top"><b><i>Technology Bulletin</i></b> - In-depth quarterly coverage
                                            of new technology and innovative applications by topic. </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_1" id="user_MPW_TB_1" runat="server" />&nbsp;Blowmolding
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_2" id="user_MPW_TB_2" runat="server" />&nbsp;Thermoforming
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_3" id="user_MPW_TB_3" runat="server" />&nbsp;Decorating/Assembly
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_4" id="user_MPW_TB_4" runat="server" />&nbsp;Extrusion
                                            Pipe & Profile </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_5" id="user_MPW_TB_5" runat="server" />&nbsp;Compounding
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp; </td>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_MPW_TB_6" id="user_MPW_TB_6" runat="server" />&nbsp;Auxiliaries
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="IMM" style="height: 14em;">
                                <label>
                                    <font size="4" color="black"><u>Injection Molding</u></font> - TO SUBSCRIBE, please
                                    choose your options below (check all that apply).</label><br />
                                <table width="100%" cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_IMM_1" id="user_IMM_1" runat="server" /></td>
                                        <td valign="top"><b><i>E-Shots</i></b> - An informative twice-monthly offering of the
                                            latest injection molding news, technology, and troubleshooting tips.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_IMM_2" id="user_IMM_2" runat="server" /></td>
                                        <td valign="top"><b><i>E-Product Showcase</i></b> - Monthly bulletin featuring a wide
                                            range of new products and technology.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_IMM_3" id="user_IMM_3" runat="server" /></td>
                                        <td valign="top"><b><i>Design Solutions</i></b> - Bi-monthly bulletin dedicated to injection
                                            molding design advances and challenges geared towards part designers and OEMs.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <input type="checkbox" value="y" name="user_IMM_4" id="user_IMM_4" runat="server" /></td>
                                        <td valign="top"><b><i>Moldmaking Solutions</i></b> - Bi-monthly bulletin filled with
                                            information on mold design, troubleshooting, and repair for mold professionals.</td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="profile">
                        <p>
                            <label>
                                * Email:</label>
                            <asp:TextBox runat="server" ID="txtemail" Width="200"></asp:TextBox>&nbsp;<asp:Button
                                ID="btnEmailClick" runat="server" Visible="true" Text="submit" OnClick="btnEmailClick_Click"
                                Width="100" />
                        </p>
                        <asp:Panel ID="pnlProfile" Visible="false" runat="server">
                            <p>
                                <label>
                                    * First Name:</label><asp:TextBox runat="server" ID="txtfirstname" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * Last Name:</label><asp:TextBox runat="server" ID="txtlastname" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * Job Title:</label><asp:TextBox runat="server" ID="txttitle" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * Company:</label><asp:TextBox runat="server" ID="txtcompany" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * Address:</label><asp:TextBox runat="server" ID="txtaddress" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    Address 2:</label><asp:TextBox runat="server" ID="txtaddress2" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * City:</label><asp:TextBox runat="server" ID="txtcity" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    * State/Province:</label>
                                <asp:DropDownList ID="drpstate" runat="server">
                                </asp:DropDownList>
                            </p>
                            <p class="zip">
                                <label>
                                    * Postcode:</label><asp:TextBox runat="server" ID="txtzip" Width="200"></asp:TextBox>
                            </p>
                            <p class="country">
                                <label>
                                    Country:</label>
                                <asp:DropDownList ID="drpcountry" runat="server">
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
                                    Phone:</label><asp:TextBox runat="server" ID="txtphone" Width="200"></asp:TextBox>
                            </p>
                            <p>
                                <label>
                                    Fax:</label><asp:TextBox runat="server" ID="txtfax" Width="200"></asp:TextBox>
                            </p>
                        </asp:Panel>
                    </div>
                    <br />
                    <div id="questions">
                        <asp:Panel ID="pnlquestions" runat="server" Visible="false">
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="pnllogos" Visible="false" runat="server">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                        <input type="reset" value="Reset" />
                    </asp:Panel>
                </form>
                <form name="frmsub" id="frmsub" action="http://emailactivity.ecn5.com/engines/subscribe.aspx">
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
                    <input type="hidden" name="g" value="12911" />
                    <input type="hidden" name="c" value="1797" />
                    <input type="hidden" name="sfID" value="687" />
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
