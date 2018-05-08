// Copyright � 2001 by Apple Computer, Inc., All Rights Reserved.
//
// You may incorporate this Apple sample code into your own code
// without restriction. This Apple sample code has been provided "AS IS"
// and the responsibility for its operation is yours. You may redistribute
// this code, but you are not permitted to redistribute it as
// "Apple sample code" after having made changes.

// ugly workaround for missing support for selectorText in Netscape6/Mozilla
// call onLoad() or before you need to do anything you would have otherwise used
// selectorText for.
var ugly_selectorText_workaround_flag = false;
var allStyleRules;
// code developed using the following workaround (CVS v1.15) as an example.
// http://lxr.mozilla.org/seamonkey/source/extensions/xmlterm/ui/content/XMLTermCommands.js
function ugly_selectorText_workaround() {
	if((navigator.userAgent.indexOf("Gecko") == -1) ||
	   (ugly_selectorText_workaround_flag)) {
		return; // we've already been here or shouldn't be here
	}
	var styleElements = document.getElementsByTagName("style");
	
	for(var i = 0; i < styleElements.length; i++) {
		var styleText = styleElements[i].firstChild.data;
		// this should be using match(/\b[\w-.]+(?=\s*\{)/g but ?= causes an
		// error in IE5, so we include the open brace and then strip it
		allStyleRules = styleText.match(/\b[\w-.]+(\s*\{)/g);
	}

	for(var i = 0; i < allStyleRules.length; i++) {
		// probably insufficient for people who like random gobs of 
		// whitespace in their styles
		allStyleRules[i] = allStyleRules[i].substr(0, (allStyleRules[i].length - 2));
	}
	ugly_selectorText_workaround_flag = true;
}


// setStyleById: given an element id, style property and 
// value, apply the style.
// args:
//  i - element id
//  p - property
//  v - value
//
function setStyleById(i, p, v) {
	var n = document.getElementById(i);
	n.style[p] = v;
}

// getStyleById: given an element ID and style property
// return the current setting for that property, or null.
// args:
//  i - element id
//  p - property
function getStyleById(i, p) {
	var n = document.getElementById(i);
	var s = eval("n.style." + p);

	// try inline
	if((s != "") && (s != null)) {
		return s;
	}

	// try currentStyle
	if(n.currentStyle) {
		var s = eval("n.currentStyle." + p);
		if((s != "") && (s != null)) {
			return s;
		}
	}
	
	// try styleSheets
	var sheets = document.styleSheets;
	if(sheets.length > 0) {
		// loop over each sheet
		for(var x = 0; x < sheets.length; x++) {
			// grab stylesheet rules
			var rules = sheets[x].cssRules;
			if(rules.length > 0) {
				// check each rule
				for(var y = 0; y < rules.length; y++) {
					var z = rules[y].style;
					// selectorText broken in NS 6/Mozilla: see
					// http://bugzilla.mozilla.org/show_bug.cgi?id=51944
					ugly_selectorText_workaround();
					if(allStyleRules) {
						if(allStyleRules[y] == i) {
							return z[p];
						}			
					} else {
						// use the native selectorText and style stuff
						if(((z[p] != "") && (z[p] != null)) ||
						   (rules[y].selectorText == i)) {
							return z[p];
						}
					}
				}
			}
		}
	}
	return null;
}

// setStyleByClass: given an element type and a class selector,
// style property and value, apply the style.
// args:
//  t - type of tag to check for (e.g., SPAN)
//  c - class name
//  p - CSS property
//  v - value
var ie = (document.all) ? true : false;

function setStyleByClass(t,c,p,v){

	var elements;
	if(t == '*') {
		// '*' not supported by IE/Win 5.5 and below
		elements = (ie) ? document.all : document.getElementsByTagName('*');
	} else {
		elements = document.getElementsByTagName(t);
	}
	
	for(var i = 0; i < elements.length; i++){
		var node = elements.item(i);
		for(var j = 0; j < node.attributes.length; j++) {
			if(node.attributes.item(j).nodeName == 'class') {
				if(node.attributes.item(j).nodeValue == c) {
					//alert(node.style.textAlign);
					eval('node.style.' + p + " = '" +v + "'");
					//alert(node.style.textAlign);
				}
			}
		}
	}
}

// getStyleByClass: given an element type, a class selector and a property,
// return the value of the property for that element type.
// args:
//  t - element type
//  c - class identifier
//  p - CSS property
function getStyleByClass(t, c, p) {
	// first loop over elements, because if they've been modified they
	// will contain style data more recent than that in the stylesheet
	var elements;
	if(t == '*') {
		// '*' not supported by IE/Win 5.5 and below
		elements = (ie) ? document.all : document.getElementsByTagName('*');
	} else {
		elements = document.getElementsByTagName(t);
	}
	for(var i = 0; i < elements.length; i++){
		var node = elements.item(i);
		for(var j = 0; j < node.attributes.length; j++) {
			if(node.attributes.item(j).nodeName == 'class') {
				if(node.attributes.item(j).nodeValue == c) {
					var theStyle = eval('node.style.' + p);
					if((theStyle != "") && (theStyle != null)) {
						return theStyle;
					}
				}
			}
		}		
	}
	// if we got here it's because we didn't find anything
	// try styleSheets
	var sheets = document.styleSheets;
	if(sheets.length > 0) {
		// loop over each sheet
		for(var x = 0; x < sheets.length; x++) {
			// grab stylesheet rules
			var rules = sheets[x].cssRules;
			if(rules.length > 0) {
				// check each rule
				for(var y = 0; y < rules.length; y++) {
					var z = rules[y].style;
					// selectorText broken in NS 6/Mozilla: see
					// http://bugzilla.mozilla.org/show_bug.cgi?id=51944
					ugly_selectorText_workaround();
					if(allStyleRules) {
						if((allStyleRules[y] == c) ||
						   (allStyleRules[y] == (t + "." + c))) {
							return z[p];
						}			
					} else {
						// use the native selectorText and style stuff
						if(((z[p] != "") && (z[p] != null)) &&
						   ((rules[y].selectorText == c) ||
						    (rules[y].selectorText == (t + "." + c)))) {
							return z[p];
						}
					}
				}
			}
		}
	}

	return null;
}

// setStyleByTag: given an element type, style property and 
// value, and whether the property should override inline styles or
// just global stylesheet preferences, apply the style.
// args:
//  e - element type or id
//  p - property
//  v - value
//  g - boolean 0: modify global only; 1: modify all elements in document
function setStyleByTag(e, p, v, g) {
	if(g) {
		var elements = document.getElementsByTagName(e);
		for(var i = 0; i < elements.length; i++) {
			elements.item(i).style[p] = v;
		}
	} else {
		var sheets = document.styleSheets;
		if(sheets.length > 0) {
			for(var i = 0; i < sheets.length; i++) {
				var rules = sheets[i].cssRules;
				if(rules.length > 0) {
					for(var j = 0; j < rules.length; j++) {
						var s = rules[j].style;
						// selectorText broken in NS 6/Mozilla: see
						// http://bugzilla.mozilla.org/show_bug.cgi?id=51944
						ugly_selectorText_workaround();
						if(allStyleRules) {
							if(allStyleRules[j] == e) {
								s[p] = v;
							}			
						} else {
							// use the native selectorText and style stuff
							if(((s[p] != "") && (s[p] != null)) &&
							   (rules[j].selectorText == e)) {
								s[p] = v;
							}
						}

					}
				}
			}
		}
	}
}

// getStyleByTag: given an element type and style property, return
// the property's value
// args:
//  e - element type
//  p - property
function getStyleByTag(e, p) {
	var sheets = document.styleSheets;
	if(sheets.length > 0) {
		for(var i = 0; i < sheets.length; i++) {
			var rules = sheets[i].cssRules;
			if(rules.length > 0) {
				for(var j = 0; j < rules.length; j++) {
					var s = rules[j].style;
					// selectorText broken in NS 6/Mozilla: see
					// http://bugzilla.mozilla.org/show_bug.cgi?id=51944
					ugly_selectorText_workaround();
					if(allStyleRules) {
						if(allStyleRules[j] == e) {
							return s[p];
						}			
					} else {
						// use the native selectorText and style stuff
						if(((s[p] != "") && (s[p] != null)) &&
						   (rules[j].selectorText == e)) {
							return s[p];
						}
					}

				}
			}
		}
	}

	// if we don't find any style sheets, return the value for the first
	// element of this type we encounter without a CLASS or STYLE attribute
	var elements = document.getElementsByTagName(e);
	var sawClassOrStyleAttribute = false;
	for(var i = 0; i < elements.length; i++) {
		var node = elements.item(i);
		for(var j = 0; j < node.attributes.length; j++) {
			if((node.attributes.item(j).nodeName == 'class') ||
			   (node.attributes.item(j).nodeName == 'style')){
			   sawClassOrStyleAttribute = true;
			}
		}
		if(! sawClassOrStyleAttribute) {
			return elements.item(i).style[p];
		}
	}
}
