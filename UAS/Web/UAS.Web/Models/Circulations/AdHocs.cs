using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class AdHocs
    {
        private string _adHocField;
        private string _value;
        public string AdHocField
        {
            get { return _adHocField; }
            set
            {
                _adHocField = value;
            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;

            }

        }
        public AdHocs()
        {

        }
        public AdHocs(string field, string value)
        {
            this.AdHocField = field;
            this.Value = value;
        }
        public FrameworkUAD.Object.PubSubscriptionAdHoc GetModel()
        {
            FrameworkUAD.Object.PubSubscriptionAdHoc mod = new FrameworkUAD.Object.PubSubscriptionAdHoc(this.AdHocField, this.Value);
            return mod;
        }
    }

}