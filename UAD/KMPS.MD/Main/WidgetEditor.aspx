<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WidgetEditor.aspx.cs" Inherits="KMPS.MD.Main.WidgetEditor"
    Theme="" %>

<%@ Register Assembly="Kalitte.Dashboard.Framework" Namespace="Kalitte.Dashboard.Framework"
    TagPrefix="kalitte" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Widget Editor</title>
    
    <style type="text/css">
        .body
        {
            background-color: #ffffff;
            font-size: 11px;
            font-family: 'Segoe UI' , Verdana;
        }
        .propTd
        {
            width: 100%;
            text-align: right;
        }
        .cmdBtn
        {
            width: 25px;
        }
        .footerTable
        {
            width: 100%;
        }
        .footer
        {
            width: 100%;
            height: 28px;
        }
    </style>

    <script language="javascript" type="text/javascript">



        function OnDone(hide) {
            var arg = null;
            arg = eval("arg=" + document.getElementById('arguments').value);
            var command = new parent.window.CommandData('editDone', arg, true);
            parent.window.doServerCommand(parent.window.EditorWindow.widget.widgetConfig['widgetId'] + 'Controller',
                                          parent.window.EditorWindow.widget.widgetConfig['widgetId'] + 'Command',
                                          command,
                                          parent.window.EditorWindow.Props['target'], parent.window.msg_Applying);
            if (hide == true)
                parent.window.EditorWindow.hide();
        }

        function load() {
            if (document.getElementById('hdnValue').value == "done" && parent.window.EditorWindow)
                OnDone(true);
            else if (document.getElementById('hdnValue').value == "apply" && parent.window.EditorWindow)
                OnDone(false);
        }


    </script>

</head>
<body class="body">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <kalitte:ScriptManager ID="ScriptManager2" runat="server">
    </kalitte:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnValue" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="arguments" runat="server" EnableViewState="false" Value="null" />

                <asp:PlaceHolder ID="pc" runat="server"></asp:PlaceHolder>

            <div class="footer">
                <table class="footerTable" cellpadding="5">
                    <tr>
                        <td class="cmdBtn">
                            <asp:Button ID="btnOk" runat="server" Text="Apply" OnClick="btnOk_Click" />
                        </td>
                        <td class="cmdBtn">
                            <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" Visible="false"/>
                        </td>
                        <td class="cmdBtn">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="false" DisplayAfter="1">
                                <ProgressTemplate>
                                    <div style="float: right">
                                        <img src="/Images/loading.gif" alt="" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                        <td class="propTd">
                            <asp:UpdatePanel runat="server" ID="ctlup2" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <asp:LinkButton ID="ctnEditWidget" runat="server" OnClick="ctnEditWidget_Click" Visible="false">Edit Widget Settings</asp:LinkButton>
                                    <kalitte:WidgetPropertyEditor WindowTitle="Widget Property Editor" ID="WidgetEditor1"
                                        runat="server" ShowInWindow="true" OnSaveDone="WidgetEditor1_SaveDone">
                                        <PanelSettings />
                                        <WindowSettings Modal="true" Hidden="true" Header="true" />
                                    </kalitte:WidgetPropertyEditor>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
