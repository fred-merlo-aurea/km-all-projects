<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfintegration.aspx.cs"
    Inherits="ecn.communicator.main.lists.sfintegration" EnableViewState="true" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .errorMsg
        {
            background: #cccccc;
            border: 2px solid #7b95a2;
            border-width: 4px 2px;
            font-size: medium;
            font-family: Times New Roman;
            text-align: left;
            width: 250px;
            height: 150px;
        }
        .addNewGroup
        {
            background: #cccccc;
            border: 2px solid #7b95a2;
            border-width: 4px 2px;
        }
        .modalBackground
        {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .radiobtn_list label
        {
            display: block;
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs 
            var inputElements = document.getElementsByTagName('input');

            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];

                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {

                    // Use the invoker (our calling element) as the reference 
                    //  for our checkbox status
                    myElement.checked = invoker.checked;
                }
            }
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSFLogin">
                <table width="100%">
                    <thead>
                        <caption>
                            Sales Force Login</caption>
                    </thead>
                    <tr>
                        <th align="right" width="45%">
                            User Name:
                        </th>
                        <td align="left">
                            <asp:TextBox runat="server" ID="tbUserName" ToolTip="Enter your SaleForce user name."></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="right">
                            Password:
                        </th>
                        <td align="left">
                            <asp:TextBox runat="server" ID="tbPassword" ToolTip="This is your SalesForce password followed by your SECURITY TOKEN"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="right">
                            <asp:Button runat="server" ID="btnLogin" Text="Login" OnClick="btnLogin_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlFeatures" Visible="false">
                <asp:RadioButtonList runat="server" AutoPostBack="true" ID="rblImportExport" Visible="true"
                    OnSelectedIndexChanged="rblImportExport_SelectedIndexChanged" TextAlign="Right">
                    <asp:ListItem Text="Export CONTACTS from ECN to SalesForce.com" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Import CONTACTS from SalesForce.com to ECN" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Export CAMPAIGN DATA from ECN to SalesForce.com" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlECNtoSF" Visible="false">
                <table>
                    <tr>
                        <th>
                            Select ECN Group:
                        </th>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlECNGroups" OnSelectedIndexChanged="ddlECNGroups_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <asp:Button runat="server" ID="btnSendToSF" Text="Send to SF" OnClick="btnSendToSF_Click" />
                    </tr>
                </table>
                <br />
                <asp:GridView runat="server" ID="gvECNContacts" AutoGenerateColumns="true" ShowHeader="true"
                    ShowFooter="true" CssClass="gridWizard">
                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrowWizard" />
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                    <Columns>
                        <asp:TemplateField>
                            <AlternatingItemTemplate>
                                <asp:CheckBox ID="cbRowItem" runat="server" />
                            </AlternatingItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbRowItem" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbAllRowItem" runat="server" Text="Select All" OnClick="selectAll(this)" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <AU:PagerBuilder ID="pagerECNContacts" runat="Server" Width="100%" PageSize="50"
                    ControlToPage="gvECNContacts" OnIndexChanged="pagerECNContacts_IndexChanged">
                    <PagerStyle CssClass="gridpager"></PagerStyle>
                </AU:PagerBuilder>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSFtoECN" Visible="false">
                <b>Select SalesForce Account:</b><asp:DropDownList runat="server" ID="ddlSFAccounts"
                    OnSelectedIndexChanged="ddlSFAccounts_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <b>Select ECN destination Group:</b><asp:DropDownList runat="server" ID="ddlECNGroupFromSF"
                    OnSelectedIndexChanged="ddlECNGroupFromSF_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnImportToECN" Text="Import to ECN" OnClick="btnImportToECN_Click"
                    Enabled="false" />
                <br />
                <asp:GridView runat="server" ID="gvSFContacts" AutoGenerateColumns="true" ShowHeader="true"
                    ShowFooter="true" CssClass="gridWizard">
                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrowWizard" />
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                    <Columns>
                        <asp:TemplateField>
                            <AlternatingItemTemplate>
                                <asp:CheckBox ID="cbRowItem" runat="server" />
                            </AlternatingItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbRowItem" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbAllRowItem" runat="server" Text="Select All" OnClick="selectAll(this)" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlCampaignData" Visible="false">
                <table>
                    <tr align="left">
                        <th align="right">
                            ECN Email Blast:
                        </th>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEcnEmailBlast" AutoPostBack="true" OnSelectedIndexChanged="ddlEcnEmailBlast_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <b>- UPLOAD RESULTS TO -</b>
                        </td>
                    </tr>
                    <tr align="left">
                        <th align="right">
                            SalesForce Campaign:
                        </th>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSFCampaigns" AutoPostBack="true" Enabled="false"
                                OnSelectedIndexChanged="ddlSFCampaigns_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button runat="server" ID="btnCampaignDataUpload" Enabled="false" Text="Upload Results"
                                OnClientClick="alert('Processing data....please wait for confirmation of completion.'); return true;"
                                OnClick="btnCampaignDataUpload_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlNewSfCampaign" CssClass="addNewGroup">
                <table>
                    <tr>
                        <th>
                            Campaign Name:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampName"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Description:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampDesc"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Budgeted Cost:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampBudget"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Expected Response:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampExpResp"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Expected Revenue:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampExpRev"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Start Date:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampStart"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbCampStart"
                                Format="MM/dd/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            End Date:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="tbCampEnd"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbCampEnd"
                                Format="MM/dd/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCampOK" Text="Enter" OnClick="btnCampOK_Click"
                                CausesValidation="false" UseSubmitBehavior="false" />
                        </td>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCampCancel" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="mpeNewSfCampaign" runat="server" PopupControlID="pnlNewSfCampaign"
                OkControlID="btnCampOK" CancelControlID="btnCampCancel" TargetControlID="hdnNewSfCampaign"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>
            <asp:Button ID="hdnNewSfCampaign" runat="server" style="display:none;" />
            <asp:Panel runat="server" ID="pnlNewEcnGroup" CssClass="addNewGroup">
                <table>
                    <tr>
                        <td align="right">
                            <b>Group Name:</b>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbNewGroupName"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Group Description:</b>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbNewGroupDesc"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCreateNewGroup" Text="Enter" OnClick="btnCreateNewGroup_Click"
                                CausesValidation="false" UseSubmitBehavior="false" />
                        </td>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCancelNewGroup" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="mpeNewEcnGroup" runat="server" PopupControlID="pnlNewEcnGroup"
                OkControlID="btnCreateNewGroup" CancelControlID="btnCancelNewGroup" TargetControlID="hdnNewEcnGroup"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>
            <asp:Button ID="hdnNewEcnGroup" runat="server" style="display:none;" />
            <asp:Panel ID="pnlMessage" runat="server" CssClass="errorMsg">
                <div style="text-align: center; vertical-align: middle; margin-bottom: 10px; margin-top: 10px">
                    <asp:Label runat="server" ID="lbMessage" Height="100px" Width="200px"></asp:Label>
                    <br />
                    <asp:Button ID="btnMessageOK" runat="server" Text="Close" />
                </div>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="mpeMessages" runat="server" PopupControlID="pnlMessage"
                OkControlID="btnMessageOK" TargetControlID="hdnMessage" BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>
            <asp:Button ID="hdnMessage" style="display:none;" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
