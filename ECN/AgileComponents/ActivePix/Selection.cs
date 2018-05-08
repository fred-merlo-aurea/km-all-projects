using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Selection"/> object.
	/// </summary>
	[Serializable]
	public class Selection
	{
		private int _x1, _y1, _x2, _y2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Selection"/> class.
		/// </summary>
		public Selection()
		{
			_x1 = _y1 = _x2 = _y2 = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Selection"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		public Selection(string values)
		{
			try
			{
				if (values != null && values != string.Empty)
				{
					string[] selection = values.Split(',');

					_x1 = Convert.ToInt32(selection[0]);
					_y1 = Convert.ToInt32(selection[1]);

					if (selection.Length > 2)
					{
						_x2 = Convert.ToInt32(selection[2]);
						_y2 = Convert.ToInt32(selection[3]);
					}
				}
				else
					_x1 = _y1 = _x2 = _y2 = 0;
			}
			catch
			{
				//throw new Exception("Input string was not in a correct format.");
				_x1 = _y1 = _x2 = _y2 = 0;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Selection"/> class.
		/// </summary>
		/// <param name="x1">The top left x.</param>
		/// <param name="x2">The bottom right x.</param>
		/// <param name="y1">The top left y.</param>
		/// <param name="y2">The bottom right y.</param>
		public Selection(int x1, int x2, int y1, int y2)
		{
			_x1 = x1;
			_y1 = y1;
			_x2 = x2;
			_y2 = y2;
		}

		/// <summary>
		/// Gets or sets the top left x.
		/// </summary>
		/// <value>The top left x.</value>
		public int X1
		{
			get
			{
				return _x1;
			}
			set
			{
				_x1 = value;
			}
		}

		/// <summary>
		/// Gets or sets the top left y.
		/// </summary>
		/// <value>The top left y.</value>
		public int Y1
		{
			get
			{
				return _y1;
			}
			set
			{
				_y1 = value;
			}
		}

		/// <summary>
		/// Gets or sets the bottom right x.
		/// </summary>
		/// <value>The bottom right x.</value>
		public int X2
		{
			get
			{
				return _x2;
			}
			set
			{
				_x2 = value;
			}
		}

		/// <summary>
		/// Gets or sets the bottom right y.
		/// </summary>
		/// <value>The bottom right y.</value>
		public int Y2
		{
			get
			{
				return _y2;
			}
			set
			{
				_y2 = value;
			}
		}

		/// <summary>
		/// Valids the selection.
		/// </summary>
		/// <returns></returns>
		public Selection Valid()
		{
			if (_x2 < _x1 || _y2 < _y1)
			{
				Selection adjusted = new Selection();

				if (_x2 < _x1)
				{
					adjusted.X1 = _x2;
					adjusted.X2 = _x1;
				}
				else
				{
					adjusted.X1 = _x1;
					adjusted.X2 = _x2;
				}

				if (_y2 < _y1)
				{
					adjusted.Y1 = _y2;
					adjusted.Y2 = _y1;
				}
				else
				{
					adjusted.Y1 = _y1;
					adjusted.Y2 = _y2;
				}

				return adjusted;
			}
			else
				return this;
		}

		/// <summary>
		/// Transforms to comma separated.
		/// </summary>
		/// <returns></returns>
		public string ToCommaSeparated()
		{
			return _x1 + "," + _y1 + "," + _x2 + "," + _y2;
		}

	}
}
