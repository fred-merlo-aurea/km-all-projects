using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMapperWizard.Helpers
{
    public class ApplyTransformationParameters
    {
        public int TransformationID { get; set; }
        public int FieldMappingID { get; set; }
        public int SourceFileID { get; set; }
        public int AuthorizedUserId { get; set; }
    }
}
