<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScreenSize.ascx.cs" Inherits="KMPS.MD.Main.Widgets.ScreenSize" %>
<input type="hidden" name="_height" value="<%#get_height()%>" id="_height" />
<input type="hidden" name="_width" value="<%#get_width()%>" id="_width" />
 
<%if (_IsFirstTime){%>
<script language="javascript">
    var rowURL = window.location.href;
    var _objHeight = document.getElementById("_height");
    var _objWidth = document.getElementById("_width");
//    if (_objHeight.value == "" && _objWidth.value == "")
//        window.location.href = window.location.href + "?_height=" + window.screen.availHeight + "&_width=" + window.screen.availWidth;
//    else
//        window.location.href = window.location.href.replace("_height=" + _objHeight.value, "_height=" + window.screen.availHeight).replace("_width=" + _objWidth.value, "_width=" + window.screen.availWidth);
// </script> 
<%}%>
