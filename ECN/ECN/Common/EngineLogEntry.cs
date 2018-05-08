using System;
using System.Collections;
using System.Text;

namespace ecn.common.classes
{
	public class EngineLogEntry {
		public EngineLogEntry(string engineName, DateTime date, string action, string status) {
			_engineName = engineName;
			_date = date;
			_action = action;
			_status = status;
		}

		private string _engineName;
		public string EngineName {
			get {
				return (this._engineName);
			}
			set {
				this._engineName = value;
			}
		}

		private DateTime _date;
		public DateTime Date {
			get {
				return (this._date);
			}
			set {
				this._date = value;
			}
		}

		private string _action;
		public string Action {
			get {
				return (this._action);
			}
			set {
				this._action = value;
			}
		}

		private string _status=string.Empty;
		public string Status {
			get {
				return (this._status);
			}
			set {
				this._status = value;
			}
		}

		private string _errorMessage=string.Empty;
		public string ErrorMessage {
			get {
				return (this._errorMessage);
			}
			set {
				this._errorMessage = value;
			}
		}	
	}

	public class LogWritter {
		public LogWritter(bool outputToConsole) {
			_outputToConsole = outputToConsole;
			_log = new ArrayList();
		}

		ArrayList _log;
		private bool _outputToConsole;
		public bool OutputToConsole {
			get {
				return (this._outputToConsole);
			}
			set {
				this._outputToConsole = value;
			}
		}
	
		public void Log(EngineLogEntry logEntry) {
			_log.Add(logEntry);
			if (OutputToConsole) {
				System.Console.WriteLine(string.Format("{0}:{1} -> {2} {3}",
				logEntry.Date,logEntry.Action,logEntry.Status, logEntry.ErrorMessage));
			}
		}
		
		
		public void Flush() {
			StringBuilder sql = new StringBuilder();			
			foreach(EngineLogEntry log in _log) {
				sql.Append(string.Format(@"INSERT INTO EngineLog (EngineName, ActionDate, [Action], Status, ErrorMessage) VALUES
					('{0}', '{1}', '{2}', '{3}', '{4}');
", log.EngineName, log.Date, DataFunctions.CleanString(log.Action), log.Status, DataFunctions.CleanString(log.ErrorMessage)));
			}
			DataFunctions.Execute(sql.ToString());
		}

		public EngineLogEntry LastEntry {
			get {
				if (_log == null || _log.Count == 0) {
					return null;
				}

				return _log[_log.Count-1] as EngineLogEntry;
			}
		}

		public void RemoveTheLastEntry() {
			if (_log.Count == 0) {
				return;
			}
			_log.RemoveAt(_log.Count -1);
		}
	}
}
