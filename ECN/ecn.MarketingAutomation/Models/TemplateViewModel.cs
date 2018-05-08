using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ecn.MarketingAutomation.Models
{
    public class TemplateViewModel : ModelBase
    {
        private const string TempNameRequired = "Automation Template Name is required. Please enter an Automation Template Name.";
        private const string TempNameTooLong = "Automation Template Name is limited to 100 characters"; //Task 37079:Automation Template Name validation and its Validation message
        private const int TempNameMaxLen = 100;//Task 37079:Automation Template Name validation field size limit should be 100 char and allow any special characters

        public string TotalRecordCounts { get; set; }
        public int Id { get; set; }

        [GetFromField("Name")]
        [Required(ErrorMessage = TempNameRequired)]
        [MaxLength(TempNameMaxLen, ErrorMessage = TempNameTooLong)]
        public string Name { get; set; }

        public DiagramType Type { get; set; }

        public string Diagram { get; set; }

        public List<TemplateViewModel> getFileDiagrams()
        {
            string filePath = HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            string jsonDiagrams = System.IO.File.ReadAllText(filePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<TemplateViewModel> templates;
            if (!string.IsNullOrEmpty(jsonDiagrams))
                templates = serializer.Deserialize<List<TemplateViewModel>>(jsonDiagrams);
            else
                templates = new List<TemplateViewModel>();

            return templates;
        }
        public TemplateViewModel getSingleDiagram(int id)
        {
            List<TemplateViewModel> templates = getFileDiagrams();
            TemplateViewModel currentTemplate = templates.FirstOrDefault(d => d.Id == id);
            return currentTemplate;
        }
        public void saveSingleDiagram(TemplateViewModel model)
        {
            List<TemplateViewModel> templates = getFileDiagrams();
            templates.RemoveAll(d => d.Id == model.Id);
            templates.Add(model);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonDiag = serializer.Serialize(templates);
            string filePath = HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            System.IO.File.WriteAllText(filePath, jsonDiag);
        }
    }
}