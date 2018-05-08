/*
 *  Javascript routines to handle highlighting and show/hide of the main menu
 *  Note:  this relies on both jQuery and ddmenu
 *  2015-050-12: Created (corwin)
 */

var highlightedClass = 'highlighted';
var MyOpen = function (parent, childId) {
    $(parent).addClass(highlightedClass);
    if (childId) {
        mopen(childId);
    }
};
var MyClose = function (parent, childId) {
    if (childId) {
        mclosetime();
    }
    $(parent).removeClass(highlightedClass);
};
var MyOver = function (item, parentId) {
    $(item).addClass(highlightedClass);
    $('#' + parentId).addClass(highlightedClass);
};
var MyOut = function (item, parentId) {
    $(item).removeClass(highlightedClass);
    $('#' + parentId).removeClass(highlightedClass);
};