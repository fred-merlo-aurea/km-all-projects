<%@ Control Language="c#" Inherits="ecn.communicator.main.SMSWizard.Controls.CampaignInfo"
    CodeBehind="CampaignInfo.ascx.cs" %>



<div class="section">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="150" class="headingOne">
                <!--Define Message:-->
                &nbsp;
            </td>
            <td class="headingOne highLightOne">
                (This Field is not visible to your recipients).
            </td>
        </tr>
        <tr>
            <td class="formLabel">
                Message Name
            </td>
            <td class="dataOne">
                <asp:TextBox ID="txtMessageName" runat="server" MaxLength="50" Width="250" CssClass="label10"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>