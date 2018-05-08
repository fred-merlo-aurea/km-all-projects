using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class FormStatisticLogDbManager : DbManagerBase
    {
        public FormStatisticLog GetByID(long id)
        {
            return KM.FormStatisticLogs.SingleOrDefault(x => x.FormStatistic_Seq_ID == id);
        }
        public IEnumerable<FormStatisticLog> GetByFormStatisticID(long id)
        {
            FormStatistic FormStatistic = new FormStatisticDbManager().GetByID(id);
            return KM.FormStatisticLogs.Where(x => x.FormStatistic_Seq_ID == FormStatistic.FormStatistic_Seq_ID);
        }
        public FormStatisticLog GetByFormStaticstic_IDAndNumber(long id, int number)
        {
            return KM.FormStatisticLogs.Where(x => (x.FormStatistic_Seq_ID == id && x.PageNumber == number)).OrderByDescending(x => x.StartPage).FirstOrDefault();
        }
        public void Add(FormStatisticLog newFC)
        {
            KM.FormStatisticLogs.Add(newFC);
        }
    }
}
