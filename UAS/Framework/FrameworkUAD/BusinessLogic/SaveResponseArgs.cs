using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAD_Lookup.BusinessLogic;
using KMPlatform.Entity;

namespace FrameworkUAD.BusinessLogic
{
    internal class SaveResponseArgs
    {
        public JsonFunctions JsonFunctions { get; } = new JsonFunctions();
        public List<Entity.ProductSubscriptionDetail> CurrentAnswers { get; } = new List<Entity.ProductSubscriptionDetail>();
        public List<Entity.ProductSubscriptionDetail> AddedList { get; } = new List<Entity.ProductSubscriptionDetail>();
        public List<Entity.ProductSubscriptionDetail> RemovedList { get; } = new List<Entity.ProductSubscriptionDetail>();
        public ProductSubscriptionDetail PubSubDetailWorker { get; } = new ProductSubscriptionDetail();
        public Code CodeWorker { get; } = new Code();
        public List<Entity.HistoryResponseMap> ResponseMaps { get; } = new List<Entity.HistoryResponseMap>();
        public List<UserLog> UserLogs { get; } = new List<UserLog>();
        public List<ProductSubscription.ProductSubscriptionDetailChange> PubSubDetailChanges { get; } = new List<ProductSubscription.ProductSubscriptionDetailChange>();
        public Client Client { get; set; }
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        public int PubId { get;set; }
        public int PubSubscriptionId { get; set; }
        public int HistoryId { get; set; }
    }
}
