<%@ Page Language="c#" CodeBehind="domainTrackerList.aspx.cs" Inherits="ecn.communicator.main.lists.domainTrackerList" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <script type="text/javascript">
        function pageLoad() {
            var hf = $get('<%= hfShowScriptPopup.ClientID %>');
            if (hf.value == "1") {
                var modpop = $find("modPop");
                if (!modpop) { return; }
                modpop.show();
            }
            if (hf.value == "0") {
                var modpop = $find("modPop");
                if (!modpop) { return; }
                modpop.hide();
            }
        }
    </script>--%>

    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="lblDomainTracker" runat="server" Text="Domain Tracking List" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New Domain" class="formbuttonsmall" OnClick="btnAdd_Click" />
            </td>
        </tr>

    </table>

    <br />
    <ecnCustom:ecnGridView ID="gvDomainTracker" runat="server" AllowSorting="false" AutoGenerateColumns="false" CssClass="grid"
        Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="DomainTrackerID"
        OnRowCommand="gvDomainTracker_RowCommand" OnRowDataBound="GVDomainTracker_RowDataBound">
        <HeaderStyle CssClass="gridheader"></HeaderStyle>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblDomainTrackerID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.DomainTrackerID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="35%" HeaderText="Domain">
                <ItemTemplate>
                    <asp:Label ID="lblDomain" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Domain") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="35%" HeaderText="TrackerKey">
                <ItemTemplate>
                    <asp:Label ID="lblTrackerKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TrackerKey") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" HeaderText="Script">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnGenerateTrackingScript" ImageUrl="/ecn.communicator/images/script_code_red.png" CommandName="GenerateScript" CausesValidation="false" CommandArgument='<%#DataBinder.Eval(Container,"DataItem.TrackerKey") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" HeaderText="Users">
                <ItemTemplate>
                    <a href='DomainTrackerUsers.aspx?DomainTrackerID=<%# DataBinder.Eval(Container.DataItem, "DomainTrackerID") %>'>
                        <center>
                            <img src="/ecn.communicator/images/icon-users.png" alt='View Users' border='0'></center>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" HeaderText="Reports">
                <ItemTemplate>
                    <a href='DomainTrackerReport.aspx?DomainTrackerID=<%# DataBinder.Eval(Container.DataItem, "DomainTrackerID") %>'>
                        <center>
                            <img src="/ecn.images/images/icon-reports.gif" alt='View Reports' border='0'></center>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" HeaderText="Edit">
                <ItemTemplate>
                    <a href='DomainTrackerEdit.aspx?DomainTrackerID=<%# DataBinder.Eval(Container.DataItem, "DomainTrackerID") %>'>
                        <center>
                            <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Domain Information' border='0'></center>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                        CommandName="DomainDelete" OnClientClick="return confirm('Are you sure you want to delete tracking for this Domain?')"
                        CausesValidation="false" CommandArgument='<%#Eval("DomainTrackerID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </ecnCustom:ecnGridView>

    <br />
    <asp:HiddenField ID="hfShowScriptPopup" Value="0" runat="server" />
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />;
    <ajaxToolkit:ModalPopupExtender ID="mpeGenerateScript" PopupControlID="panelScript" BackgroundCssClass="modalBackground" TargetControlID="btnShowPopup" runat="server" BehaviorID="modPop" />
    <asp:Panel runat="server" ID="panelScript" CssClass="modalPopup">
<%--        <asp:UpdatePanel ID="upnlGenerateScript" CssClass="modalPopup" ChildrenAsTriggers="true" Height="300px" Width="600px" runat="server">
            <ContentTemplate>--%>
                <table style="width: 600px; height: 300px; background-color: white; border-radius: 10px; padding: 10px;">
                    <tr>
                        <td style="text-align: left; height: 10%; font-size: medium;">Domain Tracking Script
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding-left: 10px; height: 10%;">
                            <asp:RadioButtonList ID="rblProtocol" runat="server" OnSelectedIndexChanged="rblProtocol_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="HTTP" Value="HTTP" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="HTTPS" Value="HTTPS" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding-left: 10px; height: 10%;">
                            <asp:RadioButtonList ID="rblJQuery" runat="server" OnSelectedIndexChanged="rblJQuery_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Page with jQuery" Selected="True" Value="with" />
                                <asp:ListItem Text="Page without jQuery" Value="without" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; height: 25%;">
                            <asp:TextBox ID="txtDomainScript" Width="98%" Wrap="true" Height="150px" TextMode="MultiLine" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: center; height: 20%;">
                            <asp:Button ID="btnCloseScript" runat="server" UseSubmitBehavior="true" AutoPostBack="true" CausesValidation="false" Text="Close" OnClick="btnCloseScript_Click" />
                        </td>
                    </tr>
                </table>
<%--            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
</asp:Content>
