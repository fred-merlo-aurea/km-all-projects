using KMEntities;
using ControlType = KMEnums.ControlType;

namespace KMManagers
{
    public class NoRewriteArgs
    {
        public ControlPropertyManager PropertyManager { get; set; }
        public Control Control { get; set; }
        public string RulesToAdd { get; set; }
        public ControlType ControlType { get; set; }
        public bool IsNeedRequired { get; set; }
        public string Result { get; set; }
    }
}