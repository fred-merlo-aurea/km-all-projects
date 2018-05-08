<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageWatchEditor.aspx.cs"
    Inherits="ecn.communicator.main.PageWatch.PageWatchEditor" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <tbody>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" align="center">
                                <tr>
                                    <td class="label" align="right" width="33%">
                                        &nbsp
                                    </td>
                                    <td class="label" align="center" width="34%">
                                        <asp:Button ID="btnCheckNow" runat="server" Text="Check For New Content" class="formbuttonsmall"
                                            CausesValidation="False" OnClick="btnCheckNow_Click" />
                                    </td>
                                    <td class="label" align="right" width="33%">
                                        &nbsp
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            <asp:GridView ID="gvPageWatch" runat="server" AllowPaging="True" AllowSorting="True"
                                CssClass="grid" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True"
                                DataKeyNames="PageWatchID" OnSelectedIndexChanged="gvPageWatch_SelectedIndexChanged"
                                OnPageIndexChanging="gvPageWatch_PageIndexChanging" OnRowDeleting="gvPageWatch_RowDeleting"
                                ondeletecommand="gvPageWatch_Command" OnSorting="gvPageWatch_Sorting" PageSize="5"
                                OnRowEditing="gvPageWatch_RowEditing">
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <img src='/ecn.images/images/new_animated.gif' style='<%# (Convert.ToInt32(Eval("UpdatedTags").ToString())) > 0?"display:block": "display:none"%>'
                                                title="To see updates and setup blasts click Tags" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Page Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="URL" HeaderText="Page URL" SortExpression="URL" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group" SortExpression="GroupName"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Frequency" HeaderText="Frequency" SortExpression="Frequency"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="Admin" SortExpression="UserName"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="UpdatedTags" HeaderText="Tag(s) Updated" SortExpression="UpdatedTags"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:HyperLinkField ItemStyle-Width="5%" HeaderText="Tags" HeaderStyle-HorizontalAlign="center"
                                        DataNavigateUrlFields="PageWatchID" DataNavigateUrlFormatString="pagewatchtagseditor.aspx?PageWatchID={0}"
                                        ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/tag-icon.jpg alt='View/Edit Tags' border='0'>">
                                    </asp:HyperLinkField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Page Watch' border='0'&gt;"
                                                CausesValidation="false" ID="EditPageWatchBtn" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageWatchID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Page Watch' border='0'&gt;"
                                                CausesValidation="false" ID="DeletePageWatchBtn" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageWatchID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#CBCCCE" Font-Bold="True" Height="30px" />
                                <AlternatingRowStyle BackColor="#EBEBEC" BorderColor="#A4A2A3" />
                            </asp:GridView>
                            <br />
                            <div id="divError" runat="Server" visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
                                    <tr>
                                        <td id="errorTop">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorMiddle">
                                            <table width="80%">
                                                <tr>
                                                    <td valign="top" align="center" width="20%">
                                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
                                                            alt="" />
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
                                <br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="section" align="left">
                            <fieldset>
                                <legend class="label">Edit Page Watch</legend>
                                <table cellspacing="1" cellpadding="1" width="100%" border='0'>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Page name&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:TextBox ID="txtName" runat="server" class="formfield" Columns="34" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvname" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="*"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblPageWatchID" runat="server" Text="" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Page URL&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:TextBox ID="txtURL" runat="server" class="formfield" Columns="120" MaxLength="300"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtURL"
                                                ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Admin User&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="formfield" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Group&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="formfield" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Content&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:DropDownList ID="ddlContent" runat="server" CssClass="formfield" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Frequency&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:DropDownList ID="ddlFrequencyType" runat="server" CssClass="formfield" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlFrequencyType_SelectedIndexChanged" Width="70">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlFrequencyNo" runat="server" CssClass="formfield" Width="40">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            Active&nbsp;
                                        </td>
                                        <td class="label" align="left">
                                            <asp:DropDownList ID="ddlIsActive" runat="server" CssClass="formfield" Width="70px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align='right' width="120">
                                            <asp:Button ID="btnSave" runat="server" Text="Add" class="formbutton" OnClick="btnSave_Click"
                                                Width="70px" />
                                        </td>
                                        <td class="label" valign="middle" align='left'>
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="formbutton" OnClick="btnCancel_Click"
                                                Width="70px" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
