using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class QuestionCategory
    {
        public QuestionCategory() 
        {
            QuestionCategoryID = 0;
            CategoryName = string.Empty;
            CreatedUserID = 0;
            CreatedDate = DateTime.Now;
            UpdatedUserID = 0;
            UpdatedDate = DateTime.Now;
            IsDeleted = false;
            
        }
        #region Properties
        [DataMember]
        public int QuestionCategoryID { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion
    }
}
