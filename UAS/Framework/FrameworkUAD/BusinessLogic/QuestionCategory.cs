using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class QuestionCategory
    {
        public List<Entity.QuestionCategory> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.QuestionCategory> x = null;
            x = DataAccess.QuestionCategory.Select(client).ToList();
            return x;
        }
    }
}
