using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using ecn.common.classes.billing;

namespace ecn.common.classes.license {	
	public enum ServiceTypeEnum { ClientInquirie, Training }
	public class Service {

		public Service() : this(0, 0) {}

		public Service(int allowedInquirieNumber, double rateForAdditionalInquires) : this(allowedInquirieNumber, rateForAdditionalInquires, ServiceTypeEnum.ClientInquirie) {			
		}

		public Service(int allowedInquirieNumber, double rateForAdditionalInquires, ServiceTypeEnum serviceType) {			
			_allowedInquirieCount = allowedInquirieNumber;
			_rateForAdditionalInquiries = rateForAdditionalInquires;
			_type = serviceType;
		}

		private int _quoteItemID;
		public int QuoteItemID {
			get {
				return (this._quoteItemID);
			}
			set {
				this._quoteItemID = value;
			}
		}

		private ServiceTypeEnum _type = ServiceTypeEnum.ClientInquirie;
		public ServiceTypeEnum ServiceType {
			get {
				return (this._type);
			}
			set {
				this._type = value;
			}
		}


		private int _allowedInquirieCount;
		public int AllowedInquirieCount {
			get {
				return (this._allowedInquirieCount);
			}		
			set {
				_allowedInquirieCount = value;
			}
		}

		private double _rateForAdditionalInquiries;
		public double RateForAdditionalInquiries {
			get {
				return (this._rateForAdditionalInquiries);
			}
			set {
				_rateForAdditionalInquiries = value;
			}
		}

		public override string ToString() {
			return string.Format("{0} {2}.(Each additional item is ${1})", AllowedInquirieCount, RateForAdditionalInquiries, ServiceType);
		}


		#region Static XML Serialization Methods
		public static string SerializeService(Service service) {
			XmlSerializer serviceSerializer = new XmlSerializer(typeof(Service));
			StringBuilder serviceString = new StringBuilder();
			StringWriter writer = new StringWriter(serviceString);
			serviceSerializer.Serialize(writer,service);		
			return serviceString.ToString();
		}
		public static Service DeserializeService(string service) {
			if (service == null || service.Trim().Length == 0) {
				return null;
			}

			StringReader reader = new StringReader(service);
			XmlSerializer serviceSerializer = new XmlSerializer(typeof(Service));
			return (Service) serviceSerializer.Deserialize(reader);
		}

		public static string SerializeServices(ServiceCollection services) {
			if (services == null || services.Count == 0) {
				return string.Empty;
			}
			XmlSerializer serviceSerializer = new XmlSerializer(typeof(ServiceCollection));
			StringBuilder serviceString = new StringBuilder();
			StringWriter writer = new StringWriter(serviceString);
			serviceSerializer.Serialize(writer,services);		
			return serviceString.ToString();
		}

		public static ServiceCollection DeserializeServices(string services) {
			if (services == null || services.Trim().Length == 0) {
				return new ServiceCollection();
			}

			StringReader reader = new StringReader(services);
			XmlSerializer serviceSerializer = new XmlSerializer(typeof(ServiceCollection));
			return (ServiceCollection) serviceSerializer.Deserialize(reader);
		}
		#endregion
	}

	public class ServiceCollection : CollectionBase {
		public void Add(Service service) {
			base.InnerList.Add(service);
		}

		public Service this[int index] {
			get { return (Service) base.InnerList[index];}
			set { base.InnerList[index] = value;}
		}

		public Service FindByServiceName(string serviceName) {
			foreach(Service service in base.InnerList) {
				if (service.ServiceType.ToString() == serviceName) {
					return service;
				}
			}
			return null;
		}

		public override string ToString() {
			StringBuilder names = new StringBuilder();
			foreach(Service s in this.InnerList) {
				if (names.Length > 0) {
					names.Append(Environment.NewLine);
				}
				names.Append(s.ToString());
			}
			return names.ToString();
		}

	}
}
