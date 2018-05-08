/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.addTemplates('default',
{
    imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
    templates: getTemplates()
});

function CheckFileExists(A)
{
    var status = $.ajax({
        url: A,
        async: false
    }).status;

    if (status == 200 || status == 304)
    {
        return true;
    }
    else
    {
        return false;
    }
}

function getTemplates()
{

    // Load the XML file.
    var chID = "";
    var cuID = "";
    var xmlPath = "";
    var sURL = document.URL;
    
    if (sURL.indexOf("?") > 0)
    {
        var arrParams = sURL.split("?");

        var arrURLParams = arrParams[1].split("&");

        var arrParamNames = new Array(arrURLParams.length);
        var arrParamValues = new Array(arrURLParams.length);

        var i = 0;
        for (i = 0; i < arrURLParams.length; i++)
        {
            var sParam = arrURLParams[i].split("=");
            arrParamNames[i] = sParam[0];
            if (sParam[1] != "")
                arrParamValues[i] = unescape(sParam[1]);
            else
                arrParamValues[i] = "No Value";
        }

        for (i = 0; i < arrURLParams.length; i++)
        {
            if (arrParamNames[i] == "chID")
            {
                chID = arrParamValues[i];
            } else if (arrParamNames[i] == "cuID")
            {
                cuID = arrParamValues[i];
            }
        }
    }

    if (CheckFileExists(CKEDITOR.basePath + chID + '_' + cuID + '_' + 'fcktemplates.xml'))
    {
        xmlPath = CKEDITOR.basePath + chID + '_' + cuID + '_' + 'fcktemplates.xml';
    } else if (CheckFileExists(CKEDITOR.basePath + +chID + '_' + 'fcktemplates.xml'))
    {
        xmlPath = CKEDITOR.basePath + chID + '_' + 'fcktemplates.xml';
    } else if (CheckFileExists(CKEDITOR.basePath + 'fcktemplates.xml'))
    {
        xmlPath = CKEDITOR.basePath + 'fcktemplates.xml';
    }

    // Create the Templates array.
    var arrTemplates = new Array();

    $.ajax({
        type: "GET",
        url: xmlPath,
        dataype: "xml",
        async: false,
        success: function (xml)
        {

            var i = 0;

            $(xml).find('Template').each(function ()
            {
                var oTemplate = new Object();

                var template_title = $(this).attr('title');
                var template_image = $(this).attr('image');
                var template_description = $(this).find('Description').text();
                var template_Html = $(this).find('Html').text();

                oTemplate.title = template_title;
                oTemplate.image = template_image;
                oTemplate.description = template_description;
                oTemplate.html = template_Html;

                arrTemplates[i] = oTemplate;

                i = i + 1;

            }); //close each(
        }
    });   //close $.ajax(

    return arrTemplates;
}


//function gettempTemplates() {

//    var arr = new Array();

//    var oTemplate = new Object();

//    oTemplate.title = 'Image and Title';
//    oTemplate.image = 'template1.gif';
//    oTemplate.description = 'One main image with a title and text that surround the image.';
//    oTemplate.html = '<h3><img style="margin-right: 10px" height="100" width="100" align="left"/>Type the title here</h3><p>Type the text here</p>';

//    arr[0] = oTemplate

//    oTemplate = new Object();
//    oTemplate.title = 'Strange Template',
//        oTemplate.image = 'template2.gif',
//        oTemplate.description = 'A template that defines two colums, each one with a title, and some text.',
//        oTemplate.html = '<table cellspacing="0" cellpadding="0" style="width:100%" border="0"><tr><td style="width:50%"><h3>Title 1</h3></td><td></td><td style="width:50%"><h3>Title 2</h3></td></tr><tr><td>Text 1</td><td></td><td>Text 2</td></tr></table><p>More text goes here.</p>'

//    arr[1] = oTemplate

//    oTemplate.title = 'Text and Table',
//        oTemplate.image = 'template3.gif',
//        oTemplate.description = 'A title with some text and a table.',
//        oTemplate.html = '<div style="width: 80%"><h3>Title goes here</h3><table style="width:150px;float: right" cellspacing="0" cellpadding="0" border="1"><caption style="border:solid 1px black"><strong>Table title</strong></caption></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table><p>Type the text here</p></div>'

//    arr[2] = oTemplate

//    //        var arr = [{

//    //            title: 'Image and Title',
//    //            image: 'template1.gif',
//    //            description: 'One main image with a title and text that surround the image.',
//    //            html: '<h3><img style="margin-right: 10px" height="100" width="100" align="left"/>Type the title here</h3><p>Type the text here</p>'
//    //        },
//    //            {
//    //                title: 'Strange Template',
//    //                image: 'template2.gif',
//    //                description: 'A template that defines two colums, each one with a title, and some text.',
//    //                html: '<table cellspacing="0" cellpadding="0" style="width:100%" border="0"><tr><td style="width:50%"><h3>Title 1</h3></td><td></td><td style="width:50%"><h3>Title 2</h3></td></tr><tr><td>Text 1</td><td></td><td>Text 2</td></tr></table><p>More text goes here.</p>'
//    //            },
//    //            { title: 'Text and Table', image: 'template3.gif',
//    //                description: 'A title with some text and a table.',
//    //                html: '<div style="width: 80%"><h3>Title goes here</h3><table style="width:150px;float: right" cellspacing="0" cellpadding="0" border="1"><caption style="border:solid 1px black"><strong>Table title</strong></caption></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table><p>Type the text here</p></div>'
//    //            }];


//    return arr;
//}
