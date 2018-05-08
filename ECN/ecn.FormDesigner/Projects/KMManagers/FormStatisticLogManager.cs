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
    public class FormStatisticLogManager : ManagerBase
    {
        private FormStatisticDbManager FSM
        {
            get
            {
                return DB.FormStaticsticDbManager;
            }
        }

        public IEnumerable<TModel> GetAll<TModel>(long id) where TModel : ModelBase, new()
        {
            FormStatisticLogDbManager fsm = new FormStatisticLogDbManager();
            return fsm.GetByFormStatisticID(id).ConvertToModels<TModel>();
        }

        public FormStatisticLog Create(long FormStatistic_ID, int numPage)
        {
            FormStatisticLogDbManager fsm = new FormStatisticLogDbManager();
            FormStatisticLog fs = new FormStatisticLog();
            fs.StartPage = DateTime.Now;
            fs.FormStatistic_Seq_ID = FormStatistic_ID;
            fs.PageNumber = numPage;
            using (TransactionScope t = new TransactionScope())
            {
                fsm.Add(fs);
                fsm.SaveChanges();
                t.Complete();
            }
            return fs;
        }

        public FormStatisticLog UpdateFinishTime(long FormStatistic_ID, int numPage)
        {
            FormStatisticLogDbManager fsm = new FormStatisticLogDbManager();
            FormStatisticLog fs = fsm.GetByFormStaticstic_IDAndNumber(FormStatistic_ID, numPage);
            if (fs != null)
            {
                fs.FinishPage = DateTime.Now;
                using (TransactionScope t = new TransactionScope())
                {
                    fsm.SaveChanges();
                    t.Complete();
                }                
                return fs;
            }
            else 
                return null;
        }
       
    }
}
