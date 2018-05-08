using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
{
    public class UADFilter
    {
        #region Properties
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string FilterXML { get; set; }
        public string FilterType { get; set; }
        public BusinessLogic.Enums.ViewType ViewType { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PubID { get; set; }
        public string PubName { get; set; }
        public bool IsDeleted { get; set; }
        public int UpdatedUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? BrandID { get; set; }
        public string BrandName { get; set; }
        public int? FilterCategoryID { get; set; }
        public string FilterCategoryName { get; set; }
        public bool AddtoSalesView { get; set; }
        public int? QuestionCategoryID { get; set; }
        public string QuestionCategoryName { get; set; }
        public string QuestionName { get; set; }
        public string Notes { get; set; }
        public string CreatedUserName { get; set; }
        public bool IsLocked { get; set; }
        #endregion Properties
    }
}
