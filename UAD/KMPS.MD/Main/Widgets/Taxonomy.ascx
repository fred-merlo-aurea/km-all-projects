<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Taxonomy.ascx.cs" Inherits="KMPS.MD.Main.Widgets.Taxonomy" %>

<asp:UpdatePanel ID="UpdatePanelTaxonomy" runat="server" UpdateMode="Conditional">
      <ContentTemplate>    
        <asp:Panel ID="PnlChart" runat="server"> 
       <table width="100%">
          <tr align="center">
              <td>
                 
                  <asp:Chart ID="chtTaxonomy" runat="server"  BackImageTransparentColor="White" 
                    BackSecondaryColor="White"  BackColor="#D3DFF0"  BackGradientStyle="TopBottom" >                            
                 </asp:Chart>
              </td>
         
           <tr>
      </table>
    </asp:Panel>    

    <asp:Panel ID="PnlSettings" runat="server" Visible="false">
    
      <table>  
      <tr>    
            <td>
                <asp:Label ID="lblMonthWeek" runat="server" Text="Number of of Month(s):"></asp:Label>
                &nbsp;<asp:DropDownList ID="dropdownMonths" runat="server">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem Selected="True">2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                <asp:LinkButton ID="lbtnApplyFilters" runat="server" 
                    onclick="lbtnApplyFilters_Click">Apply Filters</asp:LinkButton>
               </td>
                  
          </tr>
      </table>      
    </asp:Panel>    
    
       
          <asp:Label ID="Label1" runat="server"></asp:Label>
        </ContentTemplate>
</asp:UpdatePanel>
