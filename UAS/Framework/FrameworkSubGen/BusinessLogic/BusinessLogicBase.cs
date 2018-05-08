using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Text;
using static FrameworkSubGen.BusinessLogic.API.Authentication;
using static KM.Common.StringFunctions;

namespace FrameworkSubGen.BusinessLogic
{
    public abstract class BusinessLogicBase
    {
        protected const int FieldLength25 = 25;
        protected const int FieldLength50 = 50;
        protected const int FieldLength100 = 100;
        protected const int FieldLength250 = 250;
        protected const int FieldLength255 = 255;

        private const int BatchSize = 500;

        public bool SaveBulkXml<T>(IList<T> entities, string entityName) where T : Entity.IEntity
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var done = false;
            var total = entities.Count;
            var counter = 0;
            var processedCount = 0;
            var checkedCount = 1;

            foreach (var entity in entities)
            {
                FormatData(entity);
            }

            var builder = new StringBuilder();
            foreach (var entity in entities)
            {
                var message = $"Checking {entityName}: {checkedCount} of {total}";
                WriteLineRepeater(message, ConsoleColor.Green);

                checkedCount++;

                var xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString(entity));
                builder.AppendLine(xmlObject);

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    done = SaveEntity(entityName, builder.ToString());

                    builder.Clear();
                    counter = 0;
                }
            }

            return done;
        }

        protected string TruncateString(string input, int maxSize)
        {
            if (input != null && input.Length > maxSize)
            {
                return input.Substring(0, maxSize);
            }

            return input;
        }

        private bool SaveEntity(string entityName, string xml)
        {
            var success = false;
            try
            {
                success = Save($"<XML>{xml}</XML>");
            }
            catch (Exception exception)
            {
                SaveApiLog(exception, $"FrameworkSubGen.BusinessLogic.{entityName}", entityName);
            }

            return success;
        }

        protected abstract void FormatData(Entity.IEntity entity);

        protected abstract bool Save(string xml);
    }
}
