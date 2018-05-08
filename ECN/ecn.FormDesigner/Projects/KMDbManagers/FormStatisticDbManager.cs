using System;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Data.Entity;
using KMEntities;
using KMEnums;
using System.Data.SqlClient;

namespace KMDbManagers
{
    public class FormStatisticDbManager : DbManagerBase
    {
        public FormStatistic GetByID(long id)
        {
            return KM.FormStatistics.SingleOrDefault(x => x.FormStatistic_Seq_ID == id);
        }


        public int GetKnownCount(int id)
        {
            return KM.FormStatistics.Where(x => x.Form_Seq_ID == id).Where (x=>x.Email!=null ).Count();
        }

        public int GetCount(int id)
        {
            return KM.FormStatistics.Where(x => x.Form_Seq_ID == id).Count();
        }

        public IEnumerable<FormStatistic> GetByFormID(int id, string sorting, bool asc, int pageNumber, int pageSize)
        {
            Func<FormStatistic, Object> sortOrder = null;
            if (sorting == KMEnums.ReportParams.Email.ToString())
                sortOrder = x => x.Email;
            else if (sorting == KMEnums.ReportParams.FinishForm.ToString())
                sortOrder = x => x.FinishForm;
            else if (sorting == KMEnums.ReportParams.TotalPages.ToString())
                sortOrder = x => x.TotalPages;    
            else 
                sortOrder = x => x.StartForm;

            if (asc)
            {
                return KM.FormStatistics.Where(x => x.Form_Seq_ID == id).OrderBy(sortOrder).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else 
            {
                return KM.FormStatistics.Where(x => x.Form_Seq_ID == id).OrderByDescending(sortOrder).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            
        }   
        
        public void Add(FormStatistic newFC)
        {
            KM.FormStatistics.Add(newFC);
        }

        public void Update(int id, int newId)
        {
            KM.Form_UpdateStaticstic(id, newId);
            //KM.FormStatistics.Where(x => x.Form_Seq_ID == id).ToList().ForEach(delegate(FormStatistic fs)
            //{
            //    fs.Form_Seq_ID = newId;
            //});
        }
    }
}