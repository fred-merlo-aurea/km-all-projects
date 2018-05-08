using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KMPS_Tools.Models
{
    public class Blasts
    {
        public Blasts()
        {
            _BlastID = 0;
            _CustomerID = 0;
            _SendTime = null;
            _EmailSubject = "";
            _EmailFrom = "";
            _CustomerName = "";
        }

        #region Private Variables
        private int? _BlastID;
        private int? _CustomerID;
        private DateTime? _SendTime;
        private string _EmailSubject;
        private string _EmailFrom;
        private string _CustomerName;
        #endregion

        #region Public Properties
        public int? BlastID
        {
            get
            {
                return _BlastID;
            }
            set
            {
                _BlastID = value;
            }
        }
        public int? CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }
        public DateTime? SendTime
        {
            get
            {
                return _SendTime;
            }
            set
            {
                _SendTime = value;
            }
        }
        public string EmailSubject
        {
            get
            {
                return _EmailSubject;
            }
            set
            {
                _EmailSubject = value;
            }
        }
        public string EmailFrom
        {
            get
            {
                return _EmailFrom;
            }
            set
            {
                _EmailFrom = value;
            }
        }
        public string CustomerName
        {
            get
            {
                return _CustomerName;
            }
            set
            {
                _CustomerName = value;
            }
        }
        #endregion
    }

    public class EmailActivityLog
    {
        public EmailActivityLog()
        {
            _EAID = null;
            _EmailID = null;
            _BlastID = null;
            _ActionTypeCode = "";
            _EmailName = "";
            _EmailDomain = "";
            _ActionDate = null;
            _ActionNotes = "";
        }

        #region Private Variables
        private int? _EAID;
        private int? _EmailID;
        private int? _BlastID;
        private string _ActionTypeCode;
        private string _EmailName;
        private string _EmailDomain;
        private DateTime? _ActionDate;
        private string _ActionNotes;
        #endregion

        #region Public Properties
        public int? EAID
        {
            get
            {
                return _EAID;
            }
            set
            {
                _EAID = value;
            }
        }
        public int? EmailID
        {
            get
            {
                return _EmailID;
            }
            set
            {
                _EmailID = value;
            }
        }
        public int? BlastID
        {
            get
            {
                return _BlastID;
            }
            set
            {
                _BlastID = value;
            }
        }
        public string ActionTypeCode
        {
            get
            {
                return _ActionTypeCode;
            }
            set
            {
                _ActionTypeCode = value;
            }
        }
        public string EmailName
        {
            get
            {
                return _EmailName;
            }
            set
            {
                _EmailName = value;
            }
        }
        public string EmailDomain
        {
            get
            {
                return _EmailDomain;
            }
            set
            {
                _EmailDomain = value;
            }
        }
        public DateTime? ActionDate
        {
            get
            {
                return _ActionDate;
            }
            set
            {
                _ActionDate = value;
            }
        }
        public string ActionNotes
        {
            get
            {
                return _ActionNotes;
            }
            set
            {
                _ActionNotes = value;
            }
        }
        #endregion
    }


}
