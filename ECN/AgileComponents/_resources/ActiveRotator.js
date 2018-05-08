// Common
		
function ACR_testIfScriptPresent()
{
}

function AU_debugTrace(str)
{
	ACR_getObject("AU_Debug").value = str + "\n" + ACR_getObject("AU_Debug").value;
}
	
function ACR_getObject(id)
{
	return document.getElementById(id); 
}

// Rotator Logic

function ACR_moveObject(obj,x,y)
{    
	try
	{
	
		obj.style.left = parseFloat(x) + 'px';
		obj.style.top = parseFloat(y) + 'px';
		
	}
	catch (ex)
	{
	}
}

function ACR_getHeight(obj)
{
	return obj.clientHeight;
}

function ACR_getWidth(obj)
{
	return obj.clientWidth;
}

function ACR_startScroll(id, objid, x, y, moveTime, fps, type)
{
	ACR_doScroll(id, ACR_initScroll(id, objid, x, y, moveTime, fps, type));
}

function ACR_initScroll(id, objid, x, y, moveTime, fps, type)
{
	// Calculate the frame time
	var frameTime = Math.round(1000 / fps);
	
	// Calculate the number of frames
	var frames = Math.round(moveTime / frameTime);
	
	// Get the actual position of the slide(s)
	var origX = document.getElementById(objid).offsetLeft;
	var origY = document.getElementById(objid).offsetTop;
	
	// Calculate the distance between the origin and destination
	var diffX = x - origX;			
	var diffY = y - origY;	
	
	// Smooth logic pre-calculations
	var degStep = 90 / frames;
	var rad = (Math.PI/180);
	
	// Set the first frame count to 0
	eval(id + '_positions')[2] = 0;
	
	// End smooth
	if (type=='SmoothEnd')
	{
		for (i = 1; i <= frames; i++)
		{
			preCalc = (Math.sin((i * degStep) * rad));
			eval(id + '_scrollPosX')[i-1] = origX + Math.round(diffX * preCalc);
			eval(id + '_scrollPosY')[i-1] = origY + Math.round(diffY * preCalc);
		}
	}
	// Start smooth
	else if (type=='SmoothStart')
	{
		for (i = 1; i <= frames; i++)
		{
			preCalc = (Math.cos((i * degStep) * rad));
			eval(id + '_scrollPosX')[i-1] = origX + Math.round(diffX - (diffX * preCalc));
			eval(id + '_scrollPosY')[i-1] = origY + Math.round(diffY - (diffY * preCalc));
		}
	}
	// Start-End smooth
	else if (type=='SmoothStartAndEnd')
	{
		degStep = 90 / frames;
		halfX = Math.round(diffX / 2);
		halfY = Math.round(diffY / 2);

		for (i = 1; i <= frames; i++)
		{
			preCalc = (Math.cos((i * degStep * 2) * rad));
			eval(id + '_scrollPosX')[i-1] = origX + Math.round(halfX - (halfX * preCalc));
			eval(id + '_scrollPosY')[i-1] = origY + Math.round(halfY - (halfY * preCalc));
			if ((diffX >= 0 && eval(id + '_scrollPosX')[i-1] > x) || (diffX < 0 && eval(id + '_scrollPosX')[i-1] < x))
				eval(id + '_scrollPosX')[i-1] = x;
			if ((diffY >= 0 && eval(id + '_scrollPosY')[i-1] > y) || (diffY < 0 && eval(id + '_scrollPosY')[i-1] < y))
				eval(id + '_scrollPosY')[i-1] = y;
		}

	}
	// Normal scroll
	else
	{
		var stepLengthX = diffX / frames;
		var stepLengthY = diffY / frames;

		for (i = 1; i <= frames; i++)
		{
			eval(id + '_scrollPosX')[i-1] = origX + Math.round(i * stepLengthX);
			eval(id + '_scrollPosY')[i-1] = origY + Math.round(i * stepLengthY);
		}
	}
	
	// Ensure that the last frame is the destination position
	eval(id + '_scrollPosX')[frames-1] = x;
	eval(id + '_scrollPosY')[frames-1] = y;
		
	return frameTime;
}

function ACR_doScroll(id, frameTime)
{	
	// Create the date stamp
	d = new Date();
	var frame = eval(id + '_positions')[2];
	
	// Move the slide to the frame position
	ACR_moveObject(document.getElementById(id + '_Slide' + eval(id + '_positions')[0]), eval(id + '_scrollPosX')[frame],  eval(id + '_scrollPosY')[frame]);
	
	// Advance to next frame
	eval(id + '_positions')[2] = frame + 1;
	
	// Prepare the next frame
	if (frame < eval(id + '_scrollPosX').length)
	{
		// Create the date stamp
		mill=new Date();
		
		// Prepare the next frame based on the calculation time spent and frame time
		setTimeout("ACR_doScroll('" + id + "', " + frameTime + ")", (frameTime - (mill-d)))
	}
}

// Rotator
function ACR_startRotator(id)
{
	eval(id + '_positions')[3] = 1;
}

function ACR_stopRotator(id)
{
	eval(id + '_positions')[3] = 0;
}

function ACR_prepareSlides(id)
{	
	// Get the number of slides
	len = eval(id + '_slidesLen');
	
	// Get the actual slide position
	pos = eval(id + '_positions')[0];
	
	// Calculate the next slide position
	nextPos = parseInt(pos);
	nextPos++;
	if (nextPos >= len)
		nextPos = 0;
		
	// Get the container
	container = ACR_getObject(id);
	
	// Get the container height and width
	containerWidth = ACR_getWidth(container);
	containerHeight = ACR_getHeight(container);
	
	// Get the container position
	containerX = container.offsetLeft;
	containerY = container.offsetTop;

	for(i=0;i<len;i++)
	{
		// Get the slide
		slide = document.getElementById(id + '_Slide' + i);
		slide.position = 'absolute';
		slide.style.zIndex=250;
		
		// It's the next slide, prepare it.
		if (i == nextPos)
		{
			// Get the paremeters of the transition
			aParams = eval(id + '_aParams');
			direction = 'up';
			for(j=0;j<aParams.length;j++)
				if (aParams[j].indexOf('direction') > -1)
					direction = aParams[j].split('=')[1];

			// Show the slide
			slide.style.visibility = 'visible';
			slide.style.display = 'block';

			// Position the slide
			switch (direction)
			{
				case 'Left': ACR_moveObject(slide, containerWidth, 0); break;
				case 'Right': ACR_moveObject(slide, 0 - ACR_getWidth(slide), 0); break;
				case 'Down': ACR_moveObject(slide, 0, 0 - ACR_getHeight(slide)); break;
				case 'DownLeft': ACR_moveObject(slide, containerWidth, 0 - ACR_getHeight(slide)); break;
				case 'DownRight': ACR_moveObject(slide, - ACR_getWidth(slide), 0 - ACR_getHeight(slide)); break;
				case 'UpLeft': ACR_moveObject(slide, containerWidth, containerHeight); break;
				case 'UpRight': ACR_moveObject(slide, 0 - ACR_getWidth(slide), containerHeight); break;
				default: ACR_moveObject(slide, 0, containerHeight); break;
			}
				
			slide.style.zIndex=260;
		}
		// It's another slide, hide it
		else if (i != pos)
		{
			slide.style.visibility = 'hidden';
			slide.style.display = 'none';
		}
		// It's the active slide, ensure visibility
		else if (i == pos)
		{
			slide.style.visibility = 'visible';
			slide.style.display = 'block';
		}
	}
}

function ACR_moveSlides(id, index)
{
	// Get the slide parameters
	aParams = eval(id + '_aParams');
	smooth = 'SmoothStartAndEnd';
	for(j=0;j<aParams.length;j++)
		if (aParams[j].indexOf('smoothstyle') > -1)
			smooth = aParams[j].split('=')[1];

	// Start the scroll
	ACR_doScroll(id, ACR_initScroll(id, id + '_Slide' + index, 0, 0, eval(id + '_speed'), eval(id + '_fps'), smooth));
}

function ACR_initRotator(id, start)
{
	// It's the transition type is not smoothscroll, write the first slide
	if (eval(id + '_transition') != 'smoothscroll')
		ACR_getObject(id).innerHTML = ACR_getObject(id + '_Slide0').innerHTML;
	else
	{
		document.getElementById(id + '_Slide' + eval(id + '_positions')[0]).style.visibility = 'visible';
		document.getElementById(id + '_Slide' + eval(id + '_positions')[0]).style.display = 'block';
	}
	
	// Start the rotation if specified
	if (start)
		setTimeout('ACR_rotate(\'' + id + '\');', eval(id + '_pause'));
}

function ACR_rotate(id)
{
	if (eval(id + '_positions')[3] == 0)
		setTimeout('ACR_waitState(\'' + id + '\');', 10);
	else
	{	
	// Get the rotator
	var rotator = ACR_getObject(id);
	
	// Get the transition time
	var msSpeed = eval(id + '_speed')
	var speed = Math.round(msSpeed / 1000);
	
	// Get the transition type
	var transition = eval(id + '_transition');
	
	// Get the slide position
	var pos = parseInt(eval(id + '_positions')[0]);
	
	// Calculate the next slide position
	var nextPos = pos;
	nextPos++;
	if (nextPos >= eval(id + '_slidesLen'))
		nextPos = 0;
	
	// If SmoothScroll, prepare differently
	if (transition == "smoothscroll")
	{
		ACR_prepareSlides(id)
		eval(id + '_positions')[0] = nextPos;
		ACR_moveSlides(id, nextPos);
	}
	// Standard MSIE transitions
	else
	{
		// Advance the slide position
		eval(id + '_positions')[0] = nextPos;
		
		// If MSIE 5.5 filters are allowed, proceed
		if (ACR_filtersAllowed)
		{
			// Get the transition
			var transition = "progid:DXImageTransform.Microsoft." + transition + "(" + eval(id + '_params') + ")";
			
			// Assign the transition
			rotator.style.filter=transition;
			
			// Ensure one more time if the transition can occurs and apply it
			if (rotator.filters && rotator.filters[0])
			{
				rotator.filters[0].Duration=speed;
				rotator.filters[0].Apply();
				rotator.filters[0].Play();
			}
		}
		
		// Start the transition
		rotator.innerHTML = ACR_getObject(id + '_Slide' + nextPos).innerHTML;
	}
	
	// Prepare the next transition
	//if (eval(id + '_positions')[3] == 0)
	//	setTimeout('ACR_waitState(\'' + id + '\');', 10);
	//else
		setTimeout('ACR_rotate(\'' + id + '\');', parseInt(eval(id + '_pause')) + msSpeed);
		}
}

function ACR_waitState(id, work)
{
	if (eval(id + '_positions')[3] == 1)
	{
		//var msSpeed = eval(id + '_speed');
		//setTimeout('ACR_rotate(\'' + id + '\');', parseInt(eval(id + '_pause')) + msSpeed);
		ACR_rotate(id);
	}
	else
		setTimeout('ACR_waitState(\'' + id + '\');', 300);
}

function ACR_rotateOver(id)
{
}

function ACR_rotateOut(id)
{
}


// Ticker Logic

function ACR_IntToHex(n)
{
	var result = n.toString(16);
	
	if (result.length==1)
		result = "0" + result;
	
	return result;
}

function ACR_HexToInt(hex)
{
	return parseInt(hex, 16);
}

function ACR_initFadeColors(id, from, to)
{
	var colors = eval(id + '_fadeColors');
	len = colors.length; 

	if (from.charAt(0)=='#')
		from = from.substring(1);
	if (to.charAt(0)=='#')
		to = to.substring(1);

	var r = ACR_HexToInt(from.substring(0,2));
	var g = ACR_HexToInt(from.substring(2,4));
	var b = ACR_HexToInt(from.substring(4,6));
	var r2 = ACR_HexToInt(to.substring(0,2));
	var g2 = ACR_HexToInt(to.substring(2,4));
	var b2 = ACR_HexToInt(to.substring(4,6));

	var rStep = Math.round((r2 - r) / len);
	var gStep = Math.round((g2 - g) / len);
	var bStep = Math.round((b2 - b) / len);

	for (i = 0; i < len-1; i++)
	{
		colors[i] = "#" + ACR_IntToHex(r) + ACR_IntToHex(g) + ACR_IntToHex(b);
		r += rStep;
		g += gStep;
		b += bStep;
	}
	colors[len-1] = to;
}

function ACR_Right(str, count)
{
	if (count <= 0)
		return '';
	else if (count > String(str).length)
		return str;
	else
	{
		var len = String(str).length;
		return String(str).substring(len, len - count);
	}
}

function ACR_ApplyFade(id, str, padding)
{
	var newStr = '';
	var index = 0;
	var pad = 0;

	if (padding)
		pad = eval(id + '_fadeChars') - str.length;
	for(index=0;index<str.length;index++)
		newStr += "<font color=\'" + eval(id + '_fadeColors')[index+pad] + "\'>" + str.charAt(index) + "</font>";
	return newStr;
}

function ACR_doTicker(id)
{
	var tickPos = parseInt(eval(id + '_positions')[1]);
	var slidePos = parseInt(eval(id + '_positions')[0]);
	var tickFontFadeChars = parseInt(eval(id + '_fadeChars'));
	var message = ACR_getObject(id + '_Slide' + slidePos).innerHTML;
	var tickMessageLength = message.length;
	// If we are still scrolling, 
	
	if (tickPos <= tickMessageLength + tickFontFadeChars)
	{
		var scrollStr = message.substring(0, tickPos);
		var startStr = scrollStr.substring(0, tickPos - tickFontFadeChars);
		var endStr = '';
		
		if (tickFontFadeChars != 0)
		{
			endStr = ACR_Right(scrollStr, (tickPos <= tickMessageLength ? tickFontFadeChars : tickFontFadeChars - (tickPos - tickMessageLength)));
			endStr = ACR_ApplyFade(id, endStr, (tickPos <= tickFontFadeChars ? true : false));
		}
		
		str = startStr + endStr;
		
		ACR_getObject(id).innerHTML = str;
		eval(id + '_positions')[1] = tickPos + 1;
		setTimeout('ACR_doTicker(\'' + id + '\');', eval(id + '_speed'));
	}
	// Proceed to the next message.
	else
	{
		slidePos++;
		if (slidePos >= eval(id + '_slidesLen'))
			slidePos = 0;

		eval(id + '_positions')[0] = slidePos;
		eval(id + '_positions')[1] = 0;
		
		setTimeout('ACR_doTicker(\'' + id + '\');', eval(id + '_pause'));
	}
}

function ACR_initTicker(id, start)
{
	// Create the fade colors
	ACR_initFadeColors(id, eval(id + '_textColor'), eval(id + '_fadeColor'));
	
	// Start the ticker
	if (start)
		ACR_doTicker(id);
}