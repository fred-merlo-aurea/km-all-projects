/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

(function () {
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined; 

            var media = window.open(editor.config.dynamictagUrl, "_blank", "width=200px,height=100px, resizable=yes, toolbar=no,status=no,menubar=no,location=no", false);

        }
    },

  b = 'dynamictag';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('dynamictag', {
                label: 'DynamicTag',
                icon: '/ecn.editor/ckeditor/plugins/dynamictag/dynamictag.jpg',
                command: b
            });
        }
    });

})();

CKEDITOR.config.dynamictag_base = "";