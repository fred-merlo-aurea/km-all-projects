using System;
using System.Linq.Expressions;
using KMEntities;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace KMDbManagers
{
    public abstract class DbManagerBase
    {
       
        protected kmEntities KM = new kmEntities();

        protected Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> expression)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
        }

        public void SaveChanges()
        {
            KM.ChangeTracker.DetectChanges(); // Force EF to match associations.
            var objectContext = ((IObjectContextAdapter)KM).ObjectContext;
            objectContext.CommandTimeout = 0;
            var objectStateManager = objectContext.ObjectStateManager;
            var fieldInfo = objectStateManager.GetType().GetField("_entriesWithConceptualNulls", BindingFlags.Instance | BindingFlags.NonPublic);
            var conceptualNulls = fieldInfo.GetValue(objectStateManager);            
            KM.SaveChanges();
        }
    }
}