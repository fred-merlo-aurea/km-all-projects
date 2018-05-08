<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_RelatedPubs.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_RelatedPubs"
    Title="KMPS Form Builder - Related Pubs" %>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage Related Publications">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="PUBS" />
                        </td>
                        <td width="2%">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Related Publications">
                                <table style="width: 100%;" border="0">
                                    <tr>
                                        <td style="text-align: center">
                                            <b>PUBS</b><br />
                                            <br />
                                            <asp:ListBox ID="lstSource" runat="server" Width="225px" Height="200px" SelectionMode="Multiple"
                                                DataSourceID="SqlDataSourcePNonRelatedPubsConnect"  DataTextField="PubName" DataValueField="PubId">
                                            </asp:ListBox>
                                            <asp:SqlDataSource ID="SqlDataSourcePNonRelatedPubsConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                SelectCommand="sp_PublicationNonRelationPub" OnSelecting="SqlDataSourcePNonRelatedPubsConnect_Onselecting" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                        Type="Int32" />                                                      
                                                    <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                    <asp:Parameter Name="ECNcustomerID" Type="String"/>
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Button ID="btnAdd" runat="server" Text=">>" OnClick="btnAdd_Click" CssClass="buttonSmall" /><br />
                                            <asp:Button ID="btnRemove" runat="server" Text="<<" OnClick="btnRemove_Click" CssClass="buttonSmall" />
                                        </td>
                                        <td style="text-align: center">
                                            <b>Selected PUBS</b><br />
                                            <br />
                                            <asp:ListBox ID="lstDestination" runat="server" Width="225px" Height="200px" SelectionMode="Multiple"
                                                DataSourceID="SqlDataSourcePRelatedPubsConnect" DataTextField="PubName" DataValueField="PubId">
                                            </asp:ListBox>
                                            <asp:SqlDataSource ID="SqlDataSourcePRelatedPubsConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                SelectCommand="sp_PublicationRelationPubs" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                        Type="Int32" />
                                                    <asp:Parameter Name="LinkedToPubID" Type="Int32" DefaultValue="0" />
                                                    <asp:Parameter Name="IsActive" Type="Int32" DefaultValue="1" />
                                                    <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                    <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center">
                                            <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label><br />
                                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" />
                                        </td>
                                    </tr>
                                </table>
                            </JF:BoxPanel>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
