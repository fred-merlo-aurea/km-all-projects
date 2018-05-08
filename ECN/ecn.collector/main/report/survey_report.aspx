<%@ Page Language="c#" Inherits="ecn.collector.main.report.SurveyReport" CodeBehind="survey_report.aspx.cs"
    MasterPageFile="~/MasterPages/Collector.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ MasterType VirtualPath="~/MasterPages/Collector.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function clearTableBorder() {
            document.getElementById("dgQuestions").setAttribute("border", "0");
        }

        function prepareRollRow() {
            if (!document.getElementsByTagName) return false;
            if (!document.getElementById("dgQuestions")) return false;
            var rollTable = document.getElementById("dgQuestions");
            var rowRolls = rollTable.getElementsByTagName("tr");
            for (var i = 0; i < rowRolls.length; i++) {

                if (i != 0 && i % 2) rowRolls[i].onmouseover = function () {
                    this.style.background = "#fbef87";
                }

                if (i != 0 && i % 2) rowRolls[i].onmouseout = function () {
                    this.style.background = "#ffffff";
                }

                if (i != 0 && !(i % 2)) rowRolls[i].onmouseover = function () {
                    this.style.background = "#fbef87";
                }

                if (i != 0 && !(i % 2)) rowRolls[i].onmouseout = function () {
                    this.style.background = "#EBEBEC";
                }
            }
        }

        function addLoadEvent(func) {
            var oldonload = window.onload;
            if (typeof window.onload != 'function') {
                window.onload = func;
            } else {
                window.onload = function () {
                    oldonload();
                    func();
                }
            }
        }

        addLoadEvent(prepareRollRow);

        function setDimmerSize() {
            var winW = 630, winH = 460;

            if (parseInt(navigator.appVersion) > 3) {
                if (navigator.appName == "Netscape") {
                    winW = window.innerWidth;
                    winH = window.innerHeight;
                }
                if (navigator.appName.indexOf("Microsoft") != -1) {
                    winW = document.body.offsetWidth;
                    winH = document.body.offsetHeight;
                }
            }

            Dimmer = document.getElementById("dimmer")
            Dimmer.style.height = winH + 'px';
            Dimmer.style.width = winW + 'px';

            if (document.getElementById("dimmerIE")) {
                DimmerTwo = document.getElementById("dimmerIE")
                DimmerTwo.style.height = winH + 'px';
                DimmerTwo.style.width = winW + 'px';
            }

            DimmerContainer = document.getElementById("dimmerContainer")
            DimmerContainer.style.height = winH + 'px';
            DimmerContainer.style.width = winW + 'px';

            ContCell = document.getElementById("containerCell")
            ContCell.style.height = winH + 'px';
        }

        function hideBody() {
            var browser = navigator.appName
            if (browser == "Microsoft Internet Explorer") {
                bodyElement = document.getElementsByTagName('body')[0];
                bodyElement.style.overflow = 'hidden';
                document.getElementById("dimmerIE").style.display = "block";
            }
        }

        function showBody() {
            var browser = navigator.appName
            if (browser == "Microsoft Internet Explorer") {
                bodyElement = document.getElementsByTagName('body')[0];
                bodyElement.style.overflow = 'auto';
                document.getElementById("dimmerIE").style.display = "none";
            }
        }

        function getobj(id) {
            id = 'ctl00_ContentPlaceHolder1_' + id;
            if (document.all && !document.getElementById)
                obj = eval('document.all.' + id);
            else if (document.layers)
                obj = eval('document.' + id);
            else if (document.getElementById)
                obj = document.getElementById(id);

            return obj;
        }

        function ExportSurveyResponses() {
            var btnExport = document.getElementById('<%=btnExport.ClientID%>');
            btnExport.click();
        }

        function CloseExportWindow() {
            var btnClose = document.getElementById('<%=imgbtnCloseExport.ClientID%>');
            btnClose.click();
        }

        //function showDivPopup(divName) {
        //    document.getElementById("dimmer").style.display = 'block';
        //    document.getElementById("dimmerContainer").style.display = 'block';
        //    document.getElementById(divName).style.display = 'block';
        //    setDimmerSize();
        //    hideBody();
        //    return false;
        //}

        function hideDivPopup(divName) {
            document.getElementById("dimmer").style.display = 'none';
            document.getElementById("dimmerContainer").style.display = 'none';
            document.getElementById(divName).style.display = 'none';
            showBody();
        }

        function resetcheckbox(ctrl, qID) {
            var inputs = document.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'checkbox' && ctrl != inputs[i] && inputs[i].name.indexOf('checkbox_' + qID + '_') > 0) {
                    inputs[i].checked = false;
                }
            }
        }


    </script>
    <script language='javascript'>        onresize = function () { setDimmerSize() };</script>
    <style type='text/css'>
        #dimmer {
            position: fixed;
            top: 0;
            display: none;
            left: 0px;
            width: 100%;
            z-index: 10;
        }

        * html #dimmer {
            position: absolute;
        }

        html > /**/ body #dimmer {
            background: url(/ecn.images/images/gray.png) top left repeat;
        }

        * #dimmerIE {
            position: absolute;
            top: 0;
            display: none;
            left: 0px;
            width: 100%;
            z-index: 2;
            _background-image: none;
            _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=scale src='/ecn.images/images/gray.png');
        }

        #dimmerContainer {
            position: fixed;
            top: 0;
            display: none;
            left: 0px;
            font-family: Arial;
            font-weight: bold;
            width: 100%;
            z-index: 100;
        }

        * html #dimmerContainer {
            position: absolute;
        }

        div.dimming {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-style: normal;
            position: relative;
        }

        #divQuestion, #divPage, #divReOrder, #divBranch, #divAlert {
            display: none;
        }

        .descWiz {
            font-family: "Arial";
            font-size: 12px;
            padding: 6px;
        }

        .descWizsmall {
            font-family: "Arial";
            font-size: 11px;
            padding: 6px;
        }

        .dimmerInner {
            max-height: 300px;
            overflow: auto;
        }

        * html .dimmerInner {
            height: 300px;
            overflow: auto;
        }

        #dgQuestions {
            border-top: none;
        }

            #dgQuestions tr td {
                padding: 5px;
            }

        #frmReport .gridheaderWizard {
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cr:crystalreportviewer id="crv" runat="Server" width="350px" separatepages="False"
        height="50px" toolpanelview="None" viewstatemode="Disabled" enabledrilldown="False"
        displaytoolbar="False" visible="false"></cr:crystalreportviewer>
    <br />

    <script type="text/javascript">
        var browser = navigator.appName;
        if (browser == "Microsoft Internet Explorer") {
            document.write('<div id="dimmerIE"></div>');
        }
    </script>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="5" Visible="true"
        AssociatedUpdatePanelID="upExportToGroup" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="http://images.ecn5.com/images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Button ID="hfExportToGroup" style="display:none;" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeExportToGroup" runat="server" TargetControlID="hfExportToGroup" PopupControlID="upExportToGroup" BackgroundCssClass="ECN-ModalBackground" />
    <asp:UpdatePanel ID="upExportToGroup" UpdateMode="Always" runat="server">

        <ContentTemplate>
            
            <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="position: relative;">
                <tr>
                    <td align="center" valign="middle" id="containerCell">
                            <table width="325" align="center" border='0' cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dimTopLeft">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="dimTopCenter" width="100%">
                                        <span>Add Respondent to group.</span>
                                    </td>
                                    <td class="dimTopRight" style="width:20px;"><asp:LinkButton ID="imgbtnCloseExport" runat="server" CausesValidation="false" CssClass="transparent-button" ImageUrl="/ecn.images/images/divs_05.gif" OnClick="imgbtnCloseExport_Click" /></td>
                                </tr>
                                <tr>
                                    <td class="dimMiddleLeft"></td>
                                    <td class="dimMiddleCenter">
                                        <table align="center" border='0' cellpadding="0" cellspacing="0" width="99%">
                                            <tr>
                                                <td bgcolor="#ffffff" valign="top">
                                                    <table border='0' cellpadding="5" cellspacing="0" width="304">
                                                        <tr>
                                                            <td valign="top" style="text-align:left;">
                                                                
                                                                    <asp:RadioButton ID="rbNewGroup" runat="Server" Text="Create New Email List" GroupName="grpSelect"
                                                                        AutoPostBack="true" OnCheckedChanged="rbNewGroup_CheckedChanged"></asp:RadioButton>
                                                                    <br />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:PlaceHolder ID="plNewGroup" runat="Server" Visible="false">
                                                                    <asp:TextBox ID="txtGroupName" CssClass="dataOne" runat="Server" Width="200" Style="width: 200px; position: relative;"
                                                                        MaxLength="100"></asp:TextBox>&nbsp;
																	<asp:RequiredFieldValidator ID="rfvtxtGroupName" runat="Server" Font-Size="xx-small" ControlToValidate="txtGroupName"
                                                                        ErrorMessage="Â« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator></asp:PlaceHolder>
                                                               </tr>
                                                               <tr>
                                                                <td style="text-align:left;">
                                                                    <asp:RadioButton ID="rbExistingGroup" AutoPostBack="true" GroupName="grpSelect" Text="Use Existing Email List"
                                                                        runat="Server" OnCheckedChanged="rbExistingGroup_CheckedChanged"></asp:RadioButton>
                                                                    <br />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:PlaceHolder ID="plExistingGroup" Visible="false" runat="Server">
                                                                    <asp:DropDownList ID="drpGroup" CssClass="dataOne" runat="Server" Width="250px" Style="width: 200px; position: relative;"></asp:DropDownList>&nbsp;
																	<asp:RequiredFieldValidator ID="rfvdrpGroup" runat="Server" Font-Size="xx-small" ControlToValidate="drpGroup"
                                                                        ErrorMessage="Â« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator></asp:PlaceHolder>
                                                               </td>
                                                                   </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblErrorMessage" runat="Server" Visible="false" ForeColor="Red" Font-Size="x-small" Style="position:relative"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                    
                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#ffffff" valign="top">
                                                    <table border='0' cellpadding="1" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btnSaveToGroup" Text="Finish" runat="Server" OnClick="btnSaveToGroup_Click"></asp:Button>
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="dimMiddleRight" width="53"></td>
                                </tr>
                                <tr>
                                    <td class="dimBottomLeft">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="dimBottomCenter"></td>
                                    <td class="dimBottomRight">
                                        <div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
  
    <table cellpadding="0" cellspacing="0" border='0' width="100%">
        <tr>
            <td style="background: #f0f1f2; font-size: 16px; font-weight: bold; padding: 5px 10px; border: 1px #a4a2a3 solid; border-right: none;"
                valign="middle">
                <asp:Label ID="lblSurveyTitle" runat="Server"></asp:Label>
            </td>
            <!-- +++++++ buttons reversed below because they float from right ++++++++++++-->
            <td align='right' style="background: #f0f1f2; border: 1px #a4a2a3 solid; border-left: none;"
                valign="bottom" class="survRepTabs">
                <asp:LinkButton ID="lnkToRespondents" runat="Server" Text="<span>View Respondents</span>"
                    CausesValidation="false" Style="float: right; width: 125px;" OnClick="lnkToRespondents_Click"></asp:LinkButton>
                <asp:LinkButton ID="lnkSurveyResults" runat="Server" Text="<span>Survey Results</span>"
                    CausesValidation="false" Style="float: right; width: 100px;" OnClick="lnkSurveyResults_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td width="50%" valign="top" class="greySidesB" style="border-right: none; padding: 10px 0;">
                <table cellpadding="2" cellspacing="0" border='0' width="100%">
                    <tr>
                        <td width="30%" align='right' class="formLabel">Respondents :&nbsp;
                        </td>
                        <td class="dataTwo" width="30%">
                            <asp:Label ID="lblTotalRespondents" runat="Server"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="formLabel" width="40%" align='right' valign="middle" style="padding-right: 50px">PDF Report&nbsp;:&nbsp;
                        <asp:ImageButton ID="lnkToPDF" runat="Server" ImageUrl="/ecn.images/images/icon-pdf.gif"
                            CausesValidation="false" OnClick="lnkToPDF_Click"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="formLabel">Completed :&nbsp;
                        </td>
                        <td class="dataTwo">
                            <asp:Label ID="lblTotalCompleted" runat="Server"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="formLabel" align='right' valign="middle" style="padding-right: 50px">Excel Report&nbsp;:&nbsp;
                        <asp:ImageButton ID="lnktoExl" runat="Server" ImageUrl="/ecn.images/images/icon-xls.gif"
                            CausesValidation="false" OnClick="lnktoExl_Click"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="formLabel" valign="top">Incomplete :&nbsp;
                        </td>
                        <td class="dataTwo" valign="top">
                            <asp:Label ID="lblTotalAbondoned" runat="Server"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="formLabel">Filter Count :&nbsp;
                        </td>
                        <td class="dataTwo">
                            <asp:Label ID="lblFilterCount" runat="Server"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="formLabel">Calculate % using :&nbsp;
                        </td>
                        <td class="dataTwo" colspan="2">
                            <asp:RadioButtonList ID="rbpercentusing" runat="server" class="formLabel" RepeatDirection="horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rbpercentusing_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="Total Respondents" Selected="true" />
                                <asp:ListItem Value="2" Text="Total Respondents/Question" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="50%" valign="top" align="center" class="greySidesB">
                <asp:DataGrid ID="dgFilter" runat="Server" CssClass="girdWizardNoBorder" AutoGenerateColumns="False"
                    AllowSorting="True" HorizontalAlign="Center" Width="100%" Style="border-collapse: separate;"
                    border='0' OnItemCommand="dgFilter_ItemCommand">
                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                    <AlternatingItemStyle CssClass="gridaltrowWizard" />
                    <Columns>
                        <asp:BoundColumn DataField="Question" HeaderText="Question" HeaderStyle-Width="15%" ItemStyle-Width="15%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Filter" HeaderText="Filter" HeaderStyle-Width="60%" ItemStyle-Width="60%"></asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="Question" HeaderText="Remove All Filters" HeaderStyle-Width="30%" HeaderStyle-CssClass="label10"
                            ItemStyle-Width="30%" ItemStyle-CssClass="label10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="Server" Text='Remove Filter' CommandName="remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <asp:PlaceHolder ID="phResults" runat="Server" Visible="false">
            <tr>
                <td class="tableHeader" colspan="2">
                    <asp:DataGrid ID="dgQuestions" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False"
                        AllowSorting="True" HorizontalAlign="Center" Width="100%" DataKeyField="QuestionID">
                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <AlternatingItemStyle HorizontalAlign="Center" CssClass="gridaltrowWizard" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Q. #" ItemStyle-Width="5%" ItemStyle-CssClass="label10" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <a style="font-weight: normal; text-decoration: none" href='#<%#DataBinder.Eval(Container.DataItem, "QuestionID")%>'>Q.
										<%#DataBinder.Eval(Container.DataItem, "number")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Question Index" ItemStyle-Width="82%" ItemStyle-CssClass="label10" HeaderStyle-HorizontalAlign="left"
                                ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <a style="font-weight: normal; text-decoration: none" href='#<%#DataBinder.Eval(Container.DataItem, "QuestionID")%>'>
                                        <%#DataBinder.Eval(Container.DataItem, "QuestionText")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Total Respondents" ItemStyle-Width="13%" ItemStyle-CssClass="label10"
                                HeaderStyle-HorizontalAlign='right' ItemStyle-HorizontalAlign='right'>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuestionTotal" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "TotalCount") %>' runat="server"></asp:Label>
                                    <a style="font-weight: normal; text-decoration: none" href='#<%#DataBinder.Eval(Container.DataItem, "QuestionID")%>'>
                                        <%#DataBinder.Eval(Container.DataItem, "TotalCount")%>
                                    </a>&nbsp;
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid><br />
                    <br />
                </td>
            </tr>
            <script type="text/javascript">
                if (document.getElementById('lnkToRespondents') && document.getElementById('lnkSurveyResults')) {
                    document.getElementById('lnkToRespondents').className = "";
                    document.getElementById('lnkSurveyResults').className = "selected";
                }
            </script>
            <tr>
                <td class="tableHeader" colspan="2" align="center" width="100%">
                    <asp:Repeater ID="repQuestions" runat="Server" OnItemDataBound="repQuestions_ItemDataBound" OnItemCommand="repQuestions_ItemCommand">
                        <ItemTemplate>
                            <a name='<%#DataBinder.Eval(Container.DataItem, "QuestionID")%>'></a>
                            <asp:Label ID="lblQuestiontype" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "format") %>'>
                            </asp:Label>
                            <asp:Label ID="lblQuestionID" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>
                            </asp:Label>
                            <table cellpadding="0" cellspacing="0" width="100%" align="center">
                                <tr>
                                    <td align="left" width="50%" style="padding: 0 5px;">
                                        <table cellpadding="0" cellspacing="0" width="100%" class="formLabel">
                                            <tr>
                                                <td class="bubble" valign="bottom" align="center" width="48" style="padding-bottom: 5px;">
                                                    <div></div>
                                                    <strong>Q&nbsp;<%# DataBinder.Eval(Container.DataItem, "number") %></strong></td>
                                                <td valign="bottom" style="padding: 0 0 5px 10px;" class="headingTwo"><%# DataBinder.Eval(Container.DataItem, "QuestionText") %>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr bgcolor="#ffffff">
                                    <td align="center" style="padding: 0 5px">
                                        <asp:PlaceHolder ID="plotherformat" runat="Server">
                                            <table border='0' cellpadding="1" cellspacing="0" width="100%" align="center" style="border-top: 1px #A4A2A3 solid; border-left: 1px #A4A2A3 solid; border-right: 1px #A4A2A3 solid">
                                                <tr>
                                                    <td width="4%" class="formLabel surveyQuestionNumber" style="padding: 0; margin: 0;">
                                                        <div style="width: 46px; text-align: center; padding: 0; margin: 0;">Filter</div>
                                                    </td>
                                                    <td width="22%" class="formLabel surveyQuestionNumber">&nbsp;Answer</td>
                                                    <td width="7%" class="formLabel surveyQuestionNumber">&nbsp;</td>
                                                    <td width="55%" align="left" class="formLabel surveyQuestionNumber">&nbsp;Responses 
														by %</td>
                                                    <td width="14%" align='right' class="formLabel surveyQuestionNumber" style="border-right: none;">Responses&nbsp;</td>
                                                </tr>
                                                <asp:Repeater ID="repAnswers" runat="Server" OnItemDataBound="repAnswers_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="surveyQuestionHeading">
                                                            <td height="22" style="border-bottom: 1px #a4a2a3 solid; border-right: 1px #a4a2a3 solid;"
                                                                align="center" valign="middle" class="label10">
                                                                <asp:Label ID="lblQID" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblOID" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OptionID") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblHasOtherResponse" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "HasOtherResponse") %>'>
                                                                </asp:Label>

                                                                <asp:PlaceHolder ID="plCheckbox" runat="Server"></asp:PlaceHolder>
                                                            </td>
                                                            <td height="22" style="border-bottom: 1px #a4a2a3 solid; border-right: 1px #a4a2a3 solid;"
                                                                align="left" valign="middle" class="label10">&nbsp;
																<asp:Label ID="lbloptionvalue" runat="Server" Text='<%# DataBinder.Eval(Container.DataItem, "OptionValue") %>'>
                                                                </asp:Label>&nbsp;
                                                            </td>
                                                            <td style="border-bottom: 1px #a4a2a3 solid; border-right: 1px #a4a2a3 solid;" class="label10"
                                                                align='right' valign="middle"><%# DataBinder.Eval(Container.DataItem, "ratio") %>&nbsp;%&nbsp;&nbsp;</td>
                                                            <td style="border-bottom: 1px #a4a2a3 solid; border-right: 1px #a4a2a3 solid;" class="label10"
                                                                align="left" valign="middle">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="<%# DataBinder.Eval(Container.DataItem, "ratio") %>%">
                                                                            <asp:Label ID="lblRatio" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ratio") %>'>
                                                                            </asp:Label>
                                                                            <asp:PlaceHolder ID="plbar" runat="Server"></asp:PlaceHolder>
                                                                        </td>
                                                                        <td width="100%">&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="border-bottom: 1px #a4a2a3 solid;" class="label10" align='right' valign="middle"><%# DataBinder.Eval(Container.DataItem, "responsecount") %>&nbsp;</td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="plgridformat" runat="Server">
                                            <asp:DataGrid ID="dgGridResponse" runat="Server" AutoGenerateColumns="true" CssClass="gridWizard"
                                                Width="100%" OnItemDataBound="dgGridResponse_ItemDataBound">
                                                <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <AlternatingItemStyle HorizontalAlign="Center" CssClass="gridaltrowWizard" />
                                            </asp:DataGrid>
                                        </asp:PlaceHolder>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' style="padding: 0 5px;">
                                        <div style="background: #e7e8e9;">
                                            <table cellpadding="0" cellspacing="0" width="100%" class="label10">
                                                <tr>
                                                    <td align="left" style="padding: 5px;" class="ltButtonSmall">
                                                        <asp:LinkButton ID="lbFilter" runat="Server" Text='<span>Add Filter</span>' CommandName="filter"></asp:LinkButton>
                                                    </td>
                                                    <td align='right' style="padding: 5px;">Total Responses :
														<%# DataBinder.Eval(Container.DataItem, "Totalcount") %>
														/
														<asp:Label ID="lblTotalRespondentsCount" runat="Server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' class="backToTop" style="padding-right: 5px;">
                                        <a href='#top'>Back To Top</a>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <br />
                        </SeparatorTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phRespondent" runat="Server" Visible="false">
            <tr>
                <td class="tableHeader" colspan="2" align='right'>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" Width="220px" CssClass="roundbutton" runat="server" Text="Export Survey Responses" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnExportToGroup" Width="220px" CssClass="roundbutton" runat="server" OnClick="btnExportToGroup_Click" Text ="Export Respondents to a Group" />
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td>
                    <div class="descWiz" align="right">
                        <asp:Label ID="lblExportToGroupMessage" runat="Server" Visible="false" ForeColor="Red" Font-Size="x-small"></asp:Label>
                    </div>
                </td>
            </tr>
            <script type="text/javascript">
                if (document.getElementById('lnkToRespondents') && document.getElementById('lnkSurveyResults')) {
                    document.getElementById('lnkToRespondents').className = "selected";
                    document.getElementById('lnkSurveyResults').className = "";
                }
            </script>
            <tr>
                <td class="tableHeader" colspan="2">
                    <asp:DataGrid ID="dgRespondent" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False"
                        AllowSorting="True" HorizontalAlign="Center" Width="100%" OnSortCommand="dgRespondent_SortCommand">
                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        <AlternatingItemStyle CssClass="gridaltrowWizard" />
                        <Columns>
                            <asp:BoundColumn DataField="EmailAddress" SortExpression="EmailAddress" HeaderText="Email Address"
                                ItemStyle-Width="23%"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-Width="10%" ItemStyle-CssClass="label10" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href='ViewResponse.aspx?surveyID=<%#DataBinder.Eval(Container.DataItem, "SurveyID")%>&EmailID=<%#DataBinder.Eval(Container.DataItem, "EmailID")%>'>View Survey</a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="RespondentPager" runat="Server" Width="100%" ControlToPage="dgRespondent" PageSize="25" OnIndexChanged="RespondentPager_IndexChanged">
                        <PagerStyle CssClass="gridpager"></PagerStyle>
                    </AU:PagerBuilder>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
    <br />

    <script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/browser.js"></script>

    <script type="text/javascript">    // -->
        if (BrowserDetect.browser == "Safari") {
            if (document.getElementById("txtGroupName")) {
                document.getElementById("txtGroupName").style.position = "static";
            }

            if (document.getElementById("drpGroup")) {
                document.getElementById("drpGroup").style.position = "static";
            }


        }

    </script>
</asp:Content>
