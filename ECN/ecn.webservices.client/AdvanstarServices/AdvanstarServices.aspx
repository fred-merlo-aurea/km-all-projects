<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" MasterPageFile="~/Site.master" CodeBehind="AdvanstarServices.aspx.cs" Inherits="ecn.webservices.client.AdvanstarServices.AdvanstarServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/AdvanstarServices/Login.ascx" TagName="Login" TagPrefix="uc3" %>
   
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
                <Header><a href="" class="accordionLink">1. Login</a></Header>
                <Content>
                     <p>
                        <uc3:Login ID="Login" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>               
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>

