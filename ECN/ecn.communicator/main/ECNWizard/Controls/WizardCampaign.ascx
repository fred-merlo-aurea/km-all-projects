<%@ Control Language="c#" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardCampaign" CodeBehind="WizardCampaign.ascx.cs" %>
<%@ Register Src="~/main/ECNWizard/OtherControls/BlastFieldsConfig.ascx" TagName="BlastFieldsConfig" TagPrefix="uc1" %>

<script type="text/javascript" src="/ecn.communicator/scripts/jquery-1.3.2.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        
        doMouseOver();
    });

    function doMouseOver()
    {
        $('.lblInfo').each(function () {
            if ($(this).attr("mouseover") != null) {
                $(this).qtip(
   {
       content: $(this).attr("mouseover"),
       show: {
           when: { event: 'mouseover' },
           ready: false
       },

       style: {
           name: 'blue',
           tip: 'topLeft'
       }
   });
            }
        });
    }
</script>
<style type="text/css">
    fieldset
    {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }
    .styled-select
    {
        width: 240px;
        background: transparent;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
    .styled-text
    {
        width: 240px;
         height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
     .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalPopupFull
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width:100%; height:100%;        
        overflow:auto; 
    }
     .modalPopup
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;   
          overflow:auto;     
    }
    .buttonMedium
    {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }
    .TransparentGrayBackground
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }
    .overlay
    {
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
    * html .overlay
    {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }
    .loader
    {
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
    * html .loader
    {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
</style>

<script type="text/javascript" src="/ecn.communicator/scripts/jquery.qtip-1.0.0-rc3.min.js"></script>
<asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
            <asp:Panel ID="Panel4" CssClass="loader" runat="server">
                <div>
                    <center>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        
        <br />
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr align="left" valign="middle">
                <td width="20%"></td>
                <td class="formLabel" style="padding-left: 30px">Select one of the Campaign options: 
                </td>
                <td class="formLabel">
                    <asp:RadioButton ID="rbNewCampaign" GroupName="grpCampaign" Text="Create New Campaign"
                        runat="server" AutoPostBack="True" CssClass="expandAccent" Checked="false" OnCheckedChanged="rbNewCampaign_CheckedChanged"></asp:RadioButton>
                </td>
                <td class="formLabel">
                    <asp:RadioButton ID="rbExistingCampaign" CssClass="expandAccent" AutoPostBack="True"
                        runat="server" Text="Use Existing Campaign" GroupName="grpCampaign" Checked="True" OnCheckedChanged="rbExistingCampaign_CheckedChanged"></asp:RadioButton>
                </td>
                <td width="20%"></td>
            </tr>
        </table>
        <div class="section bottomDiv" id="content" style="padding-left: 30px; padding-right: 30px">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td class="dataOne" align="left" colspan="2">
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Campaign Item
                                        </td>
                                    </tr>
                                </table>
                            </legend>
                            <table border="0" cellpadding="0" cellspacing="0" style="padding-left: 30px">
                                <asp:PlaceHolder ID="plCreateCampaign" Visible="false" runat="server">
                                    <tr>
                                        <td class="formLabel" width="200">Campaign Name<label class="lblInfo" mouseover="The category that the email campaign item is a part of.">
                                            <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>

                                        </td>
                                        <td width="250">
                                            <asp:TextBox ID="txtCampaignName" runat="server" MaxLength="250" Width="250" CssClass="label10"></asp:TextBox>
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plExistingCampaign" Visible="false" runat="server">
                                    <tr>
                                        <td class="formLabel" width="200">Select Campaign <label class="lblInfo" mouseover="The category that the email campaign item is a part of.">
                                            <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>
                                        </td>
                                        <td class="formLabel"  width="250">
                                            <asp:DropDownList ID="drpdownCampaign" runat="server"  CssClass="styled-select">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td class="formLabel" width="200">Campaign Item Name<label class="lblInfo" mouseover="Title of the specific email campaign.">
                                        <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>
                                    </td>
                                    <td class="dataOne"  style=" height: 28px;width:250px">
                                        <asp:TextBox ID="txtCampaignItemName" runat="server" MaxLength="50" Width="250" CssClass="label10"></asp:TextBox>
                                    </td>
                                </tr>
                                 <asp:PlaceHolder ID="plCampaignItemTemplate" Visible="true" runat="server">
                                    <tr>
                                        <td class="formLabel" width="200">
                                            Campaign Item Template
                                        </td>
                                        <td class="dataOne"  style=" height: 28px;width:250px">
                                            <asp:Label ID="lblCampaignItemTemplate" runat="server" Text=""></asp:Label>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td class="formLabel" width="200">
                                    </td>
                                    <td class="dataOne"  style=" height: 28px;width:250px">
                                        <asp:CheckBox ID="cbIgnoreSuppression" runat="server" Text="Transactional Emails - Don't Apply Suppression" Checked="false" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:Panel ID="pnlBlastFields" runat="Server" Visible="true">
                            <fieldset>
                                <legend>Blast Fields</legend>
                                <table cellspacing="1" cellpadding="1" border='0'  style="padding-left: 30px">
                                    <tr>
                                        <td class="formLabel" valign="top" align='left' width="200">
                                            <asp:Label ID="lblBlastField1" runat="server" Text="Field1"></asp:Label>&nbsp;
                                             <asp:ImageButton ID="imgBtnBlastFieldsConfig1" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgBtnBlastFieldsConfig1_Click" Visible="true"/>                                     
                                 
                                        </td>
                                        <td class="label" align="left" width="254">
                                            <asp:DropDownList ID="drpBlastField1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBlastField1_SelectedIndexChanged" DataValueField="Value" DataTextField="Value" CssClass="styled-select">
                                            </asp:DropDownList>                                          
                                        </td>
                                        <td>
                                              <asp:TextBox ID="txtBlastField1" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="top" align='left' width="200">
                                              <asp:Label ID="lblBlastField2" runat="server" Text="Field2"></asp:Label>&nbsp;
                                             <asp:ImageButton ID="imgBtnBlastFieldsConfig2" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgBtnBlastFieldsConfig2_Click" Visible="true"/>
                                        </td>
                                        <td class="label" align="left" width="254">
                                             <asp:DropDownList ID="drpBlastField2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBlastField2_SelectedIndexChanged" CssClass="styled-select" DataValueField="Value" DataTextField="Value" >
                                            </asp:DropDownList>                                          
                                        </td>
                                        <td>
                                              <asp:TextBox ID="txtBlastField2" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="top" align='left' width="200">  
                                            <asp:Label ID="lblBlastField3" runat="server" Text="Field3"></asp:Label>&nbsp;
                                             <asp:ImageButton ID="imgBtnBlastFieldsConfig3" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgBtnBlastFieldsConfig3_Click" Visible="true"/>
                                        </td>
                                        <td class="label" align="left" width="254">
                                             <asp:DropDownList ID="drpBlastField3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBlastField3_SelectedIndexChanged" CssClass="styled-select" DataValueField="Value" DataTextField="Value" >
                                            </asp:DropDownList>                                         
                                        </td>
                                        <td>
                                               <asp:TextBox ID="txtBlastField3" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="top" align='left' width="200">
                                             <asp:Label ID="lblBlastField4" runat="server" Text="Field4"></asp:Label>&nbsp;
                                             <asp:ImageButton ID="imgBtnBlastFieldsConfig4" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgBtnBlastFieldsConfig4_Click" Visible="true"/>
                                        </td>
                                        <td class="label" align="left" width="254">
                                             <asp:DropDownList ID="drpBlastField4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBlastField4_SelectedIndexChanged" CssClass="styled-select" DataValueField="Value" DataTextField="Value" >
                                            </asp:DropDownList>                                            
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBlastField4" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="top" align='left' width="200">
                                             <asp:Label ID="lblBlastField5" runat="server" Text="Field5"></asp:Label>&nbsp;
                                             <asp:ImageButton ID="imgBtnBlastFieldsConfig5" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgBtnBlastFieldsConfig5_Click" Visible="true"/>
                                        </td>
                                        <td class="label" align="left" width="254">
                                            <asp:DropDownList ID="drpBlastField5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBlastField5_SelectedIndexChanged" data CssClass="styled-select" DataValueField="Value" DataTextField="Value" >
                                            </asp:DropDownList>                                          
                                        </td>
                                        <td>
                                              <asp:TextBox ID="txtBlastField5" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>


<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupBlastFieldsConfig" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlBlastFieldsConfig" TargetControlID="btnShowPopup1">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlBlastFieldsConfig" CssClass="modalPopup">
<asp:UpdateProgress ID="upBlastFieldsConfigProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upBlastFieldsConfig" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upBlastFieldsConfigProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upBlastFieldsConfigProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
   <asp:UpdatePanel ID="upBlastFieldsConfig" runat="server">
        <ContentTemplate>
        <uc1:BlastFieldsConfig ID="BlastFieldsConfig1" runat="server" />
        <table align="center" class="style1">
            <tr>
            <td style="text-align: right" >
                <asp:Button runat="server" Text="Save" ID="btnBlastFieldsConfigSave" CssClass="formfield"
                    OnClick="btnBlastFieldsConfig_Save"></asp:Button>
            </td>
                 <td style="text-align: left">
                    <asp:Button runat="server" Text="Cancel" ID="btnBlastFieldsConfigCancel"  OnClick="btnBlastFieldsConfig_Cancel" CssClass="formfield"></asp:Button>
                </td>
            </tr>
        </table>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>