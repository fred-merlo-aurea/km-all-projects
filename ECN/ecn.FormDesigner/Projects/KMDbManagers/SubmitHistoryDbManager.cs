using System;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class SubmitHistoryDbManager : DbManagerBase
    {
        public void Add(SubmitHistory history)
        {
            KM.SubmitHistories.Add(history);
        }

        public void AddData(SubmitData data)
        {
            KM.SubmitDatas.Add(data);
        }

        public SubmitHistory GetByToken(Guid token)
        {
            return KM.SubmitHistories.SingleOrDefault(x => x.HistoryToken == token);
        }

        public void Delete(SubmitHistory history)
        {
            KM.SubmitHistories.Remove(history);
        }
    }
}