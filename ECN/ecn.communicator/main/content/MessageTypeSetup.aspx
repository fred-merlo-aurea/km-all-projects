<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.contentmanager.MessageTypeSetup"
    CodeBehind="MessageTypeSetup.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
           <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
                                    <tr>
                                        <td id="Td1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td2">
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
                                        <td id="Td3">
                                        </td>
                                    </tr>
                                </table>
    </asp:PlaceHolder>
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <tbody>
                    <tr>
                        <td class="label" align="left">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            <asp:GridView ID="gvMessageTypes" runat="server" AllowPaging="True" AllowSorting="True"
                                CssClass="grid" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True"
                                DataKeyNames="MessageTypeID" OnSelectedIndexChanged="gvMessageTypes_SelectedIndexChanged"
                                OnPageIndexChanging="gvMessageTypes_PageIndexChanging" OnRowDeleting="gvMessageTypes_RowDeleting"
                                ondeletecommand="gvMessageTypes_Command" OnSorting="gvMessageTypes_Sorting" PageSize="10">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Enable Threshold" SortExpression="Threshold">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("Threshold").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enable Priority" SortExpression="Priority">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("Priority").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="True" HeaderText="Edit" ButtonType="Image" SelectImageUrl="/ecn.images/images/icon-edits1.gif"
                                        ShowEditButton="False" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:CommandField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Message Type' border='0'&gt;"
                                                CausesValidation="false" ID="DeleteContentBtn" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.MessageTypeID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#CBCCCE" Font-Bold="True" Height="30px" />
                                <AlternatingRowStyle BackColor="#EBEBEC" BorderColor="#A4A2A3" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="section" align="left">
                            <asp:DetailsView ID="dvMessageTypes" runat="server" Height="50px" Width="100%" AutoGenerateRows="False"
                                DataKeyNames="MessageTypeID" DefaultMode="Insert" CaptionAlign="Left" OnItemInserting="dvMessageTypes_ItemInserting"
                                OnItemInserted="dvMessageTypes_ItemInserted" OnItemUpdated="dvMessageTypes_ItemUpdated"
                                OnModeChanging="dvMessageTypes_ModeChanging" HeaderText="Add Message Type" OnItemUpdating="dvMessageTypes_ItemUpdating"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="label" BackColor="#cbccce" Font-Bold="True" ForeColor="Black" />
                                <EditRowStyle BackColor="White" Font-Bold="True" ForeColor="#000000" />
                                <Fields>
                                    <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' Width="250px"
                                                CssClass="formfield"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="label" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'
                                                Width="250px" CssClass="formfield"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="label" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enable Threshold">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlThreshold" runat="server" CssClass="formfield" SelectedValue='<%# Bind("Threshold") %>'>
                                                <asp:ListItem Value="False">No</asp:ListItem>
                                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="label" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enable Priority">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="formfield" SelectedValue='<%# Bind("Priority") %>'>
                                                <asp:ListItem Value="False">No</asp:ListItem>
                                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="label" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlIsActive" runat="server" CssClass="formfield" SelectedValue='<%# Bind("IsActive") %>'>
                                                <asp:ListItem Value="False">No</asp:ListItem>
                                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="label" />
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Button" ShowInsertButton="True" ShowEditButton="True"
                                        InsertText="SAVE" ShowHeader="False" ControlStyle-CssClass="formbutton" CancelText="CANCEL"
                                        UpdateText="UPDATE" ItemStyle-HorizontalAlign="Left">
                                        <ControlStyle CssClass="button" Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:CommandField>
                                </Fields>
                            </asp:DetailsView>
                        </td>
                    </tr>
                </tbody>
            </table>
      
</asp:Content>
