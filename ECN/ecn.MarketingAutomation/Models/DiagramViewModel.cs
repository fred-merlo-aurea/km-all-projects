using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

namespace ecn.MarketingAutomation.Models
{
    public class DiagramViewModel : ModelBase
    {
        [GetFromField("Diagram_Seq_ID")]
        public int Id { get; set; }

        public string Name { get; set; }

        [GetFromField("DiagramType")]
        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType Type { get; set; }

        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [GetFromField("TokenUID")]
        public Guid TokenUID { get; set; }

        public string Diagram { get; set; }

        public string Goal { get; set; }


        public List<DiagramViewModel> getFileDiagrams()
        {
            string filePath = HttpContext.Current.Server.MapPath("/LocalData") + "/MockupDiagrams.txt";
            string jsonDiagrams = System.IO.File.ReadAllText(filePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DiagramViewModel> ActiveDiagrams;
            if (!string.IsNullOrEmpty(jsonDiagrams))
                ActiveDiagrams = serializer.Deserialize<List<DiagramViewModel>>(jsonDiagrams);
            else
                ActiveDiagrams = new List<DiagramViewModel>();

            return ActiveDiagrams;
        }
        public DiagramViewModel getSingleDiagram(int id)
        {
            List<DiagramViewModel> ActiveDiagrams = getFileDiagrams();
            DiagramViewModel currentAutomation = ActiveDiagrams.FirstOrDefault(d => d.Id == id);
            currentAutomation.StartDate = currentAutomation.StartDate.Value.AddHours(-5);
            currentAutomation.EndDate = currentAutomation.EndDate.Value.AddHours(-5);
            return currentAutomation;
        }
        public void saveSingleDiagram(DiagramViewModel model)
        {
            List<DiagramViewModel> ActiveDiagrams = getFileDiagrams();
            ActiveDiagrams.RemoveAll(d => d.Id == model.Id);
            ActiveDiagrams.Add(model);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonDiag = serializer.Serialize(ActiveDiagrams);
            string filePath = HttpContext.Current.Server.MapPath("/LocalData") + "/MockupDiagrams.txt";
            System.IO.File.WriteAllText(filePath, jsonDiag);
        }
    }

    public enum DiagramType
    {
        [Display(Name = "Simple")]
        Simple = 0
    }

    public enum DiagramStatus
    {
        Saved = 0,
        Active = 1,
        Paused = 2,
        Completed = 3,
        Archived = 4
    }

    public enum DiagramFromTemplate
    {
        No,
        Yes        
    }
}