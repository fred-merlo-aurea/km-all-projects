using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class PublicationReports
    {
        public List<Entity.PublicationReports> SelectPublication(int publicationID)
        {
            List<Entity.PublicationReports> x = null;
            x = DataAccess.PublicationReports.SelectPublication(publicationID);
            return x;
        }
    }
}
