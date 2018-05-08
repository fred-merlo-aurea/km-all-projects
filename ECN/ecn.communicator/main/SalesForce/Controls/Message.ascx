<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Message.ascx.cs" Inherits="ecn.communicator.main.Salesforce.Controls.Message" %>
<script type="text/javascript">
    function ShowPopup(message, title) {
        $(function () {
            $("#dialog").html(message);
            $("#dialog").dialog({
                title: title,
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true,
                width: 450
            });
        });
    };
    </script>
<div id="dialog" style="display: none">
</div>