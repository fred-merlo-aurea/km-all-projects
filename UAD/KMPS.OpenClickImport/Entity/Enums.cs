using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.ActivityImport.Entity
{
    public enum ProcessType
    {
        OpenImport,
        ClickImport,
        TopicImport,
        VisitImport,
        StatusUpdateImport
    }

    public enum FileExtension
    {
        xls,
        xlxs,
        csv,
        txt,
        xml
    }
    public enum ColumnDelimiter
    {
        comma,
        tab,
        semicolon,
        colon,
        tild
    }
}
