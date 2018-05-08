using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAS.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UAS.UnitTests.Helpers
{
    public static class CreateSubscriberTestHelper
    {
        public static FieldMapping CreateFieldMapping(string incommingName, string MAFname, int type)
        {
            return new FieldMapping()
            {
                IncomingField = incommingName,
                FieldMappingTypeID = type,
                MAFField = MAFname,
                DataType = string.Empty,
                PreviewData = string.Empty,
            };
        }

        public static FieldMultiMap CreateMultiMapping(string MAFname, int type)
        {
            return new FieldMultiMap()
            {
                FieldMappingTypeID = type,
                MAFField = MAFname,
                DataType = string.Empty,
                PreviewData = string.Empty,
            };
        }
    }
}
