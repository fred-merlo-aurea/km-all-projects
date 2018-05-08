<%@ Page Language="c#" CodeBehind="transnippets.aspx.cs" AutoEventWireup="True" EnableEventValidation="false" Inherits="ecn.communicator.contentmanager.ckeditor.dialog.transnippets" %>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/ckeditor/controls/groupexplorer.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<html>
<head>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <base target="_self" />
    
    <script src="../../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    
    <style type="text/css">
        BODY {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
            background: #F7F7F7;
        }

        TD {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        INPUT {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        SELECT {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        TEXTAREA {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        BUTTON {
            FONT-SIZE: 11px;
            FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        tr.warnPad td {
            padding-top: 5px;
        }

        .border {
            border-bottom: 1px #ccc solid;
        }

        #udfRow td {
            padding-top: 20px;
        }

            #udfRow td td {
                padding-top: 0px;
            }

        .labelCell {
            width: 120px;
        }
    </style>
    <script type="text/Javascript">
        var HTMLTOREPLACE = "";
        var isPageLoad;
        //if (window.attachEvent) {
        //    window.attachEvent("onload", initDialog);
        //}
        //else if (window.addEventListener) {
        //    window.addEventListener("load", initDialog, false);
        //}

        //function getRadWindow() {
        //    if (window.radWindow) {
        //        return window.radWindow;
        //    }

        //    if (window.frameElement && window.frameElement.radWindow) {
        //        return window.frameElement.radWindow;
        //    }

        //    return null;
        //}

        function qs(key) {
            key = key.replace(/[*+?^$.\[\]{}()|\\\/]/g, "\\$&"); // escape RegEx meta chars
            var match = location.search.match(new RegExp("[?&]" + key + "=([^&]+)(&|$)"));
            return match && decodeURIComponent(match[1].replace(/\+/g, " "));
        }

        <%--       $(document).ready(function () {
            CKEDITOR.on('instanceReady', function (evt) {
                if (typeof isPageLoad === 'undefined') {

                    var clientParams = new Array();
                    var paramsToPass = "";
                    var htmlToReplace = "";
                    var origName = qs('editorName');
                    var openerCK = window.opener.CKEDITOR.instances[origName];

                    var selected = openerCK.getSelection().getRanges([0]);
                    if (selected != null) {

                        var RegexTable = new RegExp("[^\s\S]*?(<table[^<>]+?id=[\"']transnippet_[^<>]+?[\"'][^>]*?>.+?</table>)");
                        var matches = RegexTable.exec(selected[0].startContainer.getText());
                        if (matches) {
                            paramsToPass = matches[1];
                            htmlToReplace = matches[1];
                        }
                        else {
                            var trElement = selected[0].startContainer;
                            var textCheck = trElement.getText();
                            if (textCheck.length > 0) {
                                var RegexTR = new RegExp("[^\\s\\S]*?(<table[^<>]+?id=[\"']transnippet_[^<>]+?[\"'][^>]*?>[\\s\\S]*?" + trElement.getText() + "[\\s\\S]*?</table>)");
                                var matches = RegexTR.exec(openerCK.getData(false, null));
                                if (matches) {
                                    paramsToPass = matches[1];
                                    htmlToReplace = matches[1];
                                }
                            }
                        }

                    }

                    clientParams[0] = paramsToPass;
                    clientParams[1] = htmlToReplace;
                    HTMLTOREPLACE = htmlToReplace;


                    if (clientParams && clientParams[0] != null && clientParams[0].length > 0) {
                        var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];
                        if (radEditor) {
                            radEditor.setData(clientParams[0]);

                            if(getobj("ddlTransaction").length == 0)
                                __doPostBack('btnLoadOptions', 'OnClick');
                        }
                        //var RegexGroup = new RegExp("<table.*?group_id=[\"'](.*?)[\"'].*?transaction=[\"'](.*?)[\"'].*?>");
                        //var matches = RegexGroup.exec(clientParameters[0]);
                        //if(matches != null && matches[1] != null)
                        //{
                        //    $('#Groups >option[value="' + matches[1] + '"]').attr("selected", "selected");
                        //    getobj("Groups").onchange();
                        //}
                        //if(matches != null && matches[2] != null)
                        //{
                        //    $('#ddlTransaction > option[value="' + matches[2] + '"]').attr("selected", "selected");
                        //    getobj("ddlTransaction").onchange();

                        //    var RegexUDF = new RegExp("<tr.*?id=[\"']detail[\"'].*?>(.*?)</tr>");

                        //}
                    }
                    if (!clientParams && clientParams[1] != null)
                        HTMLTOREPLACE = clientParameters[1];
                }
                else {
                    var cEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];
                    if (cEditor)
                        HTMLTOREPLACE = cEditor.getData();
                }
                isPageLoad = false;
            });
        });--%>
        <%--function initDialog() {
            if (getobj("ddlTransaction").length == 0) {
                var clientParams = new Array();
                clientParams = getRadWindow().ClientParameters;
                var clientParameters = getRadWindow().ClientParameters; //return the arguments supplied from the parent page   
                if (clientParams && clientParams[0] != null) {
                    var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];;
                    radEditor.setData(clientParameters[0]);

                    __doPostBack('btnLoadOptions', 'OnClick');

                    //var RegexGroup = new RegExp("<table.*?group_id=[\"'](.*?)[\"'].*?transaction=[\"'](.*?)[\"'].*?>");
                    //var matches = RegexGroup.exec(clientParameters[0]);
                    //if(matches != null && matches[1] != null)
                    //{
                    //    $('#Groups >option[value="' + matches[1] + '"]').attr("selected", "selected");
                    //    getobj("Groups").onchange();
                    //}
                    //if(matches != null && matches[2] != null)
                    //{
                    //    $('#ddlTransaction > option[value="' + matches[2] + '"]').attr("selected", "selected");
                    //    getobj("ddlTransaction").onchange();

                    //    var RegexUDF = new RegExp("<tr.*?id=[\"']detail[\"'].*?>(.*?)</tr>");

                    //}
                }
                if (!clientParameters && clientParameters[1] != null)
                    HTMLTOREPLACE = clientParameters[1];
            }
            else {
                HTMLTOREPLACE = CKEDITOR.instances['<%= tranEditor.ClientID %>'].getData();
            }
        }--%>

        function ok() {
            var transnippetName = getobj("transnippetName").value;
            var GroupID = getobj("hfSelectGroupID").value;
            var Transaction = getobj("ddlTransaction").value;
            var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];
            var filterDD = getobj("filterFields");
            var filterValue = getobj("filterValue");
            if (filterDD.selectedIndex > 0)
            {
                if (filterValue.value.trim().length == 0) {
                    alert("Please enter a value for filtering");
                    return;
                }
            }

            if (transnippetName.length == 0) {
                alert("Please enter a name for the Transnippet. Only Numbers [0-9], Letters [A-Z/a-z] and Special Characters [. (dot), _ (underscore), - (dash) and space] are allowed.");
                getobj("transnippetName").focus();
                return;
            } else if (GroupID == "0") {
                alert("Please select a Group to choose UDFs.");

                return;
            } else {
                //##TRANSNIPPET|GRID|$PICKUPDT,$LOCATION,$EMAILOPTIN|HDR-STYLE=font-family:Arial;font-size:10px;background-color:#FF0000;color:#000000;font-weight:bold|TBL-STYLE=font-family:Arial;font-size:10px;background-color:#ffac99;color:#000000;width:100%;border:1px solid;##
                var selectedUDFs = ""; var selectedUDFHeaders = "";
                for (var i = 0; i < getobj("UDFSelectedList").options.length; i++) {
                    selectedUDFs += "<td nowrap=\"nowrap\"><strong>" + getobj("UDFSelectedList").options[i].value + "</strong></td>";
                    selectedUDFHeaders += getobj("UDFSelectedList").options[i].value + ",";
                }
                if (selectedUDFs.length == 0) {
                    alert("Please select at least one UDF.");
                    getobj("GroupID").focus();
                    return;
                } else {


                    var HTMLToSEND = radEditor.getData();
                    alert("Please DO NOT make any modifications to the Transnippet script after it's inserted in the Editor.\nIf you want to make changes , delete the transnippet and start over.");
                    //var RegexGroup = new RegExp("<table[\\s\\S]*id=[\"']transnippet_.*?[\"']([\s\S]*>)(group_id=[\"'].*?[\"'])?[\\s\\S]*(transaction=[\"'].*?[\"'])?.*?>[\\s\\S]*<tr[\\s\\S]*[^</table>][\\s\\S]*</tr>[\\s\\S]*</table>");
                    var tranTable = $("<div>" + HTMLToSEND + "</div>");
                    tranTable.find("table").attr("id", "transnippet_" + transnippetName);
                    tranTable.find('#transnippet_' + transnippetName).attr("group_id", GroupID);
                    tranTable.find('#transnippet_' + transnippetName).attr("transaction", Transaction);



                    HTMLToSEND = tranTable.html();
                    var ParamsToSend = new Array();
                    ParamsToSend[0] = HTMLToSEND;
                    ParamsToSend[1] = HTMLTOREPLACE;
                    //getRadWindow().close(ParamsToSend);
                    window.opener.setTranValue(ParamsToSend);
                    window.close();
                }
            }
        }

        function checkconfirm() {
            var selected = getobj("UDFSelectedList");
            if (selected.length > 0) {
                return confirm('Changing the Transaction will clear out any data that you have already entered.  Proceed?');
            }
            else {
                __doPostBack("<%= ddlTransaction.ClientID  %>", '');
                return true;
            }
            return true;

        }

        function getobj(id) {
            if (document.all && !document.getElementById)
                obj = eval('document.all.' + id);
            else if (document.layers)
                obj = eval('document.' + id);
            else if (document.getElementById)
                obj = document.getElementById(id);

            return obj;
        }

        function cancel() {
            window.close();
        }
    </script>
    <script language="JavaScript" type="text/JavaScript">
        function move(fbox, tbox) {
            var arrFbox = new Array();
            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            for (i = 0; i < tbox.options.length; i++) {
                arrLookup[tbox.options[i].text] = tbox.options[i].value;
                arrTbox[i] = tbox.options[i].text;
            }
            var fLength = 0;
            var tLength = arrTbox.length;
            for (i = 0; i < fbox.options.length; i++) {
                arrLookup[fbox.options[i].text] = fbox.options[i].value;
                if (fbox.options[i].selected && fbox.options[i].value != "") {
                    arrTbox[tLength] = fbox.options[i].text;
                    tLength++;
                } else {
                    arrFbox[fLength] = fbox.options[i].text;
                    fLength++;
                }
            }
            fbox.length = 0;
            tbox.length = 0;
            var c;
            for (c = 0; c < arrFbox.length; c++) {
                var no = new Option();
                no.value = arrLookup[arrFbox[c]];
                no.text = arrFbox[c];
                fbox[c] = no;
            }
            for (c = 0; c < arrTbox.length; c++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[c]];
                no.text = arrTbox[c];
                tbox[c] = no;
            }

            var sortDD = getobj("sortField");
            var addList = getobj("UDFSelectedList");
            var filterDD = getobj("filterFields");


            if (filterDD.length > 0 && $("#UDFSelectedList option[value='" + filterDD.options[filterDD.selectedIndex].value + "']").length > 0) {
                var selectedFilter = filterDD.options[filterDD.selectedIndex].value;
                filterDD.length = 0;

                filterDD.add(new Option("--SELECT--", ""));
                var z;
                for (z = 1; z < addList.length + 1; z++) {
                    var isSelected = selectedFilter == addList[z - 1].value;
                    filterDD.add(new Option(addList[z - 1].text, addList[z - 1].value, false, isSelected));

                }



            }
            else {
                filterDD.length = 0;
                var filterText = getobj("filterValue");
                filterText.value = "";
                filterDD.add(new Option("--SELECT--", ""));
                var z;
                for (z = 1; z < addList.length + 1; z++) {

                    filterDD.add(new Option(addList[z - 1].text, addList[z - 1].value));
                }
            }

            if (sortDD.length > 0 && $("#UDFSelectedList option[value='" + sortDD.options[sortDD.selectedIndex].value + "']").length > 0) {
                var selectedSort = sortDD.options[sortDD.selectedIndex].value;
                sortDD.length = 0;
                sortDD.add(new Option("--SELECT--", ""));
                var z;
                for (z = 1; z < addList.length + 1; z++) {
                    var isSelectedSort = selectedSort == addList[z - 1].value;
                    sortDD.add(new Option(addList[z - 1].text, addList[z - 1].value, false, isSelectedSort));

                }
            }
            else {
                sortDD.length = 0;
                sortDD.add(new Option("--SELECT--", ""));
                var z;
                for (z = 1; z < addList.length + 1; z++) {

                    sortDD.add(new Option(addList[z - 1].text, addList[z - 1].value));

                }
            }

            if (sortDD.length > 1)
            {
                sortDD.disabled = false;
            }
            else
            {
                sortDD.disabled = true;
            }
            var txtFilterValue  = getobj("filterValue");
            if (filterDD.length > 1)
            {
                filterDD.disabled = false;
                txtFilterValue.disabled = false;
            }
            else
            {
                filterDD.disabled = true;
                txtFilterValue.value = '';
                txtFilterValue.disabled = true;
            }


            var FinalTR = ""
            //code to insert changed filter value
            var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];;
            //get the html
            var originalHTML = radEditor.getData();
            var TXTTranName = getobj("transnippetName");

            var RegexString = new RegExp("<table.*?id=[\"']transnippet_" + TXTTranName.value + "[\"'](.*?filter_field=[\"'].*?[\"'].*?filter_value=[\"'].*?[\"'].*?)>.*?<tr id='header'(.*?)>.*?</tr>.*?<tr id='detail'(.*?)>.*?</tr>.*?</table>");
            var matches = RegexString.exec(originalHTML);

            var selectedUDFs = ""; var selectedUDFHeaders = "";
            for (var i = 0; i < getobj("UDFSelectedList").options.length; i++) {
                selectedUDFs += "<td nowrap=\"nowrap\"><strong>" + getobj("UDFSelectedList").options[i].text + "</strong></td>";
                selectedUDFHeaders += getobj("UDFSelectedList").options[i].text + ",";
            }

            var tableAttr = "";
            var trAttr = "";
            var tdAttr = "";
            if (matches != null && matches[1] != null)
                tableAttr = matches[1];
            if (matches != null && matches[2] != null)
                trAttr = matches[2];
            if (matches != null && matches[3] != null)
                tdAttr = matches[3];


            FinalTR = "<table id='transnippet_" + TXTTranName.value + "' " + tableAttr + " >";
            FinalTR += "<tr id='header' " + trAttr + ">";

            FinalTR += selectedUDFs + "</tr>";

            FinalTR += "<tr id='detail' " + tdAttr + ">";
            FinalTR += selectedUDFs.replace(/<strong>/g, "##").replace(/<\/strong>/g, "##") + "</tr>";
            FinalTR += "</table>";

            if (matches != null)
                originalHTML = originalHTML.replace(matches, FinalTR);
            else
                originalHTML = FinalTR;

            radEditor.setData(originalHTML);

        }

        function filterfieldchanged() {
            var FinalTR = "";
            var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];
            var DDFilterField = getobj("filterFields");

            var originalHTML = radEditor.getData();
            var RegexFilter = new RegExp("<table.*?(filter_field=[\"'].*?[\"']).*?>");
            var matches = RegexFilter.exec(originalHTML);
            if (matches != null && matches[1] != null) {
                if (DDFilterField.selectedIndex > 0) {
                    FinalTR = " filter_field='" + DDFilterField.options[DDFilterField.selectedIndex].text + "' ";
                    originalHTML = originalHTML.replace(matches[1], FinalTR);
                    var txtFilterValue = getobj("filterValue");
                    txtFilterValue.disabled = false;
                }
                else {
                    FinalTR = "";
                    originalHTML = originalHTML.replace(matches[1], FinalTR);
                    var RegexValue = new RegExp("<table.*?(filter_value=[\"'].*?[\"']).*?>");
                    var valueMatch = RegexValue.exec(originalHTML);
                    if (valueMatch != null && valueMatch[1] != null) {
                        originalHTML = originalHTML.replace(valueMatch[1], "");
                    }
                    var txtFilterValue = getobj("filterValue");
                    txtFilterValue.value = "";
                    txtFilterValue.disabled = true;
                }
            }
            else {
                var RegExFull = new RegExp("<table(.*?)>");
                var matchesFull = RegExFull.exec(originalHTML);
                if (matchesFull != null && matchesFull[1] != null) {
                    if (DDFilterField.selectedIndex > 0) {
                        FinalTR = matchesFull[1] + " filter_field='" + DDFilterField.options[DDFilterField.selectedIndex].text + "' ";
                        originalHTML = originalHTML.replace(matchesFull[1], FinalTR);
                        var txtFilterValue = getobj("filterValue");
                        txtFilterValue.disabled = false;
                    }

                }
            }
            radEditor.setData(originalHTML);

        }

        function filtervaluechanged() {
            var FinalTR = "";
            //code to insert changed filter value
            var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];
            var TXTFilterValue = getobj("filterValue");
            //get the html
            var originalHTML = radEditor.getData();
            var RegexString = new RegExp("<table.*?(filter_value=[\"'].*?[\"']).*?>");
            var matches = RegexString.exec(originalHTML);
            if (matches != null && matches[1] != null) {
                FinalTR = " filter_value='" + TXTFilterValue.value + "' ";
                originalHTML = originalHTML.replace(matches[1], FinalTR);
            }
            else {
                var RegExFull = new RegExp("<table(.*?)>");
                var matchesFull = RegExFull.exec(originalHTML);
                if (matchesFull != null && matchesFull[1] != null) {
                    FinalTR = matchesFull[1] + " filter_value='" + TXTFilterValue.value + "' ";
                    originalHTML = originalHTML.replace(matchesFull[1], FinalTR);
                }
            }


            radEditor.setData(originalHTML);

        }

        function sortFieldChanged() {
            var FinalTR = ""
            //code to insert changed filter value
            var radEditor = CKEDITOR.instances['<%= tranEditor.ClientID %>'];

            //get the html
            var originalHTML = radEditor.getData();
            var RegexFilter = new RegExp("<table.*?(sort=[\"'].*?[\"']).*?>");
            var matches = RegexFilter.exec(originalHTML);

            var DDFilterField = getobj("sortField");

            if (DDFilterField.selectedIndex > 0) {
                if (matches != null && matches[1] != null) {
                    FinalTR = " sort='" + DDFilterField.options[DDFilterField.selectedIndex].text + "' ";
                    originalHTML = originalHTML.replace(matches[1], FinalTR);
                }
                else {
                    var RegExFull = new RegExp("<table(.*?)>");
                    var matchesFull = RegExFull.exec(originalHTML);
                    if (matchesFull != null && matchesFull[1] != null) {
                        FinalTR = matchesFull[1] + " sort='" + DDFilterField.options[DDFilterField.selectedIndex].text + "' ";
                        originalHTML = originalHTML.replace(matchesFull[1], FinalTR);
                    }
                }
            }
            else {
                FinalTR = "";
                if (matches != null && matches[1] != null) {
                    originalHTML = originalHTML.replace(matches[1], FinalTR);
                }
            }


            radEditor.setData(originalHTML);

        }

        function uploadContentSource() {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;

            var media = window.open('/ecn.editor/ckeditor/plugins/sourceHtmlUpload/sourcehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);


        }

        function setUploadHTMLContentSource(args) {
            var textControl = document.getElementById('<%= tranEditor.ClientID %>');
            if (textControl)
                textControl.value = args;
        }

        function uploadContentMobile() {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;

            var media = window.open('/ecn.editor/ckeditor/plugins/mobileHtmlUpload/mobilehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);

        }

        function setUploadContentMobile(args) {
            var textMobile = document.getElementById('<%= tranEditor.ClientID %>');
            if (textMobile)
                textMobile.value = args
        }

        function setValue(args) {
                CKEDITOR.instances['<%= tranEditor.ClientID %>'].insertHtml(args);
            

        }

        

        function setData(args) {
           
                CKEDITOR.instances['<%= tranEditor.ClientID %>'].setData(args);
           
        }
    </script>
    <script type="text/javascript" src="/ecn.collector/scripts/Templatestyle.js"></script>

    <title>Insert Transnippet
    </title>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <div id="divpalette" style="border: 1px solid; padding-right: 5px; display: none; padding-left: 5px; z-index: 101; background: #000000; left: 0px; padding-bottom: 5px; width: 360px; padding-top: 5px; position: absolute; height: 206px;">
        <table cellspacing="0" cellpadding="0" border="0" style="color: #FFFFFF">
            <tr>
                <td id="ColorTableCell" valign="top" nowrap align="left"></td>
                <td>&nbsp;</td>
                <td valign="top" nowrap align="center">
                    <input style="margin-bottom: 6px; width: 75px; height: 22px" onclick="apply();" type="button"
                        value="OK" name="btnOK"><br>
                    <input style="margin-bottom: 6px; width: 75px; height: 22px" onclick="javascript: btncancel_onclick();"
                        type="button" value="Cancel" name="btnCancel"><br>
                    <span><b>Highlight</b></span>:
						<div id="hicolor" style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid; width: 74px; border-bottom: 1px solid; height: 20px"></div>
                    <div id="hicolortext" style="margin-bottom: 7px; width: 75px; text-align: right"></div>
                    <span><b>Selected</b></span>:
						<div id="selhicolor" style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid; width: 74px; border-bottom: 1px solid; height: 20px"></div>
                    <input id="selcolor" style="margin-top: 0px; margin-bottom: 7px; width: 75px; height: 20px"
                        type="text" maxlength="20" onchange="selcolor_onpropertychange()">
                </td>
            </tr>
        </table>
    </div>
    <form id="transnippetForm" runat="Server">
        <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="3600" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="pnlMain" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="imgSelectGroup" />
                <asp:PostBackTrigger ControlID="groupExplorer" />
            </Triggers>
            <ContentTemplate>

                <table id="Table2" cellspacing="0" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td valign="top" colspan="3" style="">
                            <div style="border:3px solid #8FC4F0;background-color:white;border-radius:10px;float:left;vertical-align:middle; width:90%;">
                                <img src="http://images.ecn5.com/images/infoEx.jpg" style="float:left;border-radius:10px;">
                                <br /><br />
                                Selecting User Defined Fields [UDFs] from different Groups for the same Campaign will cause Campaign not to work correctly. Please choose UDFs from the Group you are trying to send this Campaign.
                                <asp:Button ID="btnLoadOptions" runat="server" Visible="false" OnClick="btnLoadOptions_Click" />
                            </div>
                            
                            
                        </td>
                    </tr>
                    <tr class="warnPad">
                        <td align="right" valign="top" width="120">
                            <div class="labelCell"><b>Table Title:</b></div>
                        </td>
                        <td valign="top" style="width: 100%; padding-bottom: 10px;" colspan="2">
                            <input type="text" runat="server" name="transnippetName" id="transnippetName" style="width: 250px">
                            (alphanumeric characters only)<br>
                            This name will appear as the table title.
                            <div style="float:right;">
                                <input onclick="ok();" type="button" value="OK"><!-- fckLang="DlgBtnOK"-->&nbsp; 
							&nbsp;<input onclick="cancel();" type="button" value="Cancel"><!-- fckLang="DlgBtnCancel"-->
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="right" valign="middle" width="120"><b>Select&nbsp;Group:</b></td>
                        <td valign="middle" colspan="2">
                            
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>
                                        <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                        <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                           </td>
                    </tr>
                    <tr>
                        <td align="right" valign="middle" width="120"><b>Select Transaction:</b>

                        </td>
                        <td valign="middle" colspan="2">
                            <asp:DropDownList ID="ddlTransaction" runat="server" class="formfield" onchange="return checkconfirm();" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlTransaction_SelectedIndexChanged">
                                
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="udfRow">
                        <td align="right" valign="top" width="120"><b>Available UDF's: </b></td>
                        <td valign="top" colspan="2">
                            <table>
                                <tr>
                                    <td>Select UDF(s):<br />
                                        <asp:ListBox ID="UDFList" runat="server" Width="200" Rows="7" SelectionMode="Multiple"></asp:ListBox></td>
                                    <td>
                                        <input type="button" onclick="move(UDFList, UDFSelectedList)" value=">>"><br>
                                        <br>
                                        <input type="button" onclick="move(UDFSelectedList, UDFList)" value="<<">
                                    </td>
                                    <td>Add to Transnippet:<br />
                                        <asp:ListBox runat="server" Rows="7" SelectionMode="Multiple" ID="UDFSelectedList" Width="200"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <b>Sort By:</b>
                        </td>
                        <td style="vertical-align: top;">
                            <asp:DropDownList ID="sortField" runat="server" Width="150px" Enabled="false" onchange="sortFieldChanged()">
                                <asp:ListItem Text="--SELECT--" Value="-1" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;"><b>Filter By:</b></td>
                        <td style="vertical-align: top;">
                            <asp:DropDownList ID="filterFields" runat="server" Width="150px" Enabled="false" onchange="filterfieldchanged()">
                                <asp:ListItem Text="--SELECT--" Value="-1" />
                            </asp:DropDownList>
                        =
                            <asp:TextBox ID="filterValue" runat="server" Enabled="false" onblur="filtervaluechanged()" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <b>Transnippet HTML</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <CKEditor:CKEditorControl ID="tranEditor" Toolbar="Basic" ToolbarBasic="|Source|-|Preview|xmltemplates|
                                                                                                    |Cut|Copy|Paste|PasteText|PasteFromWord|-|Undo|Redo|
                                                                                                    |Find|Replace|SelectAll|Scayt|
                                                                                                    |Image|Table|HorizontalRule|PageBreak|SpecialChar|codesnippet|dynamictag|htmlUpload|socialshare|rssfeed|
                                                                                                    /
                                                                                                    |Maximize|ShowBlocks|About|
                                                                                                    /
                                                                                                    |Bold|Italic|Underline|Strike|Subscript|Superscript|-|RemoveFormat|
                                                                                                    |NumberedList|BulletedList|-|Outdent|Indent|-|Blockquote|CreateDiv|-|JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock|BidiLtr|BidiRtl|
                                                                                                    |Link|Unlink|Anchor|
                                                                                                    /
                                                                                                    |Styles|Format|Font|FontSize|-|TextColor|BGColor|-" runat="server" Skin="kama" Height="450px" Width="780px" BasePath="/ecn.editor/ckeditor"></CKEditor:CKEditorControl>
                            <%--<telerik:RadEditor ID="tranEditor" runat="server" Height="500px" Width="80%" EditModes="Html,Preview" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools_Simple.xml" Visible="true" />--%>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">

                    <tr>
                        <td align="right" colspan="3" style="padding-top: 10px;">
                            <input onclick="ok();" type="button" value="OK"><!-- fckLang="DlgBtnOK"-->&nbsp; 
							&nbsp;<input onclick="cancel();" type="button" value="Cancel"><!-- fckLang="DlgBtnCancel"-->
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:groupsLookup ID="groupExplorer" runat="server" UDFFilter="transactional" Visible="false" />
    </form>
    

</body>
</html>
