<%@ Page Language="c#" AutoEventWireup="false" Inherits="ecn.communicator.contentmanager.ckeditor.dialog.codeSnippets" CodeBehind="codeSnippets.aspx.cs" %>

<%@ Register Src="~/ckeditor/controls/groupexplorer.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html>
<head>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <base target="_self" />
    <style type="text/css">
        body, td, input, select, textarea, button {
            font-size: 11px;
            font-family: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }
    </style>
    <script src="common/fck_dialog_common.js" type="text/javascript"></script>
    <link href="common/fck_dialog_common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function getQueryStringParam(name) {
            // Simple Regex -- Could easily be extended by 
            // increasing the complexity of the Regex
            var pattern = new RegExp(name + '.*');
            var result = pattern.exec(window.location.search)[0];
            var param = result.split('=')[1];
            return param;
        }
        function getEditorID() {
            return getQueryStringParam('editor_id');
            }


        function updateSelection() {
            var selected = getobj("ddlCodeSnippet");
            var codeSnippet = getobj("codeSnippet");
            codeSnippet.value = selected.options[selected.selectedIndex].value.toString();
        }
        function ok() {
            var hfCodeSnippet = getobj("hfCodeSnippet");
            var selected = hfCodeSnippet.value;
            var editor_id = window.editor_id;
            if(!editor_id) // For IE compatibility
                editor_id = getEditorID();
            if (!editor_id)
                console.log("Could not obtain editor_id");
            console.log("selected "+ selected);

            if (selected.length == 0) {
                console.log("inside if");
                cancel();
            }
            else {
                console.log("inside else");

                if (window.opener) {
                    console.log("codesnippet" + selected);
                    // Add the text from the selected form to the editor
                    window.opener.CKEDITOR.instances[editor_id].insertText(selected);

                }
            }

            window.close();
        }

        function cancel() {
            window.returnValue = "";
            this.close();
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

    </script>
    <title>Insert Codesnippet
    </title>

</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    
    <form id="codeSnippetForm" runat="server">
     <asp:UpdatePanel runat="server">
      <ContentTemplate>
        <table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table2">
            <tr>
                <td align="left" >
                   <div style="background-color:white;border:3px solid #8FC4F0;border-radius:10px;float:left;vertical-align:middle;width:90%;">
                                <img src="http://images.ecn5.com/images/infoEx.jpg" style="float:left;border-radius:10px;">
                                <br /><br />
                                Selecting User Defined Fields [UDFs] from different Groups for the same Campaign will cause Campaign not to work correctly. Please choose UDFs from the Group you are trying to send this Campaign.
                                
                            </div>
                            
                </td>
            </tr>
            <tr>
                <td valign="top" style="padding-top: 5px"><b>Select&nbsp;Group</b></td>
            </tr>

            <tr>
                <td valign="top">


                    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="3600" runat="server">
                    </asp:ScriptManager>
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
                        
            </tr>

            <tr>
                <td height="8"></td>
            </tr>
            <tr>
                <td valign="top">
                    <b>Select&nbsp;Code&nbsp;Snippet</b><br>

                    <asp:DropDownList name="ddlCodeSnippet" ID="ddlCodeSnippet" runat="server" OnSelectedIndexChanged="ddlCodeSnippet_SelectedIndexChanged" onchange="updateSelection();" AutoPostBack="true">
                    </asp:DropDownList>

                    <input name="codeSnippet" id="codeSnippet" type="hidden">
                    <asp:HiddenField ID="hfCodeSnippet" runat="server" />
                </td>
            </tr>

            <tr>
                <td height="7"></td>
            </tr>
            <tr>
                <td align="right" height="26" valign="top">
                    <input type="button" onclick="ok();" value="OK">&nbsp;
					&nbsp;<input type="button" value="Cancel" onclick="cancel();">
                </td>
            </tr>

        </table>

        <uc1:groupsLookup ID="groupExplorer" runat="server" UDFFilter="all" Visible="false" />
     </ContentTemplate>
    </asp:UpdatePanel>
    </form>
        
</body>

</html>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>