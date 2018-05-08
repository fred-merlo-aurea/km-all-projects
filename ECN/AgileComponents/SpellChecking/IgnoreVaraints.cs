using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ActiveUp.WebControls
{
	#region IgnoreVariants
	/// <summary>
	/// this singleton used to save ignore variants
	/// </summary>
	internal class IgnoreVariants
	{
		#region Static members
		/// <summary>
		/// The static IgnoreVariants instance
		/// </summary>
		private static IgnoreVariants instance = new IgnoreVariants();
		#endregion
    		
		#region Members
		private bool ignorMixed = false;
		private bool ignoreNumbers = false ;
		private bool ignoreLowercase = false ;
		private bool ignoreUppercase = false ;
		private bool ignoreAll;
		#endregion
    		
		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether [ignore all].
		/// </summary>
		/// <value><c>true</c> if [ignore all]; otherwise, <c>false</c>.</value>
		public bool IgnoreAll
		{
			get{ return ignoreAll; }
			set{ ignoreAll = value; }
		}
		/// <summary>
		/// this bool true when want to ignore words with uppercase
		/// </summary>
		public bool IgnoreUppercase
		{
			get{ return ignoreUppercase; }
			set{ ignoreUppercase = value; }
		}
		/// <summary>
		/// this bool true when want to ignore words with lowercase
		/// </summary>
		public bool IgnoreLowercase
		{
			get{ return ignoreLowercase; }
			set{ ignoreLowercase = value; }
		}
		public bool IgnoreNumbers
		{
			get{ return ignoreNumbers; }
			set{ ignoreNumbers = value; }
		}
		public bool IgnorMixed
		{
			get{ return ignorMixed; }
			set{ ignorMixed = value; }
		}
		#endregion
    		
		#region Static properties
		/// <summary>
		/// Gets the static IgnoreVariants instance. Used to be shared between IgnoreVariants' users.
		/// </summary>
		/// <value>The singleton IgnoreVariants instance.</value>
		public static IgnoreVariants Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion
     	
		#region Initialize/Finalize methods
		/// <summary>
		/// Initializes a new instance of the <see cref="IgnoreVariants"/> class.
		/// Constructor is private to allow only creation of IgnoreVariants instances only once.
		/// </summary>
		private IgnoreVariants(){}
		#endregion
	}
	#endregion
}