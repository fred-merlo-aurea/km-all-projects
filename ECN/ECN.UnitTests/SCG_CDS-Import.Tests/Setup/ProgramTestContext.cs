using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCG_CDS_Import.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class ProgramTestContext
    {
        public ProgramTestContext()
        {
            UpdateToDBXmlProfile = new List<string>();
            UpdateToDBXmlUDF = new List<string>();
            UpdateToDBImportFile = new List<ImportFile>();
            ImportFileLogs = new List<string>();
            Logs = new List<string>();
        }
        public Exception LogCriticalErrorException { get; set; }

        public string LogCriticalErrorSourceMethod { get; set; }

        public int LogCriticalErrorApplicationID { get; set; }

        public string LogCriticalErrorNote { get; set; }

        public int LogCriticalErrorGDCharityID { get; set; }

        public int LogCriticalErrorECNCustomerID { get; set; }

        public int LogCriticalErrorReturnValue { get; internal set; }

        public IList<string> UpdateToDBXmlProfile { get; set; }

        public IList<string> UpdateToDBXmlUDF { get; set; }

        public IList<ImportFile> UpdateToDBImportFile { get; set; }

        public string EmailFunctionsNotifyAdminSubject { get; set; }

        public string EmailFunctionsNotifyAdminTextMessage { get; set; }

        public string EmailFunctionsNotifyAdminReturnValue { get; set; }

        public IList<string> ImportFileLogs { get; set; }

        public Exception AppSettingsExceptionToThrow { get; set; }

        public IList<string> Logs { get; set; }
    }
}
