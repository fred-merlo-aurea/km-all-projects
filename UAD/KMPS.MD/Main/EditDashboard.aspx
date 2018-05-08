<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Site.Master"  CodeBehind="EditDashboard.aspx.cs" Inherits="KMPS.MD.Main.EditDashboard" %>

<%@ Register src="~/Main/DashboardEditor/Editor.ascx" tagname="Editor" tagprefix="uc1" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">	   
    <div style="font-family: Calibri">    
        <uc1:Editor ID="ctlEditor" runat="server" />
   </div>
</asp:Content>