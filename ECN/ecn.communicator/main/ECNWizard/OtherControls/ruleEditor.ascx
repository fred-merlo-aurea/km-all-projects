<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ruleEditor.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.ruleEditor" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
  <style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalPopupContentExplorer
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }
    .overlay
    {
        position: fixed;
        z-index: 99;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }
    .loader
    {
        z-index: 100;
        position: fixed;
        width: 120px;
        margin-left: -60px;
        background-color: #F4F3E1;
        font-size: x-small;
        color: black;
        border: solid 2px Black;
        top: 40%;
        left: 50%;
    }    
    .styled-select
    {
        width: 240px;
        background: transparent;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
    .styled-text
    {
        width: 240px;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
    .reorderStyle
    {
	    list-style-type:disc;
	    font:Verdana;
	    font-size:12px;
    }
    .reorderStyle li
    {
	    list-style-type:none;
        padding-bottom: 1em;
    }
</style>

<div style="text-align:left">   
    <br />
    <asp:Label ID="lblRule" runat="server" Text="Add Rule" Font-Bold="true" Font-Size="Medium"></asp:Label> <br />
    <br /> <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
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
        <table>
            <tr>
                <td>
                      <asp:Label ID="Label7" runat="server" Text="Name" Font-Size="Small" />
         
                </td>
                <td>
                    <asp:TextBox ID="txtRuleName" runat="server" CssClass="styled-text"></asp:TextBox>
                </td>
                 <td>
                      <asp:Label ID="Label1" runat="server" Text="Condition Connector" Font-Size="Small" />
         
                </td>
                <td>
                    <asp:DropDownList ID="drpConditionConnector" runat="server" CssClass="styled-select" Width="60px">
                        <asp:ListItem Value="OR">OR</asp:ListItem>
                        <asp:ListItem Value="AND">AND</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table> <br />
         <asp:Label ID="Label6" runat="server" Text="Conditions" Font-Bold="true" Font-Size="Medium"></asp:Label>
        <br /> <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Field Type" Font-Size="Small" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblFieldType" runat="server" OnSelectedIndexChanged="rblFieldType_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="formLabel">
                        <asp:ListItem Selected="True" Value="Profile">Profile Field</asp:ListItem>
                        <asp:ListItem Value="UDF">User Defined Field</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            </table> <br />
       <table>
            <tr>
                 <td>
                     <asp:Label ID="Label2" runat="server" Text="Field" Font-Size="Small" />
                </td>
                 <td>
                     <asp:DropDownList ID="drpField" runat="server" OnSelectedIndexChanged="drpField_SelectedIndexChanged" AutoPostBack="true" CssClass="styled-select"></asp:DropDownList>
                </td>
                <asp:Panel ID="pnlDataType" Visible="false" runat="server">
                 <td>
                      <asp:Label ID="Label3" runat="server" Text="Data Type" Font-Size="Small" />
                </td>
                 <td>
                     <asp:DropDownList ID="drpDataType" runat="server" OnSelectedIndexChanged="drpDataType_SelectedIndexChanged" AutoPostBack="true" CssClass="styled-select" Width="100px">
                         <asp:ListItem Selected="True">String</asp:ListItem>
                         <asp:ListItem>Date</asp:ListItem>
                         <asp:ListItem>Number</asp:ListItem>
                     </asp:DropDownList>
                </td>
                 </asp:Panel>
                 <td>
                     <asp:Label ID="Label4" runat="server" Text="Comparator" Font-Size="Small" />
                </td>
                 <td>
                     <asp:DropDownList ID="drpComparator" runat="server" AutoPostBack="true" CssClass="styled-select" Width="100px">
                         <asp:ListItem>Equals</asp:ListItem>
                         <asp:ListItem>Contains</asp:ListItem>
                         <asp:ListItem>Starts With</asp:ListItem>
                         <asp:ListItem>Ends With</asp:ListItem>
                         
                         <asp:ListItem>Is Empty</asp:ListItem>
                         <asp:ListItem>Is Not Empty</asp:ListItem>
                     </asp:DropDownList>
                </td>
                 <td>
                     <asp:Label ID="Label5" runat="server" Text="Value" Font-Size="Small" />
                </td>
                 <td>
                     <asp:TextBox ID="txtValue" runat="server" CssClass="styled-text"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txtValueCalendarExtender" runat="server" TargetControlID="txtValue" Enabled="false"></ajaxToolkit:CalendarExtender>
                </td>
                 <td>
                     <asp:Button ID="btnAddCondition" runat="server" Text="Add" OnClick="btnAddCondition_Click" CssClass="ECN-Button-Small"/>
                </td>
            </tr>
        </table><br />
        <table width="100%" >
            <tr>
                <td align="center">
                    <asp:GridView  ID="gvRuleCondition" runat="server" AutoGenerateColumns="false" 
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="RuleConditionID"  CssClass="grid" GridLines="Horizontal"
                            OnRowCommand="gvRuleCondition_RowCommand" ShowEmptyTable="true">
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>                                    
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRuleConditionID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RuleConditionID") %>'></asp:Label>
                                        </ItemTemplate>                                                            
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="2%" HeaderText="Field" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                               <asp:Label ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Field") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Comparator" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComparator" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Comparator") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="2%" HeaderText="Value" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Value") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="RuleConditionDelete" OnClientClick="return confirm('Are you sure, you want to delete this Condition?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("RuleConditionID")%>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                        <AlternatingRowStyle CssClass="grd_alternate" />
                  </asp:GridView>
                </td>
            </tr>
        </table>
    </div>