<%@ Page language="c#" AutoEventWireup="false"%>

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
	//alert(URL);
	window.close() ;
}
</script>
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        string ImagesWebPath = "";
        try
        {
            ImagesWebPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Request.QueryString["cuID"].ToString() + "/data/");

            if (!System.IO.Directory.Exists(ImagesWebPath))
                System.IO.Directory.CreateDirectory(ImagesWebPath);

        }
        catch (Exception ex)
        {
            Response.Write("Error Occured. Please Contact System Administrator");
        }

        string[] files = null;
        files = System.IO.Directory.GetFiles(ImagesWebPath, "*.*");
        System.IO.FileInfo file = null;
        string filename = "", fileNameFull = ""; ;

        for (int i = 0; i <= files.Length - 1; i++)
        {
            file = new System.IO.FileInfo(files[i]);
            filename = file.Name.ToString();
            fileNameFull = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"].ToLower().Replace("http://","") + "/customers/" + Request.QueryString["cuID"].ToString() + "/data/" + filename;
            Response.Write("<font face=verdana size=1><a href='#' onClick=\"JavaScript:getit('" + fileNameFull + "');\">" + filename + "</a></font><br>");
        }
    }
</script>
</body>
</html>