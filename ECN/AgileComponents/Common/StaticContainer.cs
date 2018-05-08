using System;

namespace ActiveUp.WebControls.Common
{
	/// <summary>
	/// Represents a <see cref="StaticContainer"/> object.
	/// </summary>
	internal class StaticContainer
	{
		internal static int UsageCount
		{
			get
			{
				return 10;
			}
		}

		internal static string TrialMessage
		{
			get
			{
				return "<table bgcolor=red><tr><td><font face=verdana color=white><b>Reload the page to continue your evaluation.</b></font></td></tr></table>";
			}
		}

		/*internal static string VersionString
		{
			get
			{
				//return string.Format("{0}_{1}_{2}_0", VersionObject.Major.ToString(),
				//	VersionObject.Minor.ToString(), VersionObject.Build.ToString(),
				//	VersionObject.Revision.ToString());
				return "3_2_1815_0";
			}
		}*/

		internal static System.Version VersionObject
		{
			get
			{
				try
				{
					System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
					System.Version v = asm.GetName().Version;
					return v;
				}
				catch
				{
					return new System.Version(3,0,0,0);
				} 
			}
		}
	}
}
