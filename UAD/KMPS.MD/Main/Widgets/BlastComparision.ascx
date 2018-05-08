<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlastComparision.ascx.cs" Inherits="KMPS.MD.Main.Widgets.BlastComparision" %>
   <asp:UpdatePanel ID="UpdatePanelBlastComparision" runat="server" UpdateMode="Conditional">
      <ContentTemplate>  
     <asp:Panel ID="PnlChart" runat="server">
   <table width="100%">
          <tr align="center">
            <td>
                <asp:Chart ID="chtBlastComparision" runat="server" BackColor="#D3DFF0"  AntiAliasing="Graphics"   BackGradientStyle="TopBottom"   >                           
                </asp:Chart>
            </td>
    </tr>
    </table>
    </asp:Panel>    

    <asp:Panel ID="PnlSettings" runat="server" Visible="false">
    <table>
     <tr>
            <td>
                Groups&nbsp
            </td>
            <td>
                View <asp:DropDownList ID="dropdownBlastsNum" runat="server">
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>&nbsp Most Recent Blasts
            </td>
    </tr>
      <tr>
            <td colspan="2">  
          <%--  <script type="text/javascript" language="javascript">
                function onClientSelectedIndexChangingHandler(source, arguments) {
                    var listbox = document.getElementById('<%=listboxGroups.ClientID%>');
                    var selectedCount = 0;
                    for (var index = 0; index < listbox.options.length; index++) {
                        if (listbox.options[index].selected)
                            selectedCount += 1;
                    }
                    if (selectedCount < 5)
                        arguments.IsValid = true;
                    else
                        arguments.IsValid = false;

                } 
            </script>         --%>
                <asp:listbox id="listboxGroups" rows="4" width="100%"  selectionmode="Multiple" 
                 runat="Server" CausesValidation="true"/>
              <%--   <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You can select a maximum of 5 groups" 
            ClientValidationFunction="onClientSelectedIndexChangingHandler"  ValidationGroup="bvalidate"></asp:CustomValidator>--%>
            </td>
    </tr>
    <tr>
            <td>
                    <asp:checkboxlist id="ReportOn" runat="Server" class="formfield" repeatdirection="horizontal"
                    repeatcolumns="6" repeatlayout="table" textalign="Right" cellspacing="1" cellpadding="5">
				        <asp:ListItem selected="True" value="open">Opens</asp:ListItem>
				        <asp:ListItem selected="True" value="click">Clicks</asp:ListItem>
				        <asp:ListItem value="bounce">Bounces</asp:ListItem>
				        <asp:ListItem value="subscribe">Opt-outs</asp:ListItem>
				        <asp:ListItem value="complaint">Complaints</asp:ListItem>
                    </asp:checkboxlist>
            </td>
            <td>
                    <asp:LinkButton ID="DrawChartButton" runat="Server" onclick="DrawChart" text="Apply Filters" ValidationGroup="bvalidate"/>
            </td>
    </tr>
    </table>  
    </asp:Panel>                                
<asp:Label ID="Label1" runat="server"></asp:Label>
  </ContentTemplate>  
</asp:UpdatePanel>