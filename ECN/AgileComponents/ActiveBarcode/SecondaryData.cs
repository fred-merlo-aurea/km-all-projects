using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ActiveUp.WebControls
{
    [
        TypeConverterAttribute(typeof(ExpandableObjectConverter)),
        Description("Secondary data for HIBC mode."),
        Serializable
    ]
    public class SecondaryData
    {
        private string _date;
        private string _quantity;
        private string _serial;
        private SecondaryEncodingMode _secondaryEncoding;
        private HibcDateFormat _dateFormat;

        public SecondaryData()
        {
            _date = string.Empty;
            _quantity = "0";
            _serial = string.Empty;
            _secondaryEncoding = SecondaryEncodingMode.None;
            _dateFormat = ActiveUp.WebControls.HibcDateFormat.YearMonthDay;
        }

        [
            NotifyParentPropertyAttribute(true)
        ]
        public string Date
        { 
            get
            {
                return _date;
            }

            set
            {
                _date = value;
            }
        }

        [
            NotifyParentPropertyAttribute(true),
            DefaultValue(0)
        ]
        public string Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                _quantity = value;
            }
        }

        [
             NotifyParentPropertyAttribute(true)
        ]
        public string Serial
        {
            get
            {
                return _serial;
            }

            set
            {
                _serial = value;
            }
        }

        [
            NotifyParentPropertyAttribute(true),
            DefaultValue(typeof(SecondaryEncodingMode),"None")
        ]
        public SecondaryEncodingMode SecondaryEncoding
        {
            get
            {
                return _secondaryEncoding;
            }

            set
            {
                _secondaryEncoding = value;
            }
        }

        [
            NotifyParentPropertyAttribute(true),
            DefaultValue(typeof(HibcDateFormat),"YearMonthDay")
        ]
        public HibcDateFormat DateFormat
        {
            get
            {
                return _dateFormat;
            }

            set
            {
                _dateFormat = value;
            }
        }
    }
}
