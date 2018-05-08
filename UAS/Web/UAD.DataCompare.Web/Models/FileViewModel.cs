using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
    
namespace UAD.DataCompare.Web.Models
{
    public class FileViewModel
    {
        public FileViewModel()
        {
            FileName = "";
            NotificationEmail = "";
            Delimiter = "";
            Quotation = "";
            DataFile = null;
            ColumnMapping = new List<ColumnMapper>();
            NotificationMessages = new Dictionary<string, string>();
        }

       

        [Required]
        [Display(Name = "Save Filename As:")]
        [MaxLength(50, ErrorMessage ="File name cannot exceed 50 characters.")]
        public string  FileName { get; set; }

        [Required]
        [Display(Name = "Notification Email Address:")]
         public string NotificationEmail { get; set; }

        [Required]
        [Display(Name = "File Delimiter:")]
        public string Delimiter { get; set; }

        [Required]
        [Display(Name = "Does this contain double quotation mark:")]
        public string Quotation { get; set; }


        [Required]
        [Display(Name = "Select a File to Map:")]
        public HttpPostedFileBase DataFile { get; set; }

        public string fileFullPath { get; set; }


        public List<ColumnMapper> ColumnMapping { get; set; }

        public  Dictionary<string,string> NotificationMessages { get; set; }




    }
}