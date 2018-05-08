using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class Country
    {
        public  List<Entity.Country> Select()
        {
            List<Entity.Country> retList = null;
            retList = DataAccess.Country.Select();

            return retList;
        }

        public bool CountryRegionCleanse(int sourceFileID, string processCode,  KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.Country.CountryRegionCleanse(sourceFileID, processCode,client);
                scope.Complete();
                done = true;
            }
            return done;
        }

        public static IEnumerable<SelectListItem> GetCountriesforSelectList()
        {
            return DataAccess.Country.Select().OrderBy(x => x.SortOrder).Select(i => new SelectListItem()
            {
                Text = i.ShortName,
                Value = i.CountryID.ToString()
            });
        }
    }
}
