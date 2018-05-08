using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Collector
{
    [Serializable]
    [DataContract]
    public class Templates
    {
        public Templates()
        {
            TemplateID = -1;
            CustomerID = -1;    
            TemplateName = string.Empty;        
            TemplateImage = string.Empty;
            IsDefault = false;
            pbgcolor = string.Empty;        
            pAlign = string.Empty;
            pBorder = false;      
            pBordercolor = string.Empty;        
            pfontfamily = string.Empty;        
            pWidth = string.Empty;        
            hImage = string.Empty;        
            hAlign = string.Empty;        
            hMargin = string.Empty;        
            hbgcolor = string.Empty;        
            phbgcolor = string.Empty;        
            phfontsize = string.Empty;        
            phcolor = string.Empty;
            phBold = false;      
            pdbgcolor = string.Empty;        
            pdfontsize = string.Empty;        
            pdcolor = string.Empty;
            pdbold = false;   
            bbgcolor = string.Empty;        
            qcolor = string.Empty;        
            qfontsize = string.Empty;
            qbold = false;
            ShowQuestionNo = false;
            acolor = string.Empty;
            abold = false;   
            afontsize = string.Empty;        
            fImage = string.Empty;        
            fAlign = string.Empty;        
            fMargin = string.Empty;        
            fbgcolor = string.Empty;
            IsActive = false;
            CreatedDate = null;
            CreatedUserID = -1;
            UpdatedDate = null;
            UpdatedUserID = -1; 
        }

        [DataMember]
        public int TemplateID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string TemplateImage { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public string pbgcolor { get; set; }
        [DataMember]
        public string pAlign { get; set; }
        [DataMember]
        public bool pBorder { get; set; }
        [DataMember]
        public string pBordercolor { get; set; }
        [DataMember]
        public string pfontfamily { get; set; }
        [DataMember]
        public string pWidth { get; set; }
        [DataMember]
        public string hImage { get; set; }
        [DataMember]
        public string hAlign { get; set; }
        [DataMember]
        public string hMargin { get; set; }
        [DataMember]
        public string hbgcolor { get; set; }
        [DataMember]
        public string phbgcolor { get; set; }
        [DataMember]
        public string phfontsize { get; set; }
        [DataMember]
        public string phcolor { get; set; }
        [DataMember]
        public bool phBold { get; set; }
        [DataMember]
        public string pdbgcolor { get; set; }
        [DataMember]
        public string pdfontsize { get; set; }
        [DataMember]
        public string pdcolor { get; set; }
        [DataMember]
        public bool pdbold { get; set; }
        [DataMember]
        public string bbgcolor { get; set; }
        [DataMember]
        public string qcolor { get; set; }
        [DataMember]
        public string qfontsize { get; set; }
        [DataMember]
        public bool qbold { get; set; }
        [DataMember]
        public bool ShowQuestionNo { get; set; }
        [DataMember]
        public string acolor { get; set; }
        [DataMember]
        public bool abold { get; set; }
        [DataMember]
        public string afontsize { get; set; }
        [DataMember]
        public string fImage { get; set; }
        [DataMember]
        public string fAlign { get; set; }
        [DataMember]
        public string fMargin { get; set; }
        [DataMember]
        public string fbgcolor { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
    }
}
