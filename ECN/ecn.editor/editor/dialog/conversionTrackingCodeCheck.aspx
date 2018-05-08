<%@ Page language="c#" AutoEventWireup="false" %>
<script runat="server">
protected override void OnLoad(EventArgs e) {
    ecn.common.classes.ECN_Framework_BusinessLayer.Application.ECNSession es = ecn.common.classes.ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
    
	if(es.HasProductFeature("ecn.communicator","Conversion Tracking")){
		Response.Write("document.write(\"<span><input type='checkbox' value='' NAME='conversionCheckbx' onclick='conversion()' ID='conversionCheckbx'>Add Conversion Tracking Code to URL</span>\")");
	}else{
		Response.Write("document.write(\"<span><input type='checkbox' value='' NAME='conversionCheckbx' onclick='conversion()' ID='conversionCheckbx'  disabled>Add Conversion Tracking Code to URL</span>\")");	
	}
}
</script>