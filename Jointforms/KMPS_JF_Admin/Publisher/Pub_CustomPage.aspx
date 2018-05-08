<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_CustomPage.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_CustomPage"
    Title="KMPS Form Builder - Custom Page"  ValidateRequest="false"%>

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
   
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage Custom Pages">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="PAGES" />
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdPubCustom" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            EmptyDataText="NO CUSTOM PAGES CREATED YET .. ! Use the form below to create a new Custom Page"
                                            Width="100%" AllowPaging="true" DataKeyNames="PCPID" DataSourceID="SqlDataSourcePCustomConnect"
                                            OnRowCommand="grdPubCustom_RowCommand">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPubCustomID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PCPID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PageName" HeaderText="Page Name" ReadOnly="true" SortExpression="PageName"
                                                    ItemStyle-Width="75%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="5%" HeaderText="Active" SortExpression="IsActive">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIsActive" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive").ToString().ToUpper()=="Y"? "YES" : "NO"%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="5%" HeaderText="Preview" SortExpression="IsActive">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hyplnkUDF" runat="server" ImageUrl="~/images/icon-preview.gif"
                                                            Target="_blank" NavigateUrl='<%# String.Format("http://eforms.kmpsgroup.com/jointforms/forms/ViewPage.aspx?PCPID={0}", Eval("PCPID")) %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPubCustomPageHTML" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PageHTML") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnCustomPageEdit" runat="server" ImageUrl="~/images/icon-edit.gif"
                                                            CommandName="CustomEdit" CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnCustomPageDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                            CommandName="CustomDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePCustomConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="sp_PublishersCustomPage" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="PubID" Name="PubID" Type="Int32" DefaultValue="0" />
                                                <asp:Parameter Name="PCPID" Type="Int32" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="txtPageName" PropertyName="Text" Name="PageName"
                                                    DefaultValue="0" />
                                                <asp:ControlParameter DefaultValue="0" ControlID="RadEditorPageHTML" Name="PageHTML" PropertyName="Content"
                                                    Type="String" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="rbtlstIsActive" PropertyName="SelectedValue" Name="IsActive"
                                                    DefaultValue="0" />
                                                <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="ModifiedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
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
                                        <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Add Custom page">
                                            <table style="width: 100%;" border="0px" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left; width: 15%">
                                                        Page Name
                                                    </td>
                                                    <td style="text-align: left; width: 85%">
                                                        <asp:TextBox ID="txtPageName" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqPageName" runat="server" ControlToValidate="txtPageName"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Is Active
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbtlstIsActive" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;" colspan="2">
                                                        Page HTML<br />
                                                        <br />
                                                        <telerik:RadEditor runat="server" ID="RadEditorPageHTML" OnClientLoad="OnClientLoad" Height="300px" Width="100%"></telerik:RadEditor>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                                        <asp:HiddenField ID="hfldPubCustomId" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: right">
                                                        <asp:Button ID="btnAdd" runat="server" Text="SAVE" OnClick="btnAdd_Click" CssClass="button" />&nbsp;&nbsp;
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
