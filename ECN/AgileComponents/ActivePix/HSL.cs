using System;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="HSL"/> object.
	/// </summary>
	public class HSL
	{
		private float h;
		private float s;
		private float l;

		/// <summary>
		/// Gets or sets the hue.
		/// </summary>
		/// <value>The hue.</value>
		public float Hue
		{
			get
			{
				return h;
			}
			set
			{
				h = Math.Abs(value)%360;
			}
		}

		/// <summary>
		/// Gets or sets the saturation.
		/// </summary>
		/// <value>The saturation.</value>
		public float Saturation
		{
			get
			{
				return s;
			}
			set
			{
				s = (float)Math.Max(Math.Min(1.0, value), 0.0);
			}
		}

		/// <summary>
		/// Gets or sets the luminance.
		/// </summary>
		/// <value>The luminance.</value>
		public float Luminance
		{
			get
			{
				return l;
			}
			set
			{
				l = (float)Math.Max(Math.Min(1.0, value), 0.0);
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="HSL"/> class.
		/// </summary>
		private HSL()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HSL"/> class.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="luminance">The luminance.</param>
		public HSL(float hue, float saturation, float luminance)
		{
			Hue = hue;
			Saturation = saturation;
			Luminance = luminance;
		}


		/// <summary>
		/// Gets the RGB.
		/// </summary>
		/// <value>The RGB.</value>
		public Color RGB
		{
			get
			{
				double r=0,g=0,b=0; 

				double temp1,temp2; 

				double normalisedH = h/360.0;

				if(l==0) 
				{ 
					r=g=b=0; 
				} 
				else 
				{ 
					if(s==0) 
					{ 
						r=g=b=l; 
					} 
					else 
					{ 
						temp2 = ((l<=0.5) ? l*(1.0+s) : l+s-(l*s)); 

						temp1 = 2.0*l-temp2; 

						double[] t3=new double[]{normalisedH+1.0/3.0,normalisedH,normalisedH-1.0/3.0}; 

						double[] clr=new double[]{0,0,0}; 

						for(int i=0;i<3;++i) 
						{ 
							if(t3[i]<0) 
								t3[i]+=1.0; 

							if(t3[i]>1) 
								t3[i]-=1.0; 

							if(6.0*t3[i] < 1.0) 
								clr[i]=temp1+(temp2-temp1)*t3[i]*6.0; 
							else if(2.0*t3[i] < 1.0) 
								clr[i]=temp2; 
							else if(3.0*t3[i] < 2.0) 
								clr[i]=(temp1+(temp2-temp1)*((2.0/3.0)-t3[i])*6.0); 
							else 
								clr[i]=temp1; 

						} 

						r=clr[0]; 
						g=clr[1]; 
						b=clr[2]; 
					} 

				} 
				return Color.FromArgb((int)(255*r),(int)(255*g),(int)(255*b)); 
			}
		}

		/// <summary>
		/// <see cref="HSL"/> from the RGB colors.
		/// </summary>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		/// <returns></returns>
		public static HSL FromRGB(byte red, byte green, byte blue)
		{
			return FromRGB(Color.FromArgb(red, green, blue));
		}

		/// <summary>
		/// <see cref="HSL"/> from the RGB color.
		/// </summary>
		/// <param name="c">The color.</param>
		/// <returns></returns>
		public static HSL FromRGB(Color c)
		{
			return new HSL(c.GetHue(), c.GetSaturation(), c.GetBrightness());
		}
	}
}
