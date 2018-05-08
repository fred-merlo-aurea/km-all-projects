using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADMS.Events;
using FrameworkUAS.BusinessLogic;

namespace AutomatedDataMigrationService.Services.Transformer
{
    public class Transformer : ITransformer
    {
        public event Action<FileTransformed> FileTransformed;
        //RowValidated
        //Service Entry Point
        public void HandleRowValidated(RowValidated eventMessage)
        {
            #region old - done in Validator now
            //FrameworkUAS.Entity.SourceFile sourceFile = eventMessage.SourceFile;
            //FrameworkUAS.Entity.Client client = eventMessage.Client;
            //DataRow data = eventMessage.ValidatedDataRow;
            //DataTable fileData = eventMessage.FileData;


            //List<FrameworkUAS.Entity.FieldMapping> mapping = eventMessage.FieldMapping;

            //Dictionary<int, string> insertData = new Dictionary<int, string>();

            //FrameworkUAS.Entity.SubscriberOriginal subscriberOriginal = new FrameworkUAS.Entity.SubscriberOriginal()
            //{
            //    ClientID = eventMessage.Client.ClientID,
            //    SourceFileID = eventMessage.SourceFile.SourceFileID,
            //};

            //foreach (var map in mapping)
            //{
            //    if (map.MAFField != "Ignore" && !map.IsIgnored)
            //    {
            //        PropertyInfo prop = subscriberOriginal.GetType().GetProperty(map.MAFField, BindingFlags.Public | BindingFlags.Instance);
            //        if (prop != null && prop.CanWrite)
            //        {
            //            prop.SetValue(subscriberOriginal, data[map.ColumnOrder-1], null);
            //        }
            //    }
            //}

            //int result = FrameworkUAS.BusinessLogic.SubscriberOriginal.Save(subscriberOriginal);
            #endregion

        }
    }
}
