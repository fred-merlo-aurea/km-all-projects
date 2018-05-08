using System;
using System.Linq;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class FormResultDbManager : DbManagerBase
    {
        public FormResult GetByID(int id)
        {
            return KM.FormResults.SingleOrDefault(x => x.FormResult_Seq_ID == id);
        }

        public void AddResults(FormResult[] results)
        {
            foreach (var fr in results)
            {
                Add(fr);
            }
        }

        public void Add(FormResult fr)
        {
            KM.FormResults.Add(fr);
        }

        public FormResult GetFormResultByFormIDAndType(int id, FormResultType type)
        {
            return KM.FormResults.Include("ThirdPartyQueryValues").SingleOrDefault(x => x.Form_Seq_ID == id && x.ResultType == (int)type);
        }

        public void DeleteByID(int id)
        {
            KM.FormResults.Remove(GetByID(id));
        }
    }
}