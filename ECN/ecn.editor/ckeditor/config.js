/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/



CKEDITOR.editorConfig = function (config) {

    config.toolbar = "Custom";
    config.toolbar = "Full";

    config.extraPlugins = 'codesnippet,transnippet,xmltemplates,htmlUpload,dynamictag,rssfeed,socialshare';
    config.removePlugins = 'print,div,blockquote';
    config.htmlUploadUrl = '/ecn.editor/ckeditor/plugins/htmlUpload/htmlUpload.aspx';
    config.dynamictagUrl = '/ecn.editor/ckeditor/plugins/dynamictag/dynamictag.aspx';
    config.codesnippetUrl = '/ecn.editor/ckeditor/plugins/codesnippet/codeSnippets.aspx';
    config.transnippetUrl = '/ecn.editor/ckeditor/plugins/transnippet/tranSnippets.aspx';
    config.xmltemplateUrl = '/ecn.editor/ckeditor/plugins/xmltemplates/xmltemplates.aspx';
    config.rssfeedUrl = '/ecn.editor/ckeditor/plugins/rssfeed/rssfeed.aspx';
    config.mobilehtmlUploadUrl = '/ecn.editor/ckeditor/plugins/mobileHtmlUpload/htmlUpload.aspx';
    config.sourcehtmlUploadUrl = '/ecn.editor/ckeditor/plugins/sourceHtmlUpload/htmlUpload.aspx';
    config.socialshareUrl = '/ecn.editor/ckeditor/plugins/socialshare/socialshare.aspx';
    config.format_tags = "p;h1;h2;h3;h4;h5;h6;pre;address;div";
    config.defaultLanguage = "en";

   

    config.transnippet = {
        htmlFromEditor: '',
        htmlTransHTML : ''
    }

    config.filebrowserBrowseUrl = '/ecn.editor/ckeditor/filemanager/FileManager.aspx?chID=12&cuID=1';
    config.filebrowserImageBrowseUrl = '/ecn.editor/ckeditor/filemanager/FileManager.aspx?chID=12&cuID=1';

    //link
    config.linkShowAdvancedTab = false;
    config.linkShowTargetTab = true;

    config.enterMode = CKEDITOR.ENTER_BR ;

    config.ProtectedTags = 'TRANSNIPPET|TRANSNIPPET_DETAIL|ecn_dt';

    config.fullPage = false;

    config.baseFloatZIndex = 100000;

    //Display Special Symbols
    config.entities = true;

    config.indentOffset = 4;

    //Compatible Config Settings from FCKconfig.js(old)
    config.bodyId = '';
    config.bodyClass = '';
    config.docType = '';
    config.baseHref = '';   
    config.tabSpaces = 0;
    config.forcePasteAsPlainText = false;
    config.entities_processNumerical = true;
    config.browserContextMenuOnCtrl = false;
    config.forceSimpleAmpersand = false;
    config.disableObjectResizing = false;
    config.entities_greek = true;
    config.entities_latin = true;
    config.fillEmptyBlocks = true;
    config.startupShowBorders = true;
};


CKEDITOR.on('instanceReady', function (ev) {
    // Ends self closing tags the HTML4 way, like <br>.
    ev.editor.dataProcessor.htmlFilter.addRules(
    {
        elements:
        {
            $: function (element) {
                // Output dimensions of images as width and height
                if (element.name == 'img') {
                    var style = element.attributes.style;

                    if (style) {
                        // Get the width from the style.
                        var match = /(?:^|\s)width\s*:\s*(\d+)px/i.exec(style),
                            width = match && match[1];

                        // Get the height from the style.
                        match = /(?:^|\s)height\s*:\s*(\d+)px/i.exec(style);
                        var height = match && match[1];

                        // Get the float value from the style.
                        match = /(?:^|\s)float\s*:\s*(left|right)/i.exec(style);
                        var align = match && match[1];

                        if (width) {
                            element.attributes.style = element.attributes.style.replace(/(?:^|\s)width\s*:\s*(\d+)px;?/i, '');
                            element.attributes.width = width;
                        }

                        if (height) {
                            element.attributes.style = element.attributes.style.replace(/(?:^|\s)height\s*:\s*(\d+)px;?/i, '');
                            element.attributes.height = height;
                        }

                        if (align) {
                            element.attributes.style = element.attributes.style.replace(/(?:^|\s)float\s*:\s*(left|right);?/i, '');
                            element.attributes.align = align;
                        }
                    }
                }



                if (!element.attributes.style)
                    delete element.attributes.style;

                return element;
            }
        }
    });
});