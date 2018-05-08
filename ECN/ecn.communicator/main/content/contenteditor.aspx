<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.contentmanager.contenteditor"
    CodeBehind="contenteditor.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>
    
<%@ Register src="~/main/ECNWizard/Content/contentEditor.ascx" tagname="contentEditor" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000);

        function setValue(args) {
            var fckEditor = document.getElementById(<%= contentEditor1.ClientID %>);
            if (fckEditor.setValue != undefined) {
                fckEditor.setValue(args);
            }
        }

        function setData(args) {
            var fckEditor = document.getElementById(<%= contentEditor1.ClientID %>);
            if (fckEditor.setData != undefined) {
                fckEditor.setData(args);
            }
        }

    </script>
    <script type="text/javascript">

        function toastrContentSaved(contentID) 
        {
            toastr.options =
                {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-ECN-style",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "tapToDismiss": false,
                }
            toastr.info('<table>'
                            +'<tbody>'
                                +'<tr>'
                                    + '<td valign="middle"><img  class="PopUpImage" alt="Success Check Mark" height="68" src="/ecn.images/images/successEx.jpg" width="70" /></td>'
                                    + '<td valign="middle"><span class="PopUpText"><span>Content saved successfully</span></span></td>'
                                +'</tr>'
                             +'</tbody>'
                         +'</table>'
                         );
            

            $(".toast").css("position", "relative");
            $(".toast").css("margin-top", "-26%");
            $(".toast").css("left", "33%");
            $(".toast-info").css("background-color", "#FFFFFF");

   
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
    <tr>
        <td>
          <uc1:contentEditor ID="contentEditor1" runat="server" />
        </td>  
    </tr>
    <tr align="center" >
        <td>
            <br />
            <asp:Button ID="SaveButton" OnClick="CreateContent" Visible="true" Text="Save Content" class="formbutton" runat="Server"/>
        </td>
    </tr>


</table>   
</asp:Content>