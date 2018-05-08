using System;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace KMPS.MD.Administration.Models
{
    public class DataLoadComponent
    {
        private const string ExceptionSessionKeyPrefixNullOrWhiteSpace = "Session key prefix is null or white space.";

        public DataLoadComponent(HttpSessionState session, string sessionKeyPrefix)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (string.IsNullOrWhiteSpace(sessionKeyPrefix))
            {
                throw new ArgumentException(ExceptionSessionKeyPrefixNullOrWhiteSpace);
            }

            _session = session;
            _keyStep1Done = $"{sessionKeyPrefix}1Done";
            _keyStep1Continue = $"{sessionKeyPrefix}1Continue";
            _keyStep2Done = $"{sessionKeyPrefix}2Done";
            _keyStep3Done = $"{sessionKeyPrefix}3Done";
            _keyStep4Done = $"{sessionKeyPrefix}4Done";
        }

        private readonly HttpSessionState _session;
        private readonly string _keyStep1Done;
        private readonly string _keyStep1Continue;
        private readonly string _keyStep2Done;
        private readonly string _keyStep3Done;
        private readonly string _keyStep4Done;

        public bool Step1Done
        {
            get { return (bool?)_session[_keyStep1Done] ?? false; }
            set { _session[_keyStep1Done] = value; }
        }

        public bool Step1Continue
        {
            get { return (bool?)_session[_keyStep1Continue] ?? false; }
            set { _session[_keyStep1Continue] = value; }
        }

        public bool Step2Done
        {
            get { return (bool?)_session[_keyStep2Done] ?? false; }
            set { _session[_keyStep2Done] = value; }
        }

        public bool Step3Done
        {
            get { return (bool?)_session[_keyStep3Done] ?? false; }
            set { _session[_keyStep3Done] = value; }
        }

        public bool Step4Done
        {
            get { return (bool?)_session[_keyStep4Done] ?? false; }
            set { _session[_keyStep4Done] = value; }
        }

        public DataLoadMessages Messages { get;set; }
        public DropDownList DbDropDownList { get; set; }
        public Button ActionButton { get; set; }

        public void Reset()
        {
            Step1Done = false;
            Step1Continue = false;
            Step2Done = false;
            Step3Done = false;
            Step4Done = false;
            Messages?.Reset();
        }
    }
}
