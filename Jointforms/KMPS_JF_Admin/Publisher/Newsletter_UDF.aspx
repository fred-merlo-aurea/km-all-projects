<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Newsletter_UDF.aspx.cs"
    Inherits="KMPS_JF_Setup.Publisher.Newsletter_UDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="script1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage UDF's">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:GridView ID="grdNewsletterUDF" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                Width="100%" AllowPaging="true" DataKeyNames="GroupDatafieldsID" DataSourceID="SqlDataSourceGDF"
                                OnRowCommand="grdNewsletterUDF_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="LongName" HeaderText="Description" SortExpression="UDFDesc"
                                        ItemStyle-Width="60%" />
                                    <asp:BoundField DataField="Shortname" HeaderText="Code Snippet" ItemStyle-Width="20%"
                                        DataFormatString="%%{0}%%" />
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
                                SelectCommand="select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupID = @groupID and IsDeleted=0"
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
                                            Copy from Publication UDF
                                        </td>
                                        <td style="text-align: left; width: 75%">
                                            <asp:DropDownList ID="drpPubUDFs" runat="server" Width="250px" 
                                                AutoPostBack="true" onselectedindexchanged="drpPubUDFs_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            UDF Name
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:HiddenField ID="hfldGDFId" runat="server" />
                                            <asp:TextBox ID="txtShortName" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqShortName" runat="server" ControlToValidate="txtShortName"
                                                ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Long Description
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLongName" runat="server" MaxLength="255" Width="250px" Rows="3" ></asp:TextBox>
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
                                        <td colspan="2" style="text-align: right">
                                            <asp:Button ID="btnAddUDF" runat="server" Text="SAVE" OnClick="btnAddUDF_Click" CssClass="button" />
                                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click" CssClass="button" />
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
    </form>
</body>
</html>
