using System;

namespace ecn.communicator.classes
{	
	public interface IZipCodeLocator
	{
		int GetIDOfNearestObject(string zipcode);
	}
}
