<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageWatchTagsEditor.aspx.cs"
    Inherits="ecn.communicator.main.PageWatch.PageWatchTagsEditor" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagName="radEditor" Src="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.ascx" TagPrefix="radEditor" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">
        window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000);
    </script>
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
                                    <td class="label" align="right" width="25%">
                                        &nbsp
                                    </td>
                                    <td class="label" align="right" width="25%">
                                        <asp:Button ID="btnAccept" runat="server" Text="Setup Blast" class="formbuttonsmall"
                                            CausesValidation="False" OnClick="btnAccept_Click" />
                                    </td>
                                    <td class="label" align="left" width="25%">
                                        <asp:Button ID="btnIgnore" runat="server" Text="Discard All Changes" class="formbuttonsmall"
                                            CausesValidation="False" OnClick="btnIgnore_Click" />
                                    </td>
                                    <td class="label" align="right" width="25%">
                                        &nbsp<%--<asp:HyperLink runat="server" NavigateUrl="PageWatchEditor.aspx">Return to Page Watches</asp:HyperLink>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label" align="right" width="25%">
                                        &nbsp
                                    </td>
                                    <td class="label" align="right" width="25%">
                                        &nbsp
                                    </td>
                                    <td class="label" align="right" width="25%">
                                        &nbsp
                                    </td>
                                    <td class="label" align="right" width="25%">
                                        <asp:Button ID="btnAddTag" runat="server" Text="Add New Tag" class="formbuttonsmall"
                                            CausesValidation="False" OnClick="btnAddTag_Click" />
                                        &nbsp
                                    </td>
                            </table>
                        </td>
                        <td class="label" align="right">
                            <%--&nbsp<asp:HyperLink runat="server" NavigateUrl="PageWatchEditor.aspx">Return to Page Watches</asp:HyperLink>--%>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            <asp:GridView ID="gvPageWatchTags" runat="server" AllowPaging="True" AllowSorting="True"
                                CssClass="grid" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True"
                                DataKeyNames="PageWatchTagID" OnSelectedIndexChanged="gvPageWatchTags_SelectedIndexChanged"
                                OnPageIndexChanging="gvPageWatchTags_PageIndexChanging" OnRowDeleting="gvPageWatchTags_RowDeleting1"
                                ondeletecommand="gvPageWatchTags_Command" OnSorting="gvPageWatchTags_Sorting"
                                PageSize="5" OnRowEditing="gvPageWatchTags_RowEditing" OnRowCommand="gvPageWatchTags_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <img src='/ecn.images/images/new_animated.gif' style='<%# (Boolean.Parse(Eval("IsChanged").ToString())) ? "display:block": "display:none"%>'
                                                title="To compare current and previous content click View" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Tag Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WatchTag" HeaderText="Tag" SortExpression="WatchTag" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Changed" SortExpression="IsChanged"> 
                                        <ItemTemplate><%# (Boolean.Parse(Eval("IsChanged").ToString())) ? "Yes" : "No"%></ItemTemplate> 
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="View" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-preview-HTML.gif alt='View Page Watch Tag' border='0'&gt;"
                                                CausesValidation="false" ID="ViewPageWatchTagBtn" CommandName="View" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageWatchTagID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Page Watch Tag' border='0'&gt;"
                                                CausesValidation="false" ID="EditPageWatchTagBtn" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageWatchTagID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Page Watch Tag' border='0'&gt;"
                                                CausesValidation="false" ID="DeletePageWatchTagBtn" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageWatchTagID") %>'></asp:LinkButton>
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
                            <asp:Panel ID="pnlEdit" runat="server" Visible="true">
                                <fieldset>
                                    <legend class="label">Edit Page Watch Tag</legend>
                                    <table cellspacing="1" cellpadding="1" width="100%" border='0'>
                                        <tr>
                                            <td class="label" valign="middle" align='right' width="120">
                                                Tag name&nbsp;
                                            </td>
                                            <td class="label" align="left">
                                                <asp:TextBox ID="txtName" runat="server" class="formfield" Columns="34" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvname" runat="server" ControlToValidate="txtName"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblPageWatchTagID" runat="server" Text="" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label" valign="middle" align='right' width="120">
                                                &nbsp;
                                            </td>
                                            <td class="label" align="left">
                                                ( Example: &lt;MyTag&gt; )
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label" valign="middle" align='right' width="120">
                                                Tag&nbsp;
                                            </td>
                                            <td class="label" align="left">
                                                <asp:TextBox ID="txtTag" runat="server" class="formfield" Columns="34" MaxLength="300"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTag" runat="server" ControlToValidate="txtTag"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revTag" runat="server" ErrorMessage="*" ControlToValidate="txtTag"
                                                    ValidationExpression="&lt;([A-Za-z]*)&gt;"></asp:RegularExpressionValidator>
                                                <asp:Button ID="btnValidate" runat="server" Text="Validate" class="formbuttonsmall"
                                                    OnClick="btnValidate_Click" Visible="false"></asp:Button>
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
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlCompare" runat="server" Visible="true">
                                <fieldset>
                                    <legend class="label">Compare HTML</legend>
                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border='0'>
                                        <tbody>
                                            <tr>
                                                <td width="50%" class="label" align="center" colspan="2" style="width: 100%">
                                                    <asp:Label ID="lblView" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%" class="label" align="center">
                                                    Previous HTML
                                                </td>
                                                <td width="50%" class="label" align="center">
                                                    Current HTML
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%" class="label" align="left">
                                                   <%-- <radEditor:radEditor ID="FCKeditorPrevious" runat="server"  Height="450px" Width="700px" EditModes="Html,Design,Preview"  ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" />--%>
                                                    <CKEditor:CKEditorControl ID="FCKeditorPrevious" runat="server" Skin="kama" Height="450"
                                                        Width="100%" BasePath="/ecn.editor/ckeditor/" ToolbarStartExpanded="false"></CKEditor:CKEditorControl>
                                                </td>
                                                <td width="50%" class="label" align="left">
                                                    <%--<radEditor:radEditor ID="FCKeditorCurrent"  Height="450px" Width="700px" EditModes="Html,Design,Preview"  ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" runat="server" />--%>
                                                    <CKEditor:CKEditorControl ID="FCKeditorCurrent" runat="server" Skin="kama" Height="450"
                                                        Width="100%" BasePath="/ecn.editor/ckeditor/" ToolbarStartExpanded="false"></CKEditor:CKEditorControl>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                        </td>
                    </tr>
                    <%--<tr>
                        <td width="100%" class="label" align="left">
                            Previous HTML
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            <FredCK:FCKeditor id="FCKeditorPrevious" runat="Server" height="450" width="100%" BasePath="/ecn.editor/" ToolbarStartExpanded="false"></FredCK:FCKeditor>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            Current HTML
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" class="label" align="left">
                            <FredCK:FCKeditor id="FCKeditorCurrent" runat="Server" height="450" width="100%" BasePath="/ecn.editor/" ToolbarStartExpanded="false"></FredCK:FCKeditor>
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
            <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
</asp:Content>
