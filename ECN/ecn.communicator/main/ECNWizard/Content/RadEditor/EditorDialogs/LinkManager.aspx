<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkManager.aspx.cs" Inherits="ecn.communicator.main.ECNWizard.Content.RadEditor.EditorDialogs.LinkManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
    <script type="text/javascript">
        var HTMLTOREPLACE = "";
        var FULLCONTENT;
        var ITFROMLINK;
        if (window.attachEvent) {
            window.attachEvent("onload", initDialog);
        }
        else if (window.addEventListener) {
            window.addEventListener("load", initDialog, false);
        }

        function getRadWindow() {
            if (window.radWindow) {
                return window.radWindow;
            }

            if (window.frameElement && window.frameElement.radWindow) {
                return window.frameElement.radWindow;
            }

            return null;
        }


        function initDialog() {
            var clientParameters = getRadWindow().ClientParameters; //return the arguments supplied from the parent page   

            if (clientParameters[0] != null) {
                HTMLTOREPLACE = clientParameters[0];
                FULLCONTENT = clientParameters[1];
            }
            var divURL = document.getElementById("divURL");
            var divAnchor = document.getElementById("divAnchor");
            var divEmail = document.getElementById("divEmail");

            var ITRegex = />(.*?)<[/]a>/i;
            var linkText = document.getElementById("LinkText");
            try {
                var innerText = ITRegex.exec(HTMLTOREPLACE)[1];
                ITFROMLINK = innerText;
                linkText.value = innerText;
            }
            catch (err) { linkText.value = HTMLTOREPLACE; }

            var URLRegex = /<a.*?href="(.*?)".*?>/i;
            var ddlType = document.getElementById("ddlLinkType");
            var linkType = "";
            try {
                var linkURL = URLRegex.exec(HTMLTOREPLACE)[1];
                if (linkURL.startsWith("http") || linkURL.startsWith("ftp") || linkURL.startsWith("news")) {
                    linkType = "url";
                    divURL.style.display = "block";
                    divAnchor.style.display = "none";
                    divEmail.style.display = "none";
                    
                    setUpURL(linkURL);
                }
                else if (linkURL.startsWith("mailto")) {
                    linkType = "mailto";
                    divURL.style.display = "none";
                    divAnchor.style.display = "none";
                    divEmail.style.display = "block";
                    setUpMailTo(linkURL);
                }
                else if (linkURL.startsWith("#")) {
                    linkType = "anchor";
                    divURL.style.display = "none";
                    divAnchor.style.display = "block";
                    divEmail.style.display = "none";
                    setUpAnchor(linkURL);
                }

            } catch (err) {
                divURL.style.display = "block";
                divAnchor.style.display = "none";
                divEmail.style.display = "none";
                setUpURL("");
            }

            for (var i = 0; i < ddlType.options.length; i++) {
                if (ddlType.options[i].value === linkType) {
                    ddlType.selectedIndex = i;
                    break;
                }
            }


        }

        function setUpURL(url) {

            var linkURLTB = document.getElementById("LinkUrl");
            linkURLTB.value = url;
            var targetRegex = /<a.*?target="(.*?)".*?>/i;
            var targetTB = document.getElementById("LinkTargetCombo");
            try {
                var targetValue = targetRegex.exec(HTMLTOREPLACE)[1];

                targetTB.value = targetValue;
            }
            catch (err) { targetTB.value = "_self"; }
        }

        function setUpMailTo(url) {
            var txtEmailAddress = document.getElementById('txtEmailAddress');
            var txtSubject = document.getElementById('txtSubject');
            var txtBody = document.getElementById('txtBody');

            var mailtoAddressRegex = /mailto:(^[&?].*)/i;
            var mailtoSubjectRegex = /mailto:.*?subject=(^[?&])/i;
            var mailtoBodyRegex = /mailto:.*?body=(^[?&])/i;

            var email = "";
            var subject = "";
            var body = "";
            try
            {  email = url.exec(mailtoAddressRegex)[1];}catch(err){}
            try{subject = url.exec(mailtoSubjectRegex)[1];}catch(err){}
            try{ body = url.exec(mailtoBodyRegex)[1];}catch(err){}

            txtEmailAddress.value = email;
            txtSubject.value = subject;
            txtBody.value = body;


        }

        function setUpAnchor(url) {
            var anchorRegex = /(#.*?)/i;
            var value = url.exec(anchorRegex)[1];
            var ddlAnchorName = document.getElementById('ddlAnchorName');
            var ddlElementID = document.getElementById('ddlElementID');

            var emptyOption = document.createElement("option");
            var emptyOption2 = document.createElement("option");
            emptyOption.text = "";
            emptyOption2.text = "";
            ddlAnchorName.add(emptyOption2);
            ddlElementID.add(emptyOption);

            var html = $.parseHTML(FULLCONTENT);

            $.each(html, function (i, el) {
                if ($(this).attr('name') !== undefined && $(this).attr('name') != "") {
                    var newAnchor = document.createElement("OPTION");
                    newAnchor.text = $(this).attr('name');
                    ddlAnchorName.add(newAnchor);
                }
            });

            $.each(html, function (i, el) {
                //add elements here
                if ($(this).attr('id') !== undefined && $(this).attr('id') != '') {
                    var newElement = document.createElement("OPTION");
                    newElement.text = $(this).attr('id');
                    ddlElementID.add(newElement);
                }
            });

            //look through anchors first
            for (var i = 0; i < ddlType.options.length; i++) {
                if (ddlType.options[i].value === linkType) {
                    ddlType.selectedIndex = i;
                    break;
                }
            }

        }

        function ok() {
            var type = "";
            var ddlType = document.getElementById("ddlLinkType");
            type = ddlType.options[ddlType.selectedIndex].value;
            var ParamsToSend = new Array();
            var aFinal = $('<a></a>');
            var aReplace = $(HTMLTOREPLACE);// document.createElement('ANCHOR');

            if (HTMLTOREPLACE.length > 0 && HTMLTOREPLACE != "<a></a>")
                ParamsToSend[2] = true;
            else
                ParamsToSend[2] = false;

            if (type == "url") {
                aFinal.attr('href', document.getElementById("LinkUrl").value);
                var targetDropDown = document.getElementById("LinkTargetCombo");
                aFinal.attr('target',targetDropDown.options[targetDropDown.selectedIndex].value);
                
                aFinal.text(document.getElementById('LinkText').value);

                if (validateURL(aFinal.attr('href')).length > 0) {
                    var finalLink = { href: "", target: "", name: "" };

                    ParamsToSend[0] = HTMLTOREPLACE;
                    aFinal[0].innerHTML = aReplace[0].outerHTML;
                    ParamsToSend[1] = aFinal[0].outerHTML;

                    getRadWindow().close(ParamsToSend);

                }
                else {
                    
                    return;
                }
            }
            else if (type == "anchor") {
                var ddlAnchorName = document.getElementById("ddlAnchorName");
                var ddlElementID = document.getElementById("ddlElementID");
                var newHref = "";
                if (ddlAnchorName.selectedIndex > 0) {
                    newHref = "#" + ddlAnchorName.options[ddlAnchorName.selectedIndex].text;
                }
                else if (ddlElementID.selectedIndex > 0) {
                    newHref = "#" + ddlElementID.options[ddlElementID.selectedIndex].text;
                }
                aFinal.attr('href', newHref);
                aFinal.text(document.getElementById('LinkText').value);

                ParamsToSend[0] = HTMLTOREPLACE;
                ParamsToSend[1] = aFinal[0].outerHTML;

                getRadWindow().close(ParamsToSend);
            }
            else if (type == "mailto") {
                var finalLink = "";
                var address = document.getElementById('txtEmailAddress').value;
                var subject = document.getElementById('txtSubject').value;
                var body = document.getElementById('txtBody').value;
                finalLink = "mailto:" + address;
                if(subject.length > 0)
                    finalLink = finalLink + "?subject=" + subject;
                if (body.length > 0) {
                    if (finalLink.indexOf('?') < 0)
                        finalLink = finalLink + "?";
                    else
                        finalLink = finalLink + "&";
                    finalLink = finalLink + "body=" + body;
                }
                aFinal.attr('href', finalLink);
                aFinal.text(document.getElementById('LinkText').value);


                ParamsToSend[0] = HTMLTOREPLACE;
                ParamsToSend[1] = aFinal[0].outerHTML;

                getRadWindow().close(ParamsToSend);
            }


        }

        function cancel() {
            getRadWindow().close();
        }

        function validateURL() {
            var url = document.getElementById("LinkUrl").value;

            var urlRegex = new RegExp("(https?|ftps?|news):\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}");
            if (!url.match(urlRegex)) {
                var lblURL = document.getElementById("lblInvalidLink");
                lblURL.style.display = 'block';
                return "";
            }
            else {
                var lblURL = document.getElementById("lblInvalidLink");
                lblURL.style.display = 'none';
                return url;
            }
        }

        function ddlLinkType_Changed() {
            var selected = "";
            var ddlLinkType = document.getElementById("ddlLinkType");
            selected = ddlLinkType.options[ddlLinkType.selectedIndex].value;
            var divURL = document.getElementById("divURL");
            var divAnchor = document.getElementById("divAnchor");
            var divEmail = document.getElementById("divEmail");

            if (selected == "url") {
                divURL.style.display = "block";
                divAnchor.style.display = "none";
                divEmail.style.display = "none";
            }
            else if (selected == "anchor") {
                divURL.style.display = "none";
                divAnchor.style.display = "block";
                divEmail.style.display = "none";
                var ddlAnchorname = document.getElementById("ddlAnchorName");
                var ddlElementID = document.getElementById("ddlElementID");
                if (ddlAnchorname.options.length == 0 && ddlElementID.options.length == 0) {
                    var emptyOption = document.createElement("option");
                    var emptyOption2 = document.createElement("option");
                    emptyOption.text = "";
                    emptyOption2.text = "";
                    ddlAnchorname.add(emptyOption2);
                    ddlElementID.add(emptyOption);

                    var html = $.parseHTML(FULLCONTENT);
                    
                    

                    $.each(html, function (i, el){
                        if ($(this).attr('name') !== undefined && $(this).attr('name') != "") {
                            var newAnchor = document.createElement("OPTION");
                            newAnchor.text = $(this).attr('name');
                            ddlAnchorname.add(newAnchor);
                        }
                    });



                    $.each(html,function (i,el) {
                        //add elements here
                        if ($(this).attr('id') !== undefined && $(this).attr('id') != '') {
                            var newElement = document.createElement("OPTION");
                            newElement.text = $(this).attr('id');
                            ddlElementID.add(newElement);
                        }
                    }

                        );
                }
            }
            else if (selected == "mailto") {
                divURL.style.display = "none";
                divAnchor.style.display = "none";
                divEmail.style.display = "block";
            }
        }

        function ddlAnchorName_Change() {
            var ddlElement = document.getElementById("ddlElementID");
            ddlElement.options[0].selected = true;
        }

        function ddlElementID_Change() {
            var ddlAnchor = document.getElementById("ddlAnchorName");
            ddlAnchor.options[0].selected = true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="InsertLink">
            <table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
                <caption style="display: none;">It contains the Insert Link light dialog, which has the important properties to put a hyperlink in your document: URL, Link Text and Target. In the light dialog you also have a button (All Properties) that allows you to switch from Insert Link dialog to Hyperlink Manager dialog if you need to access all hyperlink options.</caption>
                <thead style="display: none;">
                    <tr>
                        <th scope="col">
                            <span>Labels - URL, Link Text and Target</span>
                        </th>
                        <th scope="col">
                            <span>URL, Link Text and Target</span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <label for="LinkType" class="reDialogLabelLight">
                                <span>Link Type</span>
                            </label>
                        </td>
                        <td>
                            <select id="ddlLinkType" onchange="ddlLinkType_Changed()">
                                <option selected="selected" value="url">URL
                                </option>
                                <option value="anchor">Link to anchor in the text
                                </option>
                                <option value="mailto">E-mail
                                </option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="divURL">
                                <table>
                                    <tr>
                                        <th scope="row" style="display: none;">URL
                                        </th>
                                        <td>
                                            <label for="LinkURL" class="reDialogLabelLight">
                                                <span>URL</span>
                                            </label>
                                        </td>
                                        <td class="reControlCellLight">
                                            <input type="text" id="LinkUrl" class="rfdIgnore" />
                                            <span id="lblInvalidLink" style="color: red; display: none;">Invalid URL</span>
                                        </td>
                                    </tr>
                                    <tr id="texTextBoxParentNode">
                                        <th scope="row" style="display: none;">Link Text
                                        </th>
                                        <td>
                                            <label for="LinkText" class="reDialogLabelLight">
                                                <span>Link Text</span>
                                            </label>
                                        </td>
                                        <td class="reControlCellLight">
                                            <input type="text" id="LinkText" class="rfdIgnore" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th scope="row" style="display: none;">Target
                                        </th>
                                        <td>
                                            <label for="LinkTargetCombo" class="reDialogLabelLight">
                                                <span>Link Target</span>
                                            </label>
                                        </td>
                                        <td class="reControlCellLight">
                                            <select id="LinkTargetCombo" class="rfdIgnore">
                                                <optgroup label="PresetTargets">
                                                    <option selected="selected" value="_self">Same Window</option>
                                                    <option value="_blank">New Window</option>
                                                </optgroup>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divAnchor">
                                <table>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Select an Anchor</legend>
                                                <table>
                                                    <tr>
                                                        <td>By Anchor Name
                                                        </td>
                                                        <td>By Element ID
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <select id="ddlAnchorName" onchange="ddlAnchorName_Change()"></select>
                                                        </td>
                                                        <td>
                                                            <select id="ddlElementID" onchange="ddlElementID_Change()"></select>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEmail">
                                <table>
                                    <tr>

                                        <td>
                                            <label for="LinkURL" class="reDialogLabelLight">
                                                <span>E-Mail Address</span>
                                            </label>
                                        </td>
                                        <td>
                                            <input id="txtEmailAddress" type="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Message Subject</span>
                                        </td>
                                        <td>
                                            <input id="txtSubject" type="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Message Body</span>
                                        </td>
                                        <td>
                                            <input id="txtBody" type="text" />
                                        </td>
                                    </tr>


                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="0" style="width: 100%;" cellspacing="0" class="reConfirmCancelButtonsTblLight">
                                <tbody>
                                    <tr>

                                        <td style="width: 50%; text-align: center;">
                                            <input type="button" onclick="ok()" value="OK" id="lmlInsertBtn" />

                                        </td>
                                        <td style="width: 50%; text-align: center;">
                                            <input type="button" onclick="cancel()" value="Cancel" id="lmlCancelBtn" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
