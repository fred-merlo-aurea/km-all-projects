<%@ Import Namespace="ecn.communicator.classes" %>
<%@ Import Namespace="ecn.common.classes" %>
<%@ Import Namespace="System.Web.SessionState" %>

<%@ Page language="c#" AutoEventWireup="false" CodeBehind="browseDoc.aspx.cs" Inherits="ecn.communicator.contentmanager.editor.browseDoc" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<title>Files Browser</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>
<body>
<SCRIPT language=javascript>
function getit(URL) {
	window.opener.setImage(URL) ;
	window.close() ;
}
</script>
<script runat="server">
protected override void OnLoad(EventArgs e) {
	SecurityCheck securityCheck = new SecurityCheck();
	ChannelCheck cc = new ChannelCheck();
			
	string ImagesWebPath=cc.getAssetsPath("accounts")+"/channelID_"+securityCheck.ChannelID()+"/customers/"+securityCheck.CustomerID()+"/data/";

	string sMapImg = Server.MapPath( ImagesWebPath );
	string[] files = null;	
	files = System.IO.Directory.GetFiles(sMapImg,"*.*");
	System.IO.FileInfo file = null;	
	string filename = "", fileNameFull = "";;	
	
	for (int i=0; i<=files.Length-1; i++) {	
		file = new System.IO.FileInfo(files[i]);
		filename = file.Name.ToString();
		fileNameFull = "http://"+cc.getHostName()+ImagesWebPath+filename;
		Response.Write("<font face=verdana size=1><a href='#' onClick=\"JavaScript:getit('"+fileNameFull+"');\">"+ filename+ "</a></font><br>");
	}
}
</script>
</body>
</html>