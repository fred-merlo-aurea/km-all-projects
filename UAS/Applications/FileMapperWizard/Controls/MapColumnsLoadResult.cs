using System.Collections.Generic;
using FrameworkUAS.Entity;

namespace FileMapperWizard.Controls
{
    internal class MapColumnsLoadResult 
    {
        public IList<FieldMapping> Mapping { get; set; }
        public IList<TransformationFieldMap> Alltfm { get; set; }
        public IList<TransformationFieldMultiMap> Alltfmm { get; set; }
    }
}