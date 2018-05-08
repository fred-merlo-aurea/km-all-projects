<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Marketo.ascx.cs" Inherits="KMPS.MD.Controls.Marketo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td valign="top">
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td align="left" colspan="2">
                        <b>Marketo Host</b>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">BaseURL
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMarketoBaseURL" runat="server" Width="230px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMarketoBaseURL" runat="server" ControlToValidate="txtMarketoBaseURL" ValidationGroup="Export"
                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">ClientID 
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMarketoClientID" runat="server" Width="230px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMarketoClientID" runat="server" ControlToValidate="txtMarketoClientID" ValidationGroup="Export"
                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">ClientSecret
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMarketoClientSecret" runat="server" Width="230px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMarketoClientSecret" runat="server" ControlToValidate="txtMarketoClientSecret" ValidationGroup="Export"
                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">Partition</td>
                    <td align="left">
                        <asp:TextBox ID="txtMarketoPartition" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td align="left">
            <br />
            <br />
            <img id="ImgMarketo" style="padding: 0px 0 0 0;" src="~/images/ic-Marketo.gif" runat="server"
                alt="" />
            <br />
            <asp:Button ID="btnTestConnection" runat="server" Text="Test Connection" CssClass="buttonMedium" OnClick="btnTestConnection_Click" />
            <br />
            <asp:Label ID="lblTestConnErrorMessage" runat="server" Text="" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<table cellpadding="5" cellspacing="5" border="0" style="width: 800px;">
    <tr valign="top">
        <td style="text-align: left;">

            <table cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td align="left">
                        <b>Marketo Mapping</b>
                    </td>
                </tr>
                <tr>
                    <td>Marketo Lists 
                        <asp:DropDownList ID="ddlMarketoList" runat="server" Visible="true" DataTextField="name" DataValueField="id" Width ="180px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtMarketoNames" runat="server" Width="250px"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Button ID="btnSearchMarketoList" runat="server" CssClass="button" Text="Search" OnClick="btnSearchMarketoList_Click" />
                        <asp:Label ID="lblTestErrMsg" runat="server" Text="" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td><b>Email required for Marketo export</b></br></td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 100%;">Name
                                        <asp:TextBox ID="txtQSName" runat="server" MaxLength="50" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtQSName" runat="server" ControlToValidate="txtQSName" ValidationGroup="SaveMappingField"
                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                &nbsp;Value
                                        <asp:DropDownList ID="ddlQSValue" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlQSValue_SelectedIndexChanged">
                                        </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvdrpQSValue" runat="server" ControlToValidate="ddlQSValue" ValidationGroup="SaveMappingField"
                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    <asp:PlaceHolder ID="phCustomValue" runat="server" Visible="false">
                                    &nbsp; 
                                            <asp:TextBox ID="txtQSValue" runat="server" MaxLength="200" Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtQSValue" runat="server" ControlToValidate="txtQSValue" ValidationGroup="SaveMappingField" 
                                        ErrorMessage=" *" ForeColor="red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </asp:PlaceHolder>
                                &nbsp;
            
                                        <asp:Button ID="btnAddHttpPostURL" runat="server" CssClass="button" Text="Add" OnClick="btnAddHttpPostURL_Click" ValidationGroup="SaveMappingField" />

                                <br />
                                <asp:Label ID="txtErrMsg" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 100%">
                                <asp:GridView ID="gvHttpPost" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                    Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="HttpPostParamsID"
                                    OnRowCommand="gvHttpPost_RowCommand">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHttpPostParamsID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.HttpPostParamsID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParamName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParamValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamValue")  %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblParamDisplayName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamDisplayName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="CustomValue">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomValue") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                    CommandName="ParamDelete" OnClientClick="return confirm('Are you sure, you want to delete this parameter?')"
                                                    CausesValidation="false" CommandArgument='<%#Eval("HttpPostParamsID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: left; width: 100%">
                                <asp:Label ID="lblHttpPostPreview" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="Server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
