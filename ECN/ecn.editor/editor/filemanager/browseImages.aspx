<%@ Page Language="c#" AutoEventWireup="false" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="System.Web.UI.HtmlControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Image Browser</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet_default.css" type="text/css">

    <script type="text/javascript" src="/ecn.accounts/js/overlib/overlib.js"></script>

</head>
<body>
    <div id="overDiv" style="position: absolute; visibility: hidden; z-index: 1000;">
    </div>

    <script language="javascript">
		function getit(URL) {
			window.opener.setImage(URL) ;
			window.close() ;
		}
    </script>

    <script runat="Server">
protected override void OnLoad(EventArgs e) {
			string ImagesWebPathURL = "";
			string custID	= Request.QueryString["cuID"].ToString();
			string chID		= Request.QueryString["chID"].ToString();
			string folder		= "";
			try{
				folder			= Request.QueryString["folder"].ToString();
			}catch{
				folder = "";
			}

			try{
				ImagesWebPathURL="/ecn.accounts/assets/channelID_"+chID+"/customers/"+custID+"/collector/images";
			}catch{
				Response.Write("Error Occured. Please Contact System Administrator");
			}
	
			System.IO.DirectoryInfo dir = null;
			string[] dirs			= null;
			string foldersCode		= "";
			string dirname		= "";	
			string selectedStyle = "class='gridaltrow'";
			string normalStyle = "class='tableContent'";
			dirs = System.IO.Directory.GetDirectories(Server.MapPath(ImagesWebPathURL));
			dir = new System.IO.DirectoryInfo(Server.MapPath(ImagesWebPathURL));	

			foldersCode += "<tr "+((folder.Length > 0)?normalStyle:selectedStyle)+"><td colspan='3'><table border='0'><tr><td><img src="+((folder.Length > 0)?"'/ecn.images/icons/folder_img_25_closed.gif'":"'/ecn.images/icons/folder_img_25_open.gif'")+"></td><td valign=middle>&nbsp;&nbsp;<a href='browseImages.aspx' >ROOT</a></td></tr></table></td></tr>";	

			for (int i=0; i<=dirs.Length-1; i++) {	
				dir = new System.IO.DirectoryInfo(dirs[i]);
				dirname = dir.Name.ToString();
				foldersCode += "<tr><td width=20></td><td style='Padding-bottom:3px' align='center' class='tableContent' "+((folder.Equals("/"+dirname))?"bgcolor='#dde4e8'":"")+"><img src="+((folder.Equals("/"+dirname))?"'/ecn.images/icons/folder_img_25_open.gif'":"'/ecn.images/icons/folder_img_25_closed.gif'")+"></td><td style='Padding-left:4px;Padding-top:4px' valign=middle "+((folder.Equals("/"+dirname))?"bgcolor='#dde4e8'":"")+">";

				foldersCode += "<a href='browseImages.aspx?chID="+chID+"&cuID="+custID+"&folder=/"+dirname+" '>"+ dirname+"</a></td></tr>";
			}

			FolderSrc.Text = foldersCode;
			
			ImagesWebPathURL += folder;		
				
			DataTable dtPictures = new DataTable();
			DataColumn dcPicturesImage = new DataColumn("ThumbImage",typeof(string));
			DataColumn dcChckBx = new DataColumn("DeleteChckBx",typeof(string));
			DataRow drPictures;
			dtPictures.Columns.Add(dcPicturesImage);
			dtPictures.Columns.Add(dcChckBx);

			System.IO.FileInfo file = null;
			string[] currentFolderFiles = null;
			string filename = "";	
			currentFolderFiles = System.IO.Directory.GetFiles(Server.MapPath(ImagesWebPathURL),"*.*");

			for (int i=0; i<=currentFolderFiles.Length-1; i++) {
				//Create a new FileInfo object for this filename
				file = new System.IO.FileInfo(currentFolderFiles[i]);
				filename = file.Name.ToString();
				string mouseAlt = "", imgContent = "", chkBxContent = "";
				if (filename.ToLower().EndsWith(".jpg") || filename.ToLower().EndsWith(".gif") ) {

					//System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(Server.UrlPathEncode(ImagesWebPathURL+"/"+file.Name)));
					mouseAlt		= "<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: #B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;\\'>";
					mouseAlt	  += "<table border='0' width=350><tr>";
					mouseAlt	  += "<td><img src=\\'/ecn.communicator/includes/thumbnail.aspx?size=200&image="+Server.UrlPathEncode(ImagesWebPathURL+"/"+file.Name)+"\\'></td>";
					mouseAlt	  += "<td class=TableContent><b>Name: "+file.Name.ToString()+"</b><br />"+
																"<br /><b>Size:</b> "+Convert.ToInt32(file.Length/1000).ToString()+"kb"+
																"<br /><b>Date:</b> "+file.LastWriteTime.ToShortDateString()+
																//"<br /><b>Height:</b> "+g.Height.ToString()+" pixels "+
																//"<br /><b>Width:</b> "+g.Width.ToString()+" pixels "+
																//"<br /><b>Resolution:</b> "+g.HorizontalResolution.ToString()+" dpi";
																"<br /><br /><b><u>NOTE:</u></b><br />Click on the image to view its original size in a separate browser window.";
					mouseAlt	  += "</td></tr></table></div>";

					imgContent = "<tr><td valign=middle align='center' colspan=2><img style='cursor:hand;' src='/ecn.communicator/includes/thumbnail.aspx?size=100&image="+Server.UrlPathEncode(ImagesWebPathURL+"/"+file.Name)+"' onclick=\"javascript:window.open('"+Server.UrlPathEncode(ImagesWebPathURL+"/"+file.Name)+"')\" onmouseover=\"return overlib('"+mouseAlt+"', FULLHTML, VAUTO, HAUTO, LEFT, WIDTH, 350);\" onmouseout=\"return nd();\"></td></tr>";
					

					string fileNameFull = ImagesWebPathURL+"/"+filename;
					chkBxContent = "<tr><td valign=bottom align='right' height=10% class=tableContent></td><td valign=bottom align='right' width=10% class=tableContent><a href='#' onClick=\"JavaScript:getit('"+fileNameFull+"');\"><img src='/ecn.images/images/icon-add.gif' border='0' ></a></td></tr>";						

					drPictures = dtPictures.NewRow();
					drPictures[0]=imgContent;
					drPictures[1]=chkBxContent;
					dtPictures.Rows.Add(drPictures);
				}
			}
			DataView dvPictures = new DataView(dtPictures);
			imagerepeater.DataSource = dvPictures;
			imagerepeater.DataBind();
}
    </script>

    <form id="ImageBrowseForm" method="post" enctype="multipart/form-data" runat="Server">
        <table cellspacing="0" cellpadding="0" width="100%" border='0' style="border-top: #B6BCC6 1px solid;
            border-bottom: #B6BCC6 1px solid; border-left: #B6BCC6 1px solid; border-right: #B6BCC6 1px solid;">
            <tr>
                <td align="left" style="padding-left: 7px; padding-top: 7px" valign="top" width="20%">
                    <table align="left" class="tableContent" border='0' cellspacing="0" width="100%">
                        <asp:Label ID="FolderSrc" runat="Server" Text="Name"></asp:Label>
                    </table>
                </td>
                <td align='right' width="80%">
                    <asp:DataList ID="imagerepeater" runat="Server" CellSpacing="0" CellPadding="4" GridLines="None"
                        BorderWidth="0" BorderColor="black" RepeatColumns="5" RepeatLayout="Table">
                        <ItemTemplate>
                            <fieldset style="height: 135px">
                                <table class="tableContent" align="center" border='0' height="135px" width="100%">
                                    <%#DataBinder.Eval(Container.DataItem,"ThumbImage")%>
                                    <%#DataBinder.Eval(Container.DataItem,"DeleteChckBx")%>
                                </table>
                            </fieldset>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
