using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ControlDbManager : DbManagerBase
    {
        private static List<int> NotValuedTypes = new List<int>();
        private static List<int> NotVisibleTypes = new List<int>();
        private static List<int> VisibleOverwriteDataValueType = new List<int>();
        private static List<int> NotRequestQueryVisibleType = new List<int>();
        static ControlDbManager()
        {
            NotValuedTypes.AddRange(new[] { 
                                            (int)KMEnums.ControlType.PageBreak,
                                            (int)KMEnums.ControlType.Literal,
                                            (int)KMEnums.ControlType.Grid,
                                            (int)KMEnums.ControlType.Captcha
            });
            NotVisibleTypes.AddRange(new[] {
                                            (int)KMEnums.ControlType.PageBreak,
                                            (int)KMEnums.ControlType.Hidden,
                                            (int)KMEnums.ControlType.Email,
                                            (int)KMEnums.ControlType.Captcha
            });
            NotRequestQueryVisibleType.AddRange(new[] {
                                            (int)KMEnums.ControlType.PageBreak,
                                            (int)KMEnums.ControlType.Literal,
                                            (int)KMEnums.ControlType.Captcha,
                                            (int)KMEnums.ControlType.NewsLetter
            });
            VisibleOverwriteDataValueType.AddRange(new[] {
                                            (int)KMEnums.ControlType.TextBox,
                                            (int)KMEnums.ControlType.TextArea,
                                            (int)KMEnums.ControlType.RadioButton,
                                            (int)KMEnums.ControlType.CheckBox,
                                            (int)KMEnums.ControlType.DropDown,
                                            (int)KMEnums.ControlType.ListBox,
                                            (int)KMEnums.ControlType.Grid,
                                            (int)KMEnums.ControlType.Hidden
            });
        }

        public Control GetByID(int id)
        {
            return KM.Controls.Single(x => x.Control_ID == id);
        }

        public IEnumerable<Control> GetAllByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId).OrderBy(x => x.Order).ToList();
        }

        public IEnumerable<Control> GetAllStandardByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && x.ControlType.MainType_ID.HasValue).OrderBy(x => x.Order).ToList();
        }

        public IEnumerable<Control> GetAllCustomWithFieldByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && x.FieldID.HasValue).OrderBy(x => x.Order).ToList();
        }

        public bool HasCustomWithField(int formId)
        {
            return KM.Controls.Any(x => x.Form_Seq_ID == formId && x.FieldID.HasValue);
        }

        public IEnumerable<Control> GetAllValuedByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && !NotValuedTypes.Contains(x.Type_Seq_ID)).OrderBy(x => x.Order).ToList();
        }
        
        public IEnumerable<Control> GetAllVisibleByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && !NotVisibleTypes.Contains(x.Type_Seq_ID)).OrderBy(x => x.Order).ToList();
        }

        public IEnumerable<Control> GetAllVisibleOverwriteDataFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && VisibleOverwriteDataValueType.Contains(x.Type_Seq_ID)).OrderBy(x => x.Order).ToList();
        }
        public IEnumerable<Control> GetAllRequestQueryByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && !NotRequestQueryVisibleType.Contains(x.Type_Seq_ID)).OrderBy(x => x.Order).ToList();
        }
        public IEnumerable<Control> GetPageBreaksByFormID(int formId)
        {
            return KM.Controls.Where(x => x.Form_Seq_ID == formId && x.Type_Seq_ID == (int)KMEnums.ControlType.PageBreak).OrderBy(x => x.Order).ToList();
        }

        public void Add(Control nc)
        {
            KM.Controls.Add(nc);
        }

        public void Remove(Control c)
        {
            KM.Controls.Remove(KM.Controls.Single(x => x.Control_ID == c.Control_ID));
        }
    }
}