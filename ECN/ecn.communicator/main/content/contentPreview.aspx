<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.contentPreview" CodeBehind="contentPreview.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>contentPreview</title>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <asp:ScriptManager ID="scriptMgr" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updateCP" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
            <ContentTemplate>
                <div style="position: fixed; width: 15%; height: 100%; overflow: auto; background-color: #e6e7e8; border: 2px solid black; padding: 20px 20px 20px 0px">
                    <asp:Panel ID="pnlSideBar" runat="server" Style="text-align:center;">
                        <asp:Label ID="lbEmailClients" runat="server" CssClass="label" Text="Dynamic Preview" Font-Size="Large" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <br />
                        <br />
                        <asp:Repeater ID="rptSideBar" runat="server" OnItemDataBound="rptSideBar_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label ID="lbTag" runat="server" CssClass="label" ForeColor="Black"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddlTagRule" runat="server" OnSelectedIndexChanged="ddlTagRule_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <br />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </div>
                <div style="float: right; width: 80%; height: 100%; overflow: auto; color: black; background-color: #e6e7e8; border: 2px solid black; padding: 20px 20px 20px 20px;">
                    <asp:Panel ID="pnlImage" runat="server" Style="background-color:white; overflow: auto;" Height="100%">
                        <center>
                            <asp:Label ID="LabelPreview" runat="Server" Text=""></asp:Label><br />
                            <asp:TextBox ID="TextPreview" runat="Server" EnableViewState="true" Width="90%" Height="90%" TextMode="multiline" CssClass="formfield"></asp:TextBox>
                        </center>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
