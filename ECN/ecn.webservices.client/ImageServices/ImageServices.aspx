<%@ Page Title="Home Page" Language="C#" validateRequest="false" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ImageServices.aspx.cs" Inherits="ecn.webservices.client.ImageServices.ImageServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/ImageServices/GetFolders.ascx" TagName="GetFolders" TagPrefix="uc1" %>
<%@ Register Src="~/ImageServices/AddFolder.ascx" TagName="AddFolder" TagPrefix="uc2" %>
<%@ Register Src="~/ImageServices/GetImages.ascx" TagName="GetImages" TagPrefix="uc3" %>
<%@ Register Src="~/ImageServices/AddImage.ascx" TagName="AddImage" TagPrefix="uc4" %>


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
                <Header><a href="" class="accordionLink">1. Get Folders</a></Header>
                <Content>
                   <p>
                        <uc1:GetFolders ID="GetFolders" runat="server" />
                   </p>                    
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header><a href="" class="accordionLink">2. Add Folder</a></Header>
                <Content>
                   <p>
                        <uc2:AddFolder ID="AddFolder" runat="server" />
                   </p>                    
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header><a href="" class="accordionLink">3. Get Images</a></Header>
                <Content>
                   <p>
                        <uc3:GetImages ID="GetImages" runat="server" />
                   </p>                    
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header><a href="" class="accordionLink">4. Add Image</a></Header>
                <Content>
                   <p>
                        <uc4:AddImage ID="AddImage" runat="server" />
                   </p>                    
                </Content>
            </ajaxToolkit:AccordionPane> 
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>

