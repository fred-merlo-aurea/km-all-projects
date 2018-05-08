function NAV_getObject(id)
{
	return document.getElementById(id);
}

function NAV_selectedIndex(id)
{
	var selectedIndex = document.getElementById(id).value;
	
	if (selectedIndex >= 0)
		return selectedIndex;
	else
		return 0;
}

function NAV_buttonOver(id, index)
{
	if (NAV_selectedIndex(id) != index)
	{
		var left = NAV_getObject(id + "_TabButton_Left" + index);
		var center = NAV_getObject(id + "_TabButton_Center" + index);
		var right = NAV_getObject(id + "_TabButton_Right" + index);
		
		left.src = eval(id + "_Images")[2];
		center.background = eval(id + "_Images")[5];
		right.src = eval(id + "_Images")[8];
	}
}

function NAV_buttonOut(id, index)
{
	if (NAV_selectedIndex(id) != index)
	{
		var left = NAV_getObject(id + "_TabButton_Left" + index);
		var center = NAV_getObject(id + "_TabButton_Center" + index);
		var right = NAV_getObject(id + "_TabButton_Right" + index);
		
		left.src = eval(id + "_Images")[0];
		center.background = eval(id + "_Images")[3];
		right.src = eval(id + "_Images")[6];
	}
}

function NAV_selectTabPage(id, index)
{
	var tab = NAV_getObject(id + "_TabButton" + index);
	var oldIndex = NAV_selectedIndex(id);

	// Make the old tab content hidden
	var activeTab = NAV_getObject(id + "_TabPage" + oldIndex);
	activeTab.style.visibility = 'hidden';
	activeTab.style.display = 'none';
	
	// Make the new tab content visible
	var newTab = NAV_getObject(id + "_TabPage" + index);
	newTab.style.visibility = 'visible';
	newTab.style.display = 'block';
	
	// Set the selected index
	document.getElementById(id).value = index;
	
	// Set on state on the clicked button
	var left = NAV_getObject(id + "_TabButton_Left" + index);
	var center = NAV_getObject(id + "_TabButton_Center" + index);
	var right = NAV_getObject(id + "_TabButton_Right" + index);
	
	left.src = eval(id + "_Images")[1];
	center.background = eval(id + "_Images")[4];
	right.src = eval(id + "_Images")[7];
	
	// Set off state on the previous button
	NAV_buttonOut(id, oldIndex);
}