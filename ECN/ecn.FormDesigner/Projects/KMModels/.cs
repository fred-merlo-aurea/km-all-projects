using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KMEntities;
using KMEnums;
using System.ComponentModel.DataAnnotations;


namespace KMModels
{
    public class RequestQueryValuePostModel : ModelBase
    {
        protected const string Required = "Required";
        private const int NameMaxLen = 30;
        private const string TooLong = "Name is too long";
        private const string RegexError = "Invalid QueryString Name. Only allowed a-z, A-Z, 0-9 and _ characters.";


        [Required(ErrorMessage = Required)]
        [MaxLength(NameMaxLen, ErrorMessage = TooLong)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = RegexError)]
        public string Name { get; set; }

        [GetFromField("Control_ID")]
        public int Value { get; set; }
        public bool IsDeleted { get; set; }
    }
}
