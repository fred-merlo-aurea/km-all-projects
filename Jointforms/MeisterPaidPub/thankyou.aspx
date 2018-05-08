<%@ Page Language="C#" EnableEventValidation="false" Theme="Default" AutoEventWireup="true"
    CodeBehind="thankyou.aspx.cs" Inherits="PaidPub.subscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title></title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css">

    <script src="http://www.kmpsgroup.com/subforms/validators.js"></script>

    <!-- START Conversion Tracking for KMPS -->

    <script language="javascript" src="http://www.ecn5.com/ecn.accounts/assets/channelID_16/js/getConversionData.js"></script>

    <!-- END Conversion Tracking for KMPS -->

    <script language="Javascript">

        function ValidateAll() {
            var allOk = false;

            allOk =
            				(arValidator("Please start/continue my subscription to Especially Pet for FREE", document.frmsub.user_SUBSCRIBE) &&
            				   svValidator("First Name", document.frmsub.fn.value) && svValidator("Last Name", document.frmsub.ln.value) &&
            					svValidator("Title", document.frmsub.t.value) &&
            					svValidator("Company Name", document.frmsub.compname.value) &&
            					svValidator("Email", document.frmsub.e.value) &&
            					svValidator("Phone Number", document.frmsub.ph.value) &&
            					svValidator("Company Address", document.frmsub.adr.value) &&
            					svValidator("City", document.frmsub.city.value) &&
            					svValidator("Country", document.frmsub.ctry.value) &&
            					svValidator("Job Function", document.frmsub.user_FUNCTION.value) &&
            					svValidator("Primary Occupation", document.frmsub.user_BUSINESS.value)
            					);

            if (allOk) {
                var emailRegxp = /^([\w]+)(.[\w]+)*@([\w]+)(.[\w]{2,3}){1,2}$/;
                if (emailRegxp.test(document.forms[0].e.value) != true) {
                    allOk = false;
                    alert("Invalid Email Address.");
                }

            }

            if (allOk) {
                if (document.frmsub.user_FUNCTION.value == '499' && document.frmsub.user_FUNCTIONTXT.value == "") {
                    alert('Please Enter the Job Function that best matches your category');
                    allOk = false;
                } else if (document.frmsub.user_BUSINESS.value == '249' && document.frmsub.user_BUSINESSTXT.value == "") {
                    alert('Please complete the field for category that best describes the Your Business');
                    allOk = false;
                }
                else if (document.frmsub.user_PA1FUNCTION.value == '499' && document.frmsub.user_PA1FUNCTIONTXT.value == "") {
                    alert('Please complete the Contact person information');
                    allOk = false;
                }
                else {
                    document.frmsub.submit();
                }

            }

            if (allOk) {
                if ((document.frmsub.user_PA1FNAME.value != "" || document.frmsub.user_PA1LNAME.value != "" || document.frmsub.user_PA1FUNCTION.value != "" || document.frmsub.user_PA1EMAIL.value != "") && (document.frmsub.user_PA1FNAME.value == "" || document.frmsub.user_PA1LNAME.value == "" || document.frmsub.user_PA1FUNCTION.value == "" || document.frmsub.user_PA1EMAIL.value == "")) {
                    allOk = false;
                    alert('Please Enter First Name, Last Name, Job Title and Email');
                }
            }

            return allOk;
        }

        function showHideFields(param) {
            enableTB("user_PA1FNAME", param);
            enableTB("user_PA1LNAME", param);
            enableTB("user_PA1EMAIL", param);
            enableTB("user_PA1FUNCTION", param);
        } 
        
    </script>

    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <style type="text/css">
        .style1
        {
            width: 20%;
        }
        .style2
        {
            width: 110px;
        }
    </style>
</head>
<body>
    <div id="container">
        <div id="innerContainer">
            <div id="container-content">
                <div id="banner">
                    <img border="0" src="images/header.jpg" width="640px" />
                    <p>
                        <h1>
                            Your request has been processed.</h1>
                        <br />
                        <h2>
                            Thank you for your registration.</h2>
                    </p>
                    </p>
                </div>
            </div>
        </div>
        <!-- end container-content -->
        <div id="footer">
        </div>
        <!--end footer-->
    </div>
</body>
</html>
