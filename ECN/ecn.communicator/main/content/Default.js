function deleteContent(theID) {
	if (confirm("Are you Sure?\n Selected content will be permanently deleted.")) {
		window.location = "default.aspx?ContentID=" + theID + "&action=delete";
	}
}

function deleteLayout(theID) {
	if (confirm("Are you Sure?\n Selected Message and all blast previews\n associated with Message will be permanently deleted.")) {
		window.location = "default.aspx?LayoutID=" + theID + "&action=delete";
	}
}

function MM_goToURL() { //v3.0
	var i, args = MM_goToURL.arguments;
	document.MM_returnValue = false;
	for (i = 0; i < (args.length - 1); i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
}