/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.dialog.add('about', function (a) { var b = a.lang.about; return { title: CKEDITOR.env.ie ? 'Browser Information' : 'Browser Information', minWidth: 500, minHeight: 350, contents: [{ id: 'tab1', label: '', title: '', expand: true, padding: 0, elements: [{ type: 'html', html: '<style type="text/css">.cke_about_container{color:#000 !important;padding:10px 10px 0;margin-top:5px}.cke_about_container p{margin: 0 0 10px;}.cke_about_container .cke_about_container a' + '{' + 'cursor:pointer !important;' + 'color:blue !important;' + 'text-decoration:underline !important;' + '}' + '</style>' + '<div class="cke_about_container">' + '<div class="cke_about_logo"></div>' + '<p><b>User Agent:<\/b><br>' + window.navigator.userAgent + '<br><br><b>Browser:<\/b><br>' + window.navigator.appName + ' ' + window.navigator.appVersion + '<br><br><b>Platform:<\/b><br>' + window.navigator.platform + '<br><br></p>' + '</div>'}]}], buttons: [CKEDITOR.dialog.cancelButton] }; });
