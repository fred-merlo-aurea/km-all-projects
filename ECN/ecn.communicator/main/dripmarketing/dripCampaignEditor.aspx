<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dripCampaignEditor.aspx.cs"
    Inherits="ecn.communicator.main.dripmarketing.dripCampaignEditor" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground2
        {
            position: absolute !important;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup2
        {
            background-color: transparent;
            position: absolute !important;
        }
        .modalPopup3
        {
            background-color: #ffffff;
            vertical-align: top;
        }
    </style>
    <link rel="stylesheet" href="styles/jquery.ui.all.css" />
   <link rel="stylesheet" href="styles/diagram.css" />
   <script src="../../scripts/dripMarketing/jquery-1.6.2.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.core.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.widget.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.mouse.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.draggable.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.resizable.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.droppable.min.js"></script>
    <script src="../../scripts/dripMarketing/jquery.ui.button.min.js"></script>
    <script src="../../scripts/dripMarketing/wz_jsgraphics.js"></script>
    <script src="../../scripts/dripMarketing/jgraphui.js"></script>
    <% if (false)
       { %>
    <script type="text/javascript" src="../../scripts/jquery-1.7.1-vsdoc.js"></script>
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {
            try {
               
                var diagram = new Diagram(
			                                {
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
			                                    onSave: function (data) {
			                                        document.getElementById('xmlSaveData').value = data;
			                                        $find('ModalSaveBehaviour').show();
			                                    },
			                                    loadTemplates: function () {
			                                        alert('This is Load Tempalte Button');

			                                    },
			                                    configureActions: function (nodeType, nodeID, inputNodes, ouputNodes) {
			                                        //alert(nodeID + ' ' + nodeType + ' ' + inputNodes[0][1].toString());
			                                        for (i = 0; i < inputNodes.length; i++) {
			                                            alert(inputNodes[i].toString());
			                                        }
			                                        //if (nodeType.toString() == 'EMAIL') {
			                                        //for (i = 0; i < inputNodes.length; i++) {
			                                        //      alert(inputNodes[i].toString());
			                                        //}			                                   
			                                        //$find('ModalBlastBehaviour').show();
			                                        //}
			                                        //else {
			                                        //    alert(nodeID + ' ' + nodeType);
			                                        //}
			                                    }
			                                });

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
                                    diagram.addNode(new Node({
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
                                diagram.addNode(new Node({
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
                                    diagram.addConnection(new Connection(connection.nodeFrom.toString(), connection.portFrom.toString(), connection.nodeTo.toString(), connection.portTo.toString(), '#969696', '3'));
                                }
                            }
                            else {
                                diagram.addConnection(new Connection(subObj.nodeFrom.toString(), subObj.portFrom.toString(), subObj.nodeTo.toString(), subObj.portTo.toString(), '#969696', '3'));
                            }

                        }
                    }
                };
            }
            catch (e) {
            }
        });
        </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="btnMdlPopupSave" Style="display: none" runat="server"></asp:Button>
            <ajaxToolkit:ModalPopupExtender ID="mdlPopSave" runat="server" TargetControlID="btnMdlPopupSave"
                PopupControlID="pnlSave" CancelControlID="btnClose" BackgroundCssClass="modalBackground2"
                BehaviorID="ModalSaveBehaviour" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" TargetControlID="pnlRoundSave"
                Radius="5" Corners="All" />
            <asp:Panel ID="pnlSave" runat="server" Width="370px" Style="display: none" CssClass="modalPopup2">
                <asp:Panel ID="pnlRoundSave" runat="server" Width="370px" CssClass="modalPopup3">
                    <div align="center" style="text-align: center; height: 150px; padding: 10px 10px 10px 10px;">
                        <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                            <tr style="background-color: #5783BD;">
                                <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">
                                    Save Campaign
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 10px" align="left">
                                    Campaign Name
                                    <asp:TextBox ID="txtCampaignName" Width="300px" runat="server" value="" ValidationGroup="SaveCampaign"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCampaignName"
                                        ErrorMessage="* required" ValidationGroup="SaveCampaign" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnPopupSaveLocation" Text="Save" CssClass="button" ValidationGroup="SaveLocation"
                                        runat="server" OnClick="btnPopupSaveCampaign_Click" />
                                    <asp:Button ID="btnClose" Text="Cancel" CssClass="button" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <input id="xmlSaveData" type="hidden" value="" runat="server" />
            <asp:Label ID="msglabel" runat="server" CssClass="greenBack" Visible="true" />
            <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" style="height: 600px;"
                border='0'>
                <tbody>
                    <tr>
                        <td id="canvasarea" width="500px" >
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
