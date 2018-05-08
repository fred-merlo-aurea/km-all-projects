<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLinks.aspx.cs" Inherits="ecn.publisher.main.Edition.AddLinks"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/ecn.images/images/marker.css" />
    <script type="text/javascript">

        var MarqueeTool;
        function onMarqueeUpdate() {
            var coords = MarqueeTool.getCoords();
            $('<%=tb_x.ClientID%>').value = $('coord_x').value = coords.x1;
            $('<%=tb_y.ClientID%>').value = $('coord_y').value = coords.y1;
            $('<%=tb_w.ClientID%>').value = $('coord_w').value = coords.width;
            $('<%=tb_h.ClientID%>').value = $('coord_h').value = coords.height;
        }

        //Event.observe(window, 'load', onWindowLoad);
        $(window).load(onWindowLoad);

        function onWindowLoad() {
            new PreviewToolTip('element_container', { id: 'preview' });
            if ($('<%=tb_x.ClientID%>').value != "" & $('<%=tb_y.ClientID%>').value != "" && $('<%=tb_w.ClientID%>').value != "" && $('<%=tb_h.ClientID%>').value != "") {
                MarqueeTool = new Marquee('<%=ImgDE.ClientID%>', { preview: 'preview', color: '#333' });
                setlinkposition(parseInt($('<%=tb_x.ClientID%>').value), parseInt($('<%=tb_y.ClientID%>').value), parseInt($('<%=tb_w.ClientID%>').value), parseInt($('<%=tb_h.ClientID%>').value));
            }
            else {
                MarqueeTool = new Marquee('<%=ImgDE.ClientID%>', { preview: 'preview', color: '#333' });
            }

            MarqueeTool.setOnUpdateCallback(onMarqueeUpdate);
        }

        function setlinkposition(x, y, w, h) {
            MarqueeTool.setCoords(x, y, w, h);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Label ID="lblErrorMessage" runat="Server" Visible="False" ForeColor="Red" Font-Size="x-small"></asp:Label>
    </div>
    <div style="float: left; padding-top: 10px; padding-bottom: 5px;" class="tableHeader">
        Links in Page #
        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
    </div>
    <div style="float: right; vertical-align: middle; text-align: right; padding-top: 5px;
        padding-bottom: 5px;" class="tableHeader">
        Go to Page #
        <asp:DropDownList ID="ddlPageNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageNo_SelectedIndexChanged"
            Width="50" CssClass="formfield">
        </asp:DropDownList>
    </div>
    <br />
    <asp:GridView ID="grdLinks" runat="server" AutoGenerateColumns="False" ShowFooter="False"
        AllowPaging="False" AllowSorting="False" Width="100%" CellPadding="10" DataKeyNames="LinkID"
        CssClass="grid" Style="float:left">
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right"></PagerStyle>
        <HeaderStyle CssClass="gridheader"></HeaderStyle>
        <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
        <Columns>
            <asp:TemplateField HeaderText="Link/URL" ItemStyle-Width="60%" ItemStyle-HorizontalAlign="left">
                <ItemTemplate>
                    <a href='<%# DataBinder.Eval(Container, "DataItem.LinkURL") %>' target="_blank" title='<%# DataBinder.Eval(Container, "DataItem.LinkURL") %>'>
                        <%# Eval("LinkURL").ToString().Substring(0, Math.Min(Eval("LinkURL").ToString().Length, 50))%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Alias" HeaderText="Link Alias" ItemStyle-Width="25%" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="left" />
            <asp:TemplateField ItemStyle-Width="5%" HeaderText="View" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:HyperLink ID="hlEcn" runat="server" ImageUrl="/ecn.images/images/icon-preview.gif"
                        NavigateUrl='javascript:void(0)' onclick='<%# "javascript:setlinkposition(" + DataBinder.Eval(Container.DataItem, "x").ToString() + "," + DataBinder.Eval(Container.DataItem, "y").ToString() + "," + DataBinder.Eval(Container.DataItem, "w").ToString() + "," + DataBinder.Eval(Container.DataItem, "h").ToString() + ");"%>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="5%" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:ImageButton ID="ibedit" CommandName="editlink" AlternateText="Edit Link" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>'
                        OnCommand="ibedit_Command" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif">
                    </asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="5%" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:ImageButton ID="ibdelete" runat="Server" AlternateText="Delete Link" ImageUrl="/ecn.images/images/icon-delete1.gif"
                        CommandName="deletelink" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>'
                        OnCommand="ibdelete_Command" OnClientClick="return confirm('Are you sure you want to delete?')" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />

    <p style="text-align: right">
        <asp:Button ID="btnPrevious" CssClass="formbutton" runat="server" Text="Previous"
            OnClick="btnPrevious_Click" />&nbsp;&nbsp;<asp:Button ID="btnNext" CssClass="formbutton"
                runat="server" Text="Next" OnClick="btnNext_Click" /></p>
    <h1 class="tableHeader">
        Add/Update Links:</h1>
    <div style="text-align: left; padding-left: 5px;">
        <table border="0" cellpadding="5" cellspacing="3" id="coordinates" width="100%">
            <tr>
                <td nowrap="nowrap" align="right" width="20%" class="tableHeader">
                    <b>Link :</b>
                </td>
                <td align="left" width="75%">
                    <asp:Label ID="lblLinkID" Text="0" runat="server" Visible="false"></asp:Label>
                    <asp:TextBox ID="tblink" runat="server" Width="400" MaxLength="500" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="tableHeader">
                    <b>Link Alias :</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="tblinkAlias" runat="server" Width="400" MaxLength="100" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" class="tableHeader">
                    <b>Select Position :</b>
                </td>
                <td align="left" rowspan="2" valign="top">
                    <div style="position: relative; width: 100%; margin: 0 auto;" id="element_container">
                        <asp:Image ID="ImgDE" runat="server" ImageUrl="http://www.ecndigitaledition.com/images/transparentpixel.gif" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" valign="middle">
                    <font style="color: #7D26CD"><b>Hotkeys :</b><br />
                        <br />
                        <b>R</b> - Change Background Color<br />
                        <b>Alt + Up/Down</b> - increase/reduce opacity<br />
                    </font>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Add Link" OnClick="btnSave_Click" CssClass="formbutton" />&nbsp;&nbsp;<asp:Button
                        ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="formbutton" />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" name="x" value="0" id="coord_x" />
    <input type="hidden" name="y" value="0" id="coord_y" />
    <input type="hidden" name="w" value="0" id="coord_w" />
    <input type="hidden" name="h" value="0" id="coord_h" />
    <span style="visibility: hidden">&nbsp;<asp:TextBox ID="tb_x" runat="server"></asp:TextBox>px
        &nbsp;<asp:TextBox ID="tb_y" runat="server"></asp:TextBox>px &nbsp;<asp:TextBox ID="tb_w"
            runat="server"></asp:TextBox>px &nbsp;<asp:TextBox ID="tb_h" runat="server"></asp:TextBox>px</span>
</asp:Content>
