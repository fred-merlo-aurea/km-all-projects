using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMapperWizard.Helpers
{
    public class SaveTransformationParameters
    {
        public int TransformationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TransformationCodeId { get; set; }
        public int ClientId { get; set; }
        public int AuthorizedUserId { get; set; }
    }
}
