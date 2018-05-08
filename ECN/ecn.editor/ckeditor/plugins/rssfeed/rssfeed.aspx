<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rssfeed.aspx.cs" Inherits="ecn.editor.ckeditor.plugins.rssfeed.rssfeed" %>
<html>
<head>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <base target="_self" />
        <style type="text/css">
			BODY { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;background:#F7F7F7; }
			TD { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			INPUT { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			SELECT { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			TEXTAREA { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			BUTTON { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			tr.warnPad td { padding-top:5px; }
			.border { border-bottom:1px #ccc solid; }
			#udfRow td { padding-top:20px; }
			#udfRow td td { padding-top:0px; }
			.labelCell { width:120px; }
		</style>
    <script type="text/javascript" src="/ecn.collector/scripts/Templatestyle.js"></script>
    		<script src="common/fck_dialog_common.js" type="text/javascript"></script>
		<link href="common/fck_dialog_common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function updateSelection() {
            var selected = getobj("ddlSelectRSSFeed");

            rssFeed.value = selected.options[selected.selectedIndex].value;
        }
        function ok() {
            var selected = rssFeed;
            if (selected.length == 0) {
                cancel();
            }
            else {
                if (window.opener) {
                    if (window.opener.setValue != undefined) {
                        window.opener.setValue(rssFeed.value.toString());
        }
            }

            }
            this.close();
        }

        function cancel() {
            window.returnValue = "";
            this.close();
        }

    </script>
    <script type="text/javascript" language="JavaScript">


        function getobj(id) {
            if (document.all && !document.getElementById)
                obj = eval('document.all.' + id);
            else if (document.layers)
                obj = eval('document.' + id);
            else if (document.getElementById)
                obj = document.getElementById(id);

            return obj;
        }
    </script>
    <title>Insert RSSFeed</title>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">

    <table cellspacing="0" cellpadding="0" border="0" style="margin-left:auto;margin-right:auto;" ID="Table2">
        <tr>
            <td style="vertical-align:top;padding:5px;">
                <b>Select an RSSFeed</b>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;padding:5px;">
                    <form id="form1" runat="server">
                <select name="ddlSelectRSSFeed" id="ddlSelectRSSFeed" onchange="updateSelection();">
                    <% Response.Write(RSSFeeds); %>
                    </select>
                        </form>
                <input name="rssFeed" id="rssFeed" type="hidden" >
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;padding:5px;">
                <input type="button" onclick="ok();" value="OK" /> &nbsp;&nbsp;
                <input type="button" value="Cancel" onclick="cancel();" />
            </td>
        </tr>
    </table>
</body>
</html>
