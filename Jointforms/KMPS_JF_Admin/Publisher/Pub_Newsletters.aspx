<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_Newsletters.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_Newsletters"
    Title="KMPS Form Builder - Newsletters" ValidateRequest="false"%>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" src="../scripts/telerik_replaceChars.js"></script>
    <script type="text/javascript">
        function OnClientLoad(editor, args)    
    {    
       editor.get_filtersManager().add(new RadEditorCustomFilter());    
    }    
    RadEditorCustomFilter = function()    
    {    
       RadEditorCustomFilter.initializeBase(this);    
       this.set_isDom(false);    
       this.set_enabled(true);    
       this.set_name("RadEditor HTMLCharacter");    
       this.set_description("RadEditor Convert characters to html entities");    
    }
    
    RadEditorCustomFilter.prototype =    
    {    
       getHtmlContent : function(content)    
       {
           var regUni = new RegExp('[\u2600-\u26FF]');
           var newContent = content;
           if (regUni.test(newContent)) {
               for (var i = 0; i < tel_RepCharacters.length; i++) {
                   var regex = new RegExp(tel_RepCharacters[i].character, "gi");
                   newContent = newContent.replace(regex, tel_RepCharacters[i].html);
               }
           }
  
         return newContent;    
       }  
    }    
    RadEditorCustomFilter.registerClass('RadEditorCustomFilter', Telerik.Web.UI.Editor.Filter);    
</script>   
    <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage Publication Newsletters">
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 20%; vertical-align: top;">
                    <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="NEWSLETTERS" />
                </td>
                <td style="width: 2%;">
                    &nbsp;
                </td>
                <td style="width: 78%; vertical-align: top;">
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:GridView ID="grdPublisherNewsletters" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                    Width="100%" AllowPaging="true" DataKeyNames="CategoryId" DataSourceID="SqlDataSourcePNewslettersConnect"
                                    OnRowCommand="grdPublisherNewsletters_RowCommand">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNewslettersID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NewsletterID") %>'></asp:Label>
                                                 <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ECNGroupID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CategoryID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DisplayName" HeaderText="Name" ReadOnly="true" SortExpression="DisplayName"
                                            ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-VerticalAlign="Top" />
                                        <asp:TemplateField ItemStyle-Width="1%" Visible="false" HeaderText="" SortExpression=""
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Description") %>'
                                                    Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="29%" HeaderText="Customer" SortExpression="CustomerName"
                                            Visible="True" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomerID") %>'
                                                    Visible="false"></asp:Label>
                                                <%# DataBinder.Eval(Container,"DataItem.CustomerName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CategoryName" HeaderText="Category" ReadOnly="true" SortExpression="CategoryName"
                                            ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="2%" HeaderText="Active" SortExpression="IsActive">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsActive" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive").ToString().ToUpper()=="Y"? "YES" : "NO"%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="2%" HeaderText="ShowDisplayName" SortExpression="IsActive">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDisplayName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ShowDisplayName").ToString().ToUpper()=="Y"? "YES" : "NO"%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="UDF" ItemStyle-HorizontalAlign="center"
                                            ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <div class="buttonSmall">
                                                    <a href='Newsletter_UDF.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&NewsletterID=<%# DataBinder.Eval(Container,"DataItem.NewsletterID") %>'
                                                        onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:530, width:800, objectHeight:530, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                        UDF</a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnNewsletterEdit" runat="server" ImageUrl="~/images/icon-edit.gif"
                                                    CommandName="NewsletterEdit" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnNewsletterDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                    CommandName="NewsletterDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                    CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSourcePNewslettersConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="sp_PublishersNewsletter" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="PubID" Name="PubID" Type="Int32" DefaultValue="0" />
                                        <asp:Parameter Name="NewsletterID" Type="Int32" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="txtDisplayName" PropertyName="Text" Name="DisplayName"
                                            DefaultValue="0" />
                                        <asp:ControlParameter ControlID="RadEditorDescription" PropertyName="Content" Name="Description"
                                            DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlCustomer" DefaultValue="0" Name="CustomerID"
                                            PropertyName="Text" Type="String" />
                                        <asp:Parameter Name="ECNGroupID" Type="Int32" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlCategory" PropertyName="SelectedValue" Name="CategoryID"
                                            DefaultValue="0" />
                                        <asp:ControlParameter ControlID="rbtlstIsActive" PropertyName="SelectedValue" Name="IsActive"
                                            DefaultValue="0" />
                                        <asp:ControlParameter ControlID="rblstDisplayName" PropertyName="SelectedValue" Name="ShowDisplayName"
                                            DefaultValue="1" />
                                        <asp:Parameter Name="URL" Type="String" DefaultValue="0" />
                                        <asp:Parameter Name="qsNameValue" Type="String" DefaultValue="0" />
                                        <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" />
                                        <asp:Parameter Name="ModifiedBy" Type="String" DefaultValue="0" />
                                        <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
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
                                <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Add Newsletter">
                                    <table style="width: 100%;" border="0px" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td>
                                                Customer
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px" DataTextField="CustomerName"
                                                    DataValueField="CustomerId">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlCustomer"
                                                    ErrorMessage="*" ValidationGroup="PubValidation" InitialValue=""></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%">
                                                Name
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqNewsletterName" runat="server" ControlToValidate="txtDisplayName"
                                                    ErrorMessage="*" Font-Bold="false" ValidationGroup="NewsLetterAdd"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%">
                                                Show Display Name
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:RadioButtonList ID="rblstDisplayName" runat="server" CellSpacing="5" CellPadding="1"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Description
                                            </td>
                                            <td>
                                                <telerik:RadEditor runat="server" ID="RadEditorDescription" Width="100%" OnClientLoad="OnClientLoad"></telerik:RadEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Category
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategory" runat="server" Width="200px" DataSourceID="SqlDataPNewslettersCategoryConnect"
                                                    DataTextField="CategoryName" DataValueField="CategoryID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Req1" runat="server" ErrorMessage="*" Font-Bold="false"
                                                    ControlToValidate="ddlCategory" InitialValue="0" ValidationGroup="NewsLetterAdd"></asp:RequiredFieldValidator>
                                                <asp:SqlDataSource ID="SqlDataPNewslettersCategoryConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                    SelectCommand="sp_PublishersNewslettersCategory" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter QueryStringField="PubID" Name="PubID" Type="Int32" DefaultValue="0" />
                                                        <asp:Parameter Name="CategoryID" Type="Int32" DefaultValue="0" />
                                                        <asp:Parameter Name="CategoryType" Type="String" DefaultValue="N" />
                                                        <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                Is Active
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbtlstIsActive" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                Auto Subscription
                                            </td>
                                            <td>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td width="45%" valign="top">
                                                            <b>Available Groups</b>
                                                            <asp:ListBox ID="lstAvailableGroups" SelectionMode="Multiple" runat="server" Width="100%"
                                                                Height="200px"></asp:ListBox>
                                                        </td>
                                                        <td width="10%" align="center">
                                                            <asp:Button ID="btnAddSelectedGroups" runat="server" Text=">>" Width="50px" OnClick="btnAddSelectedGroups_Click"
                                                                CssClass="buttonSmall" /><br />
                                                            <asp:Button ID="btnRemoveSelectedGroups" runat="server" Text="<<" Width="50px" OnClick="btnRemoveSelectedGroups_Click"
                                                                CssClass="buttonSmall" />
                                                        </td>
                                                        <td width="45%" valign="top">
                                                            <b>Selected Groups</b>
                                                            <asp:ListBox ID="lstSelectedGroups" runat="server" SelectionMode="Multiple" Width="100%"
                                                                Height="200px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%">
                                                External Post URL
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:TextBox ID="txtPostURL" runat="server" MaxLength="200" Width="400px"></asp:TextBox>
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="text-align: left; width: 20%">
                                                Query String Details
                                            </td>
                                            <td style="text-align: left; width: 80%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: left; width: 100%">
                                                                    Name
                                                                    <asp:TextBox ID="txtQSName" runat="server" MaxLength="50" Width="100px"></asp:TextBox>
                                                                    &nbsp;&nbsp; Value
                                                                    <asp:DropDownList ID="drpQSValue" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="drpQSValue_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;
                                                                    <asp:TextBox ID="txtQSValue" runat="server" MaxLength="200" Width="100px" Visible="false"></asp:TextBox>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnAddHttpPostURL" runat="server" Text="Add" OnClick="btnAddHttpPostURL_Click" />
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
                                                                                    <asp:Label ID="lblParamValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ParamValue") %>'></asp:Label>
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
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:HiddenField ID="hfldNewsletterID" runat="server" />
                                                <asp:HiddenField ID="hfldGroupID" runat="server" Value="0"/>
                                                <asp:HiddenField ID="hfHttpPostID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left">
                                                <asp:Button ID="btnAdd" runat="server" Text="SAVE" OnClick="btnAdd_Click" CssClass="button"
                                                    ValidationGroup="NewsLetterAdd" />&nbsp;&nbsp;
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
</asp:Content>
