using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using KMEnums;

using KMEntities;

namespace KMModels.PostModels
{
    public class FormOutputPostModel: PostModelBase
    {        
        private const string TooLong = "External Post URL is too long";              
        private const int UrlMaxLen = 200;
        private const string NoExternalURL = "External Post URL Required";

        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        [GetFromField("FormResult_Seq_ID")]
        public  int ResultId { get; set; }

        [GetFromField("URL")]     
        [Required(ErrorMessage = NoExternalURL)]
        [MaxLength(UrlMaxLen, ErrorMessage = TooLong)]
        public string ExternalUrl { get; set; }

        public IEnumerable<ThirdPartyQueryValueModel> ThirdPartyQueryValue { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);
            ThirdPartyQueryValue = ((FormResult)entity).ThirdPartyQueryValues.ConvertToModels<ThirdPartyQueryValueModel>();
        }
    }

    public class ThirdPartyQueryValueModel : ModelBase 
    {
        protected const string Required = "Querystring Parameter Name Required";
        private const int NameMaxLen = 30;
        private const string TooLong = "Name is too long";
        private const string RegexError = "Invalid QueryString Name. Only allowed a-z, A-Z, 0-9, _ and . characters."; 
       

        [Required(ErrorMessage = Required)]
        [MaxLength(NameMaxLen, ErrorMessage = TooLong)]
        [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = RegexError)]
        public string Name { get; set; }

        [GetFromField("Control_ID")]
        public int  Value { get; set; }        
    }
  
}
