using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KMEnums;

namespace KMModels.PostModels
{
    public class FormPostModel : FormPostModelBase
    {
        [GetFromField("FormType")]
        public String Type { get; set; }
    }
}