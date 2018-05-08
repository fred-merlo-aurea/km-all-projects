<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FieldSettings.aspx.cs"
    Inherits="KMPS_JF_Setup.Publisher.FieldSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color: White; margin:5px 5px 5px 5px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="script1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table width="100%" style="background-color: white"  cellpadding="2" cellspacing="0">
                <tr>
                    <td style="text-align: left">
                        <table width="100%" border="0" cellpadding="5" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                   Select the Branching Field? 
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlECNField" runat="server" Width="200px" 
                                        AppendDataBoundItems="true" AutoPostBack="true"> 
                                        <asp:ListItem Selected="" Text="Select Field" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="2" align="left">
                                    Select branching values to show this Field on the form?<br />
                                    <asp:GridView ID="grdFieldValues" SkinID="skingrid2" runat="server" AllowPaging="false" AllowSorting="false" DataSourceID="dsFieldValues" 
                                        AutoGenerateColumns="false" Width="90%" ShowFooter="false" DataKeyNames="DataValue">
                                        <Columns>
                                            <asp:BoundField DataField="DataValue" HeaderText="Value" ItemStyle-Width="35%"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="DataText" HeaderText="Text" ItemStyle-Width="55%"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>                                           
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="10%" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelected" runat="server" Checked='<%# Eval("IsSelected").ToString().ToUpper()=="Y"?true:false %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsFieldValues" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select distinct psfd.DataText, psfd.DataValue, case when (psfd.datavalue = fs.datavalue and fs.PFFieldID = @FSPFFieldID) then 'Y' else 'N' end as 'IsSelected' from PubSubscriptionFieldData psfd join  pubformfields pff on pff.PSFieldID = psfd.PSFieldID left outer join fieldsettings fs on fs.PFFReferenceID = pff.PFFieldID and pff.PFID = @PFID and fs.datavalue = psfd.datavalue and fs.PFFieldID = @FSPFFieldID where PFF.PFFieldID = @PFFieldID order by psfd.datavalue"   SelectCommandType="Text"> 
                                            <SelectParameters>
                                               <asp:ControlParameter Name="PFFieldID" ControlID="ddlECNField" Type="Int32" PropertyName="SelectedValue" 
                                                ConvertEmptyStringToNull="false" /> 
                                                <asp:QueryStringParameter DefaultValue="0" Name="PFID" QueryStringField="PFID"
                                                Type="Int32" />
                                                <asp:QueryStringParameter DefaultValue="0" Name="FSPFFieldID" QueryStringField="PFFieldID"
                                                Type="Int32" />
                                            </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button CssClass="button" ID="btnAdd" runat="server" Text="Save" onclick="btnAdd_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
