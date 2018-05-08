<%@ Register TagPrefix="ecn" TagName="gallery" Src="~/ckeditor/controls/imageGallery.ascx" %>

<%@ Page Language="c#" Inherits="ecn.editor.ckeditor.filemanager" Codebehind="FileManager.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Image Browser</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet_default.css" type="text/css">
    <style type='text/css'>
#maingallery_ImageListGrid	a
{
	text-decoration:none;
}

#maingallery_ImageListGrid h5
{
	font-size:11px;
	padding:0;
	margin:0;
	text-align:left;
	text-indent:14px;
	background:url(/ecn.images/images/sort_btn.gif) 0 -3px no-repeat;
}

</style>
</head>
<body>

    <script language="javascript">
		function getit(URL) {

		    var funcNum = getUrlParam('CKEditorFuncNum');
		    window.opener.CKEDITOR.tools.callFunction(funcNum, URL);
			window.close() ;
}

function getUrlParam(paramName)
{
    var reParam = new RegExp('(?:[\?&]|&amp;)' + paramName + '=([^&]+)', 'i');
    var match = window.location.search.match(reParam);

    return (match && match.length > 1) ? match[1] : '';
}

    </script>

    <form id="ImageManagerForm" method="post" enctype="multipart/form-data" runat="Server">
        <ecn:gallery ID="maingallery" runat="Server" borderWidth="0" imagesPerColumn="5"
            thumbnailSize="100"></ecn:gallery>
    </form>
</body>
</html>
