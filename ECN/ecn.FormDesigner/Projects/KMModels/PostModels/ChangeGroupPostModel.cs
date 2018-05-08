using System;
using System.Collections.Generic;

namespace KMModels.PostModels
{
    public class ChangeGroupPostModel
    {
        public int FormId { get; set; }

        public int CustomerId { get; set; }

        public int GroupId { get; set; }

        public bool ChangeFormGroup { get; set; }

        public IEnumerable<ControlFieldModel> Fields { get; set; }
    }
}
