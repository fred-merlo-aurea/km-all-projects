using System.Collections.Generic;
using KMPS.MD.Objects;
using EntityFilterSegmentation = FrameworkUAD.Entity.FilterSegmentation;

namespace KMPS.MD.Controls
{
    public class FilterSegmentationSaveArgs
    {
        public bool IsAddNew { get; set; }
        public bool IsEdit { get; set; }
        public bool IsAddExisting { get; set; }
        public bool IsFilterSegmentScheduled { get; set; }
        public bool DeleteGroup { get; set; }
        public int FilterId { get; set; }
        public int FilterSegmentationId { get; set; }
        public EntityFilterSegmentation FilterSegmentation { get; set; }
        public IList<FilterGroup> FilterGroups { get; set; } = new List<FilterGroup>();
    }
}