<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.ATHBimportDatafromFile"
    CodeBehind="ATHBimportDatafromFile.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
                                    <tr>
                                        <td id="errorTop">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorMiddle">
                                            <table height="67" width="80%">
                                                <tr>
                                                    <td valign="top" align="center" width="20%">
                                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                                    </td>
                                                    <td valign="middle" align="left" width="80%" height="100%">
                                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorBottom">
                                        </td>
                                    </tr>
                                </table>
    </asp:PlaceHolder> <br />
    <asp:Label ID="lblGUID" runat="server" Text="" Visible="false"></asp:Label>
     <asp:Label ID="lblGroupName" runat="server" Text="" Visible="true" Font-Size="Small" Font-Bold="true"></asp:Label> <br /> <br />
    
     <asp:Label ID="lblFileName" runat="server" Text="" Visible="true" Font-Size="Small" Font-Bold="true"></asp:Label> <br />
     <br />
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <asp:Label ID="errlabel" runat="Server" Visible="false" CssClass="errormsg"></asp:Label><asp:Label
                ID="msglabel" runat="Server" Visible="false" CssClass="TableHeader"></asp:Label>
            <td align="center">
                <br />
                <table id="dataCollectionTable" cellspacing="1" cellpadding="3" align="center" border='0'
                    runat="Server">
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvImport" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" Width="100%"  OnRowCommand="gvImport_Command"
                OnRowDataBound="gvImport_OnRowDataBound">
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                            HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Totals" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="true">
                                    <ItemTemplate>
                                     <asp:Label runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.ActionCode") %> ID="lblActionCode" Visible="false"></asp:Label> 
                                     <asp:Label runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.Totals") %> ID="lblTotals" Visible="false"></asp:Label>                             
                                        <asp:LinkButton runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.Totals") %> CausesValidation="false" ID="lnkTotals" CommandName="DownloadEmails" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActionCode") %>'  Visible="false"></asp:LinkButton>
                                    </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td valign="middle" align="center" height="50">
                <asp:Button ID="ImportButton" OnClick="ImportData" runat="Server" Visible="true"
                    CssClass="formbutton" Text="Import Data"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
