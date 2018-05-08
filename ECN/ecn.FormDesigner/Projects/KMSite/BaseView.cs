using KMEnums;
using KMModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using DataType = KMEnums.DataType;

namespace KMSite
{
    public class BaseView<T> : WebViewPage<T>
    {
        public const int PageSize = 10;

        public const string DateFormat = "{0:MM/dd/yyyy}";
        public const string DateTimeFormat = "{0:MM/dd/yyyy hh:mm}";

        public const string JavaScriptDateFormat = "MM/dd/yyyy";
        public const string JavaScriptDateTimeFormat = "MM/dd/yyyy hh:mm";

        #region Temporary Code

        public readonly Array Fonts = new[] { "Arial", "Calibri", "Courier New", "Georgia", "Times New Roman", "Tahoma", "Verdana" };

        public readonly Array Sizes = new int[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };

        #endregion Temporary Code

        public string GetName<TModel>(Expression<Func<TModel, object>> property) 
        {
            return GetPropertyName<TModel>(property);
        }

        public string GetName<TModel>(string prefix, Expression<Func<TModel, object>> property)
        {
            return string.Format("{0}.{1}", prefix, GetPropertyName<TModel>(property));
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            var body = property.Body as MemberExpression;
            if (body == null)
            {
                body = ((UnaryExpression)property.Body).Operand as MemberExpression;
            }
            return body.Member.Name;
        }

        public string FormatID(string s)
        {
            return s.Replace("]", "_").Replace("[", "_").Replace(".", "_");
        }

        public string Config(string key) 
        {
            if (HttpContext.Current.Request.IsSecureConnection)
                return ConfigurationManager.AppSettings.Get(key).Replace("http://", "https://");
            return ConfigurationManager.AppSettings.Get(key);
        }

        public Array GetComparisonTypeValues(DataType dataType) 
        {
            return Enum.GetValues(GetComparisonType(dataType));
        }

        public string[] GetComparisonTypeNames(DataType dataType) 
        {
            return Enum.GetNames(GetComparisonType(dataType));
        }

        private Type GetComparisonType(DataType dataType) 
        {
            Type type = null;
            switch (dataType)
            {
                case DataType.Selection: type = typeof(SelectionComparisonType); break;
                case DataType.Text: type = typeof(TextComparisonType); break;
                case DataType.Date: type = typeof(DateComparisonType); break;
                case DataType.Number: type = typeof(NumberComparisonType); break;
                case DataType.Decimal: type = typeof(NumberComparisonType); break;
                case DataType.Newsletter: type = typeof(NewsLetterComparisonType); break;
            }
            return type;
        }

        public string GetControlSnippet(ControlModel control) 
        {
            return string.Format("%%<span cid='{1}'>{0}</span>%%", control.FieldLabel, control.Id);
        }

        public string GetDisplayName(Enum enumValue)
        {
            var attribute = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttributes(typeof(DisplayAttribute), false)
                            .FirstOrDefault();

            return attribute != null ? ((DisplayAttribute)attribute).GetName() : enumValue.ToString();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}