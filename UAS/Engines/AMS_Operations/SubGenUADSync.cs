using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using FrameworkSubGen.Entity;
using KM.Common;
using KMPlatform.BusinessLogic;
using Enums = FrameworkSubGen.Entity.Enums;
using BusinessEnums = KMPlatform.BusinessLogic.Enums;
using Subscriber = FrameworkSubGen.Entity.Subscriber;
using Subscription = FrameworkSubGen.Entity.Subscription;

namespace AMS_Operations
{
    public delegate void CdCtoImportSubscriberDelegate(ChangeDataCapture changeData, Enums.Client sgClient, Account account);

    public class SubGenUadSync
    {
        private const string SubGenSyncClientsSettingName = "SubGenSyncClients";
        private const string SubGenSyncIntervalSettingName = "SubGenSyncInterval";
        private const string SourceMethod = "AMS_Operations.SyncSubGenSubscribers";
        private readonly List<Account> _accounts;
        private readonly Action<string> _consoleMessage;
        private readonly Action<Exception, string> _logError;
        private readonly CdCtoImportSubscriberDelegate _cdCtoImportSubscriber;

        public SubGenUadSync(
            Action<string> consoleMessageFunc,
            Action<Exception, string> logErrorFunc,
            CdCtoImportSubscriberDelegate cdCtoImportSubscriberFunc,
            List<Account> accounts)
        {
            _consoleMessage = consoleMessageFunc;
            _accounts = accounts;
            _logError = logErrorFunc;
            _cdCtoImportSubscriber = cdCtoImportSubscriberFunc;
        }

        public void SubGenToUad()
        {
            var syncInterval = 5;
            int.TryParse(ConfigurationManager.AppSettings[SubGenSyncIntervalSettingName], out syncInterval);

            var counter = 1;
            while (true)
            {
                try
                {
                    _consoleMessage($"CDC check: {counter} at {DateTime.Now}");
                    var cdcStamp = DateTime.Now.AddMinutes(-syncInterval);

                    if (ConfigurationManager.AppSettings[SubGenSyncClientsSettingName] != null)
                    {
                        var clientIds = ConfigurationManager.AppSettings[SubGenSyncClientsSettingName].Split(',');
                        SubGenToUadForClient(clientIds, cdcStamp, InitEntitiesCommonFields);
                    }
                    else
                    {
                        var cWorker = new Client();
                        var clients = cWorker.AMS_SelectPaid();
                        var clientIds = clients.Select(c => c.DisplayName);
                        SubGenToUadForClient(clientIds, cdcStamp, null);
                    }
                }
                catch (Exception ex)
                {
                    _logError(ex, GetType().Name);
                }

                Console.WriteLine($"Finished check: {counter} at {DateTime.Now}");
                Console.WriteLine($"thread sleep for {syncInterval} minutes.");
                Thread.Sleep(TimeSpan.FromSeconds(syncInterval));
                counter++;
            }
        }

        private void SubGenToUadForClient(IEnumerable<string> clients, DateTime cdcStamp, Action<ChangeDataCapture> additionalInit)
        {
            foreach (var sgClient in clients.Select(c => Enums.GetClient(c.Trim())))
            {
                _consoleMessage("CDC check for " + sgClient);
                var account = _accounts.Single(x =>
                    x.company_name.Equals(sgClient.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
                var entities = new List<Enums.Entities>
                {
                    Enums.Entities.addresses,
                    Enums.Entities.purchases,
                    Enums.Entities.subscribers,
                    Enums.Entities.subscriptions,
                    Enums.Entities.bundles
                };

                var allChangeData = new ChangeDataCapture();
                foreach (var entity in entities)
                {
                    _consoleMessage($"Getting CDC values for {entity} {DateTime.Now}");
                    var changeData = SyncSubGen(sgClient, entity, cdcStamp);

                    switch (entity)
                    {
                        case Enums.Entities.addresses:
                            allChangeData.addresses = changeData.addresses;
                            break;
                        case Enums.Entities.customfields:
                            allChangeData.custom_fields = changeData.custom_fields;
                            break;
                        case Enums.Entities.purchases:
                            allChangeData.purchases = changeData.purchases;
                            break;
                        case Enums.Entities.subscribers:
                            allChangeData.subscribers = changeData.subscribers;
                            break;
                        case Enums.Entities.subscriptions:
                            allChangeData.subscriptions = changeData.subscriptions;
                            break;
                        case Enums.Entities.bundles:
                            allChangeData.bundles = changeData.bundles;
                            break;
                    }

                    allChangeData.account_id = changeData.account_id;
                }

                additionalInit?.Invoke(allChangeData);
                _cdCtoImportSubscriber(allChangeData, sgClient, account);
            }
        }

        private static void InitEntitiesCommonFields(ChangeDataCapture allChangeData)
        {
            allChangeData.addresses = allChangeData.addresses ?? new List<Address>();
            allChangeData.bundles = allChangeData.bundles ?? new List<Bundle>();
            allChangeData.custom_fields = allChangeData.custom_fields ?? new List<CustomField>();
            allChangeData.purchases = allChangeData.purchases ?? new List<Purchase>();
            allChangeData.subscribers = allChangeData.subscribers ?? new List<Subscriber>();
            allChangeData.subscriptions = allChangeData.subscriptions ?? new List<Subscription>();
        }

        private static ChangeDataCapture SyncSubGen(Enums.Client client, Enums.Entities entity, DateTime? cdcStamp = null)
        {
            var changeData = new ChangeDataCapture();
            try
            {
                var syncInterval = 5;
                int.TryParse(ConfigurationManager.AppSettings[SubGenSyncIntervalSettingName], out syncInterval);

                if (cdcStamp == null)
                {
                    cdcStamp = DateTime.Now.AddMinutes(-syncInterval);
                }

                var cdcWorker = new FrameworkSubGen.BusinessLogic.API.ChangeDataCapture();
                var entities = new List<Enums.Entities> {entity};
                changeData = cdcWorker.Get(client, cdcStamp.Value, entities);
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var alWorker = new ApplicationLog();
                alWorker.LogCriticalError(
                    message,
                    SourceMethod,
                    BusinessEnums.Applications.AMS_Operations,
                    $"SubGenClient:{client}");
            }

            return changeData;
        }
    }
}
