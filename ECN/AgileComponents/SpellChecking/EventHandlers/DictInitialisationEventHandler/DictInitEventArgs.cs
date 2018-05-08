using System;

namespace ActiveUp.WebControls
{
	#region DictInitEventArgs

	/// <summary>
	/// Represents a <see cref="DictInitEventArgs"/> object.
	/// </summary>
	public class DictInitEventArgs : EventArgs
	{
		#region Members
		private DictInitLangugeEnum languge;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the problem.
		/// </summary>
		/// <value>The problem.</value>
		public DictInitLangugeEnum Languge
		{
			get{return this.languge;}
			set{this.languge = value;}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="IncorrectWordEventArgs"/> class.
		/// </summary>
		public DictInitEventArgs()
		{
			this.languge = DictInitLangugeEnum.LangEnglish;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="IncorrectWordEventArgs"/> class.
		/// </summary>
		/// <param name="langEnum">The langage.</param>
		public DictInitEventArgs( DictInitLangugeEnum langEnum )
		{
			this.languge = langEnum;
		}


		#endregion
	}
	#endregion
}

