using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMModels;
using KMModels.PostModels;

namespace KMManagers
{
    public class FormStatisticManager:  ManagerBase
    {
        private FormStatisticDbManager FSM
        {
            get
            {
                return DB.FormStaticsticDbManager;
            }
        }
        public FormStatistic Create(Form form, int totalPages,string email) 
        {
            FormStatisticDbManager fsm = new FormStatisticDbManager();
            FormStatistic fs = new FormStatistic();
            fs.StartForm = DateTime.Now;
            fs.Form_Seq_ID = form.Form_Seq_ID;
            fs.TotalPages = totalPages;
            fs.Email = email;
            fs.IsSubmitted = false;
            using (TransactionScope t = new TransactionScope())
            {
                fsm.Add(fs);
                fsm.SaveChanges();
                t.Complete();                
            }            
            return fs;
        }
        public IEnumerable<TModel> GetAll<TModel>(int id, string sorting, bool asc, int pageNumber, int pageSize) where TModel : ModelBase, new()
        {
            return FSM.GetByFormID(id, sorting, asc, pageNumber, pageSize).ConvertToModels<TModel>();
        }

        public int GetKnowCount(int id) 
        {
            return FSM.GetKnownCount(id);
        }

        public int GetCount(int id)
        {
            return FSM.GetCount(id);
        }

        public FormStatistic UpdateEmailForm(long FormStaticticID, string email)
        {
            FormStatisticDbManager fsm = new FormStatisticDbManager();

            FormStatistic FormStatistic = fsm.GetByID(FormStaticticID);

            FormStatistic.Email = email;

            using (TransactionScope t = new TransactionScope())
            {
                fsm.SaveChanges();
                t.Complete();
            }
            return FormStatistic;
        }

        public FormStatistic UpdateTotalPages(long FormStaticticID, int totalPages) 
        {
            FormStatisticDbManager fsm = new FormStatisticDbManager();
            FormStatistic FormStatistic = fsm.GetByID(FormStaticticID);
            if (totalPages > FormStatistic.TotalPages)
            {
                FormStatistic.TotalPages = totalPages;
                using (TransactionScope t = new TransactionScope())
                {
                    fsm.SaveChanges();
                    t.Complete();
                }
            }
            return FormStatistic;
        } 

        public FormStatistic SubmitForm(long FormStaticticID, int numberPage, string email)
        {
            
            FormStatisticDbManager fsm = new FormStatisticDbManager() ;
            
            FormStatistic FormStatistic = fsm.GetByID(FormStaticticID);
            
            FormStatistic.Email = email;
            FormStatistic.FinishForm = DateTime.Now;
            FormStatistic.TotalPages = numberPage;
            FormStatistic.IsSubmitted = true;
          
            new FormStatisticLogManager().UpdateFinishTime(FormStaticticID, numberPage);

            using (TransactionScope t = new TransactionScope())
            {
                fsm.SaveChanges();
                t.Complete();
            }
            return FormStatistic;
        }
        public FormStatistic UnloadFormStatistic(long FormStaticticID, int numberPage)
        {
            FormStatisticDbManager fsm = new FormStatisticDbManager();
            FormStatistic FormStatistic = fsm.GetByID(FormStaticticID);
            FormStatistic.FinishForm = DateTime.Now;
            new FormStatisticLogManager().UpdateFinishTime(FormStaticticID, numberPage);

            using (TransactionScope t = new TransactionScope())
            {
                fsm.SaveChanges();
                t.Complete();
            }
            return FormStatistic;
        }
      }
    
}
