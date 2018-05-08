<%@ Page Title="Home Page" Language="C#" validateRequest="false" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ContentServices.aspx.cs" Inherits="ecn.webservices.client.ContentServices.ContentServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/ContentServices/GetContentFolders.ascx" TagName="GetContentFolders" TagPrefix="uc1" %>
<%@ Register Src="~/ContentServices/GetTemplates.ascx" TagName="GetTemplates" TagPrefix="uc2" %>
<%@ Register Src="~/ContentServices/GetCustomerDepts.ascx" TagName="GetCustomerDepts" TagPrefix="uc3" %>
<%@ Register Src="~/ContentServices/AddContentToFolder.ascx" TagName="AddContentToFolder" TagPrefix="uc4" %>
<%@ Register Src="~/ContentServices/AddMessageToFolder.ascx" TagName="AddMessageToFolder" TagPrefix="uc5" %>
<%@ Register Src="~/ContentServices/DeleteFolder.ascx" TagName="DeleteFolder" TagPrefix="uc6" %>
<%@ Register Src="~/ContentServices/AddFolder.ascx" TagName="AddFolder" TagPrefix="uc7" %>
<%@ Register Src="~/ContentServices/UpdateContent.ascx" TagName="UpdateContent" TagPrefix="uc8" %>
<%@ Register Src="~/ContentServices/DeleteContent.ascx" TagName="DeleteContent" TagPrefix="uc9" %>
<%@ Register Src="~/ContentServices/DeleteMessage.ascx" TagName="DeleteMessage" TagPrefix="uc10" %>
<%@ Register Src="~/ContentServices/UpdateMessage.ascx" TagName="UpdateMessage" TagPrefix="uc11" %>
<%@ Register Src="~/ContentServices/PreviewContent.ascx" TagName="PreviewContent" TagPrefix="uc12" %>
<%@ Register Src="~/ContentServices/PreviewMessage.ascx" TagName="PreviewMessage" TagPrefix="uc13" %>
<%@ Register Src="~/ContentServices/SearchForContent.ascx" TagName="SearchForContent" TagPrefix="uc14" %>
<%@ Register Src="~/ContentServices/GetContent.ascx" TagName="GetContent" TagPrefix="uc15" %>
<%@ Register Src="~/ContentServices/SearchForLayout.ascx" TagName="SearchForLayout" TagPrefix="uc16" %>
<%@ Register Src="~/ContentServices/GetLayout.ascx" TagName="GetLayout" TagPrefix="uc17" %>
<%@ Register Src="~/ContentServices/GetLayoutByFolder.ascx" TagName="GetLayoutByFolder" TagPrefix="uc18" %>
<%@ Register Src="~/ContentServices/GetContentByFolderID.ascx" TagName="GetContentByFolderID" TagPrefix="uc19" %>
    
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
        ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40" 
        TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                <Header><a href="" class="accordionLink">1. Add Message To Folder</a></Header>
                <Content>
                     <p>
                        <uc5:AddMessageToFolder ID="AddMessageToFolder" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header><a href="" class="accordionLink">2. Add Content To Folder</a></Header>
                <Content>
                     <p>
                        <uc4:AddContentToFolder ID="AddContentToFolder" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>             
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header><a href="" class="accordionLink">3. Get Content Folders</a></Header>
                <Content>
                     <p>
                        <uc1:GetContentFolders ID="GetContentFolders" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header><a href="" class="accordionLink">4. Get Templates</a></Header>
                <Content>
                     <p>
                        <uc2:GetTemplates ID="GetTemplates" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header><a href="" class="accordionLink">5. Get Customer Departments</a></Header>
                <Content>
                     <p>
                        <uc3:GetCustomerDepts ID="GetCustomerDepts" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>    
            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                <Header><a href="" class="accordionLink">6. Delete Content Folder</a></Header>
                <Content>
                     <p>
                        <uc6:DeleteFolder ID="DeleteFolder" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane7" runat="server">
                <Header><a href="" class="accordionLink">7. Add Content Folder</a></Header>
                <Content>
                     <p>
                        <uc7:AddFolder ID="AddFolder" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane8" runat="server">
                <Header><a href="" class="accordionLink">8. Update Content</a></Header>
                <Content>
                     <p>
                        <uc8:UpdateContent ID="UpdateContent" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane9" runat="server">
                <Header><a href="" class="accordionLink">9. Delete Content</a></Header>
                <Content>
                     <p>
                        <uc9:DeleteContent ID="DeleteContent" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane10" runat="server">
                <Header><a href="" class="accordionLink">10. Delete Message</a></Header>
                <Content>
                     <p>
                        <uc10:DeleteMessage ID="DeleteMessage" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane11" runat="server">
                <Header><a href="" class="accordionLink">11. Update Message</a></Header>
                <Content>
                     <p>
                        <uc11:UpdateMessage ID="UpdateMessage" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>    
            <ajaxToolkit:AccordionPane ID="AccordionPane12" runat="server">
                <Header><a href="" class="accordionLink">12. Preview Content</a></Header>
                <Content>
                     <p>
                        <uc12:PreviewContent ID="PreviewContent" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane13" runat="server">
                <Header><a href="" class="accordionLink">13. Preview Message</a></Header>
                <Content>
                     <p>
                        <uc13:PreviewMessage ID="PreviewMessage" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane14" runat="server">
                <Header><a href="" class="accordionLink">14. Search For Content</a></Header>
                <Content>
                     <p>
                        <uc14:SearchForContent ID="SearchForContent" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane15" runat="server">
                <Header><a href="" class="accordionLink">15. Get Content</a></Header>
                <Content>
                     <p>
                        <uc15:GetContent ID="GetContent" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>    
            <ajaxToolkit:AccordionPane ID="AccordionPane16" runat="server">
                <Header><a href="" class="accordionLink">16. Search For Messages</a></Header>
                <Content>
                     <p>
                        <uc16:SearchForLayout ID="SearchForLayout" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>   
            <ajaxToolkit:AccordionPane ID="AccordionPane17" runat="server">
                <Header><a href="" class="accordionLink">17. Get Message</a></Header>
                <Content>
                     <p>
                        <uc17:GetLayout ID="GetLayout" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>   
            <ajaxToolkit:AccordionPane ID="AccordionPane18" runat="server">
                <Header><a href="" class="accordionLink">18. Get Message List By Folder</a></Header>
                <Content>
                     <p>
                        <uc18:GetLayoutByFolder ID="GetLayoutByFolder" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>                
            <ajaxToolkit:AccordionPane ID="AccordionPane19" runat="server">
                <Header><a href="" class="accordionLink">19. Get Content List By Folder</a></Header>
                <Content>
                     <p>
                        <uc19:GetContentByFolderID ID="GetContentByFolderID" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>

