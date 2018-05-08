using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="TransitionProperties"/> object.
	/// </summary>
	public class TransitionProperties
	{
		private Direction _direction;
		private Motion _motion;
		private Orientation _orientation;
		private IrisStyle _irisStyle;
		private SlideStyle _slideStyle;
		private StretchStyle _stretchStyle;
		private SmoothStyle _smoothStyle;
		private WipeStyle _wipeStyle;
		private int _bands, _squaresX, _squaresY, _maxSquare, _gridSizeX, _gridSizeY, _spokes;
		private float _overlap, _gradientSize;

		/// <summary>
		/// Initializes a new instance of the <see cref="TransitionProperties"/> class.
		/// </summary>
		public TransitionProperties()
		{
			_bands = -1;
			_squaresX = -1;
			_squaresY = -1;
			_maxSquare = -1;
			_gridSizeX = -1;
			_gridSizeY = -1;
			_spokes = -1;
			_overlap = -1;
			_gradientSize = -1;
		}

		/// <summary>
		/// Gets or sets the smooth style.
		/// </summary>
		/// <value>The smooth style.</value>
		public SmoothStyle SmoothStyle
		{
			get
			{
				return _smoothStyle;
			}
			set
			{
				_smoothStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the wipe style.
		/// </summary>
		/// <value>The wipe style.</value>
		public WipeStyle WipeStyle
		{
			get
			{
				return _wipeStyle;
			}
			set
			{
				_wipeStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public Direction Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				_direction = value;
			}
		}

		/// <summary>
		/// Gets or sets the motion.
		/// </summary>
		/// <value>The motion.</value>
		public Motion Motion
		{
			get
			{
				return _motion;
			}
			set
			{
				_motion = value;
			}
		}

		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public Orientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				_orientation = value;
			}
		}

		/// <summary>
		/// Gets or sets the iris style.
		/// </summary>
		/// <value>The iris style.</value>
		public IrisStyle IrisStyle
		{
			get
			{
				return _irisStyle;
			}
			set
			{
				_irisStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the slide style.
		/// </summary>
		/// <value>The slide style.</value>
		public SlideStyle SlideStyle
		{
			get
			{
				return _slideStyle;
			}
			set
			{
				_slideStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the stretch style.
		/// </summary>
		/// <value>The stretch style.</value>
		public StretchStyle StretchStyle
		{
			get
			{
				return _stretchStyle;
			}
			set
			{
				_stretchStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the spokes.
		/// </summary>
		/// <value>The spokes.</value>
		public int Spokes
		{
			get
			{
				return _spokes;
			}
			set
			{
				_spokes = value;
			}
		}

		/// <summary>
		/// Gets or sets the grid size Y.
		/// </summary>
		/// <value>The grid size Y.</value>
		public int GridSizeY
		{
			get
			{
				return _gridSizeY;
			}
			set
			{
				_gridSizeY = value;
			}
		}

		/// <summary>
		/// Gets or sets the grid size X.
		/// </summary>
		/// <value>The grid size X.</value>
		public int GridSizeX
		{
			get
			{
				return _gridSizeX;
			}
			set
			{
				_gridSizeX = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum square.
		/// </summary>
		/// <value>The maximum square.</value>
		public int MaxSquare
		{
			get
			{
				return _maxSquare;
			}
			set
			{
				_maxSquare = value;
			}
		}

		/// <summary>
		/// Gets or sets the squares Y.
		/// </summary>
		/// <value>The squares Y.</value>
		public int SquaresY
		{
			get
			{
				return _squaresY;
			}
			set
			{
				_squaresY = value;
			}
		}

		/// <summary>
		/// Gets or sets the squares X.
		/// </summary>
		/// <value>The squares X.</value>
		public int SquaresX
		{
			get
			{
				return _squaresX;
			}
			set
			{
				_squaresX = value;
			}
		}

		/// <summary>
		/// Gets or sets the bands.
		/// </summary>
		/// <value>The bands.</value>
		public int Bands
		{
			get
			{
				return _bands;
			}
			set
			{
				_bands = value;
			}
		}

		/*public int Percent
		{
			get
			{
				return _percent;
			}
			set
			{
				_percent = value;
			}
		}*/

		/// <summary>
		/// Gets or sets the overlap.
		/// </summary>
		/// <value>The overlap.</value>
		public float Overlap
		{
			get
			{
				return _overlap;
			}
			set
			{
				_overlap = value;
			}
		}

		/// <summary>
		/// Gets or sets the gradient size.
		/// </summary>
		/// <value>The gradient size.</value>
		public float GradientSize
		{
			get
			{
				return _gradientSize;
			}
			set
			{
				_gradientSize = value;
			}
		}
	}
}
