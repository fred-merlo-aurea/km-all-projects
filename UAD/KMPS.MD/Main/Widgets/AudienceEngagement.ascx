<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AudienceEngagement.ascx.cs" Inherits="KMPS.MD.Main.Widgets.AudienceEngagement" %>


<asp:UpdatePanel ID="UpdatePanelAudienceEngagement" runat="server" UpdateMode="Conditional">
      <ContentTemplate> 
          <asp:Panel ID="PnlChart" runat="server">
          <table width="100%">
          <tr align="center">
                  <td>                 
                      <asp:Chart ID="chtAudienceEngagement" runat="server" Palette="BrightPastel" BackSecondaryColor="White"  
                        BackColor="#D3DFF0"  BackGradientStyle="TopBottom"  AntiAliasing="Graphics">                            
                     </asp:Chart>
                  </td>                 
              </tr>    
          </table> 
          </asp:Panel>    

          <asp:Panel ID="PnlSettings" runat="server" Visible="false">
          <table>
              <tr>
                    <td>
                        <asp:Label ID="lblGroupList" runat="server" Text="Group/List"></asp:Label>
                    </td>
                    <td>    
                        <asp:DropDownList ID="drpGroup" runat="server">
                        </asp:DropDownList>
                    </td>
              </tr>
              <tr>
                     <td>
                        <asp:Label ID="lblClick" runat="server" Text="Click %"></asp:Label>
                    </td>
                    <td>    <asp:TextBox ID="txtClickPercent" runat="server" Text="35"></asp:TextBox>
                
                    </td>
             </tr>
             <tr>
                     <td>
                        <asp:Label ID="lblDays" runat="server" Text="Days" ></asp:Label>
                    </td>
                    <td>    <asp:TextBox ID="txtDays" runat="server"  Text="60"></asp:TextBox>
             
                    </td>        
              </tr>
          </table> 
              <asp:Button ID="btnApply" runat="server" Text="Apply" 
                  onclick="btnApply_Click" />       
          </asp:Panel>
         
          <asp:Label ID="Label1" runat="server"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>