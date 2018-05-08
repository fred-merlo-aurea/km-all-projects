using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Menu
    {
        public Menu() 
        {
            MenuID = 0;
            ApplicationID = 0;
            IsServiceFeature = false;
            ServiceFeatureID = 0;
            MenuName = string.Empty;
            Description = string.Empty;
            IsParent = false;
            ParentMenuID = 0;
            URL = string.Empty;
            IsActive = false;
            MenuOrder = 0;
            HasFeatures = false;
            ImagePath = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ServiceID = 0;
            //MenuFeatures = new List<MenuFeature>();
            ChildItems = new List<Menu>();
        }
        public Menu(string name, string url, bool isParent, int appID)
        {
            MenuID = 0;
            ApplicationID = appID;
            IsServiceFeature = false;
            ServiceFeatureID = 0;
            MenuName = name.ToUpper();
            Description = string.Empty;
            IsParent = isParent;
            ParentMenuID = 0;
            URL = url;
            IsActive = true;
            MenuOrder = 0;
            HasFeatures = false;
            ImagePath = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ServiceID = 0;
            //MenuFeatures = new List<MenuFeature>();
            ChildItems = new List<Menu>();
        }
        #region Properties
        [DataMember]
        public int MenuID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public bool IsServiceFeature { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsParent { get; set; }
        [DataMember]
        public int ParentMenuID { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int MenuOrder { get; set; }
        [DataMember]
        public bool HasFeatures { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        #endregion

        //[DataMember]
        //public List<MenuFeature> MenuFeatures { get; set; }
        public List<Menu> ChildItems { get; set; }
    }
}
