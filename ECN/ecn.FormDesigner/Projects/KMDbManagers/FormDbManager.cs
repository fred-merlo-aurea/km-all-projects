using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class FormDbManager : DbManagerBase
    {
        private readonly Expression<Func<Form, bool>> FormIsActive = x => ((x.Active == (int)FormActive.UseActivationDates && x.ActivationDateFrom <= DateTime.Now && x.ActivationDateTo > DateTime.Now) || x.Active == (int)FormActive.Active);
        public const string SubmitButtonDefaultText = "Submit";
        
        public IEnumerable<Form> GetAll(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms =  KM.Forms.ToList();
            return forms.Where(x => cList.Any(y => y.CustomerID == x.CustomerID)).ToList();
        }

        public void Add(Form form)
        {
            if (form.SubmitButtonText == null)
            {
                form.SubmitButtonText = SubmitButtonDefaultText;
            }
            KM.Forms.Add(form);
        }

        public Form GetByID(int ChannelID, int id)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            Form forms =  KM.Forms.SingleOrDefault(x => x.Form_Seq_ID == id);
            if (forms != null)
                return cList.Any(x => x.CustomerID == forms.CustomerID) ? forms : null;
            else
                return null;
        }

        public Form GetByID_ChannelID(int ChannelID, int id)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            Form forms = KM.Forms.SingleOrDefault(x => cList.Any(y => y.CustomerID == x.CustomerID) && x.Form_Seq_ID == id);
            return cList.Any(x => x.CustomerID == forms.CustomerID) ? forms : null;
        }
        
        public Form GetByTokenUID(string tokenuid)
        {
            Form res = GetParentByTokenUID(tokenuid);
            if (res == null)
            {
                res = GetChildByTokenUID(tokenuid);
            }

            return res;
        }

        public Form GetChildByTokenUID(string tokenuid)
        {
            Form res = null;
            try
            {
                res = KM.Forms.SingleOrDefault(x => x.TokenUID == new Guid(tokenuid) && x.Status == FormStatus.Saved.ToString());
            }
            catch
            { }

            return res;
        }

        public Form GetParentByTokenUID(string tokenuid)
        {
            Form res = null;
            try
            {
                res = KM.Forms.SingleOrDefault(x => x.TokenUID == new Guid(tokenuid) && x.Status != FormStatus.Saved.ToString());
            }
            catch
            { }

            return res;
        }

        public IEnumerable<Form> GetActive(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms = KM.Forms.Include(x => x.Form1).Where(x => cList.Any(y => y.CustomerID == x.CustomerID)).Where(FormIsActive)
                        .Where(x => x.Form2 == null).OrderByDescending(x => x.Form_Seq_ID).ToList();
            return GetFormsIEnum(forms, cList);
        }

        public IEnumerable<Form> GetActiveByChannel(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms =  KM.Forms.Include(x => x.Form1).Where(FormIsActive)
                        .Where(x => x.Form2 == null).OrderByDescending(x => x.Form_Seq_ID).ToList();
            return GetFormsIEnum(forms, cList);
        }


        public IEnumerable<Form> GetInactive(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms = KM.Forms.Include(x => x.Form1).Where(x => cList.Any(y => y.CustomerID == x.CustomerID )).Where(Not(FormIsActive))
                        .Where(x => x.Form2 == null).OrderByDescending(x => x.Form_Seq_ID).ToList();
            return GetFormsIEnum(forms, cList);
        }

        public IEnumerable<Form> GetInactiveByChannel(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms =  KM.Forms.Include(x => x.Form1).Where(Not(FormIsActive))
                        .Where(x => x.Form2 == null).OrderByDescending(x => x.Form_Seq_ID).ToList();
            return GetFormsIEnum(forms, cList);
            
        }

        private IEnumerable<Form> GetFormsIEnum(List<Form> forms, List<ECN_Framework_Entities.Accounts.Customer> cList)
        {
            var formsIENUM = from f in forms
                             where cList.Any(x => x.CustomerID == f.CustomerID)
                             select new Form
                             {
                                 ActivationDateFrom = f.ActivationDateFrom,
                                 ActivationDateTo = f.ActivationDateTo,
                                 Active = f.Active,
                                 Controls = f.Controls,
                                 CssFile = f.CssFile,
                                 CssFile_Seq_ID = f.CssFile_Seq_ID,
                                 CssUri = f.CssUri,
                                 CustomerAccessKey = f.CustomerAccessKey,
                                 CustomerID = f.CustomerID,
                                 CustomerName = cList.First(x => x.CustomerID == f.CustomerID).CustomerName,
                                 FooterHTML = f.FooterHTML,
                                 Form_Seq_ID = f.Form_Seq_ID,
                                 Form1 = f.Form1,
                                 Form2 = f.Form2,
                                 FormResults = f.FormResults,
                                 FormStatistics = f.FormStatistics,
                                 FormType = f.FormType,
                                 GroupID = f.GroupID,
                                 HeaderHTML = f.HeaderHTML,
                                 LastPublished = f.LastPublished,
                                 LastUpdated = f.LastUpdated,
                                 Name = f.Name,
                                 Notifications = f.Notifications,
                                 OptInType = f.OptInType,
                                 ParentForm_ID = f.ParentForm_ID,
                                 PublishAfter = f.PublishAfter,
                                 Rules = f.Rules,
                                 Status = f.Status,
                                 StylingType = f.StylingType,
                                 SubmitButtonText = f.SubmitButtonText,
                                 SubmitHistories = f.SubmitHistories,
                                 TokenUID = f.TokenUID,
                                 UpdatedBy = f.UpdatedBy,
                                 UserID = f.UserID

                             };
            return formsIENUM;
        }

        public bool CheckNameIsUnique(int ChannelID, string name, int currId, int parentId)
        {
            return !GetAll(ChannelID).Any(x => x.Form_Seq_ID != currId && x.Form_Seq_ID != parentId && x.Name.ToLower() == name.ToLower());
        }

        public void Remove(Form form)
        {
            KM.Database.ExecuteSqlCommand("DELETE Form WHERE Form_Seq_ID = {0}", form.Form_Seq_ID);
            //KM.Forms.Remove(form);
        }

        public string GetCustomerName(int ChannelID, int id) 
        {
            return GetByID(ChannelID, id).CustomerName;
        }


        public IEnumerable<Form> GetMustPublishByChannel(int ChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> cList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ChannelID);
            List<Form> forms =KM.Forms.Where(x => x.Status == FormStatus.Saved.ToString() && x.PublishAfter.HasValue && x.PublishAfter.Value < DateTime.Now).ToList();
            return forms.Where(x => cList.Any(y => y.CustomerID == x.CustomerID)).ToList();
        }
    }
}