<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.lists_main" CodeBehind="default.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register src="~/main/ECNWizard/Group/groupExplorer.ascx" tagname="groupExplorer" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
         <uc1:groupExplorer ID="groupExplorer1" runat="server" />
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

