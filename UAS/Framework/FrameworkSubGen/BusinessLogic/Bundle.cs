using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;
using static FrameworkSubGen.BusinessLogic.API.Authentication;

namespace FrameworkSubGen.BusinessLogic
{
    public class Bundle : BusinessLogicBase
    {
        private const string EntityName = "Bundle";

        public bool SaveBulkXml(IList<Entity.Bundle> bundles, int accountId)
        {
            if (bundles == null)
            {
                throw new ArgumentNullException(nameof(bundles));
            }

            foreach (var bundle in bundles)
            {
                bundle.account_id = accountId;

                foreach (var subscription in bundle.subscriptions)
                {
                    bundle.publication_id = subscription.publication_id;
                    bundle.issues = subscription.issues;
                }
            }

            return SaveBulkXml(bundles, EntityName);
        }

        public Entity.Bundle Select(int bundleId)
        {
            return DataAccess.Bundle.Select(bundleId);
        }

        public Entity.Bundle Select(string name, int accountId)
        {
            return DataAccess.Bundle.Select(name, accountId);
        }

        protected override void FormatData(IEntity entity)
        {
            try
            {
                var bundle = entity as Entity.Bundle;

                if (bundle == null)
                {
                    return;
                }

                bundle.name = TruncateString(bundle.name, FieldLength250);
                bundle.promo_code = TruncateString(bundle.promo_code, FieldLength25);
                bundle.description = TruncateString(bundle.description, FieldLength250);
            }
            catch (FormatException formatException)
            {
                SaveApiLog(formatException, GetType().ToString(), GetType().Name);
            }
        }

        protected override bool Save(string xml)
        {
            return DataAccess.Bundle.SaveBulkXml(xml);
        }
    }
}
