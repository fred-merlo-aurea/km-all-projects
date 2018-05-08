using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;

namespace FrameworkSubGen.BusinessLogic
{
    public class History : BusinessLogicBase
    {
        private const string EntityName = "History";

        public bool SaveBulkXml(IList<Entity.History> list)
        {
            return SaveBulkXml(list, EntityName);
        }

        protected override void FormatData(IEntity entity)
        {
            try
            {
                var history = entity as Entity.History;

                if (history == null)
                {
                    return;
                }

                history.notes = TruncateString(history.notes, FieldLength50);

                if (history.date == DateTime.Parse("0001-01-01T00:00:00")
                    || history.date == DateTime.MinValue
                    || history.date <= DateTime.Parse("1/1/1900"))
                {
                    history.date = DateTime.Now;
                }
            }
            catch (FormatException formatException)
            {
                API.Authentication.SaveApiLog(formatException, GetType().ToString(), GetType().Name);
            }
        }

        protected override bool Save(string xml)
        {
            return DataAccess.History.SaveBulkXml(xml);
        }
    }
}
