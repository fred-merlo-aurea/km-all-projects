<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WATTAPI.aspx.cs" Inherits="ecn.webservices.client.WATTAPI.WATTAPI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/WATTAPI/GetTokenForSubscriber.ascx" TagName="GetTokenForSubscriber" TagPrefix="watt" %>
<%@ Register Src="~/WATTAPI/GetIssueURL.ascx" TagName="GetIssueURL" TagPrefix="watt" %>
<%@ Register Src="~/WATTAPI/GetNextTokenForSubscriber.ascx" TagName="GetNextTokenForSubscriber" TagPrefix="watt" %>
<%@ Register Src="~/WATTAPI/SubscriberExists.ascx" TagName="SubscriberExists" TagPrefix="watt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="apAddSolicitationFilter" runat="server">
                <Header>
                    <a href="" class="accordionLink">1. Get Token For Subscriber</a>
                </Header>
                <Content>
                    <p>
                        <watt:GetTokenForSubscriber ID="GetTokenForSubscriber" runat="server" />

                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="apGetIssueURL" runat="server">
                <Header>
                    <a href="" class="accordionLink">2. Get Issue URL</a>
                </Header>
                <Content>
                    <p>
                        <watt:GetIssueURL ID="GetIssueURL" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="apGetNextTokenForSubscriber" runat="server">
                <Header>
                    <a href="" class="accordionLink">3. Get Next Token For Subscriber</a>
                </Header>
                <Content>
                    <p>
                        <watt:GetNextTokenForSubscriber ID="GetNextTokenForSubscriber" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>

            <ajaxToolkit:AccordionPane ID="apSubscriberExists" runat="server">
                <Header>
                    <a href="" class="accordionLink">4. Subscriber Exists</a>
                </Header>
                <Content>
                    <p>
                        <watt:SubscriberExists ID="SubscriberExists" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>
