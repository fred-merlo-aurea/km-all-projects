using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ADMS
{
    public class FileProperty
    {
        public List<string> FindUnexpectedColumns(List<string> NewColumns, List<string> ExpectedColumns)
        {
            return NewColumns.Where(x => !ExpectedColumns.Any(x1 => x1.Equals(x,StringComparison.CurrentCultureIgnoreCase))).ToList();
        }

        public List<string> FindNotFoundColumns(List<string> NewColumns, List<string> ExpectedColumns)
        {
            return ExpectedColumns.Where(x => !NewColumns.Any(x1 => x1.Equals(x,StringComparison.CurrentCultureIgnoreCase))).ToList();
        }

    }
}
