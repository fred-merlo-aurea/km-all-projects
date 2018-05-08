using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEnums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Mvc;

namespace KMModels.Controls
{
    [Bind(Exclude = "Usages")]
    public abstract class Control
    {
        protected const string prepopulatefrom_property = "Prepopulate from";
        protected const string querystring_property = "Querystring Parameter";
        public const string datatype_property = "Data Type";
        protected const string regex_property = "Regular expression";
        protected const string characters_property = "Characters";
        protected const string fieldsize_property = "Field Size";
        protected const string numberofcolumns_property = "Number of Columns";
        protected const string numberofvaluesallowed_property = "Number of Values allowed";
        protected const string columns_property = "Columns";
        protected const string rows_property = "Rows";
        protected const string gridvalidation_property = "Grid Validation";
        protected const string gridcontrols_property = "Grid Controls";
        protected const string previousbutton_property = "Previous";
        protected const string nextbutton_property = "Next";
        protected const string submitbutton_property = "Submit";
        //protected const string captcha_property = "Captcha";
        protected const string value_property = "Value";
        protected const string letter_type_property = "LetterType";
        protected const string customer_id_property = "CustomerID";
        protected const string group_id_property = "GroupID";

        public int Id { get; set; }

        public abstract ControlType Type { get; }

        public int Order { get; set; }

        public int? FieldId { get; set; }

        public int Grid { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<ControlContext> Usages { get; set; }

        public bool Default { get; set; }

        public bool IsStandard 
        {
            get 
            {
                return Enum.IsDefined(typeof(StandardControlType), (int)Type);
            }
        }

        public virtual void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            Id = control.Control_ID;
            Order = control.Order;
            if (control.FieldID.HasValue)
            {
                FieldId = control.FieldID.Value;
            }
        }

        internal void SetUsage(bool hasRules, bool hasNotifications, bool hasOutput, bool hasNotificationTemplates)
        {
            List<ControlContext> lst = new List<ControlContext>();
            if (hasRules)
            {
                lst.Add(ControlContext.Rules);
            }
            if (hasNotifications)
            {
                lst.Add(ControlContext.Notifications);
            }
            if (hasOutput)
            {
                lst.Add(ControlContext.Output);
            }
            if (hasNotificationTemplates)
            {
                lst.Add(ControlContext.NotificationTemplates);
            }

            Usages = lst;
        }
    }
}
