using System;
using System.IO;
using System.Text;
using KM.Common;
using KM.Common.Utilities.Logging;

namespace KMPS.Hubspot.Integration
{
	public class KMPSLogger
	{
		private StreamWriter _mainLog;
		private StreamWriter _customerLog;

		private int _applicationID = 0;
		public KMPSLogger(StreamWriter mainLog, StreamWriter customerLog, int applicationID)
		{
			_mainLog = mainLog;
			_customerLog = customerLog;
			_applicationID = applicationID;
		}

		public void MainLogWrite(string text)
		{
			Console.WriteLine(text.ToString());
            StreamLogger.LogAndFlushWriter(_mainLog, text);
		}

		public void MainLogWrite(StringBuilder text)
		{
            MainLogWrite(text.ToString());
		}

		public void CustomerLogWrite(string text)
		{
			Console.WriteLine(text.ToString());
            StreamLogger.LogAndFlushWriter(_customerLog, text);
		}

		public void CustomerLogWrite(StringBuilder text)
		{
            CustomerLogWrite(text.ToString());
		}

	    public StringBuilder FormatException(Exception ex)
	    {
	        return new StringBuilder(StringFunctions.FormatException(ex));
	    }

	    public void LogCustomerExeception(Exception ex, string method)
		{
			if (ex.Message.Contains("No File in FTP Folder;"))
				KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, Constants.KmpsHubimportintegrationAppName + "." + method, _applicationID, Constants.KmpsHubimportintegrationAppName + ": Unhandled Exception", -1, -1);
			else
				KM.Common.Entity.ApplicationLog.LogCriticalError(ex, Constants.KmpsHubimportintegrationAppName + "." + method, _applicationID, Constants.KmpsHubimportintegrationAppName + ": Unhandled Exception", -1, -1);
			CustomerLogWrite(StringFunctions.FormatException(ex));
		}
		public void LogMainExeception(Exception ex, string method)
		{
			KM.Common.Entity.ApplicationLog.LogCriticalError(ex, Constants.KmpsHubimportintegrationAppName + "." + method, _applicationID, Constants.KmpsHubimportintegrationAppName + ": Unhandled Exception", -1, -1);
			MainLogWrite(StringFunctions.FormatException(ex));
		}
	}
}