using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;
using static FrameworkSubGen.BusinessLogic.API.Authentication;

namespace FrameworkSubGen.BusinessLogic
{
    public class Account : BusinessLogicBase
    {
        private const string EntityName = "Account";

        public bool SaveBulkXml(IList<Entity.Account> list)
        {
            return SaveBulkXml(list, EntityName);
        }

        public Entity.Account Select(FrameworkUAS.BusinessLogic.Enums.Clients client)
        {
            return DataAccess.Account.Select(client.ToString());
        }

        public Entity.Account Select(int clientId)
        {
            return DataAccess.Account.Select(clientId);
        }

        public IList<Entity.Account> Select()
        {
            return DataAccess.Account.Select();
        }

        protected override void FormatData(IEntity entity)
        {
            try
            {
                var account = entity as Entity.Account;

                if (account == null)
                {
                    return;
                }

                account.company_name = TruncateString(account.company_name, FieldLength50);
                account.email = TruncateString(account.email, FieldLength100);
                account.website = TruncateString(account.website, FieldLength100);
                account.api_key = TruncateString(account.api_key, FieldLength100);
                account.api_login = TruncateString(account.api_login, FieldLength50);
                account.audit_type = TruncateString(account.audit_type, FieldLength50);
                account.master_checkout = TruncateString(account.master_checkout, FieldLength255);

                if (account.created == DateTime.Parse("0001-01-01T00:00:00")
                    || account.created == DateTime.MinValue
                    || account.created <= DateTime.Parse("1/1/1900"))
                {
                    account.created = DateTime.Now;
                }
            }
            catch (FormatException formatException)
            {
                SaveApiLog(formatException, GetType().ToString(), GetType().Name);
            }
        }

        protected override bool Save(string xml)
        {
            return DataAccess.Account.SaveBulkXml(xml);
        }
    }
}
