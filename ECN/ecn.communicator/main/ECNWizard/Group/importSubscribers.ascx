<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="importSubscribers.ascx.cs"
    Inherits="ecn.communicator.main.ECNWizard.Group.newGroup_Import" %>
   <script type="text/javascript">
       function uploadComplete() {
           //Postback is necessary for asyncfileupload
           //__doPostBack('UpdatePanel3', '');
       }
  </script>
   <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
     <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="gvImport" />
        </Triggers>
        <ContentTemplate>
   <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
         <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
        </tr>
        <tr>
            <td id="errorMiddle">
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
    </asp:PlaceHolder>
             <asp:Label ID="lblImportFileName" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblGUID" runat="server" Text="" Visible="false"></asp:Label>
<table cellspacing="0" cellpadding="0" width="100%" border="0" style="padding-left: 30px">
                <asp:PlaceHolder ID="plBrowse" runat="server">
                    <tr>
                        <td class="section" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="headingOne" align="left" colspan="2">
                                        Select File to Import:&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align="left" width="15%">
                                        Import From:&nbsp;
                                    </td>
                                    <td class="dataOne" align="left" width="80%">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                       &nbsp;<span
                                            class="highLightOne">(CSV, TXT, XLS, XLSX or XML File)</span>
                                        <asp:Label ID="lblfilename" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align="left">
                                        Worksheet Name :&nbsp;
                                    </td>
                                    <td class="dataOne" align="left">
                                        <asp:TextBox ID="txtSheetName" CssClass="dataOne" runat="server" Width="200px" value="Sheet1"></asp:TextBox>&nbsp;<span
                                            class="highLightOne">(Excel Only)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" align="left">
                                        Delimiter:&nbsp;
                                    </td>
                                    <td class="dataOne" align="left">
                                        <asp:DropDownList ID="drpDelimiter" CssClass="dataOne" runat="server" Width="70">
                                            <asp:ListItem runat="server" Value="c" Text="Commas" Selected="true" ID="Listitem1"
                                                NAME="Listitem1" />
                                            <asp:ListItem runat="server" Value="TabDelimited" Text="Tabs" ID="Listitem2" NAME="Listitem2" />
                                        </asp:DropDownList>
                                        &nbsp;<span class="highLightOne">(for TEXT File)</span> &nbsp    &nbsp    &nbsp                                     
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="label10" OnClick="btnUpload_Click">
                                        </asp:Button>
                                   
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                      </asp:PlaceHolder> 
                    <asp:PlaceHolder ID="plUpload" runat="server" Visible="false">
                        <tr>
                            <td class="headingOne" align="left">
                                Map Data with Profile fields:&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100%">
                                <table id="dataCollectionTable" bordercolor="#cccccc" cellspacing="0" cellpadding="3"
                                    width="100%" align="left" border="1" runat="server">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnImport" runat="server" Text="Import Data" CssClass="formLabel"
                                    OnClick="btnImport_Click"></asp:Button>
                            </td>
                        </tr>
                   </asp:PlaceHolder> 
                    <asp:PlaceHolder ID="plImportCompleted" runat="server" Visible="false">
                        <tr>
                            <td style="padding-left: 30px">
                                <table cellspacing="0" cellpadding="0" width="80%" border="0">
                                    <tr>
                                        <td class="label" align="left" colspan="3">
                                            <font color="#05b835"><strong>Import Successful</strong></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvImport" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" Width="100%"  OnRowCommand="gvImport_Command"
                OnRowDataBound="gvImport_OnRowDataBound">
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                            HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Totals" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" Visible="true">
                                    <ItemTemplate>
                                     <asp:Label runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.ActionCode") %> ID="lblActionCode" Visible="false"></asp:Label> 
                                     <asp:Label runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.Totals") %> ID="lblTotals" Visible="false"></asp:Label>                             
                                    <asp:LinkButton runat="Server" Text=<%# DataBinder.Eval(Container, "DataItem.Totals") %> CausesValidation="false" ID="lnkTotals" CommandName="DownloadEmails" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActionCode") %>'  Visible="false"></asp:LinkButton>
                                    </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                </asp:PlaceHolder>
  </table>
  <br />  <br />


</ContentTemplate>
</asp:UpdatePanel>