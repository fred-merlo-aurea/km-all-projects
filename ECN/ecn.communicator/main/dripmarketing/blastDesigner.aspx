<%@ Page Language="c#" Inherits="ecn.communicator.main.dripmarketing.blastDesigner"
    CodeBehind="blastDesigner.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="regBlast" TagName="regconfig" Src="~/main/dripmarketing/Controls/regBlast.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .rowBackground
        {
            -moz-box-shadow:inset 0px 1px 0px 0px #ffffff;
	        -webkit-box-shadow:inset 0px 1px 0px 0px #ffffff;
	        box-shadow:inset 0px 1px 0px 0px #ffffff;
	        background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	        background:-moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
            background-color: #ededed;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            font-family: arial;
            font-size: px;
            font-weight: bold;
            padding:4px 18px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }
        .aspBtn1
        {
            -moz-box-shadow:inset 0px 1px 0px 0px #ffffff;
	        -webkit-box-shadow:inset 0px 1px 0px 0px #ffffff;
	        box-shadow:inset 0px 1px 0px 0px #ffffff;
	        background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	        background:-moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
            background-color: #ededed;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            color: black;
            font-family: arial;
            font-size: px;
            font-weight: bold;
            padding:4px 18px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }
        .aspBtn1:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background-color: #dfdfdf;
        }
        .aspBtn1:active
        {
            position: relative;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.5;
        }
        .modalPopupFull
        {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 100%;
            height: 100%;
            overflow: auto;
        }
        .modalPopupSaveCampaign
        {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }
        .modalPopupBlastConfig
        {
           background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
            width: 45%;
        }
        .modalPopupImport
        {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            height: 60%;
            overflow: auto;
        }
        .style1
        {
            width: 100%;
        }
        .buttonMedium
        {
            width: 135px;
            background: url(buttonMedium.gif) no-repeat left top;
            border: 0;
            font-weight: bold;
            color: #ffffff;
            height: 20px;
            cursor: pointer;
            padding-top: 2px;
        }
        .TransparentGrayBackground
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            height: 100%;
            width: 100%;
            min-height: 100%;
            min-width: 100%;
        }
        .overlay
        {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
        .loader
        {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }
        * html .loader
        {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
    </style>
    <link rel="stylesheet" href="styles/jquery.ui.all.css" />
    <link rel="stylesheet" href="styles/diagram.css" />
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.core.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.widget.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.mouse.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.draggable.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.resizable.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.droppable.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jquery.ui.button.min.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/wz_jsgraphics.js"></script>
    <script type="text/javascript" src="../../scripts/dripMarketing/jgraphui.js"></script>
    <script type="text/javascript">

        var diagramStatic;
        function viewReport_confirm(campaignItemID) {
            var r = confirm("This Campaign Item has been sent. Would you like to view the Report?")
            if (r == true) {
                window.open("../../main/blasts/reports.aspx?campaignItemID=" + campaignItemID, "_blank", "", "")
            }
        }

        function deleteNode(nodeID) {
            diagramStatic.deleteNode(nodeID);
            diagramStatic.toXML();
        }

        function updateNodeContent(nodeID, nodeContent) {
            diagramStatic.updateNodeContent(nodeID, "Message: "+nodeContent);
            $("div.EMAIL" + nodeID.toString()).html("Message: " + nodeContent);
            diagramStatic.toXML();
        }

        $(document).ready(function () {
            try {
                var myDiagram = new Diagram(
			                        {
			                            'id': 'myCanvas',
			                            'xPosition': $('#canvasarea').offset().left,
			                            'yPosition': $('#canvasarea').offset().top,
			                            'width': $('#canvasarea').width(),
			                            'height': 550,
			                            'imagesPath': 'images/',
			                            'connectionColor': '#969696',
			                            'toolbar_load_button': false,
			                            'toolbar_background_color_button': false,
			                            'toolbar_border_color_button': false,
			                            'toolbar_font_color_button': false,
			                            'toolbar_font_size_button': false,
			                            'toolbar_font_family_button': false,
			                            'toolbar_border_width_button': false,
			                            onSave: function (xmlData, allNodes) {

			                                for (i = 0; i < allNodes.length; i++) {
			                                    if (allNodes[i].toString().indexOf("EMAIL") != -1) {
			                                        var desc = $('div.EMAIL' + allNodes[i].toString()).html();
			                                        var searchString = "<node nodeId=\"" + allNodes[i].toString() + "\" nodeType=\"IMAGE\" nodeContent=\"EMAIL\"";
			                                        xmlData = xmlData.replace(searchString,
                                                             "<node nodeId=\"" + allNodes[i].toString() + "\" nodeType=\"IMAGE\" nodeContent=\"" + desc + "\"");
			                                    }
			                                }
			                                document.getElementById('<%=xmlSaveData.ClientID%>').value = xmlData;
			                                var val = "<%=btnSavePostBack.ClientID%>";
			                                var savePopUp = document.getElementById(val);
			                                if (savePopUp) {
			                                    savePopUp.click();
			                                }
			                            },
			                            loadTemplates: function () {

			                            },
			                            deleteNodes: function (nodeID) {
			                                document.getElementById("<%= currentNode.ClientID %>").value = nodeID;
			                                if (nodeID.indexOf("EMAIL") > -1) {
			                                    var btn = document.getElementById('<%=btnDeletePostback.ClientID %>');
			                                    if (btn) btn.click();
			                                }
			                                else {
			                                    deleteNode(nodeID);
			                                }
			                            },
			                            configureActions: function (nodeType, nodeID, inputNodes, outputNodes, xmlData, allNodes) {
			                                for (i = 0; i < allNodes.length; i++) {
			                                    if (allNodes[i].toString().indexOf("EMAIL") != -1) {
			                                        var desc = $('div.EMAIL' + allNodes[i].toString()).html();
			                                        var searchString = "<node nodeId=\"" + allNodes[i].toString() + "\" nodeType=\"IMAGE\" nodeContent=\"EMAIL\"";
			                                        xmlData = xmlData.replace(searchString,
                                                             "<node nodeId=\"" + allNodes[i].toString() + "\" nodeType=\"IMAGE\" nodeContent=\"" + desc + "\"");
			                                    }
			                                }

			                                document.getElementById('<%=xmlSaveData.ClientID%>').value = xmlData;
			                                if (nodeID.indexOf("EMAIL") != -1) {
			                                    if (inputNodes.length > 0) {
			                                        for (i = 0; i < inputNodes.length; i++) {
			                                            if (inputNodes[i].toString().indexOf("START") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = "0";
			                                            }
			                                            if (inputNodes[i].toString().indexOf("EMAIL") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = "0";
			                                            }

			                                            if (inputNodes[i].toString().indexOf("NOCLICK") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = inputNodes[i].toString();
			                                            }
			                                            else if (inputNodes[i].toString().indexOf("NOOPEN") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = inputNodes[i].toString();
			                                            }
			                                            else if (inputNodes[i].toString().indexOf("CLICK") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = inputNodes[i].toString();
			                                            }
			                                            else if (inputNodes[i].toString().indexOf("OPEN") != -1) {
			                                                document.getElementById("<%= originNode.ClientID %>").value = inputNodes[i].toString();
			                                            }
			                                        }
			                                        document.getElementById("<%= currentNode.ClientID %>").value = nodeID;
			                                        var btn = document.getElementById('<%=btnConfigurePostback.ClientID %>');
			                                        if (btn) btn.click();
			                                    }
			                                    else {
			                                        alert('This action must have a parent');
			                                    }
			                                }
			                            }
			                        });
                var campaignID = document.getElementById('<%=campaignID.ClientID%>').value;
                if (campaignID != 0) {

                    var JSONobj = eval('(' + document.getElementById('<%=xmlSaveData.ClientID %>').value + ')');
                    for (x in JSONobj.diagram) {
                        if (typeof (JSONobj.diagram[x]) == "object") {
                            var subObj = JSONobj.diagram[x];

                            //if array, creates multiple connection or multiple nodes
                            if (subObj instanceof Array) {
                                var filter = subObj[0];
                            }
                            else {
                                var filter = subObj;
                            }

                            //nodeFrom is a property of a connection               
                            if (filter.nodeFrom == null) {
                                if (subObj instanceof Array) {
                                    for (var i = 0; i < subObj.length; i++) {
                                        var node = subObj[i];
                                        myDiagram.addNode(new Node({
                                            'nodeId': node.nodeId,
                                            'nodeType': node.nodeType,
                                            'nodeContent': node.nodeContent,
                                            'xPosition': parseInt(node.xPosition),
                                            'yPosition': parseInt(node.yPosition),
                                            'width': node.width,
                                            'height': node.height,
                                            'bgColor': node.bgColor,
                                            'borderColor': node.borderColor,
                                            'borderWidth': node.borderWidth,
                                            'fontColor': node.fontColor,
                                            'fontSize': node.fontSize,
                                            'fontType': node.fontType,
                                            'minHeight': node.minHeight,
                                            'maxHeight': node.maxHeight,
                                            'minWidth': node.minWidth,
                                            'maxWidth': node.maxWidth,
                                            'nPort': (node.nPort === 'true'),
                                            'ePort': (node.ePort === 'true'),
                                            'sPort': (node.sPort === 'true'),
                                            'wPort': (node.wPort === 'true'),
                                            'image': node.image,
                                            'draggable': (node.draggable === 'true'),
                                            'resizable': (node.resizable === 'true'),
                                            'editable': (node.editable === 'true'),
                                            'selectable': (node.selectable === 'true'),
                                            'deletable': (node.deletable === 'true'),
                                            'nPortMakeConnection': (node.nPortMakeConnection === 'true'),
                                            'ePortMakeConnection': (node.ePortMakeConnection === 'true'),
                                            'sPortMakeConnection': (node.sPortMakeConnection === 'true'),
                                            'wPortMakeConnection': (node.wPortMakeConnection === 'true'),
                                            'nPortAcceptConnection': (node.nPortAcceptConnection === 'true'),
                                            'ePortAcceptConnection': (node.ePortAcceptConnection === 'true'),
                                            'sPortAcceptConnection': (node.sPortAcceptConnection === 'true'),
                                            'wPortAcceptConnection': (node.wPortAcceptConnection === 'true')

                                        }));
                                    }
                                }
                                else {
                                    var node = subObj;
                                    myDiagram.addNode(new Node({
                                        'nodeId': node.nodeId,
                                        'nodeType': node.nodeType,
                                        'nodeContent': node.nodeContent,
                                        'xPosition': parseInt(node.xPosition),
                                        'yPosition': parseInt(node.yPosition),
                                        'width': node.width,
                                        'height': node.height,
                                        'bgColor': node.bgColor,
                                        'borderColor': node.borderColor,
                                        'borderWidth': node.borderWidth,
                                        'fontColor': node.fontColor,
                                        'fontSize': node.fontSize,
                                        'fontType': node.fontType,
                                        'minHeight': node.minHeight,
                                        'maxHeight': node.maxHeight,
                                        'minWidth': node.minWidth,
                                        'maxWidth': node.maxWidth,
                                        'nPort': (node.nPort === 'true'),
                                        'ePort': (node.ePort === 'true'),
                                        'sPort': (node.sPort === 'true'),
                                        'wPort': (node.wPort === 'true'),
                                        'image': node.image,
                                        'draggable': (node.draggable === 'true'),
                                        'resizable': (node.resizable === 'true'),
                                        'editable': (node.editable === 'true'),
                                        'selectable': (node.selectable === 'true'),
                                        'deletable': (node.deletable === 'true'),
                                        'nPortMakeConnection': (node.nPortMakeConnection === 'true'),
                                        'ePortMakeConnection': (node.ePortMakeConnection === 'true'),
                                        'sPortMakeConnection': (node.sPortMakeConnection === 'true'),
                                        'wPortMakeConnection': (node.wPortMakeConnection === 'true'),
                                        'nPortAcceptConnection': (node.nPortAcceptConnection === 'true'),
                                        'ePortAcceptConnection': (node.ePortAcceptConnection === 'true'),
                                        'sPortAcceptConnection': (node.sPortAcceptConnection === 'true'),
                                        'wPortAcceptConnection': (node.wPortAcceptConnection === 'true')

                                    }));
                                }
                            }
                        }
                    };

                    for (x in JSONobj.diagram) {

                        if (typeof (JSONobj.diagram[x]) == "object") {
                            var subObj = JSONobj.diagram[x];

                            //if array, creates multiple connection or multiple nodes
                            if (subObj instanceof Array) {
                                var filter = subObj[0];
                            }
                            else {
                                var filter = subObj;
                            }

                            //nodeFrom is a property of a connection               
                            if (filter.nodeFrom != null) {

                                if (subObj instanceof Array) {
                                    for (var i = 0; i < subObj.length; i++) {
                                        var connection = subObj[i];
                                        myDiagram.addConnection(new Connection(connection.nodeFrom.toString(), connection.portFrom.toString(), connection.nodeTo.toString(), connection.portTo.toString(), '#969696', '3'));
                                    }
                                }
                                else {
                                    myDiagram.addConnection(new Connection(subObj.nodeFrom.toString(), subObj.portFrom.toString(), subObj.nodeTo.toString(), subObj.portTo.toString(), '#969696', '3'));
                                }

                            }
                        }
                    };

                }
                diagramStatic = myDiagram;

            }
            catch (e) {
            }
        });        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upMainProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input id="xmlSaveData" type="hidden" value="" runat="server" />
            <input id="originNode" type="hidden" value="" runat="server" />
            <input id="currentNode" type="hidden" value="" runat="server" />
            <input id="campaignID" type="hidden" value="0" runat="server" />
            <asp:Button ID="btnSavePostBack" Style="display: none" runat="server" OnClick="btnMdlPopupSave_Click">
            </asp:Button>
            <asp:Button ID="btnDeletePostback" Text="btn" runat="server" Style="display: none"
                OnClick="btnDeletePostback_Click" />
            <asp:Button ID="btnConfigurePostback" Text="btn" runat="server" Style="display: none"
                OnClick="btnConfigurePostback_Click" />
            <asp:Label ID="msglabel" runat="server" CssClass="greenBack" Visible="true" />
            <br />
            <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" style="height: 600px;"
                border='0'>
                <tbody>
                    <tr>
                        <td id="canvasarea" width="500px">
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnMdlPopupSave" Style="display: none" runat="server"></asp:Button>
    <ajaxToolkit:ModalPopupExtender ID="mdlPopSave" runat="server" TargetControlID="btnMdlPopupSave"
        PopupControlID="pnlSave" BackgroundCssClass="modalBackground" />
    <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="pnlSave">
    </ajaxToolkit:RoundedCornersExtender>
    <asp:Panel ID="pnlSave" runat="server" CssClass="modalPopupSaveCampaign">
     <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
        DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upMainProgressSaveP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upMainProgressSaveP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div align="center">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                        <tr style="background-color: #5783BD;" align="center">
                            <td style="padding: 5px; font-size: 18px; color: #ffffff; font-weight: bold">
                                Save Campaign
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px" align="left">
                                Campaign Name
                                <asp:TextBox ID="txtCampaignName" Width="300px" runat="server" value="" ValidationGroup="SaveCampaign"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <asp:Button ID="btnPopupSaveLocation" Text="Save" CssClass="aspBtn1" ValidationGroup="SaveLocation"
                                    runat="server" OnClick="btnPopupSaveCampaign_Click" />
                                <asp:Button ID="btnClose" Text="Cancel" CssClass="aspBtn1" runat="server" OnClick="btnPopupCloseSaveCampaign_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="mdlPopNewBlast" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlNewBlast" TargetControlID="btnShowPopup3">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlNewBlast" CssClass="modalPopupBlastConfig">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
            DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upMainNewBlastP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upMainNewBlastP2" CssClass="loader" runat="server">
                        <div>
                            <center>
                                <br />
                                <br />
                                <b>Processing...</b><br />
                                <br />
                                <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                                <br />
                                <br />
                                <br />
                            </center>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div align="center" style="background: white;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" >
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">
                                Campaign Item Details
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <regBlast:regconfig ID="regBlast1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSaveBlast" Text="Save" CssClass="aspBtn1" runat="server" OnClick="Schedule_Save" />
                            </td>
                            <td align="left">
                                <asp:Button ID="btnCancel" Text="Cancel" CssClass="aspBtn1" runat="server" OnClick="Schedule_Close" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
