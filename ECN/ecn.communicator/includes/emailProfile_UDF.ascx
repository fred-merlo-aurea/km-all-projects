<%@ Control Language="c#" Inherits="ecn.communicator.includes.emailProfile_UDF" Codebehind="emailProfile_UDF.ascx.cs" %>
<br>
<table style="border-right: #281163 1px solid; border-top: #281163 1px solid; border-left: #281163 1px solid;
    border-bottom: #281163 1px solid" cellspacing="2" cellpadding="2" width="770"
    align="center" border="0">
    <tr>
        <td bgcolor="#281163" colspan="3">
            <div align="center">
                <font face="Verdana" color="#ffffff" size="2"><strong>User Defined Data</strong></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <div align="left">
                <font style="font-weight: bold; color: red">
                    <asp:Label ID="MessageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
                        Visible="False"></asp:Label></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DataGrid ID="StandAloneUDFGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                Visible="True" DataKeyField="EmailDataValuesID" OnItemCommand="StandAloneUDFGrid_Command"
                OnUpdateCommand="StandAloneUDFGrid_Update" OnCancelCommand="StandAloneUDFGrid_Cancel"
                OnEditCommand="StandAloneUDFGrid_Edit" CssClass="grid">
                <ItemStyle></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundColumn DataField="ShortName" HeaderText="Field Name" ReadOnly="true" ItemStyle-Width="25%">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="LongName" HeaderText="Field Description" ReadOnly="true"
                        ItemStyle-Width="55%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="DataValue" HeaderText="Value" ItemStyle-Width="20%"></asp:BoundColumn>
                </Columns>
                <AlternatingItemStyle CssClass="gridaltrow" />
            </asp:DataGrid>
        </td>
    </tr>
</table>
