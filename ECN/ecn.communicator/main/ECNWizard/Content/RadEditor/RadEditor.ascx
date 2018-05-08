<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadEditor.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Content.RadEditor.RadEditor" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
<script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
<script type="text/javascript">
    <%--var tempEditor;
    function repaint()
    {
        if (tempEditor != null) {
            tempEditor.set_mode(2);
            tempEditor.set_mode(1);
            tempEditor.repaint();
        }

    }--%>

</script>
<script type="text/javascript" language="javascript">
    function OnClientCommandExecuting(editor, args) {
        var commandName = args.get_commandName();
        if (commandName == "ToggleScreenMode") {
            if (!editor.isFullScreen()) //if the editor is placed in fullscreen mode
            {
                $(".modalBackground").css("display", "none");
                fullscreen = true;
            }
            else {
                $(".modalBackground").css("display", "none");
                $(".modalBackgroundTemp").css("display", "");
            }
        }
        //tempEditor = editor;
        editor.repaint();
    }

    function AfterClientLoad(editor,args)
    {
        tempEditor = editor;
        editor.repaint();
    }



    function getElementsByClassName(classname, node) {
        if (!node) node = document.getElementsByTagName("body")[0];
        var a = [];
        var re = new RegExp('\\b' + classname + '\\b');
        var els = node.getElementsByTagName("*");
        for (var i = 0, j = els.length; i < j; i++)
            if (re.test(els[i].className)) a.push(els[i]);
        return a;
    }
            </script>
 <style>
    /*.RadWindow.RadWindow_Default.rwNormalWindow.rwTransparentWindow
    {
         z-index:100020 !important;
    }
    .RadWindow.RadWindow_Default.rwNormalWindow.rwTransparentWindow
    {
         z-index:100020 !important;
    }*/
    .Default.reDropDownBody {
        z-index:100020 !important;
    }
    </style>
<script type="text/javascript">
    
    //function OnClientLoad(sender, args) {
    //    var radEditor = sender;
    //    radEditor.set_mode(2);
    //    radEditor.set_mode(1);
    //}
</script>
<table style="width:100%;">
    <tr>
        <td>
            
            <telerik:RadEditor ID="radEditor" ContentAreaMode="Div" OnClientLoad="OnClientLoad" NewLineMode="Br" OnClientModeChange="OnClientModeChanged" ExternalDialogsPath="~/main/ECNWizard/Content/RadEditor/EditorDialogs" OnClientCommandExecuting="OnClientCommandExecuting" AllowScripts="true" runat="server" Height="500px" Width="780px" EditModes="Html,Design"  ToolsFile="~/main/ECNWizard/Content/RadEditor/Tools.xml" ContentFilters="MakeUrlsAbsolute,EncodeScripts" Visible="true">
            </telerik:RadEditor>
        </td>
    </tr>
</table>
