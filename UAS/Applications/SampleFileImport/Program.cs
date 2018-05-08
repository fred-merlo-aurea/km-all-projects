using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using KM.Common;
using DataFunctions = KM.Common.DataFunctions;

namespace SampleFileImport
{
    class Program
    {
        private const string SqSelectFromTmp = "select * from tmp_SAE_SourceFile_948 order by importrownumber";

        static void Main(string[] args)
        {
            int clientID = 22;
            int sourcefileID = 948;

            int counter = 1;

            Console.WriteLine(string.Format("Start - {0}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.f")));

            List<FrameworkUAD.Entity.SubscriberOriginal> lSO = new List<FrameworkUAD.Entity.SubscriberOriginal>();
            List<FrameworkUAD.Entity.SubscriberDemographicOriginal> lSDO = new List<FrameworkUAD.Entity.SubscriberDemographicOriginal>();

            KMPlatform.Entity.Client client = KMPlatform.DataAccess.Client.Select(clientID);

            List<FrameworkUAS.Entity.FieldMapping> lfm = FrameworkUAS.DataAccess.FieldMapping.Select(sourcefileID).FindAll(x => x.FieldMappingTypeID == 3).ToList();


            using (var rdr = DataFunctions.ExecuteReader(SqSelectFromTmp, client.ClientLiveDBConnectionString))
            {
                if (rdr != null)
                {
                    FrameworkUAD.Entity.SubscriberOriginal retItem = new FrameworkUAD.Entity.SubscriberOriginal();
                    DynamicBuilder<FrameworkUAD.Entity.SubscriberOriginal> builder = DynamicBuilder<FrameworkUAD.Entity.SubscriberOriginal>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retItem.SORecordIdentifier = new Guid();

                        lSO.Add(retItem);


                        foreach(FrameworkUAS.Entity.FieldMapping fm in lfm)
                        {
                            if (rdr[fm.MAFField].ToString() != string.Empty)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal sdo = new FrameworkUAD.Entity.SubscriberDemographicOriginal();

                                sdo.SORecordIdentifier = retItem.SORecordIdentifier;
                                sdo.MAFField = fm.MAFField;
                                sdo.Value = rdr[fm.MAFField].ToString();
                                sdo.PubID = 0;

                                lSDO.Add(sdo);
                            }
                        }

                        if (counter == 10000)
                        {
                            Console.WriteLine(string.Format("{0} - {1}", retItem.ImportRowNumber, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.f")));
                            counter = 1;
                        }

                        counter++;

                    }
                    
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            Console.WriteLine(string.Format("Subscriber Original - {0}", lSO.Count));
            Console.WriteLine(string.Format("Subscriber Original Demo - {0} ", lSDO.Count));

            lSO.Clear();
            lSDO.Clear();

            GC.Collect();

            Console.WriteLine("Press key to continue!!!");
            Console.ReadLine();
        }
    }
}
