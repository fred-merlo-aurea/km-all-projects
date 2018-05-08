<%@ Page Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="NotificationsSetup.aspx.cs" Inherits="ecn.accounts.main.Notification.NotificationsSetup" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }

        .style1 {
            width: 100%;
        }

        .overlay {
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

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
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

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }

        fieldset
        {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }
       .cke_source 
    {
    white-space: pre-wrap !important;
    }
    </style>
    
    <script type="text/javascript" src="../../scripts/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="../../scripts/jquery-ui-sliderAccess.js"></script>
    
    <%--<script type="text/javascript" src="http://localhost/ecn.communicator/scripts/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="http://localhost/ecn.communicator/scripts/jquery-ui-sliderAccess.js"></script>--%>

    <script language="javascript" type="text/javascript">
        function enabledtpicker() {
            $("#<%= txtStartDate.ClientID %>").datepicker();
            $("#<%= txtEndDate.ClientID %>").datepicker();

            $('#<%=txtStartTime.ClientID%>').timepicker({
                ampm: false,
                addSliderAccess: true,
                sliderAccessArgs: { touchonly: false },
                timeFormat: 'hh:mm:ss',
                showSecond: true
            });

            $('#<%=txtEndTime.ClientID%>').timepicker({
                ampm: false,
                addSliderAccess: true,
                sliderAccessArgs: { touchonly: false },
                timeFormat: 'hh:mm:ss',
                showSecond: true
            });
        }
    </script>
    
    <link href="/ecn.communicator/scripts/toastr.css" rel="stylesheet"/>
    <script src="/ecn.communicator/scripts/toastr.js"  type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("[id*='emptycolor']").hide();
        });

        function toastrNotification() {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-ECN-style",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "10000000000000",
                //"timeOut": "10000",
                "extendedTimeOut": "10000000000000",
                //"extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            
            var tmp = CKEDITOR.instances['<%= texteditor.ClientID %>'].getData();
            toastr.info(tmp);
            
            //toastr.info(jQuery("[id*='texteditor']").val());
            var allImages = $("[id*='toast-container'] img");
            for (var i = 0; i < allImages.length; i++) {
                if (allImages[i].width > 490) {
                    var curImg = $("img[src$='"+allImages[i].src+"']");
                    curImg.attr("width", "490");
                }
            }

            SetColors();

            $(".toast").css("position", "relative");
            $(".toast").css("margin-top", "-26%");
            $(".toast").css("left", "33%");
        }

        function SetColors() {
            $(".toast-info").css("background-color", $("[id*='colorTwo']").val());
            $(".toast-close-button").css("color", $("[id*='colorOne']").val());
            $(".toast-close-button:hover").css("color", $("[id*='colorOne']").val());
        }

        function OnClientColorChangeRcp1(sender, eventArgs) {
            var color = sender.get_selectedColor();
            $("[id*='colorOne']").val(color);
            SetColors();
        }
        function OnClientColorChangeRcp2(sender, eventArgs) {
            var color = sender.get_selectedColor();
            $("[id*='colorTwo']").val(color);
            SetColors();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:textbox id="colorOne" runat="server" style="display: none"></asp:textbox>
    <asp:textbox id="colorTwo" runat="server" style="display: none"></asp:textbox>

    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="mdlPreview" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpreview" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="pnlpreview" runat="server" Width="600px" Height="420px" Style="display: none"
        CssClass="modalPopup">
        <asp:UpdatePanel ID="ModalPopupUpdatePanel" runat="server">
            <ContentTemplate>
                <div style="text-align: right; width:97%">
                    <asp:Button ID="CloseButton" runat="server" Text="Close" CssClass="button"
                        OnClick="btnClose_Click" />&nbsp;
                </div>
                <div id="Div1" align="center" style="text-align: center; height: 400px; padding: 10px 10px 10px 10px; overflow: scroll"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                        <tr>
                            <td style="padding: 5px;" align="left">
                                <asp:Label ID="lblNotifiationName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px;" align="left">
                                <asp:Label ID="lblPreview" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table height="67" width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <br />
            <table cellpadding="5" cellspacing="5" width="100%" border='0' class="formLabel">
                <tr>
                    <td align="right"><b>Notification Name :&nbsp;</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtName" runat="server" CssClass="styled-text" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="« required" ValidationGroup="save" />
                    </td>
                </tr>
                <tr>
                    <td align='right'>
                        <b>Start Date&nbsp;:&nbsp;</b>
                    </td>
                    <td width="70%" align="left">
                        <asp:TextBox ID="txtStartDate" runat="Server" Width="80"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate" ValidationGroup="save" ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpRecTxtStartDate" runat="server" ControlToValidate="txtstartDate" ErrorMessage="Please enter a valid date" Operator="DataTypeCheck" Type="Date" ValidationGroup="save"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align='right'>
                        <b>Start Time&nbsp;:&nbsp;</b>
                    </td>
                    <td width="70%" align="left">
                        <asp:TextBox ID="txtStartTime" runat="server" Width="80"></asp:TextBox> (CST)
                        <asp:RequiredFieldValidator ID="rfvdrpStartTime" runat="server" ControlToValidate="txtStartTime" ValidationGroup="save"
                                    ErrorMessage="« required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align='right'>
                        <b>End Date&nbsp;:&nbsp;</b>
                    </td>
                    <td width="70%" align="left">
                        <asp:TextBox ID="txtEndDate" runat="Server" Width="80"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rfvEndDate" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate" ValidationGroup="save" ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpRecTxtEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Please enter a valid date" Operator="DataTypeCheck" Type="Date" ValidationGroup="save"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align='right'>
                        <b>End Time&nbsp;:&nbsp;</b>
                    </td>
                    <td width="70%" align="left">
                        <asp:TextBox ID="txtEndTime" runat="server" AutoPostBack="false" Width="80"></asp:TextBox> (CST)
                                <asp:RequiredFieldValidator ID="rfvEndTime" runat="server" ControlToValidate="txtEndTime" ValidationGroup="save"
                                    ErrorMessage="« required"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td align="right" valign="top"><b>Background Color :</b></td>
                    <td>
                         <telerik:RadColorPicker AutoPostBack="false"  runat="server" SelectedColor="#f47e1f" ID="RadColorPicker2" Preset="None" Width="290" OnClientColorChange="OnClientColorChangeRcp2">
                            <telerik:ColorPickerItem Title="KM Dark Blue" Value="#045DA4"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Orange" Value="#F47E1F"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Blue" Value="#4B87BC"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Gray 1" Value="#EBEBEB"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Gray 2" Value="#C7CACC"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Medium Gray" Value="#9DA2A7"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Dark Gray" Value="#3F3F40"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM New Light Blue" Value="#559EDB"></telerik:ColorPickerItem>
                        </telerik:RadColorPicker>
                    </td>
                </tr>
                 <tr>
                    <td align="right" valign="top"><b>Close Button Color :</b></td>
                    <td>
                        <telerik:RadColorPicker AutoPostBack="false"  runat="server" SelectedColor="#045da4" ID="RadColorPicker1" Preset="None" Width="290" OnClientColorChange="OnClientColorChangeRcp1">
                            <telerik:ColorPickerItem Title="KM Dark Blue" Value="#045DA4"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Orange" Value="#F47E1F"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Blue" Value="#4B87BC"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Gray 1" Value="#EBEBEB"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Light Gray 2" Value="#C7CACC"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Medium Gray" Value="#9DA2A7"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM Dark Gray" Value="#3F3F40"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Title="KM New Light Blue" Value="#559EDB"></telerik:ColorPickerItem>
                        </telerik:RadColorPicker>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top"><b>Image/Text :</b></td>
                    <td>
                       <%--<telerik:RadEditor ID="texteditor" runat="server" Height="450px" Width="700px" EditModes="Html,Design,Preview"  ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools_Simple.xml" Visible="true" />--%>
                 
                        <CKEditor:CKEditorControl ID="texteditor" runat="server" Height="450px" Width="780px" BasePath="/ecn.editor/ckeditor" Toolbar="Custom"></CKEditor:CKEditorControl>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSave" OnClick="btnSave_click" Visible="true" Text="Save" CssClass="formfield" ValidationGroup="save" runat="Server" />&nbsp;
                        <asp:Button ID="btnCancel" OnClick="btnCancel_click" Visible="true" Text="Cancel" CssClass="formfield" runat="Server" />&nbsp;
                        <asp:Button ID="btnPreview" OnClientClick="toastrNotification(); return false;" Visible="true" Text="Preview" CssClass="formfield" runat="Server" />
                        <%--<asp:Button ID="btnPreview" OnClick="btnPreview_click" Visible="true" Text="Preview" CssClass="formfield" runat="Server" />--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
