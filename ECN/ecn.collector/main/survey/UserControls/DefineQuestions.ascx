<%@ Control Language="c#" Inherits="ecn.collector.main.survey.UserControls.DefineQuestions"
    CodeBehind="DefineQuestions.ascx.cs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<style>
    #dimmer
    {
        position: fixed;
        top: 0px;
        display: none;
        left: 0;
        width: 100%;
        z-index: 10;
    }

    * html #dimmer
    {
        position: absolute;
    }

    html >/**/ body #dimmer
    {
        background: url(/ecn.images/images/gray.png) top left repeat;
    }

    * #dimmerIE
    {
        position: absolute;
        top: 0;
        display: none;
        left: 0px;
        width: 100%;
        z-index: 2;
        _background-image: none;
        _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=scale src='/ecn.images/images/gray.png');
    }

    #dimmerContainer
    {
        position: absolute;
        top: 0;
        display: none;
        left: 0px;
        font-family: Arial;
        font-weight: bold;
        width: 100%;
        z-index: 100;
    }

    * html #dimmerContainer
    {
        position: absolute;
    }



    div.dimming
    {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        font-style: normal;
        position: relative;
    }

    #divQuestion, #divPage, #divReOrder, #divBranch, #divAlert
    {
        display: none;
    }

    .descWiz
    {
        font-family: "Arial";
        font-size: 11px;
        padding: 6px;
    }

    .descWizsmall
    {
        font-family: "Arial";
        font-size: 11px;
        padding: 6px;
    }

    .dimmerInner
    {
        max-height: 300px;
        overflow: auto;
    }

    * html .dimmerInner
    {
        height: 300px;
        overflow: auto;
    }
    	.reLayoutWrapper{
		
		width:100% !important;
	}

</style>
<!--[if gte IE 5.5]>
		<style type="text/css">
div#dimmerContainer {
left: expression( ( 0 + ( ignoreMe2 = document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ) ) + 'px' );
top: expression( ( 0 + ( ignoreMe = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ) ) + 'px' );
}

		</style>
		<![endif]-->

<script type="text/javascript">

    function ModeChange(editor, args)
    {
        editor.repaint();
    }

    function repaint(editor, args) {
        editor.repaint();
        editor.set_mode(2);
        editor.set_mode(1);
    }

    function setDimmerSize() {
        var winW = 940, winH = 460;

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

        if (winH < 960) {
            var height = 960 - winH;
            height = height / 2;
            Dimmer.style.top = height + "px";
        }
        if (winH > 960) {

            var height = winH - 960;
            height = height / 2;
            Dimmer.style.top = "-" + height + "px";
        }

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
            //scroll(0,0);
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
        if (id.indexOf("SurveyWizard_") != -1) {
            id = 'ctl00_ContentPlaceHolder1_' + id;
        }
        if (document.all && !document.getElementById)
            obj = eval('document.all.' + id);
        else if (document.layers)
            obj = eval('document.' + id);
        else if (document.getElementById)
            obj = document.getElementById(id);

        return obj;
    }


    String.prototype.trim = function () { return this.replace(/^\s+|\s+$/, ''); };

    function qValidate() {
        var selectedFormat = "";
        var returnvalue = true;

        if (getobj("SurveyWizard_rbQuestionFormat_0").checked)
            selectedFormat = "C";
        else if (getobj("SurveyWizard_rbQuestionFormat_1").checked)
            selectedFormat = "D";
        else if (getobj("SurveyWizard_rbQuestionFormat_2").checked)
            selectedFormat = "R";
        else if (getobj("SurveyWizard_rbQuestionFormat_3").checked)
            selectedFormat = "G";
        else if (getobj("SurveyWizard_rbQuestionFormat_4").checked)
            selectedFormat = "T";
        else {
            getobj("spnqformat").style.display = "block";
            returnvalue = false;
        }

        if (selectedFormat != "")
            getobj("spnqformat").style.display = "none";
        var content = CKEDITOR.instances['<%= txtQuestion.ClientID %>'].getData();

        if (content.trim() == "") {
            getobj("spnquestion").style.display = "block";
            returnvalue = false;
        }
        else {
            getobj("spnquestion").style.display = "none"
        }

        if (selectedFormat == 'C' || selectedFormat == 'D' || selectedFormat == 'R' || selectedFormat == 'G') {
            if (selectedFormat == 'G') {
                if (getobj("SurveyWizard_txtOptions").value.trim() == "") {
                    getobj("spnqoption").style.display = "block";
                    returnvalue = false;
                }
                else {
                    getobj("spnqoption").style.display = "none";
                }

                if (getobj("SurveyWizard_txtGridRow").value.trim() == "") {
                    getobj("spnqstatement").style.display = "block";
                    returnvalue = false;
                }
                else {
                    getobj("spnqstatement").style.display = "none";
                }

                if (getobj("SurveyWizard_rbGridType_0").checked || getobj("SurveyWizard_rbGridType_1").checked) {
                    getobj("spngformat").style.display = "none";
                }
                else {
                    getobj("spngformat").style.display = "block";
                    returnvalue = false;
                }
            }
        }
        return returnvalue;

    }

    function showQ() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divQuestion").style.display = 'block';
        setDimmerSize();
        hideBody();
        QF_onchecked();
    }
    function hideQ() {
        getobj("divQuestion").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function showP() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divPage").style.display = 'block';
        setDimmerSize();
        hideBody();
        return false;
    }
    function hideP() {
        getobj("divPage").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function showB() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divBranch").style.display = 'block';
        setDimmerSize();
        hideBody();
        return false;
    }
    function hideB() {
        getobj("divBranch").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function showR() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divReOrder").style.display = 'block';
        setDimmerSize();
        hideBody();
        return false;
    }
    function hideR() {
        getobj("divReOrder").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function showAlertR() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divAlert").style.display = 'block';
        getobj("reorderPageTitle").style.display = 'inline';
        getobj("reorderPageConfirm").style.display = 'block';
        setDimmerSize();
        hideBody();
        return false;
    }
    function hideAlertR() {

        getobj("reorderPageConfirm").style.display = 'none';
        getobj("reorderPageTitle").style.display = 'none';
        getobj("divAlert").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function showAlertD() {
        getobj("dimmer").style.display = 'block';
        getobj("dimmerContainer").style.display = 'block';
        getobj("divAlert").style.display = 'block';
        getobj("deleteQuestionTitle").style.display = 'inline';
        getobj("deleteQuestionConfirm").style.display = 'block';
        setDimmerSize();
        hideBody();
        return false;
    }
    function hideAlertD() {

        getobj("deleteQuestionConfirm").style.display = 'none';
        getobj("deleteQuestionTitle").style.display = 'none';
        getobj("divAlert").style.display = 'none';
        getobj("dimmerContainer").style.display = 'none';
        getobj("dimmer").style.display = 'none';
        showBody();
    }

    function QF_onchecked() {
        var selectedFormat = "";

        if (getobj("SurveyWizard_rbQuestionFormat_0").checked)
            selectedFormat = "C";
        else if (getobj("SurveyWizard_rbQuestionFormat_1").checked)
            selectedFormat = "D";
        else if (getobj("SurveyWizard_rbQuestionFormat_2").checked)
            selectedFormat = "R";
        else if (getobj("SurveyWizard_rbQuestionFormat_3").checked)
            selectedFormat = "G";
        else if (getobj("SurveyWizard_rbQuestionFormat_4").checked)
            selectedFormat = "T";

        if (selectedFormat == 'C' || selectedFormat == 'D' || selectedFormat == 'R') {
            getobj("DivCDRG").style.display = 'block';
            getobj("DivCDRG2").style.display = 'block';
            getobj("DivText").style.display = 'none';
            getobj("DivRequired").style.display = 'block';
        }
        else if (selectedFormat == 'T') {
            getobj("DivCDRG").style.display = 'none';
            getobj("DivCDRG2").style.display = 'none';
            getobj("DivText").style.display = 'block';
            getobj("DivRequired").style.display = 'block';
        }

        if (selectedFormat == 'G') {
            getobj("DivRequired").style.display = 'none';
            getobj("DivGridRequired").style.display = 'block';
            getobj("DivCDRG").style.display = 'none';
            getobj("DivGrid").style.display = 'block';
            getobj("DivCDRG2").style.display = 'none';

            getobj("imgGridCol").style.display = 'block';
            getobj("imgGridRow").style.display = 'block';
        }
        else {
            getobj("DivGridRequired").style.display = 'none';
            getobj("DivGrid").style.display = 'none';
            getobj("imgGridCol").style.display = 'none';
            getobj("imgGridRow").style.display = 'none';

        }

    }
</script>


<asp:Label ID="lblpageID" runat="server" Visible="False"></asp:Label>
<asp:Label ID="lblquestionID" runat="server" Visible="False"></asp:Label>

<script language='javascript'>onresize = function () { setDimmerSize() };</script>
<div align="center">
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="98%">
        <tr>
            <td width="187" valign="top">
                <table cellpadding="0" cellspacing="0" width="100%" style="margin-left: 3px;">
                    <tr height="20" width="60%">
                        <td class="gradientTwo formLabel" style="border-right: none;">&nbsp;<span>
                            <asp:CheckBox ID="chkShowAllPages" runat="server" Text="<span style='position:relative;top:-2px;'>Show All Pages</span>"
                                OnCheckedChanged="chkShowAllPages_oncheckchanged" CssClass="formLabel" AutoPostBack="True"></asp:CheckBox>
                        </span>
                        </td>
                        <td class="gradientTwo addPage" style="border-left: none;">
                            <div style="height: 20px; min-width: 1px;">
                                <asp:LinkButton ID="lbpAdd" runat="server" Text='Add Page'></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="greySidesLtB" style="background: #fff;" colspan="2">&nbsp;<br>
                            <asp:DataList ID="dlPages" runat="server" Width="100%" DataKeyField="PageID" OnItemCommand="dlPages_itemcommand"
                                OnItemDataBound="dlPages_itemdatabound">
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center" bgcolor="<%# DataBinder.Eval(Container.DataItem, "bgcolor") %>"
                                        style="padding: 5px 0;">
                                        <tr>
                                            <td align="center" class="label10">
                                                <span style="position: relative; top: -2px;">
                                                    <asp:ImageButton ImageUrl="/ecn.images/images/page.gif" AlternateText='<%# DataBinder.Eval(Container.DataItem, "PageHeader") %>'
                                                        runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'
                                                        ID="Imagebutton1"></asp:ImageButton></span>
                                                <br />
                                                P.<%# DataBinder.Eval(Container.DataItem, "number") %>
                                            </td>
                                            <td valign="top" class="label10 pagesNav">
                                                <div class="container">
                                                    <asp:LinkButton ID="lbpedit" CssClass="survPageCol" runat="server" Text='EDIT' CommandName="EDIT"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'>													</asp:LinkButton><br />
                                                    <asp:LinkButton ID="lbpDelete" CssClass="survPageCol" runat="server" Text='DELETE'
                                                        CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'>													</asp:LinkButton><br />
                                                    <asp:LinkButton ID="lbpReorder" CssClass="survPageCol" runat="server" Text='RE-ORDER'
                                                        CommandName="REORDER" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'>													</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
                <br>
            </td>
            <td valign="top">
                <asp:Repeater ID="repPages" runat="server" OnItemCommand="repPages_ItemCommand" OnItemDataBound="repPages_ItemDataBound">
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" width="98%" align="center" style="margin-left: 12px;">
                            <tr class='surveyPageHeadingDef'>
                                <td align="left" width="65%" class="formLabel left">
                                    <div>
                                        <strong>Pg&nbsp;<%# DataBinder.Eval(Container.DataItem, "number") %></strong> &nbsp;&nbsp;|&nbsp;&nbsp;<span
                                            style="color: #000;"><%# DataBinder.Eval(Container.DataItem, "PageHeader") %></span>
                                    </div>
                                </td>
                                <td class="right" width="35%" align="right">
                                    <div class="right">
                                        <asp:LinkButton ID="lbqAdd" runat="server" Text='<span>+&nbsp;Add Question</span>'
                                            CommandName="add" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="left">
	                                    <asp:LinkButton ID="lbqBranch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PageCount").Equals(1) ? "hide" : DataBinder.Eval(Container.DataItem, "IsLastPage").Equals(false) ? DataBinder.Eval(Container.DataItem, "branchingExists").Equals(true) ? "+&nbsp;Edit Branching":"+&nbsp;Add Branching" : "hide" %>'
                                            CommandName="branch" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageID") %>'>
                                        </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr bgcolor="#ffffff">
                                <td align="center" style="padding: 5px" colspan="2" class='surveyPageBottomDef'>
                                    <p class="formLabel" style="text-align: left; margin-left: 7px;">
                                        <span style="color: #999;">Page Description:</span>
                                        <%# DataBinder.Eval(Container.DataItem, "PageDesc") %>
                                    </p>
                                    <asp:Repeater ID="rpQuestionsGrid" runat="server" OnItemCommand="rpQuestionsGrid_itemcommand"
                                        OnItemDataBound="rpQuestionsGrid_ItemDataBound" DataSource='<%# LoadQuestionGrid(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PageID"))) %>'>
                                        <ItemTemplate>
                                            <table border="0" bordercolor="gray" cellpadding="1" cellspacing="0" width="100%"
                                                align="center" style="border: 1px #A4A2A3 solid;">
                                                <tr bgcolor="#e5e5e5" class="surveyQuestionHeading">
                                                    <td width="10%" height="22" style="border-bottom: 1px #a4a2a3 solid;" class="headingTwo surveyQuestionNumber"
                                                        align="center">
                                                        <span style="font-size: 12px;">
                                                            <%# DataBinder.Eval(Container.DataItem, "number") %>
                                                            .</span></td>
                                                    <td width="100%" class="headingTwo bottomBorder" align="left">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td class="headingTwo">
                                                                    <p class="highLightTwo" style="margin: 0 0 0 2px; padding: 0; text-transform: capitalize; font-weight: normal;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "format") %>
                                                                    </p>
                                                                </td>
                                                                <td align="right" class="headingTwo" style="padding-right: 10px;">
                                                                    <%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "required"))?"<span class='required'>(Required)</span>":"") %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="70" class="label10 bottomBorder" align="center" valign="bottom">
                                                        <div style="height: 20px; min-width: 1px;">
                                                            <asp:LinkButton runat="server" CssClass="headingLinkOne surveyQuestionsEdit" Text='<img src="/ecn.images/images/icon-edits1.gif" border="0" style="position:relative;top:2px;"/><span style="position:relative;top:-3px;left:3px;">Edit</span>'
                                                                ID="lbqEdit" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                    <td width="85" class="label10 bottomBorder" align="center" valign="bottom">
                                                        <div style="height: 20px; min-width: 1px;">
                                                            <asp:LinkButton ID="lbqReOrder" runat="server" CssClass="headingLinkOne surveyQuestionsReorder"
                                                                Text='<img src="/ecn.images/images/re-order.gif" border="0" style="position:relative;top:2px;"/><span style="position:relative;top:-2px;left:3px;">Re-Order</span>'
                                                                CommandName="reorder" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                    <td width="75" class="label10 bottomBorder" align="center" valign="bottom">
                                                        <div style="height: 20px; min-width: 1px;">
                                                            <asp:LinkButton ID="lbqDelete" CssClass="headingLinkOne surveyQuestionsDelete" runat="server"
                                                                Text='<img src="/ecn.images/images/delete.gif" border="0" style="position:relative;top:2px;" /><span style="position:relative;top:-3px;left:3px;">Delete</span>'
                                                                CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr bgcolor="#ffffff">
                                                    <td class="headingTwo qAndA" align="center">
                                                        <span style="color: #d31a1a">Q.</span></td>
                                                    <td class="headingTwo" align="left" colspan="4">&nbsp;<%# DataBinder.Eval(Container.DataItem, "QuestionText") %></td>
                                                </tr>
                                                <%# (DataBinder.Eval(Container.DataItem, "format").ToString() == "grid"?"<tr bgcolor='#f4f4f4'><td class='headingTwo qAndA' align=center><span style='color:#299843;'>Row</span></td><td class='label10' align=left colspan='4'>" +  DataBinder.Eval(Container.DataItem, "rows") + "&nbsp;</td></TR>":"")%>
                                                <%# (DataBinder.Eval(Container.DataItem, "format").ToString() != "textbox"?"<tr bgcolor='#f4f4f4'><td class='headingTwo qAndA' align=center><span style='color:#299843;'>" + (DataBinder.Eval(Container.DataItem, "format").ToString() == "grid"?"Col.":"A.") + "</span></td><td class='label10' align=left colspan='4'>&nbsp;" +  DataBinder.Eval(Container.DataItem, "options") + "&nbsp;</td></TR>":"")%>
                                                <%# (DataBinder.Eval(Container.DataItem, "BranchingExists").ToString() == "1"?"<tr bgcolor='#cccccc'><td class='headingTwo qAndA' align=center><span style='color:#299843;'>B.</span></td><td class='label10' align=left colspan='4'>&nbsp;Active Branching&nbsp;</td></TR>":"")%>
                                            </table>
                                            <br>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                        <p class="backToTop">
                            <a href="#">Back to Top</a>
                        </p>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br>
                    </SeparatorTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer") {
        document.write('<div id="dimmerIE"></div>');
    }
</script>

<div id="dimmer">
    <div id="dimmerContainer">
        <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="position: relative;">
            <tr>
                <td align="center" valign="middle" id="containerCell">
                    <div class="dimming" id="divQuestion" style="width: 900px; height: 400px; margin-top:auto;margin-bottom:auto;">
                        <table border="0" cellpadding="0" cellspacing="0" width="900">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Add / Edit Questions</span></td>
                                <td class="dimTopRight">
                                    <asp:ImageButton ID="btnQuestionCancel" ImageUrl="/ecn.images/images/divs_05.gif"
                                        AlternateText="Close" runat="server"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft"></td>
                                <td class="dimMiddleCenter" align="right">
                                    <table border="0" cellpadding="0" cellspacing="0" style="height:425px;" width="100%">
                                        <tr>
                                            <td bgcolor="#ffffff" height="300" valign="top" class="dimmerLabel">
                                                <p style="margin-top: 5px;">
                                                    Page:
                                                    <asp:Label ID="lblQPageno" runat="server"></asp:Label>&nbsp;
                                                </p>
                                                <asp:PlaceHolder ID="plQuestionno" runat="server">
                                                    <p>
                                                        Question:&nbsp;<asp:Label ID="lblQno" runat="server"></asp:Label>&nbsp;
                                                    </p>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="plQposition" runat="server">
                                                    <div class="descWiz" align="left">
                                                        Position:<br>
                                                        <asp:DropDownList ID="drpQPosition" runat="server" CssClass="dimmerLabel">
                                                            <asp:ListItem Value="b">Before</asp:ListItem>
                                                            <asp:ListItem Value="a">After</asp:ListItem>
                                                        </asp:DropDownList><br />
                                                        <asp:DropDownList ID="drpQuestion" runat="server" CssClass="dimmerLabel" Style="margin-top: 5px;">
                                                        </asp:DropDownList>
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div style="padding-right: 15px;" class="dimmerLabel">
                                                    <p style="border-top: 1px #A7A6AA solid;">
                                                        <br />
                                                        * Question Format:&nbsp;<span id="spnqformat" style="display: none; color: red;">(required)</span><br>
                                                        <asp:RadioButtonList RepeatDirection="Horizontal" ID="rbQuestionFormat" runat="server"
                                                            CssClass="dimmerLabel" onclick="javascript:QF_onchecked();">
                                                            <asp:ListItem Value="checkbox">Check Box</asp:ListItem>
                                                            <asp:ListItem Value="dropdown">Drop Down</asp:ListItem>
                                                            <asp:ListItem Value="radio">Radio Button</asp:ListItem>
                                                            <asp:ListItem Value="grid">Grid</asp:ListItem>
                                                            <asp:ListItem Value="textbox">Text</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </p>
                                                </div>
                                                <div class="descWiz" style="height:300px;" align="left">
                                                    Question:&nbsp;<span id="spnquestion" style="display: none; color: red; font-size: 10px">(required)</span><br>
                                                    <%--<telerik:RadEditor ID="txtQuestion" runat="server" OnClientModeChange="ModeChange" ContentAreaMode="Div" OnClientLoad="repaint()" Height="95%" Width="95%" ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools_Simple.xml" />--%>
                                                    <CKEditor:CKEditorControl ID="txtQuestion" runat="server" Toolbar="Source
Bold|Italic|Underline|-
|Outdent|Indent
/
Styles|Font|FontSize|TextColor|BGColor|Image|"  BasePath="/ecn.editor/ckeditor" ToolbarCanCollapse="false" Height="95%" Width="95%" />

                                                </div>
                                                <div class="descWiz" id="DivRequired" style="display: none;" align="left">
                                                    Required:<br>
                                                    <asp:RadioButtonList ID="rbRequired" runat="server" CssClass="dimmerLabel" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class="descWiz" id="DivGridRequired" style="display: none;" align="left">
                                                    Grid Validation:<br>
                                                    <asp:RadioButtonList ID="rbGridRequired" runat="server" CssClass="dimmerLabel" RepeatDirection="vertical">
                                                        <asp:ListItem Value="0">Not a required question</asp:ListItem>
                                                        <asp:ListItem Value="1">One response is required</asp:ListItem>
                                                        <asp:ListItem Value="2">At least One response is required</asp:ListItem>
                                                        <asp:ListItem Value="3">At least One response per line is required</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </td>
                                            <td bgcolor="#f5f5f5" align="center" style="width:350px;" valign="top">
                                                <div style="width: 320px">
                                                    <div style="padding-left: 10px;">
                                                        <div id="DivText" style="display: none;">
                                                            <div style="padding: 0" class="dimmerLabel">
                                                                Maximum Characters:&nbsp;<asp:TextBox ID="txtMaxChars" Visible="True" runat="server"
                                                                    CssClass="dimmerLabel" Width="50" Style="width: 50px;" Text="499"></asp:TextBox>
                                                               <ajaxToolkit:FilteredTextBoxExtender ID="ftbeMaxChars" runat="server" TargetControlID="txtMaxChars" FilterType="Numbers" />
                                                                
                                                            </div>
                                                        </div>
                                                        <div id="DivCDRG" style="display: none; margin-top: 10px;">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <asp:ValidationSummary ID="valSum" runat="server" Width="300px" Font-Names="Arial"
                                                                        Font-Size="8" ValidationGroup="valCDRG" />
                                                                    <table class="grid" cellspacing="0" cellpadding="4" rules="cols" border="1" id="ctl00_Content_gvResponseOption"
                                                                        style="color: Black; width: 300px; border-collapse: collapse;">
                                                                        <tr class="gridheader">
                                                                            <th align="left" scope="col" style="width: 70%;">Response option</th>
                                                                            <th align="left" scope="col" style="width: 20%;">Score</th>
                                                                            <th align="center" valign="middle" scope="col" width="10%">Add</th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" valign="middle" style="width: 70%;">
                                                                                <asp:TextBox ID="txtOption" runat="server" Width="150" Font-Size="XX-Small"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                    ErrorMessage="Response option is required." ControlToValidate="txtOption" ID="rfv3" ValidationGroup="valCDRG"
                                                                                    runat="server">*</asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td align="left" valign="middle" style="width: 20%;">
                                                                                <asp:TextBox ID="txtScore" runat="server" Width="50" Font-Size="XX-Small"></asp:TextBox>&nbsp;&nbsp;<asp:RegularExpressionValidator
                                                                                    ID="rfvtxtScore" runat="server" Font-Bold="True" Font-Italic="True" ErrorMessage="Only numbers allowed." ValidationGroup="valCDRG"
                                                                                    Font-Size="XX-Small" ControlToValidate="txtScore" ValidationExpression="^\d*\.?\d*$"
                                                                                    Font-Overline="False">*</asp:RegularExpressionValidator>
                                                                            </td>
                                                                            <td align="center" valign="middle">
                                                                                <asp:ImageButton ID="imgAdd" ImageUrl="/ecn.images/images/icon-add.gif" runat="server" ValidationGroup="valCDRG"
                                                                                    OnClick="AddOptions" /></td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                    <div style="overflow: auto; width: 320px; height: 150px;">
                                                                        <asp:GridView DataKeyNames="optionID" ID="gvResponseOption" ShowFooter="False" runat="server"
                                                                            AutoGenerateColumns="False" Width="300px" CssClass="grid" EmptyDataText="No Records Found."
                                                                            PageSize="20" CellPadding="4" ForeColor="Black" GridLines="Vertical" OnRowEditing="gvResponseOption_RowEditing"
                                                                            OnRowCancelingEdit="gvResponseOption_RowCancelingEdit" OnRowDeleting="gvResponseOption_RowDeleting"
                                                                            OnRowUpdating="gvResponseOption_RowUpdating">

                                                                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right"></PagerStyle>
                                                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Response option">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOption" runat="server" Text='<%# Eval("text") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:TextBox ID="txtOptionE" runat="server" Width="150" Text='<%# Eval("text") %>'
                                                                                            Font-Size="XX-Small"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ErrorMessage="Response option  is required."
                                                                                                ControlToValidate="txtOptionE" ID="rfv3E" runat="server" ValidationGroup="valCDRG">*</asp:RequiredFieldValidator>
                                                                                    </EditItemTemplate>
                                                                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="top" />
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Score">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblScore" runat="server" Text='<%# Eval("score") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:TextBox ID="txtScoreE" runat="server" Width="50" Text='<%# Eval("score") %>'
                                                                                            Font-Size="XX-Small"></asp:TextBox>&nbsp;&nbsp;<asp:RegularExpressionValidator ID="rfvtxtScoreE"
                                                                                                runat="server" Font-Bold="True" Font-Italic="True" ErrorMessage="Only numbers allowed."
                                                                                                Font-Size="XX-Small" ControlToValidate="txtScoreE" ValidationExpression="^\d*\.?\d*$" ValidationGroup="valCDRG"
                                                                                                Font-Overline="False">*</asp:RegularExpressionValidator>
                                                                                    </EditItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                                                                </asp:TemplateField>
                                                                                <asp:CommandField HeaderStyle-HorizontalAlign="center" HeaderStyle-VerticalAlign="Middle"
                                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Edit"
                                                                                    ButtonType="image" EditImageUrl="/ecn.images/images/icon-edits1.gif" EditText="edit"
                                                                                    ShowEditButton="true" UpdateImageUrl="/ecn.images/images/ic-save.gif" CancelImageUrl="/ecn.images/images/ic-cancel.gif" />
                                                                                <asp:CommandField HeaderStyle-HorizontalAlign="center" HeaderStyle-VerticalAlign="Middle"
                                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Delete"
                                                                                    ButtonType="image" DeleteImageUrl="/ecn.images/images/icon-delete1.gif" EditText="delete"
                                                                                    ShowDeleteButton="true" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div id="DivGrid" style="margin-top: 5px; display: none;">
                                                            <div align="left">
                                                                <table cellpadding="0" cellspacing="0" width="230" class="dimmerLabel">
                                                                    <tr>
                                                                        <td>Response Options:&nbsp;<span id="spnqoption" style="display: none; color: red; font-size: 10px">(required)</span><br>
                                                                            (one response per line)<br />
                                                                        </td>
                                                                        <td valign="bottom" align="right">
                                                                            <img id="imgGridCol" src="/ecn.images/images/cols.gif" /></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:TextBox ID="txtOptions" Visible="True" runat="server" Width="100%" Height="100"
                                                                    TextMode="Multiline" Style="margin-top: 5px;" CssClass="dimmerLabel dimmerTextbox"></asp:TextBox>
                                                            </div>
                                                            <div align="left">
                                                                <table cellpadding="0" cellspacing="0" width="230" class="dimmerLabel">
                                                                    <tr>
                                                                        <td>Response Statement: <span id="spnqstatement" style="display: none; color: red; font-size: 10px">(required)</span></td>
                                                                        <td valign="bottom" align="right">
                                                                            <img id="imgGridRow" src="/ecn.images/images/rows.gif" /></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:TextBox ID="txtGridRow" Visible="True" runat="server" Width="100%" Height="100"
                                                                    TextMode="Multiline" Style="margin-top: 5px;" CssClass="dimmerLabel dimmerTextbox"></asp:TextBox>
                                                            </div>
                                                            <div class="descWiz" align="left">
                                                                Grid Format:&nbsp;<span id="spngformat" style="display: none; color: red; font-size: 10px">(required)</span><br>
                                                                <asp:RadioButtonList ID="rbGridType" runat="server" CssClass="dimmerLabel" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="r">Radio Button</asp:ListItem>
                                                                    <asp:ListItem Value="c">Check Box</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                        <div id="DivCDRG2" style="display: none;">
                                                            <div class="descWiz" align="left">
                                                                Add optional Text Box:<br>
                                                                <asp:RadioButtonList ID="rbAddTextbox" runat="server" CssClass="dimmerLabel" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#ffffff" valign="top">&nbsp;</td>
                                            <td bgcolor="#f5f5f5" valign="middle">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btnQuestionSave" ImageUrl="/ecn.images/images/btn_finish.gif"
                                                                AlternateText="Save" runat="server" CausesValidation="true" ></asp:ImageButton>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="dimMiddleRightGrey" bgcolor="#ff0000" width="53"></td>
                            </tr>
                            <tr>
                                <td class="dimBottomLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimBottomCenter" align="right">
                                    <div class="dimBottomCenterGrey">
                                    </div>
                                </td>
                                <td class="dimBottomRightGrey">
                                    <div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="dimming" id="divPage" style="width: 300px; height: 200px;">
                        <table width="300" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Add / Edit Pages</span></td>
                                <td class="dimTopRight">
                                    <asp:ImageButton ID="btnPageCancel" ImageUrl="/ecn.images/images/divs_05.gif" AlternateText="Close"
                                        runat="server"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft"></td>
                                <td class="dimMiddleCenter">
                                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr>
                                            <td bgcolor="#ffffff" valign="top">
                                                <table border="0" cellpadding="5" cellspacing="0" width="279">
                                                    <tr>
                                                        <td valign="top">
                                                            <div class="descWiz" align="left">
                                                                Page Header:<br>
                                                                <asp:TextBox ID="txtPageHeader" runat="server" CssClass="dimmerLabel" Style="width: 300px"></asp:TextBox>
                                                            </div>
                                                            <div class="descWiz" align="left">
                                                                Page Description:<br>
                                                                <asp:TextBox ID="txtPageDesc" runat="server" TextMode="MultiLine" Rows="3" Columns="60"
                                                                    CssClass="dimmerLabel" Style="width: 300px"></asp:TextBox>
                                                            </div>
                                                            <asp:PlaceHolder ID="plposition" runat="server">
                                                                <div class="descWiz" align="left">
                                                                    Page Order:<br>
                                                                    <asp:DropDownList ID="drpPosition" runat="server" CssClass="dimmerLabel" Style="width: 65px">
                                                                        <asp:ListItem Value="0">Before</asp:ListItem>
                                                                        <asp:ListItem Value="1" Selected="true">After</asp:ListItem>
                                                                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="drpPages" runat="server" CssClass="dimmerLabel"
                                                                        Style="width: 232px;">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </asp:PlaceHolder>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#ffffff" valign="top">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btnPageSave" ImageUrl="/ecn.images/images/btn_finish.gif" AlternateText="Finish"
                                                                runat="server"></asp:ImageButton>
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
                    </div>
                    <div class="dimming" id="divReOrder" style="width: 300px; height: 200px;">
                        <table width="300" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <asp:Label ID="lblReorderTitle" runat="server"></asp:Label></td>
                                <td class="dimTopRight">
                                    <asp:ImageButton ID="btnReOrderCancel" ImageUrl="/ecn.images/images/divs_05.gif"
                                        AlternateText="Close" runat="server"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft"></td>
                                <td class="dimMiddleCenter">
                                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr>
                                            <td bgcolor="#ffffff" valign="top">
                                                <asp:PlaceHolder ID="plPReorder" runat="server">
                                                    <div class="descWiz" align="left">
                                                        <span>Page:</span>&nbsp;<asp:Label ID="lblRPNo" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="descWiz" align="left">
                                                        Position:<br>
                                                        <asp:DropDownList ID="drpRPPosition" runat="server" CssClass="dimmerLabel" Style="margin-top: 5px">
                                                            <asp:ListItem Value="b">Before</asp:ListItem>
                                                            <asp:ListItem Value="a">After</asp:ListItem>
                                                        </asp:DropDownList>&nbsp;
                                                    </div>
                                                    <div class="descWiz" align="left">
                                                        Page No:<br>
                                                        <asp:DropDownList ID="drpRToPage" runat="server" CssClass="dimmerLabel">
                                                        </asp:DropDownList>&nbsp;
                                                    </div>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="plQReorder" runat="server">
                                                    <div class="descWiz" align="left">
                                                        Question:&nbsp;<asp:Label ID="lblRQNo" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="descWiz" align="left">
                                                        Move to Page:<br>
                                                        <asp:DropDownList ID="drpQPage" runat="server" CssClass="dimmerLabel" AutoPostBack="true"
                                                            OnSelectedIndexChanged="drpQPage_SelectedIndexChanged">
                                                        </asp:DropDownList>&nbsp;
                                                    </div>
                                                    <div class="descWiz" align="left">
                                                        Position:<br>
                                                        <asp:DropDownList ID="drpRQPosition" runat="server" CssClass="dimmerLabel">
                                                            <asp:ListItem Value="b">Before</asp:ListItem>
                                                            <asp:ListItem Value="a">After</asp:ListItem>
                                                        </asp:DropDownList>&nbsp;
                                                    </div>
                                                    <div class="descWiz" align="left">
                                                        Question:<br>
                                                        <asp:DropDownList ID="drpRToQuestion" runat="server" CssClass="dimmerLabel">
                                                        </asp:DropDownList>&nbsp;
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div align="center">
                                                    <asp:ImageButton ID="btnReOrderSave" ImageUrl="/ecn.images/images/btn_finish.gif"
                                                        AlternateText="Finish" runat="server"></asp:ImageButton>
                                                </div>
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
                    </div>
                    <div class="dimming" id="divBranch" style="width: 650px; height: 200px;">
                        <table style="width: 650px;" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Add / Edit Branching Logic</span></td>
                                <td class="dimTopRight">
                                    <asp:ImageButton ID="btnbranchCancel" ImageUrl="/ecn.images/images/divs_05.gif" AlternateText="Close"
                                        runat="server"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft"></td>
                                <td class="dimMiddleCenter">
                                    <div class="overFlowDimmer">
                                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
                                            <tr>
                                                <td bgcolor="#ffffff" valign="top">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <div class="descWiz" align="left">
                                                                    Select Question:<br>
                                                                    <asp:DropDownList ID="drpBQuestion" runat="server" CssClass="dimmerLabel" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="drpBQuestion_SelectedIndexChanged">
                                                                    </asp:DropDownList>&nbsp;
                                                                </div>
                                                                <asp:PlaceHolder ID="plbranch" runat="server" Visible="False">
                                                                    <div class="descWiz" align="left">
                                                                        <span id="defBranch">Define Branching:</span><br>
                                                                        <div style="text-align: center;">
                                                                            <div style="width: 550px; text-align: left; margin: 0 auto;">
                                                                                <asp:DataGrid ID="dgBranch" Style="width: 550px;" runat="server" AutoGenerateColumns="False"
                                                                                    CssClass="gridWizard" DataKeyField="OptionID">
                                                                                    <ItemStyle></ItemStyle>
                                                                                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundColumn DataField="OptionValue" HeaderText="Response Options"></asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="Page">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="drpBPage" runat="server" CssClass="dimmerLabel">
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                    </Columns>
                                                                                </asp:DataGrid>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </asp:PlaceHolder>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#ffffff" valign="top">
                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnbranchSave" ImageUrl="/ecn.images/images/btn_finish.gif"
                                                                    AlternateText="Finish" runat="server"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
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
                    </div>
                    <div class="dimming" id="divAlert" style="width: 450px; height: 300px;">
                        <table width="450" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span id="reorderPageTitle" style="display: none; font-weight: normal;">Re-order Page</span><span
                                        id="deleteQuestionTitle" style="display: none;">Delete Question</span></td>
                                <td class="dimTopRight">
                                    <a href="javascript:hideAlertR();hideAlertD();">close</a></td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft"></td>
                                <td class="dimMiddleCenter">
                                    <div id="reorderPageConfirm" style="display: none" class="dimmerLabel">
                                        <img src="/ecn.images/images/alertIcon.gif" align="left" />
                                        <p>
                                            Branching Exists. Cannot Re-order this page
                                        </p>
                                        <p class="ltButtonSmall clearfix" style="margin: 0 0 0 150px;">
                                            <a href="#" style="float: left; margin: 0 10px;"><span>OK</span></a>
                                        </p>
                                    </div>
                                    <div id="deleteQuestionConfirm" style="display: none" class="dimmerLabel">
                                        <img src="/ecn.images/images/questionConfirm.jpg" align="left" />
                                        <p>
                                            Are you sure you want to delete this question?
                                        </p>
                                        <p class="ltButtonSmall clearfix" style="margin: 0 0 0 100px;">
                                            <a href="#" style="float: left; margin: 0 10px;"><span>OK</span></a> <a href="javascript:hideAlertD();"
                                                style="float: left; margin: 0 10px;"><span>Cancel</span></a>
                                        </p>
                                    </div>
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
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
