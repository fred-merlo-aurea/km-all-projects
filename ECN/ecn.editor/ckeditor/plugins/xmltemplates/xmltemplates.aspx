<%@ Page language="c#" AutoEventWireup="true" Inherits="ecn.communicator.contentmanager.ckeditor.dialog.xmltemplates" Codebehind="xmltemplates.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

  <head runat="server">
    <title>Templates</title>
        <base target="_self" />		
      <script type="text/javascript">
          //if (window.attachEvent) {
          //    window.attachEvent("onload", initDialog);
          //}
          //else if (window.addEventListener) {
          //    window.addEventListener("load", initDialog, false);
          //}

          //function getRadWindow() {
          //    if (window.radWindow) {
          //        return window.radWindow;
          //    }

          //    if (window.frameElement && window.frameElement.radWindow) {
          //        return window.frameElement.radWindow;
          //    }

          //    return null;
          //}


          //function initDialog() {
          //    var clientParameters = getRadWindow().ClientParameters; //return the arguments supplied from the parent page   


          //}

          //function ok(selected) {
          //    selected = selected.nextSibling;
          //    while (selected.nodeType != 1) {
          //        selected = selected.nextSibling;
          //    }
          //    getRadWindow().close(selected.value);
          //}
          //function cancel() {
          //    getRadWindow().close();
          //}
      </script>
		<script language="JavaScript" type="text/JavaScript">
		    function ok(selected) {
		        selected = selected.nextSibling;
		        while (selected.nodeType != 1) {
		            selected = selected.nextSibling;
		        }
		        if (window.opener) {
		            if (window.opener.setData != undefined) {
		                window.opener.setData(selected.value.toString());
		            }
		        }

		        this.close();
		    }

			function cancel() {
				window.returnValue ="" ;
			}			
		</script>
       
</head>
	<body>
     <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <table width="100%" cellpadding="10">
            </HeaderTemplate>
                <ItemTemplate>      
                    <%# (Container.ItemIndex + 4) % 4 == 0 ? "<tr>" : string.Empty %>
                        <td align="center"  valign="top" nowrap>   
                            <asp:Label ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "templateTitle") %>'></asp:Label>
                            <br />                      
                            <a href="javascript:void(0)" onclick="ok(this);"><asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "templateImage") %>' BorderWidth="1px" BorderColor="DimGray"/></a>

                           <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "templateHTML") %>'/>
                        </td>
                    <%# (Container.ItemIndex + 4) % 4 == 3 ? "</tr>" : string.Empty %>
                </ItemTemplate>
            <FooterTemplate>
            </table>
            </FooterTemplate>
            </asp:Repeater>    
    </form>
	</body>
</html>
