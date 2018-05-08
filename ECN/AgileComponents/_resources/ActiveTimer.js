function AWT_testIfScriptPresent()
{
}

function AWT_DoTimer(id, code, repeat, countdown, init, synch)
{
	// Process if enabled only
	if (AWT_IsEnabled(id))
	{
		// Get the interval
		var interval = 1000;
		
		// If countdown enabled, update the label
		if (countdown)
			AWT_UpdateLabel(id, code, synch);
		else
			interval = AWT_GetInterval(id);
			
		// Execute the code
		if (!init && !countdown)
			setTimeout(code, 10);

		// Repeat the timer ?
		if (repeat)
		{
			repeatStr = "AWT_DoTimer(\"" + id + "\", \"" + code + "\", " + repeat + ", " + countdown + ", false, " + synch + ");";
			setTimeout(repeatStr, interval);
		}
		// Define the one shot timer
		else
			setTimeout(code, interval);
	}
}

function AWT_UpdateLabel(id, code, synch)
{
	var code, amount;
	var dateNow = new Date();
		
	// Synchronize with server time;
	if (synch)
		dateNow = new Date(dateNow.getTime() - eval('AWT_DateDiff'));
		
	if (AWT_GetTargetDate(id).getFullYear() < 1902)
	{
		var t = new Date(dateNow.getTime() + AWT_GetInterval(id));
		AWT_SetTargetDate(id, t.getFullYear(), t.getMonth(), t.getDate(), t.getHours(), t.getMinutes(), t.getSeconds());
	}
	
	var dateFuture = AWT_GetTargetDate(id);

	// Get amount of remaining time in milliseconds.
	var amount = dateFuture.getTime() - dateNow.getTime();
	
	// If amount is less than 0, then countdown is over;
	if (amount < 0)
	{
		setTimeout(code, 1);
		if (code.indexOf("__doPostBack") == -1)
			AWT_DisableTimer(id);
	}
	// Else, update the label.
	else
	{
		var days = 0;
		var hours = 0;
		var mins = 0;
		var secs = 0;

		amount = Math.floor(amount/1000); //kill the milliseconds

		days = Math.floor(amount/86400); //days
		amount = amount % 86400;

		hours = Math.floor(amount/3600); //hours
		amount = amount % 3600;

		mins = Math.floor(amount/60); //minutes
		amount = amount % 60;

		secs = Math.floor(amount); //seconds

		code = ""; 
		if(days != 0)
			code = code + days + " day" + ((days!=1)?"s":"")+", ";
		if(days != 0 || hours != 0)
			code = code + hours +" hour"+((hours!=1)?"s":"")+", ";
		if(days != 0 || hours != 0 || mins != 0)
			code = code + mins +" minute"+((mins!=1)?"s":"")+", ";
		code = code + secs + " seconds";
	 
		AWT_GetObj(id).innerHTML = code;
	}
}

function AWT_GetObj(clientid)
{
	return document.getElementById(clientid);
}

function AWT_SetInterval(id, interval)
{
	AWT_GetObj(id + '_Interval').value = interval;
}

function AWT_GetInterval(id)
{
	return parseInt(AWT_GetObj(id + '_Interval').value);
}

function AWT_SetTargetDate(id, year, month, day, hour, minute, second)
{
	AWT_GetObj(id + '_TargetDate').value = year + ',' + parseInt(month+1) + ',' + day + ',' + hour + ',' + minute + ',' + second;
}

function AWT_ParseDate(str)
{
	var dArr = str.split(",");
	return new Date(dArr[0],dArr[1]-1,dArr[2],dArr[3],dArr[4],dArr[5]);
}

function AWT_GetTargetDate(id)
{
	return AWT_ParseDate(AWT_GetObj(id + '_TargetDate').value);
}

function AWT_GetServerDate()
{
	return AWT_ParseDate(eval('AWT_ServerTime'));
}

function AWT_IsEnabled(id)
{
	var enabled = AWT_GetObj(id + '_Enabled').value;
	if (enabled.toLowerCase() == "true")
		return true;
	else
		return false;
}

function AWT_EnableTimer(id)
{
	AWT_GetObj(id + '_Enabled').value = 'true';
}

function AWT_DisableTimer(id)
{
	AWT_GetObj(id + '_Enabled').value = 'false';
}

