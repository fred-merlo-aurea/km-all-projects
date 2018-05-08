using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class Error
    {
        /// <summary>
        /// Identification number for this Error
        /// </summary>
        public int LogID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ApplicationID { get; set; }
        /// <summary>
        /// Integer for assigning Severity
        /// </summary>
        public int SeverityID { get; set; }
        /// <summary>
        /// The method where this error originated
        /// </summary>
        public string SourceMethod { get; set; }
        /// <summary>
        /// System exception
        /// </summary>
        public string Exception { get; set; }
        public string LogNote { get; set; }
        
        #region Nested exception class (not used)
        /*
        public class Exception : System.Exception
        {
            public Exception() : base() { }

            public Exception(string message) : base(message) { }
            public Exception(string message, System.Exception e) : base(message, e) { }
            public static implicit operator string(Exception e)
            {
                return KM.Common.Entity.ApplicationLog.FormatException(e);
            }
            public static implicit operator Exception(System.Exception e)
            {
                Exception newException = null != e.InnerException
                                       ? new Exception(e.Message, e.InnerException)
                                       : new Exception(e.Message);
                newException.HelpLink = e.HelpLink;
                newException.HResult = e.HResult;
                newException.Source = e.Source;
                //InnerException = e.InnerException,
                //TargetSite = e.TargetSite
                foreach (var key in e.Data.Keys)
                {
                    newException.Data.Add(key, e.Data[key]);
                }
                return newException;
            }
            public static implicit operator Exception(string message)
            {
                return new Exception(message);
            }
        }
        */ 
        #endregion Nested exception class
    }
}