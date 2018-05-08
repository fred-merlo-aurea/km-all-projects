<%@ Page Language="c#" Inherits="ecn.creator.pages.pagedetail" CodeBehind="pagedetail.aspx.cs" MasterPageFile="~/Creator.Master" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="ecn" TagName="templates" Src="../../includes/templates.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td valign="top" colspan='3'>
                    <ecn:templates ID="TemplateBrowser" runat="Server" />
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr size="1" color="#000000">
                </td>
            </tr>
            <asp:Panel ID="URLPanel" runat="Server" Visible="false">
                <tr>
                    <td colspan='3' class="tableHeader" height="35" valign="top"><font color="#FF0000">WebSite URL for this Page</font>
                        <asp:TextBox ID="WebsiteURL" runat="Server" Columns="80" CssClass="formfield" value="" ReadOnly />
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td class="tableHeader" align='right'>Page Title&nbsp;</td>
                <td>
                    <asp:TextBox ID="PageName" runat="Server" Columns="30" CssClass="formfield"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="Server" ID="val_PageName" ControlToValidate="PageName"
                        ErrorMessage="Page Name is a required field." CssClass="errormsg" Display="Static"><-- Required</asp:RequiredFieldValidator>
                </td>
                <td rowspan="7" width="270" class="tableHeader" align="center" valign="top" bgcolor="#eeeeee">
                    <cpanel:DataPanel ID="DataPanel1" runat="Server" ExpandImageUrl="../../assets/images/expand.gif"
                        CollapseImageUrl="../../assets/images/collapse.gif" CollapseText="Click to hide Page Properties"
                        ExpandText="Click to display Page Properties" Collapsed="False" TitleText="View / Edit Page Properties"
                        AllowTitleExpandCollapse="True">
                        <asp:Panel ID="PgPropPanel" runat="Server" Visible="true" CssClass="smallBold"
                            HorizontalAlign="left" BorderWidth="1" BorderColor="#000000">
                            <table border='0' width="100%">
                                <tr>
                                    <td><b>
                                        <asp:Label ID="bg_color" runat="Server" Visible="true" Text="Color" CssClass="smallBold" /><br />
                                        <asp:TextBox ID="pg_bg_color" runat="Server" Columns="9" CssClass="formfield formtextfieldsmall" value="" /></b></td>
                                    <td>
                                        <asp:Label ID="txt_color" runat="Server" Visible="true" Text="Text Color" CssClass="smallBold" /><br />
                                        <asp:TextBox ID="pg_txt_color" runat="Server" Columns="9" CssClass="formfield formtextfieldsmall" value="" /></td>
                                    <tr>
                                        <td>
                                            <asp:Label ID="link_color" runat="Server" Visible="true" Text="Link Color" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_link_color" runat="Server" Columns="9" CssClass="formfield formtextfieldsmall" value="" /></td>
                                        <td>
                                            <asp:Label ID="vlink_color" runat="Server" Visible="true" Text="Visited Link Color" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_vlink_color" runat="Server" Columns="9" CssClass="formfield formtextfieldsmall" value="" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="alink_color" runat="Server" Visible="true" Text="Active Link Color" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_alink_color" runat="Server" Columns="9" CssClass="formfield formtextfieldsmall" value="" /></td>
                                        <td class="smallBold">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="smallBold">
                                            <asp:Label ID="margin_width" runat="Server" Visible="true" Text="Margin Width [pixels]" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_margin_width" runat="Server" Columns="3" CssClass="formfield formtextfieldsmall" value="0" /></td>
                                        <td class="smallBold">
                                            <asp:Label ID="margin_height" runat="Server" Visible="true" Text="Margin Height [pixels]" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_margin_height" runat="Server" Columns="3" CssClass="formfield formtextfieldsmall" value="0" /></td>
                                    </tr>
                                    <tr>
                                        <td class="smallBold">
                                            <asp:Label ID="left_margin" runat="Server" Visible="true" Text="Left Margin [pixels]" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_left_margin" runat="Server" Columns="3" CssClass="formfield formtextfieldsmall" value="0" /></td>
                                        <td class="smallBold">
                                            <asp:Label ID="right_margin" runat="Server" Visible="true" Text="Top Margin [pixels]" CssClass="smallBold" /><br />
                                            <asp:TextBox ID="pg_top_margin" runat="Server" Columns="3" CssClass="formfield formtextfieldsmall" value="0" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="other_props" runat="Server" Visible="true" Text="Other Properties" CssClass="smallBold" /><center>
                                                <asp:TextBox ID="pg_other_props" runat="Server" Columns="43" Rows="3" TextMode="multiline" CssClass="formfield formtextfieldsmall" />
                                        </td>
                                    </tr>
                            </table>
                        </asp:Panel>
                    </cpanel:DataPanel>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>Page Identifier&nbsp;</td>
                <td>
                    <asp:TextBox ID="QueryValue" runat="Server" Columns="20" CssClass="formfield"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="Server" ID="val_QueryValue" ControlToValidate="QueryValue"
                        ErrorMessage="Page Identifier is a required field." CssClass="errormsg" Display="Static"><-- Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>Header Footer&nbsp;</td>

                <script>
                    function openWindow() {
                        window.open('viewHeaderFooter.aspx?hfID=<%=hfID%>', '', 'width=800,height=600,resizable=yes,scrollbars=yes')
				}
                </script>

                <td>
                    <asp:DropDownList runat="Server" ID="HeaderFooter" Visible="True" EnableViewState="True"
                        DataValueField="HeaderFooterID" DataTextField="HeaderFooterName" AutoPostBack="true"
                        OnSelectedIndexChanged="setViewHeaderFooterURL" CssClass="formfield">
                    </asp:DropDownList>
                    &nbsp;&nbsp;<a href="javascript:openWindow()"><img src="/ecn.images/images/icon-preview-HTML.gif"
                        alt='Preview Content as HTML' border='0'></a></td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>PageTypeCode&nbsp;</td>
                <td>
                    <asp:DropDownList runat="Server" ID="PageTypeCode" Visible="True" EnableViewState="True"
                        CssClass="formfield">
                    </asp:DropDownList>
            </tr>
            <!--			   
                <td valign="top">
                    <asp:Button id="CreateAsNewTopButton" runat="Server" Text="Create as new Campaign" class="formbuttonsmall"
                        Visible="false" />
                </td>
            </tr>-->
            <tr>
                <td class="tableHeader" align='right'>&nbsp;Folder&nbsp;</td>
                <td>
                    <asp:DropDownList EnableViewState="true" ID="FolderID" runat="Server" DataValueField="FolderID"
                        DataTextField="FolderName" CssClass="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' nowrap>Display on Website&nbsp;</td>
                <td class="tableHeader">
                    <asp:CheckBox ID="DisplayFlag" runat="Server"></asp:CheckBox>
                    &nbsp;&nbsp; Home Page&nbsp;<asp:CheckBox ID="HomePageFlag" runat="Server"></asp:CheckBox>
                </td>
            </tr>

            <script>
                function notyet() {
                    confirm('This function not available yet.');
                }
            </script>

            <tr>
                <td class="tableHeader" align='right' valign="middle">Assignments&nbsp;</td>
                <td valign="middle">
                    <table class="tableContent" border='0'>
                        <asp:Panel ID="Slot1" runat="Server">
                            <tr>
                                <td>Slot 1 
							<asp:DropDownList ID="ContentSlot1" runat="Server" width="99%" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot2" runat="Server">
                            <tr>
                                <td>Slot 2 
							<asp:DropDownList ID="ContentSlot2" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot3" runat="Server">
                            <tr>
                                <td>Slot 3 
							<asp:DropDownList ID="ContentSlot3" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot4" runat="Server">
                            <tr>
                                <td>Slot 4 
							<asp:DropDownList ID="ContentSlot4" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot5" runat="Server">
                            <tr>
                                <td>Slot 5 
							<asp:DropDownList ID="ContentSlot5" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot6" runat="Server">
                            <tr>
                                <td>Slot 6 
							<asp:DropDownList ID="ContentSlot6" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot7" runat="Server">
                            <tr>
                                <td>Slot 7 
							<asp:DropDownList ID="ContentSlot7" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot8" runat="Server">
                            <tr>
                                <td>Slot 8 
							<asp:DropDownList ID="ContentSlot8" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="Slot9" runat="Server">
                            <tr>
                                <td>Slot 9 
							<asp:DropDownList ID="ContentSlot9" runat="Server" DataTextField="ContentTitle" DataValueField="ContentID" CssClass="formfield" AutoPostBack></asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr size="1" color="#000000">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">Size of Page:&nbsp;</td>
                <td colspan="2">
                    <asp:Label ID="SizeLabel" runat="Server" CssClass="tableContent"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan='3' align="center">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="PageID" runat="Server"></asp:TextBox>
                    <asp:Button ID="SaveButton" OnClick="CreatePage" runat="Server" Text="Create Page"
                        class="formbutton" Visible="true" />
                    <asp:Button ID="UpdateButton" OnClick="UpdatePage" runat="Server" Text="Update Page"
                        class="formbutton" Visible="false" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

