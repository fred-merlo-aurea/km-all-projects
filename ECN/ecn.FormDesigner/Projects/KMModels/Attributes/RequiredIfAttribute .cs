using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private string ErrorMessage { get; set; }
        private object Value { get; set; }

        public RequiredIfAttribute(string propertyName, object value, string errormessage)
        {
            this.PropertyName = propertyName;
            this.Value = value;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var proprtyValue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyValue.ToString() == Value.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
