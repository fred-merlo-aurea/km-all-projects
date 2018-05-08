using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EmailPreview
{
    [Serializable]
    [DataContract]
    public class CodeAnalysisTest
    {
        public CodeAnalysisTest() 
        { 
            CompatibilityProblems = new List<CodeAnalysisPotentialProblems>();
            CompatibilityRulesCount = 0;
            HtmlProblems = new List<CodeAnalysisHtmlValidation>();
            HTML = string.Empty;
        }
        #region Properties
        [DataMember]
        public List<CodeAnalysisPotentialProblems> CompatibilityProblems { get; set; }
        [DataMember]
        public int CompatibilityRulesCount { get; set; }
        [DataMember]
        public List<CodeAnalysisHtmlValidation> HtmlProblems { get; set; }

        [DataMember]
        public string HTML { get; set; }
        #endregion
    }

    public class CodeAnalysisPotentialProblems
    {
        public CodeAnalysisPotentialProblems() { }
        #region Properties
        [DataMember]
        public List<string> ApiIds { get; set; }
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public int Severity { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion
    }

    public class CodeAnalysisHtmlValidation
    {
        #region Properties
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion
    }

    public class CodeAnalysisResult
    {
        public CodeAnalysisResult() { }
        #region Properties
        [DataMember]
        public string ApplicationID { get; set; }
        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public string ApplicationLongName { get; set; }
        [DataMember]
        public List<CodeAnalysisResultDetail> CodeAnalysisResultDetails { get; set; }
        #endregion
    }

    public class CodeAnalysisResultDetail
    {
        public CodeAnalysisResultDetail() { }
        public CodeAnalysisResultDetail(int linenumber, int severity, string description)
        {
            LineNumber = linenumber;
            Severity = severity;
            Description = description;
        }

        #region Properties
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public int Severity { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion
    }
}
