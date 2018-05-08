<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SocialFileManager.aspx.cs" Inherits="ecn.communicator.main.content.SocialFileManager" %>

<%@ Register TagPrefix="ecn" TagName="gallery" Src="~/main/ecnwizard/othercontrols/imageSelector.ascx" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Image Browser</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="/ecn.images/images/stylesheet_default.css" type="text/css">
    <style type='text/css'>
        #maingallery_ImageListGrid a {
            text-decoration: none;
        }

        #maingallery_ImageListGrid h5 {
            font-size: 11px;
            padding: 0;
            margin: 0;
            text-align: left;
            text-indent: 14px;
            background: url(/ecn.images/images/sort_btn.gif) 0 -3px no-repeat;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function getit(URL) {

            var imgToSet = getUrlParam('imgcontrol');
            var hfToSet = getUrlParam('hfToSet');
            var btnToSet = getUrlParam('btnToSet');
            if (imgToSet != undefined && hfToSet != undefined && btnToSet != undefined) {
                if (window.opener.setImgBtnURL != undefined) {
                    window.opener.setImgBtnURL(URL, imgToSet, hfToSet, btnToSet);

                }
            }
            window.close();
        }

        function getUrlParam(paramName) {
            var reParam = new RegExp('(?:[\?&]|&amp;)' + paramName + '=([^&]+)', 'i');
            var match = window.location.search.match(reParam);

            return (match && match.length > 1) ? match[1] : '';
        }

    </script>
</head>
<body>
    <form id="ImageManagerForm" method="post" enctype="multipart/form-data" runat="Server">
        <table style="width:100%;">
            <tr>
                <td>
                    <ecn:gallery ID="maingallery" runat="Server" borderWidth="0" imagesPerColumn="5"
                        thumbnailSize="100"></ecn:gallery>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
