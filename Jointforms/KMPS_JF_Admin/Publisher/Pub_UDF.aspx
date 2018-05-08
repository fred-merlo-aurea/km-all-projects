<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_UDF.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_UDF" Title="KMPS Form Builder - UDF" %>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage UDF's">
                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="UDF" />
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdPublisherUDF" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            Width="100%" AllowPaging="true" DataKeyNames="GroupDatafieldsID" DataSourceID="SqlDataSourceGDF"
                                            OnRowCommand="grdPublisherUDF_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="LongName" HeaderText="Description" SortExpression="LongName"
                                                    ItemStyle-Width="60%"   headerstyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="Shortname" HeaderText="Code Snippet" ItemStyle-Width="20%"
                                                    DataFormatString="%%{0}%%"   headerstyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="IsPublic" HeaderText="IsPublic" ReadOnly="true" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnUDFEdit" runat="server" ImageUrl="~/images/icon-edit.gif"
                                                            CommandName="UDFEdit" CausesValidation="false" CommandArgument='<%# Eval("GroupDatafieldsID") %>' />
                                                        &nbsp;
                                                        <asp:ImageButton ID="imgbtnUDFDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                            CommandName="UDFDelete" CommandArgument='<%# Eval("GroupDatafieldsID") %>' OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourceGDF" runat="server" ConnectionString="<%$ ConnectionStrings:ecn5_communicator %>"
                                            SelectCommand="select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupID = @groupID and  IsDeleted=0 and isnull(datafieldSetID,0) = 0"
                                            SelectCommandType="Text" OnSelecting="SqlDataSourceGDF_Selecting">
                                            <SelectParameters>
                                                <asp:Parameter Name="GroupID" Type="Int32" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: left">
                                        <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Add UDF">
                                            <table style="width: 90%;" border="0px" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left; width: 25%">
                                                        Copy from Existing
                                                    </td>
                                                    <td style="text-align: left; width: 75%">
                                                        <asp:DropDownList ID="drpUDFs" runat="server" Width="250px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="drpUDFs_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 20%">
                                                        UDF Name
                                                    </td>
                                                    <td style="text-align: left; width: 70%">
                                                        <asp:HiddenField ID="hfldGDFId" runat="server" />
                                                        <asp:TextBox ID="txtShortName" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqShortName" runat="server" ControlToValidate="txtShortName"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Long Description
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLongName" runat="server" MaxLength="255" Width="350px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqLongName" runat="server" ControlToValidate="txtLongName"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Is Public
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsPublic" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: left">
                                                        <asp:Button ID="btnAddUDF" runat="server" Text="SAVE" OnClick="btnAddUDF_Click" CssClass="button" />&nbsp;&nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                                            CausesValidation="false" CssClass="button" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </JF:BoxPanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
