
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.filterPreview" Codebehind="filterPreview.aspx.cs" %>

<head runat="server">
    <title>Filter Preview</title>
<style type="text/css">
			@import url( "/ecn.images/images/ecnstyle.css" ); 
			@import url( "/ecn.images/images/stylesheet.css" );
			@import url( "/ecn.images/images//stylesheet_default.css" );
		</style>
<script>
    function getobj(id) {
        if (document.all && !document.getElementById)
            obj = eval('document.all.' + id);
        else if (document.layers)
            obj = eval('document.' + id);
        else if (document.getElementById)
            obj = document.getElementById(id);

        return obj;
    }

    function openDownloadPage() {
        var channelID = "";
        var customerID = getobj("custID_Hidden").value;
        var groupID = getobj("grpID_Hidden").value;
        var fileType = getobj("FilteredDownloadType").value;
        var subType = "S";
        var srchType = "";
        var srchEm = "";
        var filterID = getobj("filterID_Hidden").value;


        window.open('download.aspx?chID=' + channelID + '&custID=' + customerID + '&grpID=' + groupID + '&fileType=' + fileType + '&subType=' + subType + '&srchType=' + srchType +'&srchEm=' + srchEm + '&filterID=' + filterID + '', '', 'width=500,height=200status=yes,toolbar=no,scrollbar=yes');
    }
</script>
</head><body runat="server">
<form id="PreviewEmailsForm" runat="Server">
    <asp:panel id="DownloadPanel" runat="Server" visible="true">
		Export this view to&nbsp; 
        <asp:DropDownList class="formfield" id="FilteredDownloadType" runat="Server" EnableViewState="true"
			visible="true">
			<asp:ListItem value=".xml">XML [.xml]</asp:ListItem>
			<asp:ListItem selected="true" Value=".xls">EXCEL [.xls]</asp:ListItem>
			<asp:ListItem value=".csv">CSV [.csv]</asp:ListItem>
			<asp:ListItem value=".txt">TXT [.txt]</asp:ListItem>
		</asp:DropDownList>&nbsp; 
		<input type="button" id="DownloadFilteredEmailsButton" onclick="openDownloadPage()" value="Export" class="formbuttonsmall">
		<br />
        <asp:label id="xsdDownloadLbl" runat="Server" visible="true"></asp:label>
        <HR color="#000000" noShade SIZE="1">	
    </asp:panel>
    <asp:datagrid id="EmailsGrid" runat="Server" width="100%" autogeneratecolumns="False"
        cssclass="grid">
        <ItemStyle></ItemStyle>
	    <HeaderStyle CssClass="gridheader"></HeaderStyle>
	    <FooterStyle CssClass="tableHeader1"></FooterStyle>
	    <Columns>
		    <asp:BoundColumn ItemStyle-Width="10%" DataField="EmailID" HeaderText="EmailID"></asp:BoundColumn>
		    <asp:BoundColumn ItemStyle-Width="40%" DataField="EmailAddress" HeaderText="EmailAddress"></asp:BoundColumn>
	    </Columns>
	    <AlternatingItemStyle CssClass="gridaltrow"/>
    </asp:datagrid>
    <AU:PagerBuilder ID="EmailsPager" runat="Server" ControlToPage="EmailsGrid" PageSize="100"
        Width="100%">
        <PagerStyle CssClass="gridpager"></PagerStyle>
    </AU:PagerBuilder>
    
    <input type="hidden" runat="Server" id="custID_Hidden" name='custID_Hidden'>
    <input type="hidden" runat="Server" id="grpID_Hidden" name='grpID_Hidden'>
    <input type="hidden" runat="Server" id="filterID_Hidden" name='filterID_Hidden'>
</form></body>