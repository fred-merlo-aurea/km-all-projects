using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FrameworkSubGen.DataAccess;
using KMPlatform.BusinessLogic;
using Core_AMS.Utilities;
using ServiceStack.Text;
using static KMPlatform.BusinessLogic.Enums;
using FrameworkSubGen.BusinessLogic.API;

namespace FrameworkSubGen.BusinessLogic
{
    public class CoreImport
    {
        private const string BatchSizeExceptionMessageFormat = "Parameter '{0}' must be strictly higher than 0.";
        private const string ProgressMessageFormat = "Checking {0}: {1} of {2}";
        private const string XmlStringFormat = "<XML>{0}</XML>";

        private const int BatchSize = 250;
        private const bool DefaultDoneValue = false;
        private const int DefaultCounterValue = 0;
        private const int DefaultProcessedCount = 0;

        /// <summary>
        /// Processes a list of items as XML, in batches.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list of items</param>
        /// <param name="processXml">The Action called for each batch of items processed as XML</param>
        /// <param name="batchSize">The batch size (strictly greater than 0). Default = 250</param>
        /// <param name="errorMessage">The custom error message to log when the processing of a batch failes</param>
        /// <returns></returns>
        public bool CoreSaveBulkXml<T>(IList<T> list, Action<string> processXml,
            int batchSize = 250, string errorMessage = "", 
            bool onExceptionSaveToApiLog = true, bool logProcessProgress = false)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (batchSize <= 0)
            {
                throw new InvalidOperationException(
                    String.Format(BatchSizeExceptionMessageFormat, nameof(batchSize)));
            }

            var done = DefaultDoneValue;
            var totalItemsToProcess = list.Count;
            var itemProcessedForCurrentBatch = DefaultCounterValue;
            var itemsProcessedInTotal = DefaultProcessedCount;

            var sbXml = new StringBuilder();
            foreach (var item in list)
            {
                itemsProcessedInTotal++;
                itemProcessedForCurrentBatch++;

                if (logProcessProgress)
                {
                    LogProcessProgress(itemsProcessedInTotal, totalItemsToProcess, typeof(T));
                }

                var xmlObject = DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString(item));
                sbXml.AppendLine(xmlObject);

                if (itemsProcessedInTotal == totalItemsToProcess || itemProcessedForCurrentBatch == batchSize)
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            processXml(String.Format(XmlStringFormat, sbXml));
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            LogException(ex, errorMessage, onExceptionSaveToApiLog);                            
                        }
                    }
                    sbXml.Clear();
                    itemProcessedForCurrentBatch = 0;
                }
            }
            return done;
        }

        private void LogProcessProgress(int itemsProcessedInTotal, int totalItemsToProcess, Type itemType)
        {
            var msg = String.Format(ProgressMessageFormat, itemType.Name,
                itemsProcessedInTotal + 1, totalItemsToProcess);

            StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);
        }

        private void LogException(Exception ex, string errorMessage, bool onExceptionSaveToApiLog)
        {
            var alWorker = new ApplicationLog();
            var error = StringFunctions.FormatException(ex);

            alWorker.LogCriticalError(error, errorMessage, Applications.SubGen_Integration);

            if (onExceptionSaveToApiLog)
            {
                Authentication.SaveApiLog(ex, GetType().ToString(), GetType().Name.ToString());
            }
        }
    }
}
