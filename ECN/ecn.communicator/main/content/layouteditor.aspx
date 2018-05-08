<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.layouteditor" CodeBehind="layouteditor.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/main/ECNWizard/Content/layoutEditorNew.ascx" tagname="layoutEditor" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.aspBtn1 {
	-moz-box-shadow:inset 0px 1px 0px 0px #ffffff;
	-webkit-box-shadow:inset 0px 1px 0px 0px #ffffff;
	box-shadow:inset 0px 1px 0px 0px #ffffff;
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	background:-moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-moz-border-radius:6px;
	-webkit-border-radius:6px;
	border-radius:6px;
	border:2px solid #dcdcdc;
	display:inline-block;
	color:#000000;
	font-family:arial;
	font-size:15px;
	font-weight:bold;
	padding:6px 24px;
	text-decoration:none;
	text-shadow:1px 1px 0px #ffffff;
}.aspBtn1:hover {
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
	background:-moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
	background-color:#dfdfdf;
}.aspBtn1:active {
	position:relative;
	top:1px;
}
</style>
   
    
<script type="text/javascript">
    $(document).ready(function () {
        window.addEventListener("beforeunload", function (e) {
            $.ajax({
                url: "layouteditor.aspx/RemovePageSession",
                type: "POST",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });
        });

    });
   
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
    <asp:PlaceHolder ID="phWarning" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="674" align="center" style="border: 2px solid #856200; border-radius: 25px; background-color: white">
                        <tr>
                            <td id="">
                                <table height="67" width="90%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <div style="padding-top: 20px">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/warningEx.jpg">    
                                            </div>
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblWarningMessage" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                               
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </asp:PlaceHolder>
<table width="100%">
            <tr align="center">
                <td>
                  <uc1:layoutEditor ID="layoutEditor1" runat="server" />
                </td>
            </tr>
             <tr align="center">
                <td>
                 <br />
                <asp:Button ID="btnSave" OnClick="btnSave_Click" Visible="true" Text="Save Message" CssClass="aspBtn1" runat="Server"/>
                </td>
            </tr>
</table>       
</asp:Content>
