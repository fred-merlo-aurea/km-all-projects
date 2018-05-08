<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" validateRequest="false" AutoEventWireup="true" CodeBehind="SaversAPI.aspx.cs" Inherits="ecn.webservices.client.SaversAPI.SaversAPI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/SaversAPI/AddSolicitationFilter.ascx" TagName="AddSolicitationFilter" TagPrefix="savers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .accordionLink
        {
            margin: 0px;
            padding: 0px;
            color: blue;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxtoolkit:Accordion id="MyAccordion" runat="server" selectedindex="0" headercssclass="accordionHeader"
        headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
        fadetransitions="false" framespersecond="40" transitionduration="250" autosize="None"
        requireopenedpane="false" suppressheaderpostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="apAddSolicitationFilter" runat="server">
                <Header>
                <a href="" class="accordionLink">1. Add Solicitation Filter</a></Header>
                <Content>
                    <p>
                        <savers:AddSolicitationFilter ID="AddSolicitationFilter" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>
