using System.Collections.Generic;
using System.Linq;
using KMPS.MD.Objects;

namespace UAD.Web.Admin.Models
{
    public class CodeSheetWrapper
    {
        public CodeSheetWrapper(List<Pubs> p)
        {
            codeSheets = Enumerable.Empty<CodeSheet>();
            pubs = p;
            responseGroup = new List<ResponseGroup>();
            pubID = 0;
            responseGroupID = 0;
        }
        public IEnumerable<CodeSheet> codeSheets { get; set; }
        public List<Pubs> pubs { get; set; }
        public List<ResponseGroup> responseGroup { get; set; }
        public int pubID;
        public int responseGroupID;
    }
}