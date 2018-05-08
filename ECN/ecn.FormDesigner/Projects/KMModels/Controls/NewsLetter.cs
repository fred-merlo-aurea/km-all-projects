using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class NewsLetter : HeadedControl
    {
        public bool IsPrepopulateFromDb { get; set; }

        public NewsLetter()
        {
            Categories = new List<ControlCategory>();
            Groups = new List<GroupModel>();
        }

        public NewsLetter(bool setDefaultValues = false) 
        {
            if (setDefaultValues)
            {
                Label = "NewsLetter";
                LabelHTML = "NewsLetter";
                Categories = new List<ControlCategory>();
                Groups = new List<GroupModel>();
            }
        }

        public override ControlType Type
        {
            get { return ControlType.NewsLetter; }
        }

        public bool Subscribe { get; set; }

        public int Columns { get; set; }

        public IEnumerable<ControlCategory> Categories { get; set; }

        public IEnumerable<GroupModel> Groups { get; set; }        

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Subscribe = int.Parse(control.GetFormPropertyValue(letter_type_property, properties)) == 1;
            string columns_value = control.GetFormPropertyValue(numberofcolumns_property, properties);
            if (columns_value != null)
            {
                Columns = int.Parse(columns_value);
            }
            SetPopulationType(control, properties);

            List<GroupModel> GModels = new List<GroupModel>();
            //IEnumerable <KMEntities.NewsletterGroup> NLGroups = control.NewsletterGroups.Where(g => g.Control_ID == control.Control_ID);
            foreach (var nlGroup in control.NewsletterGroups)
            {
                GroupModel group = new GroupModel();
                if (nlGroup.ControlCategory != null)
                {
                    group.Category.CategoryID = nlGroup.ControlCategory.ControlCategoryID;
                    group.Category.CategoryName = nlGroup.ControlCategory.LabelHTML;
                }
                else
                {
                    group.Category.CategoryID = 0;
                    group.Category.CategoryName = " -- Select -- ";
                }
                group.CustomerID = nlGroup.CustomerID;
                group.Default = nlGroup.IsPreSelected;
                group.GroupID = nlGroup.GroupID;
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID);
                group.GroupName = g.GroupName;
                group.LabelHTML = nlGroup.LabelHTML;
                group.Order = nlGroup.Order == null ? 0 : nlGroup.Order.Value;
                foreach (var nlgudf in nlGroup.NewsletterGroupUDFs)
                {                    
                    List<ECN_Framework_Entities.Communicator.GroupDataFields> listFromUdfs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(control.Form.GroupID);
                    ECN_Framework_Entities.Communicator.GroupDataFields formUdf = listFromUdfs.SingleOrDefault(u => u.GroupDataFieldsID == nlgudf.FormGroupDataFieldID);
                    if (formUdf != null)
                    {
                        Udf udf = new Udf();
                        udf.GroupDataFieldsID = formUdf.GroupDataFieldsID;
                        udf.ShortName = formUdf.ShortName;
                        group.UDFs.Add(udf);
                    }
                }
                GModels.Add(group);
            }
            Groups = GModels;

            List<ControlCategory> CCats = new List<ControlCategory>();
            foreach (var cc in control.ControlCategories)
            {
                ControlCategory controlCategory = new ControlCategory();
                controlCategory.CategoryID = cc.ControlCategoryID;
                controlCategory.CategoryName = cc.LabelHTML;
                CCats.Add(controlCategory);
            }
            Categories = CCats;
        }

        protected void SetPopulationType(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            int from = 0;
            string data = control.GetFormPropertyValue(prepopulatefrom_property, properties);
            if (data != null)
            {
                try
                {
                    from = int.Parse(data);
                }
                catch
                { }
            }
            IsPrepopulateFromDb = (KMEnums.PopulationType)from == PopulationType.Database;
        }
    }
}