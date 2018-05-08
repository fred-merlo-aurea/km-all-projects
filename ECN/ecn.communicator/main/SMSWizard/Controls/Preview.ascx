<%@ Control Language="c#" Inherits="ecn.communicator.main.SMSWizard.Controls.previewLbltext"
    CodeBehind="Preview.ascx.cs" %>
<style type="text/css">
    #previewLbltext_lblPreviewMobile
    {
        background:#fff;
        border:1px #999 solid;
        width:700px;
        text-align:left;
        display:block;
        margin:0 auto;
        overflow-x:auto;
    }
</style>

<div class="section">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="right" class="formLabel" style="padding-right: 20px;">
                Message :
            </td>
            <td>
                <asp:Label ID="lblMessageName" runat="server" CssClass="dataTwo"></asp:Label>&nbsp;
            </td>
             <td align="right" class="formLabel" style="padding-right: 20px;">
                Group Name : </td>
              <td >
                &nbsp;
                <asp:Label ID="lblGroupName" runat="server" CssClass="dataTwo">&nbsp;</asp:Label>
            </td>
        </tr>
        
        <tr>       
           
            <td align="right" class="formLabel" style="padding-right: 20px;">
                Content :</td>
            <td>
                <asp:Label ID="lblContent" runat="server" CssClass="dataTwo"></asp:Label>
            </td>        
            <td align="right" class="formLabel" style="padding-right: 20px;">
                Recipients :</td>
            <td>
                &nbsp;
                <asp:Label ID="lblReceipientCount" runat="server" CssClass="dataTwo"></asp:Label>                
                <asp:Label ID="lblFilter" runat="server" CssClass="dataTwo" Visible="false"></asp:Label>
            </td>
        </tr>
       
       
    </table>
</div>
<div class="section previewTextBox bottomDiv" style="align: center;">
    <font class="headingTwo">Text Preview:</font><br/>
    <center>
        <asp:TextBox ID="lblpreviewTxt" CssClass="formfield" runat="server" Width="700" EnableViewState="False"
            TextMode="multiline" Columns="126" Rows="15"></asp:TextBox></center>
</div>
