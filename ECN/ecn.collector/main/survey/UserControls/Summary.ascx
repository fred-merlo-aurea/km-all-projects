<%@ Control Language="c#" Inherits="ecn.collector.main.survey.UserControls.Summary" Codebehind="Summary.ascx.cs" %>
<div align="center">
    <div class="section">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    Survey Title:</td>
                <td colspan="3">
                    <asp:Label ID="lblTitle" runat="server" CssClass="dataTwo"></asp:Label></td>
            </tr>
            <tr>
                <td width="20%" align="right" class="formLabel" style="padding-right: 20px">
                    Participant Group:</td>
                <td width="30%">
                    <asp:Label ID="lblGroupName" runat="server" CssClass="dataTwo"></asp:Label></td>
                <td width="15%" align="right" class="formLabel" style="padding-right: 20px">
                    Status:</td>
                <td width="35%">
                    <asp:Label ID="lblStatus" runat="server" CssClass="dataTwo">&nbsp;</asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    Total Participants:</td>
                <td>
                    <asp:Label ID="lblTotalParticpants" runat="server" CssClass="dataTwo"></asp:Label></td>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    Activation Date:</td>
                <td>
                    <asp:Label ID="lblActivationDate" runat="server" CssClass="dataTwo"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    Total Pages:</td>
                <td>
                    <asp:Label ID="lblTotalPages" runat="server" CssClass="dataTwo"></asp:Label></td>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    Deactivation Date:</td>
                <td>
                    <asp:Label ID="lblDeActivationDate" runat="server" CssClass="dataTwo"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px;">
                    Total Questions:</td>
                <td colspan="3">
                    <asp:Label ID="lblTotalQuestions" runat="server" CssClass="dataTwo"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="4" style="border-bottom: 2px #fff solid;">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px; padding-top: 15px;">
                    Status:&nbsp;</td>
                <td class="dataOne" align="left" style="padding-top: 15px;">
                    <asp:RadioButtonList ID="rbStatus" runat="server" CssClass="label10" RepeatDirection="horizontal">
                        <asp:ListItem Value="Y">Active</asp:ListItem>
                        <asp:ListItem Value="N" Selected="True">InActive</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3" class="formLabel">
                    <br />
                    Copy and Paste this link to direct respondents to your survey.</td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    URL (Direct link to Survey):</td>
                <td colspan="3">
                    <asp:TextBox ID="txtURL" runat="server" CssClass="dataTwo" Width="650" Style="width: 650px;
                        font-weight: normal; background: #cdfdd3;"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" class="formLabel" style="padding-right: 20px">
                    URL (For Email Blast):</td>
                <td colspan="3">
                    <asp:TextBox ID="txtBlastURL" runat="server" CssClass="dataTwo" Width="650" Style="width: 650px;
                        font-weight: normal; background: #cdfdd3;"></asp:TextBox>
                    <input type="button" value="Copy URL" onclick="copy(SurveyWizard_txtURL.value)" id="copyLinkBtn"
                        style="display: none;"></td>
            </tr>
        </table>
    </div>
    <div>
        <p style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 10px;
            text-align: center">
            <asp:HyperLink ID="idPreview" Style="margin: 0px auto; text-align: left" CssClass="ltButton"
                Width="140" runat="server" Target="_blank"><span style="text-align:center;">Preview</span></asp:HyperLink></p>
    </div>
</div>

<script type="text/javascript">
function prepareCopier() {
  var browserName=navigator.appName; 
  if (browserName=="Microsoft Internet Explorer"){
    if (!document.getElementById) return false;
    var copyLink = document.getElementById("copyLinkBtn");
    copyLink.style.display='inline';
	document.getElementById("SurveyWizard_txtURL").style.width="556px";
  }
  

}

function copy(what) {
window.clipboardData.setData('Text',what.value);
}

//prepareCopier();
</script>

