<%@ Page Title="" Language="C#" EnableEventValidation="true" MasterPageFile="~/MasterPages/Communicator.Master" CodeBehind="SentCampaignsReport.aspx.cs" Inherits="ecn.communicator.main.Reports.SentCampaignsReport" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Src="~/main/ECNWizard/Controls/WizardSentCampaigns.ascx" TagName="SentCampaigns" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="upMain" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <uc1:SentCampaigns ID="SentCampaigns" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
