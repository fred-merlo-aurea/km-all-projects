<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlastEmailPreview.aspx.cs" Inherits="ecn.activityengines.engines.BlastEmailPreview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet_default.css"
        type="text/css">

    <script type="text/javascript">
        function SetFullImagePath(url, clientID) {
            var obj = document.getElementById(clientID);
            if (obj != null) {
                obj.setAttribute('src', url);
                obj.setAttribute('style', 'display:block');
            }
            return false;
        }

        function ShowModalPopup(divID) {
            document.getElementById("divmdl").innerHTML = document.getElementById(divID).innerHTML;
            $find("mpe").show();
        }

        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
    </script>
    <style type="text/css">
        .label
        {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
        }

        .modalBackground
        {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .popupbody
        {
            /*background:#fffff url(images/blank.gif) repeat-x top;*/
            z-index: 101;
            background-color: #FFFFFF;
            font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
        }

        .modalPopup {
            background-color: transparent;
            padding: 1em 6px;
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
        }

        a
        {
            color: Orange;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smBEP" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>

        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlPopup" BackgroundCssClass="modalBackground" />
        <ajax:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" BehaviorID="RoundedCornersBehavior3"
            TargetControlID="pnlPopupRound" Radius="6" Corners="All" />
        <asp:Panel ID="pnlPopup" runat="server" Width="800px" Style="display: none" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupRound" runat="server" Width="800px" CssClass="modalPopup2">
                <div style="text-align:right">
                     <br />
                   <asp:Button ID="btnHide" runat="server" Text="Close" OnClientClick="return HideModalPopup()" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div id="divmdl"></div>
            </asp:Panel>
        </asp:Panel>
        <asp:UpdatePanel ID="updateBEP" runat="server">
            <ContentTemplate>
                <ajax:TabContainer ID="tabContainter" runat="server" Height="100%">
                    <ajax:TabPanel ID="tpEmailPreview" runat="server" TabIndex="1" HeaderText="Email Preview">
                        <ContentTemplate>
                            <br />
                            <div align="center" style="position: fixed; top: 20; left: 20; width: 15%; height: 100%; overflow: auto;">
                                <asp:Panel ID="pnlSideBar" runat="server" Style="background-color: #5D5A53; border: 2px solid black; padding: 20px 20px 20px 0px">
                                    <asp:Label ID="lbEmailClients" runat="server" CssClass="label" Text="Email Clients"
                                        Font-Size="Large" ForeColor="White" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Repeater ID="rptSideBar" runat="server" OnItemDataBound="rptSideBar_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Label ID="lbEmailAppName" runat="server" CssClass="label" ForeColor="White"></asp:Label>
                                            <br />
                                            <asp:ImageButton ID="btnEmailThumb" runat="server" CausesValidation="false" ImageUrl="http://images.ecn5.com/images/loading.gif" />
                                            <br />
                                            <br />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </div>
                            <div style="top: 20; left: 350; width: 85%; height: 100%; overflow: auto; float: right; color: White;">
                                <asp:Panel ID="pnlImage" runat="server" Style="background-color: #5D5A53; padding: 20px 20px 20px 20px;">
                                    <center>
                                        <asp:Image ID="imgEmail" runat="server" ImageAlign="Middle" AlternateText="No Thumbnail Selected"
                                            Style="display: none;" />
                                    </center>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </ajax:TabPanel>
                    <ajax:TabPanel ID="tpHtmlValidation" runat="server" Visible="false" TabIndex="2" HeaderText="HTML Validation">
                        <ContentTemplate>
                            <div style="top: 20; left: 20; width: 100%; height: 100%; overflow: auto; color: black;">
                                <asp:Panel ID="pnlHtmlVal" runat="server" Style="background-color: #ffffff; padding: 20px 20px 20px 20px;">
                                    <asp:Label ID="lbHtmlVal" runat="server" CssClass="label" Text="HTML Validation"
                                        Font-Size="Large" ForeColor="black" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                    <div align="left">
                                        <asp:Literal ID="litHtml" runat="server"></asp:Literal>
                                    </div>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </ajax:TabPanel>
                    <ajax:TabPanel ID="tpSpam" runat="server" TabIndex="3" HeaderText="Spam Scores">
                        <ContentTemplate>
                            <div align="left" style="top: 20; left: 20; width: 100%; height: 100%; overflow: auto; color: black;">
                                <asp:Panel ID="pnlSpam" runat="server" Style="background-color: #ffffff; padding: 20px 20px 20px 20px;">
                                    <asp:Label ID="lbSpam" runat="server" CssClass="label" Text="Spam Analysis" Font-Size="Large"
                                        ForeColor="black" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Repeater ID="rptSpam" runat="server" OnItemDataBound="rptSpam_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlSpamHeader" runat="server" BackColor="#eeeeee" Height="40px">
                                                <table cellpadding="2" cellspacing="2" border="0">
                                                    <tr>
                                                        <td width="1%" align="center" valign="middle">&nbsp;</td>
                                                        <td width="3%" align="left" valign="middle">
                                                            <asp:Image ID="imgStatus" runat="server" ImageAlign="Middle" /></td>
                                                        <td width="40%" align="left" valign="middle">
                                                            <asp:Label ID="lblspamApplication" runat="server" CssClass="headingOne" ForeColor="black"></asp:Label></td>
                                                        <td width="55%" align="right" valign="middle">
                                                            <asp:Label ID="lblStatus" runat="server" CssClass="headingOne"></asp:Label>&nbsp;<asp:Label ID="lblScore" runat="server" CssClass="headingOne" ForeColor="black"></asp:Label></td>
                                                        <td width="1%" align="center" valign="middle">
                                                            <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="http://images.ecn5.com/images/expand_blue.jpg" /></td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlSpamDesc" runat="server" BorderColor="#eeeeee" BorderWidth="2" Height="0" Style="padding: 5px 5px 5px 5px">
                                                <br />
                                                <p>
                                                    <asp:Literal ID="litSpam" runat="server" Visible="true"></asp:Literal>
                                                </p>
                                            </asp:Panel>
                                            <br />
                                            <ajax:CollapsiblePanelExtender ID="cpe1" runat="Server" TargetControlID="pnlSpamDesc"
                                                ExpandControlID="pnlSpamHeader" CollapseControlID="pnlSpamHeader" Collapsed="False"
                                                SuppressPostBack="true" ImageControlID="imgExpand"
                                                ExpandedImage="http://images.ecn5.com/images/collapse_blue.jpg"
                                                CollapsedImage="http://images.ecn5.com/images/expand_blue.jpg"
                                                SkinID="CollapsiblePanelDemo" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </ajax:TabPanel>
                    <ajax:TabPanel ID="tpCodeAnalysis" runat="server" TabIndex="3" HeaderText="Code Analysis">
                        <ContentTemplate>
                            <div align="left" style="top: 20px; left: 20px; width: 100%; height: 100%; overflow: auto; color: black;">
                                <asp:Panel ID="pnlCodeAnalysis" runat="server" Style="background-color: #ffffff; padding: 20px 20px 20px 20px;">
                                    <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCodeAnalysisResult" runat="server" CssClass="label" Text=""
                                                    ForeColor="black" Font-Bold="true"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:PlaceHolder ID="plPotentialProblems" runat="server" Visible="false">
                                                    <asp:Label ID="lblCompatibiltyReport" runat="server" CssClass="label" Text="Potential Problems" Font-Size="Large"
                                                        ForeColor="black" Font-Bold="true"></asp:Label><br />
                                                    <br />
                                                    <asp:Repeater ID="rptPotentialProblems" runat="server" OnItemDataBound="rptPotentialProblems_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlPotentialProblemsHeader" runat="server" BackColor="#eeeeee" Height="40px">
                                                                <table cellpadding="2" cellspacing="2" border="0" height="40px">
                                                                    <tr>
                                                                        <td width="99%">
                                                                            <asp:Label ID="lblApplicationName" runat="server" CssClass="headingOne" ForeColor="black" Text='<%# Eval("ApplicationLongName")%>'></asp:Label></td>
                                                                        <td width="1%" align="center" valign="middle">
                                                                            <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="http://images.ecn5.com/images/expand_blue.jpg" /></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlPotentialProblemsDesc" runat="server" BorderColor="#eeeeee" BorderWidth="2" Height="0" Style="padding: 5px 5px 5px 5px">
                                                                <p>
                                                                    <asp:Literal ID="litPotentialProblems" runat="server" Visible="true"></asp:Literal>
                                                                </p>
                                                            </asp:Panel>
                                                            <br />
                                                            <ajax:CollapsiblePanelExtender ID="cpe1" runat="Server" TargetControlID="pnlPotentialProblemsDesc"
                                                                ExpandControlID="pnlPotentialProblemsHeader" CollapseControlID="pnlPotentialProblemsHeader" Collapsed="False"
                                                                SuppressPostBack="true" ImageControlID="imgExpand"
                                                                ExpandedImage="http://images.ecn5.com/images/collapse_blue.jpg"
                                                                CollapsedImage="http://images.ecn5.com/images/expand_blue.jpg"
                                                                SkinID="CollapsiblePanelDemo" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="plHtmlValidation" runat="server" Visible="false">
                                                    <asp:Label ID="lblHtmlValidation" runat="server" CssClass="label" Text="Html Validation" Font-Size="Large"
                                                        ForeColor="black" Font-Bold="true"></asp:Label><br />
                                                    <br />
                                                    <asp:Panel ID="pnlCodeHtmlValidationHeader" runat="server" BackColor="#eeeeee" Height="40px">
                                                        <table cellpadding="2" cellspacing="2" border="0" height="40px">
                                                            <tr>
                                                                <td width="99%">
                                                                    <asp:Label ID="lblApplicationName" runat="server" CssClass="headingOne" ForeColor="black" Text="Html Problems"></asp:Label></td>
                                                                <td width="1%" align="center" valign="middle">
                                                                    <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="http://images.ecn5.com/images/expand_blue.jpg" /></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlCodeHtmlValidationDesc" runat="server" BorderColor="#eeeeee" BorderWidth="2" Height="0" Style="padding: 5px 5px 5px 5px">
                                                        <asp:Repeater ID="rptCodeHtmlValidation" runat="server" OnItemDataBound="rptCodeHtmlValidation_ItemDataBound">
                                                            <ItemTemplate>
                                                                <p>
                                                                    <asp:Literal ID="litCodeHtmlValidation" runat="server" Visible="true"></asp:Literal>
                                                                </p>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </asp:Panel>
                                                    <ajax:CollapsiblePanelExtender ID="cpe2" runat="Server" TargetControlID="pnlCodeHtmlValidationDesc"
                                                        ExpandControlID="pnlCodeHtmlValidationHeader" CollapseControlID="pnlCodeHtmlValidationHeader" Collapsed="False"
                                                        SuppressPostBack="true" ImageControlID="imgExpand"
                                                        ExpandedImage="http://images.ecn5.com/images/collapse_blue.jpg"
                                                        CollapsedImage="http://images.ecn5.com/images/expand_blue.jpg"
                                                        SkinID="CollapsiblePanelDemo" />
                                                </asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </ajax:TabPanel>
                    <ajax:TabPanel ID="tpLinkCheck" runat="server" TabIndex="3" HeaderText="Link Check">
                        <ContentTemplate>
                            <div align="left" style="top: 20px; left: 20px; width: 100%; height: 100%; overflow: auto; color: black;">
                                <asp:Panel ID="Panel1" runat="server" Style="background-color: #ffffff; padding: 20px 20px 20px 20px;">
                                    <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLinkCheckResult" runat="server" CssClass="label" Text=""
                                                    ForeColor="black" Font-Bold="true"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ImageMap ID="ImageMap1" runat="server"></asp:ImageMap>
                                                <asp:Repeater ID="rptLinkCheck" runat="server" OnItemDataBound="rptLinkCheck_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div id='div_<%# Eval("TopLeftX")%>_<%# Eval("TopLeftY")%>' style="display: none">
                                                            <table cellpadding="2" cellspacing="2" border="0" height="40px">
                                                                <tr>
                                                                    <td width="99%" align="left" valign="middle">
                                                                        <asp:Label ID="lblPageTitle" runat="server" CssClass="headingOne" ForeColor="black" Text='<%# Eval("PageTitle")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td width="50%">
                                                                        <table cellpadding="2" cellspacing="2" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="btnLinkThumb" runat="server" CausesValidation="false" ImageUrl="http://images.ecn5.com/images/loading.gif" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <%# Eval("Url")%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="50%">
                                                                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="imgIsValid" runat="server" CausesValidation="false" />
                                                                                    Link works correctly
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="imgIsBlackListed" runat="server" CausesValidation="false" />
                                                                                    URL is not blacklisted</td>
                                                                            </tr>
<%--                                                                         <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="imgHasClickTracking" runat="server" CausesValidation="false" />
                                                                                    Click throughs are tracked</td>
                                                                            </tr>--%>
                                                                            <asp:PlaceHolder ID="plGoogleAnalystic" runat="server" Visible="false">
                                                                                <tr>
                                                                                    <td>
                                                                                        <br />
                                                                                        <br />
                                                                                        <asp:Label ID="lblGoogleAnalystic" runat="server" CssClass="headingOne" ForeColor="black" Text="Google Analystics"></asp:Label><br />
                                                                                        <br />
                                                                                        <asp:Literal ID="litLinkCheck" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                            </asp:PlaceHolder>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                         </div>
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </ajax:TabPanel>
                </ajax:TabContainer>
                <asp:Panel ID="pnlMessage" runat="server" CssClass="popupbody" Width="400px" Height="200px">
                    <div style="float: right;">
                        <asp:ImageButton ID="btnMessageClose" runat="server" ImageUrl="~/images/close.png"
                            CausesValidation="false" />
                    </div>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Image ID="imgMessage" runat="server" ImageUrl="~/images/Info_24x24.png" />
                                    <asp:Label ID="lbMessage" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <asp:Button ID="hfMessage" runat="server"  style="display:none;"/>
                <ajax:RoundedCornersExtender ID="rceMessage" runat="server" Corners="All" Radius="10"
                    TargetControlID="pnlMessage" BorderColor="AliceBlue">
                </ajax:RoundedCornersExtender>
                <ajax:ModalPopupExtender ID="mpeMessage" runat="server" BackgroundCssClass="modalBackground"
                    CancelControlID="btnMessageClose" PopupControlID="pnlMessage" TargetControlID="hfMessage">
                </ajax:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

