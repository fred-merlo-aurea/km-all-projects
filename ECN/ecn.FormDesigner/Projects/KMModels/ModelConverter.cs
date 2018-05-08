using System;
using System.Collections;
using System.Collections.Generic;

namespace KMModels
{
    public static class ModelConverter
    {
        public static IEnumerable<Model> ConvertToModels<Model>(this IEnumerable entities) where Model: ModelBase, new()
        {
            foreach (var entity in entities)
            {
                Model obj = new Model();
                obj.FillData(entity);
                yield return obj;
            }
        }

        public static Model ConvertToModel<Model>(this object entity) where Model : ModelBase, new() 
        {
            Model obj = new Model();
            obj.FillData(entity);
            return obj;
        }
    }
}