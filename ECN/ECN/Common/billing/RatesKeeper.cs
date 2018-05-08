using System;
using System.Collections;
using System.Xml.Serialization;
using System.Text;
using System.IO;

namespace ecn.common.classes.billing {

	[XmlInclude(typeof(RangeAndRate))]
	public class RatesKeeper {
		public RatesKeeper() {
			RangeAndRates = new ArrayList();
		}

		
		public ArrayList RangeAndRates;
	
		private double _defaultRate;
		public double DefaultRate {
			get {
				return (this._defaultRate);
			}
			set {
				this._defaultRate = value;
			}
		}

	
		public void Add(int lowerBoundary, int upperBoundary, double rate) {
			RangeAndRates.Add(new RangeAndRate(lowerBoundary, upperBoundary, rate));
		}

		public double GetRate(int val) {
			foreach(RangeAndRate rr in RangeAndRates) {
				if (rr.IsBetween(val)) {
					return rr.Rate;
				}
			}
			return DefaultRate;
		}		

		#region Static XML Serialization Methods
		public static string Serialize(RatesKeeper keeper) {
			if (keeper == null) {
				return string.Empty;
			}

			XmlSerializer xml = new XmlSerializer(typeof(RatesKeeper));
			
			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);
			xml.Serialize(writer, keeper);			
			writer.Flush();
			writer.Close();
			return sb.ToString();
		}

		public static RatesKeeper Deserialize(string xmlString) {
			if (xmlString == null) {
				return null;
			}

			if (xmlString.Trim() == string.Empty) {
				return null;
			}

			
			XmlSerializer xml = new XmlSerializer(typeof(RatesKeeper));			
			StringReader reader = new StringReader(xmlString);
			return xml.Deserialize(reader) as RatesKeeper;
		}
		#endregion		
	}


	public class RangeAndRate {
		public RangeAndRate() : this(0,0,0) {}
		public RangeAndRate(int lowerBoundary, int upperBoundary, double rate) {
			_lowerBoundary = lowerBoundary;
			_upperBoundary = upperBoundary;
			_rate = rate;
		}	
		private int _lowerBoundary;
		public int LowerBoundary {
			get {
				return (this._lowerBoundary);
			}
			set {
				this._lowerBoundary = value;
			}
		}

		private int _upperBoundary;
		public int UpperBoundary {
			get {
				return (this._upperBoundary);
			}
			set {
				this._upperBoundary = value;
			}
		}
	
		private double _rate;
		public double Rate {
			get {
				return (this._rate);
			}
			set {
				this._rate = value;
			}
		}

		public bool IsBetween(int val) {
			if (val < UpperBoundary && val >= LowerBoundary) {
				return true;
			}

			return false;
		}
	}
}
