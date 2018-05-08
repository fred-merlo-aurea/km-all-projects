// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using ActiveUp.WebControls.Common;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;

namespace ActiveUp.WebControls
{
	#region class DateCollectionItem

	/// <summary>
	/// Represents an item used with <see cref="DateCollection"/>.
	/// </summary>
	[
		//ControlBuilderAttribute(typeof(DateCollectionItemControlBuilder)),
		TypeConverter(typeof(ExpandableObjectConverter))
	]
    public class DateCollectionItem : IStateManager
	{
		private StateManagerImpl stateManagerImpl = new StateManagerImpl();

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DateCollectionItem() 
		{
			Date = DateTime.Parse(DateTime.Now.ToShortDateString());
		}

		/// <summary>
		/// Create a DateCollectionItem specifying the Date (must be a DateTime object).
		/// </summary>
		/// <param name="date"></param>
		public DateCollectionItem(DateTime date) 
		{
			Date = date;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		[
			Bindable(true),
		]
		public DateTime Date
		{
			get
			{
				object val = ViewState["_date"];
				if (val != null)
					return (DateTime)val;
				else
					return DateTime.MinValue;
			}

			set
			{
				ViewState["_date"] = value;
			}
		}

		/// <summary>
		/// Gets the view state of the control.
		/// </summary>
		/// <value>The view state of the control.</value>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState 
		{
			get 
			{
				return stateManagerImpl.ViewState;
			}
		}

		#endregion

		#region Methods

		internal void SetDirty() 
		{
			stateManagerImpl.SetDirty();
		}

		#endregion

		#region ViewState

		bool IStateManager.IsTrackingViewState 
		{
			get 
			{
				return stateManagerImpl.IsTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState)
		{
			stateManagerImpl.LoadViewState(savedState);
		}

		object IStateManager.SaveViewState()
		{
			return stateManagerImpl.SaveViewState();
		}

		void IStateManager.TrackViewState()
		{
			stateManagerImpl.TrackViewState();
		}

		#endregion

	}

	#endregion
}
