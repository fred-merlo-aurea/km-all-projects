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
    public class FormStatisticLoader
    {
        public string GetFormNameByUid(string formUid)
        {
            FormDbManager fm = new FormDbManager();
            Form form = fm.GetByTokenUID(formUid);
            return form.Name;
        }

        public int GetKnowVisitor(string formUid)
        {
            FormStatisticManager fsm = new FormStatisticManager();
            FormDbManager fm = new FormDbManager();
            Form form = fm.GetByTokenUID(formUid);
            if (form != null)
            {
                return fsm.GetKnowCount(form.Form_Seq_ID);
            }
            else
            {
                return 0;
            }
        }
        

        public int GetStatisticCount(string formUid) 
        {
            FormStatisticManager fsm = new FormStatisticManager();
            FormDbManager fm = new FormDbManager();
            Form form = fm.GetByTokenUID(formUid);
            if (form != null)
                return fsm.GetCount(form.Form_Seq_ID);
            else
            {  
                return 0;
            }
        }

        public IEnumerable<FormStatisticModel> GetStatistic(string formUid, string sorting, bool asc, int pageNumber, int pageSize) 
        {
            FormStatisticManager fsm = new FormStatisticManager();
            FormDbManager fm = new FormDbManager();
            Form form = fm.GetByTokenUID(formUid);
            if (form != null)
                return fsm.GetAll<FormStatisticModel>(form.Form_Seq_ID, sorting, asc, pageNumber, pageSize);
            else
            {
                var tmp = new FormStatisticModel[] {};                
                return tmp.AsEnumerable<FormStatisticModel>();                
            }
        }

        public IEnumerable<FormStatisticLogModel> GetStatisticLog(string id)
        {
            FormStatisticLogManager fsm = new FormStatisticLogManager();
            IEnumerable<FormStatisticLogModel> result = fsm.GetAll<FormStatisticLogModel>(long.Parse(id));
            return result;
        }

        public string CreateStatistic(string formUid, int totalPages, string email) 
        {
            FormStatisticManager fsm = new FormStatisticManager();
            FormStatisticLogManager fsmlog = new FormStatisticLogManager();
            FormDbManager fm = new FormDbManager();
            Form form = fm.GetByTokenUID(formUid);
            FormStatistic fs = fsm.Create(form, totalPages, email);
            FormStatisticLog fsl = fsmlog.Create(fs.FormStatistic_Seq_ID, totalPages);
            return fs.FormStatistic_Seq_ID.ToString();
        }

        public string LogFinish(long FormStatistic_ID, int currentPage) 
        {
            FormStatisticLogManager fslm = new FormStatisticLogManager();
            FormStatisticLog FormStatisticLog = fslm.UpdateFinishTime(FormStatistic_ID, currentPage);
            if (FormStatisticLog == null)
                return "";
            else
                return FormStatisticLog.FormStatistic_Seq_ID.ToString();
        }

        public string LogNewer(long FormStatistic_ID, int currentPage) 
        {
            FormStatisticLogManager fsml = new FormStatisticLogManager();
            FormStatisticLog FormStatisticLog = fsml.Create(FormStatistic_ID, currentPage);
            return FormStatisticLog.FormStatistic_Seq_ID.ToString();
        }

        public string SubmitStatistic(long FormStatistic_ID, int numberPage, string email) 
        {
            FormStatisticManager fsm = new FormStatisticManager();
            var FormStatistic = fsm.SubmitForm(FormStatistic_ID, numberPage, email);
            return FormStatistic.FormStatistic_Seq_ID.ToString();
        }
        public string UnloadFormStatistic(long FormStatistic_ID, int numberPage)
        {
            FormStatisticManager fsm = new FormStatisticManager();
            var FormStatistic = fsm.UnloadFormStatistic(FormStatistic_ID, numberPage);
            return FormStatistic.FormStatistic_Seq_ID.ToString();
        }
        public string UpdateEmail(long FormStatistic_ID, string email)
        {
            FormStatisticManager fsm = new FormStatisticManager();
            var FormStatistic = fsm.UpdateEmailForm(FormStatistic_ID, email);
            return FormStatistic.FormStatistic_Seq_ID.ToString();
        }

        public string UpdateTotalPages(long FormStatistic_ID, int totalPages)
        {
            FormStatisticManager fsm = new FormStatisticManager();
            var FormStatistic = fsm.UpdateTotalPages(FormStatistic_ID, totalPages);
            return FormStatistic.FormStatistic_Seq_ID.ToString();
        }   
    }
}
