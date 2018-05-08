using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KM.Common;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public class LoadActivityFilterArgs
    {
        public void EnsureNotNull()
        {
            Guard.NotNull(Field, nameof(Field));
            Guard.NotNull(DrpDateRange, nameof(DrpDateRange));
            Guard.NotNull(DivDateRange, nameof(DivDateRange));
            Guard.NotNull(TxtFrom, nameof(TxtFrom));
            Guard.NotNull(TxtTo, nameof(TxtTo));
            Guard.NotNull(DrpDays, nameof(DrpDays));
            Guard.NotNull(TxtActivityDays, nameof(TxtActivityDays));
            Guard.NotNull(DivYear, nameof(DivYear));
            Guard.NotNull(TxtFromYear, nameof(TxtFromYear));
            Guard.NotNull(TxtToYear, nameof(TxtToYear));
            Guard.NotNull(DivMonth, nameof(DivMonth));
            Guard.NotNull(TxtFromMonth, nameof(TxtFromMonth));
            Guard.NotNull(TxtoMonth, nameof(TxtoMonth));
        }

        public Field Field { get; set; }
        public DropDownList DrpDateRange { get; set; }
        public HtmlGenericControl DivDateRange { get; set; }
        public TextBox TxtFrom { get; set; }
        public TextBox TxtTo { get; set; }
        public DropDownList DrpDays { get; set; }
        public TextBox TxtActivityDays { get; set; }
        public HtmlGenericControl DivYear { get; set; }
        public TextBox TxtFromYear { get; set; }
        public TextBox TxtToYear { get; set; }
        public HtmlGenericControl DivMonth { get; set; }
        public TextBox TxtFromMonth { get; set; }
        public TextBox TxtoMonth { get; set; }
    }
}