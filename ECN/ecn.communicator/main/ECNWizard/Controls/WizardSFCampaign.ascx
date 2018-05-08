<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardSFCampaign.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardSFCampaign" %>
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
 <div class="section bottomDiv"  style="padding-left: 30px;padding-right: 30px">
     <table>
         <tr>
             <td>
                 <asp:Label ID="Label1" runat="server" Text="Salesforce Campaigns"></asp:Label>
             </td>
             <td>
                 <asp:DropDownList ID="drpSFCampaigns" runat="server" OnSelectedIndexChanged="drpSFCampaigns_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
             </td>
         </tr>
          <tr>
                                    <td colspan="2">
                                        <br />
                                        License Details
                                    </td>
        </tr>
        <tr>
                <td colspan="2">
                                        
                        <table width="100%">
                            <tr>
                                <td class="dataOne" width="100%">
                                        <table class="tablecontent" width="100%">
                                            <tr>
                                                <td>
                                                    <b>Licensed: </b><b>
                                                        <asp:Label ID="BlastLicensed" runat="Server"></asp:Label>
                                                    </b>
                                                </td>
                                                <td>
                                                    <b>Sent: </b><b>
                                                        <asp:Label ID="BlastUsed" runat="Server"></asp:Label>
                                                    </b>
                                                </td>
                                                <td>
                                                    <b>Remaining: </b><b>
                                                        <asp:Label ID="BlastAvailable" runat="Server"></asp:Label>
                                                    </b>
                                                </td>
                                                <td>
                                                    <b>This Blast: </b><b>
                                                        <asp:Label ID="BlastThis" runat="Server" Text="0"></asp:Label>
                                                    </b>
                                                </td>
                                            </tr>
                                        </table>
                                </td>
                            </tr>
                        </table>
                </td>
        </tr>
     </table>
     
</div>