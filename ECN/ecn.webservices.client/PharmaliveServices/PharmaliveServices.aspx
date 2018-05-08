<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" MasterPageFile="~/Site.master" CodeBehind="PharmaliveServices.aspx.cs" Inherits="ecn.webservices.client.PharmaliveServices.PharmaliveServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/PharmaliveServices/GetSubscribedNewsletters.ascx" TagName="GetSubscribedNewsletters" TagPrefix="uc1" %>
   
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
        ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40" 
        TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header><a href="" class="accordionLink">1. Get Subscribed Newsletters</a></Header>
                <Content>
                     <p>
                        <uc1:GetSubscribedNewsletters ID="GetSubscribedNewsletters" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>               
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>

