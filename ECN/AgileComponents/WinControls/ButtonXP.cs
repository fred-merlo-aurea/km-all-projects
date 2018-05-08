using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;


namespace ActiveUp.WebControls.WinControls
{
	/// <summary>
	/// Margin structure.
	/// </summary>
	public struct BT
	{
		/// <summary>
		/// Left margin.
		/// </summary>
		public const int LeftMargin = 7;
		/// <summary>
		/// Text margin.
		/// </summary>
		public const int TextMargin = 7;
	}

	/// <summary>
	/// Represents a <see cref="ButtonXP"/> object.
	/// </summary>
	public class ButtonXP : System.Windows.Forms.Control
	{

		/// <summary>
		/// Gets the name of the current theme.
		/// </summary>
		/// <param name="pszThemeFileName">Name of the PSZ theme file.</param>
		/// <param name="dwMaxNameChars">The dw maximum name chars.</param>
		/// <param name="pszColorBuff">The PSZ color buffer.</param>
		/// <param name="cchMaxColorChars">The CCH max color chars.</param>
		/// <param name="pszSizeBuff">The PSZ size buffer.</param>
		/// <param name="cchMaxSizeChars">The CCH maximum size chars.</param>
		/// <returns></returns>
		[DllImport("UxTheme.dll")]
		public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, 
			StringBuilder pszColorBuff, int cchMaxColorChars, 
			StringBuilder pszSizeBuff, int cchMaxSizeChars);



		/// <summary>
		/// Determines whether is the application is themed.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the application is themed; otherwise, <c>false</c>.
		/// </returns>
		[DllImport("UxTheme.dll")]
		public static extern bool IsAppThemed();

		private System.ComponentModel.Container components = null;

		/// <summary>
		/// States enumeration.
		/// </summary>
		public enum States
		{
			/// <summary>
			/// Normal state.
			/// </summary>
			Normal,
			/// <summary>
			/// Mouse over state.
			/// </summary>
			MouseOver,
			/// <summary>
			/// Pushed state.
			/// </summary>
			Pushed
		}
		private States state = States.Normal;

		private Size _size;

		/// <summary>
		/// Schemes enumeration.
		/// </summary>
		public enum Schemes
		{
			/// <summary>
			/// Blue scheme.
			/// </summary>
			Blue = 0,
			/// <summary>
			/// Oliver green scheme.
			/// </summary>
			OliveGreen,
			/// <summary>
			/// Silver scheme.
			/// </summary>
			Silver
		}
		private Schemes scheme = Schemes.Silver;

		private Image image;
		private Image _image;
		private Rectangle bounds;
		private bool selected;
		private bool defaultScheme;

		private Rectangle[] rects0; 
		private Rectangle[] rects1; 

		private static Pen						
			pen0, pen1, pen2,
			bluePen01, bluePen02, bluePen03, bluePen04, bluePen05, bluePen06, 
			bluePen07, bluePen08, bluePen09, bluePen10, bluePen11, bluePen12, bluePen13,
			olivePen01, olivePen02, olivePen03, olivePen04, olivePen05, olivePen06, 
			olivePen07, olivePen08, olivePen09, olivePen10, olivePen11, olivePen12,
			olivePen13;

		private static LinearGradientBrush		
			blueBrush01, blueBrush02, blueBrush03,
			oliveBrush01, oliveBrush02, oliveBrush03,
			silverBrush02;
		private static SolidBrush				
			brush1, silverBrush01, silverBrush03;



										

		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonXP"/> class.
		/// </summary>
		public ButtonXP()
		{	
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.StandardDoubleClick, false);
			this.SetStyle(ControlStyles.Selectable, true);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"/> and optionally releases the managed
		/// resources.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				DisposePensBrushes();
				if(components != null)
				{	
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Gets or sets a value indicating whether is the default scheme.
		/// </summary>
		/// <value><c>true</c> if is the default scheme; otherwise, <c>false</c>.</value>
		public bool DefaultScheme
		{
			get
			{
				return defaultScheme;
			}
			set
			{
				defaultScheme = value;
				if (defaultScheme)
				{
					try
					{
						StringBuilder sb1 = new StringBuilder(256);
						StringBuilder sb2 = new StringBuilder(256);
						StringBuilder sb3 = new StringBuilder(256);
						int i = GetCurrentThemeName(sb1, sb1.Capacity, sb2, sb2.Capacity, sb3, sb3.Capacity);

						string str = sb2.ToString();

						switch(str)
						{
							case @"HomeStead":
								scheme = Schemes.OliveGreen;
								break;
							case @"Metallic":
								scheme = Schemes.Silver;
								break;
							default:
								scheme = Schemes.Blue;
								break;
						}
					}
					catch (Exception)
					{
						return;
					}
					this.Invalidate();
				}
			}

		}

		/// <summary>
		/// Gets or sets the scheme.
		/// </summary>
		/// <value>The scheme.</value>
		public Schemes Scheme
		{
			get
			{
				return scheme;
			}
			set
			{
				scheme = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the image size button.
		/// </summary>
		/// <value>The image size button.</value>
		public Size SizeImgButton
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
				if (_size.Height != 0 && _size.Width != 0 && image != null)
					image = new Bitmap(image, _size);
				this.Invalidate();
			}
		}

		/*public System.Drawing.Image _Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
				this.Invalidate();
			}
		}*/


		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		/// <value></value>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			//if (this.Parent != null && !this.Parent.ContainsFocus) return;

	
			if (bounds.Contains(e.X, e.Y))
			{
				if (state == States.Normal)
				{
					state = States.MouseOver;
					this.Invalidate(bounds);
				}
			}
			else
			{
				if (state != States.Normal)
				{
					state = States.Normal;
					this.Invalidate(bounds);
				}
			}
			
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			if (state != States.Normal)
			{
				state = States.Normal;
				this.Invalidate(bounds);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{

			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

			if (bounds.Contains(e.X, e.Y))
			{
				state = States.Pushed;
				this.Focus();
			} 
			else
			{
				state = States.Normal;
			}
			this.Invalidate(bounds);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				state = States.Normal;
				this.Invalidate(bounds);
			}
		}


		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			var x0 = Width;
			var y0 = Height;

			e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;

			var y1 = 0;
			var x1 = 0;
			Point point;

			var y2 = (Height - Font.Height) / 2;
			if(image != null)
			{
				x1 = Text.Length == 0 ? (x0 - image.Width) / 2 : BT.LeftMargin;
				y1 = (Height - image.Height) / 2;
				point = new Point(BT.LeftMargin + image.Width + BT.TextMargin, y2);
			}
			else
			{
				var size = GetTextSize(e.Graphics, Text, Font, Size);
				var textWidth = size.Width;
				point = new Point((x0 - textWidth) / 2, y2);
			}

			if(!Enabled)
			{
				OnPaintWhenDisabled(e, x0, y0, x1, y1, point);
				return;
			}

			switch(scheme)
			{
				case Schemes.OliveGreen:
					CreatePenBrushesOliveGreenScheme(x0, y0);
					break;
				case Schemes.Silver:
					CreatePenBrushesSilverScheme(x0, y0);
					break;
				default:
					CreatePenBrushesDefaultScheme(x0, y0);
					break;
			}

			ApplySchemeDrawGraphics(e, x0, y0, x1, y1, point);

			DisposePensBrushes();
		}

		private void ApplySchemeDrawGraphics(PaintEventArgs e, int tempWidth, int tempHeight, int x, int y, Point point)
		{
			using(var brush0 = new LinearGradientBrush(
				new Rectangle(0, 0, tempWidth, tempHeight), 
				Color.FromArgb(64, 171, 168, 137), 
				Color.FromArgb(92, 255, 255, 255), 85.0f))
			{
				e.Graphics.FillRectangle(brush0, new Rectangle(0, 0, tempWidth, tempHeight));
			}

			switch(state)
			{
				case States.Normal:
					ApplySchemeNormalState(e, tempWidth, tempHeight);
					break;
				case States.MouseOver:
					ApplySchemeMouseOverState(e, tempWidth, tempHeight);
					break;
				case States.Pushed:
					ApplySchemePushedState(e, tempWidth, tempHeight);
					break;
			}

			if(image != null)
			{
				e.Graphics.DrawImage(image, x, y);
			}

			e.Graphics.DrawString(Text, Font, SystemBrushes.ControlText, point);

			e.Graphics.DrawLine(pen1, 1, 3, 3, 1);
			e.Graphics.DrawLine(pen1, tempWidth - 2, 3, tempWidth - 4, 1);
			e.Graphics.DrawLine(pen1, 1, tempHeight - 4, 3, tempHeight - 2);
			e.Graphics.DrawLine(pen1, tempWidth - 2, tempHeight - 4, tempWidth - 4, tempHeight - 2);

			e.Graphics.DrawLine(pen2, 1, 2, 2, 1);
			e.Graphics.DrawLine(pen2, 1, tempHeight - 3, 2, tempHeight - 2);
			e.Graphics.DrawLine(pen2, tempWidth - 2, 2, tempWidth - 3, 1);
			e.Graphics.DrawLine(pen2, tempWidth - 2, tempHeight - 3, tempWidth - 3, tempHeight - 2);

			e.Graphics.DrawLine(pen0, 3, 1, tempWidth - 4, 1);
			e.Graphics.DrawLine(pen0, 3, tempHeight - 2, tempWidth - 4, tempHeight - 2);
			e.Graphics.DrawLine(pen0, 1, 3, 1, tempHeight - 4);
			e.Graphics.DrawLine(pen0, tempWidth - 2, 3, tempWidth - 2, tempHeight - 4);

			e.Graphics.FillRectangles(brush1, rects1);
		}

		private void OnPaintWhenDisabled(PaintEventArgs e, int x0, int y0, int x, int y, Point point)
		{
			SolidBrush brush01;
			Pen pen01;
			Pen pen02;
			SolidBrush brush;
			switch(scheme)
			{
				case Schemes.OliveGreen:
					brush = new SolidBrush(Color.FromArgb(64, 202, 196, 184));
					brush01 = new SolidBrush(Color.FromArgb(246, 242, 233));
					pen01 = new Pen(Color.FromArgb(202, 196, 184));
					pen02 = new Pen(Color.FromArgb(170, 202, 196, 184));
					break;

				case Schemes.Silver:
					brush = new SolidBrush(Color.FromArgb(64, 196, 195, 191));
					brush01 = new SolidBrush(Color.FromArgb(241, 241, 237));
					pen01 = new Pen(Color.FromArgb(196, 195, 191));
					pen02 = new Pen(Color.FromArgb(170, 196, 195, 191));
					break;

				default:
					brush = new SolidBrush(Color.FromArgb(64, 201, 199, 186));
					brush01 = new SolidBrush(Color.FromArgb(245, 244, 234));
					pen01 = new Pen(Color.FromArgb(201, 199, 186));
					pen02 = new Pen(Color.FromArgb(170, 201, 199, 186));
					break;
			}

			e.Graphics.FillRectangle(brush01, 2, 2, x0 - 4, y0 - 4);
			e.Graphics.DrawLine(pen01, 3, 1, x0 - 4, 1);
			e.Graphics.DrawLine(pen01, 3, y0 - 2, x0 - 4, y0 - 2);
			e.Graphics.DrawLine(pen01, 1, 3, 1, y0 - 4);
			e.Graphics.DrawLine(pen01, x0 - 2, 3, x0 - 2, y0 - 4);

			e.Graphics.DrawLine(pen02, 1, 2, 2, 1);
			e.Graphics.DrawLine(pen02, 1, y0 - 3, 2, y0 - 2);
			e.Graphics.DrawLine(pen02, x0 - 2, 2, x0 - 3, 1);
			e.Graphics.DrawLine(pen02, x0 - 2, y0 - 3, x0 - 3, y0 - 2);
			e.Graphics.FillRectangles(brush, rects1);

			if(image != null)
			{
				ControlPaint.DrawImageDisabled(e.Graphics, image, x, y, BackColor);
			}

			e.Graphics.DrawString(Text, Font, SystemBrushes.ControlDark, point);

			brush.Dispose();
			brush01.Dispose();
			pen01.Dispose();
			pen02.Dispose();
		}

		private static void CreatePenBrushesOliveGreenScheme(int x, int y)
		{
			oliveBrush01 = new LinearGradientBrush(new Rectangle(2, 2, x - 5, y - 7)
				, Color.FromArgb(255, 255, 246), Color.FromArgb(246, 243, 224), 90.0f);

			olivePen01 = new Pen(Color.FromArgb(243, 238, 219));
			olivePen02 = new Pen(Color.FromArgb(236, 225, 201));
			olivePen03 = new Pen(Color.FromArgb(227, 209, 184));

			var brush05 = new LinearGradientBrush(new Rectangle(x - 3, 4, 1, y - 5)
				, Color.FromArgb(251, 247, 232), Color.FromArgb(64, 216, 181, 144), 90.0f);
			var brush06 = new LinearGradientBrush(new Rectangle(x - 2, 4, 1, y - 5)
				, Color.FromArgb(246, 241, 224), Color.FromArgb(64, 194, 156, 120), 90.0f);

			olivePen04 = new Pen(brush05);
			olivePen05 = new Pen(brush06);

			oliveBrush02 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(177, 203, 128), Color.FromArgb(144, 193, 84), 90.0f);

			olivePen06 = new Pen(Color.FromArgb(194, 209, 143));
			olivePen07 = new Pen(Color.FromArgb(177, 203, 128));
			olivePen08 = new Pen(Color.FromArgb(144, 193, 84));
			olivePen09 = new Pen(Color.FromArgb(168, 167, 102));

			oliveBrush03 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(237, 190, 150), Color.FromArgb(227, 145, 79), 90.0f);
			olivePen10 = new Pen(Color.FromArgb(252, 197, 149));
			olivePen11 = new Pen(Color.FromArgb(237, 190, 150));
			olivePen12 = new Pen(Color.FromArgb(227, 145, 79));
			olivePen13 = new Pen(Color.FromArgb(207, 114, 37));

			brush1 = new SolidBrush(Color.FromArgb(92, 109, 138, 77));
			pen0 = new Pen(Color.FromArgb(55, 98, 6));
			pen1 = new Pen(Color.FromArgb(109, 138, 77));
			pen2 = new Pen(Color.FromArgb(192, 109, 138, 77));

			brush05.Dispose();
			brush06.Dispose();
		}

		private static void CreatePenBrushesSilverScheme(int x, int y)
		{
			silverBrush01 = new SolidBrush(Color.White);
			silverBrush02 = new LinearGradientBrush(new Rectangle(3, 3, x - 6, y - 7)
				, Color.FromArgb(253, 253, 253), Color.FromArgb(201, 200, 220), 90.0f);
			silverBrush03 = new SolidBrush(Color.FromArgb(198, 197, 215));

			var relativeIntensities = new[] {0.0f, 0.008f, 1.0f};
			var relativePositions = new[] {0.0f, 0.32f, 1.0f};

			var blend = new Blend
			{
				Factors = relativeIntensities,
				Positions = relativePositions
			};
			silverBrush02.Blend = blend;

			blueBrush02 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(186, 211, 245), Color.FromArgb(137, 173, 228), 90.0f);

			bluePen06 = new Pen(Color.FromArgb(206, 231, 255));
			bluePen07 = new Pen(Color.FromArgb(188, 212, 246));
			bluePen08 = new Pen(Color.FromArgb(137, 173, 228));
			bluePen09 = new Pen(Color.FromArgb(105, 130, 238));

			blueBrush03 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(253, 216, 137), Color.FromArgb(248, 178, 48), 90.0f);

			bluePen10 = new Pen(Color.FromArgb(255, 240, 207));
			bluePen11 = new Pen(Color.FromArgb(253, 216, 137));
			bluePen12 = new Pen(Color.FromArgb(248, 178, 48));
			bluePen13 = new Pen(Color.FromArgb(229, 151, 0));

			brush1 = new SolidBrush(Color.FromArgb(92, 85, 125, 162));
			pen0 = new Pen(Color.FromArgb(0, 60, 116));
			pen1 = new Pen(Color.FromArgb(85, 125, 162));
			pen2 = new Pen(Color.FromArgb(192, 85, 125, 162));
		}

		private static void CreatePenBrushesDefaultScheme(int x, int y)
		{
			blueBrush01 = new LinearGradientBrush(new Rectangle(2, 2, x - 5, y - 7)
				, Color.FromArgb(255, 255, 255), Color.FromArgb(240, 240, 234), 90.0f);
			blueBrush02 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(186, 211, 245), Color.FromArgb(137, 173, 228), 90.0f);

			bluePen01 = new Pen(Color.FromArgb(236, 235, 230));
			bluePen02 = new Pen(Color.FromArgb(226, 223, 214));
			bluePen03 = new Pen(Color.FromArgb(214, 208, 197));

			var brush05 = new LinearGradientBrush(new Rectangle(x - 3, 4, 1, y - 5)
				, Color.FromArgb(245, 244, 242), Color.FromArgb(64, 186, 174, 160), 90.0f);
			var brush06 = new LinearGradientBrush(new Rectangle(x - 2, 4, 1, y - 5)
				, Color.FromArgb(240, 238, 234), Color.FromArgb(64, 175, 168, 142), 90.0f);

			bluePen04 = new Pen(brush05);
			bluePen05 = new Pen(brush06);

			bluePen06 = new Pen(Color.FromArgb(206, 231, 255));
			bluePen07 = new Pen(Color.FromArgb(188, 212, 246));
			bluePen08 = new Pen(Color.FromArgb(137, 173, 228));
			bluePen09 = new Pen(Color.FromArgb(105, 130, 238));

			blueBrush03 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 8)
				, Color.FromArgb(253, 216, 137), Color.FromArgb(248, 178, 48), 90.0f);
			bluePen10 = new Pen(Color.FromArgb(255, 240, 207));
			bluePen11 = new Pen(Color.FromArgb(253, 216, 137));
			bluePen12 = new Pen(Color.FromArgb(248, 178, 48));
			bluePen13 = new Pen(Color.FromArgb(229, 151, 0));

			brush1 = new SolidBrush(Color.FromArgb(92, 85, 125, 162));
			pen0 = new Pen(Color.FromArgb(0, 60, 116));
			pen1 = new Pen(Color.FromArgb(85, 125, 162));
			pen2 = new Pen(Color.FromArgb(192, 85, 125, 162));

			brush05.Dispose();
			brush06.Dispose();
		}

		private void ApplySchemeNormalState(PaintEventArgs e, int x, int y)
		{
			switch(scheme)
			{
				case Schemes.Silver:
					e.Graphics.FillRectangle(silverBrush01, 2, 2, x - 4, y - 4);
					e.Graphics.FillRectangle(silverBrush02, 3, 4, x - 6, y - 8);
					e.Graphics.FillRectangle(silverBrush03, 2, y - 4, x - 4, 2);

					if(selected)
					{
						using(var pen01 = new Pen(Color.White))
						{
							e.Graphics.FillRectangles(blueBrush02, rects0);
							e.Graphics.DrawLine(pen01, 3, 4, 3, y - 4);
							e.Graphics.DrawLine(pen01, x - 4, 4, x - 4, y - 4);
							e.Graphics.DrawLine(bluePen06, 2, 2, x - 3, 2);
							e.Graphics.DrawLine(bluePen07, 2, 3, x - 3, 3);
							e.Graphics.DrawLine(bluePen08, 2, y - 4, x - 3, y - 4);
							e.Graphics.DrawLine(bluePen09, 2, y - 3, x - 3, y - 3);

							pen01.Dispose();
						}
					}

					break;

				case Schemes.OliveGreen:
					e.Graphics.FillRectangle(oliveBrush01, 2, 2, x - 4, y - 7);
					e.Graphics.DrawLine(olivePen01, 2, y - 5, x - 2, y - 5);
					e.Graphics.DrawLine(olivePen02, 2, y - 4, x - 2, y - 4);
					e.Graphics.DrawLine(olivePen03, 2, y - 3, x - 2, y - 3);
					e.Graphics.DrawLine(olivePen04, x - 4, 4, x - 4, y - 5);
					e.Graphics.DrawLine(olivePen05, x - 3, 4, x - 3, y - 5);

					if(selected)
					{
						e.Graphics.FillRectangles(oliveBrush02, rects0);
						e.Graphics.DrawLine(olivePen06, 2, 2, x - 3, 2);
						e.Graphics.DrawLine(olivePen07, 2, 3, x - 3, 3);
						e.Graphics.DrawLine(olivePen08, 2, y - 4, x - 3, y - 4);
						e.Graphics.DrawLine(olivePen09, 2, y - 3, x - 3, y - 3);
					}

					break;

				default:
					e.Graphics.FillRectangle(blueBrush01, 2, 2, x - 4, y - 7);
					e.Graphics.DrawLine(bluePen01, 2, y - 5, x - 2, y - 5);
					e.Graphics.DrawLine(bluePen02, 2, y - 4, x - 2, y - 4);
					e.Graphics.DrawLine(bluePen03, 2, y - 3, x - 2, y - 3);
					e.Graphics.DrawLine(bluePen04, x - 4, 4, x - 4, y - 5);
					e.Graphics.DrawLine(bluePen05, x - 3, 4, x - 3, y - 5);

					if(selected)
					{
						e.Graphics.FillRectangles(blueBrush02, rects0);
						e.Graphics.DrawLine(bluePen06, 2, 2, x - 3, 2);
						e.Graphics.DrawLine(bluePen07, 2, 3, x - 3, 3);
						e.Graphics.DrawLine(bluePen08, 2, y - 4, x - 3, y - 4);
						e.Graphics.DrawLine(bluePen09, 2, y - 3, x - 3, y - 3);
					}

					break;
			}
		}

		private void ApplySchemeMouseOverState(PaintEventArgs e, int x, int y)
		{
			switch(scheme)
			{
				case Schemes.Silver:
					e.Graphics.FillRectangle(silverBrush01, 2, 2, x - 4, y - 4);
					e.Graphics.FillRectangle(silverBrush02, 3, 4, x - 6, y - 8);
					e.Graphics.FillRectangle(silverBrush03, 2, y - 4, x - 4, 2);

					e.Graphics.FillRectangles(blueBrush03, rects0);
					e.Graphics.DrawLine(bluePen10, 2, 2, x - 3, 2);
					e.Graphics.DrawLine(bluePen11, 2, 3, x - 3, 3);
					e.Graphics.DrawLine(bluePen12, 2, y - 4, x - 3, y - 4);
					e.Graphics.DrawLine(bluePen13, 2, y - 3, x - 3, y - 3);
					break;

				case Schemes.OliveGreen:
					e.Graphics.FillRectangle(oliveBrush01, 2, 2, x - 4, y - 7);
					e.Graphics.DrawLine(olivePen01, 2, y - 5, x - 4, y - 5);
					e.Graphics.DrawLine(olivePen02, 2, y - 4, x - 4, y - 4);
					e.Graphics.DrawLine(olivePen03, 2, y - 3, x - 4, y - 3);
					e.Graphics.DrawLine(olivePen04, x - 4, 4, x - 4, y - 5);
					e.Graphics.DrawLine(olivePen05, x - 3, 4, x - 3, y - 5);

					e.Graphics.FillRectangles(oliveBrush03, rects0);
					e.Graphics.DrawLine(olivePen10, 2, 2, x - 3, 2);
					e.Graphics.DrawLine(olivePen11, 2, 3, x - 3, 3);
					e.Graphics.DrawLine(olivePen12, 2, y - 4, x - 3, y - 4);
					e.Graphics.DrawLine(olivePen13, 2, y - 3, x - 3, y - 3);
					break;

				default:
					e.Graphics.FillRectangle(blueBrush01, 2, 2, x - 4, y - 7);
					e.Graphics.DrawLine(bluePen01, 2, y - 5, x - 4, y - 5);
					e.Graphics.DrawLine(bluePen02, 2, y - 4, x - 4, y - 4);
					e.Graphics.DrawLine(bluePen03, 2, y - 3, x - 4, y - 3);
					e.Graphics.DrawLine(bluePen04, x - 4, 4, x - 4, y - 5);
					e.Graphics.DrawLine(bluePen05, x - 3, 4, x - 3, y - 5);

					e.Graphics.FillRectangles(blueBrush03, rects0);
					e.Graphics.DrawLine(bluePen10, 2, 2, x - 3, 2);
					e.Graphics.DrawLine(bluePen11, 2, 3, x - 3, 3);
					e.Graphics.DrawLine(bluePen12, 2, y - 4, x - 3, y - 4);
					e.Graphics.DrawLine(bluePen13, 2, y - 3, x - 3, y - 3);
					break;
			}
		}

		private void ApplySchemePushedState(PaintEventArgs e, int x, int y)
		{
			LinearGradientBrush brush02;
			LinearGradientBrush brush03;
			LinearGradientBrush brush04;
			Pen pen01;
			Pen pen02;
			Pen pen03;
			Pen pen04;
			Pen pen05;
			Pen pen06;
			switch(scheme)
			{
				case Schemes.Silver:
					var brush01 = new SolidBrush(Color.White);
					brush02 = new LinearGradientBrush(new Rectangle(3, 3, x - 5, y - 8)
						, Color.FromArgb(172, 171, 191), Color.FromArgb(248, 252, 253), 90.0f);

					var relativeIntensities = new[]{0.0f, 0.992f, 1.0f};
					var relativePositions = new[]{ 0.0f, 0.68f, 1.0f};

					var blend = new Blend();
					blend.Factors = relativeIntensities;
					blend.Positions = relativePositions;
					brush02.Blend = blend;

					pen01 = new Pen(Color.FromArgb(172, 171, 189));

					e.Graphics.FillRectangle(brush01, 2, 2, x - 4, y - 4);
					e.Graphics.FillRectangle(brush02, 3, 4, x - 6, y - 9);
					e.Graphics.DrawLine(pen01, 4, 3, x - 4, 3);

					brush01.Dispose();
					brush02.Dispose();
					pen01.Dispose();
					break;

				case Schemes.OliveGreen:
					brush02 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 7)
						, Color.FromArgb(228, 212, 191), Color.FromArgb(229, 217, 195), 90.0f);
					brush04 = new LinearGradientBrush(new Rectangle(3, 3, x - 4, y - 7)
						, Color.FromArgb(232, 219, 197), Color.FromArgb(234, 224, 201), 90.0f);

					brush03 = new LinearGradientBrush(new Rectangle(2, 2, x - 5, y - 7)
						, Color.FromArgb(238, 230, 210), Color.FromArgb(236, 228, 206), 90.0f);

					pen01 = new Pen(Color.FromArgb(223, 205, 180));
					pen02 = new Pen(Color.FromArgb(231, 217, 195));
					pen03 = new Pen(Color.FromArgb(242, 236, 216));
					pen04 = new Pen(Color.FromArgb(248, 244, 228));
					pen05 = new Pen(brush02);
					pen06 = new Pen(brush04);

					e.Graphics.FillRectangle(brush03, 2, 4, x - 4, y - 8);
					e.Graphics.DrawLine(pen05, 2, 3, 2, y - 4);
					e.Graphics.DrawLine(pen06, 3, 3, 3, y - 4);
					e.Graphics.DrawLine(pen01, 2, 2, x - 3, 2);
					e.Graphics.DrawLine(pen02, 2, 3, x - 3, 3);
					e.Graphics.DrawLine(pen03, 2, y - 4, x - 3, y - 4);
					e.Graphics.DrawLine(pen04, 2, y - 3, x - 3, y - 3);

					brush02.Dispose();
					brush03.Dispose();
					brush04.Dispose();
					pen01.Dispose();
					pen02.Dispose();
					pen03.Dispose();
					pen04.Dispose();
					pen05.Dispose();
					pen06.Dispose();
					break;

				default:
					brush02 = new LinearGradientBrush(new Rectangle(2, 3, x - 4, y - 7)
						, Color.FromArgb(216, 212, 203), Color.FromArgb(218, 216, 207), 90.0f);
					brush04 = new LinearGradientBrush(new Rectangle(3, 3, x - 4, y - 7)
						, Color.FromArgb(221, 218, 209), Color.FromArgb(223, 222, 214), 90.0f);

					brush03 = new LinearGradientBrush(new Rectangle(2, 2, x - 5, y - 7)
						, Color.FromArgb(229, 228, 221), Color.FromArgb(226, 226, 218), 90.0f);

					pen01 = new Pen(Color.FromArgb(209, 204, 192));
					pen02 = new Pen(Color.FromArgb(220, 216, 207));
					pen03 = new Pen(Color.FromArgb(234, 233, 227));
					pen04 = new Pen(Color.FromArgb(242, 241, 238));
					pen05 = new Pen(brush02);
					pen06 = new Pen(brush04);

					e.Graphics.FillRectangle(brush03, 2, 4, x - 4, y - 8);
					e.Graphics.DrawLine(pen05, 2, 3, 2, y - 4);
					e.Graphics.DrawLine(pen06, 3, 3, 3, y - 4);
					e.Graphics.DrawLine(pen01, 2, 2, x - 3, 2);
					e.Graphics.DrawLine(pen02, 2, 3, x - 3, 3);
					e.Graphics.DrawLine(pen03, 2, y - 4, x - 3, y - 4);
					e.Graphics.DrawLine(pen04, 2, y - 3, x - 3, y - 3);

					brush02.Dispose();
					brush03.Dispose();
					brush04.Dispose();
					pen01.Dispose();
					pen02.Dispose();
					pen03.Dispose();
					pen04.Dispose();
					pen05.Dispose();
					pen06.Dispose();
					break;
			}
		}

		private void DisposePensBrushes()
		{
			brush1.Dispose();
			pen0.Dispose();
			pen1.Dispose();
			pen2.Dispose();
			switch (scheme)
			{
				case Schemes.OliveGreen:
					oliveBrush01.Dispose();
					oliveBrush02.Dispose();
					oliveBrush03.Dispose();
					olivePen01.Dispose();
					olivePen02.Dispose();
					olivePen03.Dispose();
					olivePen04.Dispose();
					olivePen05.Dispose();
					olivePen06.Dispose();
					olivePen07.Dispose();
					olivePen08.Dispose();
					olivePen09.Dispose();
					olivePen10.Dispose();
					olivePen11.Dispose();
					olivePen12.Dispose();
					olivePen13.Dispose();
					break;

				case Schemes.Silver:
					silverBrush01.Dispose();
					silverBrush02.Dispose();
					silverBrush03.Dispose();

					blueBrush02.Dispose();
					blueBrush03.Dispose();
					bluePen06.Dispose();
					bluePen07.Dispose();
					bluePen08.Dispose();
					bluePen09.Dispose();
					bluePen10.Dispose();
					bluePen11.Dispose();
					bluePen12.Dispose();
					bluePen13.Dispose();
					break;

				default:
					blueBrush01.Dispose();
					blueBrush02.Dispose();
					blueBrush03.Dispose();
					bluePen01.Dispose();
					bluePen02.Dispose();
					bluePen03.Dispose();
					bluePen04.Dispose();
					bluePen05.Dispose();
					bluePen06.Dispose();
					bluePen07.Dispose();
					bluePen08.Dispose();
					bluePen09.Dispose();
					bluePen10.Dispose();
					bluePen11.Dispose();
					bluePen12.Dispose();
					bluePen13.Dispose();
					break;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnClick(System.EventArgs e)
		{
			if (state == States.Pushed)
			{
				state = States.Normal;
				this.Invalidate(bounds);
				base.OnClick(e);
			}
		}

		private void InitializeComponent()
		{
			// 
			// ButtonXP
			// 
			this.Name = "ButtonXP";
			this.Size = new System.Drawing.Size(86, 22);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnEnter(System.EventArgs e)
		{
			selected = true;
			this.Invalidate(bounds);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Leave"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnLeave(System.EventArgs e)
		{
			selected = false;
			this.Invalidate(bounds);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnParentChanged(System.EventArgs e)
		{
			if (this.Parent == null) return;

			int X = this.Width;
			int Y = this.Height;

			this.bounds = new Rectangle(0, 0, X, Y);

			rects0 = new Rectangle[2];
			rects0[0] = new Rectangle(2, 4, 2, Y-8);
			rects0[1] = new Rectangle(X-4, 4, 2, Y-8);

			rects1 = new Rectangle[8];
			rects1[0] = new Rectangle(2, 1, 2, 2); 
			rects1[1] =	new Rectangle(1, 2, 2, 2);
			rects1[2] =	new Rectangle(X-4, 1, 2, 2);
			rects1[3] =	new Rectangle(X-3, 2, 2, 2);
			rects1[4] =	new Rectangle(2, Y-3, 2, 2);
			rects1[5] =	new Rectangle(1, Y-4, 2, 2);
			rects1[6] =	new Rectangle(X-4, Y-3, 2, 2);
			rects1[7] =	new Rectangle(X-3, Y-4, 2, 2);

			//this.BackColor = Color.FromArgb(0, this.Parent.BackColor);
			
			Point[] points = {
								 new Point(1, 0),
								 new Point(X-1, 0),
								 new Point(X-1, 1),
								 new Point(X, 1),
								 new Point(X, Y-1),
								 new Point(X-1, Y-1),
								 new Point(X-1, Y),
								 new Point(1, Y),
								 new Point(1, Y-1),
								 new Point(0, Y-1),
								 new Point(0, 1),
								 new Point(1, 1)};

			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);

			this.Region = new Region(path);

		}

		/// <summary>
		/// Raises the key down event.
		/// </summary>
		/// <param name="ke">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			if (ke.KeyData == Keys.Enter)
			{
				OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, 2, 2, 0));
			}
		}

		/// <summary>
		/// Raises the key up event.
		/// </summary>
		/// <param name="ke">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
		protected override void OnKeyUp(KeyEventArgs ke)
		{
			if (ke.KeyData == Keys.Enter)
			{
				OnClick(new EventArgs());
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnResize(System.EventArgs e)
		{
			bounds = new Rectangle(0, 0, this.Width, this.Height);
			OnParentChanged(e);
			this.Invalidate(bounds);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			this.Invalidate(bounds);
		}

		/// <summary>
		/// Gets the text size.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static Size GetTextSize(Graphics graphics, string text, Font font, Size size)
		{
			StringFormat format = new StringFormat();
			SizeF stringSize = graphics.MeasureString(text, font, size.Width, format);
			
			return new Size((int)stringSize.Width, (int)stringSize.Height);
		}

	}
}
