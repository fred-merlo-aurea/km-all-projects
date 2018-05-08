using System;

namespace ecn.common.classes.utilities
{
	/// <summary>
	/// Thrown when a queryString has expired and is therefore no longer valid.
	/// </summary>
	public class ExpiredQueryStringException : System.Exception
	{
		public ExpiredQueryStringException() : base() {}
	}
}
