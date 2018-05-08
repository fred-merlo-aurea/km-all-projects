using System.Collections.Generic;
using FrameworkUAD.BusinessLogic;
using UAS.Web.Models.Circulations;

namespace UAS.Web.Controllers.Circulations
{
    public class UpdateAddKillArgs
    {
        public int PubId { get;set; }
        public int AddCatCode { get;set; }
        public int AddTransCode { get;set; }
        public int KillTransCode { get;set; }
        public List<AddKillContainer> AddKills { get; } = new List<AddKillContainer>();
        public SubscriberAddKill SubAddKillWorker { get; } = new SubscriberAddKill();
        public ProductSubscription SubscriptionWorker { get; } = new ProductSubscription();
    }
}