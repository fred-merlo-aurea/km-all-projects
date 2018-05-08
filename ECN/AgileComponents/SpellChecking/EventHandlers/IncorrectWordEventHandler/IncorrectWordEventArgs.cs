using System;

namespace ActiveUp.WebControls
{
	#region IncorrectWordEventArgs

	/// <summary>
	/// Represents a <see cref="IncorrectWordEventArgs"/> object.
	/// </summary>
	public class IncorrectWordEventArgs : EventArgs
	{
		#region Members
		private IncorrectWordProblemEnum problem;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the problem.
		/// </summary>
		/// <value>The problem.</value>
		public IncorrectWordProblemEnum Problem
		{
			get{return this.problem;}
			set{this.problem = value;}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="IncorrectWordEventArgs"/> class.
		/// </summary>
		public IncorrectWordEventArgs()
		{
			this.problem = IncorrectWordProblemEnum.WordIsNotInDict;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="IncorrectWordEventArgs"/> class.
		/// </summary>
		/// <param name="problemEnum">The problem.</param>
		public IncorrectWordEventArgs(IncorrectWordProblemEnum problemEnum)
		{
			this.problem = problemEnum;
		}


		#endregion
	}
	#endregion
}

