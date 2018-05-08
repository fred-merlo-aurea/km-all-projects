// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a trial specific error.
	/// </summary>
	public class TrialException : Exception
	{
		/// <summary>
		/// The message exception.
		/// </summary>
		string _message;

		/// <summary>
		/// Constructor, sets message to the specified value.
		/// </summary>
		/// <param name="message">The exception's message.</param>
		public TrialException(string message)
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			this._message = message;
		}

		/// <summary>
		/// The exception's message.
		/// </summary>
		public override string Message
		{
			get
			{
				return this._message;
			}
		}
	}
}
